using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EyouSoft.Common;
using System.Collections.Generic;
using System.Text;

namespace Web.UserCenter.WorkAwake
{
    /// <summary>
    /// 页面功能：个人中心--查看明细
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class AwakeShow : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageSize = 20;
        protected int pageIndex = 1;
        protected int recordCount;
        protected int len = 0;
        EyouSoft.BLL.TourStructure.TourOrder toBll = null;
        EyouSoft.BLL.PlanStruture.TravelAgency taBll = null;
        protected string type = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("istoxls") == "1")
            {
                ToXls();
            }
            
            if (!IsPostBack)
            {
                type = Utils.GetQueryStringValue("type");
                switch (type)
                {
                    case "Appect":
                        if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_收款提醒栏目))
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_事务提醒_收款提醒栏目, false);
                        }
                        BindAppect();
                        break;
                    case "Pay":
                        if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_付款提醒栏目))
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_事务提醒_付款提醒栏目, false);
                        }
                        BindPay();
                        break;
                }
                
                
            }
        }

        /// <summary>
        /// 收款初使数据绑定
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tid"></param>
        private void BindAppect()
        {
            //初使化条件
            int tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            int sellerId = Utils.GetInt(Utils.GetQueryStringValue("sellerid"));

            var searchInfo = new EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo();

            searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            searchInfo.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
            searchInfo.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");

            //BLL实例化
            toBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            IList<EyouSoft.Model.TourStructure.TourOrder> list = null;
            list = toBll.GetOrderListByBuyCompanyId(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, tid, sellerId, searchInfo);
            len = list == null ? 0 : list.Count;
            this.repList.DataSource = list;
            this.repList.DataBind();
            //设置分页
            BindPage();

            if (len > 0)
            {
                phDaiShouKuanHeJi.Visible = true;
                decimal daiShouKuanHeJi = 0;
                toBll.GetOrderListByBuyCompanyId(CurrentUserCompanyID, tid, sellerId, searchInfo, out daiShouKuanHeJi);
                ltrDaiShouKuanHeJi.Text = daiShouKuanHeJi.ToString("C2");
            }

            RegisterScript(string.Format("var recordCount={0};", recordCount));
        }

        /// <summary>
        /// 付款初使数据绑定
        /// </summary>
        /// <param name="tid"></param>
        private void BindPay()
        {
            //初使化条件
            string[] tid_type = (Utils.GetQueryStringValue("tid")).Split('_');//供应商ID和类型
            int tid = 0; //供应商ID
            int suptype = 0; //供应商类型
            if (tid_type.Length == 2)
            {
                tid = Utils.GetInt(tid_type[0]);
                suptype = Utils.GetInt(tid_type[1]);
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //BLL实例化
            taBll = new EyouSoft.BLL.PlanStruture.TravelAgency();
            IList<EyouSoft.Model.PlanStructure.ArrearInfo> list = null;
            //EyouSoft.Model.PlanStructure.ArrearSearchInfo asiModel = new EyouSoft.Model.PlanStructure.ArrearSearchInfo();
            //asiModel.CompanyId = CurrentUserCompanyID;
            var searchInfo = new EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun();

            searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            searchInfo.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
            searchInfo.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");

            list = taBll.GetFuKuanTiXingMingXi(CurrentUserCompanyID, tid, suptype, pageSize, pageIndex, ref recordCount, searchInfo);
            len = list == null ? 0 : list.Count;
            this.repList.DataSource = list;
            this.repList.DataBind();
            BindPage();

            if (len > 0)
            {
                phDaiShouKuanHeJi.Visible = true;
                decimal weiFuHeJi = 0;
                taBll.GetFuKuanTiXingMingXi(CurrentUserCompanyID, tid, searchInfo, out weiFuHeJi);
                ltrDaiShouKuanHeJi.Text = weiFuHeJi.ToString("C2");
            }

            RegisterScript(string.Format("var recordCount={0};", recordCount));
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }

        /// <summary>
        /// to xls
        /// </summary>
        private void ToXls()
        {
            string requestType = Utils.GetQueryStringValue("type");
            if (requestType == "Appect" && !CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_收款提醒栏目)) ResponseToXls(string.Empty);
            if (requestType == "Pay" && !CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_付款提醒栏目)) ResponseToXls(string.Empty);

            StringBuilder s = new StringBuilder();
            if (requestType == "Appect")
            {
                int keHuDanWeiId = Utils.GetInt(Utils.GetQueryStringValue("tid"));
                int _pageSize = Utils.GetInt(Utils.GetQueryStringValue("recordcount"));
                if (_pageSize < 1) ResponseToXls(string.Empty);
                int _recordCount = 0;
                int sellerId = Utils.GetInt(Utils.GetQueryStringValue("sellerid"));
                var searchInfo = new EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo();
                searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
                searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
                searchInfo.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
                searchInfo.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");

                var items = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetOrderListByBuyCompanyId(_pageSize, 1, ref _recordCount, CurrentUserCompanyID, keHuDanWeiId, sellerId, searchInfo);
                s.Append("团号\t线路名称\t出团日期\t总金额\t待收金额\n");
                if (items != null&&items.Count>0)
                {
                    foreach (var item in items)
                    {
                        s.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", item.TourNo, item.RouteName, item.LeaveDate.ToString("yyyy-MM-dd"), item.FinanceSum.ToString("C2"), item.NotReceived.ToString("C2"));
                    }
                }
            }

            if (requestType == "Pay")
            {
                string[] tid_type = (Utils.GetQueryStringValue("tid")).Split('_');//供应商ID和类型
                int tid = 0; //供应商ID
                int suptype = 0; //供应商类型
                if (tid_type.Length == 2)
                {
                    tid = Utils.GetInt(tid_type[0]);
                    suptype = Utils.GetInt(tid_type[1]);
                }

                var searchInfo = new EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun();

                searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
                searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
                searchInfo.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
                searchInfo.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");

                var items = new EyouSoft.BLL.PlanStruture.TravelAgency().GetFuKuanTiXingMingXi(CurrentUserCompanyID, tid, suptype, pageSize, pageIndex, ref recordCount, searchInfo);

                if (items != null && items.Count > 0)
                {
                    s.Append("团号\t线路名称\t出团日期\t总金额\t未付金额\n");
                    if (items != null && items.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            s.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", item.TourCode, item.RouteName, item.LeaveDate.ToString("yyyy-MM-dd"), item.TotalAmount.ToString("C2"), item.Arrear.ToString("C2"));
                        }
                    }
                }
            }

            ResponseToXls(s.ToString());

        }
    }
}
