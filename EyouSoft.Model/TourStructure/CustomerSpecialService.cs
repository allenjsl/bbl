using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    /// <summary>
    /// 游客特服信息实体
    /// 创建人：luofx 2011-01-19
    /// </summary>
    public class CustomerSpecialService
    {
        #region Model
        /// <summary>
        /// 所属游客编号
        /// </summary>
        public string CustormerId { set; get; }
        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectName { set; get; }
        /// <summary>
        /// 服务内容
        /// </summary>
        public string ServiceDetail { set; get; }
        /// <summary>
        /// 增/减(true=增,false=减)
        /// </summary>
        public bool IsAdd { set; get; }
        /// <summary>
        /// 费用
        /// </summary>
        public decimal Fee { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { set; get; }
        #endregion Model
    }
}
