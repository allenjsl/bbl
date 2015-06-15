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

namespace Web.print.wh.groupend
{
    /// <summary>
    /// 望海-组团-标准行程单
    /// </summary>
    public partial class StandardSanFightPrint : Eyousoft.Common.Page.FrontPage
    {
        protected DateTime? LeaveDate = null;
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
        /// <param name="tourId">行程单ID</param>
        protected void DataInit(string tourId)
        {
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourInfo model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(tourId);
            if (model != null)
            {
                #region 页面控件赋值
                //出团日期
                LeaveDate = model.LDate;

                //线路名称
                this.lblAreaName.Text = model.RouteName;
                //团号
                this.lblTeamNum.Text = model.TourCode;
                //人数
                this.lblAdult.Text = model.PlanPeopleNumber.ToString();
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
                //行程安排
                this.rptTravel.DataSource = model.TourNormalInfo.Plans;
                this.rptTravel.DataBind();
                //包含项目
                this.rptProject.DataSource = model.TourNormalInfo.Services;
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
