$(function () {
    UE.registerUI('emotion', function (editor, uiName) {
        var btn = new UE.ui.Button({
            name: '表情',
            title: '表情',
            cssRules: 'background-position: -60px -20px;',
            onclick: function () {
                var $emotionSelector = $('#emotionSelector');
                $emotion = $(this);
                if ($emotionSelector.length == 0) {
                    var emotionUrl = $("input[name='list-emotions']").val();
                    if (emotionUrl == undefined) {
                        return false;
                    }
                    $.get(emotionUrl, function (data) {
                        $('body').append(toggleEmotionSelectorStatus($(data), $(btn.target)));
                    });
                } else {
                    toggleEmotionSelectorStatus($emotionSelector, $(btn.target));
                }
                return false;
            }
        });
        editor.addListener('selectionchange', function () {
            var state = editor.queryCommandState(uiName);
            if (state == -1) {
                btn.setDisabled(true);
                btn.setChecked(false);
            } else {
                btn.setDisabled(false);
                btn.setChecked(state);
            }
        });
        return btn;
    }, 58);

    UE.registerUI('music', function (editor, uiName) {
        var btn = new UE.ui.Button({
            name: '音乐',
            title: '音乐',
            cssRules: 'background-position: -18px -40px;',
            onclick: function () {
                var musicUrl = $("input[name='list-music']").val();
                if (musicUrl == undefined) {
                    return false;
                }
                var editorId = UE.instants.ueditorInstant0.key;
                $.get(musicUrl + "?textareaId=" + editorId, function (html) {
                    if ($("#musicContainer")[0]) {
                        $("#musicContainer").show();
                        return;
                    } else {
                        $("body").append(toggleMediaContainerStatus($(html), $(btn.target)));
                        $("#musicContainer").show();
                    }
                });
                return false;
            }
        });
        editor.addListener('selectionchange', function () {
            var state = editor.queryCommandState(uiName);
            if (state == -1) {
                btn.setDisabled(true);
                btn.setChecked(false);
            } else {
                btn.setDisabled(false);
                btn.setChecked(state);
            }
        });
        return btn;
    }, 60);

    UE.registerUI('file', function (editor, uiName) {
        var btn = new UE.ui.Button({
            name: '附件',
            title: '附件',
            cssRules: 'background-position: -620px -40px;',
            onclick: function () {
                var fileUrl = $("input[name='list-files']").val();
                if (fileUrl == undefined) {
                    return false;
                }
                var dialog = art.dialog({ id: editor.id + "_uploadFileDialog", title: false });
                $.get(fileUrl, function (html) {
                    dialog.content(html);
                });
                return false;
            }
        });
        editor.addListener('selectionchange', function () {
            var state = editor.queryCommandState(uiName);
            if (state == -1) {
                btn.setDisabled(true);
                btn.setChecked(false);
            } else {
                btn.setDisabled(false);
                btn.setChecked(state);
            }
        });
        return btn;
    }, 62);

    UE.registerUI('image', function (editor, uiName) {
        var btn = new UE.ui.Button({
            name: '图片',
            title: '图片',
            cssRules: 'background-position:-726px -77px;',
            onclick: function () {
                var imageUrl = $("input[name='list-images']").val();
                if (imageUrl == undefined) {
                    return false;
                }
                var dialog = art.dialog({ id: editor.id + "_uploadimagesDialog", title: false });
                $.get(imageUrl, function (html) {
                    dialog.content(html);
                });
                return false;
            }
        });
        editor.addListener('selectionchange', function () {
            var state = editor.queryCommandState(uiName);
            if (state == -1) {
                btn.setDisabled(true);
                btn.setChecked(false);
            } else {
                btn.setDisabled(false);
                btn.setChecked(state);
            }
        });
        return btn;
    }, 61);

    UE.registerUI('atuser', function (editor, uiName) {
        var btn = new UE.ui.Button({
            name: '朋友',
            title: '@朋友',
            cssRules: 'background-position: -723px -101px;',
            onclick: function () {
                var userUrl = $("input[name='list-atuser']").val();
                if (userUrl == undefined) {
                    return false;
                }

                var editorId = UE.instants.ueditorInstant0.key;
                $.get(userUrl + "?textareaId=" + editorId + "&seletorId=" + btn.id, function (html) {
                    if ($("div[id^=atUserView]").length > 0) {
                        return;
                    } else {
                        $("body").append(html);
                    }
                });
                return false;
            }
        });
        editor.addListener('selectionchange', function () {
            var state = editor.queryCommandState(uiName);
            if (state == -1) {
                btn.setDisabled(true);
                btn.setChecked(false);
            } else {
                btn.setDisabled(false);
                btn.setChecked(state);
            }
        });
        return btn;
    }, 63);

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

            var $emotionTabs = $("#emotion-tabs *", $(this));
            if ($(e.target).is($emotionTabs) || $(e.target).hasClass("edui-icon")) {
                return;
            }
            $(document).unbind("click", arguments.callee);
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
                $img = $(this).find('img');

                var editorId = UE.instants.ueditorInstant0.key;
                var ue = UE.getEditor(editorId);
                ue.ready(function () {
                    var emotion = {
                        src: $img.attr("src"),
                        alt: $img.attr("alt"),
                        title: $img.attr("title")
                    };
                    ue.execCommand('insertimage', emotion);
                });
                $('#emotionSelector').hide();
                return false;
            });  // end img click
        });   // end $.get
    }

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

    //var loadingMusic = false;
    //$('a[ntype="mediaPlay"]').live("click", function (event) {
    //    event.preventDefault();
    //    if (loadingMusic) {
    //        return false;
    //    }

    //    var _this = $(this);
    //    if (_this.siblings('dl').length == 0) {
    //        loadingMusic = true;
    //        $.get(_this.attr('href'), function (data) {
    //            _this.parent().append(data);
    //            loadingMusic = false;
    //            _this.siblings('a[ntype="mediaPlay"],br').andSelf().hide();
    //        });
    //    }
    //});

    //$('a[ntype="closeMedia"]').live("click", function (event) {
    //    event.preventDefault();

    //    var parent = $(this).parents('dl');
    //    parent.siblings('a[ntype=mediaPlay],br').show();
    //    parent.remove();
    //});
    $("textarea[plugin='ueditor']").livequery(function () {
        var id = $(this).attr("id");
        var data = $.parseJSON($(this).attr("data"));
        var maximumWords = $(this).attr("maximumWords");
        var ue;
        if (maximumWords) {
            var ue = UE.getEditor(id, {
                maximumWords: maximumWords
            });
        } else {
            ue = UE.getEditor(id);
        }
        ue.ready(function () {
            ue.execCommand('serverparam', data);
        });
    });
});

