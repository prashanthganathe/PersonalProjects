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
using Microsoft.SharePoint.Client.Utilities;

namespace Cloudbearing.TimeOffRequestWeb.Pages
{
    public partial class UI : PreInitPage
    {
        User CurrentUser;
        Boolean? IsGroupMember;
        
        protected void Page_Load(object sender, EventArgs e)
        {           
                if (!IsPostBack)
                {
                    LoadUserProfile();
                }
                HideAdminDiv();
                HideApproverDiv();
                LoadTimeOffTypes(); 
        }

        public void LoadUserProfile()
        {
              LoadUser();
              IsMember(CurrentUser); 
        }

        public void LoadUser()
        {
            UserClass objUser = new UserClass();
            CurrentUser = objUser.GetCurrentUserByApp();
        }

        public void IsMember(User CurrentUser)
        {
             string siteApproverGroupname = Config.TimeOffApprovers;
                ConfigListValues objConfigAppList = new ConfigListValues();
                objConfigAppList.GetConfigValues(null);
                if (objConfigAppList.items != null)
                {
                    if (objConfigAppList.items.ContainsKey("TimeOffApprovers"))
                        siteApproverGroupname = objConfigAppList.items["TimeOffApprovers"].ToString();
                }

           GroupsClass objTOR = new GroupsClass();
           IsGroupMember= objTOR.IsCurrentUserExistInGroup(siteApproverGroupname, CurrentUser.LoginName);
        }




        public void IsConfigured()
        {
                if (IsGroupMember == null || CurrentUser == null)
                    LoadUserProfile();
                if(IsGroupMember==true)
                {
                    //Redirect to Config List if not updated.
                     ConfigListClass objConfig = new ConfigListClass();
                     if (objConfig.SetConfigItems() == "AdminConfigRequired")
                         Response.Redirect(Request.QueryString["SPAppWebUrl"].ToString() + "/Lists/ConfigList", false);
                }         
        }

        public void HideApproverDiv()
        {
            this.approverdiv.Visible = false;
            if (IsGroupMember == null || CurrentUser == null)
                LoadUserProfile();
           if(IsGroupMember==true)
               this.approverdiv.Visible = true;
           
        }

        public void HideAdminDiv()
        {            
            this.divAdmin.Visible = false;
            if (IsGroupMember == null || CurrentUser == null)
                LoadUserProfile();
            if (CurrentUser.IsSiteAdmin)
            {
                this.divAdmin.Visible = true;
                IsConfigured();
            }    
        }

        public void LoadTimeOffTypes()
        {
            try
            {      
                ConfigListClass objConfigAppList = new ConfigListClass();            
                objConfigAppList.LoadTimeOffTypes();
            }
            catch (Exception ex)
            {
            }
        }
    }
}