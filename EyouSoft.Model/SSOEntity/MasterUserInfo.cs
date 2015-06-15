using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.SSOComponent.Entity
{
    /// <summary>
    /// 管理员信息
    /// </summary>
    /// 开发人：蒋胜蓝  开发时间：2010-5-31
    [Serializable]
    public class MasterUserInfo
    {
        EyouSoft.Model.CompanyStructure.PassWord _passwordinfo = new EyouSoft.Model.CompanyStructure.PassWord();

        /// <summary>
        /// default constructor
        /// </summary>
        public MasterUserInfo() { }

        /// <summary>
        /// 编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Realname { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 角色权限列表
        /// </summary>
        public int[] Permissions { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public EyouSoft.Model.CompanyStructure.PassWord PassWordInfo
        {
            get { return this._passwordinfo; }
            set { this._passwordinfo = value; }
        }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }        
        /// <summary>
        /// 登录凭据值
        /// </summary>
        public string LoginTicket { get; set; }
    }
}
