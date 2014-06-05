/// <reference path="E:\OfficeDashboard\OfficeDashboard\WebApplication1\bower_components/angular/angular.js" />



(function () {
    var appmodule = angular.module("RequestModule", ['ngResource', 'ngRoute', 'xeditable', 'ui.bootstrap']);
 
    appmodule.constant('_', window._).run(function ($rootScope) {
        $rootScope._ = window._;
    });
    
    appmodule.config(function ($routeProvider) {  

        $routeProvider.when("/", {
                        controller: '',
                        templateUrl: 'views/default.html'
        });

        $routeProvider.when("/request", {
            controller: 'RequestController',
            templateUrl: 'views/request.html'
        });

        $routeProvider.when("/legend", {
            controller: 'LegendController',
            templateUrl: 'views/legend.html'
        });

        $routeProvider.when("/seatsplitdash", {
            controller: 'SeatSplitDashboardController',
            templateUrl: 'views/seatSplitDashboard.html'
        });

        $routeProvider.when("/seatsplitDashboardBarChart", {
            controller: 'SeatSplitDashboardBarChartController',
            templateUrl: 'views/seatsplitDashboardBarChart.html'
        });

        

        $routeProvider.when("/userform", {
            controller: 'UserFormController',
            templateUrl: 'views/userform.html'
        });

        $routeProvider.when("/typeform", {
            controller: 'TypeFormController',
            templateUrl: 'views/typeform.html'
        });

        $routeProvider.otherwise({ redirectTo: "/" });


       

    });

    //angular.module("RequestModule").directive('hcPie', function () {
    //    return {
    //        restrict: 'C',
    //        replace: true,
    //        scope: {
    //            items: '='
    //        },
    //        controller: function ($scope, $element, $attrs) {
    //            console.log(2);

    //        },
    //        template: '<div id="container" style="margin: 0 auto">not working</div>',
    //        link: function (scope, element, attrs) {
    //            console.log(3);
    //            var chart = new Highcharts.Chart({
    //                chart: {
    //                    renderTo: 'container',
    //                    plotBackgroundColor: null,
    //                    plotBorderWidth: null,
    //                    plotShadow: false
    //                },
    //                title: {
    //                    text: 'Browser market shares at a specific website, 2010'
    //                },
    //                tooltip: {
    //                    pointFormat: '{series.name}: <b>{point.percentage}%</b>',
    //                    percentageDecimals: 1
    //                },
    //                plotOptions: {
    //                    pie: {
    //                        allowPointSelect: true,
    //                        cursor: 'pointer',
    //                        dataLabels: {
    //                            enabled: true,
    //                            color: '#000000',
    //                            connectorColor: '#000000',
    //                            formatter: function () {
    //                                return '<b>' + this.point.name + '</b>: ' + this.percentage + ' %';
    //                            }
    //                        }
    //                    }
    //                },
    //                series: [{
    //                    type: 'pie',
    //                    name: 'Browser share',
    //                    data: scope.items
    //                }]
    //            });


    //            scope.$watch("items", function (newValue) {
    //                chart.series[0].setData(newValue, true);
    //            }, true);
    //        }
    //    }
    //});

    appmodule.run(function ($rootScope) {
        // you can inject any instance here
        $rootScope.baseUrl = "http://localhost:12973/";//api/request
    });

    appmodule.run(function (editableOptions) {
      editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
    });
    
  



}());