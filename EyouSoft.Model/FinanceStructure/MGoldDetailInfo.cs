using System;

namespace EyouSoft.Model.FinanceStructure
{
    /// <summary>
    /// 收入、支出增加减少费用明细信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-04-29
    public class MGoldDetailInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MGoldDetailInfo() { }

        /// <summary>
        /// 明细编号
        /// </summary>
        public string DetailId { get; set; }
        /// <summary>
        /// 增加费用
        /// </summary>
        public decimal AddAmount { get; set; }
        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作员编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 关联编号
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// 关联类型
        /// </summary>
        public EyouSoft.Model.EnumType.FinanceStructure.GoldType ItemType { get; set; }
    }
}
