/*
* Version: Beta 2
* Release: 2010-5-29
*/
(function ($) {
    $.PointMessage = function PointMessage(url) {
        setTimeout(function () {
            $.get(url, { date: new Date() }, function (data) {
                $("body").append(data);
            });
        }, 1000);
    }

    $.GetRootPath = function getRootPath() {
        var strFullPath = window.document.location.href;
        var strPath = window.document.location.pathname;
        var pos = strFullPath.indexOf(strPath);
        var prePath = strFullPath.substring(0, pos);
        var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
        return (prePath + postPath);
    }
})(jQuery);
