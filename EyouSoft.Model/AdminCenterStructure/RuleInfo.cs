using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-规章制度信息实体
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class RuleInfo
    {
        #region Model      
        /// <summary>
        /// 编号
        /// </summary>
        public int Id{ set; get; }         
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId{ set; get; }
        /// <summary>
        /// 规章制度编号
        /// </summary>
        public string RoleNo{ set; get; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title{ set; get; }
        /// <summary>
        /// 内容
        /// </summary>
        public string RoleContent{ set; get; }
        /// <summary>
        /// 附件
        /// </summary>
        public string FilePath{ set; get; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId{ set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime{ set; get; }
        #endregion Model

    }
}
