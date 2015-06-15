/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) TravelSky 2011
/// 模块名称：团收入已结清账款
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\Default.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-19 15:04:21
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
using System.Text;
namespace Web.caiwuguanli
{
    /// <summary>
    /// 团款收入已结清账款
    /// </summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011-06-21
    /// 修改内容：金额栏添加金额的合计
    /// 修改：邵权江 2012-1-6 添加导出
    public partial class TeamClear : Eyousoft.Common.Page.BackPage
    {
        #region 统计参数
        //总人数
        protected int peopleNumber = 0;
        //应收账款
        protected decimal financeSum = 0;
        //已收账款
        protected decimal hasCheckMoney = 0;
        //未审核账款
        protected decimal notCheckMoney = 0;
        protected int count = 0;//
        #endregion
        /// <summary>
        /// 全局行号
        /// </summary>
        public int i = 0;
        /// <summary>
        /// 全局行号
        /// </summary>
        public int j = 0;
        /// <summary>
        /// 页面初始绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInfo();
            }
        }
        /// <summary>
        /// 列表绑定
        /// </summary>
        void BindInfo()
        {
            #region 搜索条件
            string teamType = Utils.GetQueryStringValue("teamtype");
            string order = Utils.GetQueryStringValue("order");
            string teamNum = Utils.GetQueryStringValue("teamNum");
            string sourse = Utils.GetQueryStringValue("com");
            string goDate = Utils.GetQueryStringValue("begin");
            string addDate = Utils.GetQueryStringValue("addDate");
            string OperatorId = Utils.GetQueryStringValue("OperatorId");//

            txt_addDate.Value = addDate;
            txt_goDate.Value = goDate;
            txt_Source.Value = sourse;
            txt_teamNum.Value = teamNum;
            txt_order.Value = order;
            if (sl_teamType.Items.FindByValue(teamType) != null)
            {
                sl_teamType.Items.FindByValue(teamType).Selected = true;
            }



            //int count = 0;
            EyouSoft.BLL.TourStructure.TourOrder bll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.TourStructure.OrderAboutAccountSearchInfo SearchInfo = new EyouSoft.Model.TourStructure.OrderAboutAccountSearchInfo();
            SearchInfo.CompanyId = CurrentUserCompanyID;
            SearchInfo.CompanyName = txt_Source.Value;
            //if (addDate != "")
            //{
            //    SearchInfo.CreateDateFrom = Utils.GetDateTime(addDate);
            //}
            //if (goDate != "")
            //{
            //    SearchInfo.LeaveDateFrom = Utils.GetDateTime(goDate);
            //}
            SearchInfo.LeaveDateFrom = Utils.GetDateTimeNullable(goDate);
            SearchInfo.LeaveDateTo = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("rdate"));
            SearchInfo.CreateDateFrom = Utils.GetDateTimeNullable(addDate);
            SearchInfo.CreateDateTo = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("regenddate"));
            selectOperator1.OperName = Utils.GetQueryStringValue("OperatorName");//
            selectOperator1.OperId = OperatorId;//
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
            SearchInfo.TourNo = teamNum;
            SearchInfo.OrderNo = order;

            SearchInfo.RegisterStatus = Utils.GetIntNull(Utils.GetQueryStringValue("status"));

            if (seleStatus.Items.FindByValue(SearchInfo.RegisterStatus.ToString()) != null)
            {
                seleStatus.Items.FindByValue(SearchInfo.RegisterStatus.ToString()).Selected = true;
            }

            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? ComputeOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(CurrentUserCompanyID);
            if (ComputeOrderType.HasValue)
                SearchInfo.ComputeOrderType = ComputeOrderType.Value;

            SearchInfo.SortType = Utils.GetInt(Utils.GetQueryStringValue("sorttype"));
            SearchInfo.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), teamType, null);
            #endregion

            IList<EyouSoft.Model.TourStructure.TourAboutAccountInfo> list = null;
            #region 导出客户Excel
            //导出Excel
            if (Utils.GetQueryStringValue("method") == "downexcel")
            {
                string strPageSize = Utils.GetQueryStringValue("recordcount");
                int pageSize = Utils.GetInt(strPageSize == "0" ? "1" : strPageSize);//导出游客时获取全部的查询数据
                list = bll.GetTourHasReciveAccountList(pageSize, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, SearchInfo);
                DownLoadExcel(list);
                return;
            }
            #endregion

            list = bll.GetTourHasReciveAccountList(20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, SearchInfo);
            bll.GetTourHasReciveAccountList(SearchInfo, ref peopleNumber, ref financeSum, ref hasCheckMoney, ref notCheckMoney);
            rpt_list1.DataSource = list;
            rpt_list1.DataBind();
            #region 分页
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion
        }
        /// <summary>
        /// 多对一处理
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
            //Literal lt4 = e.Item.FindControl("Literal4") as Literal;
            //lt4.Text = "rowspan='" + model.OrderAccountList.Count.ToString() + "'";
            Literal ltrTourType = e.Item.FindControl("ltrTourType") as Literal;
            string tourType = "(团)";
            switch (model.TourType)
            {
                case EyouSoft.Model.EnumType.TourStructure.TourType.单项服务: tourType = "(单)"; break;
                case EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划: tourType = "(散)"; break;
            }
            ltrTourType.Text = tourType;

        }
        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            //BindInfo();
            string param = "";
            string teamType = Utils.GetFormValue(sl_teamType.UniqueID);
            string order = Utils.GetFormValue(txt_order.UniqueID);
            string teamNum = txt_teamNum.Value;
            string com = txt_Source.Value;
            string begin = txt_goDate.Value;
            string adddate = txt_addDate.Value;
            string OperatorId = selectOperator1.OperId;//
            string OperatorName = selectOperator1.OperName;//
            string rdate = Utils.GetFormValue("txtRDate");
            string regenddate = Utils.GetFormValue("txtRegEDate");
            param = "teamtype=" + teamType + "&order=" + order + "&teamNum=" + teamNum + "&com=" + com + "&begin=" + begin + "&adddate=" + adddate + "&OperatorId=" + OperatorId + "&OperatorName=" + OperatorName + "&status=" + Utils.GetFormValue(seleStatus.UniqueID) + "&rdate=" + rdate + "&regenddate=" + regenddate;
            Response.Redirect("teamclear.aspx?" + param);
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
            strBuilder.Append("团号\t线路名称\t出团日期\t客源单位\t人数\t销售员\t对方操作员\t对方团号\t应收款\t已收款\n");
            foreach (EyouSoft.Model.TourStructure.TourAboutAccountInfo t in list)
            {
                //foreach(EyouSoft.Model.TourStructure.OrderAboutAccount o in t.OrderAccountList)
                //{
                //    PepoleNum = PepoleNum.Trim() == "" ? o.PepoleNum.ToString() : (PepoleNum + "；" + o.PepoleNum.ToString());
                //    SalerName = SalerName.Trim() == "" ? o.SalerName : (SalerName + "；" + o.SalerName);
                //    FinanceSum = FinanceSum.Trim() == "" ? o.FinanceSum.ToString("￥0.00") : (FinanceSum + "；" + o.FinanceSum.ToString("￥0.00"));
                //    HasCheckMoney = HasCheckMoney.Trim() == "" ? o.HasCheckMoney.ToString("￥0.00") : (HasCheckMoney + "；" + o.HasCheckMoney.ToString("￥0.00"));
                //    NotReciveMoney = NotReciveMoney.Trim() == "" ? o.NotReciveMoney.ToString("￥0.00") : (NotReciveMoney + "；" + o.NotReciveMoney.ToString("￥0.00"));
                //    NotCheckMoney = NotCheckMoney.Trim() == "" ? o.NotCheckMoney.ToString("￥0.00") : (NotCheckMoney + "；" + o.NotCheckMoney.ToString("￥0.00"));
                //    CompanyName = CompanyName.Trim() == "" ? o.CompanyName : (CompanyName + "；" + o.CompanyName);
                //}
                //strBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\n", t.TourNo, t.RouteName, t.LeaveDate, t.TourStatus, PepoleNum, SalerName, FinanceSum, HasCheckMoney, NotReciveMoney, NotCheckMoney, CompanyName);
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
                    strBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\n", t.TourNo + tourType
                        , ((EyouSoft.Model.EnumType.TourStructure.TourType)t.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 ? "单项服务" : t.RouteName)
                        , ((EyouSoft.Model.EnumType.TourStructure.TourType)t.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 ? "" : t.LeaveDate.ToString("yyyy-MM-dd"))
                        , o.CompanyName
                        , o.PepoleNum.ToString()
                        , o.SalerName
                        , o.BuyerContactName
                        , o.BuyerTourCode
                        , o.FinanceSum.ToString("￥0.00")
                        , o.HasCheckMoney.ToString("￥0.00"));
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
