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
    /// 芭比来-团队确认单
    /// 创建人：戴银柱
    /// 日期:2011-03-23
    /// </summary>
    public partial class TourConfirmPrint : BackPage
    {
        protected string LeaveDate = "";
        protected string lblBegin = "";
        protected string lblEnd = "";
        protected int listCount = 0;
        protected int tdCount = 0;
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
                //出团日期
                LeaveDate = model.LDate.ToString("yyyy-MM-dd");
                //出发交通
                lblBegin = model.LTraffic;
                //组团社
                this.txtZutuanName.Text = model.BuyerCName;
                //联系信息
                EyouSoft.BLL.CompanyStructure.CompanySupplier supplierBll = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
                EyouSoft.Model.CompanyStructure.CompanySupplier companyModel = supplierBll.GetModel(model.BuyerCId, SiteUserInfo.CompanyID);
                if (companyModel != null)
                {
                    if (companyModel.SupplierContact != null && companyModel.SupplierContact.Count > 0)
                    {
                        this.txtIncomeMan.Text = companyModel.SupplierContact[0].ContactName;
                        this.txtFax.Text = companyModel.SupplierContact[0].ContactFax;
                    }

                }
                //集合时间
                lblGatheredDate.Text = model.GatheringTime;
                //送团人
                if (model.SentPeoples != null && model.SentPeoples.Count > 0)
                {
                    this.txtOffName.Text = model.SentPeoples[0].OperatorName;
                }
                this.txtAddress.Text = model.GatheringPlace;


                //返程交通
                lblEnd = model.RTraffic;
                if (model.TourNormalInfo != null)
                {
                    //不含项目
                    this.lblNoProject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.BuHanXiangMu);
                    //购物安排
                    this.lblBuy.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.GouWuAnPai);
                    //注意事项
                    this.lblNote.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ZhuYiShiXiang);
                    //行程安排

                    if (model.TourNormalInfo.Plans != null)
                    {
                        this.listCount = model.TourNormalInfo.Plans.Count;
                        tdCount = model.TourNormalInfo.Plans.Count;
                        this.rptTravel.DataSource = model.TourNormalInfo.Plans;

                        this.rptTravel.DataBind();
                    }
                }
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
                //团队类型
                this.txtTourType.Text = model.TourType.ToString();


                //包含项目
                this.rptProject.DataSource = model.Services;
                this.rptProject.DataBind();

                #endregion
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

        /// <summary>
        /// 根据出团日期 获得每一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected string GetDateByIndex(int index)
        {
            if (!string.IsNullOrEmpty(LeaveDate))
            {
                DateTime dateTime = Utils.GetDateTime(LeaveDate);
                return dateTime.AddDays(index).ToString("yyyy-MM-dd");
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获得开始 结束 交通
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected string GetTraffic(int index)
        {
            string str = "";
            if (index == 1)
            {
                str = lblBegin;
            }
            if (index == listCount)
            {
                str = lblEnd;
            }
            if (index == 1 && listCount == 1)
            {
                str = lblBegin + "-" + lblEnd;
            }
            return str;
        }
    }
}
