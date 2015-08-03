namespace PHBC.Web.Constants
{
    public static class ControllerRoute
    {
        //Error
        public const string ErrorGetBadRequest = ControllerName.Error + "GetBadRequest";
        public const string ErrorGetForbidden = ControllerName.Error + "GetForbidden";
        public const string ErrorGetInternalServerError = ControllerName.Error + "GetInternalServerError";
        public const string ErrorGetMethodNotAllowed = ControllerName.Error + "GetMethodNotAllowed";
        public const string ErrorGetNotFound = ControllerName.Error + "GetNotFound";
        public const string ErrorGetUnauthorized = ControllerName.Error + "Unauthorized";

        //Home
        public const string HomeGetAbout = ControllerName.Home + "GetAbout";
        public const string HomeGetContact = ControllerName.Home + "GetContact";
        public const string HomeGetFeed = ControllerName.Home + "GetFeed";
        public const string HomeGetIndex = ControllerName.Home + "GetIndex";
        public const string HomeGetSearch = ControllerName.Home + "GetSearch";
        public const string HomeGetOpenSearchXml = ControllerName.Home + "GetOpenSearchXml";
        public const string HomeGetRobotsText = ControllerName.Home + "GetRobotsText";
        public const string HomeGetSitemapXml = ControllerName.Home + "GetSitemapXml";
    }
}