function CustomersController($scope)
{
  $scope.sortyBy='name';   
  $scope.customers =
  [
  {name:'one', city:'city1',joined:'2000-01-12',orderTotal:'34'},
  {name:'two', city:'city2',joined:'2000-01-12',orderTotal:'134'},
  {name:'three', city:'city3',joined:'2000-01-12',orderTotal:'734'},
  {name:'four', city:'city4',joined:'2000-01-12',orderTotal:'3'}
  ];
      $scope.doSort= function(propname){
          $scope.sortyBy=propname;
          $scope.reverse=!$scope.reverse;
      };
}