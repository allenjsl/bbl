using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl.wh
{
    /// <summary>
    /// 底部用户控件
    /// </summary>
    public partial class WhBottomControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CompanyId == 0)
                {
                    EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
                    CompanyId = domain.CompanyId;
                }

                //获得基本设置实体
                EyouSoft.Model.SiteStructure.SiteBasicConfig configModel = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(CompanyId);
                if (configModel != null)
                {
                    this.lclFooter.Text = configModel.Copyright;
                }

                //获得友情链接 
                int recordCount = 0;
                IList<EyouSoft.Model.SiteStructure.SiteFriendLink> linkList = new EyouSoft.BLL.SiteStructure.SiteFriendLink().GetSiteFriendLink(CompanyId, 10, 1, ref recordCount);
                this.rptLinkList.DataSource = linkList;
                this.rptLinkList.DataBind();
            }
        }
    }
}