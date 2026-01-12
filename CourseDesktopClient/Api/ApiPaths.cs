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
        public const string API_DELETE_UPDATE_CERATE_COURSE = "course";

        public const string API_GET_ALL_USERS = "user/all";
        public const string API_GET_USER_BY_ID = "user/";
        public const string API_DELETE_UPDATE_CREATE_USER= "user";

        public const string API_GET_ALL_REVIEWS = "review/all";
        public const string API_GET_REVIEW_BY_ID = "review/";
        public const string API_DELETE_UPDATE_CREATE_REVIEW = "review";

        public const string API_LOGOUT_USER = "auth/logout";
        public const string API_LOGIN_USER= "auth/login";
        public const string API_REGISTER_USER= "auth/register";
        public const string API_REFRESH_TOKEN= "auth/refresh";

        public const string API_GET_ALL_PROGRESSUSER = "progressUser/all";
        public const string API_GET_PROGRESSUSER_BY_ID = "progressUser/";
        public const string API_DELETE_UPDATE_CERATE_PROGRESSUSER = "progressUser";

    }
}
