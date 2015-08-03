namespace PHBC.Web.Constants
{
    public static class ControllerAction
    {
        //Home
        public const string HomeAbout = "About";
        public const string HomeContact = "Contact";
        public const string HomeFeed = "Feed";
        public const string HomeIndex = "Index";
        public const string HomeSearch = "Search";
        public const string HomeOpenSearchXml = "OpenSearchXml";
        public const string HomeRobotsText = "RobotsText";
        public const string HomeSitemapXml = "SitemapXml";
        
        //Account
        public const string AccountLogin = "Login";
        public const string AccountLogOff = "LogOff";
        public const string AccountRegister = "Register";
        public const string AccountResetPassword = "ResetPassword";
        public const string AccountForgotPassword = "ForgotPassword";

        //UserManage
        public const string UserManageLogin = "ChangePassword";
        public const string UserManageLogOff = "SetPassword";

        //Error
        public const string ErrorBadRequest = "BadRequest";
        public const string ErrorForbidden = "Forbidden";
        public const string ErrorInternalServerError = "InternalServerError";
        public const string ErrorMethodNotAllowed = "MethodNotAllowed";
        public const string ErrorNotFound = "NotFound";
        public const string ErrorUnauthorized = "Unauthorized";
        
        //Role
        public const string RoleAction = ActionType.Create;
    }
}