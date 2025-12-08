using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CourseDesktopClient.ApiConnection.Interfaces
{
    public interface IClientConfig
    {
        public static HttpClient? Client { get; }
    }
}
