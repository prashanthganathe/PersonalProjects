var myApp = angular.module('myApp', []);

myApp.controller('MainCtrl', ['$scope', function ($scope) {
    $scope.text = 'Hello, Angular fanatic.';
}]);

myApp.controller('NavCtrl', ['$scope', function ($scope) {}]);
myApp.controller('UserCtrl', ['$scope', function ($scope) {}]);