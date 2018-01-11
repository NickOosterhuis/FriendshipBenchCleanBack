//module
var app = angular.module('hwAppointments', ['ngRoute']);

app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode(true);
    $locationProvider.hashPrefix('');
}]);


//controller
app.controller('appointmentCtrl', function ($scope, $http, $location) {
    //get all appointments

    //get logged in user
    var email = getCookie('Email');

    //get logged in user
    $http.get('http://127.0.0.1:54618/api/account/currentUser/' + email, {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    }).then(function (response) {
        //success
        $scope.healthworkerId = response.data.id;
    }, function (response) {
        //failure
        alert('not able to retrieve current user details');
    });

    $scope.listAppointments = function () {
        $http.get('http://127.0.0.1:54618/api/Appointments', {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(function (response) {
                //first function handles succes
                var arr = response.data;
                var userAppointments = new Array();
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].healthworker.id == $scope.healthworkerId) {
                        userAppointments.push(arr[i]);
                    }
                }
                console.log(userAppointments);
                $scope.appointments = userAppointments;
                $location.path("/appointments")

            }, function (response) {
                //second function handles error
                $scope.appointments = "something went wrong!";

            });
    };

    $scope.listAppointments();
  
    // callback for ng-click 'cancelAppointment':
    $scope.cancelAppointment = function (appointmentID) {
        $http.get('http://127.0.0.1:54618/api/appointments/' + appointmentID, {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(function (response) {
                //succes
                $appointment = response.data;
                console.log($appointment);
                $scope.sendDataObject = {}

                $scope.sendDataObject.id = $appointment.id;
                $scope.sendDataObject.time = $appointment.time;
                $scope.sendDataObject.statusId = 3;
                $scope.sendDataObject.benchId = $appointment.bench.id
                $scope.sendDataObject.clientId = $appointment.client.id
                ///aaanapaaasseeen
                $scope.sendDataObject.healthworkerId = $scope.healthworkerId;
                console.log($scope.sendDataObject);

                $http.put('http://127.0.0.1:54618/api/Appointments/' + appointmentID, $scope.sendDataObject, {
                    headers: {
                        'Authorization': "Bearer " + getCookie("JWT")
                    }
                })
                    .then(function (response) {
                        alert('appointment has been updated');

                        $scope.listAppointments();

                    }, function (response) {
                        //second function handles error
                        alert('something went wrong!');

                    });

            }, function (response) {
                //failure
                alert('not able to retrieve appointment data');
            });
    };

    //callback for ng-click 'createUser':
    $scope.createAppointment = function () {
        $location.path("/create/appointment");
    }

    //delete appointment
    $scope.deleteAppointment = function (appointmentID) {
        $http.delete("http://127.0.0.1:54618/api/Appointments/" + appointmentID, {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(
            function (response) {
                //succes
                console.log('succes');
                //$location.path('/appointments');
                $scope.listAppointments();
                

            },
            function (response) {
                //failure
                console.log('failure');
            }
            );
    }

 });

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/create/appointment', { templateUrl: '/app/views/appointments/create.html', controller: 'createAppCtrl' });
    $routeProvider.when('/appointments', { templateUrl: '/app/views/appointments/list.html', controller: 'appointmentCtrl' });
    $routeProvider.when('/appointments/:id', { templateUrl: '/app/views/appointments/show.html', controller: 'showAppCtrl' });
    $routeProvider.otherwise({ redirectTo: '/' });
}]);


