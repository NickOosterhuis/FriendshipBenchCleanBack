var clients = angular.module('clients', []);

clients.controller('clientscontroller', function ($scope, $http) {
    var cookie = getCookie("JWT");
    $http.get('http://127.0.0.1:54618/api/clients/', {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
    .then(function (response) {
        $scope.clients = response.data;
        console.log("succes");
    }, function (response) {
        $scope.clients = "something went wrong! Please contact an administrator.";
        console.log("failed");
    });
});