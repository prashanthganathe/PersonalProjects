(function () {

    var CustomerController = function ($scope, foo, bar) {
        $scope.customers = {

        };
        $scope.sortBy = 'name';
        $scope.doSort = function (prop) {

        };
    };

    CustomersControllers.$inject = ['$Scope', 'foo', 'bar'];
    angular.module('customerApp')
      .controller('CustomerController', CustomerController);
});