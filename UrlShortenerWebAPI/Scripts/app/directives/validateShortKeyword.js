angular.module("urlShortenerModule").directive('validateShortKeyword', ["shortenedUrlsHttp", "appConfigs", function (shortenedUrlsHttp, appConfigs){ 
   return {
      require: 'ngModel',
      link: function ($scope, elem, attr, ngModel) {
          ngModel.$parsers.unshift(function (value) {
              shortenedUrlsHttp.getAll({$filter: "ShortKeyword eq '" + value.toLowerCase() + "'"})
                               .success(function(data){
                                   if (data.value.length > 0) {

                                       $scope.shortenedUrlId = data.value[0].Id;
                                       if (appConfigs.isAdmin) {
                                           $scope.createButtonLabel = "Update";
                                           $scope.url = data.value[0].ActualUrl;
                                           ngModel.$setValidity('validateShortKeyword', true);
                                       }
                                       else {
                                           $scope.createButtonLabel = "Create";
                                           ngModel.$setValidity('validateShortKeyword', false);
                                       }
                                   } else {
                                       $scope.createButtonLabel = "Create";
                                       ngModel.$setValidity('validateShortKeyword', true);
                                   }
                               });            
             //ngModel.$setValidity('blacklist', blacklist.indexOf(value) === -1);
             return value;
          });
      }
   };
}]);