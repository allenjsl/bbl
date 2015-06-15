using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 公司报价等级实体
    /// </summary>
    /// 鲁功源 2011-01-18
    [Serializable]
    public class CompanyPriceStand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyPriceStand()
        { }

        #region Model
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 报价名称
        /// </summary>
        public string PriceStandName
        {
            get;
            set;
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime
        {
            get;
            set;
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete
        {
            get;
            set;
        }
        /// <summary>
        /// 是否系统默认
        /// </summary>
        public bool IsSystem
        {
            get;
            set;
        }
        #endregion Model
    }
}
