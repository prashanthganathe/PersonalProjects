using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudbearing.TimeOffRequestWeb
{
    public class PreInitPage : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Uri redirectUrl;
            switch (SharePointContextProvider.CheckRedirectionStatus(
                        Context, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    return;
                case RedirectionStatus.ShouldRedirect:
                    Response.Redirect(redirectUrl.AbsoluteUri, endResponse: true);
                    break;
                case RedirectionStatus.CanNotRedirect:
                    Response.Write("Error occurred while processing your request.");
                    Response.End();
                    break;
            }
        }
    }
}