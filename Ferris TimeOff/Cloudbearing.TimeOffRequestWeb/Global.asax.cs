using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Cloudbearing.TimeOffRequestWeb
{
    public class Global : System.Web.HttpApplication
    {
        public static Guid ProductId = new Guid("{bc9eec10-74d2-4638-b99e-e48d2b97f214}");
        

        protected void Application_Start(object sender, EventArgs e)
        {
           
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //if (ConfigSettings.settings == null)
            //{
            //    Uri sharepointUrl = new Uri(Request.QueryString["SPHostUrl"]);
            //    string accessToken =
            //    TokenHelper.GetAccessToken(TokenHelper.ReadAndValidateContextToken(TokenHelper.GetContextTokenFromRequest(Request), Request.Url.Authority).RefreshToken,
            //        "00000003-0000-0ff1-ce00-000000000000", sharepointUrl.Authority, TokenHelper.GetRealmFromTargetUrl(sharepointUrl)).AccessToken;

            //    ConfigSettings.settings = (new ConfigListClass().GetConfigDetails(new Uri(Request.QueryString["SPHostUrl"]).ToString(), accessToken));
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            if (exc.GetType() == typeof(HttpException))
            {                
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;
                //Redirect HTTP errors to HttpError page
               // Server.Transfer("HttpErrorPage.aspx");
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}