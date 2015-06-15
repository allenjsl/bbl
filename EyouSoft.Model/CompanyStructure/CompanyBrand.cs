using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 公司品牌实体
    /// </summary>
    /// 鲁功源 2011-01-18
    [Serializable]
    public class CompanyBrand
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyBrand()
		{ }
        #endregion

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
		/// 品牌名称
		/// </summary>
		public string BrandName
		{
            get;
            set;
		}
		/// <summary>
		/// 内LOGO
		/// </summary>
		public string Logo1
		{
            get;
            set;
		}
		/// <summary>
		/// 外LOGO
		/// </summary>
		public string Logo2
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
		/// 公司编号
		/// </summary>
		public int CompanyId
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
		#endregion Model

    }
}
