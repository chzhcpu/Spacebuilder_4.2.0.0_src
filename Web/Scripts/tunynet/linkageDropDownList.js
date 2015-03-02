(function ($) {
    //联动下拉列表change事件
    function linkageDropDownListChange() {
        var $currentDropDownList = $(this);
        var data = parseObject($(this).parent().attr("data"));
        var index = eval(this.id.substr(data.ControlName.length + 1));
        //如果选中有效值，则将当前下拉列表设置为表单项；否则将前一个下拉列表设置为表单项
        $currentDropDownList.nextAll("select").remove();
        var value = $currentDropDownList.val();

        if (value && value.length > 0 && index < data.Level - 1) { //加载下一级
            $.getJSON(data.GetChildSelectDataUrl, { id: $currentDropDownList.val() }, function (response) {
                if (response == null)
                    return;
                if (!response.length)
                    return;
                //更新当前下拉列表后面的下拉列表
                $currentDropDownList.after("\n<select id='" + data.ControlName + "_" + eval(parseInt(index) + 1)
                                                     + "' class='tn-dropdownlist'><option value=''>" + data.OptionLabel + "</option></select>");
                $(response).each(function () {
                    $("#" + data.ControlName + "_" + eval(parseInt(index) + 1))
                          .append("<option value='" + this.id + "'>" + this.name + "</option>");
                });
                $currentDropDownList.next().change(linkageDropDownListChange);
            });
        }
        if (index > 0 && value == data.DefaultValue)
            value = $("#" + data.ControlName + "_" + eval(parseInt(index) - 1)).val();
        $('input[type="hidden"][name="' + data.ControlName + '"]').val(value);
    }

    $(document).ready(function () {
        // 联动下拉列表
        $('[plugin="linkageDropDownList"]').livequery(function () {
            $("select", $(this)).removeAttr("name");
            $("select", $(this)).change(linkageDropDownListChange);
            //初始化
            $("select:last", $(this)).change();
        });
    });
})(jQuery);
