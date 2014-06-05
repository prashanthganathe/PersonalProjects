'use strict';

(function () {

    var LegendController = function ($scope, $http, $filter) {
        $scope.legends;
        init();
        function init() {
            $scope.legends = getLegends();
            console.log($scope.legends);
        }

        /* ===========================GET/Filter Operations===============*/
        function getLegends() {
            var legendlist = [];
                    //Static
                    var templegend;
                    for (var i = 1; i < 6; i++) {
                    
                        templegend = new legendModel();
                        templegend.id = i;
                        templegend.legendName='LegendName'+i;
                        legendlist.push(templegend);
                    }
                    return legendlist;
            //implementation
            //TODO
        }
        /* ===============END OF GET/Filter Operations======================*/
       


        /* ===========================CRUD Operations===============*/
        $scope.save = function (data, id) {
            //$scope.user not updated yet
            debugger;
            angular.extend(data, { id: id });           
            return $scope.legends;
        };


        $scope.remove = function (index) {
            $scope.legends.splice(index, 1);
            $scope.totalItems = $scope.legends.length;
        };

        $scope.add = function () {

            var templegend = new legendModel();
            templegend.id = $scope.legends.length + 1;
            templegend.legendName = '';

            $scope.inserted = templegend;
            //$scope.inserted = {
            //    id: $scope.legends.length + 1,
            //    legendName: ''
            //};
           // $scope.legends.push($scope.inserted);

            var pagecount = $scope.legends.length % 5;
            if (pagecount >= 4) // if 3 which is before new page add new page
                $scope.noOfPages++;
            $scope.legends.push($scope.inserted);
            $scope.totalItems = $scope.legends.length;
        };

        /*================Pagination at initialization===============*/
        $scope.totalItems = $scope.legends.length;
        $scope.totalItems = $scope.legends.length;
        $scope.currentPage = 1;
        $scope.setPage = function (pageNo) {
            $scope.currentPage = pageNo;
        };
        /*================End of Pagination at initialization===============*/
        /* ===========================End of CRUD Operations===============*/




        /* ===============VALIDATION======================*/
        $scope.CheckLegendName = function (data, id) {
            
            //if (data.trim() == '') {
            //    return "Please fill legend Name";
            //}
        };
        /* ===============END OF VALIDATION======================*/

        /*======================filter=================*/
  

        
    };

    //angular.module('RequestModule').filter('startFrom', function () {
    //    return function (input, start) {
    //        start = +start; //parse to int
    //        return input.slice(start);
    //    }
    //});

    LegendController.$inject = ['$scope', '$http', '$filter'];
    angular.module('RequestModule').controller('LegendController', LegendController);
}());
