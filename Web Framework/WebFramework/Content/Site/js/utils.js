/*
define('utils', ['jquery', 'bootstrap', 'error-handler'], function ($) {

    var result = {
        RegisterOnload: RegisterOnload,
        RegisterDocReady: RegisterDocReady,
        Alertify: Alertify,
        Invoke: Invoke,
    };

    return result;
});
 */

    /* AJAX calls. */

    function Invoke(url, data, method, asynchronous, onSuccess, onError) {
        if (url == undefined || url == null || url == '') {
            return;
        }

        if (data == undefined) {
            data = '';
        }

        if (method == undefined) {
            method = 'GET';
        }

        if (asynchronous == undefined) {
            asynchronous = false;
        }

        if (onSuccess == undefined) {
            onSuccess = function () { };
        }

        if (onError == undefined) {
            onError = function () { };
        }

        return $.ajax({
            url: url,
            type: method,
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: asynchronous,
            cache: false,
            success: onSuccess,
            error: onError,
        });
    }

    /* Alert messages. */

    var AlertSeverity = { SUCCESS: 'Success', INFO: 'Info', ERROR: 'Error' };

    function Alertify(message, severity, title) {
        alertMessage(message, severity, title);
    }

    function alertMessage(message, severity, title) {

        var panelClass = '';
        var faClass = '';
        var btnClass = '';
        if (severity == AlertSeverity.SUCCESS) {
            faClass = 'fa fa-check-circle fa-1_3x';
            panelClass = 'panel-success';
            btnClass = 'btn btn-custom-success';
        }
        else if (severity == AlertSeverity.ERROR) {
            faClass = 'fa fa-exclamation-circle fa-1_3x';
            panelClass = 'panel-danger';
            btnClass = 'btn btn-custom-failure';
        }
        else {
            severity = AlertSeverity.INFO;
            faClass = 'fa fa-info-circle fa-1_3x';
            panelClass = 'panel-default';
            btnClass = 'btn btn-custom-default';
        }

        if (!title) {
            title = severity;
        }

        $("#alertify-msg .panel").removeClass('panel-success');
        $("#alertify-msg .panel").removeClass('panel-danger');
        $("#alertify-msg .panel").removeClass('panel-default');
        $("#alertify-msg .panel").addClass(panelClass);

        $("#alertify-msg h4 i").removeClass('fa-check-circle');
        $("#alertify-msg h4 i").removeClass('fa-exclamation-circle');
        $("#alertify-msg h4 i").removeClass('fa-info-circle');
        $("#alertify-msg h4 i").addClass(faClass);

        $("#alertify-msg h4 span").html(title);

        $("#alertify-msg .panel-body .alertify-content").html(message);

        $("#alertify-msg .panel-body .alertify-footer").removeClass('btn-custom-success');
        $("#alertify-msg .panel-body .alertify-footer").removeClass('btn-custom-failure');
        $("#alertify-msg .panel-body .alertify-footer").removeClass('btn-custom-default');
        $("#alertify-msg .panel-body .alertify-footer").addClass(btnClass);

        $("#alertify-msg").modal("show");
    }


    /* Get QueryString parameter. */

    function ShowProcessing(message) {
        if (message) {
            $("#work-in-progress div").html(message);
        }

        $("#work-in-progress").show();
    }

    function HideProcessing() {
        $("#work-in-progress div").html('');
        $("#work-in-progress").hide();
    }


    /* Get QueryString parameter. */

    function getQsValue(name, url) {
        if (!url) url = window.location.href;
        url = url.toLowerCase(); // This is just to avoid case sensitiveness  
        name = name.replace(/[\[\]]/g, "\\$&").toLowerCase();// This is just to avoid case sensitiveness for query parameter name
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    if (!String.prototype.startsWith) {
        String.prototype.startsWith = function (searchString, position) {
            position = position || 0;
            return this.indexOf(searchString, position) === position;
        };
    }

    if (!String.prototype.endsWith) {
        String.prototype.endsWith = function (suffix) {
            return this.indexOf(suffix, this.length - suffix.length) !== -1;
        };
    }


    function RegisterOnload(fn) {
        if (typeof (fn) == 'function') {
            onloadCallbacks.push(fn);
        }
    }

    window.onload = function () {
        for (var i = 0; i < onloadCallbacks.length; i++) {
            try {
                console.log('Running ' + onloadCallbacks[i].toString() + '...');
                onloadCallbacks[i]();
            }
            catch (e) {
                ProcessError(e.message, e.name, e.number, 0, e.stack);
            }
        }
    };


    function RegisterDocReady(fn) {
        if (typeof (fn) == 'function') {
            docReadyCallbacks.push(fn);
        }
    }

    $(document).ready(function () {
        for (var i = 0; i < docReadyCallbacks.length; i++) {
            try {
                docReadyCallbacks[i]();
            }
            catch (e) {
                ProcessError(e.message, e.name, e.number, 0, e.stack);
            }
        }
    });

var onloadCallbacks = [];
var docReadyCallbacks = [];
