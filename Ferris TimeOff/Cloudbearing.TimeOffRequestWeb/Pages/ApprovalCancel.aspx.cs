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
using Microsoft.Exchange.WebServices.Data;
using System.Text;

namespace Cloudbearing.TimeOffRequestWeb.Pages
{
    public partial class ApprovalCancel : System.Web.UI.Page
    {
        string timeOffRequestID;
        static User strCurrentUser;
        static int CurrentApproverLevel;
        bool IsFullDay = false;
        string currentStatus = "Pending Approval";
        static string app1Status = "";
        static string app2Status = "";
        static string RequestedByEmail = "";

        #region PageInitialization2
        string appOnlyAccessToken;
        string targetPrincipalName;
        string realm;

        const string SESSION_TARGETPRINCIPAL = "TARGETPRINCIPALNAME";
        const string SESSION_REALM = "REALM";

        public void PageInitialize2()
        {

            Uri _hostWeb = new Uri(Request.QueryString["SPHostUrl"]);

            if (Session[SESSION_TARGETPRINCIPAL] != null && Session[SESSION_REALM] != null)
            {
                targetPrincipalName = Session[SESSION_TARGETPRINCIPAL].ToString();
                realm = Session[SESSION_REALM].ToString();
            }
            else
            {
                string _contextTokenString = TokenHelper.GetContextTokenFromRequest(Page.Request);

                if (!String.IsNullOrEmpty(_contextTokenString))
                {
                    SharePointContextToken contextToken =
                            TokenHelper.ReadAndValidateContextToken(_contextTokenString, Request.Url.Authority);

                    targetPrincipalName = contextToken.TargetPrincipalName;
                    realm = contextToken.Realm;

                    Session.Add(SESSION_TARGETPRINCIPAL, targetPrincipalName);
                    Session.Add(SESSION_REALM, realm);
                }
                else
                {
                    throw new Exception("Context string is empty");
                }
            }

            appOnlyAccessToken = TokenHelper.GetAppOnlyAccessToken(targetPrincipalName, _hostWeb.Authority, realm).AccessToken;



        }
        #endregion
        #region InitializeToken
        SharePointContextToken contextToken;
        string accessToken;
        Uri sharepointUrl;
        string SHAREPOINT_2013_PRINCIPAL = "00000003-0000-0ff1-ce00-000000000000";
        string COOKIE_NAME = "CacheKey";
        public void PageInitialize()
        {
            sharepointUrl = new Uri(Request.QueryString[Config.ListURL]);

            if (Session[COOKIE_NAME] == null)
            {
                string contextTokenString = TokenHelper.GetContextTokenFromRequest(Request);
                Session["contextToken"] = contextTokenString;
                if (contextTokenString != null)
                {
                    contextToken = TokenHelper.ReadAndValidateContextToken(contextTokenString, Request.Url.Authority);
                    var cookieName = contextToken.CacheKey.Substring(0, 40);
                    Session.Add(COOKIE_NAME, cookieName);
                    var refreshToken = contextToken.RefreshToken;
                    Response.Cookies.Add(new HttpCookie(cookieName, refreshToken));
                    accessToken = TokenHelper.GetAccessToken(refreshToken,
                    SHAREPOINT_2013_PRINCIPAL, sharepointUrl.Authority, TokenHelper.GetRealmFromTargetUrl(sharepointUrl)).AccessToken;
                }
                else if (!IsPostBack)
                {
                    Response.Write("Could not find a context token.");
                    return;
                }
            }
            else
            {
                var key = Session[COOKIE_NAME] as string;
                var refreshToken = Request.Cookies[key].Value;
                accessToken = TokenHelper.GetAccessToken(refreshToken, SHAREPOINT_2013_PRINCIPAL, sharepointUrl.Authority, TokenHelper.GetRealmFromTargetUrl(sharepointUrl)).AccessToken;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PageInitialize();
            PageInitialize2();        
            if (!IsPostBack)
            {               
                btnCancel.CommandArgument = accessToken;
                btnChkCalendar.CommandArgument = accessToken;
            }
            LoadUIByGuid(timeOffRequestID, accessToken);
        }


        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("ui.aspx?" + Request.Url.Query.ToString());
        }

        public void LoadUIByGuid(string requestID, string accessToken)
        {
            if (sharepointUrl == null)
                sharepointUrl = new Uri(Config.ListURL);
            TimeOffRequests objTimeOffReq = new TimeOffRequests();
            objTimeOffReq.RequestID= Request.QueryString["RequestID"].ToString();
            objTimeOffReq = objTimeOffReq.GetRequestDetailbyRequestID(sharepointUrl.ToString(), accessToken);           
            //If(current user is concerned Approver, if it is cancelled request, and status=approved)
            if (IsCancelRequested(objTimeOffReq) && IsStatusApproved(objTimeOffReq) && IsConcernedApprover(objTimeOffReq))
            {
                LoadUI(objTimeOffReq);
            }
            else
                ShowUnAuthorizedInfo();
        
        }

        public bool IsCancelRequested(TimeOffRequests objTOR)
        {
            return objTOR.CancelStatus == "Cancel" ? true : false;
        }

        public bool IsStatusApproved(TimeOffRequests objTOR)
        {
            return objTOR.CancelStatus == "Approved" ? true : false;
        }


