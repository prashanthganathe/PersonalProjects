<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatusSummary.aspx.cs" Inherits="Cloudbearing.TimeOffRequestWeb.Pages.StatusSummary" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <meta name="viewport" content="width=device-width, user-scalable=yes" />
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
    <link href="../Style/jquery-ui-1.9.2.custom/css/custom-theme/jquery-ui-1.9.2.custom.css" rel="stylesheet" />
    <script src="../Style/jquery-ui-1.9.2.custom/js/jquery-ui-1.9.2.custom.min.js"></script>   
    
     
    <script type="text/javascript">
        var StartDate = null, EndDate = null, StartTime = null, EndTime = null;
        function SetDateTime() {
            StartDate = $('#txtStartDate').val();
            EndDate = $('#txtEndDate').val();
            StartTime = $('#ddlsrttime').val();
            EndTime = $('#ddlendtime').val();
        }
  
        $(document).ready(function () {
            $(document).ajaxStart(function () {
                $("#wait").css("display", "block");
            });
            $(document).ajaxComplete(function () {
                $("#wait").css("display", "none");
            });

            var icons = {
                header: "ui-icon-triangle-arrow-e",
                activeHeader: "ui-icon-triangle-arrow-s"
            };
            var yearselected = "";
            $("div.accordian").accordion({ active: false, collapsible: true});

          

            $('.ui-icon-triangle-arrow-e').on("click", function () {
                e.preventDefault();
                $(this).removeClass("ui-icon-triangle-arrow-e");
                $(this).addClass("ui-icon-triangle-arrow-s");
            });

          
            function GetDate(date) {
                var MyDate_String_Value = date;
                var value = new Date(parseInt(MyDate_String_Value.replace(/(^.*\()|([+-].*$)/g, '')));
                var min = value.getMinutes() > 9 ? value.getMinutes() : '0' + value.getMinutes();
                var dat = value.getMonth() + 1 + "/" + value.getDate() + "/" + value.getFullYear() + " " + value.getHours() + ":" + min;
                return dat;
            }

            function GetApproverText(App1, App2, App3) {
                var ret;
                if (App1 != null)
                    ret = App1;
                if (App2 != null)
                    ret += "," + App2;
                if (App3 != null)
                    ret += "," + App3;
                ret == undefined ? "No Approver(s) Assigned." : ret;
                return ret.substring(0, 25) + "...";
            }
            function GetListTemplateLevel3(items) {
                var listtemplate = "<div class='list level3' style='height:auto !important; style='width:800px;display: block; margin-left:100px !important;'> ";
                listtemplate += "   <ul style='margin-left:100px; width:100%;'>";
                var type;
                $.each(items, function (index, item) {
                    type = item.TimeOffType.substring(0, 19) + "...";
                    listtemplate += "<li>";
                    listtemplate += "<div class='requested'>" + item.RequestedBy + "</div>";
                    listtemplate += "<div class='type'>" + type + "</div>";
                    listtemplate += "<div class='startdate'>" + GetDate(item.StartDate) + "</div>";
                    listtemplate += "<div class='enddate'>" + GetDate(item.EndDate) + "</div>";
                    listtemplate += "<div class='accessible'>" + item.IsAccessible + "</div>";
                    listtemplate += "<div class='alternate'>" + item.Alternate + "</div>";
                    GetApproverText(item.Approver1, item.Approver2, item.Approver3)

                    listtemplate += "<div class='approver'>" + GetApproverText(item.Approver1, item.Approver2, item.Approver3) + "</div>";
                    listtemplate += "<div class='hrs'>" + item.TotalHours + "</div>";
                    listtemplate += "</li>";
                });
                listtemplate += "</ul>";
                listtemplate += "</div>";
                return listtemplate;
            }

            function FillLevel3Details(e, requestedbyid) {
                SetDateTime();
                $.ajax({
                    type: "POST",
                    url: "StatusSummary.aspx/GetDetails",
                    data: JSON.stringify({ 'year': yearselected, 'parameterkey': 'Status', 'parametervalue': requestedbyid, 'startdate': StartDate, 'enddate': EndDate, 'starttime': StartTime, 'endtime': EndTime }),
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    cache: false,
                    processData: false,
                    success: function (result) {
                        if (result != undefined && result.d.length > 0)
                            e.append(GetListTemplateLevel3(result.d));
                       
                    },
                    error: function (request, status, error) {
                    }
                });
            }

            $(document).on('click', '.level2header', function (e) {             
                var self = $(this);
                $(this).parent().find('.ui-icon').toggleClass('ui-icon-triangle-1-s').toggleClass('ui-icon-triangle-1-e');
                if ($(this).parent().hasClass("ui-accordion-header-active ui-state-active")) {
                    $(this).parent().removeClass("ui-accordion-header-active ui-state-active");
                    if ($(this).parent().next().hasClass('level3')) {
                        $(this).parent().next().hide();
                    }
                }
                else {
                    $(this).parent().addClass("ui-accordion-header-active ui-state-active");
                    if ($(this).parent().next().hasClass('level3')) {
                        $(this).parent().next().show();
                    }
                }

                if (!self.hasClass('clicked')) {
                    self.addClass('clicked');
                    if ($(this).parent().parent().find('ul').text() == "") {                       
                        FillLevel3Details($(this).find('.requestedby').parent().parent().parent(), $(this)[0].id);
                    }
                }
            });

            function FillLevel2(items) {
                var listtemplate = "";
                $.each(items, function (index, item) {
                    listtemplate += "<li class='requestedbyli'><div class='accordian'>";
                    listtemplate += "<h3 class=' ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons ui-corner-all ' >";
                    listtemplate += "<span class='ui-accordion-header-icon ui-icon ui-icon-triangle-1-e'></span><span class='level2header' id='" + item.Status + "'><span class='requestedby'  >" + item.Status + "</span><span style='float:right;margin-right:40px;'><b>" + item.TotalHours + "</b></span></span>";
                    // listtemplate += "<div style=' text-align:right;'>" + item.TotalHours + "</div>";
                    listtemplate += "</h3>";
                    //listtemplate += GetListTemplateLevel3(items);
                    listtemplate += "</li>";
                });
                return listtemplate;

            }

            function ShowYearDetails(type, e, yrDetails) {
                var div = "";
                if (yrDetails != null && yrDetails.length > 0)
                    $(e.next()).find('ul', '.level2').append(FillLevel2(yrDetails));
                else
                    $(e.next()).find('ul', '.level2').append('You don\'t have any submitted forms in the selected year.');
                $($(e).next()[0]).css('height', 'auto');
            }

            function getURL(type) {
                var url;
                if (type == "GetEmployeeDetailsByYear")
                    url = "SummaryReport.aspx/" + type;
                if (type == "GetDetailsByYearType")
                    url = "ReportByTypes.aspx/" + type;
                if (type == "GetDetailsByYearStatus")
                    url = "StatusSummary.aspx/" + type;
                return url;
            }

            function GetYearDetails(type, e) {
                // $(e).next().innerHTML = " <img src='loading.gif' id='img' style='display:none' />";
              
                var url = getURL(type);
                SetDateTime();
                $.ajax({
                    url: url,
                    data: JSON.stringify({ 'year': e.text(), 'startdate': StartDate, 'enddate': EndDate, 'starttime': StartTime, 'endtime': EndTime }),
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (result) {
                        ShowYearDetails(type, e, result.d);
                        $('#img').hide();
                    },
                    error: function (request, status, error) {
                    }
                });
            }

            $(document).on("click", ".year", function (e) {
                e.preventDefault();
                var $elt = $(this).attr('disabled', true);
                setTimeout(function () {
                    $elt.attr('disabled', false);
                }, 1500);

                
                    var self = $(this);
                    if (!self.hasClass('clicked')) {
                        self.addClass('clicked');
                        var childelement = $(this).next().find('.requestedby');
                        if (childelement.length === undefined || childelement.length === 0) {
                            $($(e).next()[0]).css('height', 'auto');
                            yearselected = $(this).text().trim();
                            var temp = $(this).next().find('.level2header');
                            if (temp == undefined || temp.length == 0) {
                                GetYearDetails("GetDetailsByYearStatus", $(this));
                            }
                        }
                    }
             
            });
        });
    </script>
    <style type="text/css">

         /*ul {
         list-style:none;
         }*/
         .list {
            height:auto !important;
        }

        .ui-widget {
            font-family: 'Segoe UI Semilight', 'Open Sans', Verdana, Arial, Helvetica, sans-serif;
        }

          .newAbsencesButton {
    margin:0px 10px 0px 0px !important;
     background-color:rgba(21, 161, 226, 0.88) !important;
     color:white;
    
}
    .newAbsencesButton:hover {
     background-color:rgba(21, 161, 226, 0.88) !important;
    }
    #AccordianParent {
    float:left;
    }

        .requested {
            height:auto;  
    width: 15%; 
    float: left;
        }
    .type{
    height:auto;  
    width: 14%; 
    float: left;
    text-overflow:ellipsis;
    }
    .status{
    height:auto;  
    width: 13%; 
    float: left;
    }

    .startdate {
    height: auto;
    width: 8%;
    float: left;
    }
    .enddate {
    height: auto;
    width: 10%;
    float: left;
    }
    .accessible {
    height: auto;
    width: 8%;
    float: left;
    }
    .alternate {
    height: auto;
    width: 6%;
    float: left;
    }
    .approver {
    height: auto;
    width: 23%;
    float: left;

    }

    .hrs {
    height: auto;
    width: 4%;
    float: left;
  
    }
   

    .ui-widget {
     font-size: 0.9em;
    }

    .divSingleline{    
        text-align:center;
        background-color:#f9f9f9;
             font-weight:bold;
    float: left;    
    line-height:20px;
    font-family: 'Segoe UI Semilight', 'Open Sans', Verdana, Arial, Helvetica, sans-serif;
    height:20px;
    color:#444444;
    }
     .body {
              font-family: 'Segoe UI', Tahoma, Arial;
        }
       .back-button {
  height: 32px;
  width: 32px;
  display: block;
  cursor:pointer;
  background-image: url(data:image/png;base64,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);
  -webkit-background-size: cover;
  -moz-background-size: cover;
  -o-background-size: cover;
  background-size: cover;
}

       .toolbar a.big,
