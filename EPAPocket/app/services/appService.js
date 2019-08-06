'use strict';
app.factory('appService', ['$http', '$q',  '$rootScope', function ($http, $q , $rootScope) {



    var serviceFactory = {};

    //var _getEmployee = function (nid, cid) {


    //    var deferred = $q.defer();
    //    $http.get(serviceBase + 'odata/employee/nid/' + cid + '/' + nid).then(function (response) {
    //        deferred.resolve(response.data);
    //    }, function (err, status) {

    //        deferred.reject(Exceptions.getMessage(err));
    //    });

    //    return deferred.promise;
    //};
    

    //var _save = function (entity) {
    //    var deferred = $q.defer();
    //    $http.post($rootScope.serviceUrl + 'odata/employee/save', entity).then(function (response) {
    //        deferred.resolve(response.data);
    //    }, function (err, status) {

    //        deferred.reject(Exceptions.getMessage(err));
    //    });

    //    return deferred.promise;
    //};
    

   // serviceFactory.getEmployee = _getEmployee;
     
   // serviceFactory.save = _save;
    
    return serviceFactory;

}]);