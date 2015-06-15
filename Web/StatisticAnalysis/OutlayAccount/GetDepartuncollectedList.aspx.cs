using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Model.PlanStructure;
using System.IO;

namespace Web.StatisticAnalysis.OutlayAccount
{
    /// <summary>
    /// 页面功能：支出对账单--按部门统计--统计图上的未收查看
    /// Author:liuym
    /// Date:2011-1-20
    /// </summary>
    public partial class GetDepartuncollectedList : BackPage
    {
        /// <summary>
        /// OnPreInit事件 设置模式窗体属性
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        #region PrivateMembers
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected string TourType = string.Empty;//团队类型
        protected string RouteName = string.Empty;//线路区域名称
        protected string OperatorId = string.Empty;//操作员ID
        protected DateTime? StartDate = null;//出团开始日期
        protected DateTime? EndDate = null;//出团结束日期
        IList<ArrearInfo> list = null;//未收集合
        protected string sup = string.Empty; //供应商
        protected decimal sumMoney;         //总支出
        protected decimal arrear;           //未付

        #endregion

        #region 初始化加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                #region 根据URL参数初始化PageSize
                if (Utils.GetInt(Request.QueryString["isAll"], 0) == 1
                    || Utils.GetInt(Request.QueryString["isExport"], 0) == 1
                    || Utils.GetInt(Request.QueryString["IsCartogram"], 0) == 1)
                {
                    PageSize = int.MaxValue - 1; //-1为了避免存储过程报错
                }
                #endregion
                IntiUnCollectCashList();

