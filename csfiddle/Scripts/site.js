$(document).ready(function () {
    csfiddle = {
        editor: CodeMirror.fromTextArea(document.getElementById('InputCode'), {
            lineNumbers: true,
            matchBrackets: true,
            mode: "text/x-csharp",
            indentUnit: 4,
            autofocus: true
        }),

        newCode: function() {
            history.pushState(null, "c#fiddle", "/");
            window.location = "/";
        },

        runCode: function() {
            $("#run").html("Running...");
            $.post('/Run', { InputCode: csfiddle.editor.getValue() }, function(data) {
                $('#result').html(data);
            })
                .always(function () { $("#run").html("Run"); })
                .fail(function () { $("#result").html("<pre>Failed to run.</pre>"); });
        },

        saveCode: function () {
            $("#save").html("Saving...");
            $.post('/Save', { Id: $("#id").val(), InputCode: csfiddle.editor.getValue() }, function(data) {
                if ($("#id").val() != data.id) {
                    history.pushState(null, "Viewing Fiddle " + data.id, "/" + data.id);
                    $("#id").val(data.id);
                }
                $('#result').html(data.result);
            }).always(function() { $("#save").html("Save and Run"); });
        }
    };
    
    $('body').layout({
        resizeWhileDragging: true,
        sizable: false,
        animatePaneSizing: true,
        west__size: "50%",
        spacing_open: 0,
        spacing_closed: 0,
        west__spacing_closed: 8,
        west__spacing_open: 8,
        west__togglerLength_closed: 105,
        west__togglerLength_open: 105,
        closable: false
    });

    $("#new").click(csfiddle.newCode);
    $("#run").click(csfiddle.runCode);
    $("#save").click(csfiddle.saveCode);
});