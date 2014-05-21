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

namespace Cloudbearing.TimeOffRequestWeb
{
    public partial class CalendarPage : PreInitPage
    {


        protected void Page_Load(object sender, EventArgs e)
        {           
            NavigateToCalendar();
        }

        public void NavigateToCalendar()
        {            
                string deptCalName = Config.DepartmentCalendar;//default from web.config
                //Get from App Config (custom)
                ConfigListValues objConfAppList = new ConfigListValues();
                objConfAppList.GetConfigValues(null);
                if (objConfAppList.items != null)
                {
                    if (objConfAppList.items["DepartmentCalendar"] != null)                  
                        deptCalName = objConfAppList.items["DepartmentCalendar"].ToString();
                }
                Response.Redirect(Request.QueryString["SPHostUrl"] + "/_layouts/15/start.aspx#/Lists/" + deptCalName, false);    
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ui.aspx?SPHostUrl=" + Request.QueryString["SPHostUrl"].ToString() + "&" + Config.ListURL + "=" + Request.QueryString["SPAppWebUrl"].ToString());
        }
    }
}