var app = angular.module("connecthr", ["report-controller",
    "date-filter",
    "ngImageCache",
    "ngMessages"])
    .config(['ImageCacheProvider', function (ImageCacheProvider) {
        ImageCacheProvider.setStorage(window.localStorage);
    }]);

app.filter('formatDate', function (dateFilter) {
    var formattedDate = '';
    return function (dt) {
        console.log(new Date(dt.split('-').join('/')));
        formattedDate = dateFilter(new Date(dt.split('-').join('/')), 'd/M/yyyy');
        return formattedDate;
    }

});  