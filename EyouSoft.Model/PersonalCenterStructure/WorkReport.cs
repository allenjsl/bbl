using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.PersonalCenterStructure
{
    #region 工作汇报实体
    /// <summary>
    /// 工作汇报实体
    /// </summary>
    /// 鲁功源  2011-01-17
    [Serializable]
    public class WorkReport
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkReport()
        { }
        #endregion

        #region Model
        /// <summary>
        /// 汇报编号
        /// </summary>
        public int ReportId
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
        /// 汇报标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报内容
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报附件路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报人所在部门编号
        /// </summary>
        public int DepartmentId
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报人编号
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报人姓名
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报时间
        /// </summary>
        public DateTime ReportingTime
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public EyouSoft.Model.EnumType.PersonalCenterStructure.CheckState Status
        {
            get;
            set;
        }
        /// <summary>
        /// 评语
        /// </summary>
        public string Comment
        {
            get;
            set;
        }
        /// <summary>
        /// 审核人
        /// </summary>
        public int CheckerId
        {
            get;
            set;
        }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime CheckTime
        {
            get;
            set;
        }
        #endregion Model

        #region 附加属性
        /// <summary>
        /// 汇报人所在部门名称
        /// </summary>
        public string DepartmentName
        {
            get;
            set;
        }
        #endregion
    }
    #endregion

    #region 工作汇报查询实体
    /// <summary>
    /// 工作汇报查询实体
    /// </summary>
    public class QueryWorkReport
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryWorkReport() { }

        #region 属性
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报部门编号
        /// </summary>
        public int DepartmentId
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报人名称
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报开始时间
        /// </summary>
        public DateTime? CreateSDate
        {
            get;
            set;
        }
        /// <summary>
        /// 汇报结束时间
        /// </summary>
        public DateTime? CreateEDate
        {
            get;
            set;
        }
        #endregion
    }
    #endregion
}
