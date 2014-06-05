'use strict';

(function () {

    angular.module('RequestModule').directive('hcPie', function () {
        return {
            restrict: 'C',
            replace: true,
            scope: {
                items: '='
            },
            controller: function ($scope, $element, $attrs) {
                console.log(2);
            },
            template: '<div id="container" style="margin: 0 auto">not working</div>',
            link: function (scope, element, attrs) {
                console.log(3);
                var chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: true,
                        options3d: {
                            enabled: true,
                            alpha: 65,
                            beta: 0
                        }
                       
                    },
                    title: {
                        text: 'Seat Split Dashboard'
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.percentage}%</b>',
                        percentageDecimals: 1
                    },
                    plotOptions: {
                        pie: {
                            depth: 35,
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                color: '#000000',
                                connectorColor: '#000000'/*,
                                formatter: function () {
                                    return '<b>' + this.point.name + '</b>: ' + this.percentage + ' %';
                                }*/
                            }
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: 'Split Seat info',
                        data: scope.items,
                        showInLegend: true
                    }]
                });


                scope.$watch("items", function (newValue) {
                    chart.series[0].setData(newValue, true);
                }, true);
            }
        }
    });
}());