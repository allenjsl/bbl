using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.print.bbl
{
    /// <summary>
    /// 芭比莱团队计划快速版行程单
    /// 功能：显示行程单
    /// 创建人：戴银柱
    /// 创建时间： 2011-02-11 
    /// </summary>
    public partial class FastTourInfoPrint : BackPage
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
                #region 人数And结算价
                if (model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                {
                    TourQuotePrint priantTQP = new TourQuotePrint();
                    string number = string.Empty;
                    string money = string.Empty;
                    priantTQP.getPepoleNum(SiteUserInfo.CompanyID, model.PlanPeopleNumber, model.TourTeamUnit, ref number, ref money);
                    this.lblAdult.Text = number;
                }
                else
                {
                    this.lblAdult.Text = model.PlanPeopleNumber.ToString();
                }
                #endregion

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
