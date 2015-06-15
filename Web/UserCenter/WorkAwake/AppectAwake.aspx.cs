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
using System.Collections.Generic;
using EyouSoft.Common;
using System.Text;

namespace Web.UserCenter.WorkAwake
{
    /// <summary>
    /// 页面功能：个人中心--收款提醒
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class AppectAwake : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int len = 0;
        EyouSoft.BLL.PersonalCenterStructure.TranRemind trBll = null;
        protected string S_QianKuanDanWei = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("istoxls") == "1")
            {
                ToXls();
            }

            trBll = new EyouSoft.BLL.PersonalCenterStructure.TranRemind(SiteUserInfo);
            if (!IsPostBack)
            {
                DataInit();
                
            }
        }

        /// <summary>
        /// 初使化
        /// </summary>
        private void DataInit()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_收款提醒栏目))
            {
                tbShow.Visible = false;
                tbinfo.InnerHtml = "欢迎 " + SiteUserInfo.UserName + " 登录系统!";
            }
            else
            {
                EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo = new EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo();
                
                searchInfo.QianKuanDanWei = S_QianKuanDanWei = Utils.GetQueryStringValue("sn");
                searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
                searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
                searchInfo.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
                searchInfo.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");

                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);                
                EyouSoft.BLL.CompanyStructure.CompanySetting csBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
                EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType rrt = csBll.GetReceiptRemindType(CurrentUserCompanyID);
                int? sellerId = null;
                if (rrt == EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.OnlySeller)
                {
                    sellerId = SiteUserInfo.ID;
                }
                IList<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind> list = null;
                list = trBll.GetReceiptRemind(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, rrt, sellerId, searchInfo);

                len = list == null ? 0 : list.Count;
                this.rptlist.DataSource = list;
                this.rptlist.DataBind();

                if (list != null && list.Count > 0)
                {
                    phDaiShouKuanHeJi.Visible = true;
                    decimal daiShouKuanHeJi = 0;
                    trBll.GetReceiptRemind(CurrentUserCompanyID, rrt, sellerId, searchInfo, out daiShouKuanHeJi);
                    ltrDaiShouKuanHeJi.Text = daiShouKuanHeJi.ToString("C2");
                }

                BindPage();

                RegisterScript(string.Format("var recordCount={0};", recordCount));
            }
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
        }
        #endregion

        /// <summary>
        /// to xls
        /// </summary>
        private void ToXls()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_收款提醒栏目)) ResponseToXls(string.Empty);
            

            EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo = new EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo();

            searchInfo.QianKuanDanWei = Utils.GetQueryStringValue("sn");
            searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            searchInfo.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
            searchInfo.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");

            EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType rrt = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetReceiptRemindType(CurrentUserCompanyID);
            int? sellerId = null;
            if (rrt == EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.OnlySeller) sellerId = SiteUserInfo.ID;

            int _pageSize = Utils.GetInt(Utils.GetQueryStringValue("recordcount"));
            int _recordCount = 0;

            if (_pageSize < 1) ResponseToXls(string.Empty);

            var items = new EyouSoft.BLL.PersonalCenterStructure.TranRemind().GetReceiptRemind(_pageSize, 1, ref _recordCount, CurrentUserCompanyID, rrt, sellerId, searchInfo);

            StringBuilder s = new StringBuilder();

            s.Append("客源单位\t联系人\t电话\t待收金额\t责任销售\n");

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    s.AppendFormat("{0}\t", item.CustomerName);
                    s.AppendFormat("{0}\t", item.ContactName);
                    s.AppendFormat("{0}\t", item.ContactTel);
                    s.AppendFormat("{0}\t", item.ArrearCash.ToString("C2"));
                    s.AppendFormat("{0}\n", item.SalerName);
                }
            }

            ResponseToXls(s.ToString());
        }
            
    }
}
