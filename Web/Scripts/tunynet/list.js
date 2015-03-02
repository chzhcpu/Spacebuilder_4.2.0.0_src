
//全选 or 取消全选
function checkAll(allCheckBox, itemName) {
    var items = document.getElementsByName(itemName);
    for (var i = 0; i < items.length; i++) {
        if (items[i].type == 'checkbox') {
            items[i].checked = allCheckBox.checked;
        }
    }
}

(function ($) {
    //获取更多控件的脚本
    $('[plugin="GetMore"]').livequery(function () {
        var autoLoadPagecount = $(this).data("pagecount");
        if (!$.pageindex)
            $.pageindex = 1;
        var $this = $(this);
        if (autoLoadPagecount > 0) {
            $(window).unbind("scroll").scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                if ($(document).height() <= totalheight && $.pageindex <= autoLoadPagecount) {
                    $this.click();
                }
            });
        }
        $(this).click(function (e) {
            e.preventDefault();
            $this = $(this);
            $this.hide();
            if ($this.nextAll(".tn-loading").length > 0)
                $this.nextAll(".tn-loading").remove();
            $this.after("<div class='tn-loading tn-border-gray tn-corner-all'></div>");
            $link = $(this).find("a");
            $.get($link.attr("href"), function (data) {
                var $next = $this.next();
                if ($next.is(".tn-loading")) {
                    $next.remove();
                }
                $this.replaceWith(data);
                $.pageindex++;
            });
        });
    });

    //Ajax分页ajaxPagingButton
    $('[plugin="ajaxPagingButton"] a').live("click", function () {
        var data = parseObject($(this).parent().attr("data"));
        $.get(this.href, function (html) {
            $("#" + data.updateTargetId).replaceWith(html);
        });
        return false;
    });

    //AJAX删除按钮
    $("a[plugin='AjaxDeleteButton']").livequery(function () {
        $(this).unbind("click").click(function () {
            var $this = $(this);
            var postHref = $this.attr("href");
            var datainfo = parseObject($(this).attr("data"));

            art.dialog.confirm(datainfo.confirm, function () {
                $.post(postHref, function (data) {
                    if (data.MessageType >= 0) {
                        $(datainfo.deleteTarget).slideUp("fast", function () {
                            $this.remove();
                            art.dialog.tips(data.MessageContent, 1.5, data.MessageType);
                            var fnsucess = eval(datainfo.SuccessFn);
                            if (fnsucess)
                                fnsucess(data, $this);
                        });
                    }
                    else {
                        var fnerror = eval(datainfo.ErrorFn);
                        if (fnerror)
                            fnerror(data);
                        else
                            alert(data.MessageContent);
                    }
                });
            }, function () {
            });

            return false;
        });
    });


})(jQuery);
