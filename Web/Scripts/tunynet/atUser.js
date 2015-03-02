
(function ($) {
    var cacheUsersAndTeams = [];
    $("input[id^='AtUser_']").livequery(function () {
        var jsonUrl = $(this).data("url");
        var idPrefix = $(this).val();
        $("[atuser^='" + idPrefix + "']").livequery(function () {
            $(this).atwho({
                at: "@",
                tpl: "<li id='${id}' data-value='@${nickName}'>${nickName}${noteName}</li>",
                search_key: 'name',
                callbacks: {
                    remote_filter: function (query, callback) {
                        var thisVal = query,
                        self = $(this);
                        if (!self.data('active') && typeof (thisVal) != 'undefined') {
                            self.data('active', true);
                            var usersAndTeams = cacheUsersAndTeams[thisVal];
                            if (typeof usersAndTeams == "object") {
                                callback(usersAndTeams);
                            } else {
                                if (self.xhr) {
                                    self.xhr.abort();
                                }
                                self.xhr = $.getJSON(jsonUrl, {
                                    query: thisVal
                                }, function (data) {
                                    cacheUsersAndTeams[thisVal] = data;
                                    callback(data);
                                });
                            }
                            self.data('active', false);
                        }
                    },
                    sorter: function (query, items, search_key) {
                        var item, _i, _len, _results;
                        if (!query) {
                            return items;
                        }
                        _results = [];
                        for (_i = 0, _len = items.length; _i < _len; _i++) {
                            item = items[_i];
                            _results.push(item);
                        }
                        return _results;
                    },
                    highlighter: function (li, query) {
                        return li;
                    }

                }
            });
        });
        $("[id^='" + idPrefix + "']").live("click", function () {
            var textarea = $(this).attr('id');
            $("[atuser=" + textarea + "]").focus();
            var value = $("[atuser=" + textarea + "]").val();
            if (value[value.length - 1] != "@") {
                $("[atuser=" + textarea + "]").val(value + "@");
            }
            $("[atuser=" + textarea + "]").keyup();
        });
    });
})(jQuery);
