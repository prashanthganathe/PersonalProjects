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
    public partial class StatusSummary : System.Web.UI.Page
    {

        User CurrentUser;
        Boolean? IsGroupMember;
        protected void Page_Load(object sender, EventArgs e)
        {          
            divgridSection.Visible = true;
            Session["CurrentSPContext"] = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);             
            if (VerifyApprover())
            {
                
                    createAccordianUsingRepeater();              
                    if (!IsPostBack)
                    {
                        filltime();                     
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

        public void filltime()
        {
            string a = string.Empty;
            List<Tuple<int, string>> datetime = new List<Tuple<int, string>>();
            List<string> list = new List<string>();
            list.Add(" ");
            for (int i = 1; i <= 12; i++)
            {
                datetime.Add(new Tuple<int, string>(i, "AM"));
                if (datetime[i - 1].Item1.ToString().Length == 1)
                {
                    list.Add(" " + datetime[i - 1].Item1 + ":00" + " " + datetime[i - 1].Item2);
                }
                else
                {
                    list.Add(datetime[i - 1].Item1 + ":00" + " " + datetime[i - 1].Item2);
                }
                if (i == 12)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        datetime.Add(new Tuple<int, string>(j, "PM"));
                        if (datetime[j - 1].Item1.ToString().Length == 1)
                        {
                            list.Add(" " + datetime[j + 11].Item1 + ":00" + " " + datetime[j + 11].Item2);
                        }
                        else
                            if (datetime[j - 1].Item1.ToString().Length == 1)
                            {
                                list.Add(datetime[j + 11].Item1 + ":00" + " " + datetime[j + 11].Item2);
                            }
                    }
                }
            }
            ddlsrttime.DataSource = list;
            ddlsrttime.DataBind();
            this.ddlendtime.DataSource = list;
            ddlendtime.DataBind();
        }
        public bool VerifyApprover()
        {
            if (IsGroupMember == null || CurrentUser == null)
                LoadUserProfile();
            if (IsGroupMember == true)
                return true;
            else
                return false;
        }

        public string GetDateTime(string date, string time)
        {
            string hr = time.Substring(0, 2).Trim().ToString();
            string min = time.Substring(3, 2).Trim().ToString();
            string ampm = time.Substring(6, 2);
            return date + " " + hr + ":" + min + ":00" + " " + ampm;
        }

        public void createAccordianUsingRepeater()
        {
            TimeOffRequests obj = new TimeOffRequests();
            DateTime? startDate = null, endDate = null;
            try
            {
                if (this.txtStartDate.Text.Trim() != "" && this.txtEndDate.Text.Trim() != "")
                {
                    if (this.ddlsrttime.SelectedIndex != 0)
                    {
                        startDate = DateTime.Parse(GetDateTime(txtStartDate.Text, this.ddlsrttime.SelectedItem.Text));
                        endDate = DateTime.Parse(GetDateTime(this.txtEndDate.Text, this.ddlendtime.SelectedItem.Text));
                    }
                    else
                    {
                        startDate = DateTime.Parse(txtStartDate.Text + " 12:00:00 AM");
                        endDate = DateTime.Parse(this.txtEndDate.Text + " 12:00:00 PM");
                    }
                }
            }
            catch { }


            List<string> list = obj.GetDistinctYear(startDate, endDate);
            if (list.Count > 0)
            {
                repAccordian.DataSource = from c in list select new { YEAR = c };
                repAccordian.DataBind();
                this.faqscontainer.Visible = true;
                blankdiv.Visible = false;
            }
            else
            {
                this.faqscontainer.Visible = false;
                blankdiv.Visible = true;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ui.aspx?SPHostUrl=" + Request.QueryString["SPHostUrl"].ToString() + "&" + Config.ListURL + "=" + Request.QueryString["SPAppWebUrl"].ToString());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            createAccordianUsingRepeater();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<YearPoco> GetDetailsByYearStatus(string year, string startdate, string enddate, string starttime, string endtime)
        {
            if (HttpContext.Current.Session["CurrentSPContext"] != null)
            {               
                TimeOffRequests obj = new TimeOffRequests();
                if (startdate.Trim() == "")
                    startdate = "01/01/" + year;
                if (enddate.Trim() == "")
                    enddate = "12/31/" + year;
                DateTime? startDate;
                startDate = DateTime.Parse(GetDateTimeStatic(startdate, starttime));//DateTime.Parse(startdate.Trim());
                DateTime? endDate;
                endDate = DateTime.Parse(GetDateTimeStatic(enddate, endtime));
                return obj.GetDetailsByYearStatus(HttpContext.Current.Session["CurrentSPContext"] as SharePointContext, year, startDate, endDate);

            }
            return new List<YearPoco>();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<TimeOffRequests> GetDetails(string year, string parameterkey, string parametervalue, string startdate, string enddate, string starttime, string endtime)
        {
            if (HttpContext.Current.Session["CurrentSPContext"] != null)
            {              
                TimeOffRequests obj = new TimeOffRequests();
                if (startdate.Trim() == "")
                    startdate = "01/01/" + year;
                if (enddate.Trim() == "")
                    enddate = "12/31/" + year;
                DateTime? startDate;
                startDate = DateTime.Parse(GetDateTimeStatic(startdate, starttime));//DateTime.Parse(startdate.Trim());
                DateTime? endDate;
                endDate = DateTime.Parse(GetDateTimeStatic(enddate, endtime));
                return obj.GetDetails(HttpContext.Current.Session["CurrentSPContext"] as SharePointContext, year, parameterkey, parametervalue, startDate, endDate);
            }
            return new List<TimeOffRequests>();
        }

        public static string GetDateTimeStatic(string date, string time)
        {
            if (time.Trim() == "")
            {
                return date + " " + "00:00:00" + " " + "AM";
            }
            string hr = time.Substring(0, 2).Trim().ToString();
            string min = time.Substring(3, 2).Trim().ToString();
            string ampm = time.Substring(6, 2);
            return date + " " + hr + ":" + min + ":00" + " " + ampm;
        }
    }
}