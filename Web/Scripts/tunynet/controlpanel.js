(function ($) {
    //后台右侧的菜单显示控制
    $('[plugin="ShortcutMenu"]').livequery(function () {
        $parent = $(this).parent();
        $parent.removeClass("tn-open");
        $parent.removeClass("tn-close");
        if ($.cookie('ShortcutMenuIsOpen') == 'false' || $.cookie('ShortcutMenuIsOpen') == false) {
            $parent.addClass("tn-close");
        } else {
            $parent.addClass("tn-open");
        }
        $(this).click(function () {
            $parent = $(this).parent();
            if ($parent.is(".tn-open")) {
                $parent.removeClass("tn-open");
                $parent.addClass("tn-close");
                $.cookie('ShortcutMenuIsOpen', null); // 删除
                $.cookie('ShortcutMenuIsOpen', false, { path: '/', expires: 7 }); //设置带时间的cookie  7天
            } else {
                $.cookie('ShortcutMenuIsOpen', null); // 删除
                $.cookie('ShortcutMenuIsOpen', true, { path: '/', expires: 7 }); //设置带时间的cookie  7天
                $parent.removeClass("tn-close");
                $parent.addClass("tn-open");
            };
        });
    });
})(jQuery);
