angular.module("urlShortenerModule").factory('shortenedUrlSchema', ['$timeout', '$window', 'shortenedUrlsHttp', 'appConfigs',
    function ($timeout, $window, shortenedUrlsHttp, appConfigs) {
        
        var getGridDS = function () {
            gridDS = new kendo.data.DataSource({
                type: "odata-v4",
                transport: {
                    read: function (options) {
                        var data = {};
                        $.each(options.data, function (key, value) {
                            if (value != undefined)
                                data[key] = value;
                        });
                        options.data = data;
                        var oDataOptions = kendo.data.transports["odata-v4"].parameterMap(options.data, "read");
                        //if (oDataOptions.$filter)
                        //  oDataOptions.$filter = oDataOptions.$filter.replace(/AppInstallNotifStatus/g, 'HotLineModels.Models.AppInstallNotifStatusType');
                        delete oDataOptions.$inlinecount;
                        oDataOptions.$count = true;

                        shortenedUrlsHttp.getAll(oDataOptions)
                                                 .success(function (data) {                                                     
                                                     options.success(data);

                                                 });
                    },
                    update: function (options) {
                        var  shortenUrlModel = options.data.models[0];
                        kendo.ui.progress($("#gridEditUsers"), true);
                        shortenedUrlsHttp.patch(shortenUrlModel)
                                         .success(function (data) {
                                             kendo.ui.progress($("#gridEditUsers"), false);
                                             options.success(data);
                                         });
                    },
                    destroy: function (options) {
                        var  shortenUrlModel = options.data.models[0];
                        kendo.ui.progress($("#gridEditUsers"), true);
                        shortenedUrlsHttp.delete(shortenUrlModel.Id)
                                         .success(function (data) {
                                             kendo.ui.progress($("#gridEditUsers"), false);
                                             options.success(data);
                                         });
                    },
                    parameterMap: function (options) {
                    }
                },
                schema: {
                    total: function (data) { return data['@odata.count']; },
                    data: "value",
                    model: {
                        id: "Id",
                        fields: {
                            Id: { type: "number" },
                            ActualUrl: { type: "string" },
                            ShortKeyword: { type: "string" },
                            CreatedDate: { type: "date", editable: false },
                            HitCount: { type: "string", editable: false },
                            LastHit: { type: "date", editable: false }
                        }
                    }
                },
                sort: { field: "CreatedDate", dir: "desc" },
                pageSize: 500,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
                change: function (e) {
                },
                batch: true,
                requestStart: function () {
                },
            });
            return gridDS;
        };


        var getGridColumns = function () {
            gridColumns = [];
        if(appConfigs.isAdmin)
            gridColumns = gridColumns.concat([{
                command: ["edit", "delete"], width: "150px"
            }]);
       gridColumns = gridColumns.concat([
                       { field: "Id", title: "ID", width: "50px", hidden: true, template: "<span class=\"{{getColorCodeClass(dataItem)}}\">{{dataItem.Id}}</span>", },                        
                       {
                           field: "ActualUrl", title: "Actual Url", width: "120px",
                       },
                       {
                           field: "ShortKeyword", title: "Short Keyword", width: "120px", template: '<a href="{{appConfigs.urlShortenerWebAPIURL +  dataItem.ShortKeyword}}" target="_blank" >{{dataItem.ShortKeyword}}</a>',
                       },
                       {
                           field: "CreatedDate", title: "Created Date", width: "230px",format: "{0:yyyy-MM-dd hh:mm tt}", filterable: {
                               ui: "datetimepicker"
                           },
                       },
                       //Some important settigns
                       {
                           field: "HitCount", title: "Hit Count", width: "160px", 
                       },
                       {
                           field: "LastHit", title: "Last Hit", width: "160px", format: "{0:yyyy-MM-dd hh:mm tt}", filterable: {
                               ui: "datetimepicker"
                           },
                       }



       ]);
       return gridColumns;
   };

        return {
            getGridColumns: function () { return getGridColumns(); },
            getGridDS: function (leadId, startTime, endTime) { return getGridDS(leadId, startTime, endTime); },
        };
    }]);