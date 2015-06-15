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

namespace Web.print.wh
{
    /// <summary>
    /// 望海-团队计划快速行程单
    /// 功能：显示行程单
    /// 创建人：戴银柱
    /// </summary>
    public partial class FastTourInfoPrint : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tourId = Utils.GetQueryStringValue("tourId");
                if (tourId != "")
                {
                    DataInit(tourId);
                }
            }
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="travelId">行程单ID</param>
        protected void DataInit(string tourId)
        {
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour();
            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourTeamInfo model = (EyouSoft.Model.TourStructure.TourTeamInfo)bll.GetTourInfo(tourId);
            if (model != null)
            {
                //线路名称
                this.lblAreaName.Text = model.RouteName;
                //团号
                this.lblTeamNum.Text = model.TourCode;
                //天数
                this.lblDay.Text = model.TourDays.ToString();
                //成人数
                this.lblAdult.Text = model.PlanPeopleNumber.ToString();
                //儿童数

                //出发交通
                this.lblBegin.Text = model.LTraffic;
                //返程交通
                this.lblEnd.Text = model.RTraffic;
                if (model.TourQuickInfo != null)
                {
                    //行程安排
                    this.litTravel.Text = model.TourQuickInfo.QuickPlan;
                    //服务标准
                    this.litService.Text = model.TourQuickInfo.Service;
                }
                //地接社信息
                this.rptDjInfo.DataSource = model.LocalAgencys;
                this.rptDjInfo.DataBind();
            }

        }
    }
}
