#region 命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Common.Enum;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Model.StatisticStructure;
#endregion

namespace Web.StatisticAnalysis.SoldStatistic
{
    /// <summary>
    /// 统计分析 销售统计
    /// </summary>
    /// 开发人员:陈志仁 开发时间:2012-02-24
    public partial class SoldStaList : BackPage
    {
        #region 内部变量
        private int ExportPageSize = 2000;//最大取两千条数据
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected DateTime? LeaveTourStartDate = null;//出团开始日期
        protected DateTime? LeaveTourEndDate = null;//出团结束日期        
        protected EyouSoft.Model.EnumType.FinanceStructure.QueryOperator QueryOperator = EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.None;//比较方式
        protected int TradeNum = 0;//送团人数
        protected int? customerId = null;//客户单位编号
        protected string customerName = string.Empty;//客户单位名称
        protected int[] SalserId = null;//销售员ID
        protected int[] cityId = null;//城市编号
        #endregion

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("istoxls") == "1")
            {
                ToXls();
            }

            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_销售统计_销售统计栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_销售统计_销售统计栏目, true);
                return;
            }
            #endregion

            UCSelectDepartment1.GetDepartmentName = Utils.GetQueryStringValue("deptnames");
            UCSelectDepartment1.GetDepartId = Utils.GetQueryStringValue("deptids");
            RouteAreaList1.RouteAreaId = Utils.GetInt(Utils.GetQueryStringValue("areaid"));

            if (!IsPostBack)
            {                
                //初始化加载组团社数据
                InitCustomerList();
                InitSumStat();
            }
        }
        #endregion

        #region 绑定列表数据
        /// <summary>
        /// 加载组团社数据
        /// </summary>
        private void InitCustomerList()
        {
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);//页码
            if (PageIndex < 1) PageIndex = 1;
            IList<EyouSoft.Model.CompanyStructure.CustomerInfo> list = null;//客户资料集合
            //客户资料排序实体
            EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo Seachinfo = new EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo();
            Seachinfo = SearchPara();
            list = new EyouSoft.BLL.StatisticStructure.SoldStatistic().GetCustomers(SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, Seachinfo);
            if (list != null && list.Count > 0)
            {
                double pageCount = Math.Ceiling((double)RecordCount / (double)PageSize);
                if (PageIndex > pageCount) PageIndex = (int)pageCount;

                rptCustomer.DataSource = list;
                rptCustomer.DataBind();
                BindExportPage();
            }
            else
            {
                rptCustomer.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无客户资料信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }

            RegisterScript(string.Format("var recordCount={0};", RecordCount));
        }

        /// <summary>
        /// 绑定纺计数据
        /// </summary>
        private void InitSumStat()
        {
            EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo Seachinfo = new EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo();
            Seachinfo = SearchPara();
            Dictionary<string, int> soldStat = new EyouSoft.BLL.StatisticStructure.SoldStatistic().GetSoldStatSumInfo(SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, Seachinfo);
            this.ltrSumTourNumber.Text = soldStat["TradeTourNum"].ToString();
            this.ltrSumTradeNumber.Text = soldStat["TradeNum"].ToString();
            this.ltrSumVistorNumber.Text = (soldStat["TradeNum"] - soldStat["TradeTourNum"]).ToString();
        }
        #endregion

        #region 绑定查询条件
        /// <summary>
        /// 绑定查询条件
        /// </summary>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo SearchPara()
        {
            EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo seachInfo = new EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo();
            LeaveTourStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("todate"));
            LeaveTourEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endate"));
            QueryOperator = (EyouSoft.Model.EnumType.FinanceStructure.QueryOperator)Utils.GetInt(Utils.GetQueryStringValue("qo"));
            TradeNum = Utils.GetInt(Utils.GetQueryStringValue("tn"));
            customerId = Utils.GetIntNull(Utils.GetQueryStringValue("customerId"));
            SalserId = Utils.GetIntArray(Utils.GetQueryStringValue("xsyId"), ",");
            cityId = Utils.GetIntArray(Utils.GetQueryStringValue("cityId"), ",");
            seachInfo.TourStartDate = LeaveTourStartDate;
            seachInfo.TourEndDate = LeaveTourEndDate;
            seachInfo.SearchCompare = QueryOperator;
            seachInfo.SendTourPeopleNumber = TradeNum;
            seachInfo.CustomerId = customerId;
            seachInfo.SellerIds = SalserId;
            seachInfo.CityIdList = cityId;
            seachInfo.AreaId = Utils.GetIntNull(Utils.GetQueryStringValue("areaid"));
            seachInfo.DeptIds = Utils.GetIntArray(Utils.GetQueryStringValue("deptids"), ",");
            seachInfo.XSTJFXOrderByType = Utils.GetInt(Utils.GetQueryStringValue("orderbytype"));

            if (seachInfo.AreaId.HasValue && seachInfo.AreaId.Value < 1) seachInfo.AreaId = null;

            return seachInfo;
        }
        #endregion

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams.Add(Request.QueryString);
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intRecordCount = RecordCount;
        }
        #endregion

        #region to xls
        /// <summary>
        /// to xls
        /// </summary>
        private void ToXls()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.统计分析_销售统计_销售统计栏目)) ResponseToXls(string.Empty);

            int pageSize = Utils.GetInt(Utils.GetQueryStringValue("recordcount"));
            int recordCount = 0;

            if (pageSize < 1) ResponseToXls(string.Empty);

            var chaXun = SearchPara();
            var items = new EyouSoft.BLL.StatisticStructure.SoldStatistic().GetCustomers(SiteUserInfo.CompanyID, pageSize, 1, ref recordCount, chaXun);

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            if (items != null && items.Count > 0)
            {
                s.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\n", "所属区域", "组团社名称", "联系人", "联系电话", "责任销售", "团队人数", "散客人数", "合计人数");

                foreach (var item in items)
                {
                    s.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\n",
                    item.ProvinceName + " " + item.CityName,
                    item.Name,
                    item.ContactName,
                    item.Phone,
                    item.Saler,
                    item.TradeNum - item.TradeTourNum,
                    item.TradeTourNum,
                    item.TradeNum);
                }
            }

            ResponseToXls(s.ToString());
        }
        #endregion
    }
}
