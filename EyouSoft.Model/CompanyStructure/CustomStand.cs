using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 公司客户等级实体
    /// </summary>
    /// 鲁功源 2011-01-18
    [Serializable]
    public class CustomStand
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomStand()
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
        /// 客户等级类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.CustomLevType LevType
        {
            get;
            set;
        }
        /// <summary>
        /// 客户等级名称
        /// </summary>
        public string CustomStandName
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
        /// 是否系统默认：0 不是；1是
        /// </summary>
        public bool IsSystem
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
        /// 销售员提成比例
        /// </summary>
        public decimal SalerCommision { get; set; }

        /// <summary>
        /// 计调员提成比例
        /// </summary>
        public decimal LogisticsCommision { get; set; }

        #endregion Model
    }
}
