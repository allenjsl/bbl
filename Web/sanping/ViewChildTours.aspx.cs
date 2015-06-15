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

namespace Web.sanping
{
    /// <summary>
    /// 创建人：张新兵 ，创建时间：2011-01-13
    /// 查看子团的日历列表
    /// </summary>
    public partial class ViewChildTours :Eyousoft.Common.Page.BackPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
