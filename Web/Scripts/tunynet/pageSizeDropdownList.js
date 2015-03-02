(function ($) {
    //页码选择器
    $('[plugin="pageSize"]').livequery(function () {
        var $select = $(this).find("select");
        $select.change(function () {
            $.cookie('PageSize', $(this).val(), { path: '/' });
            refresh();
        });
        if ($.cookie('PageSize') != null) {
            $select.val($.cookie('PageSize'), { path: '/' });
        }
    });

})(jQuery);
