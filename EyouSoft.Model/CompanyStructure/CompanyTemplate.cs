using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 打印模版实体
    /// </summary>
    /// 鲁功源 2011-01-18
    [Serializable]
    public class CompanyTemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyTemplate()
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
        /// 模版名称
        /// </summary>
        public string TemplateName
        {
            get;
            set;
        }
        /// <summary>
        /// 模版类型[枚举]
        /// </summary>
        public int TempType
        {
            get;
            set;
        }
        /// <summary>
        /// 模版编辑区
        /// </summary>
        public string TempPath
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
        #endregion Model
    }

    public class TemplateUseRange
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TemplateUseRange() { }

        #region Model
        /// <summary>
        /// 模版Id
        /// </summary>
        public int TempId
        {
            get;
            set;
        }
        /// <summary>
        /// 模版类型[枚举]
        /// </summary>
        public int TempType
        {
            get;
            set;
        }
        /// <summary>
        /// 应用范围
        /// </summary>
        public int UseRange
        {
            get;
            set;
        }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartId
        {
            get;
            set;
        }
        #endregion Model
    }
}
