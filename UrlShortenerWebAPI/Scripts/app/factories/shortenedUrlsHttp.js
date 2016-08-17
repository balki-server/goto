angular.module("urlShortenerModule").factory("shortenedUrlsHttp", ["$http", "appConfigs", function ($http, appConfigs) {
    var factory = { };
    
    factory.getAll = function (params) {
        factory.baseUrl = appConfigs.urlShortenerWebAPIURL + "/odata/ShortenedUrls";
        return $http.get(factory.baseUrl, { params: params });
    };

    factory.create = function (shortenedUrlObj) {
        factory.baseUrl = appConfigs.urlShortenerWebAPIURL + "/odata/ShortenedUrls";
        return $http.post(factory.baseUrl, shortenedUrlObj);
    };

    factory.delete = function (shortenedUrlId) {
        factory.baseUrl = appConfigs.urlShortenerWebAPIURL + "/odata/ShortenedUrls";
        return $http.delete(factory.baseUrl + "(" + shortenedUrlId + ")");
    };

    factory.patch = function (shortenedUrlObj) {
        factory.baseUrl = appConfigs.urlShortenerWebAPIURL + "/odata/ShortenedUrls";
        return $http.patch(factory.baseUrl + "(" + shortenedUrlObj.Id + ")", shortenedUrlObj);
    };

    return factory;
}]);