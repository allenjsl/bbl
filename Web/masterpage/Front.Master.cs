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
    public partial class Front : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 专线logo
        /// </summary>
        protected string BigLogo = "";
        /// <summary>
        /// 组团logo
        /// </summary>
        protected string InnerLogo = "";

        protected FrontPage frontPage= null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //初始化backPage
            frontPage = this.Page as FrontPage;
            if (frontPage == null)
            {
                throw new Exception("页面没有正确继承FrontPage");
            }

            if (!Page.IsPostBack)
            {
                //组团logo
                EyouSoft.Model.CompanyStructure.CustomerConfig Customer =
                new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerConfigModel(frontPage.SiteUserInfo.TourCompany.TourCompanyId);
                if (Customer != null)
                {
                    if (Customer.FilePathLogo != "" && !string.IsNullOrEmpty(Customer.FilePathLogo))
                    {
                        InnerLogo = Customer.FilePathLogo;
                    }
                }
                //专线Logo
                BigLogo = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetCompanyLogo(frontPage.SiteUserInfo.TourCompany.TourCompanyId, EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户);
             
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

            if (currentPageUrl.Equals(//首页
                "/GroupEnd/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Default.Attributes["class"] = h2ShowClass;
                ulDefault.Attributes["style"] = showStyle;
                linkDefault.Attributes["class"] = highLineClass;
            } if (currentPageUrl.Equals(//最新动态
                 "/GroupEnd/News/Newslist.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals(//最新动态
                 "/GroupEnd/NoticeShow.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Default.Attributes["class"] = h2ShowClass;
                ulDefault.Attributes["style"] = showStyle;
                linkNewsList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//留言
                 "/GroupEnd/Messages/MessageBoard.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Default.Attributes["class"] = h2ShowClass;
                ulDefault.Attributes["style"] = showStyle;
                linkMessage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//线路列表
                "/GroupEnd/LineList/LineProductsList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2LineProduct.Attributes["class"] = h2ShowClass;
                ulLineProduct.Attributes["style"] = showStyle;
                linkLineProduct.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals("/GroupEnd/FitHairDay/FitHairDayList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2FitHairDay.Attributes["class"] = h2ShowClass;
                ulFitHairDay.Attributes["style"] = showStyle;
                linkFitHairDay.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//我的订单
             "/GroupEnd/Orders/OrderList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2OrderList.Attributes["class"] = h2ShowClass;
                ulOrderList.Attributes["style"] = showStyle;
                linkOrderList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//我要询价
             "/GroupEnd/JourneyMoney/SelectMoney.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/GroupEnd/JourneyMoney/SelectMoneyAdd.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/GroupEnd/JourneyMoney/DownloadMoney.aspx",StringComparison.OrdinalIgnoreCase))
            {
                h2SelectMoney.Attributes["class"] = h2ShowClass;
                ulSelectMoney.Attributes["style"] = showStyle;
                linkSelectMoney.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//财务管理
                "/GroupEnd/AccountFinces/AccountList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Account.Attributes["class"] = h2ShowClass;
                ulAccount.Attributes["style"] = showStyle;
                linkAccount.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//系统设置
                "/GroupEnd/SystemSetting/CompanyInfo.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/GroupEnd/SystemSetting/AccountManager.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/GroupEnd/SystemSetting/DeployManager.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkSystemSet.Attributes["class"] = highLineClass;
            }
        }
    }
}
