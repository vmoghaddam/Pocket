 
var app = angular.module('EPAPocketApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'dx', 'ngSanitize', 'ngAnimate' ]).config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = false;
}]);
 

 



app.config(function ($routeProvider) {
    var version = 0.9;
    
    $routeProvider.when("/test", {
        controller: "testController",
        templateUrl: "/app/views/test.html?v="+version
    });
    

    
    $routeProvider.otherwise({ redirectTo: "/home" });

});   

 
 
 

 var serviceBase = 'http://localhost:58908/';
 var webBase = 'http://localhost:30273/';

app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

app.config(['$httpProvider', function ($httpProvider) {

    $httpProvider.interceptors.push('authInterceptorService');
}]);

app.run(['authService',  '$rootScope', '$location', '$templateCache', function (authService,  $rootScope, $location, $templateCache) {
 
    
    $rootScope.$on('$viewContentLoaded', function () {
        $templateCache.removeAll();
        
    });


    $rootScope.serviceUrl = serviceBase;
    $rootScope.fileHandlerUrl = webBase + 'filehandler.ashx';
    $rootScope.clientsFilesUrl = webBase + 'upload/clientsfiles/';
    $rootScope.app_title = 'سیستم یکپارچه پردازش';
    $rootScope.page_title = '';
    $rootScope.app_remark = 'Lorem ipsum dolor sit amet';
    $rootScope.module = 'آموزش همراه';
    $rootScope.moduleId = 1;
    $rootScope.moduleRemark = '';
    $rootScope.theme = '';
    $rootScope.color = '';
    $rootScope.class = '';
    $rootScope.userName = '';
    $rootScope.userTitle = '';
    $rootScope.userId = null;
    $rootScope.employeeId = null;
    $rootScope.logOut = function () { authService.logOut(); };
    $rootScope.apps = function () { $location.path('/apps'); };
    $rootScope.menu = function () {


        $('._menu').hide();
        $('#module' + $rootScope.moduleId).show();
        document.getElementById("mySidenav").style.width = "100%";
    };
    $rootScope.closeMenu = function () {
        document.getElementById("mySidenav").style.width = "0";
    };
    $rootScope.navigate = function (target, key) {

        //var rec = Enumerable.From(Config.MenuItems).Where('$.moduleId==' + $rootScope.moduleId + ' && $.key=="' + key + '"').FirstOrDefault();
       // activityService.hitMenu(key, target, 'Visiting ' + $rootScope.module + ' > ' + rec.title);

        $location.path(target);


    };
    $rootScope.headerClasses = ['app-headerx', 'wrapper-bubble', 'col-lg-12', 'col-md-12', 'col-sm-12', 'col-xs-12'];
    $rootScope.headerClasses.push('theme-blue');
    Config.CustomerId = 1;
    authService.fillAuthData();
    authService.fillModuleData();

    $rootScope.setTheme = function () {
        DevExpress.ui.themes.current($rootScope.theme);
        $rootScope.headerClasses = ['app-headerx', 'wrapper-bubble', 'col-lg-12', 'col-md-12', 'col-sm-12', 'col-xs-12'];
        $rootScope.headerClasses.push($rootScope.class);

    };



    //$rootScope.setTheme();
    $rootScope.history = [];


    
    //DevExpress.ui.themes.current('material.teal-light');

    $rootScope.$on('$routeChangeSuccess', function () {
        $rootScope.history.push($location.$$path);

    });

    $rootScope.getWindowSize = function () {

        var w = $(window).width();
        var h = $(window).height();


        return { width: w, height: h };
    };
    $rootScope.popupHeightFull = function (a, fullscreen) {
        if (!fullscreen)
            return $jq(window).height() * a;
        else
            return $jq(window).height();
    };
    $rootScope.popupHeight = function (h, mobileFull) {
        if (mobileFull) {
            return h;
        }

        return h;
    };
    $rootScope.popupWidth = function (w, fullscreen) {
        if (!fullscreen)
            return w;
        else
            return w;//$jq(window).width();
    };
    $rootScope.navigate2 = function (target, key, module) {
 
        $location.path(target);


    };
   
    $rootScope.dg_filterRow = {
        visible: true,
        showOperationChooser: true,
        operationDescriptions: { '=': 'مساوی', '<>': 'نامساوی', '<': 'کوچکتر', '<=': 'کوچکتر یا مساوی', '>': 'بزرگتر', '>=': 'بزرگتر یا مساوی', 'startsWith': 'شروع شود', 'contains': 'شامل', 'notContains': 'بدون', 'endsWith': 'خاتمه یابد', 'between': 'بین', 'equal': 'مساوی با', 'notEqual': 'نامساوی با' },
        resetOperationText: 'لغو'

    };
    //////////////////////////////////////////////////////
}]);
 
 
 