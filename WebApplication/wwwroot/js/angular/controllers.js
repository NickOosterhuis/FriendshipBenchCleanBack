var benches = angular.module('benches', []);
var bench = angular.module('bench', []);

    //controller
    benches.controller('benchescontroller', function ($scope, $http) {
        $http.get('http://127.0.0.1:54618/api/Benches')
            .then(function (response) {
                //first function handles succes
                $scope.benches = response.data;
            }, function (response) {
                //second function handles error
                $scope.benches = "something went wrong!";
            });
    });

    bench.controller('benchcontroller', function ($scope, $http, $location, $window) {
        //console.log('http://127.0.0.1:54618/api/Benches/' + location.pathname.split("/").pop());
        
        // default method run at pageload, opens the bench with requested id.
        $http.get('http://127.0.0.1:54618/api/Benches/' + location.pathname.split("/").pop())
                .then(function (response) {
                    $scope.bench = response.data;
                }, function (response) {
                    $scope.bench = "something went wrong!";
            });
        // method to save bench to the API and redirects to bench list.
        $scope.saveBench = function (method) {
            if (method == "put") {
                console.log('method is put');
                $http.put('http://127.0.0.1:54618/api/Benches/' + location.pathname.split("/").pop(), $scope.bench)
                    .then(
                    function (response) {
                        console.log('edited!');
                        $scope.redirectToBenches();
                    },
                    function (response) {
                        console.log('failed!');
                    });
            } else if (method == "post") {
                console.log('method is post');
                console.log(angular.toJson($scope.benchadd));
                $http.post('http://127.0.0.1:54618/api/Benches/', $scope.benchadd)
                    .then(
                    function (response) {
                        console.log('added!');
                        $scope.redirectToBenches();
                    },
                    function (response) {
                        console.log('failed!');
                    });
            }
            
        }
        // method to delete bench from the API and redirects to bench list.
        $scope.deleteBench = function () {
            console.log('test');
            $http.delete('http://127.0.0.1:54618/api/Benches/' + $scope.bench.id)
                .then(
                function (response) {
                    console.log('deleted!');
                    $scope.redirectToBenches();
                },
                function (response) {
                    console.log('failed to delete!');
                });
        }
        $scope.redirectToBenches = function () {
            $window.location.href = "https://localhost:44314/bench/";
        }
        
    });