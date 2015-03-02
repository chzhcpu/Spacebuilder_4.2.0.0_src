(function ($) {
    //模式框
    $('a[plugin="dialog"]').live("click", function () {
        var id = this.id || "tunynet_dialog";
        var data = parseObject($(this).attr("data"));
        data = $.extend({ id: id, title: false }, data || {});
        if (data.close) {
            data.close = eval(data.close);
        }
        else
            data.close = function () {
                if (typeof (tinyMCE) != "undefined")
                    if (tinyMCE.activeEditor != undefined)
                        tinyMCE.activeEditor.destroy();
            }
        data.init = eval(data.init);
        data.show = eval(data.show);
        var dialog = art.dialog(data);
        $.get(this.href, function (html) {
            if (typeof html == "object" && html.MessageContent) {
                dialog.close();
                art.dialog.tips(html.MessageContent, 1.5, html.MessageType);
            }
            else
                dialog.content(html);
        });
        return false;
    });

    $('[dialogOperation = "closeAll"]').live("click", function () {
        var list = art.dialog.list;
        for (var i in list) {
            list[i].close();
        };
        return false;
    });
    $('[dialogOperation = "close"]').live("click", function () {
        var dialog = artDialog.focus;
        if (dialog)
            dialog.close();
        return false;
    });
    $('[dialogOperation = "hide"]').live("click", function () {
        var dialog = artDialog.focus;
        if (dialog)
            dialog.hide();
        return false;
    });
})(jQuery);
