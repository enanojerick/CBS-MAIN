var app = angular.module('report-service', []);

app.factory('rp', function ($http, $cacheFactory) {

    var requestReportList = function () {
        return $http({
            method: 'GET',
            url: '/api/ReportList',
            cache: false,
        });
    }

    var requestPoliceReportList = function(val) {
        return $http({
            method: 'GET',
            url: '/api/PoliceReport',
            cache: false,
            params: val,
        });
    }

    /** Staff **/
    var requestJobTypeList = function () {
        return $http({
            method: 'GET',
            url: '/api/JobTypeList',
            cache: false,
        });
    }

    var requestEmploymentStatus = function () {
        return $http({
            method: 'GET',
            url: '/api/EmploymentStatus',
            cache: false,
        });
    }

    var requestEmployeeType = function () {
        return $http({
            method: 'GET',
            url: '/api/EmployeeType',
            cache: false,
        });
    }

    

    var requestStaffReportList = function (val) { console.log(val);
        return $http({
            method: 'GET',
            url: '/api/StaffReport',
            cache: false,
            params: val
        });
    }

    return {

        requestReportList: function () {
            return requestReportList();
        },

        requestPoliceReportList: function (val) {
            return requestPoliceReportList(val);
        },

        requestJobTypeList: function () {
            return requestJobTypeList();
        },

        requestStaffReportList: function (val) {
            return requestStaffReportList(val);
        },

        requestEmploymentStatus: function () {
            return requestEmploymentStatus();
        },

        requestEmployeeType: function () {
            return requestEmployeeType();
        },
        
    }
});
