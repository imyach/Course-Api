using System;
using System.Collections.Generic;
using System.Text;

namespace CourseDesktopClient.Api
{
    public class ApiPaths
    {
        public const string API_PATH = "https://localhost:7000/api/";

        public const string API_GET_ALL_COURSE = "course/all";
        public const string API_GET_ALL_CREATED_COURSE = "course/drafted";
        public const string API_GET_COURSE_BY_ID = "course/";
        public const string API_DELETE_UPDATE_CERATE_COURSE = "course";

        public const string API_GET_ALL_USERS = "user/all";
        public const string API_GET_USER_BY_EMAIL = "user";
        public const string API_GET_USER_BY_ID = "user/";
        public const string API_DELETE_UPDATE_CREATE_USER = "user";
        public const string API_UPDATE_USER_ADMIN = "user/admin";

        public const string API_LOGOUT_USER = "auth/logout";
        public const string API_LOGIN_USER = "auth/login";
        public const string API_REGISTER_USER = "auth/register";
        public const string API_REFRESH_TOKEN = "auth/refresh";
        public const string API_SEND_RECOVERY_CODE = "auth/sendRecoveryCode";
        public const string API_SEND_NEW_PASSWORD= "auth/sendNewPassword";

        public const string API_GET_ALL_REVIEWS = "review/all";
        public const string API_GET_REVIEW_BY_ID = "review/";
        public const string API_DELETE_UPDATE_CREATE_REVIEW = "review";

        public const string API_GET_ALL_MODULE = "module/all";
        public const string API_GET_MODULE_BY_ID = "module/";
        public const string API_DELETE_UPDATE_CERATE_MODULE = "module";

        public const string API_GET_ALL_MATERIAL = "material/all";
        public const string API_GET_MATERIAL_BY_ID = "material/";
        public const string API_DELETE_UPDATE_CERATE_MATERIAL = "material";

        public const string API_GET_ALL_TEST = "test/all";
        public const string API_GET_TEST_BY_ID = "test/";
        public const string API_DELETE_UPDATE_CERATE_TEST = "test";

        public const string API_GET_ALL_QUESTION = "question/all";
        public const string API_GET_QUESTION_BY_ID = "question/";
        public const string API_DELETE_UPDATE_CERATE_QUESTION = "question";

        public const string API_GET_ALL_ANSWER = "answer/all";
        public const string API_GET_ANSWER_BY_ID = "answer/";
        public const string API_DELETE_UPDATE_CERATE_ANSWER = "answer";

        public const string API_GET_ALL_PROGRESSUSER = "progressUser/all/";
        public const string API_GET_PROGRESSUSER_BY_ID = "progressUser/";
        public const string API_DELETE_UPDATE_CERATE_PROGRESSUSER = "progressUser";

        public const string API_GET_ALL_PROGRESSMODULE = "progressModule/all/";
        public const string API_GET_PROGRESSMODULE_BY_ID = "progressModule/";
        public const string API_DELETE_UPDATE_CERATE_PROGRESSMODULE = "progressModule";

        public const string API_GET_ALL_PROGRESSMATERIAL = "progressMaterial/all/";
        public const string API_GET_PROGRESSMATERIAL_BY_ID = "progressMaterial/";
        public const string API_DELETE_UPDATE_CERATE_PROGRESSMATERIAL = "progressMaterial";

        public const string API_GET_ALL_TESTRESULT = "testResult/all/";
        public const string API_GET_TESTRESULT_BY_ID = "testResult/";
        public const string API_DELETE_UPDATE_CERATE_TESTRESULT = "testResult";

        public const string API_GET_ALL_ANSWERSUSER = "answersUser/all/";
        public const string API_GET_ANSWERSUSER_BY_ID = "answersUser/";
        public const string API_DELETE_UPDATE_CERATE_ANSWERSUSER = "answersUser";

        public const string API_GET_ALL_ROLES = "role/all";
        public const string API_GET_ROLE = "role/";

        public const string API_GENERATE_USER_REPORT = "reports/users";
    }
}
