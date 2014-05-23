var customersApp= angular.module('customersApp', ['ngGrid']);
 customersApp.controller('customerCtrl', function ($scope,userRepository) { 
   getCustomers();
   function getCustomers()
     {
          customerRepository.getCustomers(function(results) {
               $scope.customerData=results;
          }
     }
 })   