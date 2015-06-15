using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;

namespace Web.SingleServe
{
    /// <summary>
    /// 页面功能：单项服务--单据打印
    /// Author:liuym
    /// Date:2011-02-13
    /// </summary>
    public partial class BillPrint :BackPage
    {
        /// <summary>
        /// OnPreInit事件 设置模式窗体属性
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
