using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.SingleServe
{
    /// <summary>
    /// 页面功能：导入游客信息
    /// Author:liuym
    /// Date:2011-01-13
    /// </summary>
    public partial class LoadVisitors : Eyousoft.Common.Page.BackPage
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


        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion
       
    }
}
