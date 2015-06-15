using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl.ucSystemSet
{   
    /// <summary>
    /// 系统设置-基础设置头部菜单
    /// xuty 2011/01/12
    /// </summary>
    public partial class BasicInfoHeaderMenu : System.Web.UI.UserControl
    {
        protected Eyousoft.Common.Page.BackPage backPage;
        protected void Page_Load(object sender, EventArgs e)
        {
            backPage = this.Page as Eyousoft.Common.Page.BackPage;
        }
        /// <summary>
        /// 菜单选中项
        /// </summary>
        public int TabIndex
        {
            get;
            set;
        }
    }
}