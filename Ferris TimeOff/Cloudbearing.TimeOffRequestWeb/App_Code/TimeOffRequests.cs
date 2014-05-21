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
using System.Text;
using Cloudbearing.TimeOffRequestWeb;

namespace Cloudbearing.TimeOffRequestWeb.Pages
{
    public class CamlPara
    {
        public string value { get; set; }
        public string type { get; set; }
    }
    public class YearPoco
    {
        public string RequestedBy { get; set; }
        public int RequestedByID { get; set; }
        public decimal? TotalHours { get; set; }
        public int FullDayCount { get; set; }
        public string Status { get; set; }
        public string TimeOffType { get; set; }
    }
    public class TimeOffRequests
    {
        public string TimeOffType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string isFullDay { get; set; }
        public decimal? TotalHours { get; set; }
        public string RequestID { get; set; }
        public int RequestedByID { get; set; }
        public string RequestedBy { get; set; }
        public bool IsAccessible { get; set; }
        public bool IsPrivate { get; set; }
        public bool Alternate { get; set; }
        public string Approver1 { get; set; }
        public string Approver2 { get; set; }
        public string Approver3 { get; set; }
        public int Approver1Id { get; set; }
        public int Approver2Id { get; set; }
        public int Approver3Id { get; set; }
        public string Approver1Status { get; set; }
        public string Approver2Status { get; set; }
        public string Approver3Status { get; set; }
        public string RequestedByEmail { get; set; }

        public string Notes { get; set; }
        public string RequestedOn { get; set; }
        public string CancelStatus { get; set; }
        public bool ExcludeWeekend { get; set; }
        public bool ExcludeHoliday { get; set; }
        public bool ExcludeOtherDay { get; set; }
        



        public List<TimeOffRequests> GetMyTimeOfRequests( string TimeOffRequests)
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
           // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateUserClientContextForSPAppWeb()) //CreateAppOnlyClientContextForSPAppWeb
            {
                try
                {
                    Web web = clientContext.Web;
                    ListCollection lists = web.Lists;
                    List selectedList = lists.GetByTitle(TimeOffRequests);
                    clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                    clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                    clientContext.ExecuteQuery();
                    clientContext.Load(web.CurrentUser);
                    clientContext.ExecuteQuery();
                    CamlQuery camlQuery = new CamlQuery();
                    StringBuilder camlwhere = new StringBuilder();
                    StringBuilder orderby = new StringBuilder();
                    orderby.Append("<OrderBy><FieldRef Name='StartDateTime' Ascending='FALSE'></FieldRef></OrderBy>");

                    camlwhere.Append("<Where>");
                    camlwhere.Append("<And>");
                    camlwhere.Append("<Eq><FieldRef Name='RequestedBy' LookupId='TRUE' /><Value Type='int'>" + clientContext.Web.CurrentUser.Id + "</Value></Eq>");

                    camlwhere.Append("<Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'><Today OffsetDays='-90' /></Value></Geq>");

                    camlwhere.Append("</And>");
                    camlwhere.Append("</Where>");
                    camlQuery.ViewXml = @"<View><Query>" + orderby.ToString() + camlwhere.ToString() + "</Query></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                }
                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                    return null;
                }

            }

            List<TimeOffRequests> listTimeoffReq = new List<TimeOffRequests>();
            TimeOffRequests objTimeOffRequests;
            foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
            {
                objTimeOffRequests = new TimeOffRequests();
                //todo
                objTimeOffRequests.StartDate = Convert.ToDateTime(oListItem["StartDateTime"].ToString()); ;// oListItem["StartDateTime"] != null ? (DateTime)oListItem["StartDateTime"] : null;
                objTimeOffRequests.EndDate = Convert.ToDateTime(oListItem["EndDateTime"].ToString());// oListItem["EndDateTime"] != null ? Convert.ToDateTime(oListItem["StartDateTime"].ToString()) : null;
                objTimeOffRequests.TimeOffType = oListItem["TimeOffType"] != null ? oListItem["TimeOffType"].ToString() : "";
                objTimeOffRequests.Status = oListItem["Status"] != null ? oListItem["Status"].ToString() : "";
                if (oListItem["Notes"] != null)
                    objTimeOffRequests.Notes = oListItem["Notes"] != null ? oListItem["Notes"].ToString() : "";


                objTimeOffRequests.isFullDay = "Full Day";
                if ((bool)oListItem["IsFullDay"] == false)
                    objTimeOffRequests.isFullDay = "Partial Day";
                objTimeOffRequests.TotalHours = Convert.ToDecimal(oListItem["TotalHours"].ToString());
                objTimeOffRequests.RequestID = (string)oListItem["RequestID"];
                objTimeOffRequests.CancelStatus = (string)oListItem["CancelStatus"];
                //========================================
                listTimeoffReq.Add(objTimeOffRequests);
            }
            return listTimeoffReq;

        }


        public List<string> GetDistinctYear( DateTime? start=null,DateTime? end=null)
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            List<string> listyear = new List<string>();
            //using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
            {
                try{
                Web web = clientContext.Web;
                ListCollection lists = web.Lists;
                List selectedList = lists.GetByTitle("TimeOffRequests");
                clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                clientContext.ExecuteQuery();

                CamlQuery camlQuery = new CamlQuery();
                StringBuilder camlwhere = new StringBuilder();
                camlwhere.Append("<Where>");
                string sDatetime = "", eDatetime = "";

                if (start != null && end != null)
                {
                    sDatetime = start.Value.Year.ToString() + "-" + start.Value.Month.ToString() + "-" + start.Value.Day.ToString() + " " + start.Value.Hour + ":" + start.Value.Minute.ToString() + ":00";
                    eDatetime = end.Value.Year.ToString() + "-" + end.Value.Month.ToString() + "-" + end.Value.Day.ToString() + " " + end.Value.Hour + ":" + end.Value.Minute.ToString() + ":00";
                    camlwhere.Append("<And>");
                    camlwhere.Append("   <And>");
                    camlwhere.Append("      <Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>"+sDatetime+"</Value></Geq>");
                    camlwhere.Append("      <Leq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>"+eDatetime+"</Value></Leq>");
                    camlwhere.Append("  </And>");
                }
                camlwhere.Append("<Neq><FieldRef Name='CancelStatus' /><Value Type='Text'>Cancel</Value></Neq>");
                if (start != null && end != null)
                     camlwhere.Append("</And>");

                camlwhere.Append("</Where>");
                camlwhere.Append("<OrderBy><FieldRef Name='StartDateTime' /></OrderBy>");
                camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                listItems = selectedList.GetItems(camlQuery);
                clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                clientContext.ExecuteQuery();

                DateTime dt;
                foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                {
                    dt = Convert.ToDateTime(oListItem["StartDateTime"].ToString());
                    if (!listyear.Contains(dt.Year.ToString()))
                        listyear.Add(dt.Year.ToString());
                }

            }
            catch (Exception ex)
                    {
                        Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                        clientContext.ExecuteQuery();                       
                    }
            }