        public bool IsConcernedApprover(TimeOffRequests objTOR)
        {
            //GetCurrentuser and check is he among 3 approver
            // validate is he concern approver
            string concernedApprover="";
            UserClass objUser = new UserClass();         
            User CurrentUser= objUser.GetCurrentUserByApp();
            if (objTOR.Approver3 != null || objTOR.Approver3 != "")
                concernedApprover = objTOR.Approver3;
            else if (objTOR.Approver2 != null || objTOR.Approver2 != "")
                 concernedApprover = objTOR.Approver2;
                 else if (objTOR.Approver1 != null || objTOR.Approver1 != "")
                       concernedApprover = objTOR.Approver1;

            if (concernedApprover == "")
                return false;
            else
            {
                if (CurrentUser.Title == objTOR.Approver1)
                    return true;
                else
                    return false;
            }
        }

        public void LoadUI(TimeOffRequests objTOR)
        {
            this.lblRequestedBy.Text = objTOR.RequestedBy;
            this.lblRequestedOn.Text = objTOR.RequestedOn;
            this.lblTimeoffType.Text = objTOR.TimeOffType;
            this.lblFullday.Text     = objTOR.isFullDay;
            if (objTOR.isFullDay == "Full Day(s)")
            {
                this.txtStartDate.Text = objTOR.StartDate.Value.ToString("MM/dd/yyyy");
                this.txtEndDate.Text = objTOR.EndDate.Value.ToString("MM/dd/yyyy");
            }
            else
            {              
                this.txtStartDate.Text = objTOR.StartDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
                this.txtEndDate.Text = objTOR.EndDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
            }
            this.rbExcludeWeekendYes.Checked = objTOR.ExcludeWeekend; 
            this.rbExcludeHolidayYes.Checked = objTOR.ExcludeHoliday;
            this.rbExcludeOtherYes.Checked = objTOR.ExcludeOtherDay;
            this.rbExcludeWeekendNo.Checked = !objTOR.ExcludeWeekend;
            this.rbExcludeHolidayNo.Checked = !objTOR.ExcludeHoliday;
            this.rbExcludeOtherNo.Checked = !objTOR.ExcludeOtherDay;

            this.lblApp1.InnerText = objTOR.Approver1;
            this.lblApp2.InnerText = objTOR.Approver2;
            this.lblApp3.InnerText = objTOR.Approver3;
            this.txtNotes.Text = objTOR.Notes;

            this.chkAccessible.Checked = objTOR.IsAccessible;
            this.chkAlternateContact.Checked = objTOR.Alternate;
            this.chkPrivate.Checked = objTOR.IsPrivate;
        }

       

       

        public void ShowUnAuthorizedInfo()
        {
            lblerrmsg.Text = " * You are not authorized to view this page.";
            divVisible.Visible = false;
            divbuttons.Visible = false;
        }

       

        protected void btnCancel_Click(object sender, EventArgs e)
        {
           // UpdateStatus(((Button)sender).CommandArgument, "Denied");
           // Response.Redirect("ui.aspx?SPHostUrl=" + Request.QueryString["SPHostUrl"].ToString() + "&approval=1&" + Config.ListURL + "=" + sharepointUrl.ToString());
            //TODO
            //Delete request
            //

             if (sharepointUrl == null)
                sharepointUrl = new Uri(Config.ListURL);
            TimeOffRequests objTimeOffReq = new TimeOffRequests();
            objTimeOffReq.RequestID= Request.QueryString["RequestID"].ToString();
            objTimeOffReq.DeleteRequest(null);
        }

        protected void btnChkCalendar_Click(object sender, EventArgs e)
        {
            VerifyApprover();
        }

        public void VerifyApprover()
        {
            GroupsClass objTOR = new GroupsClass();
            if (Request.QueryString[Config.ListURL] != null)
                sharepointUrl = new Uri(Request.QueryString[Config.ListURL]);

            string siteApproverGroupname = Config.TimeOffApprovers;//default from web.config

            //Get from App Config (custom)
            ConfigListValues objConfigAppList = new ConfigListValues();
            objConfigAppList.GetConfigValues(null);
            if (objConfigAppList.items != null)
            {
                if (objConfigAppList.items[Config.TimeOffApprovers] != null)
                    siteApproverGroupname = objConfigAppList.items[Config.TimeOffApprovers].ToString();
            }

            UserClass objUser = new UserClass();
            string strCurrentUserTitle = objUser.GetCurrentUserByApp().LoginName;
            if (!objTOR.IsCurrentUserExistInGroup( siteApproverGroupname, strCurrentUserTitle))
            {
                lblerrmsg.Text = " You do not have Access Permission";
            }
            else
            {
                string deptCalName = Config.DepartmentCalendar;//default from web.config
                //Get from App Config (custom)
                ConfigListValues objConfAppList = new ConfigListValues();
                objConfAppList.GetConfigValues(null);
                if (objConfAppList.items != null)
                {
                    if (objConfAppList.items[deptCalName] != null)
                    {
                        deptCalName = objConfAppList.items[deptCalName].ToString();
                    }
                }
                Response.Redirect(Request.QueryString["SPHostUrl"] + "/_layouts/15/start.aspx#/Lists/" + deptCalName, false);
            }
        }
      
    }
}