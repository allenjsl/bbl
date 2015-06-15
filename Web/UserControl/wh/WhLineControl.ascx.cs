using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl.wh
{
    /// <summary>
    /// 左侧联系方式用户控件
    /// </summary>
    public partial class WhLineControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CompanyId == 0)
            {
                EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
                CompanyId = domain.CompanyId;
            }

            var info = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(CompanyId);

            if (info != null)
            {
                ltr.Text = info.LianXiFangShi;
            }
        }
    }
}