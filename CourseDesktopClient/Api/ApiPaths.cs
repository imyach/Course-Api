using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Api
{
    public class ApiPaths
    {
        public const string API_PATH = "https://localhost:7000/api/";

        public const string API_GET_ALL_COURSE = "course/all";
        public const string API_GET_COURSE_BY_ID = "course/";
        public const string API_LOGOUT_USER = "auth/logout";
        public const string API_LOGIN_USER= "auth/login";
        public const string API_REGISTER_USER= "auth/register";
        public const string API_GET_USER= "user/";

        public const string API_REFRESH_TOKEN= "auth/refresh";
    }
}
