using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SysStructure
{
    /// <summary>
    /// webmaster account info
    /// </summary>
    /// Auhtor:汪奇志 2011-04-26
    public class MWebmasterInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MWebmasterInfo() { }

        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public EyouSoft.Model.CompanyStructure.PassWord Password { get; set; }
    }
}
