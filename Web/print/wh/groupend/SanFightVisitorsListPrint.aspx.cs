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
using EyouSoft.Common;
using System.Collections.Generic;

namespace Web.print.wh.groupend
{
    /// <summary>
    /// 望海-组团-游客名单
    /// </summary>
    public partial class SanFightVisitorsListPrint :Eyousoft.Common.Page.FrontPage
    {
        protected string tourId = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tourId = Utils.GetQueryStringValue("tourid");
                InitRouteInfo();
                InitVisitorList();
            }
        }

        /// <summary>
        /// 初始化线路信息
        /// </summary>
        protected void InitRouteInfo()
        {
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour();

            EyouSoft.Model.TourStructure.TourInfo tourInfo = bll.GetTourInfo(tourId) as EyouSoft.Model.TourStructure.TourInfo;

            if (tourInfo == null)
            {
                Response.Clear();
                Response.Write("未找到信息");
                Response.End();
            }

            ltrTourNumber.Text = tourInfo.TourCode;
            ltrRouteName.Text = tourInfo.RouteName;
            ltrDays.Text = tourInfo.TourDays.ToString();
        }

        /// <summary>
        /// 初始化游客信息列表
        /// </summary>
        protected void InitVisitorList()
        {
            EyouSoft.BLL.TourStructure.TourOrder order = new EyouSoft.BLL.TourStructure.TourOrder(base.SiteUserInfo);

            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> list = order.GetTravellers(tourId).Where(x => x.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList();
            ;

            rptVisitorList.DataSource = list;
            rptVisitorList.DataBind();
        }
    }
}
