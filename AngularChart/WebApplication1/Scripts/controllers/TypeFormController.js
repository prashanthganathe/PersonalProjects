'use strict';

(function () {

    var TypeFormController = function ($scope, $http, $filter) {//, requestService, requestFactory) {


        $scope.users = [
                        { id: 1, name: 'awesome user1', status: 2, group: 4, groupName: 'admin' ,address:'343/B, ABC Nagar, Silk City, ANJ 56003'},
                        { id: 2, name: 'awesome user2', status: undefined, group: 3, groupName: 'vip', address: '343/B, ABC Nagar, Silk City, ANJ 56003' },
                        { id: 3, name: 'awesome user3', status: 2, group: null, address: '343/B, ABC Nagar, Silk City, ANJ 56003' }
        ];


        $scope.statuses = [
                           { value: 1, text: 'status1' },
                           { value: 2, text: 'status2' },
                           { value: 3, text: 'status3' },
                           { value: 4, text: 'status4' }
        ];

        $scope.groups = [];       
        $scope.loadGroups = function () {
            //return $scope.groups.length ? null : $http.get('/groups').success(function (data) {
            //    $scope.groups = data;
            //});
            $scope.groups = [
                           { value: 1, text: 'group1' },
                           { value: 2, text: 'group2' },
                           { value: 3, text: 'group3' },
                           { value: 4, text: 'group4' }
            ];
            return $scope.groups;
        };


        $scope.showGroup = function (user) {
            if (user.group && $scope.groups.length) {
                var selected = $filter('filter')($scope.groups, { id: user.group });
                return selected.length ? selected[0].text : 'Not set';
            } else {
                return user.groupName || 'Not set';
            }
        };

        $scope.showStatus = function (user) {
            var selected = [];
            if (user.status) {
                selected = $filter('filter')($scope.statuses, { value: user.status });
            }
            return selected.length ? selected[0].text : 'Not set';
        };


        $scope.checkName = function (data, id) {
            if ( data !== 'awesome') {
                return "should be `awesome`";
            }
        };


        $scope.saveUser = function (data, id) {
            //$scope.user not updated yet
            angular.extend(data, { id: id });
          
         //   return $http.post('/saveUser', data);
            // return $scope.users.push(data);
            return $scope.users;
        };

        // remove user
        $scope.removeUser = function (index) {
            $scope.users.splice(index, 1);
        };

        $scope.addUser = function () {           
            $scope.inserted = {
                id: $scope.users.length + 1,
                name: '',
                status: null,
                group: null
                
            };
            $scope.users.push($scope.inserted);
        };


    }



    TypeFormController.$inject = ['$scope', '$http' ,'$filter'];
    angular.module('RequestModule').controller('TypeFormController', TypeFormController);
}());

