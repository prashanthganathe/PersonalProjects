'use strict';

(function () {

    var SeatSplitDashboardBarChartController = function ($scope, $http, $filter, limitToFilter) {
        $scope.legends;
        init();
        function init() {
            $scope.locations = getLocation();
            $scope.selectedlocationsOption = $scope.locations[1];



            $scope.offices = getOffice();
            $scope.processes = getProcess();
            $scope.dashboardTypes = getDashboardType();

            getSeatSplitDetails();
            console.log($scope.info);

            $scope.SeatStatus = GetSeatStatus();
            $scope.Location = GetLocationWiseData();
            
        }

        $scope.ideas = [
   ['ideas1', 1],
   ['ideas2', 8],
   ['ideas3', 5]
        ];

        $scope.limitedIdeas = limitToFilter($scope.ideas, 3);
        /* ===========================GET/Filter Operations===============*/
        function getLocation() {
            return [
                                { id: 1, locationname: 'Location1' },
                                { id: 2, locationname: 'Location2' },
                                { id: 3, locationname: 'Location3' },
                                { id: 4, locationname: 'Location4' }
            ];
        }


        function GetSeatStatus() {
            
            return [
                { ID: 1, Name: "Seat Utilization" },
                { ID: 2, Name: "Seat Un-utilization" },
                { ID: 3, Name: "Seat with <50% Utilization" },
                { ID: 4, Name: "Cost Trend" },
                { ID: 5, Name: "Seat Swap Request" },
                { ID: 6, Name: "Vacant Seats Trend" },
                { ID: 7, Name: "Per FTE Cost Trend" },

            ];
        }


        function getOffice() {
            return [
                                { id: 1, officename: 'Office1' },
                                { id: 2, officename: 'Office2' },
                                { id: 3, officename: 'Office3' },
                                { id: 4, officename: 'Office4' }
            ];
        }

        function getProcess() {
            return [
                                { id: 1, process: 'Process1' },
                                { id: 2, process: 'Process2' },
                                { id: 3, process: 'Process3' },
                                { id: 4, process: 'Process4' }
            ];
        }
        function getDashboardType() {
            return [
                                { id: 1, dashboardtype: 'Non Seat Split' }

            ];
        }

        function GetLocationWiseData()
        {
            var loation1 = [];
            return loation1;
        }

        function getSeatSplitDetails() {

            jQuery.noConflict();

            (function ($) { // encapsulate jQuery
                $(function () {

                    $('#container').highcharts({
                        title: {
                            text: 'Dashboard Sample - Non Seat Split & Cost'
                        },
                        xAxis: {
                            categories: ['Month1', 'Month2', 'Month3', 'Month4']
                        },

                        yAxis: {
                            min: 0,
                            title: {
                                text: '%'
                            },
                            stackLabels: {
                                enabled: true,
                                style: {
                                    fontWeight: 'bold',
                                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                                }
                            }
                        },

                        plotOptions: {
                            line: {
                                dataLabels: {
                                    enabled: true
                                },
                                enableMouseTracking: false
                            }
                        },

                        series: [

                            {
                                type: 'column',
                                name: 'Seat Utilization',
                                data: [100 / 500 * 100, 400 / 500 * 100, 300 / 500 * 100, 400 / 500 * 100]
                            },



                        {
                            type: 'column',

                            name: 'Cost Trend',
                            data: [200 / 500 * 100, 200 / 500 * 100, 400 / 500 * 100, 400 / 500 * 100]
                        },

                        {
                            type: 'column',
                            name: 'Seat with <50% Utilization',
                            data: [100 / 500 * 100, 150 / 500 * 100, 250 / 500 * 100, 200 / 500 * 100]

                        },

                        {
                            type: 'spline',

                            name: 'Seat Utilization',
                            data: [200 / 500 * 100, 200 / 500 * 100, 400 / 500 * 100, 400 / 500 * 100]
                        },

                        ]

                    });

                });

            })(jQuery);

        }
    };

    /*Dummy Data has to be removed
    
    
    
    
    
    */

    SeatSplitDashboardBarChartController.$inject = ['$scope', '$http', '$filter', 'limitToFilter'];
    angular.module('RequestModule').controller('SeatSplitDashboardBarChartController', SeatSplitDashboardBarChartController);
}());