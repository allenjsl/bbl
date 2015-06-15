using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{

    #region 供应商基本信息
    /// <summary>
    /// 供应商基本信息
    /// </summary>
    /// 鲁功源 2011-03-07
    public class SupplierBasic
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierBasic() { }
        
        /// <summary>
        /// 供应商信息编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 省份编号
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 交易次数
        /// </summary>
        public int TradeNum { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string UnitAddress { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 供应商联系人
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.SupplierContact> SupplierContact { get; set; }
        /// <summary>
        /// 供应商图片信息
        /// </summary>
        public IList<EyouSoft.Model.SupplierStructure.SupplierPic> SupplierPic { get; set; }
        /// <summary>
        /// 许可证号
        /// </summary>
        public string LicenseKey { get; set; }
    }
    #endregion

    #region 供应商地接、票务实体
    /// <summary>
    /// 供应商信息表[地接，票务]
    /// 创建人：xuqh 2011-01-19
    /// </summary>
    public class CompanySupplier : SupplierBasic
    {
        #region Model 
        /// <summary>
        /// 返佣
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// 合作协议
        /// </summary>
        public string AgreementFile { get; set; }

        /// <summary>
        /// 政策
        /// </summary>
        public string UnitPolicy { get; set; }

        #endregion
    }
    #endregion

    #region 供应商查询信息业务实体
    /// <summary>
    /// 供应商查询信息业务实体
    /// </summary>
    /// 汪奇志 2011-06-20
    public class MSupplierSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MSupplierSearchInfo() { }

        /// <summary>
        /// 省份编号
        /// </summary>
        public int? ProvinceId { get; set; }
        /// <summary>
        /// 城市编号
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }
    }
    #endregion
}
