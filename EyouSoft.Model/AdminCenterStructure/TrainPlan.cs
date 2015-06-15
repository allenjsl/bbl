using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-培训计划信息实体
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class TrainPlan
    {
        #region Model       
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int? CompanyId { set; get; }
        /// <summary>
        /// 计划标题
        /// </summary>
        public string PlanTitle { set; get; }
        /// <summary>
        /// 计划内容
        /// </summary>
        public string PlanContent { set; get; }
        /// <summary>
        /// 培训附件
        /// </summary>
        public string TrainPlanFile { set; get; }
        /// <summary>
        /// 发布人编号
        /// </summary>
        public int? OperatorId { set; get; }
        /// <summary>
        /// 发布人名称
        /// </summary>
        public string OperatorName { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? IssueTime { set; get; }
        #endregion Model
        /// <summary>
        /// 接受对象集合
        /// </summary>
        public IList<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts> AcceptList { set; get; }
    }
}