function getRootPath() {
    // 获取artDialog路径
    var path = window['_ueditor_path'] || (function (script, i, me) {
        for (i in script) {
            // 如果通过第三方脚本加载器加载本文件，请保证文件名含有"artDialog"字符
            if (script[i].src && script[i].src.indexOf('ueditor') !== -1) me = script[i];
        };

        _thisScript = me || script[script.length - 1];
        me = _thisScript.src.replace(/\\/g, '/');
        return me.lastIndexOf('/') < 0 ? '.' : me.substring(0, me.lastIndexOf('/'));
    }(document.getElementsByTagName('script')));
    if (path.indexOf("/Bundle") > 0)
        path = path.substring(0, path.indexOf("/Bundle"));
    else
        path = path.substring(0, path.indexOf("/Scripts/UEditor"));
    return path;
    //var strFullPath = window.document.location.href;
    //var strPath = window.document.location.pathname;
    //var pos = strFullPath.indexOf(strPath);
    //var prePath = strFullPath.substring(0, pos);
    //var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (prePath + postPath);
}


function getUEBasePath(docUrl, confUrl) {

    return getBasePath(docUrl || self.document.URL || self.location.href, confUrl || getConfigFilePath());

}

function getConfigFilePath() {

    var configPath = document.getElementsByTagName('script');

    return configPath[configPath.length - 1].src;

}

function getBasePath(docUrl, confUrl) {

    var basePath = confUrl;


    if (/^(\/|\\\\)/.test(confUrl)) {

        basePath = /^.+?\w(\/|\\\\)/.exec(docUrl)[0] + confUrl.replace(/^(\/|\\\\)/, '');

    } else if (!/^[a-z]+:/i.test(confUrl)) {

        docUrl = docUrl.split("#")[0].split("?")[0].replace(/[^\\\/]+$/, '');

        basePath = docUrl + "" + confUrl;

    }

    return optimizationPath(basePath);

}

function optimizationPath(path) {

    var protocol = /^[a-z]+:\/\//.exec(path)[0],
        tmp = null,
        res = [];

    path = path.replace(protocol, "").split("?")[0].split("#")[0];

    path = path.replace(/\\/g, '/').split(/\//);

    path[path.length - 1] = "";

    while (path.length) {

        if ((tmp = path.shift()) === "..") {
            res.pop();
        } else if (tmp !== ".") {
            res.push(tmp);
        }

    }

    return protocol + res.join("/");

}

window.UE = {
    getUEBasePath: getUEBasePath
};