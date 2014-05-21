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
using System.Text;

namespace Cloudbearing.TimeOffRequestWeb
{

    public static class ConfigSettings
    {
         public static Dictionary<string, string> settings;
         static ConfigSettings()
         {
             //settings= (new ConfigListClass().GetConfigDetails("",""));
         }

         //public static bool SaveDefaultConfig()
         //{
         //}
    }


    public class ConfigListClass
    {

        public bool LoadTimeOffTypes()
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            //using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
            var spContext =    SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
            {
                try
                {
                    Web web = clientContext.Web;
                    ListCollection lists = web.Lists;
                    List selectedList = lists.GetByTitle(WebConfigurationManager.AppSettings["TimeOffTypes"]);
                    clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                    clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                    clientContext.ExecuteQuery();
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = @"<View><Query><Where><IsNotNull><FieldRef Name='ID' /></IsNotNull></Where></Query><FieldRef Name='Title' /><FieldRef Name='Description' /><FieldRef Name='ApprovalRequired' /></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();

                    StringBuilder timeofftypes = new StringBuilder();
                    timeofftypes.Append("OTP Overtime Pay,LVC Vacation Pay,LSK Sick Leave Pay,LCL Consulting,LFN Funeral Leave,LJD Jury Duty,LJR Jury Duty Reversal,");
                    timeofftypes.Append("LML Military Leave ,LUB Union Business,LPR Person Leave,LPS Personal Leave Charge to Sick,LCE Compensation Hours Earned,");
                    timeofftypes.Append("LCU Compensation Hours Used,LST Short Term Disability,LWP Leave Without Pay,LMW FMLA Leave Without Pay,LMV FMLA Vacation");
                    timeofftypes.Append("LMS FMLA Sick,LMC FMLA Comp,LMB FMLA Leave w/o Pay w/Benefits,LMN FMLA Leave w/o Pay w/o Benefits,LMR FMLA Personal,");
                    timeofftypes.Append("LMP FMLA Personal Charge to Sick,LMD FMLA Short Term Disability,FST Substitute Teaching,FTV Travel Increment,PAD Additional Compensation");

                    if (listItems == null || listItems.Count == 0)
                    {
                        ListItemCreationInformation itemCreateInfo;
                        Microsoft.SharePoint.Client.ListItem newItem;
                        foreach (string item in timeofftypes.ToString().Split(','))
                        {
                            itemCreateInfo = new ListItemCreationInformation();
                            newItem = selectedList.AddItem(itemCreateInfo);
                            newItem["Title"] = item.Trim();
                            newItem["Description"] = item.Trim();
                            newItem["ApprovalRequired"] = true;
                            newItem.Update();
                        }
                        clientContext.ExecuteQuery();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                    return false;
                }
            }
        }

        public string SetConfigItems()
        {
            //try
            //{
            //    bool val = LoadTimeOffTypes(sharepointUrl, accessToken);
            //}
            //catch (Exception ex)
            //{
            //}

            Microsoft.SharePoint.Client.ListItemCollection listItems;
          //   using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
            var spContext =  SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateUserClientContextForSPAppWeb())
            {
                try
                {
                    Web web = clientContext.Web;
                    ListCollection lists = web.Lists;
                    List selectedList = lists.GetByTitle(WebConfigurationManager.AppSettings["ConfigList"]);
                    clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                    clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                    clientContext.ExecuteQuery();
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = @"<View><Query><Where><IsNotNull><FieldRef Name='ID' /></IsNotNull></Where></Query><FieldRef Name='Key' /><FieldRef Name='Value' /></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();

                    if (listItems == null || listItems.Count == 0)
                    {
                        ListItemCreationInformation itemCreateInfo;
                        Microsoft.SharePoint.Client.ListItem newItem;
                        foreach (string item in Config.ConfigItems.Split(','))
                        {
                            itemCreateInfo = new ListItemCreationInformation();
                            newItem = selectedList.AddItem(itemCreateInfo);
                            newItem["Key"] = item;
                            newItem["Values"] = GetDefaultValue(item);
                            newItem.Update();

                        }
                        clientContext.ExecuteQuery();
                        return "AdminConfigRequired";
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                    return "";
                }
            }

         
        }

