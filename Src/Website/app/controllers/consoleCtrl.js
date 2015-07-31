angular.module('console', [])
    .controller('consoleCtrl', ['$scope', '$http', '$location', '$anchorScroll', '$routeParams', function ($scope, $http, $location, $anchorScroll, $routeParams) {


        $scope.getCode = function ()
        {
            var lang = $scope.language;
            $http.get('/api/Console/Code',{params:{ language: lang }})
                .success(function (data, status, headers, config) {
                    editor.setValue(data);
                });
        }

        $scope.createNew = function () {

            if ($scope.code != '' && !window.confirm("Are you sure?"))
                    return;
            $scope.getCode();
            //new code, new id
            $scope.getHash();
            
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
                        $location.hash('consoleOutput');
                        $anchorScroll();
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
                        $scope.setSyntax();
                        $scope.output = null;
                    })
                .error(function (data, status, headers, config) {
                    //we coudln't convert so for the time being we'll get the sample code.
                    if (window.confirm("We couldn't convert! Do you want to change the language?")) {
                        $scope.getCode();
                        $scope.setSyntax();
                    } else {
                        //revert (there must be a better way but hey it's Friday).
                        $scope.language = $scope.language == "CSharp" ? "VbNet" : "CSharp";
                    }
                });
            }
        }

        $scope.postFormat = function () {
            var input =
                {
                    Code: editor.getValue(),
                    Language: $scope.language
                };

            if ($scope.code != '') {
                $http.post('/api/IDE/Format', input)
                    .success(function (data, status, headers, config) {
                        editor.setValue(data);
                    });
            }
        }

        $scope.postSave = function () {
            var input =
                {
                    Id: $scope.id,
                    Code: editor.getValue(),
                    Language: $scope.language
                };

            if ($scope.code != '') {
                $http.post('/api/Console/Save', input)
                    .success(function (data, status, headers, config) {
                        editor.setValue(data.Code);
                        $scope.output = data.Output;
                        $scope.id = data.Id;
                        $location.hash('consoleOutput');
                        $anchorScroll();
                    });
            }
        }

        $scope.getHash = function () {
          $http.get('/api/IDE/Hash')
                .success(function (data, status, headers, config) {
                    $scope.id = data;
                    //change the id in the URL
                    var search_obj = {};
                    search_obj['id'] = $scope.id;
                    $location.search(search_obj);
                });            
        }

        $scope.setSyntax = function () {
            switch ($scope.language) {
                case "CSharp":
                    editor.getSession().setMode("ace/mode/csharp");
                    break;
                case "VbNet":
                    editor.getSession().setMode("ace/mode/vbscript");
                    break;
            }
        }


        $scope.id = $routeParams.id;
        if ($scope.id == null) {
            $scope.getHash();
        }

        var editor = ace.edit("consoleEditor");
        editor.setTheme("ace/theme/visualstudio");
        editor.getSession().setMode("ace/mode/csharp");
     
    }]);