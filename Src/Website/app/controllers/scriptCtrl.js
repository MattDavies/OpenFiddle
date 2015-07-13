angular.module('script', [])
    .controller('scriptCtrl',['$scope','$http', function ($scope, $http) {

        var editor = ace.edit("consoleEditor");
        editor.setTheme("ace/theme/visualstudio");
        editor.getSession().setMode("ace/mode/csharp");

        $scope.getCode = function ()
        {
            $http.get('/api/Script/Code')
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
                $http.post('/api/Script/Run', input)
                    .success(function (data, status, headers, config) {
                        editor.setValue(data.Code);
                        $scope.output = data.Output;
                        $scope.id = data.Id;
                    });
            }
        } 

        //Get the example code when the page loads.
        $scope.getCode();
     
    }]);