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
    /// 望海-散拼计划标准行程单
    /// 修改：田想兵 时间:2011.5.24
    /// 修改说明：免登录打开
    /// </summary>
    public partial class StandardSanFightPrint : System.Web.UI.Page
    {

        protected int tdIndex = 1;
        protected int tdCount = 0;
        protected DateTime? LeaveDate = null;
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
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(null,false);
            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourInfo model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(tourId);
            if (model != null && model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
            {
                //出团日期
                LeaveDate = model.LDate;
                //旅游天数
                lblDayCount.Text = model.TourDays.ToString();
                #region 页面控件赋值
                //线路名称
                this.lblAreaName.Text = model.RouteName;
                //团号
                this.lblTeamNum.Text = model.TourCode;
                this.lblTeamNum.Visible = true;
                this.txttourcode.Visible = false;
                //人数
                this.lblAdult.Text = model.PlanPeopleNumber.ToString();
                this.lblAdult.Visible = true;
                this.TXTADULT.Visible = false;
                //出发交通
                this.lblBegin.Text = model.LTraffic;
                this.lblBegin.Visible = true;
                this.TXtBEGIN.Visible = false;
                //返程交通
                this.lblEnd.Text = model.RTraffic;
                this.lblEnd.Visible = true;
                this.TXTEnd.Visible = false;
                if (model.TourNormalInfo != null)
                {
                    //不含项目
                    this.lblNoProject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.BuHanXiangMu);
                    //购物安排
                    this.lblBuy.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.GouWuAnPai);
                    //儿童安排
                    this.lblChildPlan.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ErTongAnPai);
                    //自费项目
                    this.lblSelfProject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ZiFeiXIangMu);
                    //注意事项
                    this.lblNote.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ZhuYiShiXiang);
                    //温馨提示
                    this.lblTips.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.WenXinTiXing);
                    //行程安排
                    this.rptTravel.DataSource = model.TourNormalInfo.Plans;
                    this.rptTravel.DataBind();
                    //包含项目
                    if (model.TourNormalInfo.Services != null)
                    {
                        tdCount = model.TourNormalInfo.Services.Count * 2;
                    }
                    this.rptProject.DataSource = model.TourNormalInfo.Services;
                    this.rptProject.DataBind();
                }
                //地接社信息
                this.rptDjInfo.DataSource = model.LocalAgencys;
                this.rptDjInfo.DataBind();
                this.Panel1.Visible = true;
                #endregion
            }

            //组团散客天天发行程单
            if (EyouSoft.Common.Utils.GetQueryStringValue("action") == "tourevery")
            {
                string tourid = Utils.GetQueryStringValue("tourid");
                EyouSoft.BLL.TourStructure.TourEveryday everybl = new EyouSoft.BLL.TourStructure.TourEveryday(null, false);
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
                    this.lblDayCount.Text = everymodel.TourDays.ToString();
                    //成人数
                    this.lblAdult.Visible = false;
                    this.TXTADULT.Attributes.Add("style", "width:30px;");
                    //儿童数

                    //出发交通
                    this.lblBegin.Visible = false;
                    //返程交通
                    this.lblEnd.Visible = false;
                    if (everymodel.TourNormalInfo != null)
                    {
                        //不含项目
                        this.lblNoProject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(everymodel.TourNormalInfo.BuHanXiangMu);
                        //购物安排
                        this.lblBuy.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(everymodel.TourNormalInfo.GouWuAnPai);
                        //儿童安排
                        this.lblChildPlan.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(everymodel.TourNormalInfo.ErTongAnPai);
                        //自费项目
                        this.lblSelfProject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(everymodel.TourNormalInfo.ZiFeiXIangMu);
                        //注意事项
                        this.lblNote.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(everymodel.TourNormalInfo.ZhuYiShiXiang);
                        //温馨提示
                        this.lblTips.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(everymodel.TourNormalInfo.WenXinTiXing);
                        //行程安排
                        this.rptTravel.DataSource = everymodel.TourNormalInfo.Plans;
                        this.rptTravel.DataBind();
                        //包含项目
                        if (everymodel.TourNormalInfo.Services != null)
                        {
                            tdCount = everymodel.TourNormalInfo.Services.Count*2;
                        }
                        this.rptProject.DataSource = everymodel.TourNormalInfo.Services;
                        this.rptProject.DataBind();
                    }

                    //地接社信息
                    this.Panel1.Visible = false;
                }
            }
        }

        /// <summary>
        /// 根据出团日期 获得每一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected string GetDateByIndex(int index)
        {
            if (LeaveDate != null)
            {
                DateTime dateTime = Convert.ToDateTime(LeaveDate);
                return dateTime.AddDays(index).ToString("yyyy-MM-dd");
            }
            else
            {
                return "";
            }
        }

        protected string GetDinnerByValue(string dinner)
        {
            string str = "";
            if (dinner.Trim().Length > 0)
            {
                string[] list = dinner.Split(',');
                if (list.Contains("2"))
                {
                    str += "早,";
                }
                if (list.Contains("3"))
                {
                    str += "中,";
                }
                if (list.Contains("4"))
                {
                    str += "晚,";
                }
            }
            str = str.TrimEnd(',');
            return str;
        }
    }
}
