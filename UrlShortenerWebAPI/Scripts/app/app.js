angular.module("urlShortenerModule", ["kendo.directives", "ngCookies"]);
angular.module("urlShortenerModule").config(["$controllerProvider", "$provide", "$compileProvider", "$locationProvider",
function ($controllerProvider, $provide, $compileProvider, $locationProvider) {
    
    console.log("Config method executed.");
    // Let's keep the older references.
    var urlShortenerModule = angular.module("urlShortenerModule");
    urlShortenerModule._controller = urlShortenerModule.controller;
    urlShortenerModule._service = urlShortenerModule.service;
    urlShortenerModule._factory = urlShortenerModule.factory;
    urlShortenerModule._value = urlShortenerModule.value;
    urlShortenerModule._directive = urlShortenerModule.directive;
    // Provider-based controller.
    urlShortenerModule.controller = function (name, constructor) {
        $controllerProvider.register(name, constructor);
        return (this);
    };
    // Provider-based service.
    urlShortenerModule.service = function (name, constructor) {
        $provide.service(name, constructor);
        return (this);
    };
    // Provider-based factory.
    urlShortenerModule.factory = function (name, factory) {
        $provide.factory(name, factory);
        return (this);
    };
    // Provider-based value.
    urlShortenerModule.value = function (name, value) {
        $provide.value(name, value);
        return (this);
    };
    // Provider-based directive.
    urlShortenerModule.directive = function (name, factory) {
        $compileProvider.directive(name, factory);
        return (this);
    };

    
    urlShortenerModule.value("appConfigs", {});
    urlShortenerModule.value("appUser", {});
    // NOTE: You can do the same thing with the "filter"
    // and the "$filterProvider"; but, I don't really use
    // custom filters.                                           
}]).run(["$location", "$http", "$q", function ($location, $http, $q) {   
}]);

angular.module("urlShortenerModule").controller("appCtrl", ["$rootScope", "$scope", "$timeout", "$cookies", "appConfigs", "appUser",
    function ($rootScope, $scope, $timeout, $cookies, appConfigs, appUser) {
        $scope.init = function (options) {
            if (options) {
                appConfigs.urlShortenerWebAPIURL = options.urlShortenerWebAPIURL;
                appConfigs.isAdmin = options.isAdmin;
            }
        };
}]);

angular.module("urlShortenerModule").factory('authHttpResponseInterceptor', ['$q', '$location', "$cookies", "$rootScope", "appConfigs",
 function ($q, $location, $cookies, $rootScope, appConfigs) {
     return {
         request: function (config) {
             // do something on success
             //config.headers["Authorization"] = "Bearer " + $cookies.get("accessTokenUrlShortenerWebApi");
             config.headers["X-Requested-With"] = "XMLHttpRequest";
             
             return config;
         },
         response: function (response) {
             return response || $q.when(response);
         },
         responseError: function (rejection) {
             if (rejection.status === 401) {
                 location.href =appConfigs.urlShortenerWebAPIURL +  '/Account/LogOff';
             }
             return $q.reject(rejection);
         }
     }
 }])
.config(['$httpProvider', function ($httpProvider) {    
    $httpProvider.interceptors.push('authHttpResponseInterceptor');

}]);