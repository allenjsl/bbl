using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-固定资产管理信息实体
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class FixedAsset
    {
        #region Model
       
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { set; get; }
        /// <summary>
        /// 固定资产编号
        /// </summary>
        public string AssetNo { set; get; }
        /// <summary>
        /// 资产名称
        /// </summary>
        public string AssetName { set; get; }
        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime BuyDate { set; get; }
        /// <summary>
        /// 折旧费
        /// </summary>
        public decimal? Cost { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { set; get; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { set; get; }
        #endregion Model
    }
}
