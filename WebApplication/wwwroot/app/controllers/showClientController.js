appClient.controller('showClientCtrl', function ($http, $scope, $location, $route, $routeParams) {
    //show client by ClientId
    this.route = $route;
    this.routeParams = $routeParams;
    var ClientID = this.routeParams['id'];

    //retrieve all client
    $http.get('http://127.0.0.1:54618/api/clients/' + ClientID, {
        headers: {
            'Authorization': "Bearer " + getCookie('JWT')
        }
    })
        .then(function (response) {
            //first function handles succes
            $scope.client = response.data;
        }, function (response) {
            //second function handles error
            console.log("something went wrong!");
        });
});