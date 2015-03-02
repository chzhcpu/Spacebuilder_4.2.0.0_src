

//检测 微博输入框内容的长度是否在范围内部
var checkTextLength = function (objId, countSpan, publishButton) {

    var val = $("#" + objId).val();
    var parten = /^\s*$/;
    if (parten.test(val) && val.length > 0) {
        return false;
    }
    if (!countSpan)
        countSpan = "countSpan";
    if (!publishButton)
        publishButton = "publishButton";
    //计算 还剩余多少字可以填写
    var length = 140 - countTextLength(val);

    var $ps = $('#' + publishButton);
    var $count = $("#" + countSpan);
    if (length < 0) {
        length = -length;
        $count.html('已超出<strong class="tn-text-bright">' + length + '</strong>字');
        $ps.attr('class', 'tn-button tn-corner-all  tn-button-large tn-button-secondary');
        $ps.attr('disabled', 'disabled');
    }
    else {
        $count.html('还可以输入<strong>' + length + '</strong>字');
        if (length > 139) {
            $ps.attr('class', 'tn-button tn-corner-all  tn-button-large tn-button-secondary');
            $ps.attr('disabled', 'disabled');
        }
        else {
            $ps.attr('class', 'tn-button tn-corner-all tn-button-text-only tn-button-large tn-button-primary');
            $ps.removeAttr('disabled');
        }
    }
}

//计算 微博输入框内容的字数
var countTextLength = function (value) {
    if (!value)
        return 0;
    return parseInt((value.replace(/[^\x00-\xff]/g, "**").length + 1) / 2);
}

//发布微博 表单提交成功 调用函数
var showMicroblogCreateMessage = function (data) {
    if (data.MessageType == 0) {
        $('#msg-sussess').children('span').removeClass("tn-bigicon-accept-circle tn-bigicon-cross-circle").addClass("tn-bigicon-exclamation");
    } else if (data.MessageType == -1) {
        $('#msg-sussess').children('span').removeClass("tn-bigicon-accept-circle tn-bigicon-exclamation").addClass("tn-bigicon-cross-circle");
    } else {
        $('#msg-sussess').children('span').removeClass("tn-bigicon-exclamation tn-bigicon-cross-circle").addClass("tn-bigicon-accept-circle");
        $("#microblogBody").val("");
    }
    $('#msg-sussess').children('strong').html(data.MessageContent);
    $('#msg-sussess').show();
    setTimeout(function () { $('#msg-sussess').hide(); }, 2000);
}

//发布微博 表单提交失败 调用函数
var microblogCreateError = function (data) {
    alert("微博发布失败！");
}

//topic插入操作调用方法
//microblogBody : textArea元素jquery对象
//topicName ： 要选中的topic的名称例如：#伦敦奥运会#
var newTopicClickEvent = function (microblogBodyId, topicName) {
    var microblogBodyVal = $("#" + microblogBodyId).val();

    if (microblogBodyVal.indexOf(topicName, 0) < 0) {
        //获取光标位置
        var mousePosition = $("#mousePositionInMicroblogBody").val();
        //在光标位置插入主题
        createNewTopic(microblogBodyId, topicName, mousePosition);


    }

    selectTextContent(microblogBodyId, topicName);
}

//将传入的topic 加到微博内容中去
var createNewTopic = function (microblogBodyId, topicName, mousePosition) {
    var microblogBody = $("#" + microblogBodyId).val();
    var frontString = microblogBody.substring(0, mousePosition);
    var backString = microblogBody.substring(mousePosition, microblogBody.length);

    $("#" + microblogBodyId).val(frontString + topicName + backString);
    $("#mousePositionInMicroblogBody").val(parseInt($("#mousePositionInMicroblogBody").val()) + topicName.length);
}

//将内容设置为选中状态
var selectTextContent = function (microblogBodyId, topicName) {

    var microblogBody = $("#" + microblogBodyId);

    var topicLength = topicName.length;
    var startIndex = microblogBody.val().indexOf(topicName, 0) + 1;
    var microblogBodyLength = microblogBody.val().length;

    if (document.createRange) {
        var textObj = microblogBody.get(0);
        textObj.focus();
        textObj.selectionStart = startIndex;
        textObj.selectionEnd = startIndex + topicLength - 2;
    }
    else {
        var microblogTextArea = document.getElementsByName("microblogBody")[0];
        var range = microblogTextArea.createTextRange();
        range.moveStart("character", startIndex);
        range.moveEnd("character", startIndex + topicLength - microblogBodyLength - 2);
        range.select();
    }
}

//获取鼠标在文本中的位置
var getMousePosition = function (microblogBodyId) {

    var microblogBody = document.getElementById(microblogBodyId)
    var result = 0;

    if (microblogBody.setSelectionRange) { //IE以外 
        result = microblogBody.selectionStart
    }
    else { //IE
        if (microblogBody.value != "") {
            var range = document.selection.createRange();
            range.moveStart("character", -microblogBody.value.length);
            result = range.text.length;
        }
        else {
            result = 0;
        }
    }

    if (result < 2) {
        result = $("#" + microblogBodyId).val().length;
    }

    //alert(result);
    $("#mousePositionInMicroblogBody").val(result.toString());
    //return result;
}






