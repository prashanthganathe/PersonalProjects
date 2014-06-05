'use strict';

angular.module('RequestModule')
    .service('customerservice', function ($resource, $rootScope) {
        var customer = $resource($rootScope.baseUrl + 'customers/:id.json', { id: '@id' }, {
            update: {
                method: 'PUT',
                data: { customer: '@customer' },
                isArray: false
            },
            save: {
                method: 'POST'
            },
            remove: {
                method: 'DELETE'
            }
        })
        return customer;
    });