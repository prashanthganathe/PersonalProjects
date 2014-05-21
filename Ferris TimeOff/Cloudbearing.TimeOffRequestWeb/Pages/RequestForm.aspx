<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestForm.aspx.cs" Inherits="Cloudbearing.TimeOffRequestWeb.Pages.RequestForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Time Off Request Form</title>
     <meta name="viewport" content="width=device-width, user-scalable=yes" />
    <link href="../Style/Lite.css" rel="stylesheet" />
       <script src="../Scripts/Common.js"></script>
    <style type="text/css">
        div.container{ margin: 10px 20px; }
        .reminders {
            width: auto;
            float: left;
             margin-left:10px;
        }
        .remindersWide {
            width: 30%;
            float: left;
        }

        .label {
            width: 13%;
            float: left;
        }

        div.row {
            margin-top: 10px;
            margin-bottom: 15px;
        }

        div.section {
            width: 100%;
            float: left;
            margin-top:30px;
        }

        .validation {
        color:red;
        
        }

        .errmsg {
         font-size:13px;
         font-family: 'Segoe UI', Tahoma, Arial;
          padding-left:25px;
         
        }

        .center {
         text-align:center;
        }
        .medium {
         font-size:14px;
        }


    </style>
</head>
<body style="display:none; overflow:scroll;">
    <form id="form1" runat="server">
       <div id="chrome_ctrl_placeholder"></div>
    <div id="s4-bodyContainer">		
		<div id="contentRow">
            <div id="sideNavBox" class="ms-dialogHidden ms-forceWrap ms-noList">
              <div id="DeltaPlaceHolderLeftNavBar" class="ms-core-navigation" role="navigation"></div>
            </div>
            <div id="contentBox" aria-live="polite" aria-relevant="all">             
                <div class="page">
		            <div class="page-region">
			            <div class="page-region-content">				         
                                  <div class="divSingleline " style="width:100%;">
                                        <div class="divSingleline" style="width:90%;">
                                             <div class="divSingleline" style="width:10%;">
                                                 <a onclick="Redirect('ui.aspx')" class="back-button big page-back"></a></div>  
                                            <h2>Time Off Request Form</h2> </div>
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
                                        <div id="DeltaPlaceHolderMain">	
		                                        
                                            <div class="section ms-error " id="divErrorMessage">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="validate"  CssClass="validation"/>
                                                <asp:Label runat="server" ID="lblerrmsg"  CssClass="errmsg validation"></asp:Label><br />              
                                            </div>
                                            <div style="margin-left:10px; width:100%;">
                                                <span ><asp:CheckBox ID="chkAlternateContact" runat="server" Text=" I have an alternate Contact" /></span>&nbsp;&nbsp;
                                                <span ><asp:CheckBox ID="chkAccessible" runat="server" Text="  I will be accessible during my time off" /></span>&nbsp;&nbsp;
                                                <span ><asp:CheckBox ID="chkPrivate" runat="server" Text="  Mark as Private" /></span>
                                            </div>
                                            <hr />
                                           <h3><b>Who</b></h3>
                                            <h4><span class="ms-textLarge ms-soften label">Requested By <strong>:</strong></span> <asp:Label ID="lblCurrentUser" runat="server" ></asp:Label></h4>     
                                           
                                            <h3><b>When</b></h3>
                                            <h4><span class="ms-textLarge ms-soften label">Requested On <strong>:</strong></span> 
                                                <asp:Label ID="lblToday" runat="server" ></asp:Label></h4>
                                            <h4><span class="ms-textLarge ms-soften label">Type of Leave <strong>:</strong><label title="*" class="validation">* </label></span>                                          
                                                <asp:DropDownList ID="ddTimeoffType" runat="server" ClientIDMode="Static" style="width:250px;" ></asp:DropDownList>   
                                                <asp:RequiredFieldValidator ID="rqdTOType" ControlToValidate="ddTimeoffType" runat="server"  Display="None" ErrorMessage="Please select TimeOff Type" ValidationGroup="validate"  InitialValue="- Select -"></asp:RequiredFieldValidator>                     
                                            </h4>

                                            <div id="notesDiv">
                                                 <h4><span class="ms-textLarge ms-soften label">Notes <strong>:</strong></span>                                          
                                                   <asp:TextBox ID="txtNotes" runat="server" ClientIDMode="Static"  TextMode="MultiLine" Height="41px" Width="403px" ></asp:TextBox> 
                                                    <label runat="server" style="color:gray; font-style:italic"  ClientMode="static" id="lblnotesHint"></label>
                                                 </h4>
                                            </div>

                                            <h4><span class="ms-textLarge ms-soften label">Time Off Dates <strong>:</strong><label title="*" class="validation">* </label></span>&nbsp;&nbsp; <asp:RadioButton ID="rbFullDay" runat="server" Text="  Full Day" GroupName="FullOrPartial" Checked="true" ClientIDMode="Static"  />&nbsp;&nbsp;
                                               <asp:RadioButton ID="rbPartial" runat="server" Text="  Partial Day" GroupName="FullOrPartial"  ClientIDMode="Static" />
                                                <div style=" margin-left:150px;margin-top:6px;">   
                                                    <br />                                                 
                                                       <span class="ms-textLarge"> From<label title="*" class="validation">* </label><strong>:</strong></span>
                                                        <asp:TextBox ID="txtStartDate" runat="server" ClientIDMode="Static"  ></asp:TextBox>  
                                                        &nbsp;<asp:DropDownList ID="ddlsrttime" runat="server" ClientIDMode="Static" > </asp:DropDownList>
                                                        &nbsp;<asp:RequiredFieldValidator ID="rqdStart" ControlToValidate="txtStartDate" runat="server"  Display="None" ErrorMessage="Please fill the start date" ValidationGroup="validate"></asp:RequiredFieldValidator>
                                                        <ajaxtoolkit:calendarextender ID="CalendarExtender1" TargetControlID="txtStartDate" Format="MM/dd/yyyy" runat="server"></ajaxtoolkit:calendarextender>                      
                                                        &nbsp;&nbsp;
                                                         <span class="ms-textLarge"> To<label title="*" class="validation">* </label><strong>:</strong> </span>
                                                        <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" ></asp:TextBox>
                                                        <asp:DropDownList ID="ddlendtime" runat="server" ClientIDMode="Static" ></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rqdEndDate" ControlToValidate="txtEndDate" runat="server" Display="None" ErrorMessage="Please fill the end date" ValidationGroup="validate"></asp:RequiredFieldValidator>            
                                                        <ajaxtoolkit:calendarextender ID="CalendarExtender2" TargetControlID="txtEndDate" Format="MM/dd/yyyy" runat="server"></ajaxtoolkit:calendarextender>
                                                      <asp:CompareValidator ID="cmpVal1" ControlToCompare="txtStartDate" 
                                                         ControlToValidate="txtEndDate" Type="Date" Operator="GreaterThanEqual"       
                                                         ErrorMessage="End date should be greater/Equal to Start date" runat="server" ValidationGroup="validate" Display="None"  CssClass="validation" ></asp:CompareValidator>                 
                                               </div>
                                           </h4>
                                            <h4>
                                                <span class="ms-textLarge"> Exclude Weekends<strong>:&nbsp;&nbsp;</strong></span>
                                                <asp:RadioButton ID="rbExcludeWeekendYes" runat="server" Text=" Yes" GroupName="ExcludeWeekend" Checked="true" ClientIDMode="Static"  />&nbsp;
                                                <asp:RadioButton ID="rbExcludeWeekendNo" runat="server" Text=" No" GroupName="ExcludeWeekend" ClientIDMode="Static"  />                   
                                            </h4>
                                            <h4>
                                                <span class="ms-textLarge"> Exclude Holidays<strong>:&nbsp;&nbsp;&nbsp;&nbsp;</strong></span>
                                                  <asp:RadioButton ID="rbExcludeHolidayYes" runat="server" Text=" Yes" GroupName="ExcludeHoliday" Checked="true" ClientIDMode="Static"  />&nbsp;&nbsp;<asp:RadioButton ID="rbExcludeHolidayNo" runat="server" Text=" No" GroupName="ExcludeHoliday" ClientIDMode="Static" />
                                             </h4>
                                            <h4>
                                                <span class="ms-textLarge">Exclude Other days<strong>:&nbsp;</strong></span>
                                               <asp:RadioButton ID="rbExcludeOtherYes" runat="server" Text=" Yes" GroupName="ExcludeOther" Checked="true" ClientIDMode="Static"   />&nbsp;&nbsp;<asp:RadioButton ID="rbExcludeOtherNo" runat="server" Text=" No" GroupName="ExcludeOther" ClientIDMode="Static"  />
                                           </h4>

                                            <h4>
                                              <span class="ms-textLarge ms-soften label">Approver 1<label title="*" class="validation" runat="server" id="lblApp1" visible="false">* </label><strong>:</strong></span>
                                               <asp:DropDownList ID="ddApprover1" runat="server" ClientIDMode="Static"  style="width:200px;" ></asp:DropDownList>  
                                                <%--<asp:RequiredFieldValidator ID="rqdAppr1" runat="server" ErrorMessage="Please select Approver" Display="None" ControlToValidate="ddApprover1" ValidationGroup="validate"   InitialValue="- Select -" CssClass="validate"></asp:RequiredFieldValidator>                     --%>
                                            </h4>
                                            <h4>
                                                <span class="ms-textLarge ms-soften label"> Approver 2<label title="*" class="validation" runat="server" id="lblApp2" visible="false">* </label> <strong>:</strong></span>
                                                 <asp:DropDownList ID="ddApprover2" runat="server" ClientIDMode="Static"  style="width:200px;" ></asp:DropDownList>
                                            </h4>

                                            <h4>
                                               <span class="ms-textLarge ms-soften label"> Approver 3<label title="*" class="validation" runat="server" id="lblApp3" visible="false">*</label> <strong>:</strong></span>
                                                  <asp:DropDownList ID="ddApprover3" runat="server" ClientIDMode="Static"  style="width:200px;" ></asp:DropDownList>                  
                                            </h4>


                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="newAbsencesButton"  OnClick="btnCancel_Click"  ClientIDMode="Static" />
                                                                    <asp:Button ID="btnSubmit" class="newAbsencesButton" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="validate" />
                       
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
		</div>

   <asp:ToolKitScriptManager ID="ScriptManager1"      EnableScriptLocalization="true"      runat="server">                
        <Scripts>
            <asp:ScriptReference Path="https://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" />
            <asp:ScriptReference Path="../Scripts/jquery-1.9.1.min.js" />
            <asp:ScriptReference Path="../Scripts/Common.js"/>
            <asp:ScriptReference Path="../Scripts/ChromeLoader.js"/>           
        </Scripts>
            </asp:ToolKitScriptManager>
        <script type="text/javascript" src="../Scripts/default.js"></script>
    </form>
</body>
</html>
