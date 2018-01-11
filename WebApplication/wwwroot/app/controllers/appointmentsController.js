var appointments = angular.module('appointments', []);

appointments.controller('appointmentscontroller', function ($scope, $http) {
    var cookie = getCookie("JWT");
    $http.get('http://127.0.0.1:54618/api/appointments/', {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
    .then(function (response) {
        $scope.appointments = response.data;
        console.log("succes");
    }, function (response) {
        $scope.appointments = "something went wrong! Please contact an administrator.";
        console.log("failed");
    });
});