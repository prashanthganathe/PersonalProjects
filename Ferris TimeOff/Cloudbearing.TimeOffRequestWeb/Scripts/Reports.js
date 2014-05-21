$(document).ready(function () {
    $.ajaxSetup({ cache: false });
    var icons = {
        header: "ui-icon-circle-arrow-e",
        activeHeader: "ui-icon-circle-arrow-s"
    };

    $("#accordion").accordion({
        icons: icons,
        active: false,
        collapsible: true,
    });

    $("#toggle").button().click(function () {
        if ($("#accordion").accordion("option", "icons")) {
            $("#accordion").accordion("option", "icons", null);
        } else {
            $("#accordion").accordion("option", "icons", icons);
        }
    });

    $(document).ajaxStart(function () {
        $(this).html("<img src='loading45.gif' />");
    });

    function ExpandChild(e) {
        $(e).append("<p>Test</p>");
    }

    $('.ui-icon-circle-arrow-e').on("click",function () {
        $(this).removeClass("ui-icon-circle-arrow-e");
        $(this).addClass("ui-icon-circle-arrow-s");
        ExpandChild(this);
    });

    function GetDate(date) {
        var MyDate_String_Value = date;
        var value = new Date(parseInt(MyDate_String_Value.replace(/(^.*\()|([+-].*$)/g, '')));
        var dat = value.getMonth() + 1 + "/" + value.getDate() + "/" + value.getFullYear();
        return dat;
    }
    function ShowYearDetails(type,e, yrDetails) {
       // var div = "<table class='yrDetails' style=' width:auto; padding-left:30px;'>";
        // var a;
        var div = "<div>";
        $.each(yrDetails, function (index, value) {
            var classname = "";
                if (type == "GetEmployeeDetailsByYear") {
                    a = value.RequestedBy;
                    classname = "summary";
                }
                if (type == "GetDetailsByYearType") {
                    a = value.TimeOffType;
                    classname = "ByType";
                }
                if (type == "GetDetailsByYearStatus") {
                    a = value.Status;
                    classname = "ByStatus";
                }



                div += "<h3 class='" + classname + " ui-accordion-header ui-helper-reset ui-state-default ui-corner-all ui-accordion-icons' role='tab' id='ui-accordion-accordion-header-1' aria-controls='ui-accordion-accordion-panel-1' aria-selected='false' tabindex='-1'><span class='ui-accordion-header-icon ui-icon ui-icon-circle-arrow-e  restruct'></span>";
                div += " <span>" + a + " <div style='text-align:right;'>" + value.TotalHours + "</div></span>";
            div+=" </h3>";

      
        //        div += "<tr>";
        //        div += "<td><div style=' width:100px;' ><span class='ui-accordion-header-icon ui-icon ui-icon-circle-arrow-e'></span><span>" + a + "</span></div></td>";
        //        div += "<td><div style=' width:40px;' ></div></td>";
        //        div += "<td><div style=' width:100px;' ></div></td>";
        //        div += "<td><div style=' width:100px;' ></div></td>";
        //        div += "<td><div style=' width:100px;' ></div></td>";
        //        div += "<td><div style=' width:150px; padding-left:40px;' ></div>   </td>";
        //        div += "<td><div style=' width:100px;' ></div></td>";
        //        div += "<td><div style=' width:150px;' ></div></td>";
        //        div += "<td><div style=' width:100px;' ></div></td>";//" + value.FullDayCount + "
        //        div += "<td><div style=' width:100px;' >" + value.TotalHours + "</div></td>";
        //        div += "</tr>";
            });
        //div += "</table>";


            $($(e).next()[0]).css('height', 'auto');
            $(e).next().html(div);
       
    }


    //On Year Click
    function GetYearDetails(type,e) {
        $(e).next().innerHTML = " <img src='loading.gif' id='img' style='display:none' />";
        var year = "year";
        var value = e.text();
        var dataObj = {};
        dataObj[year] = value;

        var url ;
        if (type == "GetEmployeeDetailsByYear")
            url = "AllTimeOffRequests.aspx/" + type;
        if (type == "GetDetailsByYearType")
            url = "AllTimeOffRequestsType.aspx/" + type;
        if (type == "GetDetailsByYearStatus")
            url = "AllTimeOffRequestsStatus.aspx/" + type;
        
        $.ajax({
            url: url,
            data: dataObj,
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (result) {
                ShowYearDetails(type,e, result.d);
                $('#img').hide();
            },
            error: function (request, status, error) {
            }
        });
    }

    $(".year").on("click", function (e) {
        e.preventDefault();
        $('#img').show();
        $($(this).next()[0]).css('height', 'auto');
        if ($(this).next().html() == "") {
            GetYearDetails("GetEmployeeDetailsByYear", $(this));
        }
    });

    $(".type").on("click", function (e) {
        e.preventDefault();
        $('#img').show();
        $($(this).next()[0]).css('height', 'auto');
        if ($(this).next().html() == "") {
            GetYearDetails("GetDetailsByYearType", $(this));
        }
    });

    $(".status").on("click", function (e) {
        e.preventDefault();
        $('#img').show();
        $($(this).next()[0]).css('height', 'auto');
        if ($(this).next().html() == "") {
            GetYearDetails("GetDetailsByYearStatus", $(this));
        }
    });

});