(function ($) {
    //动态操作脚本
    $('a[plugin="activityOperation"]').live("click", function () {
        $li = $('input[name="' + $(this).attr("name") + '"]').closest("li.tn-list-item");
        $.post($(this).attr("href"), function (data) {
            if (data.MessageType >= 0) {
                $li.slideUp("fast");
            }
            else {
                alert(data.MessageContent);
            }
        });
        return false;
    });
})(jQuery);
