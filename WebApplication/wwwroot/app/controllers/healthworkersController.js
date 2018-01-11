var healthworkers = angular.module('healthworkers', []);

healthworkers.controller('healthworkerscontroller', function ($scope, $http) {
    $http.get('http://127.0.0.1:54618/api/HealthWorkers/', {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
    .then(function (response) {
        //first function handles succes
        $scope.healthworkers = response.data;
        console.log(response.data);
    }, function (response) {
        //second function handles error
        console.log($http.headers);
        $scope.healthworkers = "something went wrong! Please contact an administrator.";
    });
});