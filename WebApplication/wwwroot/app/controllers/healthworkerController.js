var healthworker = angular.module('healthworker', []);

healthworker.controller('healthworkercontroller', function ($scope, $http, $location, $window) {

    // default method run at pageload, opens the healthworker with requested id.
    $http.get('http://127.0.0.1:54618/api/healthworkers/' + location.pathname.split("/").pop(), {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
        .then(function (response) {
            $scope.healthworker = response.data;
        }, function (response) {
            $scope.healthworker = "something went wrong!";
        });
    // method to save healthworker to the API and redirects to healthworker list.
    $scope.savehealthworker = function (method) {
        if (method == "put") {
            $http.put('http://127.0.0.1:54618/api/healthworkers/edit/' + location.pathname.split("/").pop(), $scope.healthworker, {
                headers: {
                    'Authorization': "Bearer " + getCookie("JWT")
                }
            })
                .then(
                function (response) {
                    console.log('edited!');
                    $scope.redirectToHealthworkers();
                },
                function (response) {
                    console.log('failed!');
                });
        } else if (method == "post") {
            console.log("Method is post");
            $http.post('http://127.0.0.1:54618/api/Account/register/healthworker/', $scope.healthworkeradd, {
                headers: {
                    'Authorization': "Bearer " + getCookie("JWT")
                }
            })
                .then(
                function (response) {
                    console.log('added!');
                    $scope.redirectToHealthworkers();
                },
                function (response) {
                    console.log('failed!');
                });
        }

    }
    // method to delete healthworker from the API and redirects to healthworker list.
    $scope.deletehealthworker = function () {
        console.log('test');
        $http.delete('http://127.0.0.1:54618/api/healthworkers/' + $scope.healthworker.id, {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(
            function (response) {
                console.log('deleted!');
                $scope.redirectToHealthworkers();
            },
            function (response) {
                console.log('failed to delete!');
            });
    }
    $scope.redirectToHealthworkers = function () {
        $window.location.href = "https://localhost:44314/adminhealthworkers/";
    }
});