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

namespace Cloudbearing.TimeOffRequestWeb
{
    public partial class ApprovalForm : PreInitPage
    {
       
        string timeOffRequestID;       
        static User strCurrentUser;
        static int CurrentApproverLevel;          
        bool IsFullDay=false; 
        string currentStatus = "Pending Approval";      
        static string app1Status = "";
        static string app2Status = "";
        static string app3Status = "";
        static string RequestedByEmail = "";



        User CurrentUser;
        Boolean? IsGroupMember;
        protected void Page_Load(object sender, EventArgs e)
        {
            timeOffRequestID = Request.QueryString["RequestID"];
            if(!IsPostBack)
            {
                LoadUserProfile();
                //btnHome.CommandArgument = accessToken;
                //btnSubmit.CommandArgument = accessToken;
                //btnCancel.CommandArgument = accessToken;
                //btnChkCalendar.CommandArgument = accessToken;                                 
            }
            if (timeOffRequestID != null)
                LoadUIByGuid(timeOffRequestID);
            else
            {
                ShowUnAuthorizedInfo();
                this.lblerrmsg.Text = "* Request information is not loaded, please try again.";
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

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("ui.aspx?" + Request.Url.Query.ToString());
        }
        public void LoadUIByGuid(string requestID)
        {
            //if(sharepointUrl ==null)
            //    sharepointUrl = new Uri(Config.ListURL);
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            //using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken())
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
                {
                    try
                    {
                        Web web = clientContext.Web;
                        ListCollection lists = web.Lists;
                        List selectedList = lists.GetByTitle("TimeOffRequests");
                        clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                        clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                        clientContext.ExecuteQuery();

                        CamlQuery camlQuery = new CamlQuery();
                        StringBuilder camlwhere = new StringBuilder();
                        camlwhere.Append("<Where>");
                        camlwhere.Append("<Eq><FieldRef Name='RequestID'/><Value Type='Text'>" + requestID + "</Value></Eq>");
                        camlwhere.Append("</Where>");
                        camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                        listItems = selectedList.GetItems(camlQuery);
                        clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                        clientContext.ExecuteQuery();

                        LoadUI(listItems);
                       // VerifyApp();
                    }
                    catch (Exception ex)
                    {
                        Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                        clientContext.ExecuteQuery();
                    }
                }            
            
        }

        public void LoadUI(Microsoft.SharePoint.Client.ListItemCollection listItems)
        {
            string app1Status="", app2Status="", app3Status="";
            if (listItems == null || listItems.Count == 0)
                ShowUnAuthorizedInfo();
            else
            {
                foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                {
                    var docType = oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue;
                    if (docType != null)
                        this.lblRequestedBy.Text = docType.LookupValue;
                    if (oListItem["Created"] != null)
                        this.lblRequestedOn.Text = oListItem["Created"].ToString();
                    if (oListItem["TimeOffType"] != null)
                        this.lblTimeoffType.Text = (string)oListItem["TimeOffType"];
                    if (oListItem["IsFullDay"] != null)
                    {
                        this.lblFullday.Text = (bool)oListItem["IsFullDay"] == true ? "Full Day(s)" : "Partial Day";
                        IsFullDay = (bool)oListItem["IsFullDay"];
                    }
                    if (oListItem["StartDateTime"] != null)
                    {
                        if (IsFullDay)
                            this.txtStartDate.Text = ((DateTime)oListItem["StartDateTime"]).ToString("MM/dd/yyyy");
                        else
                            this.txtStartDate.Text = ((DateTime)oListItem["StartDateTime"]).ToString("MM/dd/yyyy hh:mm:ss tt");
                    }
                    if (oListItem["EndDateTime"] != null)
                    {
                        if (IsFullDay)
                            this.txtEndDate.Text = ((DateTime)oListItem["EndDateTime"]).ToString("MM/dd/yyyy");//TODO
                        else
                            this.txtEndDate.Text = ((DateTime)oListItem["EndDateTime"]).ToString("MM/dd/yyyy hh:mm:ss tt");//TODO
                    }
                    docType = oListItem["Status"] as Microsoft.SharePoint.Client.FieldLookupValue;
                    if (oListItem["Status"] != null)                                     
                        currentStatus = oListItem["Status"].ToString();
                    if (oListItem["ExcludeWeekends"] != null)
                    {
                        this.rbExcludeWeekendYes.Checked = (bool)oListItem["ExcludeWeekends"];
                        this.rbExcludeWeekendNo.Checked = !this.rbExcludeWeekendYes.Checked;
                    }
                    if (oListItem["ExcludeHolidays"] != null)
                    {
                        this.rbExcludeHolidayYes.Checked = (bool)oListItem["ExcludeHolidays"];
                        this.rbExcludeHolidayNo.Checked = !this.rbExcludeHolidayYes.Checked;
                    }
                    if (oListItem["ExcludeOtherDays"] != null)
                    {
                        this.rbExcludeOtherYes.Checked = (bool)oListItem["ExcludeOtherDays"];
                        this.rbExcludeOtherNo.Checked = !this.rbExcludeOtherYes.Checked;
                    }
                    docType = oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue;
                    if (docType != null)
                        this.lblappr1.Text = docType == null ? "UnAssigned" : docType.LookupValue;
                    docType = oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue;
                    if (docType != null)
                        this.lblappr2.Text = docType == null ? "UnAssigned" : docType.LookupValue;
                    docType = oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue;
                    if (docType != null)
                        this.lblappr3.Text = docType == null ? "UnAssigned" : docType.LookupValue;
                    if (oListItem["Approver1Status"] != null)
                        app1Status = oListItem["Approver1Status"].ToString();   
                    if (oListItem["Approver2Status"] != null)
                        app2Status = oListItem["Approver2Status"].ToString();
                    if (oListItem["Approver3Status"] != null)
                        app3Status = oListItem["Approver3Status"].ToString();
                    if (oListItem["RequestedByEmail"] != null)
                        RequestedByEmail = oListItem["RequestedByEmail"].ToString();

                    if (oListItem["HasAlternateContact"] != null)
                        chkAlternateContact.Checked = (bool)oListItem["HasAlternateContact"];
                    if (oListItem["IsAccessible"] != null)
                        chkAccessible.Checked = (bool)oListItem["IsAccessible"];
                    if (oListItem["IsPrivate"] != null)
                        chkPrivate.Checked = (bool)oListItem["IsPrivate"];  
                    
                if(oListItem["Notes"]!=null)
                         txtNotes.Text = oListItem["Notes"] != null ? oListItem["Notes"].ToString() : "";
             
  
                }

                if (currentStatus == "Denied" || currentStatus =="Approved")
                {
                    ShowUnAuthorizedInfo();
                    this.lblerrmsg.Text = currentStatus == "Denied" ? " Request has been already rejected. Could not process this further" : " Request has  already  been Approved. Could not process this further";
                }

                else
                {
                  

                    if (CurrentUser == null)
                        LoadUserProfile();
                
                   
                    int CurrentApproverLevel = 0;
                  
                    if (app1Status == "Pending Approval" && CurrentUser.Title == this.lblappr1.Text)
                        CurrentApproverLevel = 1;
                  
                   if(app1Status == "Approved" && app2Status == "Pending Approval" && CurrentUser.Title == this.lblappr2.Text)
                       CurrentApproverLevel = 2;
                       
                    if(app2Status == "Approved"  && app3Status == "Pending Approval" && CurrentUser.Title == this.lblappr3.Text)
                        CurrentApproverLevel = 3;

                    if (CurrentApproverLevel==0)
                    {
                        ShowUnAuthorizedInfo();
                        lblerrmsg.Text = " You are not entitled to process this request";
                    }


                     //   LoadCurrentApproverlevel();
                    if (CurrentApproverLevel == 1)
                    {
                        if(app1Status != "Pending Approval")
                        {                            
                                ShowUnAuthorizedInfo();
                                lblerrmsg.Text = "You can't process this request further.";                            
                        }
                    }
                    if (CurrentApproverLevel == 2)
                    {
                        if (app1Status != "Approved")
                        {
                            ShowUnAuthorizedInfo();
                            lblerrmsg.Text = " Approver1 : " + lblappr1.Text + " has to process this request prior your actions";
                        }
                        else
                        {
                            if (this.lblappr2.Text != CurrentUser.Title)
                            {
                                ShowUnAuthorizedInfo();
                                lblerrmsg.Text = " Approver1 : " + lblappr1.Text + " has to process this request prior your actions";
                            }
                        }
                    }
                    else
                        if (CurrentApproverLevel == 3)
                        {
                            if (app2Status != "Approved")
                            {
                                ShowUnAuthorizedInfo();
                                lblerrmsg.Text = " Approver2 : " + lblappr2.Text + " has to process this request prior your actions";
                            }
                            else
                            {
                                if (this.lblappr3.Text != CurrentUser.Title)
                                {
                                    ShowUnAuthorizedInfo();
                                    lblerrmsg.Text = CurrentUser.Title + " has to process the request.";
                                }
                            }
                        }
                }
            }
        }   

        public void ShowUnAuthorizedInfo()
        {
            lblerrmsg.Text = " * You are not authorized to view this page.";
            divVisible.Visible = false;
            divbuttons.Visible = false;
        }       

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GroupsClass obj = new GroupsClass();
           // sharepointUrl = new Uri(Request.QueryString[Config.ListURL]);
            timeOffRequestID = Request.QueryString["RequestID"];
            string[] reqdAtten = { RequestedByEmail };
            string[] optAtten={};
            if (UpdateStatus(((Button)sender).CommandArgument, "Approved"))
            {
                string msg = "Time Off Request of " + this.lblTimeoffType.Text + ": on " + this.txtStartDate.Text + " to " + this.txtEndDate.Text + " has been Approved";
                EWSClass objEWS = new EWSClass();
                if (this.lblFullday.Text == "Full Day(s)")
                {
                    this.txtStartDate.Text += " 12:00 AM";
                    this.txtEndDate.Text += " 12:00 PM";
                }

                        
                string UserID = Config.SenderEmail;//default from web.config
                string UserPassword = Config.SenderPassword;
                string WorkingHours = Config.WorkingHours;            
                ConfigListValues objConfigAppList = new ConfigListValues();
                objConfigAppList.GetConfigValues(null);
                if (objConfigAppList.items != null)
                {
                    if (objConfigAppList.items.ContainsKey("SenderEmail"))                   
                        UserID = objConfigAppList.items["SenderEmail"].ToString();  
                    if (objConfigAppList.items.ContainsKey("SenderPassword"))                  
                        UserPassword = objConfigAppList.items["SenderPassword"].ToString();
                    if (objConfigAppList.items.ContainsKey("WorkingHours"))
                        WorkingHours = objConfigAppList.items["WorkingHours"].ToString();
                }
                objEWS.SetupCalendarEvent(msg, this.txtStartDate.Text, this.txtEndDate.Text, reqdAtten, optAtten, UserID, UserPassword, WorkingHours);
                try
                {
                    string siteToken;
                    if (Session["contextToken"] != null)
                    {
                        DeptCalListClass objDept = new DeptCalListClass();
                        siteToken = Session["contextToken"] as string;
                        objDept.AddDeptCal(null, this.lblRequestedBy.Text + "-" + this.lblTimeoffType.Text, this.txtStartDate.Text, this.txtEndDate.Text);
                    }
                }
                catch { }
                Response.Redirect("ui.aspx?SPHostUrl=" + Request.QueryString["SPHostUrl"].ToString() + "&approval=1&" + Config.ListURL + "=" + Request.QueryString["SPAppWebUrl"].ToString());  
            }
            Response.Redirect("ui.aspx?SPHostUrl=" + Request.QueryString["SPHostUrl"].ToString() + "&approval=0&" + Config.ListURL + "=" + Request.QueryString["SPAppWebUrl"].ToString());               
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {          
           UpdateStatus(((Button)sender).CommandArgument, "Denied");
           Response.Redirect("ui.aspx?SPHostUrl=" + Request.QueryString["SPHostUrl"].ToString() + "&approval=1&" + Config.ListURL + "=" + Request.QueryString["SPAppWebUrl"].ToString());    
        }

        protected void btnChkCalendar_Click(object sender, EventArgs e)
        {
          //  VerifyApprover();
            string deptCalName = Config.DepartmentCalendar;//default from web.config
            //Get from App Config (custom)
            ConfigListValues objConfAppList = new ConfigListValues();
            objConfAppList.GetConfigValues(null);
            if (objConfAppList.items.Count>0 )
            {
                if (objConfAppList.items["DepartmentCalendar"] != null)             
                    deptCalName = objConfAppList.items["DepartmentCalendar"].ToString();
            }
            Response.Redirect(Request.QueryString["SPHostUrl"] + "/_layouts/15/start.aspx#/Lists/" + deptCalName, false);
        }

    
        public bool VerifyApp()
        {
            if (CurrentUser == null)
                LoadUserProfile();

            switch (CurrentApproverLevel)
            {
                case 1:
                    {
                        if (CurrentUser.Title != this.lblappr1.Text)
                        {
                            ShowUnAuthorizedInfo();
                            this.lblerrmsg.Text = this.lblappr1.Text + " has to process the request.";
                            return false;
                        }
                        break;
                    }
                case 2:
                    {
                        if (CurrentUser.Title != this.lblappr2.Text)
                        {
                            ShowUnAuthorizedInfo();
                            this.lblerrmsg.Text = this.lblappr2.Text + " has to process the request.";
                            return false;
                        }
                        break;
                    }
                case 3:
                    {
                        if (CurrentUser.Title != this.lblappr3.Text)
                        {
                            ShowUnAuthorizedInfo();
                            this.lblerrmsg.Text = this.lblappr3.Text + " has to process the request.";
                            return false;
                        }
                        break;
                    }
            }
         
            return true;
        }

        public bool UpdateStatus(string accessToken, string status)
        {
            bool returnvalue = false;
            //if (!VerifyApp())
            //    return false;
            if (Request.QueryString[Config.ListURL] != null)
            {
              //  sharepointUrl = new Uri(Request.QueryString[Config.ListURL]);
                try
                {
                  //  using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
                    var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
                    using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
                    {
                        Web web = clientContext.Web;
                        ListCollection lists = web.Lists;
                        List selectedList = lists.GetByTitle("TimeOffRequests");
                        clientContext.Load<ListCollection>(lists);
                        clientContext.Load<List>(selectedList);
                        clientContext.ExecuteQuery();

                        CamlQuery camlQuery = new CamlQuery();
                        string camlwhere = "<Where>";
                        camlwhere += "<Eq><FieldRef Name='RequestID'/><Value Type='Text'>" + timeOffRequestID + "</Value></Eq>";
                        camlwhere += "</Where>";
                        camlQuery.ViewXml = @"<View><Query>" + camlwhere + "</Query></View>";
                        Microsoft.SharePoint.Client.ListItemCollection listItems = selectedList.GetItems(camlQuery);
                        clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                        clientContext.ExecuteQuery();

                        foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                        {
                          
                            oListItem["Status"] = "Pending Approval";//default
                            oListItem["Approver1Status"] = oListItem["Approver1Status"] != null ? oListItem["Approver1Status"] : "";
                            oListItem["Approver2Status"] = oListItem["Approver2Status"] != null ? oListItem["Approver2Status"] : "";
                            oListItem["Approver3Status"] = oListItem["Approver3Status"] != null ? oListItem["Approver3Status"] : "";
                         
                            if (oListItem["Approver1"] != null && oListItem["Approver1Status"].ToString() == "Pending Approval")
                            {
                                var docType = oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue;
                                string app1 = "";
                                if (docType != null)
                                    app1 = docType == null ? "UnAssigned" : docType.LookupValue;
                                if (CurrentUser == null)
                                    LoadUserProfile();
                                if (CurrentUser.Title != app1)
                                {
                                    this.lblerrmsg.Text = " Approver1 : " + lblappr1.Text + " has to process this request prior your actions";
                                    return false;
                                }                                
                                oListItem["Approver1Modified"] = DateTime.Now;
                                oListItem["Approver1Status"] = status;
                                //If more Approver2 or 3 exists,
                                    if (status == "Denied")
                                    {
                                        oListItem["Status"] = status;
                                        returnvalue = true;
                                    }
                                    else
                                    {
                                        if (this.lblappr2.Text != "" || this.lblappr3.Text != "")
                                            oListItem["Status"] = "Pending Approval";
                                        else
                                        {
                                            oListItem["Status"] = status;
                                            if (oListItem["Status"].ToString() == "Approved")
                                            {
                                                try
                                                {                                                 
                                                    DeptCalListClass objDept = new DeptCalListClass();                                                   
                                                    objDept.AddDeptCal(null, this.lblRequestedBy.Text + "-" + this.lblTimeoffType.Text, this.txtStartDate.Text, this.txtEndDate.Text);
                                                    
                                                }
                                                catch { }
                                            }
                                            returnvalue = true;
                                        }
                                    }                                
                            }
                            //Logged user is Approver2
                            // if (CurrentApproverLevel == 2)
                            else
                            {
                                if (oListItem["Approver2"] != null && oListItem["Approver2Status"].ToString() == "Pending Approval")
                                {
                                    //if Approver1 didnt approved.
                                    if (oListItem["Approver1Status"].ToString() != "Approved")
                                    {
                                        this.lblerrmsg.Text = " Approver1 : " + lblappr1.Text + " has to process this request prior your actions";
                                        return false;
                                    }
                                    //validate current user
                                    var docType = oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue;
                                    string app2 = "";
                                    if (docType != null)
                                        app2 = docType == null ? "UnAssigned" : docType.LookupValue;

                                    if (CurrentUser == null)
                                        LoadUserProfile();
                                    if (CurrentUser.Title != app2)
                                    {
                                        this.lblerrmsg.Text = " You do not have the access to process this request.";
                                        return false;
                                    }
                                    {
                                        if (oListItem["Approver1Status"].ToString() == "Approved")
                                        {
                                            oListItem["Approver2Modified"] = DateTime.Now;
                                            oListItem["Approver2Status"] = status;
                                            //if Approver3 Exists
                                            if (status == "Denied")
                                            {
                                                oListItem["Status"] = status;
                                                returnvalue = true;
                                            }
                                            else
                                            {
                                                if (this.lblappr3.Text != "")
                                                    oListItem["Status"] = "Pending Approval";
                                                else
                                                {
                                                    oListItem["Status"] = status;
                                                    if (oListItem["Status"].ToString() == "Approved")
                                                    {
                                                        try
                                                        {
                                                            // string siteToken;
                                                            // if (Session["contextToken"] != null)
                                                            // {
                                                            DeptCalListClass objDept = new DeptCalListClass();
                                                            objDept.AddDeptCal(null, this.lblRequestedBy.Text + "-" + this.lblTimeoffType.Text, this.txtStartDate.Text, this.txtEndDate.Text);
                                                            //  }

                                                        }
                                                        catch { }
                                                    }

                                                    returnvalue = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                //Logged User = Approver3
                                //  if (CurrentApproverLevel == 3)
                                else
                                if (oListItem["Approver3"] != null && oListItem["Approver3Status"].ToString() == "Pending Approval")
                                {

                                    if (oListItem["Approver2Status"].ToString() != "Approved")
                                    {
                                        this.lblerrmsg.Text = " Approver2 : " + lblappr2.Text + " has to process this request prior your actions";
                                        return false;
                                    }
                                    var docType = oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue;
                                    string app3 = "";
                                    if (docType != null)
                                        app3 = docType == null ? "UnAssigned" : docType.LookupValue;
                                    if (CurrentUser == null)
                                        LoadUserProfile();
                                    if (CurrentUser.Title != app3)
                                    {
                                        this.lblerrmsg.Text = " You do not have the access to process this request.";
                                        return false;
                                    }
                                    if (oListItem["Approver1Status"].ToString() == "Approved" && oListItem["Approver2Status"].ToString() == "Approved")
                                    {
                                        oListItem["Approver3Modified"] = DateTime.Now;
                                        oListItem["Approver3Status"] = status;

                                        if (status == "Approved")
                                        {
                                            try
                                            {                                               
                                                DeptCalListClass objDept = new DeptCalListClass();                                              
                                                objDept.AddDeptCal(null, this.lblRequestedBy.Text + "-" + this.lblTimeoffType.Text, this.txtStartDate.Text, this.txtEndDate.Text);
              
                                            }
                                            catch { }
                                        }
                                        oListItem["Status"] = status;
                                        returnvalue = true;
                                    }
                                    else
                                    {
                                        this.lblerrmsg.Text = " Approver1 : " + lblappr1.Text + " Or Approver2: " + lblappr2.Text + " has to process this request prior your actions";
                                        return false;
                                    }
                                }
                            }
                            oListItem.Update();
                        }
                        clientContext.ExecuteQuery();
                    }
                    return returnvalue;
                }
                catch
                {
                    return false;
                }
            }
            return false;   
        }       
    }
}