using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Cloudbearing.TimeOffRequestWeb.Pages
{
    

    public class CustomException
    {
        //public TValue ErrorHandler<TValue>(Func<TValue> action)
        //{
        //    try
        //    {
        //        return action();
        //    }
        //    catch (Exception ex)
        //    {
        //        DumpErrorIntoAppLog(DateTime.Now.ToString(CultureInfo.InvariantCulture), ex.Message);
        //        return default(TValue);
        //    }
        //}

        //public void ErrorHandler(Action action)
        //{
        //    try
        //    {
        //        action();
        //    }
        //    catch (Exception ex)
        //    {
        //        DumpErrorIntoAppLog(DateTime.Now.ToString(CultureInfo.InvariantCulture), ex.Message);
        //    }
        //}


        public void DumpErrorIntoAppLog(string time, string message)
        {
            ContextUtility utility = new ContextUtility(HttpContext.Current.Request);
            using (ClientContext context = TokenHelper.GetClientContextWithContextToken(utility.ContextDetails.AppWebUrl, utility.ContextDetails.ContextTokenString, HttpContext.Current.Request.Url.Authority))
            {
                try
                {
                    List list = context.Web.Lists.GetByTitle("Errors");
                    context.Load(list);
                    context.ExecuteQuery();
                    if (list == null)
                    {
                        return;
                    }
                    CamlQuery query = CamlQuery.CreateAllItemsQuery();
                    ListItemCollection items = list.GetItems(query);
                    context.Load(items);
                    context.ExecuteQuery();

                    Microsoft.SharePoint.Client.ListItem newItem;
                    ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                    newItem = list.AddItem(itemCreateInfo);
                    newItem["Errors"] = string.Format("Time: {0} Message: {1}", time, message);
                    newItem.Update();
                    context.ExecuteQuery();
                }
                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(context, Global.ProductId, ex.Message);
                    context.ExecuteQuery();
                }
            }
        }
    }


    public class SpContext
    {
        public string HostWebUrl { get; set; }
        public string AppWebUrl { get; set; }
        public string ContextTokenString { get; set; }
        public string ServerUrl { get; set; }
    }
    public class ContextUtility
    {


        public ContextUtility(HttpRequest request)
        {
            ContextDetails = new SpContext { ServerUrl = request.Url.Authority, HostWebUrl = HttpContext.Current.Request["SPHostUrl"], AppWebUrl = HttpContext.Current.Request["SPAppWebUrl"], ContextTokenString = TokenHelper.GetContextTokenFromRequest(request) };

            if (ContextToken == null)
            {
                try
                {
                    ContextToken = TokenHelper.ReadAndValidateContextToken(ContextDetails.ContextTokenString, ContextDetails.ServerUrl);
                }
                catch (Exception)
                {
                    ContextToken = null;
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            HttpCookie cookie = new HttpCookie("SPContext", serializer.Serialize(ContextDetails));
            cookie.Expires = DateTime.Now.AddHours(12);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public ContextUtility(SpContext context)
        {
            ContextDetails = context;
            try
            {
                ContextToken = TokenHelper.ReadAndValidateContextToken(ContextDetails.ContextTokenString, ContextDetails.ServerUrl);
            }
            catch (Exception)
            {
                ContextToken = null;
            }
        }

        public ContextUtility()
        {
        }

        public SharePointContextToken ContextToken { get; set; }
        public SpContext ContextDetails { get; set; }

        public bool IsValid
        {
            get { return ContextToken != null; }
        }

        public static ContextUtility Current
        {
            get
            {
                ContextUtility spContext = null;
                if (HttpContext.Current.Request.Cookies["SPContext"] != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    spContext = new ContextUtility((SpContext)serializer.Deserialize(HttpContext.Current.Request.Cookies["SPContext"].Value, typeof(SpContext)));
                }
                if (spContext == null || !spContext.IsValid)
                {
                    spContext = new ContextUtility(HttpContext.Current.Request);
                }

                if (spContext.IsValid)
                {
                    return spContext;
                }
                HttpContext.Current.Response.Redirect(GetRedirectUrl());
                return null;
            }
        }

        private static string GetRedirectUrl()
        {
            string hostWebUrl = HttpContext.Current.Request["SPHostUrl"];
            return TokenHelper.GetAppContextTokenRequestUrl(hostWebUrl, HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.ToString()));
        }
    }

   
}