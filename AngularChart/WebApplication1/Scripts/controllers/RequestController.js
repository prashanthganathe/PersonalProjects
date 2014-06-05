/// <reference path="../Models/requestModel.js" />
'use strict';

(function () {

    var RequestController = function ($scope,$filter, $routeParams, $location, $window){//, requestService, requestFactory) {
        $scope.requests;
      
        init();
        function init() {           
            getRequests();
        }       
        function getRequests() {            
            $scope.requests = getRequestArray(); //requestFactory.requests();
            console.log($scope.requests);
        }

        //Grid customization
        $scope.filterOptions = {
            filterText: "",
            useExternalFilter: true
        };
        $scope.totalServerItems = 0;
        $scope.pagingOptions = {
            pageSizes: [5, 10, 20],
            pageSize: 5,
            currentPage: 1
        };
        $scope.setPagingData = function (data, page, pageSize) {
            var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
            $scope.myData = pagedData;
            if ($scope.myData.length == 0) { $scope.myData = $scope.requests };
            $scope.totalServerItems = data.length;
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        };
        $scope.getPagedDataAsync = function (pageSize, page, searchText) {
            setTimeout(function () {
                var data;
                if (searchText) {
                    var ft = searchText.toLowerCase();
                    data = $scope.requests.filter(function (item) {
                        return JSON.stringify(item).toLowerCase().indexOf(ft) != -1;
                    });
                    $scope.setPagingData(data, page, pageSize);
                } else {
                    $scope.setPagingData($scope.requests, page, pageSize);
                }
            }, 100);
        };
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage); 
        $scope.$watch('pagingOptions', function (newVal, oldVal) {
            if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
            }
        }, true);
        $scope.$watch('filterOptions', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
            }
        }, true);


        $scope.getTableStyle = function () {
            var rowHeight = 40;
            var headerHeight = 20;
            return {
                //height: (10 * rowHeight + headerHeight) + "px"
                height: 245 + "px"
            };
        };


        $scope.myOptions = {
            data: 'myData',
            multiSelect: false,
            jqueryUITheme: true,
            enablePaging: true,
            showFooter: true,
            totalServerItems: 'totalServerItems',
            pagingOptions: $scope.pagingOptions,
            filterOptions: $scope.filterOptions,
            selectedItems: $scope.mySelections,
            columnDefs: [
                {
                    field: 'id',
                    displayName: 'Id',
                    cellTemplate: '<div  ng-class="" ng-click="showCustomer()" ng-bind="row.getProperty(col.field)"></div>'
                },
                {
                    field: 'Process',
                    displayName: 'process',
                    cellTemplate: '<div  ng-click="showCustomer()" ng-bind="row.getProperty(col.field)"></div>'
                },
                {
                    field: 'to',
                    displayName: 'to',
                    cellTemplate: '<div  ng-click="showCustomer()" ng-bind="row.getProperty(col.field)"></div>'
                }
            ]
        };

    };

    RequestController.$inject = ['$scope', '$filter', '$routeParams', '$location', '$window'];
    angular.module('RequestModule').controller('RequestController', RequestController);
}());