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
    public partial class RequestForm : PreInitPage
    {
        User CurrentUser;
        Boolean? IsGroupMember;
        List<DateTime> hlist;
        List<DateTime> otherlist;
         Dictionary<string, bool> listTimeoffTypes;

        
        protected void Page_Load(object sender, EventArgs e)
        {
          

            if (!IsPostBack)
            {
                LoadUserProfile();             
                GetApprovers();
                GetTimeOffTypes();
                filltime();
                LoadNotesToolTip();
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


        public void LoadNotesToolTip()
        {
            ConfigListValues objConfigAppList = new ConfigListValues();
            string NotesToolTip="";
            objConfigAppList.GetConfigValues(null);
            if (objConfigAppList.items != null)
            {
                
                if (objConfigAppList.items.ContainsKey("NotesToolTip"))
                    NotesToolTip = objConfigAppList.items["NotesToolTip"].ToString();
            }
            if (NotesToolTip == null || NotesToolTip == "")
            {
                NotesToolTip = Config.NotesToolTip;
            }
          
            this.txtNotes.ToolTip = NotesToolTip;
            this.lblnotesHint.InnerText = NotesToolTip;
        }
        private void GetApprovers()
        {
            if (IsGroupMember == null || CurrentUser == null)
                LoadUserProfile();
            lblCurrentUser.Text = CurrentUser.Title;
            lblToday.Text = DateTime.Today.ToShortDateString();           
            RetrieveApprovers();            
        }

        private void RetrieveApprovers()
        {
            GroupsClass objGrp = new GroupsClass();
             string siteApproverGroupname = Config.TimeOffApprovers;
             ConfigListValues objConfigAppList = new ConfigListValues();
            objConfigAppList.GetConfigValues(null);
            if (objConfigAppList.items != null)
            {
                if (objConfigAppList.items.ContainsKey("TimeOffApprovers"))
                    siteApproverGroupname = objConfigAppList.items["TimeOffApprovers"].ToString();
            }
            UserCollection approvers= objGrp.GetUserList(siteApproverGroupname);
            ddApprover1.Items.Clear();
            ddApprover2.Items.Clear();
            ddApprover3.Items.Clear();
            ddApprover1.Items.Add("- Select -");
            ddApprover2.Items.Add("- Select -");
            ddApprover3.Items.Add("- Select -");
            foreach (User member in approvers)
            {
                ddApprover1.Items.Add(new System.Web.UI.WebControls.ListItem(member.Title, member.LoginName));
                ddApprover2.Items.Add(new System.Web.UI.WebControls.ListItem(member.Title, member.LoginName));
                ddApprover3.Items.Add(new System.Web.UI.WebControls.ListItem(member.Title, member.LoginName));
            }
        }

        

        private void GetTimeOffTypes()
        {
            try
            {
               // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
                var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
                using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
                {
                    try
                    {
                        Web web = clientContext.Web;
                        ListCollection lists = web.Lists;
                        List selectedList = lists.GetByTitle(Config.TimeOffTypes);
                        clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                        clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                        clientContext.ExecuteQuery();
                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml = @"<View><Query><Where><IsNotNull><FieldRef Name='ID' /></IsNotNull></Where></Query><ViewFields><FieldRef Name='ID' /><FieldRef Name='Title' /><FieldRef Name='ApprovalRequired' /></ViewFields></View>";
                        Microsoft.SharePoint.Client.ListItemCollection listItems = selectedList.GetItems(camlQuery);
                        clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                        clientContext.ExecuteQuery();
                        if (listTimeoffTypes == null)
                        {
                            listTimeoffTypes = new Dictionary<string, bool>();
                        }
                        ddTimeoffType.Items.Clear();
                        ddTimeoffType.Items.Add("- Select -");
                        foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                        {
                            //use the "Approval_x0020_Required" field and store it with the VALUE.
                            if (!listTimeoffTypes.ContainsKey(oListItem["Title"].ToString()))
                                listTimeoffTypes.Add(oListItem["Title"].ToString(), (bool)oListItem["ApprovalRequired"]); //Approval_x0020_Required
                            ddTimeoffType.Items.Add(new System.Web.UI.WebControls.ListItem(oListItem["Title"].ToString(), oListItem.Id.ToString()));
                        }

                        ViewState["type"] = listTimeoffTypes;
                    }
                    catch (Exception ex)
                    {
                        Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                        clientContext.ExecuteQuery();
                    }
                }
            }
            catch (Exception)
            {
                ///TODO: Write errors to log
                throw;
            }
        }

        public List<string> GetNoteReqdList()
        {
            List<string> lstTimeOffTypeNotesReqd = new List<string>();
            //TO DO get from ConfigList/ webconfig as default.
            //lstTimeOffTypeNotesReqd.Add("Funeral");
            //lstTimeOffTypeNotesReqd.Add("I will accessible");
            string exceptionTypes =Config.TimeOffTypeExceptionList;
             ConfigListValues objConfigAppList = new ConfigListValues();
            objConfigAppList.GetConfigValues(null);

            if (objConfigAppList.items != null)
            {
                if (objConfigAppList.items.ContainsKey("TimeOffTypeExceptionList"))
                {
                    exceptionTypes = objConfigAppList.items["TimeOffTypeExceptionList"].ToString();
                }
            }
            if (exceptionTypes != null || exceptionTypes != "")
                lstTimeOffTypeNotesReqd = new List<string>(exceptionTypes.Split(','));
            return lstTimeOffTypeNotesReqd;
        }

        public bool IsNotesValidationRequired()
        {
            if (chkAccessible.Checked == true && txtNotes.Text.Trim() == "")
            {
                return true;
            }
            if (txtNotes.Text.Trim() == "")
            {
                List<string> lstTimeOffTypeNotesReqd = new List<string>();
                lstTimeOffTypeNotesReqd = GetNoteReqdList();
                foreach (string exceptionalType in lstTimeOffTypeNotesReqd)
                {
                    if (this.ddTimeoffType.SelectedItem.Text.Equals(exceptionalType,StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
                return false;
            }
            return false;
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblerrmsg.Text = "";
            if (!IsNotesValidationRequired() )
            {
                if (IsApprovedRqd(ddTimeoffType.SelectedItem.Text))
                {
                    if (this.ddApprover1.SelectedValue != "- Select -")
                    {
                        if (this.ddApprover2.SelectedValue == "- Select -" && this.ddApprover2.SelectedValue != "- Select -")
                        {
                            this.lblApp1.Visible = true;
                            lblerrmsg.Text += "*  Please select the Approver2 before selecting Approver3";
                        }
                        else { CreateNewTimeOffRequest(((Button)sender).CommandArgument); }
                    }
                    else
                    {
                        this.lblApp1.Visible = true;
                        lblerrmsg.Text += "*  Please select the Approver(s)";
                    }
                }
                else
                {
                    this.ddApprover1.SelectedValue = "- Select -";
                    this.ddApprover2.SelectedValue = "- Select -";
                    this.ddApprover3.SelectedValue = "- Select -";
                    CreateNewTimeOffRequest(((Button)sender).CommandArgument);
                }
            }
            else
            {
                this.lblApp1.Visible = true;
                lblerrmsg.Text += "*  Please fill more info in the Notes.";
                this.txtNotes.Focus();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ui.aspx" + Request.Url.Query.ToString());
        }

        public string GetDateTime(string date, string time)
        {
            string hr = time.Substring(0, 2).Trim().ToString();
            string min = time.Substring(3, 2).Trim().ToString();
            string ampm = time.Substring(6, 2);
            return date + " " + hr + ":" + min + ":00" + " " + ampm;
        }

        private bool CreateNewTimeOffRequest(string accessToken)
        {
         
            lblerrmsg.Text = "";
            int offHrs = 0;
            string qs;
            string uipage;
            try
            {
                DateTime starttime = Convert.ToDateTime(ddlsrttime.SelectedItem.Text);
                DateTime endtime = Convert.ToDateTime(ddlendtime.SelectedItem.Text);
                TimeSpan diff = DateTime.Now.Subtract(starttime);
                UserClass objUser = new UserClass();         
                if (CurrentUser == null)
                    LoadUserProfile();
                if (diff.Days < 0)
                {
                    lblerrmsg.Text = "* Dates are referenced to past dates, please submit to current or future dates.";
                    return false;
                }
                if (this.rbPartial.Checked)
                {
                    if (this.txtEndDate.Text.Equals(this.txtStartDate.Text))
                    {
                        offHrs = totalhours(starttime, endtime);
                        if (offHrs <= 0)
                        {
                            lblerrmsg.Text = "* Partial day Off duration is 0 or Time difference is not proper. Please verify and Submit again.";
                            return false;
                        }
                        if (offHrs >= Convert.ToInt32(Config.WorkingHours))
                        {
                            lblerrmsg.Text = "* Partial day Off duration is more/equal to One day Working hours. Please verify";
                            return false;
                        }
                    }
                    else
                    {
                        lblerrmsg.Text = "* Start date and End date has to be same for Partial day TimeOff Request";
                        return false;
                    }
                }
              
                string redirecturl;
                var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
                using (var clientContext = spContext.CreateUserClientContextForSPAppWeb())//CreateAppOnlyClientContextForSPAppWeb
                {                   
                    try
                    {
                        Web web = clientContext.Web;                       
                   
                        clientContext.Load(web);
                        clientContext.ExecuteQuery();
                        clientContext.Load(web.CurrentUser);    //Get the current user
                        clientContext.ExecuteQuery();
                        List timeOffRequestList = clientContext.Web.Lists.GetByTitle("TimeOffRequests");                       
                        clientContext.Load(timeOffRequestList.Fields);// resolving Notes and Notes1 problem
                        clientContext.ExecuteQuery();

                        ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                        Microsoft.SharePoint.Client.ListItem newTimeOff = timeOffRequestList.AddItem(itemCreateInfo);
                        newTimeOff["TimeOffType"] = ddTimeoffType.SelectedItem.Text;
                        newTimeOff["RequestedBy"] = web.CurrentUser;
                        newTimeOff["IsFullDay"] = (rbFullDay.Checked ? 1 : 0);
                        newTimeOff["StartDateTime"] = DateTime.Parse(txtStartDate.Text + " 12:00:00 AM");
                        newTimeOff["EndDateTime"] = DateTime.Parse(txtEndDate.Text + " 12:00:00 PM");
                        newTimeOff["ExcludeWeekends"] = (rbExcludeWeekendYes.Checked ? 1 : 0);
                        newTimeOff["ExcludeHolidays"] = (rbExcludeHolidayYes.Checked ? 1 : 0);
                        newTimeOff["ExcludeOtherDays"] = (rbExcludeOtherYes.Checked ? 1 : 0); ;
                        newTimeOff["HasAlternateContact"] = (chkAlternateContact.Checked ? 1 : 0); ;
                        newTimeOff["IsAccessible"] = (chkAccessible.Checked ? 1 : 0); ;
                        newTimeOff["IsPrivate"] = (chkPrivate.Checked ? 1 : 0); ;                      
                         newTimeOff["Notes"] = this.txtNotes.Text;   
                        Guid objguid = Guid.NewGuid();
                        newTimeOff["RequestID"] = objguid.ToString();                      
                        if (CurrentUser == null)
                            LoadUserProfile();
                        newTimeOff["RequestedByEmail"] = CurrentUser.Email;                          
                        if (this.rbPartial.Checked)
                        {
                            newTimeOff["StartDateTime"] = DateTime.Parse(GetDateTime(txtStartDate.Text, this.ddlsrttime.SelectedItem.Text));//.ToString("MM/dd/yyyy hh:mm:tt").ToString(); //DateTime.Parse(txtStartDate.Text).ToString("MM/dd/yyyy hh:mm:tt") + " " + GetTime(this.ddlsrttime.SelectedItem.Text);//TODO
                            newTimeOff["EndDateTime"] = DateTime.Parse(GetDateTime(this.txtEndDate.Text, this.ddlendtime.SelectedItem.Text)); //DateTime.Parse(txtEndDate.Text).ToString("MM/dd/yyyy") + " " + GetTime(this.ddlendtime.SelectedItem.Text);//TODO
                            int hrs = DaysIgnoreWeekendsHoliday(ParseDate(this.txtStartDate.Text), ParseDate(this.txtEndDate.Text), Convert.ToInt32(Config.WorkingHours), accessToken);
                            if (hrs == Convert.ToInt32(Config.WorkingHours))
                                newTimeOff["TotalHours"] = offHrs;
                            else
                                newTimeOff["TotalHours"] = 0;
                        }
                        else
                            newTimeOff["TotalHours"] = DaysIgnoreWeekendsHoliday(ParseDate(this.txtStartDate.Text), ParseDate(this.txtEndDate.Text), Convert.ToInt32(Config.WorkingHours), accessToken);

                        TimeOffRequests objTOR = new TimeOffRequests();
                        if (CurrentUser == null)
                            LoadUserProfile();
                        if (objTOR.IsDuplicateExists(CurrentUser.Id, DateTime.Parse(newTimeOff["StartDateTime"].ToString()), DateTime.Parse(newTimeOff["EndDateTime"].ToString())))
                        {
                            lblerrmsg.Text = "* Another request for same date(s) has been requested. We cant create a new request for same date(s).";
                            return false;
                        }    
                        
                        if ((bool)listTimeoffTypes[ddTimeoffType.SelectedItem.Text]) //Save the Status
                            newTimeOff["Status"] = "Pending Approval";
                        else
                            newTimeOff["Status"] = "Approved";
                        if (ddApprover1.SelectedItem.Value != "- Select -")   //Get approvers
                        {
                            Microsoft.SharePoint.Client.User user = clientContext.Web.SiteUsers.GetByLoginName(ddApprover1.SelectedItem.Value);
                            newTimeOff["Approver1"] = user;
                            newTimeOff["Approver1Status"] = "Pending Approval";
                        }
                        if (ddApprover2.SelectedItem.Value != "- Select -")
                        {
                            Microsoft.SharePoint.Client.User user2 = clientContext.Web.SiteUsers.GetByLoginName(ddApprover2.SelectedItem.Value);
                            newTimeOff["Approver2"] = user2;
                            newTimeOff["Approver2Status"] = "Pending Approval";
                        }
                        if (ddApprover3.SelectedItem.Value != "- Select -")
                        {
                            Microsoft.SharePoint.Client.User user3 = clientContext.Web.SiteUsers.GetByLoginName(ddApprover3.SelectedItem.Value);
                            newTimeOff["Approver3"] = user3;
                            newTimeOff["Approver3Status"] = "Pending Approval";
                        }
                        if (this.ddApprover1.SelectedIndex == 0 && this.ddApprover2.SelectedIndex == 0 && this.ddApprover3.SelectedIndex == 0)
                        {
                            newTimeOff["Status"] = "Approved";
                            string msg = "TimeOff Request of " + this.ddTimeoffType.SelectedItem.Text + ": on " + this.txtStartDate.Text + " to " + this.txtEndDate.Text + " has been Approved";
                            if (this.rbFullDay.Checked)
                            {
                                this.txtStartDate.Text += " 12:00 AM";
                                this.txtEndDate.Text += " 12:00 PM";
                            }
                            else
                            {
                                this.txtStartDate.Text += " " + ParseDDLTime(this.ddlsrttime.SelectedItem.Text);
                                this.txtEndDate.Text += " " + ParseDDLTime(this.ddlendtime.SelectedItem.Text);
                            }                           
                        
                            string reqEmail = "";
                            if (CurrentUser == null)
                                LoadUserProfile();
                            reqEmail = CurrentUser.Email;
                        
                            string[] reqdAtten = { reqEmail };
                            string[] optAtten = { };
                            //TODO DeptCalendarList
                            // newTimeOff["ApproverUrl"] = redirecturl;
                            newTimeOff["RequestedByEmail"] = reqEmail;
                            qs = "&redirect_uri=https://" + Request.Url.Authority;
                            redirecturl = Request.QueryString["SPHostUrl"] + "/_layouts/15/appredirect.aspx?client_id=" + Config.ClientId
                                                 + qs + "/Pages/ApprovalForm.aspx" + Server.UrlEncode(Request.Url.Query.ToString() + "&RequestID=" + objguid.ToString());

                            newTimeOff["ApproverUrl"] = redirecturl;
                            newTimeOff.Update();
                            clientContext.ExecuteQuery();

                            string UserID = Config.SenderEmail;//default from web.config
                            string UserPassword = Config.SenderPassword;
                            string WorkingHours = Config.WorkingHours;
                            //Get UserID, UserPassword App Config (custom)
                            ConfigListValues objConfigAppList = new ConfigListValues();
                            objConfigAppList.GetConfigValues(null);
                            if (objConfigAppList.items != null)
                            {
                                if (objConfigAppList.items.ContainsKey("SenderEmail"))
                                    UserID = objConfigAppList.items["SenderEmail"].ToString();
                                if (objConfigAppList.items.ContainsKey("SenderPassword"))
                                    UserPassword = objConfigAppList.items["SenderPassword"].ToString();
                                if (objConfigAppList.items.ContainsKey("WorkingHours"))
                                    UserPassword = objConfigAppList.items["WorkingHours"].ToString();
                            }
                            EWSClass objEWS = new EWSClass();
                            objEWS.SetupCalendarEvent(msg, this.txtStartDate.Text, this.txtEndDate.Text, reqdAtten, optAtten, UserID, UserPassword, WorkingHours);
                            try
                            {       
                            DeptCalListClass objDept = new DeptCalListClass();                                  
                            objDept.AddDeptCal(null, this.lblCurrentUser.Text + "-" + this.ddTimeoffType.SelectedItem.Text, newTimeOff["StartDateTime"].ToString(), newTimeOff["EndDateTime"].ToString());                                                     
                            } catch {}
                            qs = "&redirect_uri=https://" + Request.Url.Authority;//+ "/Pages/ApprovalForm.aspx" + Server.UrlEncode(Request.Url.Query.ToString() + "&success=1&RequestID="+objguid.ToString());
                             redirecturl = Request.QueryString["SPHostUrl"] + "/_layouts/15/appredirect.aspx?client_id=" + Config.ClientId
                                                 + qs + "/Pages/ApprovalForm.aspx" + Server.UrlEncode(Request.Url.Query.ToString() + "&RequestID=" + objguid.ToString());

                            uipage = Request.QueryString["SPHostUrl"] + "/_layouts/15/appredirect.aspx?client_id=" + Config.ClientId
                                                + qs + "/Pages/Ui.aspx" + Server.UrlEncode(Request.Url.Query.ToString() + "&success=1");
                            Response.Redirect(uipage,false);
                            return true;
                        }
                      
                        qs = "&redirect_uri=https://" + Request.Url.Authority;//+ "/Pages/ApprovalForm.aspx" + Server.UrlEncode(Request.Url.Query.ToString() + "&success=1&RequestID="+objguid.ToString());
                         redirecturl = Request.QueryString["SPHostUrl"] + "/_layouts/15/appredirect.aspx?client_id=" + Config.ClientId
                                             + qs + "/Pages/ApprovalForm.aspx" + Server.UrlEncode(Request.Url.Query.ToString() + "&RequestID=" + objguid.ToString());

                        uipage = Request.QueryString["SPHostUrl"] + "/_layouts/15/appredirect.aspx?client_id=" + Config.ClientId
                                            + qs + "/Pages/Ui.aspx" + Server.UrlEncode(Request.Url.Query.ToString() + "&success=1");

                        newTimeOff["ApproverUrl"] = redirecturl;                        
                        newTimeOff.Update();
                      //  this.HyperLink1.NavigateUrl = redirecturl;
                        clientContext.ExecuteQuery();
                        Response.Redirect(uipage, false);
                    }
                    catch (Exception ex)
                    {
                        Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                        clientContext.ExecuteQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        public string ParseDDLTime(string time)
        {
            string hr = time.Substring(0, 2).Trim().ToString();
            string min = time.Substring(3, 2).Trim().ToString();
            string ampm = time.Substring(6, 2);
            return hr + ":" + min + " " + ampm;
        }

        public int totalhours(DateTime start, DateTime end)
        {
            TimeSpan timeSpan1 = new TimeSpan(start.Hour, start.Minute, start.Second);
            TimeSpan timeSpan2 = new TimeSpan(end.Hour, end.Minute, end.Second);
            int timeDiff = timeSpan2.Subtract(timeSpan1).Hours;
            return timeDiff;

        }
        private static DateTime ParseDate(string s)
        {
            DateTime result;
            if (!DateTime.TryParse(s, out result))
            {
                result = DateTime.ParseExact(s, "MM-dd-yyyyT24:mm:ssK", System.Globalization.CultureInfo.InvariantCulture);
            }
            return result;
        }

        public Dictionary<string, bool> GetTimeOffTypesDetails()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
            {

                Web web = clientContext.Web;
                ListCollection lists = web.Lists;
                List selectedList = lists.GetByTitle(Config.TimeOffTypes);
                clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                clientContext.ExecuteQuery();
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = @"<View><Query><Where><IsNotNull><FieldRef Name='ID' /></IsNotNull></Where></Query><ViewFields><FieldRef Name='ID' /><FieldRef Name='Title' /><FieldRef Name='ApprovalRequired' /></ViewFields></View>";
                Microsoft.SharePoint.Client.ListItemCollection listItems = selectedList.GetItems(camlQuery);
                clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                clientContext.ExecuteQuery();
                if (listTimeoffTypes == null)
                {
                    listTimeoffTypes = new Dictionary<string, bool>();
                }
                foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                {
                    //use the "Approval_x0020_Required" field and store it with the VALUE.
                    if (!listTimeoffTypes.ContainsKey(oListItem["Title"].ToString()))
                        listTimeoffTypes.Add(oListItem["Title"].ToString(), (bool)oListItem["ApprovalRequired"]); //Approval_x0020_Required                          
                }
                ViewState["type"] = listTimeoffTypes;
                return listTimeoffTypes;
            }
        }

        public bool IsApprovedRqd(string selectedTimeoff)
        {
            try
            {              
                listTimeoffTypes = GetTimeOffTypesDetails();               
                if (listTimeoffTypes != null)
                {
                    foreach (var item in listTimeoffTypes)
                    {
                        if (selectedTimeoff == item.Key.ToString())
                        {
                            return (bool)item.Value;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #region WorkingHoursCalculations
        private List<DateTime> GetHolidayList()
        {
            
             if (hlist!=null)
                 return hlist;
             hlist = new List<DateTime>();
             var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
                {
                     try
                    {
                        Web web = clientContext.Web;
                        ListCollection lists = web.Lists;
                        List selectedList = lists.GetByTitle(Config.HolidayList);
                        clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                        clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                        clientContext.ExecuteQuery();
                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml = @"<View><Query><Where><IsNotNull><FieldRef Name='ID' /></IsNotNull></Where></Query></View>";
                        Microsoft.SharePoint.Client.ListItemCollection listItems = selectedList.GetItems(camlQuery);
                        clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                        clientContext.ExecuteQuery();
                        foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                        {
                            DateTime holidayDates = new DateTime();
                            if(oListItem["HolidayDate"] !=null)
                            holidayDates = ParseDate(oListItem["HolidayDate"].ToString());                          
                            hlist.Add(holidayDates);
                        }
                          return hlist;
                   }
                catch(Exception ex)
                    {
                        Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                        clientContext.ExecuteQuery();
                        return hlist;
                    }
                }   
        }

        private List<DateTime> GetOtherdayList()
        {
           
            if (otherlist!=null)
                return otherlist;
            otherlist = new List<DateTime>();
            //try
            //{
               // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), accessToken))
                var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
                using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
                {
                    Web web = clientContext.Web;
                    ListCollection lists = web.Lists;
                    List selectedList = lists.GetByTitle(Config.OtherTimeOffDaysList);
                    clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                    clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                    clientContext.ExecuteQuery();
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = @"<View><Query><Where><IsNotNull><FieldRef Name='ID' /></IsNotNull></Where></Query></View>";
                    Microsoft.SharePoint.Client.ListItemCollection listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                    foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                    {
                        DateTime holidayDates = new DateTime();
                        holidayDates = ParseDate(oListItem["OtherDateHoliday"].ToString());
                        otherlist.Add(holidayDates);
                    }
                }
                return otherlist;
            //}
            //catch (Exception)
            //{
            //    ///TODO: Write errors to log
            //   // throw;
            //}
        }


        public void filltime()
        {
            string a = string.Empty;
            List<Tuple<int, string>> datetime = new List<Tuple<int, string>>();
            List<string> list = new List<string>();
            for (int i = 1; i <= 12; i++)
            {

                datetime.Add(new Tuple<int, string>(i, "AM"));
                if (datetime[i - 1].Item1.ToString().Length == 1)               
                    list.Add(" " + datetime[i - 1].Item1 + ":00" + " " + datetime[i - 1].Item2);               
                else             
                    list.Add(datetime[i - 1].Item1 + ":00" + " " + datetime[i - 1].Item2);               
                if (i == 12)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        datetime.Add(new Tuple<int, string>(j, "PM"));
                        if (datetime[j - 1].Item1.ToString().Length == 1)                       
                            list.Add(" " + datetime[j + 11].Item1 + ":00" + " " + datetime[j + 11].Item2);                     
                        else
                            if (datetime[j - 1].Item1.ToString().Length == 1)                          
                                list.Add(datetime[j + 11].Item1 + ":00" + " " + datetime[j + 11].Item2);                           
                    }
                }
            }
            ddlsrttime.DataSource = list;
            ddlsrttime.DataBind();
            this.ddlendtime.DataSource = list;
            ddlendtime.DataBind();
        }


        private int DaysIgnoreWeekendsHoliday(DateTime dtst, DateTime dtend, int hrsPerDay, string accessToken)
        {
            List<DateTime> dtList = new List<DateTime>();
            TimeSpan difference = dtend - dtst;
            double days = difference.TotalDays + 1;
            for (int i = 0; i < days; i++)
            {
                dtList.Add(dtst.AddDays(i));
            }
            if (this.rbExcludeWeekendYes.Checked)
                dtList = ExcludeWeekend(ref dtList);
            if (this.rbExcludeHolidayYes.Checked)
                dtList = ExcludeHoliday(ref dtList, accessToken);
            if (this.rbExcludeOtherYes.Checked)
                dtList = ExcludeOtherDay(ref dtList, accessToken);
            return dtList.Count * hrsPerDay;
        }

        public List<DateTime> ExcludeWeekend(ref List<DateTime> dtlist)
        {
            List<DateTime> dtTempList = new List<DateTime>();
            for (int i = 0; i < dtlist.Count; i++)
            {
                if (!IsWeekend(dtlist[i]))
                    dtTempList.Add(dtlist[i]);
            }
            return dtTempList;
        }

        public List<DateTime> ExcludeHoliday(ref List<DateTime> dtlist, string accessToken)
        {
            List<DateTime> dtTempList = new List<DateTime>();
            for (int i = 0; i < dtlist.Count; i++)
            {
                if (!IsHoliday(dtlist[i]))
                    dtTempList.Add(dtlist[i]);
            }
            return dtTempList;
        }

        public List<DateTime> ExcludeOtherDay(ref List<DateTime> dtlist, string accessToken)
        {
            List<DateTime> dtTempList = new List<DateTime>();
            for (int i = 0; i < dtlist.Count; i++)
            {
                if (!IsOtherday(dtlist[i]))
                    dtTempList.Add(dtlist[i]);
            }
            return dtTempList;
        }


        public bool IsWeekend(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                return true;
            else
                return false;
        }

        //Is Given date is holiday
        public bool IsHoliday(DateTime date)
        {
            List<DateTime> holidayList = GetHolidayList();
            foreach (DateTime dt in holidayList)
            {
                if (dt.Date.Equals(date.Date))
                    return true;
            }
            return false;
        }

        public bool IsOtherday(DateTime date)
        {
            List<DateTime> Otherdaylist = GetOtherdayList();
            foreach (DateTime dt in Otherdaylist)
            {
                if (dt.Date.Equals(date.Date))
                    return true;
            }
            return false;
        }
        #endregion   

    }          
        
}