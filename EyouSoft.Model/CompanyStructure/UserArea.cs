using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 用户线路区域
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class UserArea
    {
        #region Model
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int AreaId
        {
            set;
            get;
        }
        /// <summary>
        /// 用户姓名[应用端无须赋值]
        /// </summary>
        public string ContactName
        {
            get;
            set;
        }
        #endregion Model
    }
}
