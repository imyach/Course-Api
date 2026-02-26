using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models.DtosModel.Auth;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CourseDesktopClient.Api.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly HttpClient refreshHttpClient;
        private readonly SemaphoreSlim refreshLock = new(1, 1);
        private readonly ITokenService tokenService;
        private readonly INavigationService navigationService;
        private static bool isRefreshing = false;
        public static bool isRemember = true;

        public TokenHandler(ITokenService tokenService, INavigationService navigationService)
        {
            this.tokenService = tokenService;
            this.navigationService = navigationService;
            refreshHttpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiPaths.API_PATH),
                Timeout = TimeSpan.FromSeconds(300)
            };

            refreshHttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                await AddAccessTokenToRequestAsync(request);
                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var refreshSuccess = await TryRefreshTokenAsync(cancellationToken);

                    if (refreshSuccess)
                    {
                        await AddAccessTokenToRequestAsync(request);
                        response = await base.SendAsync(request, cancellationToken);
                    }
                    else
                    {
                        await tokenService.ClearTokensAsync();
                        navigationService.NavigateToLogin();
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }
                }

                return response;
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                navigationService.NavigateMistakePage(ex);
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("Сервер недоступен")
                };
            }
        } 

        private async Task AddAccessTokenToRequestAsync(HttpRequestMessage request)
        {
            var (accessToken, _) = await tokenService.GetTokensAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Remove("Authorization");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        private async Task<bool> TryRefreshTokenAsync(CancellationToken cancellationToken)
        {
            await refreshLock.WaitAsync(cancellationToken);

            try
            {
                if (isRefreshing)
                {
                    var waitTask = Task.Run(async () =>
                    {
                        while (isRefreshing && !cancellationToken.IsCancellationRequested)
                        {
                            await Task.Delay(100, cancellationToken);
                        }
                    }, cancellationToken);

                    await Task.WhenAny(waitTask, Task.Delay(5000, cancellationToken));

                    var (newAccessToken, _) = await tokenService.GetTokensAsync();
                    return !string.IsNullOrEmpty(newAccessToken);
                }

                isRefreshing = true;

                var (_, refreshToken) = await tokenService.GetTokensAsync();
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return false;
                }

                var refreshDto = new { refreshToken };
                var content = new StringContent(JsonSerializer.Serialize(refreshDto),
                    Encoding.UTF8, "application/json");

                var response = await refreshHttpClient.PostAsync(ApiPaths.API_REFRESH_TOKEN, content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    await tokenService.ClearTokensAsync();
                    return false;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                var newTokens = JsonSerializer.Deserialize<TokensDto>(jsonResponse);

                if (newTokens == null ||
                    string.IsNullOrEmpty(newTokens.AccessToken) ||
                    newTokens.RefreshToken == string.Empty)
                {
                    await tokenService.ClearTokensAsync();
                    return false;
                }

                await tokenService.SaveTokensAsync(newTokens.AccessToken, newTokens.RefreshToken, isRemember);

                return true;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                isRefreshing = false;
                refreshLock.Release();
            }
        }
    }
}