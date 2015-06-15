using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ComponentModel;

namespace Web.UserControl
{
    /// <summary>
    /// 张新兵，20110124，导入游客用户控件
    /// </summary>
    public partial class LoadVisitors : System.Web.UI.UserControl
    {
        protected string currentPageIframeId = "";
        /// <summary>
        /// 设置用户控件所在弹窗页面的iframeID
        /// </summary>
        [Bindable(true)]
        public string CurrentPageIframeId
        {
            set
            {
                currentPageIframeId = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(currentPageIframeId))
                {
                    throw new Exception("请设置LoadVisitors控件的CurrentPageIframeId属性");
                }
            }
        }
    }
}