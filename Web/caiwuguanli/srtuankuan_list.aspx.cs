/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) TravelSky 2011
/// 模块名称：团款
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\Default.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-19 11:04:21
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
using System.Text;
namespace Web.caiwuguanli
{
    /// 修改人：柴逸宁
    /// 修改时间：2011-06-21
    /// 修改内容：金额栏添加金额的合计
    /// 修改人：田想兵
    /// 修改时间：2011-08-09
    /// 修改内容：添加变更显示
    /// 修改：邵权江 2012-1-5 添加导出
    public partial class srtuankuan_list : Eyousoft.Common.Page.BackPage
    {
        public int i = 0;
        public int j = 0;
        protected int count = 0;
        #region 统计参数
        //总人数
        protected int peopleNumber = 0;
        //应收账款
        protected decimal financeSum = 0;
        //已收账款
        protected decimal hasCheckMoney = 0;
        //未审核账款
        protected decimal notCheckMoney = 0;
        //未审核退款
        protected decimal NotCheckTuiMoney = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款收入_栏目, false);
                }
                BindInfo();
            }
        }
        /// <summary>
        /// 绑定信息
        /// </summary>
        void BindInfo()
        {

            //int count = 0;
            #region 查询条件
            string tourCode = Utils.GetQueryStringValue("tourCode");
            string orderCode = Utils.GetQueryStringValue("orderCode");
            string companyName = Utils.GetQueryStringValue("companyName");
            string OperatorId = Utils.GetQueryStringValue("OperatorId");
            DateTime? begindate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("beginDate"));
            DateTime? endDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endDate"));
            DateTime? addDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("addDate"));



            EyouSoft.BLL.TourStructure.TourOrder bll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.TourStructure.OrderAboutAccountSearchInfo SearchInfo = new EyouSoft.Model.TourStructure.OrderAboutAccountSearchInfo();
            SearchInfo.CompanyId = CurrentUserCompanyID;
            SearchInfo.CompanyName = companyName;
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                SearchInfo.ComputeOrderType = computerOrderType.Value;
            }
            txt_com.Value = companyName;
            if (addDate != null)
                txt_addDate.Value = addDate.Value.ToString("yyyy-MM-dd");
            SearchInfo.CreateDateFrom = addDate;
            if (begindate != null)
                txt_goDate.Value = begindate.Value.ToString("yyyy-MM-dd");

            SearchInfo.LeaveDateFrom = begindate;
            if (endDate != null)
                txt_endDate.Value = endDate.Value.ToString("yyyy-MM-dd");
            SearchInfo.LeaveDateTo = endDate;
            txt_teamNum.Value = tourCode;
            SearchInfo.TourNo = tourCode;
            txt_order.Value = orderCode;
            SearchInfo.OrderNo = orderCode;
            selectOperator1.OperId = OperatorId;//
            selectOperator1.OperName = Utils.GetQueryStringValue("OperatorName");
            string[] stroprate = OperatorId.Split(',');
            if (OperatorId != null && OperatorId != "")
            {
                int[] intoprate = new int[stroprate.Length];
                for (int k = 0; k < stroprate.Length; k++)
                {
                    intoprate[k] = Utils.GetInt(stroprate[k]);
                }
                SearchInfo.SalerId = intoprate;
            }

            SearchInfo.RegisterStatus = Utils.GetIntNull(Utils.GetQueryStringValue("status"));

            if (seleStatus.Items.FindByValue(SearchInfo.RegisterStatus.ToString()) != null)
            {
                seleStatus.Items.FindByValue(SearchInfo.RegisterStatus.ToString()).Selected = true;
            }

            SearchInfo.QueryAmountType = (EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType)Utils.GetInt(Utils.GetQueryStringValue("queryAmountType"));
            SearchInfo.QueryAmountOperator = (EyouSoft.Model.EnumType.FinanceStructure.QueryOperator)Utils.GetInt(Utils.GetQueryStringValue("queryAmountOperator"));
            SearchInfo.QueryAmount = Utils.GetDecimal(Utils.GetQueryStringValue("queryAmount"));
            SearchInfo.CreateDateTo = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("regenddate"));

            SearchInfo.SortType = Utils.GetInt(Utils.GetQueryStringValue("sorttype"));
            int queryTourType = Utils.GetInt(Utils.GetQueryStringValue("queryTourType"), -1);
            if (queryTourType != -1 && Enum.IsDefined(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), queryTourType))
            {
                SearchInfo.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)queryTourType;
            }
            #endregion

            IList<EyouSoft.Model.TourStructure.TourAboutAccountInfo> list = null;
            #region 导出客户Excel
            //导出Excel
            if (Utils.GetQueryStringValue("method") == "downexcel")
            {
                string strPageSize = Utils.GetQueryStringValue("recordcount");
                int pageSize = Utils.GetInt(strPageSize == "0" ? "1" : strPageSize);//导出游客时获取全部的查询数据
                list = bll.GetTourReciveAccountList(pageSize, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, SearchInfo);
                DownLoadExcel(list);
                return;
            }
            #endregion

            list = bll.GetTourReciveAccountList(20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, SearchInfo);
            bll.GetTourReciveAccountList(SearchInfo, ref peopleNumber, ref financeSum, ref hasCheckMoney, ref notCheckMoney, ref NotCheckTuiMoney);
            rpt_list1.DataSource = list;
            rpt_list1.DataBind();
            this.rpt_list1.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='13' align='center'>暂时没有数据！</td></tr>";
            #region 分页
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            BindInfo();
        }
        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpt_list1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.Model.TourStructure.TourAboutAccountInfo model = e.Item.DataItem as EyouSoft.Model.TourStructure.TourAboutAccountInfo;
            Repeater rpt_sList = e.Item.FindControl("rpt_sList") as Repeater;
            rpt_sList.DataSource = model.OrderAccountList;
            rpt_sList.DataBind();
            Literal lt1 = e.Item.FindControl("Literal1") as Literal;
            lt1.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            Literal lt2 = e.Item.FindControl("Literal2") as Literal;
            lt2.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            Literal lt3 = e.Item.FindControl("Literal3") as Literal;
            lt3.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";

            Literal ltrTourType = e.Item.FindControl("ltrTourType") as Literal;
            string tourType="(团)";
            switch (model.TourType)
            {
                case EyouSoft.Model.EnumType.TourStructure.TourType.单项服务: tourType = "(单)"; break;
                case EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划: tourType = "(散)"; break;
            }
            ltrTourType.Text = tourType;


            //Literal lt4 = e.Item.FindControl("Literal4") as Literal;
            //lt4.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            //for (int i = 0; i < rpt_sList.Items.Count;i++ )
            //{
            //    Literal lt1 = rpt_sList.Items[i].FindControl("Literal1") as Literal;
            //    lt1.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            //    Literal lt2 = rpt_sList.Items[i].FindControl("Literal2") as Literal;
            //    lt2.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            //    Literal lt3 = rpt_sList.Items[i].FindControl("Literal3") as Literal;
            //    lt3.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            //    Literal lt4 = rpt_sList.Items[i].FindControl("Literal4") as Literal;
            //    lt4.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            //    Literal lt5 = rpt_sList.Items[i].FindControl("Literal5") as Literal;
            //    lt5.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            //    Literal lt6 = rpt_sList.Items[i].FindControl("Literal6") as Literal;
            //    lt6.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            //    Literal lt7 = rpt_sList.Items[i].FindControl("Literal7") as Literal;
            //    lt7.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            //    Literal lt8 = rpt_sList.Items[i].FindControl("Literal8") as Literal;
            //    lt8.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            //}

        }
        /// <summary>
        /// 是否是单项服务
        /// </summary>
        /// <param name="tourtype"></param>
        /// <returns></returns>
        public bool isSingle(object tourtype)
        {
            EyouSoft.Model.EnumType.TourStructure.TourType tourType = (EyouSoft.Model.EnumType.TourStructure.TourType)tourtype;
            if (tourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
            {
                return true;
            }
            else
                return false;

        }
        /// <summary>
        /// 增减费用存在变更
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <param name="toap">增减实体</param>
        /// <returns></returns>
        public string isChange(string id, EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo toap)
        {
            if (toap != null)
            {
                if (toap.AddAmount != 0 || toap.ReduceAmount != 0)
                {
                    return "<a href='PlusSubchange.aspx?add=" + toap.AddAmount.ToString() + "&sub=" + toap.ReduceAmount.ToString() + "&remark=" + Uri.EscapeUriString(toap.Remark)+ "' class='change'>变更</a>";
                }
            }
            return "";
        }

        #region 导出客户资料方法
        /// <summary>
        /// 导出客户列表
        /// </summary>
        /// <param name="list"></param>
        protected void DownLoadExcel(IList<EyouSoft.Model.TourStructure.TourAboutAccountInfo> list)
        {
            string PepoleNum = "";
            string SalerName = "";
            string FinanceSum = "";
            string HasCheckMoney = "";
            string NotReciveMoney = "";
            string NotCheckMoney = "";
            string CompanyName = "";
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("团号\t线路名称\t出团日期\t客源单位\t人数\t销售员\t对方操作人\t对方团号\t应收款\t已收款\t未收款\t待审核\t退款待审\n");
            foreach (EyouSoft.Model.TourStructure.TourAboutAccountInfo t in list)
            {
                string tourType = "(团)";
                switch (t.TourType)
                {
                    case EyouSoft.Model.EnumType.TourStructure.TourType.单项服务: tourType = "(单)"; break;
                    case EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划: tourType = "(散)"; break;
                }

                foreach (EyouSoft.Model.TourStructure.OrderAboutAccount o in t.OrderAccountList)
                {
                    PepoleNum = PepoleNum.Trim() == "" ? o.PepoleNum.ToString() : (PepoleNum + "；" + o.PepoleNum.ToString());
                    SalerName = SalerName.Trim() == "" ? o.SalerName : (SalerName + "；" + o.SalerName);
                    FinanceSum = FinanceSum.Trim() == "" ? o.FinanceSum.ToString("￥0.00") : (FinanceSum + "；" + o.FinanceSum.ToString("￥0.00"));
                    HasCheckMoney = HasCheckMoney.Trim() == "" ? o.HasCheckMoney.ToString("￥0.00") : (HasCheckMoney + "；" + o.HasCheckMoney.ToString("￥0.00"));
                    NotReciveMoney = NotReciveMoney.Trim() == "" ? o.NotReciveMoney.ToString("￥0.00") : (NotReciveMoney + "；" + o.NotReciveMoney.ToString("￥0.00"));
                    NotCheckMoney = NotCheckMoney.Trim() == "" ? o.NotCheckMoney.ToString("￥0.00") : (NotCheckMoney + "；" + o.NotCheckMoney.ToString("￥0.00"));
                    CompanyName = CompanyName.Trim() == "" ? o.CompanyName : (CompanyName + "；" + o.CompanyName);
                    strBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\n"
                        , t.TourNo + tourType
                        , ((EyouSoft.Model.EnumType.TourStructure.TourType)t.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 ? "单项服务" : t.RouteName)
                        , (isSingle(t.TourType) ? "" : t.LeaveDate.ToString("yyyy-MM-dd"))
                        , o.CompanyName
                        , o.PepoleNum.ToString()
                        , o.SalerName
                        , o.BuyerContactName
                        , o.BuyerTourCode
                        , o.FinanceSum.ToString("C2")
                        , o.HasCheckMoney.ToString("C2")
                        , o.NotReciveMoney.ToString("C2")
                        , o.NotCheckMoney.ToString("C2")
                        , o.NotCheckTuiMoney.ToString("C2"));
                }
            }
            Response.Clear();
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Write(strBuilder.ToString());
            Response.End();
        }
        #endregion
    }
}
