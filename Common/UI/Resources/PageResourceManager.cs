//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Tunynet.Utilities;
using System.Web.Mvc;
using System.Collections.Concurrent;
using System.Web.Optimization;

namespace Spacebuilder.Common
{
    /// <summary>
    /// IResourceManager的默认实现
    /// </summary>
    public class PageResourceManager : IPageResourceManager
    {
        private readonly List<string> _titleParts = new List<string>();
        private readonly List<string> _includedScripts = new List<string>();
        private readonly List<string> _includedStyles = new List<string>();
        private readonly List<string> _registeredScriptBlocks = new List<string>();
        private readonly List<string> _registeredStyleBlocks = new List<string>();
        private readonly Dictionary<string, MetaEntry> _metas;

        /// <summary>
        /// 构造函数
        /// </summary>        
        public PageResourceManager()
            : this(null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productVersionInfo">产品版本信息（用于生成generator）</param>
        public PageResourceManager(string productVersionInfo)
        {
            if (!string.IsNullOrEmpty(productVersionInfo))
            {
                _metas = new Dictionary<string, MetaEntry> 
                {
                    { "generator", new MetaEntry("generator",productVersionInfo)}
                };
            }
            else
            {
                _metas = new Dictionary<string, MetaEntry>();
            }

            //LogoUrl = "~/images/logo.png";
        }

        /// <summary>
        /// 独立的资源站（例如：http://www.resouce.com）
        /// </summary>
        public string ResourceSite { get; set; }

        #region Tilte & Shoutcut icon & Meta

        private string titleSeparator = " - ";
        /// <summary>
        /// 页面Title各组成部分分隔符
        /// </summary>
        public string TitleSeparator
        {
            get { return titleSeparator; }
            set { titleSeparator = value; }
        }

        private bool isAppendSiteName;
        /// <summary>
        /// 是否在title中附加站点名称
        /// </summary>
        public bool IsAppendSiteName
        {
            get { return isAppendSiteName; }
            set { isAppendSiteName = value; }
        }

        /// <summary>
        /// 附加TitlePart
        /// </summary>
        public void AppendTitleParts(params string[] titleParts)
        {
            if (titleParts.Length > 0)
                foreach (string titlePart in titleParts)
                    if (!string.IsNullOrEmpty(titlePart))
                        _titleParts.Add(titlePart);
        }

        /// <summary>
        /// 把TitlePart插入到第一位
        /// </summary>
        public void InsertTitlePart(string titlePart)
        {
            if (!string.IsNullOrEmpty(titlePart))
                _titleParts.Insert(0, titlePart);
        }

        /// <summary>
        /// 生成Title
        /// </summary>
        public string GenerateTitle()
        {
            return _titleParts.Count == 0
                ? String.Empty
                : String.Join(TitleSeparator, _titleParts.AsEnumerable().ToArray());
        }

        private string shortcutIcon;
        /// <summary>
        /// shortcut icon
        /// </summary>
        public string ShortcutIcon
        {
            get
            {
                if (string.IsNullOrEmpty(shortcutIcon))
                    shortcutIcon = "~/favicon.ico";

                return ResolveUrlWithResourceSite(shortcutIcon);
            }
            set { shortcutIcon = value; }
        }

        ///// <summary>
        ///// Logo Url(默认=~/images/logo.png)
        ///// </summary>
        ///// <remarks>
        ///// 个别浏览器不支持png透明图片，为了兼容这些浏览器需要输出一段特殊的css代码
        ///// </remarks>
        //public string LogoUrl { get; set; }

        /// <summary>
        /// 设置description类型的meta
        /// </summary>
        /// <param name="content">设置的Description内容</param>
        public void SetMetaOfDescription(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;

            MetaEntry meta = new MetaEntry("description", content);
            SetMeta(meta);
        }

        /// <summary>
        /// 设置keywords类型的meta
        /// </summary>
        /// <param name="content">设置的Keyword内容</param>
        public void SetMetaOfKeywords(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;

            MetaEntry meta = new MetaEntry("keywords", content);
            SetMeta(meta);
        }

        /// <summary>
        /// 附加keywords类型的meta
        /// </summary>
        /// <param name="content">附加的Keyword内容</param>
        public void AppendMetaOfKeywords(string content)
        {
            MetaEntry meta = new MetaEntry("keywords", content);
            AppendMeta(meta, ",");
        }

        /// <summary>
        /// 设置Meta
        /// </summary>
        /// <param name="meta">Meta实体</param>
        public void SetMeta(MetaEntry meta)
        {
            if (meta == null || String.IsNullOrEmpty(meta.Name))
                return;

            _metas[meta.Name] = meta;
        }

        /// <summary>
        /// 附加Meta
        /// </summary>
        /// <param name="meta">Meta实体</param>
        /// <param name="contentSeparator">合并content时使用的分隔符</param>
        public void AppendMeta(MetaEntry meta, string contentSeparator)
        {
            if (meta == null || String.IsNullOrEmpty(meta.Name))
                return;

            MetaEntry existingMeta;
            if (_metas.TryGetValue(meta.Name, out existingMeta))
                meta = MetaEntry.Combine(existingMeta, meta, contentSeparator);

            _metas[meta.Name] = meta;
        }

        /// <summary>
        /// 获取所有的Meta
        /// </summary>
        public IList<MetaEntry> GetRegisteredMetas()
        {
            return _metas.Values.ToList().AsReadOnly();
        }

        #endregion


        #region Include js/css

        /// <summary>
        /// 设置Script引用
        /// </summary>
        /// <param name="scriptUrl">引入的script路径（支持~/）</param>
        public void IncludeScript(string scriptUrl)
        {
            if (string.IsNullOrEmpty(scriptUrl))
                throw new ArgumentNullException("scriptUrl");
            //可避免js文件重复引用
            if (!_includedScripts.Contains(scriptUrl))
                _includedScripts.Add(scriptUrl);
        }

        /// <summary>
        /// 设置css引用
        /// </summary>
        /// <param name="styleUrl">引入的css路径（支持~/）</param>
        public void IncludeStyle(string styleUrl)
        {
            if (string.IsNullOrEmpty(styleUrl))
                throw new ArgumentNullException("styleUrl");
            //可避免css文件重复引用
            if (!_includedStyles.Contains(styleUrl))
                _includedStyles.Add(styleUrl);
        }

        /// <summary>
        /// 获取所有引入的script Html标签
        /// </summary>
        public IList<string> GetIncludedScriptHtmls()
        {
            List<string> scripts = new List<string>();
            foreach (var scriptUrl in _includedScripts)
            {
                //TagBuilder tagBuilder = new TagBuilder("script");
                //tagBuilder.MergeAttribute("src", ResolveUrlWithResourceSite(scriptUrl));
                //tagBuilder.MergeAttribute("type", "text/javascript");
                //scripts.Add(tagBuilder.ToString(TagRenderMode.Normal));
                scripts.Add(Scripts.Render(scriptUrl).ToHtmlString());
            }
            return scripts;
        }

        /// <summary>
        /// 获取所有引入的Style Html标签
        /// </summary>
        /// <returns>返回所有包含的css</returns>
        public IList<string> GetIncludedStyleHtmls()
        {
            List<string> styles = new List<string>();
            foreach (var styleUrl in _includedStyles)
            {
                //TagBuilder tagBuilder = new TagBuilder("link");
                //tagBuilder.MergeAttribute("href", ResolveUrlWithResourceSite(styleUrl));
                //tagBuilder.MergeAttribute("type", "text/css");
                //tagBuilder.MergeAttribute("rel", "stylesheet");
                //styles.Add(tagBuilder.ToString(TagRenderMode.SelfClosing));
                styles.Add(Styles.Render(styleUrl).ToHtmlString());
            }
            return styles;
        }

        #endregion


        #region Register script 代码块

        /// <summary>
        /// 注册在页面呈现的Script代码块
        /// </summary>
        public void RegisterScriptBlock(string scriptBlock)
        {
            _registeredScriptBlocks.Add(scriptBlock);
        }


        /// <summary>
        /// 获取注册的script代码块
        /// </summary>
        public IList<string> GetRegisteredScriptBlocks()
        {
            return _registeredScriptBlocks.ToList();
        }

        /// <summary>
        /// 注册在页面呈现的css代码块
        /// </summary>
        public void RegisterStyleBlock(string styleBlock)
        {
            _registeredStyleBlocks.Add(styleBlock);
        }

        /// <summary>
        /// 获取注册的css代码块
        /// </summary>
        public IList<string> GetRegisteredStyleBlocks()
        {
            return _registeredStyleBlocks.ToList();
        }
        #endregion


        #region Help Methods

        /// <summary>
        /// 清空脚本引用列表
        /// </summary>
        public void ClearIncludedScripts()
        {
            _includedScripts.Clear();
        }

        /// <summary>
        /// 清空样式引用列表
        /// </summary>
        public void ClearIncludedStyles()
        {
            _includedStyles.Clear();
        }

        /// <summary>
        /// 清空注册的script代码块列表
        /// </summary>
        public void ClearRegisteredScriptBlocks()
        {
            _registeredScriptBlocks.Clear();
        }

        /// <summary>
        /// 清空注册的css代码块列表
        /// </summary>
        public void ClearRegisteredStyleBlocks()
        {
            _registeredStyleBlocks.Clear();
        }

        /// <summary>
        /// 解析资源的完整虚拟路径或带资源网址的全路径
        /// </summary>
        /// <param name="url">待解析的url</param>
        /// <returns>返回完整url路径</returns>
        public string ResolveUrlWithResourceSite(string url)
        {
            string resultUrl = ResolveUrl(url);
            if (!string.IsNullOrEmpty(ResourceSite) && !url.ToLower().StartsWith("http"))
                resultUrl = ResourceSite + url;

            return resultUrl;
        }

        /// <summary>
        /// 解析url为绝对路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string ResolveUrl(string url)
        {
            return WebUtility.ResolveUrl(url);
        }
        #endregion

    }
}
