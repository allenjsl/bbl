using System;
namespace EyouSoft.Model.SysStructure
{
    /// <summary>
    /// 创建人：蒋胜蓝 2011-01-25
    /// 描述：系统域名实体类
    /// </summary>
    [Serializable]
    public class SystemDomain
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SystemDomain() { }

        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        ///目标url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 域名类型
        /// </summary>
        public EyouSoft.Model.EnumType.SysStructure.DomainType DomainType { get; set; }
    }
}

