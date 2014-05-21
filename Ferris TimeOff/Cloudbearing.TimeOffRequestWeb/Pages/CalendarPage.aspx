<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalendarPage.aspx.cs" Inherits="Cloudbearing.TimeOffRequestWeb.CalendarPage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function Redirect(page) {
            var SPHostUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
            var urldomain = "https://<%= Request.Url.Authority%>";
            var filterquerystring = window.location.search.replace("success=1&", "");
            window.location = urldomain + "/Pages/" + page + filterquerystring;
        }
    </script>
</head>
<body style="display:none; overflow:scroll;">
    <form id="form1" runat="server">
     <div id="chrome_ctrl_placeholder"></div> 
        <div class="container">
              <div class="page-header">
            <div class="page-header-content">
                <h1>&nbsp;</h1>
                <a onclick="Redirect('Ui.aspx')" class="back-button big page-back"></a>
            </div>
        </div>
            <asp:Label runat="server" ID="lblerrmsg"  CssClass="errmsg validation"></asp:Label>
        <h2 class="ms-accentText"> My Calendar</h2>
            </div>

             <asp:ToolKitScriptManager ID="ScriptManager1"  EnableScriptLocalization="true"  runat="server">                
        <Scripts>
            <asp:ScriptReference Path="https://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" />
            <asp:ScriptReference Path="../Scripts/jquery-1.9.1.min.js" />
            <asp:ScriptReference Path="../Scripts/Common.js"/>
            <asp:ScriptReference Path="../Scripts/ChromeLoader.js"/>           
        </Scripts></asp:ToolKitScriptManager>
    </form>
</body>
</html>
