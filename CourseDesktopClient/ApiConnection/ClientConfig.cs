using CourseDesktopClient.ApiConnection.Interfaces;
using CourseDesktopClient.Models.DtosModel.EntitiesLists;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CourseDesktopClient.ApiConnection
{
    public class ClientConfig : IClientConfig
    {
        public static HttpClient? Client = new HttpClient
        {
            BaseAddress = new Uri(ApiPaths.API_PATH),
        };

        //public ClientConfig()
        //{
        //    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token)
        //}
    }
}
