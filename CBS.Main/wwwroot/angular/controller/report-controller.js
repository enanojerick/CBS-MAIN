var app = angular.module("report-controller", ['angularUtils.directives.dirPagination', 'report-service', 'ngMessages']);
app.controller("report", function ($scope, rp) {

    $scope.ReportID = "";

    $scope.PoliceReportQueue = { Days: "" };
    $scope.StaffReportQueue = { JobType: 0, EmploymentStatus: 0, EmployeeType: 0 };

    $scope.flag = { ShowLoader: false, ShowPoliceReport: false, ShowStaffReport: false }

    $scope.getReportList = function ()
    {

        rp.requestReportList()
            .then(function success(response) {
                $scope.reportlist = response.data;
            }, function error(response)
            {

            });
    }

    $scope.getPoliceReportList = function () {
        $scope.flag.ShowLoader = true;
        rp.requestPoliceReportList($scope.PoliceReportQueue)
            .then(function success(response) {
                $scope.policereportlist = response.data;
                $scope.flag.ShowLoader = false;
            }, function error(response) {
                $scope.flag.ShowLoader = false;
            });
    }

    $scope.updateReportView = function () {


        if ($scope.ReportID === 1) {
            $scope.flag.ShowStaffReport = true;
            $scope.flag.ShowPoliceReport = false;
        } else if ($scope.ReportID === 2) {
            $scope.flag.ShowPoliceReport = true;
            $scope.flag.ShowStaffReport = false;

        }
        else { $scope.flag.ShowPoliceReport = false;}
    };

    /** Staff **/

    $scope.getJobTypeList = function () {
        rp.requestJobTypeList()
            .then(function success(response) {
                $scope.jobTypeList = response.data;
            }, function error(response) {

            });
    }

    $scope.getEmploymentStatus = function () {
        rp.requestEmploymentStatus()
            .then(function success(response) {
                $scope.employmentStatus = response.data;
            }, function error(response) {

            });
    }

    $scope.getEmployeeType = function () {
        rp.requestEmployeeType()
            .then(function success(response) {
                $scope.employeeType = response.data;
            }, function error(response) {

            });
    }

    

    $scope.getStaffReportList = function () {
        $scope.flag.ShowLoader = true;
        rp.requestStaffReportList($scope.StaffReportQueue)
            .then(function success(response) {
                $scope.staffreportlist = response.data;
                $scope.flag.ShowLoader = false;
            }, function error(response) {
                $scope.flag.ShowLoader = false;
            });
    }

});
