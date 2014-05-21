<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovalForm.aspx.cs" Inherits="Cloudbearing.TimeOffRequestWeb.ApprovalForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <meta name="viewport" content="width=device-width, user-scalable=yes" />
    <title>Time Off Approval</title>
        <link href="../Style/Lite.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>
        <script src="../Scripts/Common.js" type="text/javascript"></script>
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
<body style="display:none;overflow:scroll;">
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
                                                <h2>Time Off Approval Form</h2> </div>
                                        </div>
                            </div>
                        </div>
                 </div>

                     <!-- Controls -->
                  <div class="section ms-error " id="divErrorMessage" style="margin-left:120px;">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="validate"  CssClass="validation"/>
                                                <asp:Label runat="server" ID="lblerrmsg"  CssClass="errmsg validation"></asp:Label><br />              
                                            </div>
                  <div id="divVisible" runat="server" clientidmode="Static">
   <div class="page secondary">
                    <div class="page-region">
                        <div class="page-region-content">
                            <div class="grid">
                                <div class="row">
                                    <div class="span8">
                                        <div id="requestedAbsences" class="dynamicControl">
                                            
                                        <div id="DeltaPlaceHolderMain">	
		                                        
                                              <div id="divbuttons" runat="server" clientidmode="Static"  style="text-align:left; margin-left:30px; float:left; width: 726px;">
                                                 <asp:Button ID="btnChkCalendar" runat="server" Text="Check Calendar" OnClick="btnChkCalendar_Click" ValidationGroup="validate" class="newAbsencesButton"    />    
                                                            
                                                  &nbsp;&nbsp;&nbsp;                        
                                                  <asp:Button ID="btnCancel" runat="server" Text="Reject" OnClick="btnCancel_Click" ValidationGroup="validate" class="newAbsencesButton"/>
                                                  &nbsp;&nbsp;
                                                   <asp:Button ID="btnSubmit" runat="server" Text="Approve" OnClick="btnSubmit_Click" ValidationGroup="validate" class="newAbsencesButton"  />              
                                            </div>	
                                            <br />
                                            

                                              <br />
                                            

                                            <div style="margin-left:10px; width:100%;">
                                                <span ><asp:CheckBox ID="chkAlternateContact" runat="server" Text="  I have an alternate Contact" Enabled="False" /></span>&nbsp;&nbsp;
                                                <span ><asp:CheckBox ID="chkAccessible" runat="server" Text="  I will be accessible during my time off" Enabled="False" /></span>&nbsp;&nbsp;
                                                <span ><asp:CheckBox ID="chkPrivate" runat="server" Text="  Mark as Private" Enabled="False" /></span>
                                            </div>
                                            <hr />
                                           <h3><b>Who</b></h3>
                                            <h4><span class="ms-textLarge ms-soften label">Requested By <strong>:</strong></span> <asp:Label ID="lblRequestedBy" runat="server" ></asp:Label></h4>     
                                           
                                            <h3><b>When</b></h3>
                                            <h4><span class="ms-textLarge ms-soften label">Requested On <strong>:</strong></span> 
                                                <asp:Label ID="lblRequestedOn" runat="server" ></asp:Label></h4>
                                            <h4><span class="ms-textLarge ms-soften label">Type of Leave <strong>:</strong><label title="*" class="validation">* </label></span>                                          
                                               <asp:Label ID="lblTimeoffType" runat="server" ></asp:Label>    
                                            </h4>

                                             <div id="notesDiv">
                                                 <h4><span class="ms-textLarge ms-soften label">Notes <strong>:</strong></span>                                          
                                                   <asp:Label ID="txtNotes" runat="server" ClientIDMode="Static"  TextMode="MultiLine" style="height: auto; "></asp:Label>
                                            
                                                 </h4>
                                            </div>


                                            <h4><span class="ms-textLarge ms-soften label">Time Off Dates <strong>:</strong><label title="*" class="validation">* </label>
                                                </span>&nbsp;<asp:Label ID="lblFullday" runat="server" ></asp:Label>   
                                                <div style=" margin-left:150px;margin-top:6px;">   
                                                    <br />                                                 
                                                       <span class="ms-textLarge"> From<label title="*" class="validation">* </label><strong>:</strong></span>
                                                        
                                                        <asp:Label ID="txtStartDate" ClientIDMode="Static" runat="server" ></asp:Label>   
                                                        
                                                        &nbsp;&nbsp;                      
                                                        &nbsp;&nbsp;
                                                         <span class="ms-textLarge"> To<label title="*" class="validation">* </label><strong>:</strong> </span>
                                                     <asp:Label ID="txtEndDate" ClientIDMode="Static" runat="server" ></asp:Label>   
                                                      
                                                       
                                               </div>
                                           </h4>
                                            <h4>
                                                <span class="ms-textLarge"> Exclude Weekends<strong>:&nbsp;&nbsp;</strong></span>
                                                <asp:RadioButton ID="rbExcludeWeekendYes" runat="server" Text=" Yes" GroupName="ExcludeWeekend" Checked="true" ClientIDMode="Static" Enabled="False"  />&nbsp;&nbsp;
                                                <asp:RadioButton ID="rbExcludeWeekendNo" runat="server" Text=" No" GroupName="ExcludeWeekend" ClientIDMode="Static" Enabled="False"  />                   
                                            </h4>
                                            <h4>
                                                <span class="ms-textLarge"> Exclude Holidays<strong>:&nbsp;&nbsp;&nbsp;&nbsp;</strong></span>
                                                  <asp:RadioButton ID="rbExcludeHolidayYes" runat="server" Text=" Yes" GroupName="ExcludeHoliday" Checked="true" ClientIDMode="Static" Enabled="False"  />&nbsp;&nbsp;<asp:RadioButton ID="rbExcludeHolidayNo" runat="server" Text=" No" GroupName="ExcludeHoliday" ClientIDMode="Static" Enabled="False" />
                                             </h4>
                                            <h4>
                                                <span class="ms-textLarge">Exclude Other days<strong>:&nbsp;</strong></span>
                                               <asp:RadioButton ID="rbExcludeOtherYes" runat="server" Text=" Yes" GroupName="ExcludeOther" Checked="true" ClientIDMode="Static" Enabled="False"   />&nbsp;&nbsp;<asp:RadioButton ID="rbExcludeOtherNo" runat="server" Text=" No" GroupName="ExcludeOther" ClientIDMode="Static" Enabled="False"  />
                                           </h4>

                                            <h4>
                                              <span class="ms-textLarge ms-soften label">Approver 1<label title="*" class="validation" runat="server" id="lblApp1" visible="false">* </label><strong>:</strong></span>
                                               <asp:Label ID="lblappr1" runat="server" ></asp:Label><br />  
                                            </h4>
                                            <h4>
                                                <span class="ms-textLarge ms-soften label"> Approver 2<label title="*" class="validation" runat="server" id="lblApp2" visible="false">* </label> <strong>:</strong></span>
                                                 <asp:Label ID="lblappr2" runat="server" ></asp:Label>  <br />
                                            </h4>

                                            <h4>
                                               <span class="ms-textLarge ms-soften label"> Approver 3<label title="*" class="validation" runat="server" id="lblApp3" visible="false">*</label> <strong>:</strong></span>
                                                <asp:Label ID="lblappr3" runat="server" ></asp:Label>              
                                            </h4>
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
		</div>
        <%--<asp:ToolkitScriptManager  ID="ScriptManager1"  EnableScriptLocalization="true"  runat="server">                
                <Scripts>                 
                     <asp:ScriptReference Path="https://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" />
               <asp:ScriptReference Path="../Scripts/jquery-1.9.1.min.js" />
                   <asp:ScriptReference Path="../Scripts/Common.js"/>
                    <asp:ScriptReference Path="../Scripts/ChromeLoader.js"/>
                </Scripts>                
       </asp:ToolkitScriptManager>--%>
        <script type="text/javascript" src="../Scripts/ChromeLoader.js"></script>
        <script type="text/javascript" src="../Scripts/default.js"></script>      
    </form>
</body>
</html>
