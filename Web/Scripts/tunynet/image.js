(function ($) {
    //处理照片的自动缩放        
    $("img").livequery(function () {
        var $this = $(this);

        var objectWidth = $this.attr("width");      //img控件的定义宽度
        var objectHeight = $this.attr("height");    //img控件的定义高度

        var img = $("<img/>");
        img.load(function () {
            var imgWidth = this.width;               //图片实际宽度
            var imgHeight = this.height;             //图片实际高度

            if (objectWidth && objectHeight) {          //同时设置了img控件的宽度和高度
                if (objectWidth >= imgWidth && objectHeight >= imgHeight) {
                    $this.removeAttr("width");
                    $this.removeAttr("height");
                } else if (objectWidth >= imgWidth && objectHeight < imgHeight) {
                    $this.removeAttr("width");
                } else if (objectWidth < imgWidth && objectHeight >= imgHeight) {
                    $this.removeAttr("height");
                } else if (objectWidth < imgWidth && objectHeight < imgHeight) {
                    if (imgWidth >= imgHeight) {
                        $this.removeAttr("height");
                    } else {
                        $this.removeAttr("width");
                    }
                }
            } else if (objectWidth) {                   //只设置了img控件的宽度
                if (objectWidth >= imgWidth) {
                    $this.removeAttr("width");
                }
            } else if (objectHeight) {                  //只设置了img控件的高度
                if (objectHeight >= imgHeight) {
                    $this.removeAttr("height");
                }
            }
        });
        img.attr("src", $this.attr("src"));
    });
})(jQuery);