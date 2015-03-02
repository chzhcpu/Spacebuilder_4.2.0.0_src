/*
* Version: Beta 2
* Release: 2010-5-29
*/
(function ($) {
    //表情选择器
    var $emotion;
    $('a[id$=_smileybtn],span[ntype=emotion]').live("click", function () {
        var $emotionSelector = $('#emotionSelector');
        $emotion = $(this);
        if ($emotionSelector.length == 0) {
            var emotionUrl = $("input[name='list-emotions']").val();
            if (emotionUrl == undefined) {
                return false;
            }
            $.get(emotionUrl, function (data) {
                $('body').append(toggleEmotionSelectorStatus($(data), $emotion));
            });
        }
        else {
            $("input[name='list-emotions']").empty();
        }
        if ($emotionSelector.is(":hidden")) {
            toggleEmotionSelectorStatus($emotionSelector, $emotion);
        }
        return false;
    });

    function toggleEmotionSelectorStatus(obj, clickObj) {
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
            if ($('div[id=emotion-container]', obj).children('div[id=listEmotions-]').length <= 0) {
                $("#emotion-tabs a:first", obj).one('click', function () {
                    bindEmotionTabClickEvent($(this));
                    return false;
                }).click();
            }
        }
        else {
            obj.hide();
        }

        $(document).bind("click", function (e) {
            $(document).unbind("click", arguments.callee);
            var $emotionTabs = $("#emotion-tabs *", $(this));
            if ($(e.target).is($emotionTabs)) {
                return;
            }

            obj.hide();
        });

        return obj;
    }

    $("#emotion-tabs a").live('click', function () {
        bindEmotionTabClickEvent($(this));
        return false;
    });

    function bindEmotionTabClickEvent(obj) {

        var $emotionTabs = obj.parents("#emotion-tabs");
        $("li", $emotionTabs).removeClass("tn-tabs-selected");
        obj.parent().addClass("tn-tabs-selected");

        var value = obj.attr("value");
        var $listEmotions = $("#listEmotions-" + value, $emotionTabs.siblings('#emotion-container'));


        if ($listEmotions.length > 0) {
            $listEmotions.show();
        }
        else {
            LoadEmotions($emotionTabs.siblings('#emotion-container'), value, obj.attr("ohref"));
        }
        $("div[id^='listEmotions-']:not(#listEmotions-" + value + ")", $("#emotion-container")).hide();
    }

    function LoadEmotions(obj, value, ohref) {
        if (obj.children('#listEmotions-' + value).length > 0)
            return;

        $.getJSON(ohref, { directoryName: value }, function (data) {

            var $listEmotions = $("<div id='listEmotions-" + value + "' class='tn-emotion-list tn-helper-clearfix' style='height:120px;'></div>");
            $(data.Emotions).each(function () {

                $listEmotions.append("<span class=\"tn-border-gray\"><img src='" + this.ImgUrl + "' alt='" + this.Description +
                                     "' title='" + this.Description + "' value=\"" + this.Code + "\"/></span>");
            });

            $("#emotion-container").prepend($listEmotions);

            //鼠标移至表情时，显示预览图片
            $("span", obj).mouseover(function (e) {
                e.preventDefault();

                var imgHtml = "<span class='tn-widget-content tn-border-gray'><img src='" + $(this).children("img").attr("src") + "' alt='" + $(this).children("img").attr("alt") +
                     "' title='" + $(this).children("img").attr("title") + "' value='" + $(this).children("img").attr("value") + "'style=\"max-height:40px;max-width:40px;\"/></span>";

                if ($(this).index() % 12 > 5)
                    $("#leftOriginal").html(imgHtml);
                else
                    $("#rightOriginal").html(imgHtml);
            }).mouseout(function () { $("#emotion-container div.tn-emotion-original").html(""); }); // end img mouseover

            $("img", obj).parent().off('click').click(function (e) {

                e.preventDefault();

                $addEmotion = $('#addEmotions-');
                $img = $(this).find('img');

                $textArea = $emotion.parents('form').find('textarea');
                if ($emotion.is("a[id$=_smileybtn]")) {
                    var imgHtml = "<span><img src='" + $img.attr("src") + "' alt='" + $img.attr("alt") +
                    "' title='" + $img.attr("title") + "' value=\"" + $img.attr("value") + "\"/></span>";
                    var $tinyMCE = $('#' + tinyMCE.activeEditor.id);
                    $tinyMCE.insertContentToEditor(imgHtml);
                }
                else {
                    if ($.watermark)
                        $.watermark.hide($textArea);
                    insertAtCaret($textArea[0], $img.attr("value"));
                }
                $('#emotionSelector').hide();
                return false;
            });  // end img click
        });   // end $.get
    }

})(jQuery);

//向文本域中插入文本
function insertAtCaret(textObj, textFeildValue) {
    if (document.all && textObj.createTextRange && textObj.caretPos) {
        var caretPos = textObj.caretPos;
        caretPos.text = caretPos.text.charAt(caretPos.text.length - 1) == '' ? textFeildValue + '' : textFeildValue;
    } else if (textObj.setSelectionRange) {
        var rangeStart = textObj.selectionStart;
        var rangeEnd = textObj.selectionEnd;
        var tempStr1 = textObj.value.substring(0, rangeStart);
        var tempStr2 = textObj.value.substring(rangeEnd);
        textObj.value = tempStr1 + textFeildValue + tempStr2;
        textObj.focus();
        var len = textFeildValue.length;
        textObj.setSelectionRange(rangeStart + len, rangeStart + len);
        //textObj.blur();
    } else {
        textObj.value += textFeildValue;
    }
}
