using Microsoft.SharePoint.Client;
using Microsoft.IdentityModel.S2S.Tokens;
using System.Net;
using System.IO;
using System.Xml;
using System.Web.Configuration;
using System.Text;
using System;
using System.Web;

namespace Cloudbearing.TimeOffRequestWeb.Pages
{
    public class DeptCalendar
    {
        public string Title { get; set; }
        public DateTime EventTime { get; set; }


       // public bool DeleteEvent(string sharepointUrl, string accessToken, string CalendarName)
        public bool DeleteEvent( string CalendarName,  SharePointContext spContext)
        {

            Microsoft.SharePoint.Client.ListItemCollection listItems;
          //  using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            if(spContext==null)
               spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPHost())
            {

                try
                {
                    Web web = clientContext.Web;
                    ListCollection lists = web.Lists;
                    List selectedList = lists.GetByTitle(CalendarName);
                    clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                    clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                    clientContext.ExecuteQuery();

                    CamlQuery camlQuery = new CamlQuery();
                    StringBuilder camlwhere = new StringBuilder();


                    camlwhere.Append("<Where>");
                    camlwhere.Append("<And>");
                    camlwhere.Append("<Eq><FieldRef Name='Title'  /><Value Type='Text'>" + this.Title + "</Value></Eq>");
                    camlwhere.Append("<Eq><FieldRef Name='EventDate'  /><Value Type='Date'>" + this.EventTime.Year + "-" + this.EventTime.Month + "-" + this.EventTime.Day + "</Value></Eq>");
                    camlwhere.Append("</And>");
                    camlwhere.Append("</Where>");
                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();

                    foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                    {
                        if (oListItem["ID"] != null)
                        {
                            Microsoft.SharePoint.Client.ListItem listItem = selectedList.GetItemById(Convert.ToInt32(oListItem["ID"].ToString()));
                            listItem.DeleteObject();
                        }
                    }
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
