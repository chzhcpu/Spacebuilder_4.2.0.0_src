
//插入视频、话题、图片、音乐
(function ($) {
    $('a[id$=_videobtn],a[ntype=video]').live("click", function (event) {
        event.preventDefault();
        var $mediaContainer = $('#videoContainer');
        $clickObj = $(this);
        if ($mediaContainer.length == 0) {
            var href = $clickObj.attr('href');
            if (!href || href == 'javascript:;') {
                if ($('textarea[custombuttons]').length) {
                    var data = $.parseJSON($('textarea[custombuttons]').attr('custombuttons'));
                    href = data.videoButton + '?textAreaId=' + $('textarea[custombuttons]').attr('id');
                }
            }
            $.get(href, function (data) {
                $('body').append(toggleMediaContainerStatus($(data), $clickObj));
            });
        }
        else {
            toggleMediaContainerStatus($mediaContainer, $clickObj);
        }
    });

    $('a[ntype=topic]').live("click", function (event) {
        event.preventDefault();
        var $mediaContainer = $('#topicContainer');
        $clickObj = $(this);

        if ($mediaContainer.length == 0) {
            var href = $clickObj.attr('href');
            $.get(href, { date: new Date().getTime() }, function (data) {
                $('body').append(toggleMediaContainerStatus($(data), $clickObj));
            });
        }
        else {
            toggleMediaContainerStatus($mediaContainer, $clickObj);
        }
    });

    $('a[ntype=image]').die("click").live("click", function (event) {
        event.preventDefault();
        var $mediaContainer = $('#imageContainer');
        $clickObj = $(this);

        if ($mediaContainer.length == 0) {

            var href = $clickObj.attr('href');
            $.ajaxSetup({ cache: true });
            $.get(href, { date: new Date().getTime() }, function (data) {
                $('body').append(toggleMediaContainerStatus($(data), $clickObj));
            });
        }
        else {
            toggleMediaContainerStatus($mediaContainer, $clickObj);
        }
    });


    $('a[id$=_musicbtn],a[ntype=music]').live("click", function (event) {
        event.preventDefault();
        var $mediaContainer = $('#musicContainer');
        $clickObj = $(this);

        if ($mediaContainer.length == 0) {
            var href = $clickObj.attr('href');
            if (!href || href == 'javascript:;') {
                if ($('textarea[custombuttons]').length) {
                    var data = $.parseJSON($('textarea[custombuttons]').attr('custombuttons'));
                    href = data.musicButton + '?textAreaId=' + $('textarea[custombuttons]').attr('id');
                }
            }

            $.get(href, { date: new Date().getTime() }, function (data) {
                $('body').append(toggleMediaContainerStatus($(data), $clickObj));
            });
        }
        else {
            toggleMediaContainerStatus($mediaContainer, $clickObj);
        }
    });

    function toggleMediaContainerStatus(obj, clickObj) {

        if (obj.is(":hidden")) {
            var $parentNode = clickObj.parent();
            var position, top, left;

            if ($parentNode.is('span') && $parentNode.parents('table.aui_dialog').length == 0) {
                position = $parentNode.offset();
                top = (position.top + 15);
                left = position.left;
            }
            else {
                position = clickObj.offset();
                top = position.top + 15;
                left = position.left - 17;
            }
            obj.attr("style", "display:block;top:" + top + "px; left:" + left + "px;");
        }
        else {
            obj.hide();
        }

        $(document).bind("click", function (e) {
            $(document).unbind("click", arguments.callee);
            if ($(e.target).is($('*:not(.tn-smallicon-cross)', obj))) {
                return;
            }
            obj.hide();
        });

        return obj;
    }

    var loadingMusic = false;
    $('a[ntype="mediaPlay"]').live("click", function (event) {
        event.preventDefault();
        if (loadingMusic) {
            return false;
        }

        var _this = $(this);
        if (_this.siblings('dl').length == 0) {
            loadingMusic = true;
            $.get(_this.attr('href'), function (data) {
                _this.parent().append(data);
                loadingMusic = false;
                _this.siblings('a[ntype="mediaPlay"],br').andSelf().hide();
            });
        }
    });

    $('a[ntype="closeMedia"]').live("click", function (event) {
        event.preventDefault();

        var parent = $(this).parents('dl');
        parent.siblings('a[ntype=mediaPlay],br').show();
        parent.remove();
    });

})(jQuery);
