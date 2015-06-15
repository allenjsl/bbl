using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl
{
    /// <summary>
    ///叶面功能：打印按钮用户控件
    ///Author:liuym
    ///Date:2011-01-23
    /// </summary>
    public partial class UCPrintButton : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 属性
        /// <summary>
        /// 数据列表编号（table的ID）
        /// </summary>
        public string ContentId
        {
            get;
            set;
        }
        #endregion
    }
}