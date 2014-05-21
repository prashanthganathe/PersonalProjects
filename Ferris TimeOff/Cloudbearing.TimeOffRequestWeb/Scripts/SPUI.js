


$(document).ready(function () {

    $(".tileview-tile-content").mouseenter(function () {
        var titleMedium = $(this).find(".tileview-tile-detailsBox li:first");
        if (titleMedium !== null) {
            titleMedium.removeClass(titleMedium.attr('collapsed')).addClass(titleMedium.attr('expanded'));
            var titleText = titleMedium.children();
            if (titleText !== null) {
                titleText.removeClass(titleText.attr('collapsed')).addClass(titleText.attr('expanded'))
            }
        }
        $(this).find(".tileview-tile-detailsBox").stop(true, false).animate({ top: "0px" }, 300);
    }).mouseleave(function () {
        var titleMedium = $(this).find(".tileview-tile-detailsBox li:first");
        if (titleMedium !== null) {
            titleMedium.removeClass(titleMedium.attr('expanded')).addClass(titleMedium.attr('collapsed'));
            var titleText = titleMedium.children();
            if (titleText !== null) {
                titleText.removeClass(titleText.attr('expanded')).addClass(titleText.attr('collapsed'))
            }
        }
        $(this).find(".tileview-tile-detailsBox").stop(true, false).animate({ top: "100px" }, 300);
    });


    $("#divmsg").hide();    
    var success = decodeURIComponent(getQueryStringParameter("success"));
    if (success != undefined) {
        if (success == "1")
            $("#divmsg").fadeIn('slow').animate({ opacity: 1.0 }, 4500).effect("pulsate", { times: 2 }, 800).fadeOut('slow');
        else {
            if (success == "0") {
                $('#divmsg').html("Unable to save the request, Please try again.");
                $("#divmsg").fadeIn('slow').animate({ opacity: 1.0 }, 4500).effect("pulsate", { times: 2 }, 800).fadeOut('slow');
            }
        }
    }
    //Reports
   

});