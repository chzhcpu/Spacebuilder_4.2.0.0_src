//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Optimization;
using Spacebuilder.Common;
using System;
using System.Web;
using System.Text.RegularExpressions;
using Tunynet.Utilities;
using HtmlAgilityPack;
using System.Linq;
using Microsoft.Ajax.Utilities;
using System.Text;

namespace Spacebuilder.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //基础样式
            bundles.Add(new StyleBundle("~/Bundle/Styles/Site")
                .Include("~/Themes/Shared/Styles/tn.core_src.css", new FixCssRewrite())
                .Include("~/Themes/Shared/Styles/tn.widgets_src.css", new FixCssRewrite())
                .Include("~/Themes/Shared/Styles/tn.theme_src.css", new FixCssRewrite())
                .Include("~/Themes/Shared/Styles/common.css", new FixCssRewrite())
                .Include("~/Scripts/jquery/artDialog/skins/default.css", new FixCssRewrite()));

            //代码高亮插件样式
            bundles.Add(new StyleBundle("~/Bundle/Styles/CodeHighlighter")
                .Include("~/Scripts/syntaxHighlighter/styles/shCore.css", new FixCssRewrite())
                .Include("~/Scripts/syntaxHighlighter/styles/shThemeDefault.css", new FixCssRewrite()));

            //图片弹出层样式
            bundles.Add(new StyleBundle("~/Bundle/Styles/FancyBox")
                .Include("~/Scripts/jquery/fancybox/jquery.fancybox.css", new FixCssRewrite())
                .Include("~/Scripts/jquery/fancybox/helpers/jquery.fancybox-buttons.css", new FixCssRewrite()));

            //颜色选择器样式
            bundles.Add(new StyleBundle("~/Bundle/Styles/ColorPicker")
                .Include("~/Scripts/jquery/ColorSelect/css/jquery.bigcolorpicker.css", new FixCssRewrite()));

            //图片裁剪
            bundles.Add(new StyleBundle("~/Bundle/Styles/ImageCrop")
                .Include("~/Scripts/jquery/Jcrop/jquery.Jcrop.css", new FixCssRewrite()));

            //jQuery类库
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/jQuery")
            .Include("~/Scripts/jquery/jquery-1.7.1.js"));

            //站点基础脚本
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/Site")
                .Include("~/Scripts/jquery/jquery.cookie.js")
                .Include("~/Scripts/jquery/jquery.hoverIntent.js")
                .Include("~/Scripts/jquery/jquery.metaData.js")
                .Include("~/Scripts/jquery/jquery.livequery.js")
                .Include("~/Scripts/jquery/jquery.dcmegamenu.1.3.3.js")
                .Include("~/Scripts/jquery/tipsy/jquery.tipsy-1.0.0a.js")
                .Include("~/Scripts/jquery/tipsy/jquery.tipsy.hovercard.js")
                .Include("~/Scripts/jquery/template/jquery.tmpl.js")
                .Include("~/Scripts/jquery/artDialog/jquery.artDialog-4.1.4.js")
                .Include("~/Scripts/jquery/artDialog/plugins/jquery.artDialog.iframeTools.js")
                .Include("~/Scripts/jquery/watermark/jquery.watermark-3.1.4.js")
                .Include("~/Scripts/jquery/scrollTo/jquery.scrollTo-1.4.3.1.js")
                .Include("~/Scripts/jquery/ajaxForm/jquery.blockUI.js")
                .Include("~/Scripts/jquery/ajaxForm/jquery.form.js")
                .Include("~/Scripts/jquery/validate/jquery.validate.js")
                .Include("~/Scripts/jquery/jquery.validate.password.js")
                .Include("~/Scripts/jquery/validate/jquery.validate.unobtrusive.js")
                .Include("~/Scripts/tunynet/plugins/jquery.validate.additional-methods.js")
                .Include("~/Scripts/tunynet/plugins/jquery.validate.messages-zh-CN.js")
                .Include("~/Scripts/tunynet/plugins/jquery.tn.textarea.js")
                .Include("~/Scripts/tunynet/plugins/jquery.tn.menuButton.js")
                .Include("~/Scripts/tunynet/site.js")
                .Include("~/Scripts/tunynet/form.js")
                .Include("~/Scripts/tunynet/list.js")
                .Include("~/Scripts/tunynet/dialog.js")
                .Include("~/Scripts/tunynet/image.js")
                .Include("~/Scripts/tunynet/quickSearch.js")
                .Include("~/Scripts/tunynet/pointMessage.js")
                .Include("~/Scripts/UEditor/ueditor.parse.min.js")
                .Include("~/Scripts/tunynet/uploadify.js")
                .Include("~/Scripts/tunynet/emotionSelector.js")
                //.Include("~/Scripts/tunynet/jqueryUI.js")
                .Include("~/Scripts/tunynet/linkageDropDownList.js")
                //.Include("~/Scripts/tunynet/plugins/jquery.spb.collapsibleBox.js")
                //.Include("~/Scripts/tunynet/plugins/jquery.spb.sideMenu.js")
                );

            //jQueryUI
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/jQueryUI")
                .Include("~/Scripts/jquery/jqueryUI/jquery-ui-1.8.7.js")
                .Include("~/Scripts/jquery/jqueryUI/jquery-ui-timepicker-addon.js")
                .Include("~/Scripts/jquery/jqueryUI/jquery.ui.datepicker-zh-CN.js")
                .Include("~/Scripts/tunynet/plugins/jquery.spb.tabs.js")
                .Include("~/Scripts/tunynet/jqueryUI.js")
                .Include("~/Scripts/tunynet/emailAutoComplete.js"));

            //多文件上传控件
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/Uploadify")
            .Include("~/Scripts/jquery/uploadify/jquery.uploadify-3.1.js")
            //.Include("~/Scripts/tunynet/uploadify.js")
            );

            //@好友控件
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/AtUser")
            .Include("~/Scripts/jquery/AtUser/jquery.caret.js")
            .Include("~/Scripts/jquery/AtUser/jquery.atwho.js")
            .Include("~/Scripts/jquery/AtUser/jquery.atUser.js")
            .Include("~/Scripts/tunynet/atUser.js")
            );

            //TinyMCE编辑器
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/HtmlEditor")
                .Include("~/Scripts/jquery/Jeditable/jquery.jeditable.js")
                .Include("~/Scripts/tinymce/jquery.tinymce.js")
                .Include("~/Scripts/tunynet/htmlEditor.js"));

            //百度编辑器
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/UEditor")
                .Include("~/Scripts/UEditor/ueditor.init.js")
                .Include("~/Scripts/UEditor/ueditor.config.js")
                .Include("~/Scripts/UEditor/ueditor.all.js")
                .Include("~/Scripts/tunynet/emotionSelector.js")
                .Include("~/Scripts/jquery/Jeditable/jquery.jeditable.js"));

            //Html编辑器中的代码高亮插件
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/CodeHighlighter")
            .Include("~/Scripts/syntaxHighlighter/scripts/shCore-2.0.296.js")
            .Include("~/Scripts/syntaxHighlighter/scripts/shLegacy-2.0.296.js")
            .Include("~/Scripts/syntaxHighlighter/scripts/shBrushAllLanguage-2.0.296.js")
            );

            //照片详情页
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/photoViewer")
            .Include("~/Scripts/jquery/imgareaselect/jquery.imgareaselect-0.9.8.js")
            .Include("~/Scripts/jquery/Jcrop/jquery.Jcrop.Js")
            .Include("~/Scripts/jquery/jQueryRotate-2.1.js")
            .Include("~/Scripts/tunynet/plugins/jquery.photoViewer.js"));

            //图片裁剪
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/ImageCrop")
            .Include("~/Scripts/jquery/imgareaselect/jquery.imgareaselect-0.9.8.js")
            .Include("~/Scripts/jquery/Jcrop/jquery.Jcrop.Js"));


            //颜色选择器
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/ColorPicker")
            .Include("~/Scripts/tunynet/plugins/jquery.iColorPicker.js")
            .Include("~/Scripts/jquery/ColorSelect/js/jquery.bigcolorpicker.js"));

            //频道幻灯播放插件
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/SliderKit")
            .Include("~/Scripts/jquery/sliderkit/jquery.easing.1.3.js")
            .Include("~/Scripts/jquery/jquery.mousewheel-3.0.6.pack.js")
            .Include("~/Scripts/jquery/sliderkit/jquery.sliderkit-1.9.1.js")
            .Include("~/Scripts/jquery/sliderkit/addons/sliderkit.counter.1.0.js")
            .Include("~/Scripts/jquery/sliderkit/addons/sliderkit.delaycaptions.1.1.js")
            .Include("~/Scripts/jquery/sliderkit/addons/sliderkit.timer.1.0.js")
             .Include("~/Scripts/jquery/sliderkit/addons/sliderkit.imagefx.1.0.js")
            );



            //正文中使用的图片弹出层
            bundles.Add(new ScriptBundle("~/Bundle/Scripts/FancyBox")
            .Include("~/Scripts/jquery/fancybox/jquery.fancybox.js")
            .Include("~/Scripts/jquery/fancybox/helpers/jquery.fancybox-buttons.js"));
        }
    }

    public class FixCssRewrite : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            if (includedVirtualPath == null)
            {
                throw new ArgumentNullException("includedVirtualPath");
            }
            return ConvertUrlsToAbsolute(VirtualPathUtility.GetDirectory(WebUtility.ResolveUrl(includedVirtualPath)), input);
        }

        private string ConvertUrlsToAbsolute(string baseUrl, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return content;
            }
            Regex regex = new Regex("url\\(['\"]?(?<url>[^)]+?)['\"]?\\)");
            return regex.Replace(content, (MatchEvaluator)(match => ("url(" + RebaseUrlToAbsolute(baseUrl, match.Groups["url"].Value) + ")")));
        }


        private string RebaseUrlToAbsolute(string baseUrl, string url)
        {
            if ((string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(baseUrl)) || url.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                return url;
            }
            if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl = baseUrl + "/";
            }
            return WebUtility.ResolveUrl(VirtualPathUtility.ToAbsolute(baseUrl + url));
        }
    }

    public class FixJs : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            if (!context.EnableInstrumentation)
            {
                Minifier minifier = new Minifier();
                CodeSettings codeSettings = new CodeSettings
                {
                    EvalTreatment = EvalTreatment.Ignore,
                    PreserveImportantComments = false
                };
                string str = minifier.MinifyJavaScript(response.Content, codeSettings);
                if (minifier.ErrorList.Count == 0)
                {
                    response.Content = str;
                }
            }

            response.ContentType = "text/javascript";
        }
    }
}