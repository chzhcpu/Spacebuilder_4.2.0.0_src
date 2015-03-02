

//定义全局的变量,用来储藏选中文字的信息
var SelectInfo = { pageX: 0, pageY: 0, onlyComment: false, selectText: '',wikiHref: 'javascript:;', askHref: 'javascript:;' };

//返回一个range对象,参数是selection对象
var getRangeObject = function () {
    var userSelection = getSelection();
    if (userSelection.getRangeAt)
        return userSelection.getRangeAt(0);
    else { // 较老版本Safari!
        var range = document.createRange();
        range.setStart(userSelection.anchorNode, userSelection.anchorOffset);
        range.setEnd(userSelection.focusNode, userSelection.focusOffset);
        return range;
    }
}

var getuserSelection = function () {
    var userSelection;
    if (window.getSelection) { //现代浏览器
        userSelection = window.getSelection();
    } else if (document.selection) { //IE浏览器 考虑到Opera，应该放在后面
        userSelection = document.selection.createRange();
    }
    return userSelection;
}

//选中文字后触发的事件 显示菜单
function showMenu(keyword, left, top) {
    $(".menuButton").hide();
    //if (document.cookie.indexOf(".ASPXAUTH") > 0)
        //return false;
    SelectInfo.pageX = left;
    SelectInfo.pageY = top;
    var left = left - 30;
    var top = top + 10;
    var isComment = $(".tn-comment-text-area").length;
    //debugger;
    if (keyword.length > 257 && isComment>0) {
        var divHtml = (' <div id="menuButton" style="width: 47px; z-index: 999; position: absolute; left:' + left + 'px; top: ' + top + 'px;" class="menuButton spb-search-accord " >'
        + '<div class="tn-widget tn-bubble tn-bubble-arrow-top">'
            + '<div class="tn-bubble-arrow">'
                + '<b class="tn-arrow-b1 tn-border-gray"></b><b class="tn-arrow-b2 tn-widget-bubble">'
                + '</b>'
            + '</div>'
            + '<div class="tn-bubble-content tn-widget-content tn-border-gray">'
                    + '<div class="tnui-quickSearchList"><a id="comment" href="javascript:;">评论</a></div>'
            + '</div>'
        + '</div>'
    + '</div>');
    }
    else if (keyword.length <= 257 && isComment > 0) {
        var divHtml = (' <div id="menuButton" style="width: 70px; z-index: 999; position: absolute; left:' + left + 'px; top: ' + top + 'px;" class="menuButton spb-search-accord " >'
         + '<div class="tn-widget tn-bubble tn-bubble-arrow-top">'
             + '<div class="tn-bubble-arrow">'
                 + '<b class="tn-arrow-b1 tn-border-gray"></b><b class="tn-arrow-b2 tn-widget-bubble">'
                 + '</b>'
             + '</div>'
             + '<div class="tn-bubble-content tn-widget-content tn-border-gray">'
                     + '<div  class="tnui-quickSearchList"><a target="_blank" href="' + SelectInfo.askHref+'">提问问题</a></div>'
                     + '<div class="tnui-quickSearchList"><a  target="_blank" href="' + SelectInfo.wikiHref +'">创建词条</a></div>'
                     + '<div class="tnui-quickSearchList"><a id="comment" href="javascript:;">评论</a></div>'
             + '</div>'
         + '</div>'
     + '</div>');
    }
    else if (keyword.length < 257) {
        var divHtml = (' <div id="menuButton" style="width: 70px; z-index: 999; position: absolute; left:' + left + 'px; top: ' + top + 'px;" class="menuButton spb-search-accord " >'
         + '<div class="tn-widget tn-bubble tn-bubble-arrow-top">'
             + '<div class="tn-bubble-arrow">'
                 + '<b class="tn-arrow-b1 tn-border-gray"></b><b class="tn-arrow-b2 tn-widget-bubble">'
                 + '</b>'
             + '</div>'
             + '<div class="tn-bubble-content tn-widget-content tn-border-gray">'
                     + '<div  class="tnui-quickSearchList"><a target="_blank" href="' + SelectInfo.askHref + '">提问问题</a></div>'
                     + '<div class="tnui-quickSearchList"><a  target="_blank" href="' + SelectInfo.wikiHref + '">创建词条</a></div>'
             + '</div>'
         + '</div>'
     + '</div>');
    }
    if ($(".tn-icon-quit").length>0) {
        $("body").append(divHtml);
    }
    return false
}
   

//点击页面隐藏智能提示框
$(document).click(function (e) {
    var e = e || window.event;
        
    if (e.pageX == SelectInfo.pageX && e.pageY==SelectInfo.pageY) {
        return false
    };

    //if ($(e.target).attr("id") == "menuButton" || $(e.target).hasClass("tnui-quickSearchList") || $(e.target).hasClass("tnui-option") || $(e.target).hasClass("tn-list-item-row")) {
    //    return false
    //};

    $(".menuButton").hide();

});
    