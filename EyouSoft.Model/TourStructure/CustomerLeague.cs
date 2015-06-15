using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    /// <summary>
    /// 游客退团信息实体
    /// 创建人：luofx 2011-01-19
    /// </summary>
    public class CustomerLeague
    {
        #region Model
        /// <summary>
        /// 所属游客编号
        /// </summary>
        public string CustormerId { set; get; }
        /// <summary>
        /// 操作员名字
        /// </summary>
        public string OperatorName { set; get; }
        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperatorID { set; get; }
        /// <summary>
        /// 退团原因
        /// </summary>
        public string RefundReason { set; get; }
        /// <summary>
        /// 退团时间
        /// </summary>
        public DateTime? IssueTime { set; get; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal RefundAmount { set; get; }
        #endregion Model
    }
}
