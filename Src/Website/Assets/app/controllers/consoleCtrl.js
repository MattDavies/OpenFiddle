angular.module('console', [])
    .controller('consoleCtrl',['$scope','$http', function ($scope, $http) {

        //$scope.getList = function ()
        //{
        //    $http.get('/api/ConsoleApi/GetUserTodoItems')
        //        .success(function (data, status, headers, config) {
        //            $scope.todoList = data;
        //        });
        //}

        $scope.postItem = function()
        {
            item =
                {
                    code : $scope.code
                };

            if ($scope.code != '') {
                $http.post('/api/Console/Run', item)
                    .success(function (data, status, headers, config) {
                        $scope.code = data.code;
                        $scope.output=data.output;
                    });
            }
        }

        //$scope.complete = function(index)
        //{
        //    $http.post('/api/WS_Todo/CompleteTodoItem/' + $scope.todoList[index].id)
        //        .success(function (data, status, headers, config) {
        //            $scope.getList();
        //        });
        //}

        //$scope.delete = function(index)
        //{
        //    $http.post('/api/WS_Todo/DeleteTodoItem/' + $scope.todoList[index].id)
        //        .success(function (data, status, headers, config) {
        //            $scope.getList();
        //        });
        //}

        ////Get the current user's list when the page loads.
        //$scope.getList();
    }]);