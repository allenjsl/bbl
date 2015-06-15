using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 系统登录日志实体
    /// </summary>
    /// 鲁功源 2011-01-18
    [Serializable]
    public class SysLoginLog
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysLoginLog()
        { }

        #region Model
        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 登录人
        /// </summary>
        public int Operator
        {
            get;
            set;
        }
        /// <summary>
        /// 部门
        /// </summary>
        public int DepatId
        {
            get;
            set;
        }
        /// <summary>
        /// 公司id
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime
        {
            get;
            set;
        }
        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIp
        {
            get;
            set;
        }
        #endregion Model
    }
}