        public string GetDefaultValue(string item)            
        {
            switch (item)
            {
                case "WorkingHours":
                    {
                        return Config.WorkingHours;
                    }
                case "SenderEmail":
                    {
                        return Config.SenderEmail;
                    }
                case "SenderPassword":
                    {
                        return Config.SenderPassword;
                    }
                case "DepartmentCalendar":
                        {
                            return Config.DepartmentCalendar;
                        }
                case "TimeOffApprovers":
                   {
                       return Config.TimeOffApprovers;
                   }
                case "NotesToolTip":
                   {
                       return Config.NotesToolTip;
                   }

                case "TimeOffTypeExceptionList":
                   {
                       return Config.TimeOffTypeExceptionList;
                   }

                case "CancelLeaveDay":
                   {
                       return Config.CancelLeaveDay;
                   }
                default:
                    {
                        return "";
                    }
            }
        }


        public Dictionary<string, string> GetConfigDetails(SharePointContext spContext)
        {
            Dictionary<string, string> ConfigDetails = new Dictionary<string, string>();
            Microsoft.SharePoint.Client.ListItemCollection listItems;
           // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
            if(spContext==null)
             spContext =    SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb()) // CreateAppOnlyClientContextForSPAppWeb throwing err
            {
                try
                {
                    Web web = clientContext.Web;
                    ListCollection lists = web.Lists;
                    List selectedList = lists.GetByTitle(WebConfigurationManager.AppSettings["ConfigList"]);
                    clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                    clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                    clientContext.ExecuteQuery();

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = @"<View><Query><Where><IsNotNull><FieldRef Name='ID' /></IsNotNull></Where></Query><FieldRef Name='Key' /><FieldRef Name='Values' /></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                    if (listItems == null || listItems.Count == 0)
                        return null;
                    else
                    {
                        foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                        {
                            ConfigDetails.Add(oListItem["Key"].ToString(), oListItem["Values"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                }
            }
            return ConfigDetails;
        }
    }

    public static class Config
    {
        static public string ClientId { get; set; }
        static public string TimeOffApprovers { get; set; }
        static public string DepartmentCalendar { get; set; }
        static public string HolidayList { get; set; }
        static public string OtherTimeOffDaysList { get; set; }
        static public string TimeOffTypes { get; set; }
        static public string ConfigList { get; set; }
        static public string WorkingHours { get; set; }
        static public string SenderEmail { get; set; }
        static public string SenderPassword { get; set; }
        static public string ListURL { get; set; }
        static public string TimeOffRequests { get; set; }
        static public string ConfigItems { get; set; }
        static public string NotesToolTip { get; set; }
        static public string TimeOffTypeExceptionList { get; set; }
        static public string CancelLeaveDay { get; set; }


        static Config()
        {

            ConfigListClass obj = new  ConfigListClass();
          //  Dictionary<string,string> configlist = obj.GetConfigDetails( requestID,  sharepointUrl,  accessToken);
           //  Cache all these values in static properties.
            ClientId = WebConfigurationManager.AppSettings["ClientId"];
            TimeOffApprovers = WebConfigurationManager.AppSettings["TimeOffApprovers"];
            DepartmentCalendar = WebConfigurationManager.AppSettings["DepartmentCalendar"];
            HolidayList = WebConfigurationManager.AppSettings["HolidayList"];
            OtherTimeOffDaysList = WebConfigurationManager.AppSettings["OtherTimeOffDaysList"];
            TimeOffTypes = WebConfigurationManager.AppSettings["TimeOffTypes"];
            ConfigList = WebConfigurationManager.AppSettings["ConfigList"];

            SenderEmail = WebConfigurationManager.AppSettings["SenderEmail"];
            SenderPassword = WebConfigurationManager.AppSettings["SenderPassword"];           
            WorkingHours = WebConfigurationManager.AppSettings["WorkingHours"];
            ListURL = WebConfigurationManager.AppSettings["ListURL"];
            TimeOffRequests = WebConfigurationManager.AppSettings["TimeOffRequests"];
            ConfigItems = WebConfigurationManager.AppSettings["ConfigItems"];
            NotesToolTip = WebConfigurationManager.AppSettings["NotesToolTip"];
            TimeOffTypeExceptionList = WebConfigurationManager.AppSettings["TimeOffTypeExceptionList"];
            CancelLeaveDay = WebConfigurationManager.AppSettings["CancelLeaveDay"];
           
           
            
        }
    }


    public  class ConfigListValues
    {
        public  Dictionary<string, string> items;
        public  void GetConfigValues(SharePointContext spContext)
        {
            //if (items == null)
            //{
                ConfigListClass obj = new ConfigListClass();
                items = new Dictionary<string, string>();
                items = obj.GetConfigDetails(spContext);
            //}
        }
    }




        
}
