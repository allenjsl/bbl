using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    #region 上传报价信息表实体
    /// <summary>
    /// 上传报价信息表实体
    /// </summary>
    public class QuoteAttach
    {
        /// <summary>
        /// 报价附件编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 上传报价公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 上传文件路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// 上传人编号(当前登录用户编号)
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 上传人姓名
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }

        /// <summary>
        /// 上传报价附件联系人信息
        /// </summary>
        //public EyouSoft.Model.CompanyStructure.ContactPersonInfo ContactPersonInfo
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 报价信息开始有效期
        /// </summary>
        public DateTime? ValidityStart
        {
            get;
            set;
        }
        /// <summary>
        /// 报价信息结束有效期
        /// </summary>
        public DateTime? ValidityEnd
        {
            get;
            set;
        }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime? AddTime
        {
            get;
            set;
        }
    }
    #endregion
}

