using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using PetaPoco;
using Tunynet.Repositories;
using System.Text.RegularExpressions;
using Tunynet;
using System.Web.Management;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Common;


namespace Spacebuilder.Common
{
    public class SystemInfo
    {
        //操作系统名称
        private string pcName;
        public string PCName
        {
            get
            {
                //如果操作系统为空则获取一下
                if (string.IsNullOrEmpty(this.pcName))
                {
                    RegistryKey rk;
                    rk = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                    this.pcName = rk.GetValue("ProductName").ToString();
                }
                return this.pcName;
            }
        }

        private int osVersion;
        public int OSVersion
        {
            get
            {
                if (osVersion == 0)
                {
                    RegistryKey rk;
                    rk = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                    int.TryParse(rk.GetValue("CurrentBuildNumber").ToString(), out this.osVersion);
                }

                return this.osVersion;
            }
        }

        //IIS版本号
        private string iis;
        public string IIS
        {
            get
            {
                //如果IIS为空则获取
                if (string.IsNullOrEmpty(this.iis))
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp");
                    this.iis = "IIS" + key.GetValue("MajorVersion").ToString();
                }
                return this.iis;
            }
        }

        //framework版本号
        private string framework;
        public string Framework
        {
            get
            {
                //如果framework版本号为空则获取
                if (string.IsNullOrEmpty(this.framework))
                {
                    Version v = Environment.Version;
                    if (v != null)
                    {
                        this.framework = v.Major + "." + v.Minor;
                    }
                }
                return this.framework;
            }
        }

        //数据库类型
        private string dataBaseVersion;
        public string DataBaseVersion
        {

            get
            {
                //如果数据库类型为空则获取
                if (string.IsNullOrEmpty(dataBaseVersion))
                {
                    string dbType = GetDBtype();
                    var dao = Database.CreateInstance();
                    string DBversion = string.Empty;
                    if (dbType.StartsWith("MySql"))
                    {
                        DBversion = dao.ExecuteScalar<object>(Sql.Builder.Select("version()")).ToString();
                    }
                    else
                    {
                        DBversion = dao.ExecuteScalar<object>(Sql.Builder.Select("@@@version")).ToString();
                    }
                    this.dataBaseVersion = DBversion;
                    if (!string.IsNullOrEmpty(DBversion))
                    {
                        Match match = Regex.Match(DBversion, @"^(?<DBversion>.*)-");
                        if (match.Success)
                        {
                            //获得有效字符串
                            this.dataBaseVersion = match.Groups["DBversion"].Value;
                        }
                    }
                }
                return this.dataBaseVersion;
            }
        }

        //.NET信任级别
        private string netTrustLevel;
        public string NetTrustLevel
        {

            get
            {
                TrustSection t = new TrustSection();
                this.netTrustLevel = t.Level;
                return this.netTrustLevel;
            }
        }

        //数据库占用
        private string getDBSize;
        public string GetDBSize
        {

            get
            {
                if (string.IsNullOrEmpty(getDBSize))
                {
                    string dbType = GetDBtype();
                    var dao = Database.CreateInstance();
                    if (dbType.StartsWith("MySql"))
                    {
                        long dataSize = dao.First<long>(Sql.Builder.Append("SELECT sum(DATA_LENGTH)+sum(INDEX_LENGTH) FROM information_schema.TABLES where TABLE_SCHEMA=database();"));
                        this.getDBSize = (dataSize / 1048576.0) + "M";
                    }
                    else
                    {
                        dynamic databaseInfo = dao.FirstOrDefault<dynamic>("execute sp_spaceused");
                        if (databaseInfo != null)
                            this.getDBSize = databaseInfo.database_size;
                    }
                }
                return this.getDBSize;
            }
        }

        private string GetDBtype()
        {
            var providerName = "System.Data.SqlClient";
            int connectionStringsCount = ConfigurationManager.ConnectionStrings.Count;
            if (connectionStringsCount > 0)
                providerName = ConfigurationManager.ConnectionStrings[connectionStringsCount - 1].ProviderName;
            else
                throw new InvalidOperationException("Can't find a connection string '");
            DbProviderFactory _factory = null;
            if (!string.IsNullOrEmpty(providerName))
                _factory = DbProviderFactories.GetFactory(providerName);
            if (_factory != null)
                return _factory.GetType().Name;
            return string.Empty;
        }

    }
}