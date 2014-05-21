using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cloudbearing.TimeOffRequestWeb.Pages
{
    public partial class PendingRequestsApprover : PreInitPage
    {
        User CurrentUser;
        Boolean? IsGroupMember;  
        protected void Page_Load(object sender, EventArgs e)
        {            
            lblerrmsg.Text = "";
            if (!IsPostBack)
            {
                Session["CurrentSPContext"] = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
                LoadUserProfile();
                if (IsGroupMember==true)
                {
                    LoadGridCancel();
                    LoadGridNormal();
                }
                else
                {
                    lblerrmsg.Text = " * You are not authorized to view this page.";
                    requestedAbsences.Visible = false;
                    divNormal.Visible = false;
                }
            }
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
            IsGroupMember = objTOR.IsCurrentUserExistInGroup(siteApproverGroupname, CurrentUser.LoginName);
        }


        public void LoadGridNormal()
        {
            UserClass objUser = new UserClass();
            int intCurrentUserId = objUser.GetCurrentUserByApp().Id;
            string CurrentUserLoginName = objUser.GetCurrentUserByApp().LoginName;
            TimeOffRequests objTOR = new TimeOffRequests();
            List<TimeOffRequests> objTORList = objTOR.GetMyProcessingRequests( intCurrentUserId, CurrentUserLoginName);
            //ProcessDateOnFulldays(ref objTOR);
            if (objTORList.Count > 0)
            {
                lvNormal.DataSource = objTORList;
                lvNormal.DataBind();
                lvNormal.Visible = true;
                this.dummyTable2.Visible = false;
            }
            else
            {
                this.lvNormal.Visible = false;
                this.dummyTable2.Visible = true;
            }                
        }
        public void LoadGridCancel()
        {
           
                UserClass objUser = new UserClass();
                int intCurrentUserId = objUser.GetCurrentUserByApp().Id;

                TimeOffRequests objTOR = new TimeOffRequests();
                List<TimeOffRequests> objTORList = objTOR.GetMyCancelAlerts(intCurrentUserId);
                //ProcessDateOnFulldays(ref objTOR);
                if (objTORList.Count > 0)
                {
                    LVreqDetails.DataSource = objTORList;
                    LVreqDetails.DataBind();
                    LVreqDetails.Visible = true;
                    this.dummytable.Visible = false;
                }
                else
                {
                    LVreqDetails.Visible = false;
                    this.dummytable.Visible = false;
                    this.Table2.Visible = false;
                }            
           
        }




        [WebMethod(EnableSession = true)]
        public static string DeleteRequest(string requestid, string startdate, string enddate, string requestorname, string type, string status)
        {

            if (HttpContext.Current.Session["CurrentSPContext"] != null)
            {
                TimeOffRequests obj = new TimeOffRequests();
                obj.RequestID = requestid;
                obj.StartDate = DateTime.Parse(startdate);
                obj.EndDate = DateTime.Parse(enddate);
                if (status == "Approved")
                {
                    string CancelLeaveDay = Config.CancelLeaveDay;
                    ConfigListValues objConfigAppList = new ConfigListValues();
                    objConfigAppList.GetConfigValues( HttpContext.Current.Session["CurrentSPContext"] as SharePointContext);
                    if (objConfigAppList.items != null)
                    {
                        if (objConfigAppList.items["CancelLeaveDay"] != null)
                        { CancelLeaveDay = objConfigAppList.items["CancelLeaveDay"].ToString(); }
                    }
                    DateTime dtNew = DateTime.Now.AddDays(Convert.ToInt32(CancelLeaveDay));
                    if (obj.StartDate > dtNew)
                    {
                        if (obj.DeleteRequest(HttpContext.Current.Session["CurrentSPContext"] as SharePointContext,"1"))
                        {
                            string deptCalName = Config.DepartmentCalendar;//default from web.config                        
                            try
                            {
                                ConfigListValues objConfAppList = new ConfigListValues();
                                objConfAppList.GetConfigValues( HttpContext.Current.Session["CurrentSPContext"] as SharePointContext);
                                if (objConfAppList.items != null)
                                {
                                    if (objConfAppList.items["DepartmentCalendar"] != null)
                                    {
                                        deptCalName = objConfAppList.items["DepartmentCalendar"].ToString();
                                    }
                                }
                                DeptCalendar objDept = new DeptCalendar();
                                objDept.Title = requestorname + "-" + type;
                                DateTime startDate = DateTime.Parse(startdate);
                                objDept.EventTime = startDate;
                                objDept.DeleteEvent(deptCalName,HttpContext.Current.Session["CurrentSPContext"] as SharePointContext);
                                return "Request has been cancelled and deleted.";
                            }
                            catch (Exception ex)
                            {
                            }
                            return "Your request has been successfully submitted for Cancellation. Approver has to review.";
                        }
                        else
                            return "Unable to cancel the Approved Request. Please try again.";
                    }
                    else
                        return "You can cancel the request only which are requested after " + dtNew.ToShortDateString();
                }
                else
                    return "Only Approved Requests will be processed.";
            }
            return "";
        }
    }
}