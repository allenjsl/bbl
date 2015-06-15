using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.SSOComponent.Entity
{
    #region 用户信息
    /// <summary>
    /// 用户信息
    /// </summary>
    /// 开发人：蒋胜蓝  开发时间：2010-5-31
    [Serializable]
    public class UserInfo
    {        
        EyouSoft.Model.CompanyStructure.PassWord _passwordinfo = new EyouSoft.Model.CompanyStructure.PassWord();
        EyouSoft.Model.CompanyStructure.ContactPersonInfo _ContactPersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo();
        TourCompany _TourCompany = new TourCompany();
        LocalAgencyCompanyInfo _LocalAgencyCompanyInfo = new LocalAgencyCompanyInfo();
        
        #region 属性
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司编号（组团用户登录时是专线公司编号）
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 联系信息
        /// </summary>
        public EyouSoft.Model.CompanyStructure.ContactPersonInfo ContactInfo { get { return _ContactPersonInfo; } /*set { _ContactPersonInfo = value; } */}
        /// <summary>
        /// 用户密码
        /// </summary>
        public EyouSoft.Model.CompanyStructure.PassWord PassWordInfo { get { return this._passwordinfo; } }
        /// <summary>
        /// 角色权限列表
        /// </summary>
        public int[] PermissionList { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 兼管部门编号
        /// </summary>
        public int JGDepartId { get; set; }       
        /// <summary>
        /// 登录凭据值
        /// </summary>
        public string LoginTicket { get; set; }
        /// <summary>
        /// 部门信息集合
        /// </summary>
        public int[] Departs { get { return new int[] { this.DepartId, this.JGDepartId }; } }
        /// <summary>
        /// 用户线路区域信息集合
        /// </summary>
        public int[] Areas { get; set; }
        
        /// <summary>
        /// 组团社用户信息
        /// </summary>
        public TourCompany TourCompany
        {
            get { return this._TourCompany; }
        }
        /// <summary>
        /// 用户类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType UserType { get; set; }
        /// <summary>
        /// 地接社公司信息业务实体
        /// </summary>
        public LocalAgencyCompanyInfo LocalAgencyCompanyInfo { get { return this._LocalAgencyCompanyInfo; } }
        #endregion
    }    
    #endregion

    #region 登录实体类
    /// <summary>
    /// 登录实体类
    /// </summary>
    public class LocalUserInfo
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UID
        {
            get;
            set;
        }
        /// <summary>
        /// 登录凭据值
        /// </summary>
        public string LoginTicket { get; set; }
        /// <summary>
        /// 原始登录凭据值
        /// </summary>
        public DecryptLoginTicket DecryptLoginTicket { get; set; }
    }
    #endregion

    #region 原始登录凭据
    /// <summary>
    /// 原始登录凭据
    /// </summary>
    public class DecryptLoginTicket
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime
        {
            get;
            set;
        }
    }
    #endregion

    #region 用户密码类型
    /// <summary>
    /// 用户密码类型
    /// </summary>
    public enum PasswordType
    {
        /// <summary>
        /// MD5
        /// </summary>
        MD5
    }
    #endregion

    #region 组团社用户信息
    /// <summary>
    /// 组团社用户信息
    /// </summary>
    [Serializable]
    public class TourCompany
    {
        /// <summary>
        /// 组团公司编号
        /// </summary>
        public int TourCompanyId { get; set; }
        /// <summary>
        /// 客户等级
        /// </summary>
        public int CustomerLevel { get; set; }
        /// <summary>
        /// 组团社名称
        /// </summary>
        public string CompanyName { get; set; }
    }
    #endregion

    #region 地接社公司信息业务实体
    /// <summary>
    /// 地接社公司信息业务实体
    /// </summary>
    [Serializable]
    public class LocalAgencyCompanyInfo
    {
        /// <summary>
        /// 地接社公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 地接社公司名称
        /// </summary>
        public string CompanyName { get; set; }
    }
    #endregion
}
