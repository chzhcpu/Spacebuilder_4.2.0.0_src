/// <reference path="jquery-1.7.1.js" />
/// <reference path="jquery-ui-1.8.7.js" />
///zhengw:用于解决IE9下关闭不了上传图片对话框问题，
///问题描述：jqueryUI1.8.7有个很变态的地方，它会重写cleanData，把对的改成了错的，
///用本文件再次替换为jquery类库中的方法。
(function ($) {

    /*
   *快捷回复：响应Ctrl+Enter快捷键
   */
    $.fn.ShortcutSubmit = function () {
        this.keyup(function (e) {
            var stat = false;
            if (e.keyCode == 17) {
                stat = true;
                //取消等待
                setTimeout(function () {
                    stat = false;
                }, 500);
            }
            if (((e.keyCode || e.which) == 13) && (e.ctrlKey || stat)) {
                $(this).parents("form:first").submit();
            }
        })
    }

    $(document).ready(function () {
        //响应ctrl+enter按键
        $("[plugin='ShortcutSubmit']").livequery(function () {
            $(this).ShortcutSubmit();
        });
    });
})(jQuery);