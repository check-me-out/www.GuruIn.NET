/*
define('common', ['jquery', 'bootstrap', 'utils'], function ($, utils) {

});
*/
    function docReady() {

        var minutesBeforeRefresh = 21;
        setTimeout(function () { window.location.reload(1); }, minutesBeforeRefresh * 60 * 1000);

        if ($('#bootstrap-cdn-down').is(':visible') === true) {
            $('<link href="/Content/Bootstrap/less/css/bootstrap.min.css" rel="stylesheet" type="text/css" />').appendTo('head');
            $('<link href="/Content/Bootstrap/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />').appendTo('head');
        }
    }

    function NavbarManager() {
        if (window.location.pathname == '/') {
            $(".brand-name-xs").addClass('navbar-selected');
            $(".brand-name-non-xs").parent().addClass('navbar-selected');
        }

        $("#menu").children().each(function () {
            var href = $(this).find('a').first().attr('href');
            if (href && href != '/' && window.location.pathname.toLowerCase().startsWith(href.toLowerCase())) {
                $(this).addClass('navbar-selected');
            }
        });

        $(".sub-menu-navbar li").click(function () {
            var val = $(this).find('a').first().attr('href');
            if (val) {
                window.location.href = val;
            }
            return false;
        });

        /*$("#menu > li > ul").on("mouseenter", function () {
            $(this).parent().addClass("navbar-highlight-clone");
        });
        $("#menu > li > ul").on("mouseleave", function () {
            $(this).parent().removeClass("navbar-highlight-clone");
        });
    
        $(".brand-container > ul").on("mouseenter", function () {
            $(this).parent().find('a').first().addClass("navbar-highlight-clone");
        });
        $(".brand-container > ul").on("mouseleave", function () {
            $(this).parent().find('a').first().removeClass("navbar-highlight-clone");
        });*/
    }

    function VirtualKeypadClick() {
        $(".virtual-keypad span").on("click", function (e) {
            var code = $("#code").val();
            code = !code ? '' : code;
            $("#code").val(code + $(this).text());
        });
    }

    function ShowUserMessage() {
        var msg = $("#user-message").val();
        var msgSeverity = $("#user-message-severity").val();
        var title = $("#user-message-title").val();

        $("#user-message").val('');
        $("#user-message-severity").val('');
        $("#user-message-title").val('');

        if (msg) {
            Alertify(msg, msgSeverity, title);
        }
    }

    function AdjustMainContentHeight(e) {
        var headerHeight = $('nav').outerHeight() + $('.breadcrumbs-container').outerHeight();
        var footerMenuHeight = $('.footer-menu').outerHeight();
        var footerHeight = $('footer').outerHeight();
        var windowHeight = $(window).outerHeight();
        var mainContentHeight = windowHeight - headerHeight - footerMenuHeight - footerHeight;
        $('#main-content').css({ 'min-height': mainContentHeight + 6 + 'px' });
    }

    RegisterDocReady(docReady);

    RegisterOnload(AdjustMainContentHeight);
    window.onresize = AdjustMainContentHeight;

    RegisterOnload(NavbarManager);
    RegisterOnload(VirtualKeypadClick);
    RegisterOnload(ShowUserMessage);
