/// <reference path="../Models/requestModel.js" />
/// <reference path="../Models/resourceModel.js" />

'use strict';

(function () {
    var requestService = function ($resource, $rootScope) {


        //var requests = [
        //    {
        //        id:'1',
        //        location: 'New Delhi',
        //        office: '2000-12-02',
        //        numberofSeats: 'John',
        //        seatType: 'Cabin',
        //        process:'process1',
        //        from:'2012-12-02',
        //        to:'2013-12-02',
        //        projectCode :'Project001'
              
        //    },
        //    {
        //        id: '2',
        //        location: 'Mumbai',
        //        office: '2000-12-02',
        //        numberofSeats: 'John',
        //        seatType: 'L-Shaped',
        //        process: 'process1',
        //        from: '2012-12-02',
        //        to: '2013-12-02',
        //        projectCode: 'Project002'

        //    },
        //    {
        //        id: '3',
        //        location: 'New Delhi',
        //        office: '2000-12-02',
        //        numberofSeats: 'John',
        //        seatType: 'Cabin',
        //        process: 'process2',
        //        from: '2012-12-02',
        //        to: '2013-12-02',
        //        projectCode: 'Project001'

        //    }          
           
        //];

        var request;
        //request = $resource($rootScope.baseUrl + 'requests/:id.json', { id: '@id' },
        //                        {
        //                        update: {
        //                            method: 'PUT',
        //                            data: { request: '@request' },
        //                            isArray: false
        //                        },
        //                        save: {
        //                            method: 'POST'
        //                        },
        //                        remove: {
        //                            method: 'DELETE'
        //                        }
        //                    })

        request = getRequestArray();

       
    };

    requestService.$inject = ['$resource', '$rootScope'];
    angular.module('RequestModule').service('requestService', requestService);



}());