(function ($) {
    $(document).ready(function () {
        //ztree
        $('[plugin="Tree"]').livequery(function () {
            var treeid = $(this).attr("id");
            //展开树之前回调函数
            var curExpandNode = null;

            //单一路径函数
            function singlePath(newNode) {
                if (newNode === curExpandNode) return;
                if (curExpandNode && curExpandNode.open == true) {
                    var zTree = $.fn.zTree.getZTreeObj(treeid);
                    if (newNode.parentTId === curExpandNode.parentTId) {
                        zTree.expandNode(curExpandNode, false);
                    } else {
                        var newParents = [];
                        while (newNode) {
                            newNode = newNode.getParentNode();
                            if (newNode === curExpandNode) {
                                newParents = null;
                                break;
                            } else if (newNode) {
                                newParents.push(newNode);
                            }
                        }
                        if (newParents != null) {
                            var oldNode = curExpandNode;
                            var oldParents = [];
                            while (oldNode) {
                                oldNode = oldNode.getParentNode();
                                if (oldNode) {
                                    oldParents.push(oldNode);
                                }
                            }
                            if (newParents.length > 0) {
                                for (var i = Math.min(newParents.length, oldParents.length) - 1; i >= 0; i--) {
                                    if (newParents[i] !== oldParents[i]) {
                                        zTree.expandNode(oldParents[i], false);
                                        break;
                                    }
                                }
                            } else {
                                zTree.expandNode(oldParents[oldParents.length - 1], false);
                            }
                        }
                    }
                }
                curExpandNode = newNode;
            }
            //被展开之后回调函数
            function onExpand(event, treeId, treeNode) {
                //如果存在额外方法则执行
                var addOnExpand = data.Settings.callback.addOnExpand;
                if (addOnExpand) {
                    addOnExpand(event, treeId, treeNode);
                }
                curExpandNode = treeNode;
            }
            function beforeExpand(treeId, treeNode) {
                //如果存在额外方法则执行
                var addBeforeExpand = data.Settings.callback.addBeforeExpand;
                if (addBeforeExpand) {
                    addBeforeExpand(treeId, treeNode);
                }
                var pNode = curExpandNode ? curExpandNode.getParentNode() : null;
                var treeNodeP = treeNode.parentTId ? treeNode.getParentNode() : null;
                var zTree = $.fn.zTree.getZTreeObj(treeId);
                for (var i = 0, l = !treeNodeP ? 0 : treeNodeP.children.length; i < l; i++) {
                    if (treeNode !== treeNodeP.children[i]) {
                        zTree.expandNode(treeNodeP.children[i], false);
                    }
                }
                while (pNode) {
                    if (pNode === treeNode) {
                        break;
                    }
                    pNode = pNode.getParentNode();
                }
                if (!pNode) {
                    singlePath(treeNode);
                }
            }
            var data = eval("(" + ($(this).attr("data")) + ")");
            if (data.TreeNodes) {
                data.TreeNodes = eval(data.TreeNodes);
            }
            if (data.Settings) {
                data.Settings = eval(data.Settings);
            }
            //初始化
            $.fn.zTree.init($("#" + $(this).attr("id")), data.Settings, data.TreeNodes);
        });
    });
})(jQuery);