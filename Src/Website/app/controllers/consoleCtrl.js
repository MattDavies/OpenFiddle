angular.module('console', [])
    .controller('consoleCtrl',['$scope','$http', function ($scope, $http) {

        var editor = ace.edit("consoleEditor");
        editor.setTheme("ace/theme/visualstudio");
        editor.getSession().setMode("ace/mode/csharp");

        $scope.getCode = function ()
        {
            $http.get('/api/Console/Code')
                .success(function (data, status, headers, config) {
                    editor.setValue(data);
                });
        }

        $scope.postRun = function()
        {
            var input =
                {
                    Id: $scope.id,
                    Code:editor.getValue()  
                };

            if ($scope.code != '') {
                $http.post('/api/Console/Run', input)
                    .success(function (data, status, headers, config) {
                        editor.setValue(data.Code);
                        $scope.output = data.Output;
                        $scope.id = data.Id;
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

        //Get the example code when the page loads.
        $scope.getCode();
     
    }]);