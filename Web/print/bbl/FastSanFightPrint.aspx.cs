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
using Eyousoft.Common.Page;

namespace Web.print.bbl
{
    /// <summary>
    /// 芭比来-散拼计划快速行程单
    /// </summary>
    public partial class FastSanFightPrint : Eyousoft.Common.Page.BackPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string travelId = Utils.GetQueryStringValue("tourid");
                if (travelId != "")
                {
                    DataInit(travelId);
                }
            }
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="travelId">行程单ID</param>
        protected void DataInit(string travelId)
        {
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour();
            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourInfo model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(travelId);
            if (model != null)
            {
                //线路名称
                this.lblAreaName.Text = model.RouteName;
                //团号
                this.lblTeamNum.Text = model.TourCode;
                this.lblTeamNum.Visible = true;
                this.txttourcode.Visible = false;
                //天数
                this.lblDay.Text = model.TourDays.ToString();
                //成人数
                this.lblAdult.Text = model.PlanPeopleNumber.ToString();
                this.lblAdult.Visible = true;
                this.TXTADULT.Visible = false;
                //儿童数
                this.lblChild.Visible = true;
                this.TXTCHILD.Visible = false;
                //出发交通
                this.lblBegin.Text = model.LTraffic;
                this.lblBegin.Visible = true;
                this.TXtBEGIN.Visible = false;
                //返程交通
                this.lblEnd.Text = model.RTraffic;
                this.lblEnd.Visible = true;
                this.TXTEnd.Visible = false;
                //行程安排
                this.litTravel.Text = model.TourQuickInfo.QuickPlan;
                //服务标准
                this.litService.Text = model.TourQuickInfo.Service;
                //地接社信息
                this.rptDjInfo.DataSource = model.LocalAgencys;
                this.rptDjInfo.DataBind();
                this.pnedij.Visible = true;
            }

            //组团散客天天发行程单
            if (EyouSoft.Common.Utils.GetQueryStringValue("action") == "tourevery")
            {
                string tourid = Utils.GetQueryStringValue("tourid");
                EyouSoft.BLL.TourStructure.TourEveryday everybl = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
                EyouSoft.Model.TourStructure.TourEverydayInfo everymodel = new EyouSoft.Model.TourStructure.TourEverydayInfo();
                everymodel = everybl.GetTourEverydayInfo(tourid);
                if (everymodel != null)
                {
                    //线路名称
                    this.lblAreaName.Text = everymodel.RouteName;                    
                    //团号                                     
                    this.lblTeamNum.Visible = false;
                    this.txttourcode.Visible = true;
                    //天数
                    this.lblDay.Text = everymodel.TourDays.ToString();
                    //成人数
                    this.lblAdult.Visible = false;
                    this.TXTADULT.Visible = true;
                    this.TXTADULT.Attributes.Add("style", "width:30px;");
                    //儿童数
                    this.lblChild.Visible = false;
                    this.TXTCHILD.Visible = true;
                    this.TXTCHILD.Attributes.Add("style", "width:30px;");
                    //出发交通
                    this.lblBegin.Visible = false;
                    this.TXtBEGIN.Visible = true;
                    //返程交通
                    this.lblEnd.Visible = false;
                    this.TXTEnd.Visible = true;
                   
                    //行程安排
                    this.litTravel.Text = everymodel.TourQuickInfo.QuickPlan;
                    //服务标准
                    this.litService.Text = everymodel.TourQuickInfo.Service;
                    //地接社信息
                    this.pnedij.Visible = false;
                }                
            }
        }
    }
}
