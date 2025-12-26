using CourseDesktopClient.Interfaces;
using CourseDesktopClient.Models;
using CourseDesktopClient.Models.DtosModel.Auth;
using CredentialManagement;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CourseDesktopClient.Services
{
    public class TokenService : ITokenService
    {
        private const string CREDENTIAL_TARGET = "CourseAppTokens";
        private static readonly object locker = new();
        private static TokensDto Tokens;

        public async Task SaveTokensAsync(string access, string refresh, bool rememberMe)
        {
            if (rememberMe)
                await SaveToCredentialManagerAsync(access, refresh);
            else
            {
                lock (locker)
                {
                    Tokens = new TokensDto { AccessToken = access, RefreshToken = refresh };
                }
            }
        }

        public async Task<(string? AccessToken, string? RefreshToken)> GetTokensAsync()
        {
            lock (locker)
            {
                if (Tokens is not null)
                    return (Tokens.AccessToken, Tokens.RefreshToken);
            }
            return await GetFromCredentialManagerAsync();
        }

        public async Task ClearTokensAsync()
        {
            lock (locker)
            {
                Tokens = null;
            }
            await ClearCredentialManagerAsync();
        }

        public async Task<bool> HasTokensAsync()
        {
            var (access,refresh) = await GetTokensAsync();
            return !string.IsNullOrEmpty(access) && !string.IsNullOrEmpty(refresh);
        }

        private async Task SaveToCredentialManagerAsync(string access, string refresh)
        {
            await Task.Run(() =>
            {
                var tokenData = new TokensDto
                {
                    AccessToken = access,
                    RefreshToken = refresh
                };
                var jsonData = JsonSerializer.Serialize(tokenData);

                var credential = new Credential
                {
                    Target = CREDENTIAL_TARGET,
                    Password = jsonData,
                    PersistanceType = PersistanceType.LocalComputer
                };
                credential.Save();
            });
        }

        private async Task<(string? AccessToken, string? RefreshToken)> GetFromCredentialManagerAsync()
        {
            return await Task.Run(() =>
            {
                var credential = new Credential { Target = CREDENTIAL_TARGET };
                if (!credential.Load()) return (null, null);

                try
                {
                    var tokenData = JsonSerializer.Deserialize<TokensDto>(credential.Password);
                    return (tokenData?.AccessToken, tokenData?.RefreshToken);
                }
                catch
                {
                    return (null, null);
                }
            });
        }

        public async Task ClearCredentialManagerAsync()
        {
            await Task.Run(() => 
            {
                var credential = new Credential { Target = CREDENTIAL_TARGET };
                credential.Delete();
            });
        }

        public async Task<CurrentUserInfo> GetCurrentUserInfoAsync(string token)
        {
            return await Task.Run(() =>
            {
                var claims = GetClaims(token);

                return new CurrentUserInfo
                {
                    Id = claims["nameid"],
                    EmailUser = claims["email"],
                    RoleUser = claims["role"],
                    NameUser = claims["unique_name"]
                };
            });
        }

        private static JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken;
        }


        private static Dictionary<string, string> GetClaims(string token)
        {
            var jwtToken = DecodeToken(token);
            var claims = new Dictionary<string, string>();

            foreach (var claim in jwtToken.Claims)
            {
                claims[claim.Type] = claim.Value;
            }

            return claims;
        }
    }
}
