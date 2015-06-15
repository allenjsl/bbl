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

namespace Web.print.bbl
{
    /// <summary>
    /// 散客天天发游客列表
    /// by 田想兵 3.25
    /// </summary>
    public partial class DayDayPublishVisitorsListPrint : Eyousoft.Common.Page.BackPage
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
            EyouSoft.BLL.TourStructure.TourEveryday bl = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
            string tourId = Utils.GetQueryStringValue("tourid");
            string ApplyId = Utils.GetQueryStringValue("ApplyId");
            EyouSoft.Model.TourStructure.TourEverydayInfo model = bl.GetTourEverydayInfo(tourId);
            if (model == null)
            {
                Response.Clear();
                Response.Write("未找到信息");
                Response.End();
            }

            ltrRouteName.Text = model.RouteName;
            ltrDays.Text = model.TourDays.ToString();
        }

        /// <summary>
        /// 初始化游客信息列表
        /// </summary>
        protected void InitVisitorList()
        {
            EyouSoft.BLL.TourStructure.TourEveryday bl = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourEverydayApplyTravellerInfo applyModel = new EyouSoft.Model.TourStructure.TourEverydayApplyTravellerInfo();
            string tourId = Utils.GetQueryStringValue("tourid");
            string ApplyId = Utils.GetQueryStringValue("ApplyId");
            applyModel = bl.GetTourEverydayApplyTravellerInfo(ApplyId);
            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> list = applyModel.Travellers;
            rptVisitorList.DataSource = list.Where(x => x.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList();
            rptVisitorList.DataBind();
        }
    }
}
