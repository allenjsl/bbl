using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SupplierControl.Hotels
{
    /// <summary>
    /// 供应商管理
    /// 李晓欢
    /// 2011-3-8
    /// </summary>
    public partial class Transaction : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        protected int len = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                int id = Utils.GetInt(Utils.GetQueryStringValue("tid"));
                this.replist.DataSource = "";
                this.replist.DataBind();
                BindPage();
            }
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
        #endregion
    }
}
