var app = angular.module('app');
app.controller('chatController',
    function QuestionController($scope, $http) {
        $scope.loaded = false;

        $http.get('/chat/onlineusers').success(function (data) {
            $scope.users = data;
            $scope.loaded = true;
        });

        $scope.add = function (user) {
            $scope.users.push(user);
            alert($scope.users.length);
        };
        $scope.delete = function (userId) {
            alert();
        };
    }
)