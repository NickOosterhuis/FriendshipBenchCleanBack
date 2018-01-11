//module
var questionnaires = angular.module('questionnaires', ['ngRoute']);

questionnaires.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode(true);
    $locationProvider.hashPrefix('');
}]);

//controller
questionnaires.controller('questionnairesCtrl', function ($scope, $http, $location) {

    //get logged in user
    var email = getCookie('Email');
    
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


    ///get my clients

    $http.get('http://127.0.0.1:54618/api/clients', {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    }).then(function (response) {
        //success

        var clients = response.data;
        $scope.myClients = new Array();
        for (var i = 0; i < clients.length; i++) {
            if (clients[i].id == $scope.healthworkerId) {

                $scope.myClients.push(clients[i].id);
            }
        }

    }, function (response) {
        //failure
        alert('not able to retrieve current user details');
    });



    $scope.listQuestionnaires = function () {
        $http.get('http://127.0.0.1:54618/api/Questionnaires', {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(function (response) {
                //succes
                $scope.questionnaires = response.data;

                $location.path('/questionnaires');
            }, function (response) {
                //failure
                alert('not able to retrieve the questionnaires');

            });
    }

    $scope.listQuestionnaires();

    $scope.deleteQuestionnaire = function (questionnaireId) {
        //delete a questionnaire
        $http.delete("http://127.0.0.1:54618/api/Questionnaires/" + questionnaireId, {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(
            function (response) {
                //succes
                console.log('succes');
                $scope.listQuestionnaires();
            },
            function (response) {
                //failure
                console.log('failure');
                alert('not able to delete questionnaire');
            }
            );
    }
    
    $scope.showAnswers = function (questionnaireId) {
        $location.path('/questionnaire/' + questionnaireId);
    }
});

questionnaires.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/questionnaires', { templateUrl: '/app/views/questionnaires/list.html', controller: 'questionnairesCtrl' });
    $routeProvider.when('/questionnaires/show', { templateUrl: '/app/views/questionnaires/show.html', controller: 'questionnairesCtrl' });
    $routeProvider.when('/questionnaire/:id', { templateUrl: '/app/views/questionnaires/answers.html', controller: 'answersCtrl' });
    $routeProvider.otherwise({ redirectTo: '/' });
}]);


