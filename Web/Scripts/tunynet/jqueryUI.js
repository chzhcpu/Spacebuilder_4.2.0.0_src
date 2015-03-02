/// <reference path="jquery-1.7.1.js" />
/// <reference path="jquery-ui-1.8.7.js" />
(function ($) {


    ///zhengw:用于解决IE9下关闭不了上传图片对话框问题，
    ///问题描述：jqueryUI1.8.7有个很变态的地方，它会重写cleanData，把对的改成了错的，
    ///用本文件再次替换为jquery类库中的方法。
    $.cleanData = function (elems) {
        var data, id,
			cache = jQuery.cache,
			special = jQuery.event.special,
			deleteExpando = jQuery.support.deleteExpando;

        for (var i = 0, elem; (elem = elems[i]) != null; i++) {
            if (elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()]) {
                continue;
            }

            id = elem[jQuery.expando];

            if (id) {
                data = cache[id];

                if (data && data.events) {
                    for (var type in data.events) {
                        if (special[type]) {
                            jQuery.event.remove(elem, type);

                            // This is a shortcut to avoid jQuery.event.remove's overhead
                        } else {
                            jQuery.removeEvent(elem, type, data.handle);
                        }
                    }

                    // Null the DOM reference to avoid IE6/7/8 leak (#7054)
                    if (data.handle) {
                        data.handle.elem = null;
                    }
                }

                if (deleteExpando) {
                    delete elem[jQuery.expando];

                } else if (elem.removeAttribute) {
                    elem.removeAttribute(jQuery.expando);
                }

                delete cache[id];
            }
        }
    }

    $(function () {
        //tabs 插件
        $('[plugin="tabs"]').livequery(function () {
            var data = parseObject($(this).attr("data"));
            if (data.select)
                data.select = eval(data.select);
            if (data.isSample)
                $(this).spbtabs(data);
            else
                $(this).tabs(data);
            if ($("a[tabtarget]", $(this)).length)
                $(this).bind("tabsselect", function (event, ui) {
                    var url = $.data(ui.tab, 'load.tabs');
                    if (data.isSample)
                        url = $.data(ui.tab, 'load.spbtabs');
                    if (url) {
                        if ($(ui.tab).is("a[tabtarget='self']")) {
                            location.href = url;
                            return false;
                        }
                        else if ($(ui.tab).is("a[tabtarget='blank']")) {
                            window.open($.data(ui.tab, 'load.tabs'));
                            if (data.isSample)
                                window.open($.data(ui.tab, 'load.spbtabs'));

                            return false;
                        }
                    }
                    return true;
                });
        });

        //日期选择器
        $('[plugin="datetimepicker"]').livequery(function () {
            var data = parseObject($(this).attr("data"));
            var minDate = $(this).attr("data-val-rangedate-min");
            var maxDate = $(this).attr("data-val-rangedate-max");
            if (minDate)
                $.extend(data, { minDate: minDate });
            if (maxDate)
                $.extend(data, { maxDate: maxDate });
            if (data.onSelect)
                data.onSelect = eval(data.onSelect);
            if (data.onClose)
                data.onClose = eval(data.onClose);
            if (data.beforeShow)
                data.beforeShow = eval(data.beforeShow);
            if (Boolean(data.showTime))
                $(this).datetimepicker(data);
            else
                $(this).datepicker(data);
        });
    });
}(jQuery));