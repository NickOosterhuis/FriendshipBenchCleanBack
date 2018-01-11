var appointment = angular.module('appointment', []);

appointment.controller('appointmentcontroller', function ($scope, $http, $location, $window) {

    // default method run at pageload, opens the appointment with requested id.
    $http.get('http://127.0.0.1:54618/api/appointments/' + location.pathname.split("/").pop(), {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
        .then(function (response) {
            $scope.appointment = response.data;
        }, function (response) {
            $scope.appointment = "something went wrong!";
        });
    
    // method to delete appointment from the API and redirects to appointment list.
    $scope.cancelAppointment = function () {
        $scope.test = {};
        $scope.test.id = $scope.appointment.id;
        $scope.test.statusId = 3;
        $scope.test['time'] = $scope.appointment.time;
        $scope.test['clientId'] = $scope.appointment.client.id;
        $scope.test['healthworkerId'] = $scope.appointment.healthworker.id;
        $scope.test['benchId'] = $scope.appointment.bench.id;
        
        $http.put('http://127.0.0.1:54618/api/appointments/' + $scope.appointment.id, $scope.test, {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(
            function (response) {
                console.log('canceled!');
                $scope.redirectToAppointments();
            },
            function (response) {
                console.log('failed to cancel!');
            });
        
    }
    $scope.redirectToAppointments = function () {
        $window.location.href = "https://localhost:44314/adminappointments/";
    }
});