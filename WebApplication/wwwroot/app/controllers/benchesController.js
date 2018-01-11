var benches = angular.module('benches', []);

benches.controller('benchescontroller', function ($scope, $http) {
    $http.get('http://127.0.0.1:54618/api/Benches', {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
        .then(function (response) {
            //first function handles succes
            $scope.benches = response.data;
        }, function (response) {
            //second function handles error
            $scope.benches = "something went wrong!";
        });
});