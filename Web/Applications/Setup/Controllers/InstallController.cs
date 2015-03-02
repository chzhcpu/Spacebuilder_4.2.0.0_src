using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Spacebuilder.Common;
using Tunynet.Common;
using Tunynet.UI;
using System.Collections.Concurrent;

using Tunynet.Utilities;
using System.Configuration;
using System.Data.Common;
using Tunynet.Repositories;
using PetaPoco;
using System.Xml.Linq;

namespace Spacebuilder.Setup.Controllers
{
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    public class InstallController : Controller
    {
        /// <summary>
        /// 
        /// 安装开始
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Start()
        {
            return View();
        }

        /// <summary>
        /// 第一步环境检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Step1_EnvironmentCheck()
        {
            return View();
        }

        /// <summary>
        /// 环境检查局部页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Step1_EnvironmentCheck()
        {
            Dictionary<string, bool> directoryPermissions = new Dictionary<string, bool>();
            directoryPermissions["App_Data"] = CheckFolderWriteable(Server.MapPath(@"~\App_Data"));
            directoryPermissions["Themes"] = CheckFolderWriteable(Server.MapPath(@"~\Themes"));
            directoryPermissions["Webconfig"] = CheckWebConfig();
            ViewData["DirectoryPermissions"] = directoryPermissions;

            return View();
        }

        /// <summary>
        /// 第二步填写数据库相关信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Step2_DatabaseInfo()
        {

            DataBaseInfoModel model = new DataBaseInfoModel();
            if (TempData["TempModel"] != null)
            {
                model = TempData["TempModel"] as DataBaseInfoModel;
                TempData["TempModel"] = TempData["TempModel"];
            }

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Step2_Wait(DataBaseInfoModel model)
        {
            return View(model);
        }

        /// <summary>
        /// 第二步-等待安装完成
        /// </summary>
        /// <remarks>主要处理数据库结构及</remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Step2_Wait(DataBaseInfoModel model)
        {
            string server = string.Format("{0}{1}{2}", model.Server
                                                     , !string.IsNullOrEmpty(model.Port) ? ":" + model.Port : ""
                                                     , !string.IsNullOrEmpty(model.Instance) ? "\\" + model.Instance : "");

            string connectString = string.Format("server={0};uid={1};pwd={2};",
                                                 server, model.DataBaseUserName, model.DataBasePassword);

            if (model.DBType == DBType.MySql)
            {
                connectString += "Charset=utf8;";
            }

            TempData["TempModel"] = model;
            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();

            Database db = CreateDatabase(connectString, model.DBType, ref messages);

            //尝试打开数据库链接，检查数据库是否能够链接上
            try
            {
                db.OpenSharedConnection();
                db.CloseSharedConnection();
            }
            catch (Exception e)
            {
                bool success = false;
                //如果是SQL Server数据库，则再次尝试打开SQLEXPRESS
                if (model.DBType == DBType.SqlServer && string.IsNullOrEmpty(model.Instance))
                {
                    try
                    {
                        connectString = string.Format("server={0};uid={1};pwd={2};",
                                                      model.Server + "\\SQLEXPRESS", model.DataBaseUserName, model.DataBasePassword);
                        db = CreateDatabase(connectString, model.DBType, ref messages);
                        db.OpenSharedConnection();
                        db.CloseSharedConnection();
                        success = true;
                    }
                    catch { }
                }
                if (!success)
                {
                    messages["数据库帐号或密码错误，无法登录数据库服务器"] = e.Message;
                    TempData["Error"] = messages;
                    return Json(new { });
                }
            }

            if (model.DBType == DBType.SqlServer)
            {
                #region SQL Server

                var val = db.FirstOrDefault<string>("select @@@@Version");

                if (string.IsNullOrEmpty(val))
                {
                    messages["要求数据库为Sql 2005及以上"] = string.Empty;
                    TempData["Error"] = messages;
                    return Json(new { });
                }

                int dbVersion = Convert.ToInt32(val.Substring(21, 4));
                if (dbVersion < 2005)
                {
                    messages["要求数据库为Sql 2005及以上,当前为" + val] = string.Empty;
                    TempData["Error"] = messages;
                    return Json(new { });
                }

                val = db.FirstOrDefault<string>("select 1 from master..sysdatabases where [name]=@0", model.DataBase);

                //创建空数据库
                if (string.IsNullOrEmpty(val))
                {
                    try
                    {
                        db.Execute(string.Format("create database {0}; ALTER DATABASE {0} SET RECOVERY SIMPLE; ", model.DataBase));
                    }
                    catch (Exception e)
                    {
                        messages[e.Message] = e.StackTrace;
                        TempData["Error"] = messages;
                        return Json(new { });
                    }
                }
                else
                {
                    //检查当前数据库是否为本程序数据库或一个空库
                    string dbConnectString = connectString + ";database=" + model.DataBase;
                    db = CreateDatabase(dbConnectString, model.DBType, ref messages);

                    int tableCount = db.FirstOrDefault<int>("select COUNT(*) from sysobjects where xtype='U'");

                    string tableName = db.FirstOrDefault<string>("select name from sysobjects where name=@0", "tn_SystemData");

                    if (tableCount > 0 && string.IsNullOrEmpty(tableName))
                    {
                        messages["当前数据库不是本程序数据库！"] = string.Empty;
                        TempData["Error"] = messages;
                        return Json(new { });
                    }
                }
                #endregion
            }
            else if (model.DBType == DBType.MySql)
            {
                #region MySql
                string information_schema_ConnectString = connectString + "database=information_schema;";
                db = CreateDatabase(information_schema_ConnectString, model.DBType, ref messages);

                //检查数据库是否已创建
                string SCHEMA_NAME = db.FirstOrDefault<string>(Sql.Builder.Select("SCHEMA_NAME").From("SCHEMATA").Where("SCHEMA_NAME=@0", model.DataBase));
                if (string.IsNullOrEmpty(SCHEMA_NAME))
                {
                    //创建空数据库
                    db.Execute(string.Format("CREATE DATABASE `{0}` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci; ", model.DataBase));
                }
                else
                {
                    int tableCount = db.FirstOrDefault<int>("SELECT COUNT(*) FROM information_schema.TABLES where TABLE_SCHEMA = '@0';", model.DataBase);
                    
                    //当前数据库不是本程序数据库或一个空库
                    string TABLE_NAME = db.FirstOrDefault<string>(Sql.Builder.Select("TABLE_NAME").From("TABLES").Where("TABLE_NAME=\"tn_SystemData\""));
                    if (tableCount > 0 && string.IsNullOrEmpty(SCHEMA_NAME))
                    {
                        messages["当前数据库不是本程序数据库！"] = string.Empty;
                        TempData["Error"] = messages;
                        return Json(new { });
                    }
                }

                #endregion
            }
            //修改web.config中数据库链接字符串
            connectString += "database=" + model.DataBase;

            SetWebConfig(connectString, model.DBType, out messages);
            return Json(new { success = true, connectString = connectString, DBType = model.DBType });
        }

        /// <summary>
        /// 安装数据库表结构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Step2_Install_Schema()
        {
            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
            string connectString = Request.Form.Get<string>("connectString", string.Empty);
            DBType dbType = Request.Form.Get<DBType>("DBType", DBType.SqlServer);
            //连接新库
            Database db = CreateDatabase(connectString, dbType, ref messages);

            if (messages.Keys.Count > 0)
            {
                WriteLogFile(messages);
                return Json(new { });
            }

            List<string> fileList = SetupHelper.GetInstallFiles(dbType).Where(n => n.Contains("Schema")).ToList();
            string message = string.Empty;
            foreach (var file in fileList)
            {
                try
                {
                    SetupHelper.ExecuteInFile(db, file, out messages);
                }
                catch { }
                if (messages.Count > 0)
                {
                    WriteLogFile(messages);
                    return Json(new StatusMessageData(StatusMessageType.Error, "安装数据库表结构时出现错误，请查看安装日志！"));
                }
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "安装数据库表结构成功！"));
        }

        /// <summary>
        /// 数据库初始化及创建系统管理员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _Step2_Install_InitialData()
        {
            ConcurrentDictionary<string, string> messages = new ConcurrentDictionary<string, string>();
            string connectString = Request.Form.Get<string>("connectString", string.Empty);
            DBType dbType = Request.Form.Get<DBType>("DBType", DBType.SqlServer);
            Database db = CreateDatabase(connectString, dbType, ref messages);

            if (messages.Keys.Count > 0)
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "连接字符串不对！"));
            }
            string administrator = Request.Form.Get<string>("Administrator", string.Empty);
            string userPassword = Request.Form.Get<string>("UserPassword", string.Empty);
            KeyValuePair<string, string> adminInfo = new KeyValuePair<string, string>(administrator, UserPasswordHelper.EncodePassword(userPassword, Tunynet.Common.UserPasswordFormat.MD5));
            string mainRootSiteUrl = Request.Form.Get<string>("MainRootSiteUrl", string.Empty);
            List<string> fileList = SetupHelper.GetInstallFiles(dbType, null, true).Where(n => n.Contains("InitialData") || n.Contains("CreateAdministrator")).ToList();
            string message = string.Empty;
            foreach (var file in fileList)
            {
                try
                {
                    SetupHelper.ExecuteInFile(db, file, out messages, adminInfo, mainRootSiteUrl);
                }
                catch { }
                if (messages.Count > 0)
                {
                    WriteLogFile(messages);
                    return Json(new StatusMessageData(StatusMessageType.Error, "执行数据库初始化脚本时出现错误，请查看安装日志！"));
                }
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "安装数据库表结构成功！"));
        }

        /// <summary>
        /// 安装日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult InstallLog()
        {
            return File(GetLogFileName(), "text/plain", "install.log");
        }

        /// <summary>
        /// 获取安装日志文件名
        /// </summary>
        /// <returns></returns>
        private string GetLogFileName()
        {
            string currentDirectory = WebUtility.GetPhysicalFilePath("~/Uploads");
            return currentDirectory + "\\install.log";
        }

        /// <summary>
        /// 确保文件已被创建
        /// </summary>
        /// <param name="fileName">带路径的文件名</param>
        /// <returns></returns>
        private bool EnsureFileExist(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                return true;
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                    fs.Close();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 将升级信息写入升级日志中
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        private bool WriteLogFile(ConcurrentDictionary<string, string> messages)
        {

            string fileName = GetLogFileName();
            if (!EnsureFileExist(fileName))
                return false;

            StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8);   //该编码类型不会改变已有文件的编码类型
            foreach (var message in messages)
            {
                sw.WriteLine(DateTime.Now.ToString() + "：" + string.Format("{0}:{1}", message.Key, message.Value));
            }
            sw.Close();
            return true;
        }


        /// <summary>
        /// 安装成功
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Success()
        {
            try
            {
                var dao = Database.CreateInstance();
                dao.Execute("insert into tn_SystemData(Datakey,LongValue,DecimalValue) values ('SPBVersion',0,4.2)");
            }
            catch { }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetSite()
        {
            CheckWebConfig();
            return new EmptyResult();
        }


        #region Helper Method

        private bool CheckFolderWriteable(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(Server.MapPath(path));
                return true;
            }

            try
            {
                string testFilePath = string.Format("{0}/test{1}{2}{3}{4}.txt", path, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                FileStream TestFile = System.IO.File.Create(testFilePath);
                TestFile.WriteByte(Convert.ToByte(true));
                TestFile.Close();
                System.IO.File.Delete(testFilePath);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 检测web.config的权限
        /// </summary>
        /// <returns></returns>
        private bool CheckWebConfig()
        {
            FileInfo FileInfo = new FileInfo(Server.MapPath("~/Web.config"));
            if (!FileInfo.Exists)
                return false;

            System.Xml.XmlDocument xmldocument = new System.Xml.XmlDocument();
            xmldocument.Load(FileInfo.FullName);
            try
            {
                XmlNode moduleNode = xmldocument.SelectSingleNode("//httpModules");
                if (moduleNode.HasChildNodes)
                {
                    for (int i = 0; i < moduleNode.ChildNodes.Count; i++)
                    {
                        XmlNode node = moduleNode.ChildNodes[i];
                        if (node.Name == "add")
                        {
                            if (node.Attributes.GetNamedItem("name").Value == "SpaceBuilderModule")
                            {
                                moduleNode.RemoveChild(node);
                                break;
                            }
                        }
                    }
                }
                xmldocument.Save(FileInfo.FullName);
            }
            catch
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 设置文件权限
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="username">需要设置权限的用户名</param>
        private bool SetAccount(string filePath, string username)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            FileSecurity fileSecurity = fileInfo.GetAccessControl();

            try
            {
                fileSecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, AccessControlType.Allow));
                fileInfo.SetAccessControl(fileSecurity);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置文件夹访问权限
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="userName">需要设置权限的用户名</param>
        /// <param name="rights">访问权限</param>
        /// <param name="allowOrDeny">允许拒绝访问</param>
        private bool SetFolderACL(string folderPath, string userName, FileSystemRights rights, AccessControlType allowOrDeny)
        {

            InheritanceFlags inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            return SetFolderACL(folderPath, userName, rights, allowOrDeny, inherits, PropagationFlags.None, AccessControlModification.Add);

        }

        /// <summary>
        /// 设置文件夹访问权限
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="userName">需要设置权限的用户名</param>
        /// <param name="rights">访问权限</param>
        /// <param name="allowOrDeny">允许拒绝访问</param>
        /// <param name="inherits">继承标志指定访问控制项 (ACE) 的继承语义</param>
        /// <param name="propagateToChildren">指定如何将访问面控制项 (ACE) 传播到子对象。仅当存在继承标志时，这些标志才有意义</param>
        /// <param name="addResetOrRemove">指定要执行的访问控制修改的类型。此枚举由 System.Security.AccessControl.ObjectSecurity 类及其子类的方法使用</param>
        private bool SetFolderACL(string folderPath, string userName, FileSystemRights rights, AccessControlType allowOrDeny, InheritanceFlags inherits, PropagationFlags propagateToChildren, AccessControlModification addResetOrRemove)
        {
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            DirectorySecurity dSecurity = folder.GetAccessControl(AccessControlSections.All);
            FileSystemAccessRule accRule = new FileSystemAccessRule(userName, rights, inherits, propagateToChildren, allowOrDeny);

            bool modified;
            dSecurity.ModifyAccessRule(addResetOrRemove, accRule, out modified);
            folder.SetAccessControl(dSecurity);

            return modified;
        }

        //设置web.config
        private void SetWebConfig(string connectionString, DBType dbType, out ConcurrentDictionary<string, string> messages)
        {
            messages = new ConcurrentDictionary<string, string>();
            System.IO.FileInfo FileInfo = new FileInfo(Server.MapPath("~/web.config"));

            if (!FileInfo.Exists)
            {
                messages[string.Format("文件 : {0} 不存在", Server.MapPath("~/web.config"))] = "";
            }

            XElement rootElement = XElement.Load(FileInfo.FullName);

            XElement connectionStringsElement = rootElement.Descendants("connectionStrings").FirstOrDefault();
            if (connectionStringsElement != null && connectionStringsElement.HasElements)
            {
                XElement element = connectionStringsElement.Elements("add").LastOrDefault(n => n.NodeType != XmlNodeType.Comment);
                if (element != null)
                {
                    try
                    {
                        element.Attribute("name").Value = dbType.ToString();
                        element.Attribute("connectionString").Value = connectionString;
                        element.SetAttributeValue("providerName", GetProviderName(dbType));
                    }
                    catch (Exception e)
                    {
                        messages[e.Message] = e.StackTrace;
                    }
                }
            }

            rootElement.Save(FileInfo.FullName);
        }

        /// <summary>
        /// 从web.config中获取连接字符串
        /// </summary>
        /// <returns></returns>
        private string GetConnectionStringFromWebConfig()
        {
            string connectionString = string.Empty;
            System.IO.FileInfo FileInfo = new FileInfo(Server.MapPath("~/web.config"));

            if (!FileInfo.Exists)
                return string.Empty;

            XElement rootElement = XElement.Load(FileInfo.FullName);
            XElement connectionStringsElement = rootElement.Descendants("connectionStrings").FirstOrDefault();
            if (connectionStringsElement != null && connectionStringsElement.HasElements)
            {
                XElement element = connectionStringsElement.Elements("add").LastOrDefault(n => n.NodeType != XmlNodeType.Comment);
                if (element != null)
                {
                    try
                    {
                        connectionString = element.Attribute("connectionString").Value;
                    }
                    catch { }
                }
            }
            return connectionString;
        }

        /// <summary>
        /// 获取数据库链接提供者
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private string GetProviderName(DBType dbType)
        {
            var providerName = string.Empty;
            switch (dbType)
            {
                case DBType.MySql:
                    providerName = "MySql.Data.MySqlClient";
                    break;
                //case DBType.SqlCE:
                //    providerName = "System.Data.EntityClient";
                //    break;
                case DBType.SqlServer:
                default:
                    providerName = "System.Data.SqlClient";
                    break;
            }
            return providerName;
        }

        /// <summary>
        /// 创建数据库访问对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dbType"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        private Database CreateDatabase(string connectionString, DBType dbType, ref ConcurrentDictionary<string, string> messages)
        {
            if (messages == null)
                messages = new ConcurrentDictionary<string, string>();

            //DbProviderFactory factory = DbProviderFactories.GetFactory(GetProviderName(dbType));
            string providerName = GetProviderName(dbType);
            try
            {
                return new Database(connectionString, providerName);
            }
            catch (Exception e)
            {
                messages[e.Message] = e.StackTrace;
                return null;
            }
        }

        #endregion
    }
}