.toolbar button.big {
  min-height: 48px;
  height: 48px;
  font-size: 1.2em;
}

       .toolbar a.big,
.toolbar button.big {
  min-width: 48px;
  width: 48px;
}

         .page.secondary .page-back {
    width: 24px;
    height: 24px;
    left: 0px !important;
  }

         .shead{    
        text-align:right;
    float: left;}
          #blankdiv {
            margin-left:30px;background-color:#f9f9f9;height:30px;width:93%;border-width:1px;margin-left: -2px;
        }

           .validation {
        color:red;
        padding-top:30px;
        margin-top:30px;
        font-size:18px;
        
        }
    </style>
</head>
<body  style="display:none; overflow:scroll;">
    <form id="form1" runat="server">
        <div id="chrome_ctrl_placeholder"></div>
          <div class="container">

        
          <div class="page-header">
          <div class="page-header-content" style="width:100%;margin-left:40px;">
             <div style="float:left; width:10%;"><a onclick="Redirect('Ui.aspx')" class="back-button big page-back"></a></div>
              <div style="float:left; width:90%; margin-left:-50px"><h2>All Time Off Requests by Status<small></small></h2></div>
          </div>
         </div>
               <div style="text-align:center;"><asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="validate"  CssClass="validation"/></div> 
         <asp:Label runat="server" ID="lblerrmsg"  CssClass="errmsg validation"></asp:Label>

               <div style="width:100%; text-align:center;display:none;">
                 <div style=" margin-left:150px;margin-top:6px;">   
                                                    <br />                                                 
                                                       <span class="ms-textLarge"> From<strong>:</strong></span>
                                                        <asp:TextBox ID="txtStartDate" runat="server" ClientIDMode="Static"  ></asp:TextBox>  
                                                        &nbsp;<asp:DropDownList ID="ddlsrttime" runat="server" ClientIDMode="Static" > </asp:DropDownList>
                                                        &nbsp;<ajaxtoolkit:calendarextender ID="CalendarExtender1" TargetControlID="txtStartDate" Format="MM/dd/yyyy" runat="server"></ajaxtoolkit:calendarextender>                      
                                                        &nbsp;&nbsp;
                                                         <span class="ms-textLarge"> To<label title="*" class="validation"> </label><strong>:</strong> </span>
                                                        <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" ></asp:TextBox>
                                                        <asp:DropDownList ID="ddlendtime" runat="server" ClientIDMode="Static" ></asp:DropDownList>
                                                        <ajaxtoolkit:calendarextender ID="CalendarExtender2" TargetControlID="txtEndDate" Format="MM/dd/yyyy" runat="server"></ajaxtoolkit:calendarextender>
                                                      <asp:CompareValidator ID="cmpVal1" ControlToCompare="txtStartDate" 
                                                         ControlToValidate="txtEndDate" Type="Date" Operator="GreaterThanEqual"       
                                                         ErrorMessage="End date should be greater/Equal to Start date" runat="server" ValidationGroup="validate" Display="None"  CssClass="validation" ></asp:CompareValidator>                 
                                                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"  ValidationGroup="validate" CssClass="newAbsencesButton"  ForeColor="White"   />
                                               </div>
             </div>

