<%-- The following 4 lines are ASP.NET directives needed when using SharePoint components --%>
<%@ Page Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" language="C#" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.0.min.js" type="text/javascript"></script>  
    <script type="text/javascript" src="../Scripts/App.js"></script>
    <link rel="Stylesheet" type="text/css" href="../Content/App.css" /> 

    </head>

<body style="display:none; overflow:scroll;">
     <form id="form1" runat="server">   
 <div id="chrome_ctrl_placeholder"></div>

 <div id="content">
 <div id="left">
<WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" >
<WebPartPages:XsltListViewWebPart ID="XsltListViewWebPart2" runat="server" ListUrl="Lists/TimeOffRequests" IsIncluded="True" 
NoDefaultStyle="TRUE" Title="TimeOff Requests" PageType="PAGE_NORMALVIEW" Default="False" ViewContentTypeId="0x"> 
</WebPartPages:XsltListViewWebPart>
</WebPartPages:WebPartZone>
</div></div>
         </form>
         </body>
    </html>
