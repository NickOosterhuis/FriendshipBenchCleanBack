//module
var appClient = angular.module('userClients', ['ngRoute']);

appClient.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode(true);
    $locationProvider.hashPrefix('');
}]);

//controller
appClient.controller('clientsCtrl', function ($scope, $http, $location) {
    var cookie = getCookie("JWT");
    var email = getCookie("Email");
    //get all clients connected to the logged in healthworker
    $http.get('http://127.0.0.1:54618/api/Clients/connected/' + email, {
        headers: {
            'Authorization': "Bearer " + cookie
        }
    })
        .then(function (response) {
            //first function handles succes
            $scope.clients = response.data;
            console.log($scope.clients);
            $location.path("/clients");

        }, function (response) {
            //second function handles error
            alert('error');
            $scope.clients = "something went wrong!";

        });

    // callback for ng-click 'showClient':
    $scope.showClient = function (ClientID) {
        console.log('redirecting to: ' + "/clients/show/" + ClientID);
        $location.path("/clients/show/" + ClientID);
    };

 });

appClient.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/clients', { templateUrl: '/app/views/clients/list.html', controller: 'clientsCtrl' });
    $routeProvider.when('/clients/show/:id', { templateUrl: '/app/views/clients/show.html', controller: 'showClientCtrl' });
    $routeProvider.otherwise({ redirectTo: '/' });
}]);


