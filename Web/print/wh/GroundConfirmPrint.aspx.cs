using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.print.wh
{
    /// <summary>
    /// 望海- 确认单（地接）
    /// 功能：显示确认单
    /// 创建人：戴银柱
    /// </summary>
    public partial class GroundConfirmPrint : Eyousoft.Common.Page.BackPage
    {
        protected DateTime? LeaveDate = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tourId = Utils.GetQueryStringValue("tourId");
                if (tourId != "")
                {
                    DataInit(tourId);
                    this.txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
            EyouSoft.Model.TourStructure.TourBaseInfo baseinfo = bll.GetTourInfo(tourId);

            //地接的操作BLL
            EyouSoft.BLL.PlanStruture.TravelAgency travelbll = new EyouSoft.BLL.PlanStruture.TravelAgency();
            //获得该计划下的所有地接的集合
            IList<EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo> listAgency = travelbll.GetList(tourId);
            rpt_area.DataSource = listAgency;
            rpt_area.DataBind();

            if (baseinfo != null)
            {
                this.lblTourCode.Text = baseinfo.TourCode;
                // this.lblContactName.Text = baseinfo.
                txtDate1.Text = baseinfo.PlanPeopleNumber.ToString();
                DateTime Ldate = baseinfo.LDate;
                DateTime Edate = Ldate.AddDays(baseinfo.TourDays);
                txtDate0.Text = Ldate.ToString("MM月dd日") + "-" + Edate.ToString("MM月dd日");
                if (baseinfo.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                {
                    EyouSoft.Model.TourStructure.TourTeamInfo model = (EyouSoft.Model.TourStructure.TourTeamInfo)baseinfo;
                    if (model != null && model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                    {
                        if (model.TourNormalInfo != null)
                        {
                            //行程安排
                            this.rptTravel.DataSource = model.TourNormalInfo.Plans;
                            this.rptTravel.DataBind();
                            //内部信息
                            this.txtRemarks.Text = model.TourNormalInfo.NeiBuXingXi;

                        }
                    }
                    else
                    {
                        this.tr_Treval.Visible = false;
                        trXc.Visible = true;
                        if (model.TourQuickInfo != null)
                        { 
                            lt_xc.Text = model.TourQuickInfo.QuickPlan;
                        }
                        
                    }
                    #region 页面控件赋值
                    //包含项目
                    this.rptProject.DataSource = model.Services;
                    this.rptProject.DataBind();
                    if (model.Services != null && model.Services.Count > 0)
                    {
                        string str = "";
                        decimal sum = 0;
                        for (int i = 0; i < model.Services.Count; i++)
                        {
                            if ((i + 1) % 2 == 0)
                            {
                                str += "<tr><td bgcolor=\"#ffffff\" align=\"left\">" + (i + 1) + "、" + model.Services[i].ServiceType.ToString() + "：" + Utils.FilterEndOfTheZeroString(model.Services[i].LocalPrice.ToString("0.00")) + "</td>";
                            }
                            else
                            {
                                str += "<td bgcolor=\"#ffffff\" align=\"left\">" + (i + 1) + "、" + model.Services[i].ServiceType.ToString() + "：" + Utils.FilterEndOfTheZeroString(model.Services[i].LocalPrice.ToString("0.00")) + "</td></tr>";
                                sum += model.Services[i].LocalPrice;
                            }
                        }
                        if (model.Services.Count % 2 != 0)
                        {
                            str += "<td bgcolor=\"#ffffff\" align=\"left\"></td></tr>";
                        }
                        this.litList.Text = str;
                        txt_priceSum.Text = sum.ToString("0.00");
                    }
                    //出团日期
                    LeaveDate = model.LDate;
                    #endregion

                }
                else
                {
                    //散拼计划
                    EyouSoft.Model.TourStructure.TourInfo model = (EyouSoft.Model.TourStructure.TourInfo)baseinfo;
                    //标准版
                    if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                    {
                        this.lblMoney.Text = "";
                        if (model.TourNormalInfo != null)
                        {
                            //行程安排
                            this.rptTravel.DataSource = model.TourNormalInfo.Plans;
                            this.rptTravel.DataBind();
                            //包含项目
                            this.rptProject.DataSource = model.TourNormalInfo.Services;
                            this.rptProject.DataBind();
                            //内部信息
                            this.txtRemarks.Text = model.TourNormalInfo.NeiBuXingXi;
                        }
                    }
                    else
                    {
                        //没有数据 不操作
                        this.lblMsg.Text = "";
                        this.tr_Treval.Visible = false;
                        this.lblMoney.Text = "";
                        trXc.Visible = true;
                        if (model.TourQuickInfo != null)
                        { 
                            lt_xc.Text =model.TourQuickInfo.QuickPlan;
                        }

                    }
                    LeaveDate = model.LDate;

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

        protected void rpt_area_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo item = e.Item.DataItem as EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo;
            EyouSoft.BLL.CompanyStructure.CompanySupplier bll = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            EyouSoft.Model.CompanyStructure.CompanySupplier companyModel = bll.GetModel(item.TravelAgencyID, CurrentUserCompanyID);
            Literal lt_cName = e.Item.FindControl("lt_cName") as Literal;
            Literal lt_cTel = e.Item.FindControl("lt_cTel") as Literal;
            Literal lt_cFax = e.Item.FindControl("lt_cFax") as Literal;
            if (companyModel != null&&companyModel.SupplierContact.Count>0)
            {
                lt_cName.Text = companyModel.SupplierContact[0].ContactName;
                lt_cTel.Text = companyModel.SupplierContact[0].ContactTel;
                lt_cFax.Text = companyModel.SupplierContact[0].ContactFax;
            }
        }
    }
}
