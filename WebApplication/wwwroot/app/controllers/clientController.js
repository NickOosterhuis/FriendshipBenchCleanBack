var client = angular.module('client', []);

client.controller('clientcontroller', function ($scope, $http, $location, $window) {

    // default method run at pageload, opens the client with requested id.
    $http.get('http://127.0.0.1:54618/api/Clients/' + location.pathname.split("/").pop(), {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
        .then(function (response) {
            $scope.client = response.data;
        }, function (response) {
            $scope.client = "something went wrong!";
        });
    // method to save client to the API and redirects to client list.
    $scope.saveclient = function (method) {
        if (method == "put") {
            $http.put('http://127.0.0.1:54618/api/Clients/' + location.pathname.split("/").pop(), $scope.client, {
                headers: {
                    'Authorization': "Bearer " + getCookie("JWT")
                }
            })
                .then(
                function (response) {
                    console.log('edited!');
                    $scope.redirectToClients();
                },
                function (response) {
                    console.log('failed!');
                });
        } 
    }
    // method to delete client from the API and redirects to client list.
    $scope.deleteclient = function () {
        $http.delete('http://127.0.0.1:54618/api/Clients/' + $scope.client.id, {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(
            function (response) {
                console.log('deleted!');
                $scope.redirectToClients();
            },
            function (response) {
                console.log('failed to delete!');
            });
    }
    $scope.redirectToClients = function () {
        $window.location.href = "https://localhost:44314/adminClients/";
    }
});