            var newList = listyear.OrderByDescending(x => x)
                  .ToList();
            return newList;
        }




        public List<TimeOffRequests> GetRequestDetailsByYear( SharePointContext spContext,string year, Dictionary<string, CamlPara> FilterParameters = null, DateTime? start=null,DateTime? end=null)
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            List<TimeOffRequests> listTimeoffReq = new List<TimeOffRequests>();
            List<string> listyear = new List<string>();
           // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            if(spContext==null)
             spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
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
                    camlwhere.Append("  <And>");
                    camlwhere.Append("     <And>");
                    string sDatetime, eDatetime;
                    if (start != null && end != null)
                    {
                        sDatetime = start.Value.Year.ToString() + "-" + start.Value.Month.ToString() + "-" + start.Value.Day.ToString() + " " + start.Value.Hour + ":" + start.Value.Minute.ToString() + ":00";
                        eDatetime = end.Value.Year.ToString() + "-" + end.Value.Month.ToString() + "-" + end.Value.Day.ToString() + " " + end.Value.Hour + ":" + end.Value.Minute.ToString() + ":00";

                        camlwhere.Append("      <Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + sDatetime + "</Value></Geq>");
                        camlwhere.Append("      <Leq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + eDatetime + "</Value></Leq>");
                    }
                    else
                    {
                        camlwhere.Append("     <Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-01-01 00:00:00</Value></Geq>");
                        camlwhere.Append("     <Leq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-12-31 00:00:00</Value></Leq>");
                    }

                    camlwhere.Append("     </And>");
                    camlwhere.Append("     <Neq><FieldRef Name='CancelStatus' /><Value Type='Text'>Cancel</Value></Neq>");
                    camlwhere.Append(" </And>");
                    camlwhere.Append("</Where>");
                    //if (FilterParameters != null)
                    //{
                    //    foreach(var item in FilterParameters)
                    //    {
                    //    // camlwhere.Append("<And>");
                    //     camlwhere.Append("<Eq>");
                    //     if (item.Value.type == "Text")
                    //           {
                    //               camlwhere.Append("<FieldRef Name ='" + item.Key + "'  />");
                    //               camlwhere.Append("<Value Type ='Text'>");
                    //                camlwhere.Append(item.Value.value);
                    //                camlwhere.Append("</Value>");
                    //           }
                    //     if(item.Value.type == "lookupid")
                    //           {
                    //             camlwhere.Append("<FieldRef Name ='"+item.Key.ToString()+"' LookupId='TRUE' />");
                    //             camlwhere.Append("<Value Type ='Integer'>");
                    //             camlwhere.Append(item.Value.value);
                    //             camlwhere.Append("</Value>");    



                    //           }
                    //    camlwhere.Append("</Eq>");                                
                    //     }

                    //}
                    // camlwhere.Append("</And>");         
                    // camlwhere.Append("</And></Where>");
                    string viewFields = "";// "<ViewFields><FieldRef Name='IsFullDay' /><FieldRef Name='TotalHours' /><FieldRef Name='RequestedBy' /><FieldRef Name='TimeOffType' /><FieldRef Name='Status' /></ViewFields>";
                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query>" + viewFields + "</View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                    TimeOffRequests objTimeOffRequests;
                    foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                    {
                        objTimeOffRequests = new TimeOffRequests();
                        objTimeOffRequests.isFullDay = oListItem["IsFullDay"].ToString() == "True" ? "Full Day" : "Parital day";
                        objTimeOffRequests.TimeOffType = oListItem["TimeOffType"].ToString() == null ? "UnAssigned" : oListItem["TimeOffType"].ToString();
                        objTimeOffRequests.TotalHours = oListItem["TotalHours"].ToString() == null ? 0 : Convert.ToDecimal(oListItem["TotalHours"].ToString());
                        objTimeOffRequests.Status = oListItem["Status"].ToString() == null ? "" : oListItem["Status"].ToString();
                        var arr = oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        objTimeOffRequests.RequestedByID = arr == null ? 0 : arr.LookupId;
                        objTimeOffRequests.RequestedBy = arr == null ? "UnAssigned" : arr.LookupValue.ToString();

                        objTimeOffRequests.StartDate = DateTime.Parse(oListItem["StartDateTime"].ToString());
                        objTimeOffRequests.EndDate = DateTime.Parse(oListItem["EndDateTime"].ToString());
                        objTimeOffRequests.IsAccessible = (bool)oListItem["IsAccessible"];
                        objTimeOffRequests.Alternate = (bool)oListItem["HasAlternateContact"];
                        objTimeOffRequests.Notes = oListItem["Notes"] == null ? "" : oListItem["Notes"].ToString();

                        var docType = oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        if (docType != null)
                            objTimeOffRequests.Approver1 = docType == null ? "UnAssigned" : docType.LookupValue;
                        docType = oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        if (docType != null)
                            objTimeOffRequests.Approver2 = docType == null ? "UnAssigned" : docType.LookupValue;
                        docType = oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        if (docType != null)
                            objTimeOffRequests.Approver3 = docType == null ? "UnAssigned" : docType.LookupValue;


                        listTimeoffReq.Add(objTimeOffRequests);
                    }
                }
                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                  
                }
            }

            return listTimeoffReq;
        }

        public List<YearPoco> GetDetailsByYearEmployee( SharePointContext spContext,string year, DateTime? start=null, DateTime? end=null )
        {
            List<TimeOffRequests> listTimeoffReq = GetRequestDetailsByYear(spContext, year, null, start, end );
            var query = from row in listTimeoffReq.AsEnumerable()
                        group row by new { row.RequestedByID, row.RequestedBy, } into g
                        select new YearPoco
                        {
                            RequestedBy = g.Key.RequestedBy,
                            RequestedByID = g.Key.RequestedByID,
                            TotalHours = g.Sum(p => p.TotalHours)
                        };
            return query.ToList();
        }


        public List<YearPoco> GetDetailsByYearType(SharePointContext spContext, string year, DateTime? start = null, DateTime? end = null)
        {
            List<TimeOffRequests> listTimeoffReq = GetRequestDetailsByYear(spContext, year,null, start, end);
            var query = from row in listTimeoffReq.AsEnumerable()
                        group row by new { row.TimeOffType } into g
                        select new YearPoco
                        {
                            RequestedBy = RequestedBy,
                            Status = Status,
                            TimeOffType = g.Key.TimeOffType,
                            TotalHours = g.Sum(p => p.TotalHours)
                        };
            return query.ToList();
        }


        public List<YearPoco> GetDetailsByYearStatus(SharePointContext spContext, string year, DateTime? start = null, DateTime? end = null)
        {

            List<TimeOffRequests> listTimeoffReq = GetRequestDetailsByYear(spContext, year,null, start, end);
            var query = from row in listTimeoffReq.AsEnumerable()
                        group row by new { row.Status } into g
                        select new YearPoco
                        {
                            RequestedBy = RequestedBy,
                            TimeOffType = TimeOffType,
                            Status = g.Key.Status,
                            TotalHours = g.Sum(p => p.TotalHours),
                            FullDayCount = g.Count(p => p.isFullDay == "Full Day")
                        };
            return query.ToList();
        }


        public List<TimeOffRequests> GetDetails( SharePointContext spContext, string year, string parameterkey, string parametervalue, DateTime? startdate=null, DateTime? enddate=null)
        {
            Dictionary<string, CamlPara> FilterPara = new Dictionary<string, CamlPara>();
            if (parameterkey == "RequestedBy")
                FilterPara.Add(parameterkey, new CamlPara { type = "lookupid", value = parametervalue });
            else
                FilterPara.Add(parameterkey, new CamlPara { type = "Text", value = parametervalue });
            switch (parameterkey)
            {
                case "RequestedBy":
                    {
                        return GetRequestDetailsSummary(spContext,year, FilterPara, startdate, enddate);
                    }
                case "TimeOffType":
                    {
                        return GetRequestDetailsType(spContext,year, FilterPara, startdate, enddate );
                    }
                case "Status":
                    {
                        return GetRequestDetailsStatus(spContext,year, FilterPara, startdate, enddate );
                    }
                default:
                    return GetRequestDetailsSummary(spContext,year, FilterPara, startdate, enddate );
            }


        }

        public List<TimeOffRequests> GetRequestDetailsSummary(SharePointContext spContext,string year, Dictionary<string, CamlPara> FilterParameters = null, DateTime? startdate = null, DateTime? enddate = null)
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            List<TimeOffRequests> listTimeoffReq = new List<TimeOffRequests>();
            List<string> listyear = new List<string>();
         //   using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            if (spContext==null)
             spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
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
                    //camlwhere.Append("<Where><And>");
                    //camlwhere.Append("<And><Geq>");
                    //camlwhere.Append("<FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-01-01 00:00:00</Value>");
                    //camlwhere.Append("</Geq>");
                    //camlwhere.Append("<Leq>");
                    //camlwhere.Append("<FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-12-31 00:00:00</Value>");
                    //camlwhere.Append("</Leq></And>");

                    //camlwhere.Append("<Eq>");
                    //camlwhere.Append("<FieldRef Name='RequestedBy' LookupId='True' />");
                    //camlwhere.Append("<Value Type='Integer'>");
                    //camlwhere.Append(FilterParameters["RequestedBy"].value);
                    //camlwhere.Append("</Value>");
                    //camlwhere.Append("</Eq></And>");

                    //camlwhere.Append("</Where>");
                    camlwhere.Append("<Where>");
                    camlwhere.Append("<And>");
                    camlwhere.Append("     <And>");
                    camlwhere.Append("             <And>");
                    if (startdate != null && enddate != null)
                    {
                        camlwhere.Append("               <Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + startdate.Value.Year.ToString() + "-" + startdate.Value.Month.ToString() + "-" + startdate.Value.Day.ToString() + " " + startdate.Value.Hour.ToString() + ":" + startdate.Value.Minute + ":00</Value></Geq>");
                        camlwhere.Append("               <Leq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + enddate.Value.Year.ToString() + "-" + enddate.Value.Month.ToString() + "-" + enddate.Value.Day.ToString() + " " + enddate.Value.Hour.ToString() + ":" + enddate.Value.Minute + ":00</Value></Leq>");
                    }
                    else
                    {
                        camlwhere.Append("               <Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-01-01 00:00:00</Value></Geq>");
                        camlwhere.Append("               <Leq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-12-31 00:00:00</Value></Leq>");
                    }
                    camlwhere.Append("            </And>");
                    camlwhere.Append("          <Eq><FieldRef Name='RequestedBy' LookupId='True' /><Value Type='Integer'>" + FilterParameters["RequestedBy"].value + "</Value></Eq>");
                    camlwhere.Append("     </And>");
                    camlwhere.Append("<Neq><FieldRef Name='CancelStatus' /><Value Type='Text'>Cancel</Value></Neq>");
                    camlwhere.Append("</And>");
                    camlwhere.Append("</Where>");

                    string viewFields = "";// "<ViewFields><FieldRef Name='IsFullDay' /><FieldRef Name='TotalHours' /><FieldRef Name='RequestedBy' /><FieldRef Name='TimeOffType' /><FieldRef Name='Status' /></ViewFields>";
                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query>" + viewFields + "</View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                    TimeOffRequests objTimeOffRequests;
                    foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                    {
                        objTimeOffRequests = new TimeOffRequests();
                        objTimeOffRequests.isFullDay = oListItem["IsFullDay"].ToString() == "True" ? "Full Day" : "Parital day";
                        objTimeOffRequests.TimeOffType = oListItem["TimeOffType"].ToString() == null ? "UnAssigned" : oListItem["TimeOffType"].ToString();
                        objTimeOffRequests.TotalHours = oListItem["TotalHours"].ToString() == null ? 0 : Convert.ToDecimal(oListItem["TotalHours"].ToString());
                        objTimeOffRequests.Status = oListItem["Status"].ToString() == null ? "" : oListItem["Status"].ToString();
                        objTimeOffRequests.Notes = oListItem["Notes"] == null ? "" : oListItem["Notes"].ToString();
                        var arr = oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        objTimeOffRequests.RequestedByID = arr == null ? 0 : arr.LookupId;
                        objTimeOffRequests.RequestedBy = arr == null ? "UnAssigned" : arr.LookupValue.ToString();

                        objTimeOffRequests.StartDate = DateTime.Parse(oListItem["StartDateTime"].ToString());
                        objTimeOffRequests.EndDate = DateTime.Parse(oListItem["EndDateTime"].ToString());
                        objTimeOffRequests.IsAccessible = (bool)oListItem["IsAccessible"];
                        objTimeOffRequests.Alternate = (bool)oListItem["HasAlternateContact"];

                        var docType = oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        if (docType != null)
                        {
                            objTimeOffRequests.Approver1 = docType == null ? "UnAssigned" : docType.LookupValue;
                            objTimeOffRequests.Approver1Id = docType == null ? 0 : docType.LookupId;
                        }
                        docType = oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        if (docType != null)
                        {
                            objTimeOffRequests.Approver2 = docType == null ? "UnAssigned" : docType.LookupValue;
                            objTimeOffRequests.Approver2Id = docType == null ? 0 : docType.LookupId;
                        }
                        if (docType != null)
                        {
                            objTimeOffRequests.Approver3 = docType == null ? "UnAssigned" : docType.LookupValue;
                            objTimeOffRequests.Approver3Id = docType == null ? 0 : docType.LookupId;
                        }


                        listTimeoffReq.Add(objTimeOffRequests);
                    }
                }

                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();                    
                }
            }
            return listTimeoffReq;
        }


        public List<TimeOffRequests> GetRequestDetailsType( SharePointContext spContext,string year, Dictionary<string, CamlPara> FilterParameters = null, DateTime? startdate = null, DateTime? enddate = null)
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            List<TimeOffRequests> listTimeoffReq = new List<TimeOffRequests>();
            List<string> listyear = new List<string>();
          //  using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            if (spContext == null)
                spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
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
                    //camlwhere.Append("<Where><And>");
                    //camlwhere.Append("<And><Geq>");
                    //camlwhere.Append("<FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-01-01 00:00:00</Value>");
                    //camlwhere.Append("</Geq>");
                    //camlwhere.Append("<Leq>");
                    //camlwhere.Append("<FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-12-31 00:00:00</Value>");
                    //camlwhere.Append("</Leq></And>");

                    //camlwhere.Append("<Eq>");
                    //camlwhere.Append("<FieldRef Name='TimeOffType' />");
                    //camlwhere.Append("<Value Type='Text'>");
                    //camlwhere.Append(FilterParameters["TimeOffType"].value);
                    //camlwhere.Append("</Value>");
                    //camlwhere.Append("</Eq></And>");

                    //camlwhere.Append("</Where>");


                    camlwhere.Append("<Where>");
                    camlwhere.Append("<And>");
                    camlwhere.Append("         <And>");
                    camlwhere.Append("                <And>");

                    if (startdate != null && enddate != null)
                    {
                        camlwhere.Append("               <Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + startdate.Value.Year.ToString() + "-" + startdate.Value.Month.ToString() + "-" + startdate.Value.Day.ToString() + " " + startdate.Value.Hour.ToString() + ":" + startdate.Value.Minute + ":00</Value></Geq>");
                        camlwhere.Append("               <Leq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + enddate.Value.Year.ToString() + "-" + enddate.Value.Month.ToString() + "-" + enddate.Value.Day.ToString() + " " + enddate.Value.Hour.ToString() + ":" + enddate.Value.Minute + ":00</Value></Leq>");
                    }
                    else
                    {
                        camlwhere.Append("               <Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-01-01 00:00:00</Value></Geq>");
                        camlwhere.Append("               <Leq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-12-31 00:00:00</Value></Leq>");
                    }


                    camlwhere.Append("                </And>");
                    camlwhere.Append("                <Eq><FieldRef Name='TimeOffType' /><Value Type='Text'>" + FilterParameters["TimeOffType"].value + "</Value></Eq>");
                    camlwhere.Append("        </And>");
                    camlwhere.Append("          <Neq><FieldRef Name='CancelStatus' /><Value Type='Text'>Cancel</Value></Neq>");
                    camlwhere.Append("</And>");
                    camlwhere.Append("</Where>");



                    string viewFields = "";// "<ViewFields><FieldRef Name='IsFullDay' /><FieldRef Name='TotalHours' /><FieldRef Name='RequestedBy' /><FieldRef Name='TimeOffType' /><FieldRef Name='Status' /></ViewFields>";
                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query>" + viewFields + "</View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                    TimeOffRequests objTimeOffRequests;
                    foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                    {
                        objTimeOffRequests = new TimeOffRequests();
                        objTimeOffRequests.isFullDay = oListItem["IsFullDay"].ToString() == "True" ? "Full Day" : "Parital day";
                        objTimeOffRequests.TimeOffType = oListItem["TimeOffType"].ToString() == null ? "UnAssigned" : oListItem["TimeOffType"].ToString();
                        objTimeOffRequests.TotalHours = oListItem["TotalHours"].ToString() == null ? 0 : Convert.ToDecimal(oListItem["TotalHours"].ToString());
                        objTimeOffRequests.Status = oListItem["Status"].ToString() == null ? "" : oListItem["Status"].ToString();
                        var arr = oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        objTimeOffRequests.RequestedByID = arr == null ? 0 : arr.LookupId;
                        objTimeOffRequests.RequestedBy = arr == null ? "UnAssigned" : arr.LookupValue.ToString();

                        objTimeOffRequests.StartDate = DateTime.Parse(oListItem["StartDateTime"].ToString());
                        objTimeOffRequests.EndDate = DateTime.Parse(oListItem["EndDateTime"].ToString());
                        objTimeOffRequests.IsAccessible = (bool)oListItem["IsAccessible"];
                        objTimeOffRequests.Alternate = (bool)oListItem["HasAlternateContact"];

                        var docType = oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        if (docType != null)
                        {
                            objTimeOffRequests.Approver1 = docType == null ? "UnAssigned" : docType.LookupValue;
                            objTimeOffRequests.Approver1Id = docType == null ? 0 : docType.LookupId;
                        }
                        docType = oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        if (docType != null)
                        {
                            objTimeOffRequests.Approver2 = docType == null ? "UnAssigned" : docType.LookupValue;
                            objTimeOffRequests.Approver2Id = docType == null ? 0 : docType.LookupId;
                        }
                        docType = oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue;
                        if (docType != null)
                        {
                            objTimeOffRequests.Approver3 = docType == null ? "UnAssigned" : docType.LookupValue;
                            objTimeOffRequests.Approver2Id = docType == null ? 0 : docType.LookupId;
                        }


                        listTimeoffReq.Add(objTimeOffRequests);
                    }
                }

                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();

                }
                
            }
            return listTimeoffReq;
        }

        public List<TimeOffRequests> GetRequestDetailsStatus(SharePointContext spContext,string year, Dictionary<string, CamlPara> FilterParameters = null, DateTime? startdate = null, DateTime? enddate = null)
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            List<TimeOffRequests> listTimeoffReq = new List<TimeOffRequests>();
            List<string> listyear = new List<string>();
          //  using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            if (spContext == null)
                spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
            {
                try{
                Web web = clientContext.Web;
                ListCollection lists = web.Lists;
                List selectedList = lists.GetByTitle("TimeOffRequests");
                clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                clientContext.ExecuteQuery();
                CamlQuery camlQuery = new CamlQuery();
                StringBuilder camlwhere = new StringBuilder();
                //camlwhere.Append("<Where><And>");
                //camlwhere.Append("<And><Geq>");
                //camlwhere.Append("<FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-01-01 00:00:00</Value>");
                //camlwhere.Append("</Geq>");
                //camlwhere.Append("<Leq>");
                //camlwhere.Append("<FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-12-31 00:00:00</Value>");
                //camlwhere.Append("</Leq></And>");

                //camlwhere.Append("<Eq>");
                //camlwhere.Append("<FieldRef Name='Status' />");
                //camlwhere.Append("<Value Type='Text'>");
                //camlwhere.Append(FilterParameters["Status"].value);
                //camlwhere.Append("</Value>");
                //camlwhere.Append("</Eq></And>");

                //camlwhere.Append("</Where>");

                camlwhere.Append("<Where>");
                camlwhere.Append("<And>");
                camlwhere.Append("     <And>");
                camlwhere.Append("          <And>");
                if (startdate != null && enddate != null)
                {
                    camlwhere.Append("               <Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + startdate.Value.Year.ToString() + "-" + startdate.Value.Month.ToString() + "-" + startdate.Value.Day.ToString() + " " + startdate.Value.Hour.ToString() + ":" + startdate.Value.Minute + ":00</Value></Geq>");
                    camlwhere.Append("               <Leq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + enddate.Value.Year.ToString() + "-" + enddate.Value.Month.ToString() + "-" + enddate.Value.Day.ToString() + " " + enddate.Value.Hour.ToString() + ":" + enddate.Value.Minute + ":00</Value></Leq>");
                }
                else
                {
                    camlwhere.Append("               <Geq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-01-01 00:00:00</Value></Geq>");
                    camlwhere.Append("               <Leq><FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + year + "-12-31 00:00:00</Value></Leq>");
                }
                camlwhere.Append("         </And>");
                camlwhere.Append("                <Eq> <FieldRef Name='Status' /><Value Type='Text'>"+FilterParameters["Status"].value+"</Value></Eq>");
                camlwhere.Append("      </And>");
                camlwhere.Append("       <Neq><FieldRef Name='CancelStatus' /><Value Type='Text'>Cancel</Value></Neq>");
                camlwhere.Append("</And>");
                camlwhere.Append("</Where>");


                string viewFields = "";// "<ViewFields><FieldRef Name='IsFullDay' /><FieldRef Name='TotalHours' /><FieldRef Name='RequestedBy' /><FieldRef Name='TimeOffType' /><FieldRef Name='Status' /></ViewFields>";
                camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query>" + viewFields + "</View>";
                listItems = selectedList.GetItems(camlQuery);
                clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                clientContext.ExecuteQuery();
                TimeOffRequests objTimeOffRequests;
                foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                {
                    objTimeOffRequests = new TimeOffRequests();
                    objTimeOffRequests.isFullDay = oListItem["IsFullDay"].ToString() == "True" ? "Full Day" : "Parital day";
                    objTimeOffRequests.TimeOffType = oListItem["TimeOffType"].ToString() == null ? "UnAssigned" : oListItem["TimeOffType"].ToString();
                    objTimeOffRequests.TotalHours = oListItem["TotalHours"].ToString() == null ? 0 : Convert.ToDecimal(oListItem["TotalHours"].ToString());
                    objTimeOffRequests.Status = oListItem["Status"].ToString() == null ? "" : oListItem["Status"].ToString();
                    var arr = oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue;
                    objTimeOffRequests.RequestedByID = arr == null ? 0 : arr.LookupId;
                    objTimeOffRequests.RequestedBy = arr == null ? "UnAssigned" : arr.LookupValue.ToString();

                    objTimeOffRequests.StartDate = DateTime.Parse(oListItem["StartDateTime"].ToString());
                    objTimeOffRequests.EndDate = DateTime.Parse(oListItem["EndDateTime"].ToString());
                    objTimeOffRequests.IsAccessible = (bool)oListItem["IsAccessible"];
                    objTimeOffRequests.Alternate = (bool)oListItem["HasAlternateContact"];

                    var docType = oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue;
                    if (docType != null)
                    {
                        objTimeOffRequests.Approver1 = docType == null ? "UnAssigned" : docType.LookupValue;
                        objTimeOffRequests.Approver1Id = docType == null ? 0 : docType.LookupId;
                    }
                    docType = oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue;
                    if (docType != null)
                    {
                        objTimeOffRequests.Approver2 = docType == null ? "UnAssigned" : docType.LookupValue;
                        objTimeOffRequests.Approver2Id = docType == null ? 0 : docType.LookupId;
                    }
                    docType = oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue;
                    if (docType != null)
                    {
                        objTimeOffRequests.Approver3 = docType == null ? "UnAssigned" : docType.LookupValue;
                        objTimeOffRequests.Approver3Id = docType == null ? 0 : docType.LookupId;
                    }


                    listTimeoffReq.Add(objTimeOffRequests);
                }
            }
                catch (Exception ex)
                    {
                        Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                        clientContext.ExecuteQuery();
                        
                    }
            }
            return listTimeoffReq;
        }


        //public bool CancelRequest(string sharepointUrl, string accessToken,string hosturl,string apponlytoken, string status)
        public bool CancelRequest(SharePointContext spContext, string status)
      //  public bool CancelRequest( string status)
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
          // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            if(spContext==null)
              spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
            {

                try
                {
                    //if (this.StartDate > DateTime.Now)
                    //{
                    Web web = clientContext.Web;
                    ListCollection lists = web.Lists;
                    List selectedList = lists.GetByTitle("TimeOffRequests");
                    clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                    clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                    clientContext.ExecuteQuery();

                    CamlQuery camlQuery = new CamlQuery();
                    StringBuilder camlwhere = new StringBuilder();
                    camlwhere.Append("<Where>");
                    camlwhere.Append("<Eq><FieldRef Name='RequestID'/><Value Type='Text'>" + this.RequestID + "</Value></Eq>");
                    camlwhere.Append("</Where>");
                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();

                    string concernApprover = "";

                    foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                    {
                        if (status == "Approved")
                        {
                            if (oListItem["ID"] != null)
                            {
                                Microsoft.SharePoint.Client.ListItem listItem = selectedList.GetItemById(Convert.ToInt32(oListItem["ID"].ToString()));
                                //  var app1 = oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue;
                                //var app2 = oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue;
                                //var app3 = oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue;

                                //var status1 = oListItem["Approver1Status"] as string;
                                //var status2 = oListItem["Approver2Status"] as string;
                                //var status3 = oListItem["Approver3Status"] as string;

                                //if (status1 != "")                                    
                                //    concernApprover = app1.ToString();                                  
                                //if (status2 != "")
                                //    concernApprover = app2.ToString();
                                //if (status3 != "")                                 
                                //    concernApprover = app3.ToString();
                                // concernApprover = app1.ToString();
                                //Send an Cancellation email to approver1
                                //update "CancelStatus"
                                if (oListItem["Approver1"] == null)  // if there is no Approvers
                                {
                                    // listItem.DeleteObject();//implemented in workflow
                                    listItem["CancelStatus"] = "Cancel";
                                    listItem.Update();
                                    // Delete from calendar

                                    try
                                    {

                                        string deptCalName = Config.DepartmentCalendar;//default from web.config
                                        //Get from App Config (custom)
                                        ConfigListValues objConfAppList = new ConfigListValues();
                                        objConfAppList.GetConfigValues(spContext);
                                        if (objConfAppList.items != null)
                                        {
                                            if (objConfAppList.items["DepartmentCalendar"] != null)
                                            {
                                                deptCalName = objConfAppList.items["DepartmentCalendar"].ToString();
                                            }
                                        }


                                        DeptCalendar obj = new DeptCalendar();
                                        string requestorName = oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue == null ? "" : (oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue;
                                        string type = oListItem["TimeOffType"].ToString() == null ? "UnAssigned" : oListItem["TimeOffType"].ToString();
                                        obj.Title = requestorName + "-" + type;
                                        DateTime startDate = (DateTime)oListItem["StartDateTime"];
                                        obj.EventTime = startDate;

                                        // obj.DeleteEvent(hosturl, apponlytoken, deptCalName);
                                        obj.DeleteEvent(deptCalName, spContext);
                                    }
                                    catch
                                    {
                                    }

                                }
                                else
                                {
                                    listItem["CancelStatus"] = "Cancel";
                                    listItem.Update();
                                }
                            }
                        }
                        else
                        {
                            Microsoft.SharePoint.Client.ListItem listItem = selectedList.GetItemById(Convert.ToInt32(oListItem["ID"].ToString()));

                            listItem["CancelStatus"] = "Cancel";
                            listItem.Update();

                            //listItem.DeleteObject(); //implemented in workflow


                        }
                    }

                    clientContext.ExecuteQuery();

                    // }
                }

                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                  
                }

                return true;
            }
        }


        public TimeOffRequests LoadObject(Microsoft.SharePoint.Client.ListItemCollection listItems)
        {
            foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
            {
                this.RequestedBy = (oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue != null ? (oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue : "";
                this.RequestedOn = oListItem["Created"] != null ? oListItem["Created"].ToString() : "";
                this.TimeOffType = (string)oListItem["TimeOffType"];
                this.isFullDay = (bool)oListItem["IsFullDay"] == true ? "Full Day(s)" : "Partial Day";
                this.StartDate = ((DateTime)oListItem["StartDateTime"]);
                this.EndDate = ((DateTime)oListItem["EndDateTime"]);
                this.Status = oListItem["Status"] != null ? oListItem["Status"].ToString() : "";
                this.ExcludeWeekend = (bool)oListItem["ExcludeWeekends"];
                this.ExcludeHoliday = (bool)oListItem["ExcludeHolidays"];
                this.ExcludeOtherDay = (bool)oListItem["ExcludeOtherDays"];
                this.Approver1 = (oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue : "";
                this.Approver2 = (oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue : "";
                this.Approver3 = (oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue : "";

                //objTimeOffRequests.Approver1Id = docType == null ? 0 : docType.LookupId;
                this.Approver1Id = (oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupId : 0;
                this.Approver2Id = (oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupId : 0;
                this.Approver3Id = (oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupId : 0;
                
                
                this.Approver1Status = oListItem["Approver1Status"] != null ? oListItem["Approver1Status"].ToString() : "";
                this.Approver2Status = oListItem["Approver2Status"] != null ? oListItem["Approver2Status"].ToString() : "";
                this.Approver3Status = oListItem["Approver3Status"] != null ? oListItem["Approver3Status"].ToString() : "";
                this.RequestedByEmail = oListItem["RequestedByEmail"] != null ? oListItem["RequestedByEmail"].ToString() : "";
                this.Alternate = (bool)oListItem["HasAlternateContact"];
                this.IsAccessible = (bool)oListItem["IsAccessible"];
                this.IsPrivate = (bool)oListItem["IsPrivate"];
                this.CancelStatus = (string)oListItem["CancelStatus"];
                this.Notes = oListItem["Notes"] != null ? oListItem["Notes"].ToString() : "";
                this.RequestID = oListItem["RequestID"] != null ? oListItem["RequestID"].ToString() : "";
                this.TotalHours = oListItem["TotalHours"] != null ? Convert.ToDecimal(oListItem["TotalHours"].ToString()) : 0;
            }
            return this;
        }


        public TimeOffRequests GetRequestDetailbyRequestID(string sharepointUrl, string accessToken)
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
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
                    //camlwhere.Append("<And>");
                    camlwhere.Append("<Eq><FieldRef Name='RequestID'/><Value Type='Text'>" + this.RequestID + "</Value></Eq>");
                    // camlwhere.Append("<Eq><FieldRef Name='CancelStatus'/><Value Type='Text'>Cancel</Value></Eq>");
                    //camlwhere.Append("</And>");
                    camlwhere.Append("</Where>");
                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                    return LoadObject(listItems);
                }
                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                    return null;
                }
            }
        }

        public TimeOffRequests GetRequestDetailbyRequestIDAndCancelStatus(string sharepointUrl, string accessToken,  string CancelStatus)
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
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
                    camlwhere.Append("<And>");
                    camlwhere.Append("<Eq><FieldRef Name='RequestID'/><Value Type='Text'>" + this.RequestID + "</Value></Eq>");
                    camlwhere.Append("<Eq><FieldRef Name='CancelStatus'/><Value Type='Text'>" + CancelStatus + "</Value></Eq>");
                    camlwhere.Append("</And>");
                    camlwhere.Append("</Where>");
                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                    return LoadObject(listItems);
                }
                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                    return null;
                }
            }
        }


        


        public List<TimeOffRequests> GetMyProcessingRequests( int userid, string userloginname="")
        {
            TimeOffRequests objTOR = new TimeOffRequests();
            Microsoft.SharePoint.Client.ListItemCollection listItems;
           // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())

            {
                Web web = clientContext.Web;
                ListCollection lists = web.Lists;
                List selectedList = lists.GetByTitle("TimeOffRequests");
                clientContext.Load<ListCollection>(lists); // this lists object is loaded successfully
                clientContext.Load<List>(selectedList);  // this list object is loaded successfully
                clientContext.ExecuteQuery();

                CamlQuery camlQuery = new CamlQuery();
                StringBuilder camlwhere = new StringBuilder();
                //camlwhere.Append("<Where>");
                //camlwhere.Append("<And>");
                //camlwhere.Append("<Eq><FieldRef Name='Approver1' LookupId='TRUE' /><Value Type='int'>" + userid + "</Value></Eq>");
                //camlwhere.Append("<Eq><FieldRef Name='Status'  /><Value Type='Text'>Pending Approval</Value></Eq>");
                //camlwhere.Append("</And>");               
                //camlwhere.Append("</Where>");

                camlwhere.Append("<Where>");
                camlwhere.Append("     <And>");
                camlwhere.Append("              <Or>");
                camlwhere.Append("                  <Or>");
                camlwhere.Append("                     <Eq><FieldRef Name='Approver1' LookupId='TRUE' /><Value Type='int'>" + userid + "</Value></Eq>");
                camlwhere.Append("                     <Eq><FieldRef Name='Approver2' LookupId='TRUE' /><Value Type='int'>" + userid + "</Value></Eq>");
                camlwhere.Append("                  </Or>");
                camlwhere.Append("                     <Eq><FieldRef Name='Approver3' LookupId='TRUE' /><Value Type='int'>" + userid + "</Value></Eq>");
                camlwhere.Append("             </Or>");
                camlwhere.Append("                <Eq><FieldRef Name='Status'  /><Value Type='Text'>Pending Approval</Value></Eq>");
                camlwhere.Append("        </And>");               
                camlwhere.Append("</Where>");
                camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                listItems = selectedList.GetItems(camlQuery);
                clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                clientContext.ExecuteQuery();
                List<TimeOffRequests> objTemp= LoadObjectList(listItems);

                List<TimeOffRequests> objTemp2 = new List<TimeOffRequests>();

                if (userloginname != "")
                {
                    //int i = 0;
                    
                    foreach (var reqItem in objTemp)
                    {
                        //if (  (reqItem.Approver1Id != userid && reqItem.Approver1Status != "Pending Approval") ||
                        //      (reqItem.Approver2Id != userid && reqItem.Approver1Status != "Approved") ||
                        //      (reqItem.Approver3Id != userid && reqItem.Approver2Status != "Approved"))
                        //{
                        //    objTemp.RemoveAt(i);
                        //}
                        //i++;
                        if ((reqItem.Approver1Id == userid && reqItem.Approver1Status == "Pending Approval") ||
                              (reqItem.Approver2Id == userid && reqItem.Approver1Status == "Approved" && reqItem.Approver2Status == "Pending Approval") ||
                              (reqItem.Approver3Id == userid && reqItem.Approver2Status == "Approved" && reqItem.Approver3Status == "Pending Approval"))
                        {
                            objTemp2.Add(reqItem);
                        }

                    }
                    return objTemp2;
                }
                else
                    return objTemp;


            }
        }

        public List<TimeOffRequests> GetMyCancelAlerts( int userid)
        {
            TimeOffRequests objTOR = new TimeOffRequests();
            Microsoft.SharePoint.Client.ListItemCollection listItems;
           // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
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
                    camlwhere.Append("<And>");
                    camlwhere.Append("   <And>");
                    camlwhere.Append("     <Eq><FieldRef Name='Approver1' LookupId='TRUE' /><Value Type='int'>" + userid + "</Value></Eq>");
                    camlwhere.Append("     <Eq><FieldRef Name='Status'  /><Value Type='Text'>Approved</Value></Eq>");
                    camlwhere.Append("   </And>");
                    camlwhere.Append("  <Eq><FieldRef Name='CancelStatus'  /><Value Type='Text'>Cancel</Value></Eq>");
                    camlwhere.Append("</And>");
                    camlwhere.Append("</Where>");
                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                    return LoadObjectList(listItems);
                }
                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                    return null;
                }
            }
        }

        public List<TimeOffRequests> LoadObjectList(Microsoft.SharePoint.Client.ListItemCollection listItems)
        {
            List<TimeOffRequests> objTORList=new List<TimeOffRequests>();
            TimeOffRequests objTOR;

            foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
            {
                objTOR = new TimeOffRequests();
                objTOR.RequestedBy = (oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue != null ? (oListItem["RequestedBy"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue : "";
                objTOR.RequestedOn = oListItem["Created"] != null ? oListItem["Created"].ToString() : "";
                objTOR.TimeOffType = (string)oListItem["TimeOffType"];
                objTOR.isFullDay = (bool)oListItem["IsFullDay"] == true ? "Full Day(s)" : "Partial Day";
                objTOR.StartDate = ((DateTime)oListItem["StartDateTime"]);
                objTOR.EndDate = ((DateTime)oListItem["EndDateTime"]);
                objTOR.Status = oListItem["Status"] != null ? oListItem["Status"].ToString() : "";
                objTOR.ExcludeWeekend = (bool)oListItem["ExcludeWeekends"];
                objTOR.ExcludeHoliday = (bool)oListItem["ExcludeHolidays"];
                objTOR.ExcludeOtherDay = (bool)oListItem["ExcludeOtherDays"];
                objTOR.Approver1 = (oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue : "";
                objTOR.Approver2 = (oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue : "";
                objTOR.Approver3 = (oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupValue : "";
                objTOR.Approver1Id = (oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver1"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupId : 0;
                objTOR.Approver2Id = (oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver2"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupId : 0;
                objTOR.Approver3Id = (oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue) != null ? (oListItem["Approver3"] as Microsoft.SharePoint.Client.FieldLookupValue).LookupId : 0;
                objTOR.Approver1Status = oListItem["Approver1Status"] != null ? oListItem["Approver1Status"].ToString() : "";
                objTOR.Approver2Status = oListItem["Approver2Status"] != null ? oListItem["Approver2Status"].ToString() : "";
                objTOR.Approver3Status = oListItem["Approver3Status"] != null ? oListItem["Approver3Status"].ToString() : "";
                objTOR.RequestedByEmail = oListItem["RequestedByEmail"] != null ? oListItem["RequestedByEmail"].ToString() : "";
                objTOR.Alternate = (bool)oListItem["HasAlternateContact"];
                objTOR.IsAccessible = (bool)oListItem["IsAccessible"];
                objTOR.IsPrivate = (bool)oListItem["IsPrivate"];
                objTOR.CancelStatus = (string)oListItem["CancelStatus"];
                objTOR.Notes = oListItem["Notes"] != null ? oListItem["Notes"].ToString() : "";
                objTOR.RequestID = oListItem["RequestID"] != null ? oListItem["RequestID"].ToString() : "";
                objTOR.TotalHours = oListItem["TotalHours"] != null ? Convert.ToDecimal(oListItem["TotalHours"].ToString()) : 0;
                objTORList.Add(objTOR);
            }
            return objTORList;
        }



        public bool DeleteRequest(SharePointContext spContext, string forcedelete = "0")
        {
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            // using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))
            if (spContext == null)
                spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
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
                    camlwhere.Append("<Eq><FieldRef Name='RequestID'/><Value Type='Text'>" + this.RequestID + "</Value></Eq>");
                    camlwhere.Append("</Where>");
                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();

                    foreach (Microsoft.SharePoint.Client.ListItem oListItem in listItems)
                    {
                        if (oListItem["ID"] != null)
                        {
                            Microsoft.SharePoint.Client.ListItem listItem = selectedList.GetItemById(Convert.ToInt32(oListItem["ID"].ToString()));

                            if (forcedelete == "0")
                            {
                                listItem["CancelStatus"] = "Cancel";
                                listItem.Update();
                            }
                            else
                                listItem.DeleteObject();//implemented in workflow
                        }
                    }
                    clientContext.ExecuteQuery();
                    return true;
                }

                catch (Exception ex)
                {
                    Microsoft.SharePoint.Client.Utilities.Utility.LogCustomRemoteAppError(clientContext, Global.ProductId, ex.Message);
                    clientContext.ExecuteQuery();
                    return false;
                }

            }
        }


        public bool IsDuplicateExists(int userid, DateTime startdate, DateTime endDate)
        {
            TimeOffRequests objTOR = new TimeOffRequests();
            Microsoft.SharePoint.Client.ListItemCollection listItems;
            //using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl, accessToken))

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
                    camlwhere.Append("<And>");
                    camlwhere.Append("<And>");
                    camlwhere.Append("<Geq>");
                    camlwhere.Append("<FieldRef Name='StartDateTime' /><Value Type='DateTime'>" + startdate.Year + "-" + startdate.Month + "-" + startdate.Day + " 00:00:00</Value>");
                    camlwhere.Append("</Geq>");
                    camlwhere.Append("<Leq>");
                    camlwhere.Append("<FieldRef Name='EndDateTime' /><Value Type='DateTime'>" + endDate.Year + "-" + endDate.Month + "-" + endDate.Day + " 00:00:00</Value>");
                    camlwhere.Append("</Leq>");
                    camlwhere.Append("</And>");
                    camlwhere.Append("<Eq><FieldRef Name='RequestedBy' LookupId='TRUE' /><Value Type='int'>" + userid + "</Value></Eq>");
                    camlwhere.Append("</And>");
                    camlwhere.Append("</Where>");


                    camlQuery.ViewXml = @"<View><Query>" + camlwhere.ToString() + "</Query></View>";
                    listItems = selectedList.GetItems(camlQuery);
                    clientContext.Load<Microsoft.SharePoint.Client.ListItemCollection>(listItems);
                    clientContext.ExecuteQuery();
                    if (listItems.Count > 0)
                        return true;
                    else
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

    }
}