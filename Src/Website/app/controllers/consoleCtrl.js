angular.module('console', [])
    .controller('consoleCtrl',['$scope','$http', function ($scope, $http) {

        var editor = ace.edit("consoleEditor");
        editor.setTheme("ace/theme/visualstudio");
        editor.getSession().setMode("ace/mode/csharp");        

        $scope.getCode = function ()
        {

            var lang = $scope.language;
            $http.get('/api/Console/Code',{params:{ language: lang }})
                .success(function (data, status, headers, config) {
                    editor.setValue(data);
                });
        }

        $scope.postRun = function()
        {
            var input =
                {
                    Id: $scope.id,
                    Code: editor.getValue(),
                    Language: $scope.language
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

        $scope.postConvert = function () {
            var input =
                {
                    Code: editor.getValue(),
                    Language: $scope.language
                };

            if ($scope.code != '') {
                $http.post('/api/IDE/Convert', input)
                    .success(function (data, status, headers, config) {
                        editor.setValue(data);
                        switch($scope.language)
                        {
                            case "CSharp":
                                editor.getSession().setMode("ace/mode/csharp");
                                break;
                            case "VbNet":
                                editor.getSession().setMode("ace/mode/vbscript");
                                break;
                        }
                    });
            }
        }

        $scope.getGUID = function () {
          $http.get('/api/IDE/GUID')
                .success(function (data, status, headers, config) {
                    id.setValue(data);
                });            
        }
     
    }]);