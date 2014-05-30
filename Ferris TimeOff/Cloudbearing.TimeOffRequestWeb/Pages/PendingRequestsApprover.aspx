<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PendingRequestsApprover.aspx.cs" Inherits="Cloudbearing.TimeOffRequestWeb.Pages.PendingRequestsApprover" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pending Time Off Requests</title>
      <meta name="viewport" content="width=device-width, user-scalable=yes" />
    <link href="../Style/Lite.css" rel="stylesheet" />   
    <style type="text/css">
        .cancel {
             background-color:rgba(21, 161, 226, 0.88);
             color: #fff;
        }
    </style>
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
   <script type="text/javascript">

       $(document).ready(function () {
           function removeParameter(url, parameter)
           {
               var fragment = url.split('#');
               var urlparts= fragment[0].split('?');
               if (urlparts.length>=2)
               {
                   var urlBase=urlparts.shift(); //get first part, and remove from array
                   var queryString=urlparts.join("?"); //join it back up
                   var prefix = encodeURIComponent(parameter)+'=';
                   var pars = queryString.split(/[&;]/g);
                   for (var i= pars.length; i-->0;) {               //reverse iteration as may be destructive
                       if (pars[i].lastIndexOf(prefix, 0)!==-1) {   //idiom for string.startsWith
                           pars.splice(i, 1);
                       }
                   }
                   url = urlBase+'?'+pars.join('&');
                   if (fragment[1]) {
                       url += "#" + fragment[1];
                   }
               }
               return url;
           }

           $(document).on('click', '.gotoApprover', function (e) {
               var requestid = $(this).parent().parent()[0].id;          
               var newQS = "&RequestID=" + requestid;
               var approvalPage = window.location.href.replace('PendingRequestsApprover.aspx', 'ApprovalForm.aspx');
               approvalPage = removeParameter(approvalPage, "RequestID");            
               window.location = approvalPage + newQS;
           });

           $(document).on('click', '.ApproverMe', function (e) {
               //var requestid = $(this).parent().parent()[0].id;
               //var newQS = "&RequestID=" + requestid;
               //var approvalPage = window.location.href.replace('PendingRequestsApprover.aspx', 'ApprovalForm.aspx');
               //approvalPage = removeParameter(approvalPage, "RequestID");
               //window.location = approvalPage + newQS;
               var requestid = $(this).parent().parent()[0].id;

               $.ajax({


                   url: "PendingRequestsApprover.aspx/ApproveRequest",
                   data: JSON.stringify({ 'requestid': requestid}),
                   type: "POST",
                   contentType: "application/json;charset=utf-8",
                   dataType: "json",
                   cache: false,
                   success: function (result) {
                       //TODO cancel the request and refresh the page             
                       alert(result.d);
                       location.reload(true);
                   },
                   error: function (request, status, error) {
                   }
               });
           });
           


           function DeleteRequest(requestid, currentitemStartDate, currentitemEndDate, currentitemrequestedby, currentitemtype, status, e) {
               $.ajax({
                   url: "PendingRequestsApprover.aspx/DeleteRequest",
                   data: JSON.stringify({ 'requestid': requestid, 'startdate': currentitemStartDate, 'enddate': currentitemEndDate, 'requestorname': currentitemrequestedby, 'type': currentitemtype, 'status': status }),
                   type: "POST",
                   contentType: "application/json;charset=utf-8",
                   dataType: "json",
                   cache: false,
                   success: function (result) {
                       //TODO cancel the request and refresh the page             
                       alert(result.d);
                       location.reload(true);
                   },
                   error: function (request, status, error) {
                   }
               });
           }

           $(document).on('click', '.AppCancelTimeOff', function (e) {
               var temp = $(this).parent().parent().parent();
               var requestid = $(this).parent().parent()[0].id;
               var currentitemStartDate = temp.find('.startDateTableData').html();
               var currentitemEndDate = temp.parent().find('.endDateTableData').html();
               var currentitemStatus = temp.find('.statusTableData').html();
               var currentitemrequestedby = temp.find('.requestedby').html();
               var currentitemtype = temp.find('.type').html();
               
               DeleteRequest(requestid, currentitemStartDate, currentitemEndDate, currentitemrequestedby, currentitemtype,currentitemStatus, e);
           });
           
       });
   </script>




