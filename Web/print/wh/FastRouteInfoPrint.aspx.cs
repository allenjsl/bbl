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

namespace Web.print.wh
{
    /// <summary>
    /// 望海-线路快速行程单
    /// 显示快速线路行程单
    /// 创建人：李晓欢
    /// 创建时间：2011-02-15
    /// </summary>
    public partial class FastRouteInfoPrint : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                string RouteID = EyouSoft.Common.Utils.GetQueryStringValue("RouteID");
                if (RouteID != "")
                {
                    DataInit(RouteID);
                }
            }         
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="RouteId">行程单ID</param>
        protected void DataInit(string RouteId)
        {
            //声明bll对象
            EyouSoft.BLL.RouteStructure.Route Route = new EyouSoft.BLL.RouteStructure.Route(SiteUserInfo);
            //声明团队计划对象
            EyouSoft.Model.RouteStructure.RouteInfo RouteInfo = (EyouSoft.Model.RouteStructure.RouteInfo)Route.GetRouteInfo(Convert.ToInt32(RouteId));
            if (RouteInfo != null)
            {
                //线路名称
                this.lblAreaName.Text = RouteInfo.RouteName;
                //天数
                this.lblDay.Text = RouteInfo.RouteDays.ToString();
                //发布人姓名
                this.lblAuthor.Text = RouteInfo.OperatorName;
                //上团数
                this.lblTourCount.Text = RouteInfo.TourCount.ToString();
                //收客数
                this.lblVisitorCount.Text = RouteInfo.VisitorCount.ToString();
                //行程安排
                this.litTravel.Text = RouteInfo.RouteQuickInfo.QuickPlan;
                //服务标准
                this.litService.Text = RouteInfo.RouteQuickInfo.Service;
            }

        }
    }
}
