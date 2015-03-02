using Autofac;
using Fasterflect;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Tunynet.Utilities;

namespace Spacebuilder.MobileClient.Common
{
    /// <summary>
    /// 移动接口的配置文件
    /// </summary>
    [Serializable]
    public abstract class MobileClientApplicationConfig
    {
        private static readonly object lockObject = new object();
        private static ConcurrentDictionary<int, MobileClientApplicationConfig> applicationConfigs = null;



        private static bool isInitialized;

        /// <summary>
        /// 加载所有的application.config
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        /// <returns>Key=ApplicationId</returns>
        public static void InitializeAll(ContainerBuilder containerBuilder)
        {
            if (!isInitialized)
            {
                lock (lockObject)
                {
                    if (!isInitialized)
                    {
                        applicationConfigs = LoadConfigs();
                        foreach (var config in applicationConfigs.Values)
                        {
                            config.Initialize(containerBuilder);
                        }
                        isInitialized = true;
                    }
                }
            }
        }


        /// <summary>
        /// 加载所有的Application.config文件
        /// </summary>
        private static ConcurrentDictionary<int, MobileClientApplicationConfig> LoadConfigs()
        {
            var configs = new ConcurrentDictionary<int, MobileClientApplicationConfig>();
            //获取Applications中所有的Application.Config
            string mobileClientDirectory = WebUtility.GetPhysicalFilePath("~/MobileClient/");
            if (!Directory.Exists(mobileClientDirectory))
                return configs;
            int i = 10000;
            foreach (var mobilePath in Directory.GetDirectories(mobileClientDirectory))
            {
                string fileName = Path.Combine(mobilePath, "Application.Config");
                if (!File.Exists(fileName))
                    continue;

                string configType = string.Empty;
                XElement mobileElement = XElement.Load(fileName);

                //读取各个application节点中的属性     
                if (mobileElement != null)
                {
                    configType = mobileElement.Attribute("configType").Value;
                    Type mobileClassType = Type.GetType(configType);
                    if (mobileClassType != null)
                    {
                        ConstructorInvoker mobileConfigConstructor = mobileClassType.DelegateForCreateInstance(typeof(XElement));
                        MobileClientApplicationConfig app = mobileConfigConstructor(mobileElement) as MobileClientApplicationConfig;
                        if (app != null)
                        {
                            configs[i] = app;
                            i++;
                        }
                    }
                }
            }
            return configs;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xElement"></param>
        public MobileClientApplicationConfig(XElement xElement)
        {
        }

        public MobileClientApplicationConfig() { }


        /// <summary>
        /// ApplicationId
        /// </summary>
        public abstract int ApplicationId { get; }


        /// <summary>
        /// 应用初始化运行环境（每次站点启动时DI容器构建前调用）
        /// </summary>
        /// <remarks>
        /// 用于注册组件、解析配置文件，不可使用DI容器解析对象因为此时尚未构建
        /// </remarks>
        /// <param name="containerBuilder">DI容器构建器(autofac)</param>
        public abstract void Initialize(ContainerBuilder containerBuilder);
    }
}
