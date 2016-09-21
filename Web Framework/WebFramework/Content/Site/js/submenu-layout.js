
function init() {

    $('#sub-menu-collapse').html($('#sidebar-items').html());

    $(".submenu").children().each(function () {
        var href = $(this).find('a').first().attr('href');
        if (href == window.location.pathname || href == window.location.href) {
            $(this).addClass('submenu-highlight');
        }
        else {
            $(this).removeClass('submenu-highlight');
        }
    });

    $(".submenu li").click(function () {
        var val = $(this).find('a').first().attr('href');
        if (val) {
            window.location.href = val;
        }
        return false;
    });
}

RegisterOnload(init);

RegisterOnload(AdjustSidebarHeight);
window.onresize = AdjustSidebarHeight;

function AdjustSidebarHeight(e) {
    var mainContentHeight = $('#main-content').outerHeight(true);
    $('#sidebar-items').css({ 'height': mainContentHeight + 'px' });
}
