(function () {
    var requestFactory = function ($http, requestService) {

        var factory = {};
        var request, requests = null;

        factory.getRequests = function () {
            requests = requestService.query({},
                         function (data) {
                             angular.forEach(data, function (row) {

                                          });
                         });
            return requests;
         };

       

        factory.getRequest = function (id) {
            request = requestService.get({ id: id });
        };
       

        factory.saveRequest = function (request) {
            requestService.save(request);
            return true;
        }

        factory.deleteRequest = function (id) {
            requestService.delete(id);
            return true;
        }

        return factory;
    };

    requestFactory.$inject = ['$http', 'requestService'];
    angular.module('RequestModule').factory('requestFactory', requestFactory);

}());