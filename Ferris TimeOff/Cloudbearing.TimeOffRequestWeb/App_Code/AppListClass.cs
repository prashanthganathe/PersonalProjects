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
    public class AppListClass
    {
        public bool IsListExists(string sharepointUrl, string accessToken,string listname)
        {
            using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
            {
                ListCollection listCollection = clientContext.Web.Lists;
                clientContext.Load(listCollection,lists => 
                                   lists.Include(list => list.Title)
                                        .Where(list => list.Title == listname));
                clientContext.ExecuteQuery();
                if (listCollection.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DeleteList(string sharepointUrl, string accessToken,string listname )
        {          
            using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
            {
                Web web = clientContext.Web;
                List list = web.Lists.GetByTitle(listname);
                list.DeleteObject();
                clientContext.ExecuteQuery();
                return true;
            }
        }


        //public bool CreateNewListFlushOld(string sharepointUrl, string accessToken,string listname, Dictionary<string, string> listFields)
        //{
        //    if(IsListExists( sharepointUrl,  accessToken, listname))
        //    {
        //        DeleteList(sharepointUrl,  accessToken, listname);
        //    }
        //    CreateList(sharepointUrl, accessToken, listname, listFields);
        //}


        //public bool CreateList(string sharepointUrl, string accessToken,string listname, Dictionary<string,string> listFields)
        //{
        //    using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
        //    {
        //        Web web = clientContext.Web;

        //        ListCreationInformation creationInfo = new ListCreationInformation();
        //        creationInfo.Title = listname;
        //        creationInfo.TemplateType = (int)ListTemplateType.GenericList; // Not sure abt the template
               
        //        list.Update();
        //        clientContext.ExecuteQuery();
        //    }
        //}

    }
}