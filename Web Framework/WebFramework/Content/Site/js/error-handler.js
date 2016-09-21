/*
define('error-handler', ['jquery'], function ($) {

});
 */

    /* Handle and display JS errors. */
    window.onerror = ProcessError;

    var ShowErrorCallback = function (message, url, lineNumber, column) { };

    function ProcessError(message, url, lineNumber, column, errorObj) {

        var col = !column ? '' : column;
        var trace = !errorObj ? '' : errorObj;

        var logMessage = 'Error: ' + message + '\r\nScript: ' + url + '\r\nLine: ' + lineNumber + '\r\nColumn: ' + col + '\r\nURL: ' + window.location + '\r\nStackTrace: ' + trace;
        if (window.console) {
            window.console.log(logMessage);
        }

        setTimeout(function () { LogErrorToServer(logMessage); }, 1000);

        setTimeout(function () { ShowErrorCallback(message, url, lineNumber, col); }, 1500);

        return false;
    }

    /*function ProcessError(message, url, lineNumber) {

        return ProcessError(message, url, lineNumber, 0, null);

    }*/

    function LogErrorToServer(msg) {
        $.ajax({
            url: '/api/Log/Error',
            type: 'POST',
            data: JSON.stringify(msg),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: true,
            cache: false,
        });
    }
