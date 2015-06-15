using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.sales
{
    /// <summary>
    /// 销售列表页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    /// 2、修改记录
    ///    修改时间：2011-5-31
    ///    修改人：柴逸宁
    ///    前台添加客户单位详细页面链接
    public partial class List : Eyousoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageSize = 20;
        protected int pageIndex = 1;
        int recordCount;
        #endregion
        #region 用户参数

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_栏目))
            {
                EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_销售收款_栏目, false);
                return;
            }
            if (!IsPostBack)
            {
                onInit();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.general;
        }

        //销售条件
        private void onInit()
        {


            #region 获取打印收据
            EyouSoft.BLL.CompanyStructure.CompanySetting CompanyBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            string printPath = CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.内部交款单);
            if (!string.IsNullOrEmpty(printPath))
            {
                hypSale.Visible = true;
                hypSale.NavigateUrl = printPath;
            }
            else
            {
                hypSale.Visible = false;
            }
            #endregion
            //出团日期
            DateTime? SDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("date"));
            DateTime? LDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LDate"));
            if (SDate != null)
            {
                this.txt_Date.Value = Convert.ToDateTime(SDate).ToString("yyyy-MM-dd");
            }
            if (LDate != null)
            {
                this.txt_Date1.Value = Convert.ToDateTime(LDate).ToString("yyyy-MM-dd");
            }
            //订单操作人
            int[] orderOperId = ConvertToIntArray(Utils.GetQueryStringValue("orderOperId").Split(','));
            this.selectOrderOperator.OperId = Utils.GetQueryStringValue("orderOperId");
            this.selectOrderOperator.OperName = Utils.GetQueryStringValue("orderOperName");
            //订单号
            string orderLot = Utils.GetQueryStringValue("orderLot");
            //团号
            string tuanHao = Utils.GetQueryStringValue("tuanHao");
            //客户单位
            string cusHao = Utils.GetQueryStringValue("cusHao");
            //线路名称
            string lineName = Utils.GetQueryStringValue("lineName");
            //操作人ID
            int[] operID = ConvertToIntArray(Utils.GetQueryStringValue("operId").Split(','));
            int curPage = Utils.GetInt(Utils.GetQueryStringValue("Page"));
            if (curPage != 0) { pageIndex = curPage; }
            this.selectOperator1.OperId = Utils.GetQueryStringValue("operId");
            this.selectOperator1.OperName = Utils.GetQueryStringValue("operName");
            bool? selSettle = null;
            if (Utils.GetQueryStringValue("settle") == "0")
            {
                selSettle = false;
            }
            else if (Utils.GetQueryStringValue("settle") == "1")
            {
                selSettle = true;
            }
            else
            {
                selSettle = null;
            }

            ListBind(SDate, LDate, orderOperId, orderLot, tuanHao, cusHao, lineName, operID, selSettle);
        }

        //列表初始化
        protected void ListBind(DateTime? sDate, DateTime? eDate, int[] orderOperId, string orderLot, string tuanHao, string cusHao, string lineName, int[] operIds, bool? selSettle)
        {
            int companyID = SiteUserInfo.CompanyID;
            EyouSoft.BLL.CompanyStructure.CompanySetting csBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? OrderTypeModel = csBll.GetComputeOrderType(companyID);
            EyouSoft.Model.TourStructure.OrderCenterSearchInfo queryModel = new EyouSoft.Model.TourStructure.OrderCenterSearchInfo();

            //根据条件搜索相关条件
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.TourStructure.SalerSearchInfo searchModel = new EyouSoft.Model.TourStructure.SalerSearchInfo();
            if (searchModel.ComputeOrderType == OrderTypeModel.Value)
            {
                searchModel.ComputeOrderType = EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单;
            }
            searchModel.SDate = sDate;
            searchModel.EDate = eDate;
            searchModel.OrderOperatorId = orderOperId;
            searchModel.CompanyName = cusHao;
            searchModel.OrderNo = orderLot;
            searchModel.TourNo = tuanHao;
            searchModel.RouteName = lineName;
            searchModel.SalerId = operIds;
            searchModel.ComputeOrderType = OrderTypeModel;
            searchModel.IsSettle = selSettle;
            System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrder> Ilist = TourOrderBll.GetOrderList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, searchModel);
            if (Ilist != null && Ilist.Count > 0)
            {
                EyouSoft.Model.TourStructure.MSaleReceivableSummaryInfo Summaryinfo = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetSaleReceivableSummary(SiteUserInfo.CompanyID, searchModel);
                //合同金额汇总
                this.LitFinanceSum.Text = Summaryinfo.TotalAmount.ToString("c2");
                //人数统计
                this.LitPeopleCount.Text = Summaryinfo.PeopleNumber.ToString();
                //已收金额
                this.LitHasCheckMoney.Text = Summaryinfo.ReceivedAmount.ToString("c2");
                //已收待审核金额
                this.LitNotCheckMoney.Text = Summaryinfo.UnauditedAmount.ToString("c2");
                //未收金额
                this.LitNotReceived.Text = Summaryinfo.NotReceivedAmount.ToString("c2");

                repList.DataSource = Ilist;
                repList.DataBind();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
                this.repList.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='14' align='center'>暂时没有数据！</td></tr>";
            }
            TourOrderBll = null;
            searchModel = null;
            BindPage();
        }

        // 分页控件绑定
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }

        //过滤小数点后的多余0
        public string FilterEndOfTheZeroDecimal(object o)
        {
            return Utils.FilterEndOfTheZeroString(o.ToString());
        }

        protected void repList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.Model.TourStructure.TourOrder model = (EyouSoft.Model.TourStructure.TourOrder)e.Item.DataItem;
            if (model.TourClassId == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
            {
                (e.Item.FindControl("Literal1") as Literal).Text = "单项服务";
            }
            else
            {
                (e.Item.FindControl("Literal1") as Literal).Text = model.RouteName;
            }
        }

        //将字符串数组转化成整型数组
        private int[] ConvertToIntArray(string[] source)
        {
            int[] to = new int[source.Length];
            for (int i = 0; i < source.Length; i++)//将全部的数字存到数组里。
            {
                if (!string.IsNullOrEmpty(source[i].ToString()))
                {
                    to[i] = Utils.GetInt(source[i].ToString());
                }
            }
            if (to[0] == 0)
            {
                return null;
            }
            return to;
        }
    }
}
