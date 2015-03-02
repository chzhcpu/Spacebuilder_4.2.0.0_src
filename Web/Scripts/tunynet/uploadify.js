(function ($) {
    $(function () {
        $('[plugin="uploadify"]').livequery(function () {
            $(this).each(function () {
                var datas = parseObject($(this).attr("data"));
                $(this).uploadify(datas);
            });
        });
    });
}(jQuery));