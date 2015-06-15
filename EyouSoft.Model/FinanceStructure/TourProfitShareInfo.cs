/*Author:汪奇志 2011-01-19*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.FinanceStructure
{
    #region 团队利润分配信息业务实体
    /// <summary>
    /// 团队利润分配信息业务实体
    /// </summary>
    public class TourProfitShareInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourProfitShareInfo() { }

        /// <summary>
        /// 分配编号
        /// </summary>
        public string ShareId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 分配项目
        /// </summary>
        public string ShareItem { get; set; }
        /// <summary>
        /// 分配金额
        /// </summary>
        public decimal ShareCost { get; set; }
        /// <summary>
        /// 分配到的部门编号
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// 分配到的部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 分配到的人员编号
        /// </summary>
        public int SaleId { get; set; }
        /// <summary>
        /// 分配到的人员名称
        /// </summary>
        public string Saler { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 分配人编号
        /// </summary>
        public int OperatorId { get; set; }
    }
    #endregion
}
