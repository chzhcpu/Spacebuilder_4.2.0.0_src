/// <reference path="jquery-1.4.4.js" />
/// <reference path="jquery-ui.js" />

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

(function ($) {
    $(document).ready(function () {
        //ajaxForm
        $('form[plugin="ajaxForm"]').livequery(function () {
            var data = parseObject($(this).attr("data"));

            data.beforeSubmit = function (arr, $form, options) {
                if ($.watermark) {
                    $form.find("input:text,textarea").each(function () {
                        $.watermark._hide($(this));
                    });
                }
                if (!$form.valid()) {
                    return false;
                }
                $form.block({
                    message: '处理中'
                });
                var beforeSubmitFn = eval(data.beforeSubmitFn);
                if (beforeSubmitFn)
                    beforeSubmitFn(arr, $form, options);
            };

            data.error = function (response, statusText, xhr, $form) {
                $form.unblock();
                var errorFn = eval(data.errorFn);
                if (errorFn) {
                    errorFn(response.responseText, statusText, xhr, $form);
                }
            };

            data.success = function (response, statusText, xhr, $form) {
                $form.unblock();
                var successFn = eval(data.successFn);
                if (successFn) {
                    $.ajaxSetup({ cache: false });
                    successFn(response, statusText, xhr, $form);
                }
                if (data.closeDialog) {
                    var list = art.dialog.list;
                    for (var i in list) {
                        if ($(list[i].content()).find($form).length)
                            list[i].close();
                    };
                }
            };
            $(this).ajaxForm(data);
        });


        $("form[plugin!='ajaxForm']").live("submit", function (e) {
            var _form = $(this);
            var _button = $("button[type='submit']", _form);

            if (_form.valid()) {
                _button.attr("disabled", true).removeClass("tn-button-primary").addClass("tn-button-disabled");
            } else {
                _button.attr('disabled', false).removeClass("tn-button-disabled").addClass("tn-button-primary");
                return false;
            }
        });

        $("button[url]").live("click", function () {
            var url = $(this).attr("url");
            if (url)
                window.location.href = url;
        });
        $("form").livequery(function () { $.validator.unobtrusive.parse(document); });


        //处理火狐下刷新后单选框和单选钮仍然选中问题
        if ($.browser.mozilla) {
            $("input[type='radio']").attr("autocomplete", "off");
            $("input[type='checkbox']").attr("autocomplete", "off");
        }

        //非ajaxForm，自动增加AntiForgeryToken
        $("form[plugin!='ajaxForm']").on('submit', function (e) {
            var _form = $(this);
            var method = _form.attr("method");

            if (method == "get" || method == "GET") {
                return true;
            }

            if (_form.valid()) {
                if ($("input[name='__RequestVerificationToken']", _form).length == 0) {
                    var securityToken = $("input[name='__RequestVerificationToken']");
                    if (securityToken.length > 0) {
                        _form.append(securityToken);
                    }
                }
            } else {
                return false;
            }
        });

        //在ajaxForm $.post $.ajax等异步提交post请求时加上AntiForgeryToken
        //$(document).on('ajaxSend', function (elm, xhr, s) {
        //    var securityToken = $("input[name='__RequestVerificationToken']").val();
        //    if ((s.type == 'post' || s.type == 'POST') && s.hasContent && securityToken) {   // handle all verbs with content
        //        var tokenParam = "__RequestVerificationToken=" + encodeURIComponent(securityToken);
        //        if (s.data) {
        //            if (typeof (s.data) == "string" && s.data.indexOf("__RequestVerificationToken") < 0) 
        //                s.data = [s.data, tokenParam].join("&");                    
        //            else if (typeof (s.data) == "object")
        //                s.data.__RequestVerificationToken = encodeURIComponent(securityToken);
        //        }
        //        else
        //            s.data = tokenParam;
        //        var tokenParam = "__RequestVerificationToken=" + encodeURIComponent(securityToken);
        //        s.data = s.data ? [s.data, tokenParam].join("&") : tokenParam;
        //        // ensure Content-Type header is present!
        //        if (s.contentType !== false) {
        //            xhr.setRequestHeader("Content-Type", s.contentType);
        //        }
        //    }
        //});

        /*Begin表单内容保存提示插件
        *表单有录入内容但未保存时，用户离开页面提示
        *调用示例：('form').enable_changed_form_confirm("您确定不保存就离开页面吗?");
        */
        //解决IE下 javascript:void(0)会触发window.onbeforeunload事件的问题
        $("a[href='javascript:void(0)']").live("click", function (e) {
            e.preventDefault();
        });
        $("a[href='javascript:void(0);']").live("click", function (e) {
            e.preventDefault();
        });
        $("a[href='javascript:;']").live("click", function (e) {
            e.preventDefault();
        });
        $.fn.enable_changed_form_confirm = function (prompt) {
            var _f = this;
            $('input:text,input:password, textarea', this).each(function () {
                $(this).attr('_value', $(this).val());
            });

            $('input:checkbox,input:radio', this).each(function () {
                var _v = this.checked ? 'on' : 'off';
                $(this).attr('_value', _v);
            });

            $('select', this).each(function () {
                $(this).attr('_value', this.options[this.selectedIndex].value);
            });

            $(this).submit(function () {
                window.onbeforeunload = null;
            });

            window.onbeforeunload = function () {
                if (is_form_changed(_f)) {
                    return prompt;
                }
            }
        }

        function is_form_changed(f) {
            var changed = false;
            $('input:text,input:password,textarea', f).each(function () {
                var _v = $(this).attr('_value');
                if (typeof (_v) == 'undefined') _v = '';
                if (_v != $(this).val()) changed = true;
            });

            $('input:checkbox,input:radio', f).each(function () {
                var _v = this.checked ? 'on' : 'off';
                if (_v != $(this).attr('_value')) changed = true;
            });

            $('select', f).each(function () {
                var _v = $(this).attr('_value');
                if (typeof (_v) == 'undefined') _v = '';
                if (_v != this.options[this.selectedIndex].value) changed = true;
            });
            return changed;
        }

        /*End表单内容保存提示插件*/


    });
})(jQuery);