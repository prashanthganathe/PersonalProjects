// Function to retrieve a query string value.
// For production purposes you may want to use
//  a library to handle the query string.
function getQueryStringParameter(paramToRetrieve) {
    var params =
        document.URL.split("?")[1].split("&");
    var strParams = "";
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramToRetrieve)
            return singleParam[1];
    }
}

function Redirect(page) {
    var SPHostUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
    var urldomain = document.URL.split("/Pages")[0];
    var filterquerystring = window.location.search.replace('success=1&', '');
    filterquerystring = filterquerystring.replace('success=1', '');
    window.location = urldomain + "/Pages/" + page + filterquerystring;
}

$(function () {
    $("document").on('.page-header-content', 'click', function () {
        var SPHostUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
        var urldomain = document.URL.split("/Pages")[0];
        var filterquerystring = window.location.search.replace('success=1&', '');
        filterquerystring = filterquerystring.replace('success=1&', '');
        window.location = urldomain + "/Pages/ui.aspx" + filterquerystring;
    });
});