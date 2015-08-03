namespace PHBC.Web
{
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Boilerplate.Web.Mvc;
    using PHBC.Web.Services;
    using NWebsec.Csp;
    using System.Data.Entity;
    using PHBC.DAO;
    using System;
    using PHBC.DAO.Bussiness;
    using System.Collections.Generic;
    using PHBC.DAO.Models;
    using PHBC.Web.Permission;
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConfigureViewEngines();
            ConfigureAntiForgeryTokens();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            ActionDbInitializer acDB = new ActionDbInitializer();
            acDB.InitializeControllAction();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            var user = Context.User;
            if (user == null || !user.Identity.IsAuthenticated)
                return;
            if (Session[PHBC.Web.Constants.Application.Session.Permisson] == null)
            {
                AppPermission appPermisson = new AppPermission(user.Identity.Name);
                Session[PHBC.Web.Constants.Application.Session.Permisson] = appPermisson;
            }

        }

        protected void Session_End(object sender, EventArgs e)
        {
            // event is raised when a session is abandoned or expires
        }

        /// <summary>
        /// Handles the Content Security Policy (CSP) violation errors. For more information see FilterConfig.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CspViolationReportEventArgs"/> instance containing the event data.</param>
        protected void NWebsecHttpHeaderSecurityModule_CspViolationReported(object sender, CspViolationReportEventArgs e)
        {
            // Log the Content Security Policy (CSP) violation.
            CspViolationReport violationReport = e.ViolationReport;
            CspReportDetails reportDetails = violationReport.Details;
            string violationReportString = string.Format(
                "UserAgent:<{0}>\r\nBlockedUri:<{1}>\r\nColumnNumber:<{2}>\r\nDocumentUri:<{3}>\r\nEffectiveDirective:<{4}>\r\nLineNumber:<{5}>\r\nOriginalPolicy:<{6}>\r\nReferrer:<{7}>\r\nScriptSample:<{8}>\r\nSourceFile:<{9}>\r\nStatusCode:<{10}>\r\nViolatedDirective:<{11}>",
                violationReport.UserAgent,
                reportDetails.BlockedUri,
                reportDetails.ColumnNumber,
                reportDetails.DocumentUri,
                reportDetails.EffectiveDirective,
                reportDetails.LineNumber,
                reportDetails.OriginalPolicy,
                reportDetails.Referrer,
                reportDetails.ScriptSample,
                reportDetails.SourceFile,
                reportDetails.StatusCode,
                reportDetails.ViolatedDirective);
            CspViolationException exception = new CspViolationException(violationReportString);
            DependencyResolver.Current.GetService<ILoggingService>().Log(exception);
        }

        /// <summary>
        /// Configures the view engines. By default, Asp.Net MVC includes the Web Forms (WebFormsViewEngine) and 
        /// Razor (RazorViewEngine) view engines. You can remove view engines you are not using here for better
        /// performance.
        /// </summary>
        private static void ConfigureViewEngines()
        {
            // Only use the RazorViewEngine.
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        /// <summary>
        /// Configures the anti-forgery tokens.
        /// </summary>
        private static void ConfigureAntiForgeryTokens()
        {
            // Rename the Anti-Forgery cookie from "__RequestVerificationToken" to "f". 
            // This adds a little security through obscurity and also saves sending a 
            // few characters over the wire. Sadly there is no way to change the form input
            // name which is hard coded in the @Html.AntiForgeryToken helper and the 
            // ValidationAntiforgeryTokenAttribute to  __RequestVerificationToken.
            // <input name="__RequestVerificationToken" type="hidden" value="..." />
            AntiForgeryConfig.CookieName = "f";

            // If you have enabled SSL. Uncomment this line to ensure that the Anti-Forgery 
            // cookie requires SSL to be sent accross the wire. 
            // AntiForgeryConfig.RequireSsl = true;
        }
    }
}
