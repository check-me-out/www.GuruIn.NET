/*
define('fiddle-scripts', ['jquery', 'bootstrap'], function ($) {

});
 */
    function init() {
        $("#css-code").val('.sample {\r    color: #2e6da4;\r    font-weight: bold;\r}\rdiv {\r    padding: 5px;\r}\r');
        $("#html-code").val('<div class="sample">\r    <div>Just what you expected?</div>\r</div>\r');
        var jsInitCode = '$(".sample").append(\'<button id="clear-demo" class="btn btn-primary">DIY Now!</button>\');';
        jsInitCode += '\r\r$("#clear-demo").click(function() {\r    $("#btn-clear").trigger(\'click\'); \r});';
        $("#js-code").val(jsInitCode);

        $("#output-html").html('<span style="color: #999; padding: 6px 12px">CLICK RUN...</span>');

        $("#btn-clear").on("click", function () {
            $("#css-code").val('');
            $("#output-css").html('');

            $("#html-code").val('');
            $("#output-html").html('<span style="color: #999; padding: 6px 12px">OUTPUT</span>');

            $("#js-code").val('');
            $("#output-js").html('');
        });

        $("#btn-run").on("click", function () {
            RunFiddle();
        });

        /* JS CONSOLE */

        var value = $("#jsCode").val();
        if (value) {
            $("#clearCode").show();
        }
        else {
            $("#clearCode").hide();
        }

        $("#jsCode").on("keyup", function (event) {
            var value = $("#jsCode").val();
            if (value) {
                $("#clearCode").show();
            }
            else {
                $("#clearCode").hide();
            }

            if (event.keyCode == 13) {
                RunJsCode();
                return false;
            }
        });

        Alertify('Testing custom "sucess" popup message!', AlertSeverity.SUCCESS);

        JSErrorOnPurposeForDemo
    }

    RegisterOnload(init);


    function RunFiddle() {
        var css = $("#css-code").val();

        var lines = css.split('}');
        for (var i = 0; i < lines.length; i++) {
            lines[i] = ' #output-html ' + lines[i];
        }
        var contextCss = lines.join(' } ');
        if (contextCss.trim().endsWith('#output-html')) {
            contextCss = contextCss.substring(0, contextCss.lastIndexOf('#output-html'));
        }

        $("#output-css").html('<style type="text/css">' + contextCss + '</style>');

        var html = $("#html-code").val();
        $("#output-html").html(html);

        var js = $("#js-code").val();
        $("#output-js").html('<script type=\"text/javascript\">' + js + "</scr" + "ipt>");
    }

    function RunJsCode() {
        var input = $("#jsCode").val();
        if (input) {
            var question = '<div> « <span>' + input + '</span></div>';

            var output = '';
            try {
                output = eval(input);// OR $.globalEval(input) OR (window.execScript || window.eval)(input);
            }
            catch (e) {
                output = '<span style="color: #d00">' + e + '</span>';
            }

            if (output == undefined) {
                output = '<span style="color: #d00">undefined</span>';
            }
            else {
                output = '<span style="color: #090">' + output + '</span>';
            }

            var answer = '<div> » ' + output + '</div>';

            $("#js-console-output").append(question);
            $("#js-console-output").append(answer);
            $("#js-console-output").append('<div class="separator"></div>');

            $("#js-console-output-parent").scrollTop($("#js-console-output").height() + '');

            $("#jsCode").val('');
            $("#clearCode").hide();
        }
    }
