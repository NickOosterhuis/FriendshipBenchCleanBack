questionnaires.controller('answersCtrl', function ($scope, $http, $location, $route, $routeParams) {
    //show questionnaire by questionnaireId
    this.route = $route;
    this.routeParams = $routeParams;
    var questionnaireId = this.routeParams['id'];

    $http.get('http://127.0.0.1:54618/api/Questionnaires/' + questionnaireId, {
        headers: {
            'Authorization': "Bearer " + getCookie("JWT")
        }
    })
        .then(function (response) {
            //succes
            console.log(response.data);
            $scope.questionnaire = response.data;

            $location.path('/questionnaire/' + questionnaireId);
        }, function (response) {
            //failure
            alert('could not retrieve answers!');
            console.log(response.data);
        });

    $scope.setRedFlag = function () {
        console.log(questionnaireId);
        $http.get('http://127.0.0.1:54618/api/Questionnaires/' + questionnaireId, {
            headers: {
                'Authorization': "Bearer " + getCookie("JWT")
            }
        })
            .then(function (response) {
                //success
                var questionnaire = response.data;
                console.log(questionnaire);
                $scope.sendDataObject = {};
                $scope.sendDataObject.id = questionnaireId;
                $scope.sendDataObject.time = questionnaire.time;
                $scope.sendDataObject.client_id = questionnaire.client.id;
                $scope.sendDataObject.redflag = true;
                console.log($scope.sendDataObject);
                $http.put('http://127.0.0.1:54618/api/Questionnaires/' + questionnaireId, $scope.sendDataObject, {
                    headers: {
                        'Authorization': "Bearer " + getCookie("JWT")
                    }
                })
                    .then(function (response) {
                        //success
                        alert('updatet red flag!');
                        $location.path('/questionnaires');

                    }, function (response) {
                        //failure
                        alert('not able to update questionnaire');
                    });

            }, function (response) {
                //failure
                alert('not able to retrieve questionnaire data');
            });
    }



});