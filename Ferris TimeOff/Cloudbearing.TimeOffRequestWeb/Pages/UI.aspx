<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UI.aspx.cs" Inherits="Cloudbearing.TimeOffRequestWeb.Pages.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/SPUI.css" rel="stylesheet" />

    <script src="../Scripts/jquery-1.9.1.min.js"></script> 
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>
    <script type="text/javascript">
        function Redirect(page) {          
           var SPHostUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
           var urldomain = "https://<%= Request.Url.Authority%>";
            var filterquerystring = window.location.search.replace("success=1&", "");           
           window.location = urldomain + "/Pages/" + page + filterquerystring;
        }

        function RedirectAppList(listname) {
            var SPHostUrl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
            //if (SPHostUrl == undefined)
            //    SPHostUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
            var currentpath = SPHostUrl + listname;
            currentpath = currentpath.replace("success=1&", "");
            window.location = currentpath;
        }

      
    </script>
   
</head>
<body style="display: none; overflow: scroll;">
    <form id="form1" runat="server">
     <div id="chrome_ctrl_placeholder"></div>
     <div class="container" style=" margin-left:150px; " >
          <div id="divmsg" class="success message"  style=" width:70%;">
                 <h3>Success</h3>
                 <p>Your request is successfully created.</p>
             </div><br />

          <table style=" border:1px; width:100%;">
                      <tr>
                          <td>
                              
     <div class="app-tileview-tiles" style="width:100%;">
	        <div class="tileview-tile-root"  >
		        <div id="myTile" class="tileview-tile-content">
			        <a onclick="javascript:Redirect('RequestForm.aspx'); return false;" href="#">
				        <span >
					        <img src="../Style/Imagem/Add.png"  class="tileImg" alt=""/>
				        </span>
				        <div class="tileview-tile-detailsBox">
					        <ul class="tileview-tile-detailsListMedium">
						        <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
							        <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">New Time Off Request</div>
						        </li>
						        <li class="tileview-tile-descriptionMedium"></li>
					        </ul>
				        </div>
			        </a>
		        </div>
	        </div>         

		    <div class="tileview-tile-root">
			    <div id="Div4" class="tileview-tile-content">
				    <a onclick="javascript:Redirect('MyList.aspx'); return false;" href="#">
					    <span>
						    <img src="../Style/Imagem/carta.png" class="tileImg" alt=""/>
					    </span>
					    <div class="tileview-tile-detailsBox">
						    <ul class="tileview-tile-detailsListMedium">
							    <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								    <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">My Time Off Requests</div>
							    </li>
							    <li class="tileview-tile-descriptionMedium"></li>
						    </ul>
					    </div>
				    </a>
			    </div>
		    </div>

         <div class="tileview-tile-root">
			    <div id="Div2" class="tileview-tile-content">
				    <a onclick="javascript:Redirect('CalendarPage.aspx'); return false;" href="#">
					    <span>
						    <img src="../Style/Imagem/cal.png" class="tileImg" alt=""/>
					    </span>
					    <div class="tileview-tile-detailsBox">
						    <ul class="tileview-tile-detailsListMedium">
							    <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								    <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">Department Time Off Calendar</div>
							    </li>
							    <li class="tileview-tile-descriptionMedium"></li>
						    </ul>
					    </div>
				    </a>
			    </div>
		    </div>




         <div id="approverdiv" runat="server">
		    <div class="tileview-tile-root">
			    <div id="Div1" class="tileview-tile-content">
				    <a onclick="javascript:Redirect('SummaryReport.aspx'); return false;" href="#">
					    <span>
						    <img src="../Style/Imagem/report2.png" class="tileImg" alt=""/>
					    </span>
					    <div class="tileview-tile-detailsBox">
						    <ul class="tileview-tile-detailsListMedium">
							    <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								    <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">Time Off Summary</div>
							    </li>
							    <li class="tileview-tile-descriptionMedium"></li>
						    </ul>
					    </div>
				    </a>
			    </div>
		    </div>


		   <div class="tileview-tile-root">
			    <div id="Div5" class="tileview-tile-content">
				    <a onclick="javascript:Redirect('ReportByTypes.aspx'); return false;" href="#">
					    <span>
						    <img src="../Style/Imagem/types.png" class="tileImg" alt=""/>
					    </span>
					    <div class="tileview-tile-detailsBox">
						    <ul class="tileview-tile-detailsListMedium">
							    <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								    <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">Time Off by Type</div>
							    </li>
							    <li class="tileview-tile-descriptionMedium"></li>
						    </ul>
					    </div>
				    </a>
			    </div>
		    </div>

          <div class="tileview-tile-root">
			    <div id="Div6" class="tileview-tile-content">
				    <a onclick="javascript:Redirect('StatusSummary.aspx'); return false;" href="#">
					    <span>
						    <img src="../Style/Imagem/status.png" class="tileImg" alt=""/>
					    </span>
					    <div class="tileview-tile-detailsBox">
						    <ul class="tileview-tile-detailsListMedium">
							    <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								    <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">Time Off by Status</div>
							    </li>
							    <li class="tileview-tile-descriptionMedium"></li>
						    </ul>
					    </div>
				    </a>
			    </div>
		    </div>

             <div class="tileview-tile-root">
                  <div id="Div10" class="tileview-tile-content">
				    <a onclick="javascript:Redirect('PendingRequestsApprover.aspx'); return false;" href="#">
					    <span>
						    <img src="../Style/Imagem/Cancelalert.png" class="tileImg" alt=""/>
					    </span>
					    <div class="tileview-tile-detailsBox">
						    <ul class="tileview-tile-detailsListMedium">
							    <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								    <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">My Alerts</div>
							    </li>
							    <li class="tileview-tile-descriptionMedium"></li>
						    </ul>
					    </div>
				    </a>
			    </div>			           
		     </div>

             

	    </div>
     </div>

                          </td>                         
                      </tr>
                       <tr>
                          <td >
                              <div class="app-tileview-tiles">
              <div id="divAdmin" runat="server">
               <div class="tileview-tile-root" style="text-align:center; vertical-align:central; font-weight:bolder; font-size:large; margin-top:60px;">		
                <strong> Admin Section</strong>
	          </div>
                 

               <div class="tileview-tile-root" >
			            <div id="Div9" class="tileview-tile-content" style="background-color: rgb(52, 86, 40);">
				            <a onclick="javascript:RedirectAppList('/Lists/TimeOffTypes');" href="#" >
					            <span>
						            <img src="../Style/Imagem/type.png" class="tileImg" alt=""/>
					            </span>
					            <div class="tileview-tile-detailsBox">
						            <ul class="tileview-tile-detailsListMedium">
							            <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								            <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">TimeOff Types</div>
							            </li>
							            <li class="tileview-tile-descriptionMedium"></li>
						            </ul>
					            </div>
				            </a>
			            </div>
		            </div>

                 <div class="tileview-tile-root" >
			            <div id="Div3" class="tileview-tile-content" style="background-color: rgb(246, 128, 0);">
				            <a onclick="javascript:RedirectAppList('/Lists/HolidayList');" href="#" >
					            <span>
						            <img src="../Style/Imagem/holiday.png" class="tileImg" alt=""/>
					            </span>
					            <div class="tileview-tile-detailsBox">
						            <ul class="tileview-tile-detailsListMedium">
							            <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								            <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">Holiday List</div>
							            </li>
							            <li class="tileview-tile-descriptionMedium"></li>
						            </ul>
					            </div>
				            </a>
			            </div>
		            </div>

                 <div class="tileview-tile-root" >
			            <div id="Div7" class="tileview-tile-content" style="background-color: rgb(128, 128, 128);">
				            <a onclick="javascript:RedirectAppList('/Lists/OtherTimeOffDaysList');" href="#">
					            <span>
						            <img src="../Style/Imagem/cal.png" class="tileImg" alt=""/>
					            </span>
					            <div class="tileview-tile-detailsBox">
						            <ul class="tileview-tile-detailsListMedium">
							            <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								            <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">OtherHolidayList List</div>
							            </li>
							            <li class="tileview-tile-descriptionMedium"></li>
						            </ul>
					            </div>
				            </a>
			            </div>
		            </div>

               <div class="tileview-tile-root" >
			            <div id="Div8" class="tileview-tile-content" style="background-color: rgb(46, 128, 6);">
				            <a  onclick="javascript:RedirectAppList('/Lists/ConfigList');" href="#">
					            <span>
						            <img src="../Style/Imagem/setting.png" class="tileImg" alt=""/>
					            </span>
					            <div class="tileview-tile-detailsBox">
						            <ul class="tileview-tile-detailsListMedium">
							            <li class="tileview-tile-titleMediumCollapsed" collapsed="tileview-tile-titleMediumCollapsed" expanded="tileview-tile-titleMediumExpanded">
								            <div class="tileview-tile-tileTextMediumCollapsed" collapsed="tileview-tile-tileTextMediumCollapsed" expanded="tileview-tile-tileTextMediumExpanded">Config List</div>
							            </li>
							            <li class="tileview-tile-descriptionMedium"></li>
						            </ul>
					            </div>
				            </a>
			            </div>
		            </div>


               </div>
              </div>


                          </td>                         
                      </tr>

                  </table>



       
          

    </form>
       <script src="../Scripts/SPUI.js"></script>  
    <script src="../Scripts/Common.js"></script>
     <script src="../Scripts/ChromeLoader.js"></script>
</body>
</html>
