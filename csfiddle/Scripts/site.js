$(document).ready(function () {
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
    
    CodeMirror.fromTextArea(document.getElementById('InputCode'), {
        lineNumbers: true,
        matchBrackets: true,
        mode: "text/x-csharp",
        indentUnit: 4
    });
});