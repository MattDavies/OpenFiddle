$(document).ready(function () {
    csfiddle.editor = CodeMirror.fromTextArea(document.getElementById('InputCode'), {
        lineNumbers: true,
        matchBrackets: true,
        mode: "text/x-csharp",
        indentUnit: 4,
        autofocus: true
    });
});