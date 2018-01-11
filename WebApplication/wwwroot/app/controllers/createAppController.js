app.controller('createAppCtrl', function ($http, $scope, $location) {

    //retrieve all benches
    $http.get('http://127.0.0.1:54618/api/benches', {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
        .then(function (response) {
            //first function handles succes
            $scope.benches = response.data;
        }, function (response) {
            //second function handles error
            console.log("something went wrong!");
        });   

    var email = getCookie('Email');

    //retrieve all connected clients
    $http.get('http://127.0.0.1:54618/api/clients/connected/' + getCookie('Email'), {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
        .then(function (response) {
            //first function handles succes
            $scope.clients = response.data;
            console.log($scope.clients);
        }, function (response) {
            //second function handles error
            console.log("something went wrong!");
        });

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

    $scope.sendData = function () {
        
        $scope.sendDataObject = {}
        
        $date = $scope.appointment.date;
        $time = $scope.appointment.time;
        $dateTime = $date + "T" + $time + ":00.801Z";

        $scope.sendDataObject.time = $dateTime;
        $scope.sendDataObject.benchId = $scope.appointment.bench.id;
        $scope.sendDataObject.clientId = $scope.appointment.clientId;
        $scope.sendDataObject.healthworkerId = $scope.healthworkerId;
        console.log($scope.sendDataObject);

        $http.post('http://127.0.0.1:54618/api/Appointments/', $scope.sendDataObject, {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(function (response) {
                alert('appointment has been created');
                $location.path('/appointments');

            }, function (response) {
                //second function handles error
                alert('something went wrong!');
                console.log(response);

            });
    };
});