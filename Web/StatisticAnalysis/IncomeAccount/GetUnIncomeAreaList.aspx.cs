using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using System.Text;
using System.IO;

namespace Web.StatisticAnalysis.IncomeAccount
{
    /// <summary>
    /// 页面功能：收入对账单--未收入统计
    /// Author:liuym
    /// Date:2011-1-19
    /// </summary>
    public partial class GetUnIncomeAreaList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected decimal sumMoneny; //总计
        protected decimal hasGetMoney; //已收
        IList<EyouSoft.Model.TourStructure.TourOrder> list = null;
        #endregion
        /// <summary>
        /// OnPreInit事件 设置模式窗体属性
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;


        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitUnIncomeList();
                string type = Utils.GetQueryStringValue("act");
                if (type == "toexcel")
                {
                    CreateExcel(crp_UnIncomeAreaList);
                }

            }

        }

        #region 初始化未收入列表
        private void InitUnIncomeList()
        {
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBLL = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
            LeaveTourStartDate.Value = Utils.GetQueryStringValue("LeaveTourStarDate");
            LeaveTourEndDate.Value = Utils.GetQueryStringValue("LeaveTourEndDate");
            TourType.Value = Utils.GetQueryStringValue("TourType");
            RouteArea.Value = Utils.GetQueryStringValue("RouteArea");
            SalserId.Value = Utils.GetQueryStringValue("SalserId");
            Company.Value = Utils.GetQueryStringValue("Company");
            EyouSoft.Model.TourStructure.SearchInfo SearchModel = new EyouSoft.Model.TourStructure.SearchInfo();
            SearchModel = RefSearchInfo();
            list = TourOrderBLL.GetOrderList(PageSize, PageIndex, ref RecordCount, SearchModel);
            if (list != null && list.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_UnIncomeAreaList.DataSource = list;
                TourOrderBLL.GetFinanceSumByOrder(SearchModel, ref sumMoneny, ref hasGetMoney);
                this.crp_UnIncomeAreaList.DataBind();
                BindPage();
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_UnIncomeAreaList.EmptyText = "<tr bgcolor=\"#e3f1fc\"><td colspan='9' height='50px' align='center'>暂时没有数据！</td></tr>";
            }
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

        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(ControlLibrary.CustomRepeater ctl)
        {
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBLL = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.TourStructure.SearchInfo searchInfo = new EyouSoft.Model.TourStructure.SearchInfo();
            searchInfo = RefSearchInfo();
            int recordCount = 0;
            //用gerList方法取得总记录的条数
            list = TourOrderBLL.GetOrderList(1, 1, ref recordCount, searchInfo);
            list = TourOrderBLL.GetOrderList(recordCount, 1, ref recordCount, searchInfo);
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=UnInComeArea" + DateTime.Now.ToShortDateString() + ".xls");

            HttpContext.Current.Response.Charset = "UTF-8";

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;

            HttpContext.Current.Response.ContentType = "application/ms-excel";
            string table = "<table width='780' border='1'  align='center' cellpadding='0' cellspacing='1' style='margin: 10px;'><tr><td>线路名称</td><td>团号</td><td>出团日期</td><td>客户单位</td><td>联系电话</td><td>人数</td><td>责任销售</td><td>总收入</td><td>已收</td><td>未收</td></tr>";
            foreach (EyouSoft.Model.TourStructure.TourOrder info in list)
            {
                table += "<tr>";
                table += "<td>" + info.RouteName + "</td><td>" + info.TourNo + "&nbsp;</td><td>" + info.LeaveDate + "</td><td>" + info.BuyCompanyName + "</td><td>" + info.ContactTel + "</td><td>" + info.PeopleNumber + "</td><td>" + info.SalerName + "</td><td>" + info.SumPrice + "</td><td>" + (info.SumPrice - info.NotReceived) + "</td><td>" + info.NotReceived + "</td>";
                table += "</tr>";
            }
            //未收金额
            decimal notRev=sumMoneny-hasGetMoney;
            table += "<tr><td>总计:</td><td colspan='3'>总收入:"+EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(sumMoneny)+"</td><td colspan='3'>已收:"+EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(hasGetMoney)+"</td><td colspan='3'>未收:"+EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(notRev)+"</td></tr></table>";
            HttpContext.Current.Response.Write(table);
            HttpContext.Current.Response.End();
            //if (recordCount > 0)
            //{   //用上面list取得的记录条数作为每页显示条数的参数来取得相应的数据
            //    list = TourOrderBLL.GetOrderList(recordCount, 1, ref recordCount, searchInfo);
            //}
            //Response.Clear();
            //Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            //Response.ContentEncoding = System.Text.Encoding.Default;
            //Response.ContentType = "application/ms-excel";

            ////取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\n", "线路名称", "团号", "出团日期", "客户单位", "人数", "责任销售", "总收入", "已收", "未收");
            //foreach (EyouSoft.Model.TourStructure.TourOrder info in list)
            //{
            //    sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\n",
            //        info.RouteName,
            //        info.TourNo,
            //        info.LeaveDate,
            //        info.BuyCompanyName,
            //        info.PeopleNumber,
            //        info.SalerName,
            //        info.SumPrice,
            //        info.SumPrice - info.NotReceived,
            //        info.NotReceived);
            //}
            //Response.Write(sb.ToString());
            //Response.End();
        }
        //取得querystring值并赋值给searchinfo
        private EyouSoft.Model.TourStructure.SearchInfo RefSearchInfo()
        {
            EyouSoft.Model.TourStructure.SearchInfo SearchModel = new EyouSoft.Model.TourStructure.SearchInfo();
            SearchModel.BuyCompanyName = Utils.GetQueryStringValue("Company");
            SearchModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(Utils.GetQueryStringValue("TourType"));
            SearchModel.LeaveDateFrom = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourStarDate"));
            SearchModel.LeaveDateTo = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourEndDate"));
            SearchModel.ComputeOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            SearchModel.AreaId = null;
            SearchModel.SalerId = null;

            if (Utils.GetQueryStringValue("SalserId") != "" && Utils.GetQueryStringValue("SalserId") != "0")
            {
                SearchModel.SalerId = int.Parse(Utils.GetQueryStringValue("SalserId"));
            }
            if (Utils.GetQueryStringValue("RouteArea") != "" && Utils.GetQueryStringValue("RouteArea") != "0")
            {
                SearchModel.AreaId = int.Parse(Utils.GetQueryStringValue("RouteArea"));
            }
            return SearchModel;
        }
    }
}
