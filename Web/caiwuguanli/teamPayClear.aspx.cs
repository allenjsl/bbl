/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) TravelSky 2011
/// 模块名称：团支出已结清账款
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\Default.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-21 12:04:21
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// ////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EyouSoft.Common;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 团支出已结清账款
    /// </summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011-06-21
    /// 修改内容：金额栏添加金额的合计
    public partial class teamPayClear : Eyousoft.Common.Page.BackPage
    {
        #region 合计参数
        /// <summary>
        /// 单项服务支出金额
        /// </summary>
        protected decimal singleAmount = 0;
        /// <summary>
        /// 单项服务支出已付金额
        /// </summary>
        protected decimal singleHasAmount = 0;
        /// <summary>
        /// 机票支出金额
        /// </summary>
        protected decimal ticketAmount = 0;
        /// <summary>
        /// 机票支出已付金额
        /// </summary>
        protected decimal ticketHasAmount = 0;
        /// <summary>
        /// 地接支出金额
        /// </summary>
        protected decimal travelAgencyAmount = 0;
        /// <summary>
        /// 地接支出已付金额
        /// </summary>
        protected decimal travelAgencyHasAmount = 0;
        #endregion
        /// <summary>
        /// 页面变量
        /// </summary>
        public int i = 0;

        EyouSoft.Model.PlanStructure.MExpendSearchInfo ExpendSearchInfo = new EyouSoft.Model.PlanStructure.MExpendSearchInfo();
        /// <summary>
        /// 是否显示单项合计项
        /// </summary>
        bool IsDanXiang = false;
        /// <summary>
        /// 是否显示地接合计项
        /// </summary>
        bool IsDiJie = false;
        /// <summary>
        /// 是否显示机票合计项
        /// </summary>
        bool IsJiPiao = false;

        /// <summary>
        /// 页面初始绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            #region 团款确认
            string act = Utils.GetQueryStringValue("act");
            if (Utils.GetQueryStringValue("act") != "")
            {
                if (act == "cbsubmit")
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_成本确认))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_成本确认, false);
                    }
                    EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
                    if (bll.SetIsCostConfirm(Utils.GetQueryStringValue("tourid"), true) > 0)
                    {
                        EyouSoft.Common.Function.MessageBox.Show(this.Page, "确认成功!");
                    }
                    else
                    {
                        EyouSoft.Common.Function.MessageBox.Show(this.Page, "操作失败!");
                    }
                }
            }
            #endregion
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_栏目, false);
                }
                BindInfo();
            }
            #region 团款确认
            if (Utils.GetQueryStringValue("act") == "submit")
            {
                string tourid = Utils.GetQueryStringValue("tourid");
                EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
                int k = bll.SetIsCostConfirm(tourid, true);
                if (k > 0)
                {
                    Response.Write("<script>alert('确认成功!');location.href='teamPayClear.aspx';</script>");
                }
            }
            #endregion
        }
        /// <summary>
        /// 绑定信息
        /// </summary>
        void BindInfo()
        {
            int count = 0;
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourSearchTKZCInfo SearchInfo = new EyouSoft.Model.TourStructure.TourSearchTKZCInfo();
            #region 查询参数
            string ddltype = Utils.GetQueryStringValue("type");
            if (select.Items.FindByValue(ddltype) != null)
            {
                select.Items.FindByValue(ddltype).Selected = true;
            }

            string teamNum = Utils.GetQueryStringValue("teamNum");
            txt_teamNum.Value = teamNum;

            string com = Utils.GetQueryStringValue("com");
            txt_com.Value = com;

            string comtype = Utils.GetQueryStringValue("comtype");
            if (ddl_comType.Items.FindByValue(comtype) != null)
            {
                ddl_comType.Items.FindByValue(comtype).Selected = true;
            }
            string goDate = Utils.GetQueryStringValue("goDate");
            txt_godate.Value = goDate;

            string fukDate = Utils.GetQueryStringValue("fukDate");
            txt_payDate.Value = fukDate;
            if (ddltype != "-1" && ddltype != "")
            {
                SearchInfo.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)Utils.GetInt(ddltype);
            }
            if (teamNum != "")
            {
                SearchInfo.TourCode = teamNum;
            }
            if (com != "")
            {
                SearchInfo.SupplierCName = com;
                ExpendSearchInfo.SupplierName = com;            
            }
            if (comtype != "-1" && comtype != "")
            {
                SearchInfo.SupplierCType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Utils.GetInt(comtype);

                if ((EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Utils.GetInt(comtype) == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务)
                {
                    this.pnlGround.Visible = false;
                    ExpendSearchInfo.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务;
                }
                if ((EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Utils.GetInt(comtype) == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接)
                {
                    this.pnlTicket.Visible = false;
                    ExpendSearchInfo.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接;
                }
            }
            if (goDate != "")
            {
                SearchInfo.SDate = Utils.GetDateTime(goDate);
            }
            if (fukDate != "")
            {
                SearchInfo.PaymentSTime = Utils.GetDateTime(fukDate);
            }

            SearchInfo.EDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            SearchInfo.PaymentETime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("regedate"));
            #endregion
            IList<EyouSoft.Model.TourStructure.LBTKZCTourInfo> list = bll.GetToursTKZC(CurrentUserCompanyID, true, 20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, SearchInfo);
            rpt_list1.DataSource = list;
            rpt_list1.DataBind();
            bll.GetToursTKZC(CurrentUserCompanyID, true, SearchInfo, ref travelAgencyAmount, ref travelAgencyHasAmount, ref ticketAmount, ref ticketHasAmount, ref singleAmount, ref singleHasAmount);

            #region 分页
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion
            rpt_list1.EmptyText = "<tr><td id=\"EmptyData\" colspan='5' bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";

            if (!IsDanXiang) phDanXiang.Visible = false;
            if (!IsDiJie) pnlGround.Visible = false;
            if (!IsJiPiao) pnlTicket.Visible = false;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            //BindInfo();
            string ddltype = Utils.GetFormValue(select.UniqueID);
            string teamNum = Utils.GetFormValue(txt_teamNum.UniqueID);
            string com = Utils.GetFormValue(txt_com.UniqueID);
            string comtype = Utils.GetFormValue(ddl_comType.UniqueID);
            string goDate = Utils.GetFormValue(txt_godate.UniqueID);
            string fukDate = Utils.GetFormValue(txt_payDate.UniqueID);
            string leaveEDate = Utils.GetFormValue("txtLeaveEDate");
            string regEDate=Utils.GetFormValue("txtRegEDate");
            Response.Redirect("teamPayClear.aspx?type=" + ddltype + "&teamNum=" + teamNum + "&com=" + com + "&comtype=" + comtype + "&goDate=" + goDate + "&fukdate=" + fukDate + "&ledate=" + leaveEDate + "&regedate=" + regEDate);
        }
        /// <summary>
        /// 多对一处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpt_list1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.Model.TourStructure.LBTKZCTourInfo dv = ((EyouSoft.Model.TourStructure.LBTKZCTourInfo)(e.Item.DataItem));
            if (dv.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
            {
                EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour();
                decimal totalAmount = 0;
                decimal unpaidAmount = 0;
                bll.GetTourExpense(dv.TourId, out totalAmount, out unpaidAmount, ExpendSearchInfo);
                Literal lt_amout = e.Item.FindControl("lt_Amount") as Literal;
                lt_amout.Text = "支出：" + Utils.FilterEndOfTheZeroDecimal(totalAmount) + "<br/>未付：" + Utils.FilterEndOfTheZeroDecimal(unpaidAmount);

                IsDanXiang = true;
            }
            else
            {
                if (dv.TourId != "")
                {
                    EyouSoft.BLL.PlanStruture.TravelAgency bll = new EyouSoft.BLL.PlanStruture.TravelAgency();
                    IList<EyouSoft.Model.PlanStructure.PaymentList> list = bll.GetSettleList(dv.TourId, ExpendSearchInfo);
                    if (list != null)
                    {
                        Repeater rpt = e.Item.FindControl("rpt_project") as Repeater;

                        if (Utils.GetInt(Utils.GetQueryStringValue("comtype")) > 0)
                        {
                            if (Utils.GetInt(Utils.GetQueryStringValue("comtype")) == 1)
                            {
                                rpt.DataSource = list.Where(p => p.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接);
                            }
                            else
                            {
                                rpt.DataSource = list.Where(p => p.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务);
                            }
                        }
                        else
                        {
                            rpt.DataSource = list;
                        }
                        rpt.DataBind();
                    }

                    if (list.FirstOrDefault(i => i.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接) != null)
                    {
                        IsDiJie = true;
                    }
                    if (list.FirstOrDefault(i => i.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务) != null)
                    {
                        IsJiPiao = true;
                    }
                }
            }
        }
        /// <summary>
        /// 获取付款和成本确认URL
        /// </summary>
        /// <param name="IsCostConfirm"></param>
        /// <param name="tourid"></param>
        /// <returns></returns>
        public string getUrl(bool IsCostConfirm, string tourid)
        {
            if (IsCostConfirm)
            {
                return "已确认";
            }
            else
            {
                return "<a href=\"zctuankuan_fukuan.aspx?tourId=" + tourid + "\"><font class=\"fblue\">付款</font></a> <a href=\"teamPayClear.aspx?act=submit&tourId=" + tourid + "\"><font class=\"fblue\">成本确认</font></a> ";
            }
        }
    }
}
