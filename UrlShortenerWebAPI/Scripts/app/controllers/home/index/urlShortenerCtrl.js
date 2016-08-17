angular.module("urlShortenerModule").controller("urlShortenerCtrl", ["$scope", "shortenedUrlSchema", "shortenedUrlsHttp", "appConfigs",
    function ($scope, shortenedUrlSchema, shortenedUrlsHttp, appConfigs) {

    $scope.gridColumns = shortenedUrlSchema.getGridColumns();
    $scope.gridDS = shortenedUrlSchema.getGridDS();
    $scope.createButtonLabel = "Create";
    $scope.appConfigs = appConfigs;

    $scope.checkLongUrl = function () {
        shortenedUrlsHttp.getAll({ $filter: "ActualUrl eq '" + $scope.url + "'" })
                         .success(function (data) {
                             if (data.value.length > 0) {
                                 $scope.keyword = data.value[0].ShortKeyword;
                                 $scope.shortenedUrlId = data.value[0].Id;
                                 if (appConfigs.isAdmin) {                                     
                                     $scope.createButtonLabel = "Update";
                                 }
                                 else {
                                     $scope.createButtonLabel = "Create";
                                     $scope.disableCreate = true;
                                 }
                             } else {
                                 $scope.urlShortenForm.keyword.$validate();
                                 $scope.disableCreate = false;
                             }
                         });
    };
    $scope.autoCompleteUrlOpts = {
        dataSource: shortenedUrlSchema.getGridDS(),
        dataTextField: "ActualUrl",
        change: function (e) {
            //var selectedOne = this.dataItem(e.item.index());
            //$scope.shortenedUrlId = selectedOne.Id;
            $scope.keyword = "";            
        },
        select: function (e) {
            var selectedOne = this.dataItem(e.item.index());            
            $scope.keyword = selectedOne.ShortKeyword;
            // Use the selected item or its text
        }
    };

    $scope.autoCompleteKeywordOpts = {
        dataSource: shortenedUrlSchema.getGridDS(),
        dataTextField: "ShortKeyword",
        change: function(e){
            //var selectedOne = this.dataItem(e.item.index());
            //$scope.shortenedUrlId = selectedOne.Id;            
            $scope.shortenedUrlId = null;
            $scope.createButtonLabel = "Create";
        },
        select: function (e) {            
            var selectedOne = this.dataItem(e.item.index());
            $scope.shortenedUrlId = selectedOne.Id;
            $scope.createButtonLabel = "Update";
            // Use the selected item or its text
        }
    };

    $scope.createUpdateShortenedUrl = function () {
        kendo.ui.progress($("form"), true);
        var httpRequest = null;
        if (appConfigs.isAdmin && $scope.createButtonLabel == "Update") {
            httpRequest  = shortenedUrlsHttp.patch({ Id: $scope.shortenedUrlId, ActualUrl: $scope.url, ShortKeyword: $scope.keyword.toLowerCase() });
        } else
            httpRequest = shortenedUrlsHttp.create({ ActualUrl: $scope.url, ShortKeyword: $scope.keyword.toLowerCase() });

        httpRequest.success(function (data) {
                             $scope.url = "";
                             $scope.keyword = "";
                             $scope.createButtonLabel = "Create";
                             kendo.ui.progress($("form"), false);
                             $scope.gridUrlShortener.dataSource.read();
                         });
    };


    $scope.gridOptions = {
        sortable: true,
        pageable: true,
        dataSource: $scope.gridDS,
        columns: $scope.gridColumns,
        //toolbar: kendo.template($("#toolbar-template").html()),
        scrollable: true,
        resizable: true,
        filterable: true,
        //groupable: true,
        columnMenu: true,
        reorderable: true,
        editable: {
            mode: "inline",
            //template: $("#gridEditUsersTemplate").html(),
            confirmation: true,
        },
        //selectable: "row",			
        edit: function (e) {
            //debugger;
        },
        saveChanges: function (e) {
        },
        remove: function (e) {
        },
        pageable: {
            input: true,
            numeric: false
        },
        dataBinding: function (e) {
        },
        //detailInit: detailInit,        
        dataBound: function () {            
        }
    };
}]);