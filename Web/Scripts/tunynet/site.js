/// <reference path="~/Bundle/Scripts/Site" />

(function ($) {
    //交互状态 
    $(document).ready(function () {
        $('.tn-state-default,.tn-menu-item').live("mouseover", function () {
            $(this).addClass('tn-state-hover');
        }).live("mouseout", function () {
            $(this).removeClass('tn-state-hover');
        });

        //给更多里的链接添加样式
        $('.tn-more-options ul a.tn-item-link').live("mouseover", function () {
            $(this).addClass("tn-bg-gray");
        }).live("mouseout", function () {
            $(this).removeClass("tn-bg-gray");
        });

        //表格
        $('.tn-table-grid-row').live("mouseover", function () {
            $(this).addClass("tn-bg-gray");
        }).live("mouseout", function () {
            $(this).removeClass("tn-bg-gray");
        });

        //按钮
        $('.tn-button-default').live("mouseover", function () {
            $(this).addClass('tn-button-default-hover');
        }).live("mouseout", function () {
            $(this).removeClass('tn-button-default-hover');
        });

        $('.tn-button-primary').live("mouseover", function () {
            $(this).addClass('tn-button-primary-hover');
        }).live("mouseout", function () {
            $(this).removeClass('tn-button-primary-hover')
        });

        $('.tn-button-secondary').live("mouseover", function () {
            $(this).addClass('tn-button-secondary-hover');
        }).live("mouseout", function () {
            $(this).removeClass('tn-button-secondary-hover');
        });

        $('.tn-button-lite').live("mouseover", function () {
            $(this).addClass('tn-button-default');
        }).live("mouseout", function () {
            $(this).removeClass('tn-button-default');
        });

        //标签式导航
        $('.spb-nav1-area li.tn-list-item-position:not(.tn-navigation-active)').hover(function () {
            $(this).addClass('tn-navigation-hover');
        }, function () {
            $(this).removeClass('tn-navigation-hover');
        });
    });

    $(document).ready(function () {

        // AjaxAction请求@html.ajaxAction
        $('[plugin="ajaxAction"] ').livequery(function () {
            var $this = $(this);
            var data = parseObject($(this).attr("data"));
            $.get(data.url, function (html) {
                $this.replaceWith(html);
            });
        });

        //弹出提示消息
        $('[plugin="tipsy"]').livequery(function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoNS });
        });

        //添加水印
        $.watermark.options.hideBeforeUnload = false;
        $('input[type="text"][watermark],textarea[watermark]').livequery(function () {
            $(this).watermark($(this).attr("watermark"));
        });

        //用户卡片气泡样式显示
        $("[plugin='tipsyHoverCard']").livequery(function () {
            $(this).tipsyHoverCard();
        });

        //拉黑
        $("a[id^='StopUser_']").live("click", function (e) {
            e.preventDefault();
            $this = $(this);

            art.dialog.confirm('加入黑名单会解除关系，确定要加入黑名单吗？&nbsp;&nbsp;', function () {
                $.ajax({
                    type: "Post",
                    url: $this.attr("href"),
                    success: function () {
                        $this.remove();
                        art.dialog.tips("加入黑名单成功", 1.5, 1);
                    },
                    error: function () {
                        art.dialog.tips("加入黑名单失败", 1.5, -1);
                    }
                });

            });
        });
    });

    //返回顶部
    jQuery.fn.topLink = function (settings) {
        settings = jQuery.extend({
            min: 1,
            fadeSpeed: 200,
            ieOffset: 50
        }, settings);
        return this.each(function () {
            //listen for scroll  
            var el = $(this);
            el.css('display', 'none'); //in case the user forgot
            $(window).scroll(function () {
                if (!jQuery.support.hrefNormalized) {
                    el.css({
                        'position': 'absolute',
                        'top': $(window).scrollTop() + $(window).height() - 100
                    });
                }
                if ($(window).scrollTop() >= settings.min) {
                    el.show();
                }
                else {
                    el.hide();
                }
            });
        });
    };

    $(document).ready(function () {
        $('#to_top').topLink({
            min: 10,
            fadeSpeed: 500
        });
        //smoothscroll
        $('#to_top').click(function (e) {
            e.preventDefault();
            $.scrollTo(0, 300);
        });
    });

})(jQuery);

//刷新当前页面
function refresh() {
    window.location.reload();
}

//转为对象类型
function parseObject(value) {
    if (value == null)
        return null;
    return eval("(" + value + ")");
}

//字符串格式化方法扩展，用于模板字符串替换
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g, function (m, i) {
        return args[i];
    });
}
