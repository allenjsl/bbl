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
    /// 页面功能：个人中心--付款提醒
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class PayAwake : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int len = 0; //数据长度
        EyouSoft.BLL.PersonalCenterStructure.TranRemind trBll = null;
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
        /// 初使化数据
        /// </summary>
        private void DataInit()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            var searchInfo = new EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun();
            searchInfo.ShouKuanDanWei = Utils.GetQueryStringValue("scname");
            searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            searchInfo.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
            searchInfo.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");

            IList<EyouSoft.Model.PersonalCenterStructure.PayRemind> list = null;
            list = trBll.GetPayRemind(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, searchInfo);

            len = list==null?0:list.Count;
            this.rptlist.DataSource = list;
            this.rptlist.DataBind();
            BindPage();

            if (len > 0)
            {
                phWeiShouHeJi.Visible = true;
                decimal weiShouHeJi;
                trBll.GetPayRemind(CurrentUserCompanyID, searchInfo, out weiShouHeJi);
                ltrWeiShouHeJi.Text = weiShouHeJi.ToString("C2");
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
            int _pageSize = Utils.GetInt(Utils.GetQueryStringValue("recordcount"));
            int _recordCount = 0;
            var searchInfo = new EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun();
            searchInfo.ShouKuanDanWei = Utils.GetQueryStringValue("scname");
            searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            searchInfo.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
            searchInfo.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");

            var items = new EyouSoft.BLL.PersonalCenterStructure.TranRemind(SiteUserInfo).GetPayRemind(_pageSize, 1, ref _recordCount, CurrentUserCompanyID, searchInfo);

            StringBuilder s = new StringBuilder();

            s.Append("收款单位\t联系人\t电话\t欠款金额\t责任计调\n");

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    s.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", item.SupplierName, item.ContactName, item.ContactTel, item.PayCash, item.JobName);
                }
            }

            Response.Clear();
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Write(s.ToString());
            Response.End();
        }
    }
}
