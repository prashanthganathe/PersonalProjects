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
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;

namespace Cloudbearing.TimeOffRequestWeb.Pages
{
    public partial class MyList : PreInitPage
    {       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {              
                Session["CurrentSPContext"] = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);             
                TimeOffRequests objTOR = new TimeOffRequests();
                List<TimeOffRequests> objTORList = objTOR.GetMyTimeOfRequests("TimeOffRequests");
                foreach (TimeOffRequests temp in objTORList)
                {
                    if (temp.CancelStatus == "Cancel")                 
                        temp.Status = temp.Status + " [Cancellation pending.]";                 
                }               
                if (objTORList.Count > 0)
                {
                    LVreqDetails.DataSource = objTORList;
                    LVreqDetails.DataBind();
                    LVreqDetails.Visible=true;
                    this.dummytable.Visible = false;
                }
                else
                {
                    LVreqDetails.Visible = false;
                    this.dummytable.Visible = true;
                }                
            }
        }      


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ui.aspx?SPHostUrl=" + Request.QueryString["SPHostUrl"].ToString() + "&" + Config.ListURL + "=" + Request.QueryString[Config.ListURL].ToString());
           
        }

        [WebMethod(EnableSession = true)]
        public static string CancelRequest(string requestid, string startdate, string enddate, string status)
        {
            if (HttpContext.Current.Session["CurrentSPContext"] != null)
            {
                TimeOffRequests obj = new TimeOffRequests();
                obj.RequestID = requestid;
                obj.StartDate = DateTime.Parse(startdate);
                obj.EndDate = DateTime.Parse(enddate);
                if (status == "Pending Approval")
                {
                    if (obj.CancelRequest(HttpContext.Current.Session["CurrentSPContext"] as SharePointContext, status))                   
                    {
                        //TO DO cancellation email to approver
                        return "Your request has been successfully cancelled.";
                    }
                    else
                        return "Could not able to cancel the request.";
                }
                else
                    if (status == "Approved")
                    {
                        string CancelLeaveDay = Config.CancelLeaveDay;
                        ConfigListValues objConfigAppList = new ConfigListValues();
                        objConfigAppList.GetConfigValues(HttpContext.Current.Session["CurrentSPContext"] as SharePointContext);

                        if (objConfigAppList.items != null)
                        {
                            if (objConfigAppList.items["CancelLeaveDay"] != null)
                            { CancelLeaveDay = objConfigAppList.items["CancelLeaveDay"].ToString(); }
                        }
                        DateTime dtNew = DateTime.Now.AddDays(Convert.ToInt32(CancelLeaveDay));
                        if (obj.StartDate > dtNew)
                        {

                            if (obj.CancelRequest(HttpContext.Current.Session["CurrentSPContext"] as SharePointContext, status))
                            //if (obj.CancelRequest(status))
                            {
                                return "Your request has been successfully submitted for Cancellation. Approver has to review.";
                            }
                        }
                        else
                            return "You can cancel the request only which are requested after " + dtNew.ToShortDateString();
                    }
                return "";
            }
            return "";
        }
    }
}