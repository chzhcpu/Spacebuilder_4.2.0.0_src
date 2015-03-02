(function ($) {
    //焦点改变的时候
    $.fn.OnFocusChange = function (focusClass, outFocusClass, exclude) {
        var comments = this;
        this.each(function () {
            $(this).die();
            $comment = $(this);
            $focusClass = focusClass || $(this).data("focus");
            $outFocusClass = outFocusClass || $(this).data("outfocus");
            if ($outFocusClass)
                $comment.addClass($outFocusClass);
            if ($focusClass)
                $comment.removeClass($focusClass);
            $(this).click(function () {
                $comment = $(this);
                $focusClass = $comment.data("focus");
                $outFocusClass = $comment.data("outfocus");
                if ($outFocusClass)
                    $comment.removeClass($outFocusClass);
                if ($focusClass)
                    $comment.addClass($focusClass);
                $exclude = exclude || $comment.data("exclude");
                $(document).bind("click", function (evt) {
                    if ($outFocusClass)
                        comments.addClass($outFocusClass);
                    if ($focusClass)
                        comments.removeClass($focusClass);
                    if ($(evt.target).closest($comment).length > 0 || ($exclude && $(evt.target).is($exclude))) {
                        if ($outFocusClass)
                            $comment.removeClass($outFocusClass);
                        if ($focusClass)
                            $comment.addClass($focusClass);
                    } else {
                        $(document).unbind("click", arguments.callee);
                    }
                });
            });
        });
    }

    //根据焦点添加和删除样式。
    $('div[plugin="OnFocus"]').livequery(function () {
        $comment = $(this);
        $focusClass = $(this).data("focus");
        $outFocusClass = $(this).data("outfocus");
        if ($outFocusClass)
            $comment.addClass($outFocusClass);
        if ($focusClass)
            $comment.removeClass($focusClass);
        $(this).click(function () {
            $comment = $(this);
            $focusClass = $comment.data("focus");
            $outFocusClass = $comment.data("outfocus");
            if ($outFocusClass)
                $comment.removeClass($outFocusClass);
            if ($focusClass)
                $comment.addClass($focusClass);
            $exclude = $comment.data("exclude");
            $(document).bind("click", function (evt) {
                if ($outFocusClass)
                    $('div[plugin="OnFocus"]').addClass($outFocusClass);
                if ($focusClass)
                    $('div[plugin="OnFocus"]').removeClass($focusClass);
                if ($(evt.target).closest($comment).length > 0 || ($exclude && $(evt.target).is($exclude))) {
                    if ($outFocusClass)
                        $comment.removeClass($outFocusClass);
                    if ($focusClass)
                        $comment.addClass($focusClass);
                } else {
                    $(document).unbind("click", arguments.callee);
                }
            });
        });
    });

})(jQuery);
