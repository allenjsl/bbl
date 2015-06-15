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
    /// 望海-团队计划标准行程单
    /// 功能：显示行程单
    /// 创建人：戴银柱
    /// </summary>
    public partial class StandardTourInfoPrint : Eyousoft.Common.Page.BackPage
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
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourTeamInfo model = (EyouSoft.Model.TourStructure.TourTeamInfo)bll.GetTourInfo(tourId);
            if (model != null && model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
            {
                #region 页面控件赋值
                //线路名称
                this.lblAreaName.Text = model.RouteName;
                //团号
                this.lblTeamNum.Text = model.TourCode;
                //人数
                this.lblAdult.Text = model.PlanPeopleNumber.ToString();
                //天数
                this.lblDayCount.Text = model.TourDays.ToString();
                //出发交通
                this.lblBegin.Text = model.LTraffic;
                //返程交通
                this.lblEnd.Text = model.RTraffic;
                //出团日期
                LeaveDate = model.LDate;
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
                }
                //包含项目
                //包含项目
                if (model.Services != null)
                {
                    tdCount = model.Services.Count * 2;
                }
                this.rptProject.DataSource = model.Services;
                this.rptProject.DataBind();
                //地接社信息
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
