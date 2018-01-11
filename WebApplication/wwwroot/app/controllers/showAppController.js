app.controller('showAppCtrl', function ($http, $scope, $location, $route, $routeParams) {
    //show appointment by appointmentId
    this.route = $route;
    this.routeParams = $routeParams;
    var appointmentID = this.routeParams['id'];

    //differint statusses
    $scope.statusses = [
        { id: 1, name: "pending" },
        { id: 2, name: "rejected" }
    ];

    //retrieve all benches
    $http.get('http://127.0.0.1:54618/api/benches')
        .then(function (response) {
            //first function handles succes
            $scope.benches = response.data;
        }, function (response) {
            //second function handles error
            console.log("something went wrong!");
        });

    //retrieve appointment details
    $http.get("http://127.0.0.1:54618/api/Appointments/" + appointmentID)
        .then(
        function (response) {
            //succes
            console.log(response.data);
            $scope.appointment = response.data;
            $location.path("/appointments/" + appointmentID);
            console.log('succes');

        },
        function (response) {
            //failure
            console.log("cant retrieve appointment data");

        }
        );

    $scope.sendData = function () {

        $scope.sendDataObject = {}

        $scope.sendDataObject.id = appointmentID;
        $scope.sendDataObject.date = $scope.appointment.date;
        $scope.sendDataObject.time = $scope.appointment.time;
        $scope.sendDataObject.statusId = $scope.appointment.status.id;
        $scope.sendDataObject.benchId = $scope.appointment.bench.id;
        $scope.sendDataObject.clientId = $scope.appointment.clientId;
        $scope.sendDataObject.healthworkerName = $scope.appointment.healthworkerName;

        $http.put('http://127.0.0.1:54618/api/Appointments/' + appointmentID, $scope.sendDataObject)
            .then(function (response) {
                alert('appointment has been saved');
                $location.path('/appointments');

            }, function (response) {
                //second function handles error
                alert('something went wrong!');

            });
    };
});