</head>
<body style="display:none; overflow:scroll;">
    <form id="form1" runat="server">
        <div id="chrome_ctrl_placeholder"></div>
         <div id="s4-bodyContainer">
    <div id="contentRow">
        <div id="sideNavBox" class="ms-dialogHidden ms-forceWrap ms-noList">
          <div id="DeltaPlaceHolderLeftNavBar" class="ms-core-navigation" role="navigation">
        </div></div>


        <div id="contentBox" aria-live="polite" aria-relevant="all">
            <div class="page">
		        <div class="page-region">
			        <div class="page-region-content">			
                              <div class="divSingleline " style="width:100%;">
                                    <div class="divSingleline" style="width:90%;">
                                         <div class="divSingleline" style="width:10%;">
                                             <a onclick="Redirect('ui.aspx')" class="back-button big page-back"></a></div>  
                                        <h2>My Alerts</h2> </div>
                                </div>
                              <div style="text-align:center;color:red;"><asp:Label ID="lblerrmsg" runat="server" CssClass="errmsg validation"></asp:Label></div>
                    </div>
                </div>
             </div>
     <!-- Controls -->
    <div class="page secondary">
        <div class="page-region">
            <div class="page-region-content">
                <div class="grid">
                    <div class="row">
                        <div class="span8">


                             <table  runat="server"  class="striped">
	                            <tbody>
                                    <tr id="Tr3" class="TableHeader">
                                         <td id="Td15" style=" text-align:left; color:gray; " ><b>Time Off Requests waiting for your Approval</b></td>
                                        </tr>
                                    </tbody>
                                 </table>

                             <div id="divNormal" runat="server" class="dynamicControl">                                
                               <%-- <table id="requestedAbsences_table" class="striped"><thead><tr><th style="">Type of leave</th><th class="right" style="width:180px">Employee</th><th class="right" style="width:112px">Workdays</th><th class="right" style="width:110px">From</th><th class="right" style="width:110px">To</th><th class="right" style="width:180px">Status</th><th style="width:170px"></th></tr></thead><tbody><tr class=""><td>Vacation</td><td class="right"><a class="" href="../_layouts/15/userdisp.aspx?ID=19">Ken Berlack</a></td><td class="right">5</td><td class="right">4/3/2014</td><td class="right">4/11/2014</td><td class="right">Pending</td><td class="right"><button type="button" onclick="ShowAbsenceToAccept(1);return false;" class="command-button bg-color-red fg-color-white wide-white"><h3 class="fg-color-white">View</h3><span class="icon-arrow-right-3"></span></button></td></tr><tr class=""><td>Vacation</td><td class="right"><a class="" href="../_layouts/15/userdisp.aspx?ID=19">Ken Berlack</a></td><td class="right">2</td><td class="right">4/15/2014</td><td class="right">4/16/2014</td><td class="right">Pending</td><td class="right"><button type="button" onclick="ShowAbsenceToAccept(2);return false;" class="command-button bg-color-red fg-color-white wide-white">
                                    <h3 class="fg-color-white">View</h3><span class="icon-arrow-right-3"></span></button></td></tr></tbody></table>--%>
                                <table id="dummyTable2" runat="server" visible="false"  class="striped">
	                            <tbody><tr id="Tr2" class="TableHeader">
                                     <td id="Td14" ><b>Requestor</b></td>
		                            <td id="Td8" ><b>Time Off Type</b></td>
		                            <td id="Td9">Time Off Day</td>
		                            <td id="Td10">Start Date</td>
		                            <td id="Td11">End Date</td>
		                            <td id="Td12">Total Hours</td>
		                            <td id="Td13">Status</td>
	                            </tr>		  
                                    <tr class="TableData">  
                                        <td colspan="7"><span id="Span1">You don't have any Requests to process.</span> </td>                                         
                                    </tr>                  
                                </tbody></table>


                                 <asp:ListView ID="lvNormal" runat="server"     >  
                                    <LayoutTemplate>  
                                        <div class="grid">
                                            <div class="row">
                                                <div class="span8">
                                                    <div id="requestedAbsences" class="dynamicControl">
                                        <table id="Table1" runat="server" class="striped">  
                                            <tr id="Tr1" runat="server" class="TableHeader" >  
                                                  <td id="Td14" ><b>Requestor</b></td>
                                                <td id="Td1" runat="server"><b>Time Off Type</b></td>  
                                                <td id="Td6" runat="server">Time Off Day</td>  
                                                <td id="Td2" runat="server">Start Date</td>  
                                                <td id="Td3" runat="server">End Date</td>  
                                                <td id="Td7" runat="server">Total Hours</td>  
                                                <td id="Td4" runat="server">Status</td> 
                                               <td id="Td5" ></td>   
                                               <%-- <td id="Td5" ></td>--%>   
                                            </tr>  
                                            <tr id="ItemPlaceholder" runat="server">  
                                            </tr>                   
                                     </table>  
                               
                                </LayoutTemplate>  
                                <ItemTemplate>  
                                    <tr class="TableData">  
                                         <td><asp:Label  ID="Label7"  runat="server" CssClass="requestedby"  Text='<%# Eval("RequestedBy")%>'    >  </asp:Label> </td>  
                                        <td><asp:Label  ID="Label1"  runat="server"   Text='<%# Eval("TimeOffType")%>'  CssClass="type"  >  </asp:Label> </td>  
                                          <td> <asp:Label ID="Label5"   runat="server" Text='<%# Eval("isFullDay")%>' >  </asp:Label>  </td>  
                                     <%--   <td> <asp:Label ID="Label2"   runat="server" Text='<%# Eval("StartDate")%>' >  </asp:Label>  </td>  --%>
                                         <td> <asp:Label ID="Label2"  CssClass="startDateTableData"  runat="server" Text='<%# Eval("isFullDay").ToString() == "Full Day" ? Eval("StartDate", "{0:d}") : Eval("StartDate") %>' >  </asp:Label>  </td>  
