using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    #region 用户帐号基本信息实体
    /// <summary>
    /// 用户帐号基本信息实体
    /// </summary>
    /// 创建人：鲁功源 2011-01-20
    [Serializable]
    public class UserAccount
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码(在应用层设置时,只需设置其NoEncryptPassword属性)
        /// </summary>
        public PassWord PassWordInfo { get; set; }
        /// <summary>
        /// 专线公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 组团公司编号
        /// </summary>
        public int TourCompanyId { get; set; }

        #region 附加属性
        /// <summary>
        /// 用户线路区域集合
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.UserArea> UserAreaList
        {
            get;
            set;
        }
        #endregion
    }
    #endregion

    #region 用户信息实体
    /// <summary>
    /// 专线公司用户信息表
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class CompanyUser : UserAccount
    {
        #region Model
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName
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
        /// <summary>
        /// 监管部门编号
        /// </summary>
        public int SuperviseDepartId
        {
            get;
            set;
        }
        /// <summary>
        /// 监管部门名称
        /// </summary>
        public string SuperviseDepartName
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人信息
        /// </summary>
        public ContactPersonInfo PersonInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 上次登录IP
        /// </summary>
        public string LastLoginIP
        {
            get;
            set;
        }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime
        {
            get;
            set;
        }
        /// <summary>
        /// 权限组(角色)编号
        /// </summary>
        public int RoleID
        {
            get;
            set;
        }
        /// <summary>
        /// 权限集合(权限值以逗号隔开)
        /// </summary>
        public string PermissionList
        {
            get;
            set;
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用 1:启用  0:停用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin
        {
            get;
            set;
        }
        /// <summary>
        /// 操作时间 
        /// </summary>
        public DateTime IssueTime
        {
            get;
            set;
        }
        #endregion Model

        /// <summary>
        /// 部门主管
        /// </summary>
        public int DepartManger { get; set; }

    }
    #endregion

    #region 用户密码实体
    /// <summary>
    /// 密码实体
    /// </summary>
    /// 创建人：鲁功源 2011-01-20
    [Serializable]
    public class PassWord
    {
        private readonly EyouSoft.Common.DataProtection.HashCrypto hashcrypto = new EyouSoft.Common.DataProtection.HashCrypto();
        /// <summary>
        /// MD5加密密码
        /// </summary>
        private string _md5password = "";

        /// <summary>
        /// 获取或设置未加密密码(只需要设置未加密密码即可)
        /// </summary>
        public string NoEncryptPassword { get; set; }
        /// <summary>
        /// 获取MD5加密密码(只需要设置未加密密码即可)
        /// </summary>
        public string MD5Password { get { return hashcrypto.MD5Encrypt(this.NoEncryptPassword); } }

        /// <summary>
        /// 构造方法
        /// </summary>
        public PassWord() { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="noencryptpassword">未加密密码</param>
        public PassWord(string noencryptpassword)
        {
            this.NoEncryptPassword = noencryptpassword;
        }

        /// <summary>
        /// 设置所有密码(该方法只需在业务逻辑层使用)
        /// </summary>
        /// <param name="noencryptpassword">未加密密码</param>
        /// <param name="md5password">MD5加密密码</param>
        public void SetEncryptPassWord(string noencryptpassword, string md5password)
        {
            this.NoEncryptPassword = noencryptpassword;
            this._md5password = md5password;
        }
    }
    #endregion

    #region 联系人信息实体类
    /// <summary>
    /// 联系人信息实体类
    /// </summary>
    /// 创建人：鲁功源 2011-01-20
    [Serializable]
    public class ContactPersonInfo
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType UserType
        {
            get;
            set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string ContactName
        {
            get;
            set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.Sex ContactSex
        {
            get;
            set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string ContactTel
        {
            get;
            set;
        }
        /// <summary>
        /// 传真
        /// </summary>
        public string ContactFax
        {
            get;
            set;
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string ContactMobile
        {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string ContactEmail
        {
            get;
            set;
        }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ
        {
            get;
            set;
        }
        /// <summary>
        /// MSN
        /// </summary>
        public string MSN
        {
            get;
            set;
        }
        /// <summary>
        /// 职务
        /// </summary>
        public string JobName
        {
            get;
            set;
        }
        /// <summary>
        /// 个人简介
        /// </summary>
        public string PeopProfile
        {
            get;
            set;
        }
        /// <summary>
        /// 个人备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
    }
    #endregion

    #region 用户查询实体

    /// <summary>
    /// 用户查询实体
    /// </summary>
    public class QueryCompanyUser
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 组团公司Id
        /// </summary>
        public int ZuTuanCompanyId { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public Model.EnumType.CompanyStructure.CompanyUserType? UserType { get; set; }
    }

    #endregion
}
