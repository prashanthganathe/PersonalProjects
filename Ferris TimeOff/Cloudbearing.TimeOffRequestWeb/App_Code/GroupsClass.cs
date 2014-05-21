using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using Microsoft.IdentityModel.S2S.Tokens;
using System.Net;
using System.IO;
using System.Xml;
using System.Web.Configuration;

namespace Cloudbearing.TimeOffRequestWeb
{

    

    public class UserClass
    {
       
        public User GetCurrentUser(string sharepointUrl, string accessToken,string hostUrl)
        {
            User objUser;
            using (ClientContext clientContext = TokenHelper.GetClientContextWithContextToken(sharepointUrl, accessToken, hostUrl))
            {
            //    try
            //    {
                    Web web = clientContext.Web;
                    clientContext.Load(web);
                    clientContext.ExecuteQuery();
                    clientContext.Load(web.CurrentUser);
                    clientContext.ExecuteQuery();
                    objUser = clientContext.Web.CurrentUser;
                //}
                //catch (Exception ex)
                //{
                //    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                //    clientContext.ExecuteQuery();
                //    return objUser;
                //}
            }
            return objUser;
        }

        public User GetCurrentUserByApp()
        {
            User objUser;
          //  using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            var spContext =    SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                //try
                //{
                Web web = clientContext.Web;
                clientContext.Load(web);
                clientContext.ExecuteQuery();
                clientContext.Load(web.CurrentUser);
                clientContext.ExecuteQuery();
                objUser = clientContext.Web.CurrentUser;
                // }
                //catch (Exception ex)
                //{
                //    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                //    return objUser;
                //}
            }
            return objUser;
        }
    }


    public class GroupsClass
    {
        //public bool IsAdmin(string sharepointUrl, string accessToken, string hostUrl, string GroupName)
        //{
        //    using (ClientContext clientContext = TokenHelper.GetClientContextWithContextToken(sharepointUrl, accessToken, hostUrl))
        //    {
        //        GroupCollection siteGroups = clientContext.Web.SiteGroups;
        //        Group membersGroup = siteGroups.GetByName(GroupName);
        //        clientContext.Load(membersGroup.Users);
        //        clientContext.ExecuteQuery();
        //        clientContext.Load(clientContext.Web.CurrentUser);
        //        clientContext.ExecuteQuery();

        //        return clientContext.Web.CurrentUser.IsSiteAdmin;
        //    }
        //}


        //public bool IsAdminByApp(string sharepointUrl, string accessToken, string GroupName)
        //{
        //    using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
        //    {
        //        //GroupCollection siteGroups = clientContext.Web.SiteGroups;
        //        //Group membersGroup = siteGroups.GetByName(GroupName);
        //        //clientContext.Load(membersGroup.Users);
        //        //clientContext.ExecuteQuery();
                

        //        return clientContext.Web.CurrentUser.IsSiteAdmin;
        //    }
        //}

        //public bool IsUserExistinGroupByApp(string GroupName, string strCurrentUserTitle)
        //{
        //   // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
        //    var spContext =    SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
        //    using (var clientContext = spContext.CreateAppOnlyClientContextForSPHost())
        //    {
        //        try
        //        {
        //            GroupCollection siteGroups = clientContext.Web.SiteGroups;
        //            Group membersGroup = siteGroups.GetByName(GroupName);
        //            clientContext.Load(membersGroup.Users);
        //            clientContext.ExecuteQuery();
        //            //clientContext.Load(clientContext.Web.CurrentUser);
        //            //clientContext.ExecuteQuery();
        //           // string currentuserTitle = clientContext.Web.CurrentUser.Title;
        //            foreach (User usr in membersGroup.Users)
        //            {
        //                if (strCurrentUserTitle.Equals(usr.LoginName,StringComparison.InvariantCultureIgnoreCase))
        //                {                           
        //                    return true;
        //                }
        //            }
        //            return false;
        //        }
        //        catch (Exception ex)
        //        {
        //            Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
        //            clientContext.ExecuteQuery();
        //            return false;
        //        }
        //    }
        //}

        public UserCollection GetUserList(string GroupName)
        {           
               // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
                {                  
                    try
                    {
                        GroupCollection siteGroups = clientContext.Web.SiteGroups;  
                        Group membersGroup = siteGroups.GetByName(GroupName);
                        clientContext.Load(membersGroup.Users);
                        clientContext.ExecuteQuery();
                        return membersGroup.Users;
                       
                    }
                    catch (Exception ex)
                    {
                        Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                        clientContext.ExecuteQuery();
                        return null ;
                    }
                }
               
            }



        public bool IsCurrentUserExistInGroup(string GroupName,string strUserTitle)
        {
            try
            {
               // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
                var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
                using (var clientContext = spContext.CreateAppOnlyClientContextForSPHost())
                {
               // using (var clientContext = TokenHelper.GetClientContextWithContextToken(sharepointUrl, accessToken, hostUrl))
               // {
               //WebCollection result = oWebsite.GetSubwebsForCurrentUser(new SubwebQuery()); 
                    try
                    {
                        GroupCollection siteGroups = clientContext.Web.SiteGroups;

                        //WebCollection result = clientContext.Web.GetSubwebsForCurrentUser(new SubwebQuery());
                        //clientContext.Load(result, n => n.Include(o => o.Title));
                        //clientContext.ExecuteQuery(); 

                        Group membersGroup = siteGroups.GetByName(GroupName);
                        clientContext.Load(membersGroup.Users);
                        clientContext.ExecuteQuery();
                        //clientContext.Load(clientContext.Web.CurrentUser);
                        //clientContext.ExecuteQuery();
                       // string currentuserTitle = clientContext.Web.CurrentUser.Title;
                        foreach (User usr in membersGroup.Users)
                        {
                            if (strUserTitle.Equals(usr.LoginName,StringComparison.InvariantCultureIgnoreCase))
                            {
                               
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                        clientContext.ExecuteQuery();
                        return false;
                    }
                }
                return false;
            }
            catch (Exception e)
            { return false; }
            
        }


        public string GetRequesterEmail(string requestID,string sharepointUrl, string accessToken)
        {
            string req = ""; 
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
            {
                Web web = clientContext.Web;
                ListCollection lists = web.Lists;
                List selectedList = lists.GetByTitle("TimeOffRequests");
                clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                clientContext.ExecuteQuery();

                CamlQuery camlQuery = new CamlQuery();
                System.Text.StringBuilder camlwhere = new System.Text.StringBuilder();
                camlwhere.Append("<Where>");
                camlwhere.Append("<Eq><FieldRef Name='RequestID'/><Value Type='Text'>" + requestID + "</Value></Eq>");
                camlwhere.Append("</Where>");
                camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                listItems = selectedList.GetItems(camlQuery);
                clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                clientContext.ExecuteQuery();               
                if (listItems == null || listItems.Count == 0)
                    return "";
                else
                {                    
                    foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                    {
                        var p = oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        break;
                    }
                }               
            }
            return req;
        }
    }
}