<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyList.aspx.cs" Inherits="Cloudbearing.TimeOffRequestWeb.Pages.MyList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Time Off List</title>
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
              
   

               //var sph = "@SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current).SPHostUrl";
               //var spa = "@SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current).SPAppWebUrl";
               //var spl = "@SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current).SPLanguage";
               //var spn = "@SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current).SPProductNumber";
               //var spc = "@SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current).SPClientTag";
               //var spct = "@(((SharePointAcsContext) SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current)).ContextToken)";

               function CancelRequest(requestid,currentitemStartDate,currentitemEndDate,status, e) {                    
                   $.ajax({
                       url: "MyList.aspx/CancelRequest",
                       data: JSON.stringify({ 'requestid': requestid, 'startdate': currentitemStartDate, 'enddate': currentitemEndDate, 'status': status}),
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

               function CheckCurrentRequestDateFuture(inputDate) {
                   //ToDO get the tolerance days from ConfigList
                   if (Date.parse(inputDate) > Date.parse(new Date())) {
                       return true;
                   }
                   else
                       return false; 
               }

               $(document).on('click', '.cancel', function (e) {
                   var temp = $(this).parent().parent().parent();
                   var currentitemStatus = temp.find('.statusTableData').html();
                   var currentitemStartDate = temp.find('.startDateTableData').html();
                   var currentitemEndDate = temp.parent().find('.endDateTableData').html();
                   var requestid = $(this).parent().parent()[0].id;
                   var currentItemProposedStatus = "Cancel";
                   var type = temp.find('.type').html();

                   if (confirm("Do you want to cancel the '" + type + "' (Time Off) Request ?")) {                   
                       if (currentitemStatus == "Pending Approval")
                           CancelRequest(requestid, currentitemStartDate, currentitemEndDate, currentitemStatus, e);
                       else {
                           if (currentitemStatus == "Approved") {
                               if (CheckCurrentRequestDateFuture(currentitemStartDate))
                                   CancelRequest(requestid, currentitemStartDate, currentitemEndDate, currentitemStatus, e);
                               else
                                   alert('Could not able to cancel the request. As the StartDate is reffered to past date Or its too late to cancel.');
                           }
                       }
                   }                  
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
                                        <h2>My Time Off List</h2> </div>
                                </div>
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
                            <div id="requestedAbsences" class="dynamicControl">
                                
                               <%-- <table id="requestedAbsences_table" class="striped"><thead><tr><th style="">Type of leave</th><th class="right" style="width:180px">Employee</th><th class="right" style="width:112px">Workdays</th><th class="right" style="width:110px">From</th><th class="right" style="width:110px">To</th><th class="right" style="width:180px">Status</th><th style="width:170px"></th></tr></thead><tbody><tr class=""><td>Vacation</td><td class="right"><a class="" href="../_layouts/15/userdisp.aspx?ID=19">Ken Berlack</a></td><td class="right">5</td><td class="right">4/3/2014</td><td class="right">4/11/2014</td><td class="right">Pending</td><td class="right"><button type="button" onclick="ShowAbsenceToAccept(1);return false;" class="command-button bg-color-red fg-color-white wide-white"><h3 class="fg-color-white">View</h3><span class="icon-arrow-right-3"></span></button></td></tr><tr class=""><td>Vacation</td><td class="right"><a class="" href="../_layouts/15/userdisp.aspx?ID=19">Ken Berlack</a></td><td class="right">2</td><td class="right">4/15/2014</td><td class="right">4/16/2014</td><td class="right">Pending</td><td class="right"><button type="button" onclick="ShowAbsenceToAccept(2);return false;" class="command-button bg-color-red fg-color-white wide-white">
                                    <h3 class="fg-color-white">View</h3><span class="icon-arrow-right-3"></span></button></td></tr></tbody></table>--%>

                                <table id="dummytable" runat="server" visible="false"  class="striped">
	                            <tbody><tr id="LVreqDetails_Tr1" class="TableHeader">
		                            <td id="LVreqDetails_Td1" ><b>Time Off Type</b></td>
		                            <td id="LVreqDetails_Td6">Time Off Day</td>
		                            <td id="LVreqDetails_Td2">Start Date</td>
		                            <td id="LVreqDetails_Td3">End Date</td>
		                            <td id="LVreqDetails_Td7">Total Hours</td>
		                            <td id="LVreqDetails_Td4">Status</td>
	                            </tr>		  
                                    <tr class="TableData">  
                                        <td colspan="6"><span id="LVreqDetails_Label1_0">You don't have any submitted forms in the last 3 months.</span> </td>                                         
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
                                        <td><asp:Label  ID="Label1"  runat="server"   Text='<%# Eval("TimeOffType")%>'  CssClass="type"  >  </asp:Label> </td>  
                                          <td> <asp:Label ID="Label5"   runat="server" Text='<%# Eval("isFullDay")%>' >  </asp:Label>  </td>  
                                     <%--   <td> <asp:Label ID="Label2"   runat="server" Text='<%# Eval("StartDate")%>' >  </asp:Label>  </td>  --%>
                                         <td> <asp:Label ID="Label2"  CssClass="startDateTableData"  runat="server" Text='<%# Eval("isFullDay").ToString() == "Full Day" ? Eval("StartDate", "{0:d}") : Eval("StartDate") %>' >  </asp:Label>  </td>  
<%--                                         <td> <asp:Label ID="Label3"   runat="server" Text='<%# Eval("EndDate")%>' >  </asp:Label>  </td> --%> 
                                        <td> <asp:Label ID="Label3"  CssClass="endDateTableData"  runat="server" Text='<%# Eval("isFullDay").ToString() == "Full Day" ? Eval("EndDate", "{0:d}") : Eval("EndDate") %>' >  </asp:Label>  </td>  
                                         <td> <asp:Label ID="Label6"   runat="server" Text='<%# Eval("TotalHours")%>' >  </asp:Label>  </td>  
                                         <td> <asp:Label ID="Label4" CssClass="statusTableData"   runat="server" Text='<%# Eval("Status")%>' >  </asp:Label>  </td>  

                                       <%--  <td id="<%# Eval("RequestID").ToString()%>"> <div id='lblbutton'    runat="server"> <%# Eval("Status").ToString() == "Pending Approval" ? "<input type='button' value='Cancel' class='newAbsencesButton cancel' style=' background-color:rgba(21, 161, 226, 0.88); color:#fff;'>" : "" %>  </div>  </td>--%>
                                        <td id='<%# Eval("RequestID").ToString()%>'> <div id='lblbutton'    runat="server"> <input type='button' value='Cancel Time Off' class='newAbsencesButton cancel' style=' background-color:rgba(21, 161, 226, 0.88); color:#fff;'> </div>  </td>
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
