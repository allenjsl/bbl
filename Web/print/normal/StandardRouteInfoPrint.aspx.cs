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

namespace Web.print.normal
{
    /// <summary>
    /// 芭比来-线路标准行程单
    /// 显示标准线路行程单
    /// 创建人：李晓欢
    /// 创建时间：2011-02-15
    /// </summary>
    public partial class StandardRouteInfoPrint : Eyousoft.Common.Page.BackPage
    {


        /// <summary>
        /// 行程安排 TD 数
        /// </summary>
        protected int rptTravelTDCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                string RouteId = EyouSoft.Common.Utils.GetQueryStringValue("RouteID");
                if (RouteId != "")
                {
                    DataInit(RouteId);
                }
            }
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="travelId">行程单ID</param>
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
                //不含项目
                this.lblNoProject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(RouteInfo.RouteNormalInfo.BuHanXiangMu);
                //购物安排
                this.lblBuy.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(RouteInfo.RouteNormalInfo.GouWuAnPai);
                //儿童安排
                this.lblChildPlan.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(RouteInfo.RouteNormalInfo.ErTongAnPai);
                //自费项目
                this.lblSelfProject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(RouteInfo.RouteNormalInfo.ZiFeiXIangMu);
                //注意事项
                this.lblNote.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(RouteInfo.RouteNormalInfo.ZhuYiShiXiang);
                //温馨提示
                this.lblTips.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(RouteInfo.RouteNormalInfo.WenXinTiXing);
                //行程安排
                if (RouteInfo.RouteNormalInfo.Plans != null && RouteInfo.RouteNormalInfo.Plans.Count > 0)
                {
                    rptTravelTDCount = RouteInfo.RouteNormalInfo.Plans.Count * 4;
                }
                this.rptTravel.DataSource = RouteInfo.RouteNormalInfo.Plans;
                this.rptTravel.DataBind();
                //包含项目
                this.rptProject.DataSource = RouteInfo.RouteNormalInfo.Services;
                this.rptProject.DataBind();
            }
        }

        /// <summary>
        /// 行程用餐转换
        /// </summary>
        /// <param name="Dinner"></param>
        /// <returns></returns>
        protected string GetDinnerValue(string Dinner)
        {
            string DinnerValue = "";
            string[] list = Dinner.Split(',');
            if (Dinner.Trim().Length > 0)
            {
                if (list.Contains("2"))
                {
                    DinnerValue += "早,";
                }
                if (list.Contains("3"))
                {
                    DinnerValue += "中,";
                }
                if (list.Contains("4"))
                {
                    DinnerValue += "晚,";
                }
            }
            return DinnerValue.TrimEnd(',');
        }

        protected string TextTohtml(string str)
        {
            string html = "";
            html = str.Replace("\n", "<br/>");
            return html;
        }
    }
}
