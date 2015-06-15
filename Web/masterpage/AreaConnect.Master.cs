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
using Eyousoft.Common.Page;

namespace Web.masterpage
{
    /// <summary>
    /// 用于地接用户页面的模板页
    /// 张新兵，2011-4-6
    /// </summary>
    public partial class AreaConnect : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 外logo
        /// </summary>
        protected string BigLogo = "";

        protected AreaConnectPage areaConnectPage = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //初始化areaConnectPage
            areaConnectPage = this.Page as AreaConnectPage;
            if (areaConnectPage == null)
            {
                throw new Exception("页面没有正确继承AreaConnectPage");
            }

            if (!Page.IsPostBack)
            {
                //初始化外Logo
                BigLogo = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetCompanyLogo(areaConnectPage.SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户);

                AutoPositionLinks();
            }
        }

        /// <summary>
        /// 根据打开的页面 ，自动定位 左边菜单那
        /// </summary>
        private void AutoPositionLinks()
        {
            string currentPageUrl = Request.Url.AbsolutePath.ToLower();
            string showStyle = "display:'';";
            string highLineClass = "listIn";
            string h2ShowClass = "firstNav";

            
            if (currentPageUrl.Equals(//个人中心-地接安排
                "/UserCenter/DjArrangeMengs/TravelAgencyList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2UserCenter.Attributes["class"] = h2ShowClass;
                ulUserCenter.Attributes["style"] = showStyle;
                linkArrangeMengs.Attributes["class"] = highLineClass;
            }
        }
    }
}
