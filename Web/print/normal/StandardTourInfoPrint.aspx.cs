using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.print.normal
{
    /// <summary>
    /// 芭比莱团队计划标准版行程单
    /// 功能：显示行程单
    /// 创建人：戴银柱
    /// 创建时间： 2011-02-11 
    /// </summary>
    public partial class StandardTourInfoPrint : BackPage
    {
        /// <summary>
        /// 出团日期
        /// </summary>
        protected DateTime? LeaveDate = null;

        /// <summary>
        /// 行程安排 TD 数
        /// </summary>
        protected int rptTravelTDCount = 0;

        /// <summary>
        /// 地接 TD 数
        /// </summary>
        protected int rptDjInfoTDCount = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="tourId">行程单ID</param>
        protected void DataInit(string tourId)
        {
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourTeamInfo model = (EyouSoft.Model.TourStructure.TourTeamInfo)bll.GetTourInfo(tourId);
            if (model != null)
            {
                #region 页面控件赋值
                //线路名称
                this.lblAreaName.Text = model.RouteName;
                //团号
                this.lblTeamNum.Text = model.TourCode;
                //人数
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
                //出团日期
                LeaveDate = model.LDate;
                //行程安排
                if (model.TourNormalInfo.Plans != null && model.TourNormalInfo.Plans.Count > 0)
                {
                    rptTravelTDCount = model.TourNormalInfo.Plans.Count * 4;
                    this.rptTravel.DataSource = model.TourNormalInfo.Plans;
                    this.rptTravel.DataBind();
                }

                //包含项目
                this.rptProject.DataSource = model.Services;
                this.rptProject.DataBind();
                //地接社信息
                if (model.LocalAgencys != null && model.LocalAgencys.Count > 0)
                {
                    rptDjInfoTDCount = model.LocalAgencys.Count;
                }
                this.rptDjInfo.DataSource = model.LocalAgencys;
                this.rptDjInfo.DataBind();

                #endregion
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dinner"></param>
        /// <returns></returns>
        protected string GetDinnerByValue(string dinner)
        {
            string str = "";
            if (dinner.Trim().Length > 0)
            {
                string[] list = dinner.Split(',');
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list[i].Trim() != "")
                    {
                        switch (list[i])
                        {
                            case "2": str += "早餐,"; break;
                            case "3": str += "中餐,"; break;
                            case "4": str += "晚餐,"; break;
                            default: break;
                        }
                    }
                }
            }
            str = str.TrimEnd(',');
            return str;
        }
    }
}
