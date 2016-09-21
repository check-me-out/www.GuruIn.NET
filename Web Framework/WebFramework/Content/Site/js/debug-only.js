
/* Debug-only JS code. */

function ShowError(message, url, lineNumber, column) {
    var maxMessageLen = 50;
    if (message.length > maxMessageLen) {
        message = message.substring(0, maxMessageLen - 3);
        message += '...';
    }
    message = '<b>Error:</b> ' + message;

    var parts = url.split('?');
    parts = parts[0].split('/')
    var location = '<b>Location:</b> ' + parts[parts.length - 1] + ' (line #' + lineNumber + ', column #' + column + ')';

    var htmlCode = '<div>' + message + '</div>';
    //htmlCode += '<div>' + location + '</div>';

    var displayTimeout = 8000;
    var mouseLeaveTimeout = 3000;

    // Generate a random number between 1 & 9999
    var randomId = Math.floor(Math.random() * 9999 + 1); //Math.floor(Math.random()*(max-min+1)+min);

    //var xIcon = '<span class="glyphicon glyphicon-remove-circle" aria-hidden="true"></span>';
    var xIcon = '<span><b>X</b></span>';
    var newDiv = '<div id="' + randomId + '">' + xIcon + '<div></div></div>';
    $("#js-error-alert-container").append(newDiv);

    $("#js-error-alert-container #" + randomId + " div").html(htmlCode);

    $("#js-error-alert-container #" + randomId).fadeIn(1000);

    $("#js-error-alert-container #" + randomId).data('clear-timer-id', setTimeout(function () { HideError(randomId) }, displayTimeout));

    $("#js-error-alert-container #" + randomId + " span").click(function () { HideError(randomId) });
    $("#js-error-alert-container #" + randomId).mouseenter(function () {
        clearTimeout($(this).data('clear-timer-id'));
    }).mouseleave(function () {
        $(this).data('clear-timer-id', setTimeout(function () { HideError(randomId) }, mouseLeaveTimeout));
    });
}

function HideError(id) {
    $("#js-error-alert-container #" + id + " span").click(function (e) { });
    $("#js-error-alert-container #" + id).mouseenter(function (e) { }).mouseleave(function (e) { });
    $("#js-error-alert-container #" + id).fadeOut(1000);
    setTimeout(function () { $("#js-error-alert-container #" + id).remove(); }, 1000);
}

ShowErrorCallback = ShowError;
