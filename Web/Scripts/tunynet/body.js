(function ($) {
    $(function () {
        SyntaxHighlighter.defaults['toolbar'] = false;
        SyntaxHighlighter.all();
        $("a[rel='fancybox']").fancybox({
        'transitionIn': 'elastic',
        'transitionOut': 'elastic',
        'speedIn': 600,
        'speedOut': 200,
        closeBtn  : false,
        helpers : {
            title : {
                    type : 'inside'
            },
            buttons	: {}
        }
    });
});
}(jQuery));