                #region 导出报表请求
                if (Utils.GetInt(Request.QueryString["isExport"], 0) == 1)
                {
                    #region 获取表单值
                    PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
                    TourType = Utils.GetQueryStringValue("TourType");
                    StartDate = Utils.GetDateTimeNullable(Request.QueryString["StartDate"], new DateTime(DateTime.Now.Year, 1, 1));
                    EndDate = Utils.GetDateTimeNullable(Request.QueryString["EndDate"], new DateTime(DateTime.Now.Year, 12, 31));
                    TourType = Utils.GetQueryStringValue("TourType");
                    RouteName = Utils.GetQueryStringValue("RouteName");
                    OperatorId = Utils.GetQueryStringValue("OperatorId");
                    sup = Utils.GetQueryStringValue("CompanyName");
                    #endregion

                    #region 实体赋值
                    EyouSoft.BLL.PlanStruture.TravelAgency TravelAgencyBLL = new EyouSoft.BLL.PlanStruture.TravelAgency(SiteUserInfo);
                    EyouSoft.Model.PlanStructure.ArrearSearchInfo SearchModel = new EyouSoft.Model.PlanStructure.ArrearSearchInfo();

                    if (TourType != "-1" && TourType != "")
                    {
                        SearchModel.TourType = int.Parse(TourType);
                    }
                    if (RouteName != "0" && RouteName != "")
                    {
                        SearchModel.AreaId = int.Parse(RouteName);
                    }
                    SearchModel.LeaveDate1 = StartDate;
                    SearchModel.LeaveDate2 = EndDate;
                    SearchModel.CompanyId = this.SiteUserInfo.CompanyID;
                    SearchModel.SeachType = Utils.GetInt(Utils.GetQueryStringValue("searchType"), 3);
                    SearchModel.SupplierName = sup;
                    if (OperatorId != "" && OperatorId != "0")
                    {
                        SearchModel.OperateId = int.Parse(OperatorId);
                    }
                    list = EyouSoft.Common.Function.SelfExportPage.GetList<ArrearInfo>(1, 9999, out RecordCount, TravelAgencyBLL.GetArrear(SearchModel));
                    TravelAgencyBLL.GetArrearAllMoeny(SearchModel, ref sumMoney, ref arrear);
                    #endregion

                    if (list != null && list.Count != 0)
                    {
                        ToExcel(this.crp_GetDepartUncollList, list);
                    }
                }
                    #endregion
            
                }
        }
                #endregion

        #region 导出Excel
        private void ToExcel(ControlLibrary.CustomRepeater ctl, IList<ArrearInfo> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=IncAreaStaticFile.xls");

            HttpContext.Current.Response.Charset = "UTF-8";

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;

            HttpContext.Current.Response.ContentType = "application/ms-excel";

            ctl.Page.EnableViewState = false;
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            ctl.RenderControl(hw);
            ctl.Visible = false;
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 设置分页
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intRecordCount = RecordCount;
        }
        #endregion

        #region 初始化未收列表
        private void IntiUnCollectCashList()
        {
            #region 获取表单值
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
            TourType = Utils.GetQueryStringValue("TourType");
            StartDate = Utils.GetDateTimeNullable(Request.QueryString["StartDate"], new DateTime(DateTime.Now.Year, 1, 1));
            EndDate = Utils.GetDateTimeNullable(Request.QueryString["EndDate"], new DateTime(DateTime.Now.Year, 12, 31));
            TourType = Utils.GetQueryStringValue("TourType");
            RouteName = Utils.GetQueryStringValue("RouteName");
            OperatorId = Utils.GetQueryStringValue("OperatorId");
            sup = Utils.GetQueryStringValue("CompanyName");
            #endregion

            #region 实体赋值
            EyouSoft.BLL.PlanStruture.TravelAgency TravelAgencyBLL = new EyouSoft.BLL.PlanStruture.TravelAgency(SiteUserInfo);
            EyouSoft.Model.PlanStructure.ArrearSearchInfo SearchModel = new EyouSoft.Model.PlanStructure.ArrearSearchInfo();

            if (TourType != "-1" && TourType != "")
            {
                SearchModel.TourType = int.Parse(TourType);
            }
            if (RouteName != "0" && RouteName != "")
            {
                SearchModel.AreaId = int.Parse(RouteName);
            }
            SearchModel.LeaveDate1 = StartDate;
            SearchModel.LeaveDate2 = EndDate;
            SearchModel.CompanyId = this.SiteUserInfo.CompanyID;
            SearchModel.SeachType = Utils.GetInt(Utils.GetQueryStringValue("searchType"), 3);
            SearchModel.SupplierName = sup;
            if (OperatorId != "" && OperatorId != "0")
            {
                SearchModel.OperateId = int.Parse(OperatorId);
            }
            list = EyouSoft.Common.Function.SelfExportPage.GetList<ArrearInfo>(PageIndex, PageSize, out RecordCount, TravelAgencyBLL.GetArrear(SearchModel));


            TravelAgencyBLL.GetArrearAllMoeny(SearchModel, ref sumMoney, ref arrear);
            #endregion
            //调用底层方法

            if (list != null && list.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_GetDepartUncollList.DataSource = list;
                this.crp_GetDepartUncollList.DataBind();
                BindPage();
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_GetDepartUncollList.EmptyText = "<tr class='odd' align='center'><td colspan='8' height='40px' class='even'>暂时没有数据！</td></tr>";
            }
        }
        #endregion

        #region 线路名称和出团日期显示
        /// <summary>
        /// 线路名称和出团日期显示
        /// </summary>
        /// <param name="type">0:线路名称显示与否，1：出团日期显示与否</param>
        /// <param name="orderType">下单类型</param>
        /// <param name="routeName">如果是单项服务，线路名称为空</param>
        /// <returns></returns>
        public string GetRouteName(string type, string orderType, string routeName, string tourDate)
        {
            string ReturnValue = string.Empty;
            if (type == "0")
            {//线路名称显示
                if (!string.IsNullOrEmpty(orderType))
                {
                    if (orderType == EyouSoft.Model.EnumType.TourStructure.OrderType.单项服务.ToString())
                        ReturnValue = orderType;//"单项服务"
                    else
                        ReturnValue = routeName;
                }
            }
            else
            {//出团日期
                if (!string.IsNullOrEmpty(orderType))
                {
                    if (orderType == EyouSoft.Model.EnumType.TourStructure.OrderType.单项服务.ToString())
                        ReturnValue = "";
                    else
                        ReturnValue = tourDate;
                }
            }
            return ReturnValue;
        }
        #endregion
    }
}