<%--                                         <td> <asp:Label ID="Label3"   runat="server" Text='<%# Eval("EndDate")%>' >  </asp:Label>  </td> --%> 
                                        <td> <asp:Label ID="Label3"  CssClass="endDateTableData"  runat="server" Text='<%# Eval("isFullDay").ToString() == "Full Day" ? Eval("EndDate", "{0:d}") : Eval("EndDate") %>' >  </asp:Label>  </td>  
                                         <td> <asp:Label ID="Label6"   runat="server" Text='<%# Eval("TotalHours")%>' >  </asp:Label>  </td>  
                                         <td> <asp:Label ID="Label4" CssClass="statusTableData"   runat="server" Text='<%# Eval("Status")%>' >  </asp:Label>  </td>  

                                       <%--  <td id="<%# Eval("RequestID").ToString()%>"> <div id='lblbutton'    runat="server"> <%# Eval("Status").ToString() == "Pending Approval" ? "<input type='button' value='Cancel' class='newAbsencesButton cancel' style=' background-color:rgba(21, 161, 226, 0.88); color:#fff;'>" : "" %>  </div>  </td>--%>
                                          <td id='<%# Eval("RequestID").ToString()%>'> <div id='lblbutton'    runat="server"> <input type='button' value='More details...' class='newAbsencesButton gotoApprover' style=' background-color:rgba(21, 161, 226, 0.88); color:#fff;'> </div>  </td>
                                       <%-- <td id='<%# Eval("RequestID").ToString()%>'> <div id='lblapprove'    runat="server"> <input type='button' value='Approve' class='newAbsencesButton ApproverMe' style=' background-color:rgba(21, 161, 226, 0.88); color:#fff;'> </div>  </td>--%>
                                    </tr>                  
                                </ItemTemplate>  
                        </asp:ListView>  

                            </div>

                            <br />
                            <hr />

                            <table id="Table2"  runat="server"   class="striped">
	                            <tbody>
                                    <tr id="Tr4" class="TableHeader">
                                         <td id="Td16" style=" text-align:left; color:orange; " ><b>Cancelled Requests waiting for your decision</b></td>
                                        </tr>
                                    </tbody>
                                 </table>

                            <div id="requestedAbsences" runat="server" class="dynamicControl">                                
                               <%-- <table id="requestedAbsences_table" class="striped"><thead><tr><th style="">Type of leave</th><th class="right" style="width:180px">Employee</th><th class="right" style="width:112px">Workdays</th><th class="right" style="width:110px">From</th><th class="right" style="width:110px">To</th><th class="right" style="width:180px">Status</th><th style="width:170px"></th></tr></thead><tbody><tr class=""><td>Vacation</td><td class="right"><a class="" href="../_layouts/15/userdisp.aspx?ID=19">Ken Berlack</a></td><td class="right">5</td><td class="right">4/3/2014</td><td class="right">4/11/2014</td><td class="right">Pending</td><td class="right"><button type="button" onclick="ShowAbsenceToAccept(1);return false;" class="command-button bg-color-red fg-color-white wide-white"><h3 class="fg-color-white">View</h3><span class="icon-arrow-right-3"></span></button></td></tr><tr class=""><td>Vacation</td><td class="right"><a class="" href="../_layouts/15/userdisp.aspx?ID=19">Ken Berlack</a></td><td class="right">2</td><td class="right">4/15/2014</td><td class="right">4/16/2014</td><td class="right">Pending</td><td class="right"><button type="button" onclick="ShowAbsenceToAccept(2);return false;" class="command-button bg-color-red fg-color-white wide-white">
                                    <h3 class="fg-color-white">View</h3><span class="icon-arrow-right-3"></span></button></td></tr></tbody></table>--%>
                                <table id="dummytable" runat="server" visible="false"  class="striped">
	                            <tbody><tr id="LVreqDetails_Tr1" class="TableHeader">
                                      <td id="Td17" ><b>Requestor</b></td>
		                            <td id="LVreqDetails_Td1" ><b>Time Off Type</b></td>
		                            <td id="LVreqDetails_Td6">Time Off Day</td>
		                            <td id="LVreqDetails_Td2">Start Date</td>
		                            <td id="LVreqDetails_Td3">End Date</td>
		                            <td id="LVreqDetails_Td7">Total Hours</td>
		                            <td id="LVreqDetails_Td4">Status</td>
	                            </tr>		  
                                    <tr class="TableData">  
                                        <td colspan="7"><span id="LVreqDetails_Label1_0">You don't have any Requests submitted for cancellations.</span> </td>                                         
                                    </tr>                  
                                </tbody></table>


                                 <asp:ListView ID="LVreqDetails" runat="server"     >  
                                    <LayoutTemplate>  
                                        <div class="grid">
                                            <div class="row">
                                                <div class="span8">
                                                    <div id="requestedAbsences" class="dynamicControl">
                                        <table id="Table1" runat="server" class="striped">  
                                            <tr id="Tr1" runat="server" class="TableHeader" >  
                                                <td id="Td18" runat="server"><b>Requestor</b></td>  
                                                <td id="Td1" runat="server"><b>Time Off Type</b></td>  
                                                <td id="Td6" runat="server">Time Off Day</td>  
                                                <td id="Td2" runat="server">Start Date</td>  
                                                <td id="Td3" runat="server">End Date</td>  
                                                <td id="Td7" runat="server">Total Hours</td>  
                                                <td id="Td4" runat="server">Status</td> 
                                               <td id="Td5" ></td>   
                                            </tr>  
                                            <tr id="ItemPlaceholder" runat="server">  
                                            </tr>                   
                                     </table>  
                               
                                </LayoutTemplate>  
                                <ItemTemplate>  
                                    <tr class="TableData">  
                                          <td><asp:Label  ID="Label7"  runat="server" CssClass="requestedby"  Text='<%# Eval("RequestedBy")%>'    >  </asp:Label> </td>  
                                        <td><asp:Label  ID="Label1"  runat="server"   Text='<%# Eval("TimeOffType")%>'  CssClass="type"  >  </asp:Label> </td>  
                                          <td> <asp:Label ID="Label5"   runat="server" Text='<%# Eval("isFullDay")%>' >  </asp:Label>  </td>  
                                     <%--   <td> <asp:Label ID="Label2"   runat="server" Text='<%# Eval("StartDate")%>' >  </asp:Label>  </td>  --%>
                                         <td> <asp:Label ID="Label2"  CssClass="startDateTableData"  runat="server" Text='<%# Eval("isFullDay").ToString() == "Full Day" ? Eval("StartDate", "{0:d}") : Eval("StartDate") %>' >  </asp:Label>  </td>  
