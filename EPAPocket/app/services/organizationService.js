'use strict';
app.factory('organizationService', ['$http', '$q', 'ngAuthSettings', '$rootScope', 'orderService', function ($http, $q, ngAuthSettings, $rootScope, orderService) {



    var serviceFactory = {};



    var _save = function (entity) {
        var deferred = $q.defer();
        $http.post($rootScope.serviceUrl + 'odata/organizations/save', entity).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {

            deferred.reject(Exceptions.getMessage(err));
        });

        return deferred.promise;
    };
    var _delete = function (entity) {
        var deferred = $q.defer();
        $http.post($rootScope.serviceUrl + 'odata/organizations/delete', entity).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {

            deferred.reject(Exceptions.getMessage(err));
        });

        return deferred.promise;
    };

    serviceFactory.getOrders = orderService._getOrders;
    serviceFactory.save = _save;
    serviceFactory.delete = _delete;
    return serviceFactory;

}]);