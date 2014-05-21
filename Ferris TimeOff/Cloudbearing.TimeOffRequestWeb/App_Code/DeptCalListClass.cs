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
    public class DeptCalListClass
    {
        public bool AddDeptCal(SharePointContext spContext, string Title, string StartDates, string EndDates)
        {

            // using (var clientContext = TokenHelper.GetClientContextWithContextToken(sharepointUrl, contextToken, hostUrl))
            if (spContext == null)
                spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPHost()) //CreateAppOnlyClientContextForSPAppWeb
            {
                    try
                    {
                        ListCollection lists = clientContext.Web.Lists;
                        List selectedList = lists.GetByTitle("Department Calendar"); // Getting Listname from ConfigList
                        ListItemCreationInformation itemCreateInfo;
                        Microsoft.SharePoint.Client.ListItem newItem;
                        itemCreateInfo = new ListItemCreationInformation();
                        newItem = selectedList.AddItem(itemCreateInfo);
                        newItem["Title"] = Title;
                        newItem["EventDate"] = Convert.ToDateTime(StartDates);
                        newItem["EndDate"] = Convert.ToDateTime(EndDates);
                        newItem["Description"] = "";
                        newItem.Update();
                        clientContext.ExecuteQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                        clientContext.ExecuteQuery();
                        return false;
                    }
            }
        }
    }
}