<div id="wait" style="display:none;position:absolute;top:50%;left:50%;padding:2px; z-index:3000;">
    <img src="loading.gif" /><br/>Loading..</div>
                  
                 <div id="divgridSection" runat="server" ClientIDMode="Static" >
 <div id="AccordianParent" style="width:99.5%; padding-left:50px;margin-top:20px;">
              <div class="divSingleline ui-helper-reset ui-state-default" style="width:5%; padding-right:8px;height:35px;">Year</div>
       <div class="divSingleline ui-helper-reset ui-state-default " style="width:9%; padding-right:8px;height:35px;">Status</div>
              <div class="divSingleline ui-helper-reset ui-state-default " style="width:10%; padding-right:8px;height:35px;">Requestor</div>            
              <div class="divSingleline ui-helper-reset ui-state-default " style="width:12%; padding-right:8px;height:35px;">TimeOffType</div>
              <div class="divSingleline ui-helper-reset ui-state-default " style="width:7%; padding-right:8px;height:35px;">Starts</div>
              <div class="divSingleline ui-helper-reset ui-state-default " style="width:7%; padding-right:8px;height:35px;">Ends</div>
              <div class="divSingleline ui-helper-reset ui-state-default " style="width:5%; padding-right:8px;height:35px;">Accesible</div>
              <div class="divSingleline ui-helper-reset ui-state-default " style="width:5%; padding-right:8px;height:35px;">Alternate</div>
              <div class="divSingleline ui-helper-reset ui-state-default " style="width:18%; padding-right:8px;height:35px;">Approver</div>          
              <div class="divSingleline ui-helper-reset ui-state-default " style="width:7.5%; padding-right:7px;height:35px;">Hours</div>  <br />    
             
  <div id="faqscontainer"  runat="server"  ClientIDMode="Static"  class="accordian" style="width:1250px !important;margin-top:20px;">
    <asp:Repeater ID="repAccordian" runat="server">
                <ItemTemplate>                
                    <h3 class="year" > <span ><%# Eval("YEAR") %></span></h3>  
                     <div class="list " style=" height:auto !important;">           
                        <ul class="level2">                         
                        </ul>
                    </div>     
                    </ItemTemplate>
        </asp:Repeater>        
</div>
      <br /><div runat="server" id="blankdiv" ClientIDMode="Static" >
                    You don't have any records .
                </div>
     </div>
                     </div>
</div>
          <asp:ToolKitScriptManager ID="ScriptManager1"      EnableScriptLocalization="true"      runat="server">                
            <Scripts>
                <asp:ScriptReference Path="https://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" />
                <asp:ScriptReference Path="../Scripts/Common.js"/>
                <asp:ScriptReference Path="../Scripts/ChromeLoader.js"/>           
            </Scripts>
       </asp:ToolKitScriptManager>               
</form>
     
</body>
</html>
