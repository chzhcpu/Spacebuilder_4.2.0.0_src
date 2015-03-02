//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-05-24</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-05-24">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using Tunynet.Repositories;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 敏感词业务逻辑类
    /// </summary>
    public class SensitiveWordService
    {
        private ISensitiveWordRepository sensitiveWordRepository;
        private IRepository<SensitiveWordType> sensitiveWordTypeRepository;

        /// <summary>
        /// 构造器方法
        /// </summary>
        public SensitiveWordService()
            : this(new SensitiveWordRepository(), new SensitiveWordTypeRepository())
        {

        }

        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="sensitiveWordRepository">敏感词数据访问</param>
        /// <param name="sensitiveWordTypeRepository">敏感词类型数据访问</param>
        public SensitiveWordService(ISensitiveWordRepository sensitiveWordRepository, IRepository<SensitiveWordType> sensitiveWordTypeRepository)
        {
            this.sensitiveWordRepository = sensitiveWordRepository;
            this.sensitiveWordTypeRepository = sensitiveWordTypeRepository;
        }

        #region SensitiveWord

        /// <summary>
        /// 添加敏感词
        /// </summary>
        /// <param name="sensitiveWord">敏感词实体类型</param>
        public int Create(SensitiveWord sensitiveWord)
        {
            EventBus<SensitiveWord>.Instance().OnBefore(sensitiveWord, new CommonEventArgs(EventOperationType.Instance().Create()));
            int judge = sensitiveWordRepository.Create(sensitiveWord);
            EventBus<SensitiveWord>.Instance().OnAfter(sensitiveWord, new CommonEventArgs(EventOperationType.Instance().Create()));
            return judge;
        }


        /// <summary>
        /// 添加敏感词
        /// </summary>
        /// <param name="stream">敏感词文件流</param>
        /// <param name="typeId">敏感词类型Id</param>
        public void BatchCreate(Stream stream, int? typeId)
        {
            List<string> words = new List<string>();

            StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default);
            string word = string.Empty;

            while (!string.IsNullOrEmpty(word = sr.ReadLine()))
            {
                if (string.IsNullOrEmpty(word))
                    continue;
                words.Add(word);
            }

            BatchCreate(words, typeId);
        }


        /// <summary>
        /// 添加敏感词
        /// </summary>
        /// <param name="words">敏感词集合</param>
        /// <param name="typeId">敏感词类型Id</param>
        public void BatchCreate(IList<string> words, int? typeId)
        {
            if (words == null)
            {
                return;
            }

            words = words.Distinct().ToList();

            List<SensitiveWord> sensitiveWords = new List<SensitiveWord>();
            foreach (string word in words)
            {
                SensitiveWord sensitiveWord = SensitiveWord.New();
                if (word.Contains("="))
                {
                    string[] parts = word.Split('=');

                    if (parts.Count() == 2)
                    {
                        sensitiveWord.Word = parts[0];
                        sensitiveWord.Replacement = parts[1];
                    }
                }
                else
                {
                    sensitiveWord.Word = word;
                    sensitiveWord.Replacement = "*";
                }

                if (typeId.HasValue)
                {
                    sensitiveWord.TypeId = typeId.Value;
                }

                sensitiveWords.Add(sensitiveWord);

            }

            EventBus<SensitiveWord>.Instance().OnBatchBefore(sensitiveWords, new CommonEventArgs(EventOperationType.Instance().Create()));
            sensitiveWordRepository.BatchInsert(sensitiveWords);
            EventBus<SensitiveWord>.Instance().OnBatchAfter(sensitiveWords, new CommonEventArgs(EventOperationType.Instance().Create()));
        }


        /// <summary>
        /// 更新敏感词
        /// </summary>
        /// <param name="sensitiveWord">待更新敏感词</param>
        /// <returns></returns>
        public int Update(SensitiveWord sensitiveWord)
        {
            EventBus<SensitiveWord>.Instance().OnBefore(sensitiveWord, new CommonEventArgs(EventOperationType.Instance().Update()));
            int judge = sensitiveWordRepository.Update(sensitiveWord);
            EventBus<SensitiveWord>.Instance().OnAfter(sensitiveWord, new CommonEventArgs(EventOperationType.Instance().Update()));
            return judge;
        }

        /// <summary>
        /// 删除敏感词
        /// </summary>
        /// <param name="id">敏感词Id</param>
        public void Delete(int id)
        {
            SensitiveWord word = sensitiveWordRepository.Get(id);

            EventBus<SensitiveWord>.Instance().OnBefore(word, new CommonEventArgs(EventOperationType.Instance().Update()));
            sensitiveWordRepository.Delete(word);
            EventBus<SensitiveWord>.Instance().OnBefore(word, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 获取敏感词实体
        /// </summary>
        /// <param name="id">敏感词Id</param>
        public SensitiveWord Get(int id)
        {
            return sensitiveWordRepository.Get(id);
        }

        /// <summary>
        /// 获取敏感词集合（后台用）
        /// </summary>
        /// <param name="keyword">敏感词关键字</param>
        /// <param name="typeId">分类Id</param>
        public IEnumerable<SensitiveWord> Gets(string keyword = "", int? typeId = null)
        {
            return sensitiveWordRepository.GetSensitiveWords(keyword, typeId);
        }

        /// <summary>
        /// 导出敏感词
        /// </summary>
        /// <returns></returns>
        public byte[] Export()
        {
            IEnumerable<SensitiveWord> sensitiveWords = Gets();
            if (sensitiveWords == null)
                return null;

            byte[] bytes = new byte[65535];

            int i = 0;
            foreach (SensitiveWord sensitiveWord in sensitiveWords)
            {
                foreach (byte b in System.Text.ASCIIEncoding.Default.GetBytes(sensitiveWord.Word + "=" + sensitiveWord.Replacement + "\r\n"))
                {
                    bytes[i] = b;
                    i++;
                }
            }

            return bytes;
        }

        #endregion

        #region SensitiveWordType

        /// <summary>
        /// 创建敏感词类型
        /// </summary>
        public void CreateType(SensitiveWordType type)
        {
            sensitiveWordTypeRepository.Insert(type);
        }

        /// <summary>
        /// 删除敏感词类型
        /// </summary>
        /// <param name="typeId">敏感词类型Id</param>
        public void DeleteType(object typeId)
        {
            sensitiveWordTypeRepository.DeleteByEntityId(typeId);
        }

        /// <summary>
        /// 更新敏感词类型
        /// </summary>
        /// <param name="type">敏感词类型实体</param>
        public void UpdateType(SensitiveWordType type)
        {
            sensitiveWordTypeRepository.Update(type);
        }

        /// <summary>
        /// 获取敏感词类型集合
        /// </summary>
        public IEnumerable<SensitiveWordType> GetAllSensitiveWordTypes()
        {
            return sensitiveWordTypeRepository.GetAll();
        }

        /// <summary>
        /// 获取敏感词类型集合
        /// </summary>
        /// <param name="id">敏感词类型Id</param>
        public SensitiveWordType GetSensitiveWordType(int id)
        {
            return sensitiveWordTypeRepository.Get(id);
        }


        #endregion
    }
}
