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

namespace Web.print.bbl.groupend
{
    /// <summary>
    /// 芭比来-组团-快速行程单
    /// </summary>
    public partial class FastSanFightPrint : Eyousoft.Common.Page.FrontPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tourId = EyouSoft.Common.Utils.GetQueryStringValue("tourId");
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
            EyouSoft.Model.TourStructure.TourInfo model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(tourId);
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
                //行程安排
                this.litTravel.Text = model.TourQuickInfo.QuickPlan;
                //服务标准
                this.litService.Text = model.TourQuickInfo.Service;
                //地接社信息
                this.rptDjInfo.DataSource = model.LocalAgencys;
                this.rptDjInfo.DataBind();
            }
        }
    }
}
