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

namespace Web.print.bbl.groupend
{
    /// <summary>
    /// 芭比来-组团订单中心游客名单
    /// 张新兵，2011-2-15
    /// </summary>
    public partial class OrderVisitorsListPrint :Eyousoft.Common.Page.FrontPage
    {
        protected string orderId = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                orderId = Utils.GetQueryStringValue("orderid");
                InitVisitorListAndRouteInfo();
            }
        }

        /// <summary>
        /// 初始化游客信息列表
        /// </summary>
        protected void InitVisitorListAndRouteInfo()
        {
            EyouSoft.BLL.TourStructure.TourOrder order = new EyouSoft.BLL.TourStructure.TourOrder(base.SiteUserInfo);

            EyouSoft.Model.TourStructure.TourOrder orderModel = order.GetOrderModel(base.SiteUserInfo.CompanyID, orderId);

            if (orderModel == null)
            {
                Response.Clear();
                Response.Write("未找到信息");
                Response.End();
            }

            ltrTourNumber.Text = orderModel.TourNo;
            ltrRouteName.Text = orderModel.RouteName;
            ltrDays.Text = orderModel.Tourdays.ToString();

            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> list = orderModel.CustomerList.Where(x=>x.CustomerStatus==EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList();

            rptVisitorList.DataSource = list;
            rptVisitorList.DataBind();
        }
    }
}
