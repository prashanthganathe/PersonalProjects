

(function () {
    var customerController = function ($scope,$http) {
        //$scope.customers = [
        //            { Name: 'Name1', Age: '24', City: 'NY', Date: '2012-2-21' },
        //            { Name: 'Name2', Age: '22', City: 'AY', Date: '2013-2-21' },
        //            { Name: 'Name3', Age: '23', City: 'BY', Date: '2014-2-21' },
        //            { Name: 'Name4', Age: '25', City: 'CY', Date: '2010-2-21' }
        //];

        // http://localhost:1745/api/default/

        $http.get('http://localhost:1745/api/default/').
                                                       success(function (data) {
                                                           debugger;
                                                           $scope.customers = data;
                                                       });
    };

    customerController.$inject = ['$scope', '$http'];
    angular.module('customerApp').controller('customerController', customerController);
}());