<%--                                         <td> <asp:Label ID="Label3"   runat="server" Text='<%# Eval("EndDate")%>' >  </asp:Label>  </td> --%> 
                                        <td> <asp:Label ID="Label3"  CssClass="endDateTableData"  runat="server" Text='<%# Eval("isFullDay").ToString() == "Full Day" ? Eval("EndDate", "{0:d}") : Eval("EndDate") %>' >  </asp:Label>  </td>  
                                         <td> <asp:Label ID="Label6"   runat="server" Text='<%# Eval("TotalHours")%>' >  </asp:Label>  </td>  
                                         <td> <asp:Label ID="Label4" CssClass="statusTableData"   runat="server" Text='<%# Eval("Status")%>' >  </asp:Label>  </td>  

                                       <%--  <td id="<%# Eval("RequestID").ToString()%>"> <div id='lblbutton'    runat="server"> <%# Eval("Status").ToString() == "Pending Approval" ? "<input type='button' value='Cancel' class='newAbsencesButton cancel' style=' background-color:rgba(21, 161, 226, 0.88); color:#fff;'>" : "" %>  </div>  </td>--%>
                                          <td id='<%# Eval("RequestID").ToString()%>'> <div id='lblbutton'    runat="server"> <input type='button' value='Cancel Time Off' class='newAbsencesButton AppCancelTimeOff' style=' background-color:rgba(239, 177, 48, 0.88); color:#fff;'> </div>  </td>
                                    </tr>                  
                                </ItemTemplate>  
                        </asp:ListView>  
                            </div>                           
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

	</div>
</div>
</div>

         <asp:ToolKitScriptManager ID="ScriptManager1"  EnableScriptLocalization="true"  runat="server">                
        <Scripts>
            <asp:ScriptReference Path="https://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" />
          <%--  <asp:ScriptReference Path="../Scripts/jquery-1.9.1.min.js" />--%>
            <asp:ScriptReference Path="../Scripts/Common.js"/>
            <asp:ScriptReference Path="../Scripts/ChromeLoader.js"/>           
        </Scripts></asp:ToolKitScriptManager>
    
    </form>
</body>
</html>