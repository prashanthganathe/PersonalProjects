/// <reference path="../Models/assignmentrequestModel.js" />


'use strict';

(function () {

    var SeatSplitDashboardController = function ($scope, $http, $filter, limitToFilter) {

        $scope.filter = {
            searchLocation: null,
            searchOffice: null,
            searchProcess: null,
            searchDashBoardType: null,
            searchFrom: null,
            searchTo:null
        };

        //$scope.$watch('filter.searchLocation', function () {
        //    //$scope.test.searchCity = null;
        //    $scope.filter.searchOffice= null;
        //    $scope.filter.searchProcess= null,
        //    $scope.filter.searchDashBoardType= null,
        //    $scope.filter.searchFrom= null,
        //    $scope.filter.searchTo=null
        //});getDummyData




        $scope.legends;
        init();
        function init() {
            $scope.locations = getLocation();
            $scope.offices = getOffice();
            $scope.processes = getProcess();
            $scope.dashboardTypes = getDashboardType();
            DateTimePickerTo();
            DateTimePickerFrom();
            $scope.fromDate = function (date) {               
            };
            $scope.toDate = function (date) {
            };

            $scope.info = getSeatSplitDetails();
            $scope.Total = $scope.info.length;
            console.log($scope.info);
        }

       
    
        var a = $scope.info;       
        
       // var groupedByProcess = _.groupBy(a, "Process");
        // var groupedByProcess = _.indexBy(a, 'Process');

        var groupedByProcess = _.groupBy(a, function (innerArray) {
            return innerArray["Process"] + "-" + innerArray["Office"] + "-" + innerArray["Location"];  //process and office
        });

        var graphdata = function (key,val) { this.key=key; this.val=val; };
        var sdf = [];
        $scope.ideas = [];
        for (var propertyName in groupedByProcess) {

           // sdf.push(new graphdata(propertyName, groupedByProcess[propertyName].length));
            $scope.ideas.push([propertyName,	groupedByProcess[propertyName].length]);
        }

        
        //$scope.ideas = [

        
        //                   ['Process1/Office1/Location1', 12],
        //                   ['Process1/Office1/Location3', 82],
        //                   ['Process1/Office1/Location2', 51],
        //                   ['Process1/Office1/Location4', 5],
        //                   ['Process1/Office1/Location5', 151],
        //                   ['Process1/Office1/Location6', 151]
        //];

        $scope.limitedIdeas = limitToFilter($scope.ideas, $scope.ideas.length);
      
        function DateTimePickerFrom()
        {
            $('#datepickerFrom').datepicker().on('changeDate', function (ev) {
                var element = angular.element($('#datepickerFrom'));
                var controller = element.controller();
                var scope = element.scope();

                scope.$apply(function () {
                    scope.fromDate(ev.date);
                });
            });
        }

        function DateTimePickerTo() {
            $('#datepickerTo').datepicker().on('changeDate', function (ev) {
                var element = angular.element($('#datepickerTo'));
                var controller = element.controller();
                var scope = element.scope();

                scope.$apply(function () {                    
                    scope.toDate(ev.date);                   
                });
            });
        }

        $("datepickerTo").blur(function () {
            alert('dddd');
        });
       
      
        function getLocation() {
            return [
                             
                               {  locationname: 'Location1' },
                               {  locationname: 'Location2' },
                               {  locationname: 'Location3' }
                               
            ];
        }

        function getOffice() {
            return [
                               
                                {  officename: 'Office1' },
                                {  officename: 'Office2' },
                                {  officename: 'Office3' },
                                {  officename: 'Office4' },
                                {  officename: 'Office5' }
            ];
        }

        function getProcess() {
            return [
                               
                                {  process: 'Process1' },
                                {  process: 'Process2' },
                                {  process: 'Process3' }
                              
            ];
        }
        function getDashboardType() {
            return [
                                {  dashboardtype: 'Split Seat' },
                                {  dashboardtype: 'Hot Seat' },
                                {  dashboardtype: 'Blocked Seat' },
                                {  dashboardtype: 'Cold Seat' }
                             
            ];
        }


        function getSeatSplitDetails() {             
            var templegend = new assignmentrequestModel();
            return templegend.getDummyData();           
        }


        /* ===========================Filter Operations===============*/

        $scope.filterDateAdded = function () {

            $scope.dateFilter = function () {
                var result = [];
                for (var i in $scope.item) {
                    if ($scope.item[i].dateAdded > $scope.search) {
                        result.push($scope.item[i]);
                    }

                }
                return result;
            };
        }
        //scope.$watch("items", function (newValue) {
        //    chart.series[0].setData(newValue, true);
        //}, true);

        //$scope.customFilter = function (data) {
        //    if (data.rating === $scope.filterItem.store.rating) {
        //        return true;
        //    } else if ($scope.filterItem.store.rating === 6) {
        //        return true;
        //    } else {
        //        return false;
        //    }
        //};
        /* ===========================END of Filter Operations===============*/
    };

   


    SeatSplitDashboardController.$inject = ['$scope', '$http', '$filter','limitToFilter'];
    angular.module('RequestModule').controller('SeatSplitDashboardController', SeatSplitDashboardController);
}());