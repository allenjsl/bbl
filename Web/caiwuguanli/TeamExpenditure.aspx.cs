/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) TravelSky 2011
/// 模块名称：支出管理
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\Default.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-19 16:04:21
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
using System.Data;
using EyouSoft.Common;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 支出管理
    /// </summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011-06-21
    /// 修改内容：金额栏添加金额的合计
    public partial class TeamExpenditure : Eyousoft.Common.Page.BackPage
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
        /// 页面变量I
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
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        void BindInfo()
        {
            int count = 0;
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourSearchTKZCInfo SearchInfo = new EyouSoft.Model.TourStructure.TourSearchTKZCInfo();
            #region 查询参数
            string ddltype = Utils.GetQueryStringValue("tourtype");

            string teamNum = Utils.GetQueryStringValue("tourCode");
            txt_teamNum.Value = teamNum;

            string com = Utils.GetQueryStringValue("companyName");
            txt_com.Value = com;

            string comtype = Utils.GetQueryStringValue("comType");

            string goDate = Utils.GetQueryStringValue("beginDate");
            txt_godate.Value = goDate;
            string fukDate = Utils.GetQueryStringValue("payDate");
            txt_payDate.Value = fukDate;
            if (ddltype != "-1" && ddltype != "")
            {
                select.Value = ddltype;

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

                ddl_comType.SelectedValue = comtype;

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
            //if (goDate != "")
            //{
            //    SearchInfo.FDate = Utils.GetDateTimeNullable(goDate);
            //}
            //if (fukDate != "")
            //{
            //    SearchInfo.PaymentFTime = Utils.GetDateTimeNullable(fukDate);
            //}

            SearchInfo.SDate = Utils.GetDateTimeNullable(goDate);
            SearchInfo.EDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("rdate"));

            SearchInfo.PaymentSTime = Utils.GetDateTimeNullable(fukDate);
            SearchInfo.PaymentETime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("payendtime"));
            #endregion
            IList<EyouSoft.Model.TourStructure.LBTKZCTourInfo> list = bll.GetToursTKZC(CurrentUserCompanyID, false, 20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, SearchInfo);
            rpt_list1.DataSource = list;
            rpt_list1.DataBind();
            //合计
            bll.GetToursTKZC(CurrentUserCompanyID, false, SearchInfo, ref travelAgencyAmount, ref travelAgencyHasAmount, ref ticketAmount, ref ticketHasAmount, ref singleAmount, ref singleHasAmount);
            this.rpt_list1.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='5' align='center'>暂时没有数据！</td></tr>";

            #region 分页
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion

            if (!IsDanXiang) phDanXiang.Visible = false;
            if (!IsDiJie) pnlGround.Visible = false;
            if (!IsJiPiao) pnlTicket.Visible = false;
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            BindInfo();
        }

        public string getFukuanUrl(int type)
        {
            if ((EyouSoft.Model.EnumType.TourStructure.TourType)type == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
            {
                return "single_fukuan.aspx";
            }
            else
            {
                return "zctuankuan_fukuan.aspx";
            }
        }
        /// <summary>
        /// 列表单条记录统计处理
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
                bll.GetTourExpense(dv.TourId, out totalAmount, out unpaidAmount,ExpendSearchInfo);
                Literal lt_amout = e.Item.FindControl("lt_Amount") as Literal;
                lt_amout.Text = "(单项)支出：" + Utils.FilterEndOfTheZeroDecimal(totalAmount) + "<br/>(单项)未付：" + Utils.FilterEndOfTheZeroDecimal(unpaidAmount);

                IsDanXiang = true;
            }
            else
            {
                if (dv.TourId != "")
                {
                    EyouSoft.BLL.PlanStruture.TravelAgency bll = new EyouSoft.BLL.PlanStruture.TravelAgency();
                    IList<EyouSoft.Model.PlanStructure.PaymentList> list = bll.GetSettleList(dv.TourId,ExpendSearchInfo);
                    Repeater rpt = e.Item.FindControl("rpt_project") as Repeater;
                    //总数计算

                    if (Utils.GetInt(Utils.GetQueryStringValue("comType")) > 0)
                    {
                        if (Utils.GetInt(Utils.GetQueryStringValue("comType")) == 1)
                        {
                            var v = from x in list
                                    where x.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接
                                    group x by x.SupplierType into g
                                    select new
                                    {
                                        PayedAmount = g.Sum(x => x.PayedAmount),
                                        TotalAmount = g.Sum(x => x.TotalAmount),
                                        SupplierType = g.Key
                                    };
                            rpt.DataSource = v;
                            rpt.DataBind();
                        }
                        else
                        {
                            var v = from x in list
                                    where x.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务
                                    group x by x.SupplierType into g
                                    select new
                                    {
                                        PayedAmount = g.Sum(x => x.PayedAmount),
                                        TotalAmount = g.Sum(x => x.TotalAmount),
                                        SupplierType = g.Key
                                    };
                            rpt.DataSource = v;
                            rpt.DataBind();
                        }
                    }
                    else
                    {
                        var v = from x in list
                                group x by x.SupplierType into g
                                select new
                                {
                                    PayedAmount = g.Sum(x => x.PayedAmount),
                                    TotalAmount = g.Sum(x => x.TotalAmount),
                                    SupplierType = g.Key
                                };
                        rpt.DataSource = v;
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
    }
}
