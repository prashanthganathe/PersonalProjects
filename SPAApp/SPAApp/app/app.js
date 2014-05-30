/// <reference path="E:\Learning\SPAApp\SPAApp\Scripts/angular.js" />
(function () {
    var app = angular.module("FoufsquareApp", ['ngRoute', 'ngResource', 'ui.bootstrap']);

    app.cconfig(function ($routeProvider) {

        $routeProvider.when("/explore", {
            controller: "placesExplorerController",
            templateUrl: "/app/views/placeresults.html"
        });

    });
}());