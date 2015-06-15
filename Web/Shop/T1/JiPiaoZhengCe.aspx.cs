using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Shop.T1
{
    /// <summary>
    /// 机票政策
    /// </summary>
    /// 汪奇志 2012-04-03
    public partial class JiPiaoZhengCe : System.Web.UI.Page
    {
        #region attributes
        /// <summary>
        ///页记录数
        /// </summary>
        protected int pageSize = 10;
        /// <summary>
        /// 当前页索引
        /// </summary>
        protected int pageIndex = 1;
        /// <summary>
        /// 总记录数
        /// </summary>
        protected int recordCount = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            InitZhengCe();
        }

        #region private members
        /// <summary>
        /// init jipiao zhengce
        /// </summary>
        void InitZhengCe()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            var items = new EyouSoft.BLL.SiteStructure.TicketPolicy().GetTicketPolicy(Master.CompanyId, pageSize, pageIndex, ref recordCount);

            if (items != null && items.Count > 0)
            {
                rpt.DataSource = items;
                rpt.DataBind();

                divPaging.Visible = true;
                divEmpty.Visible = false;

                paging.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                paging.UrlParams.Add(Request.QueryString);
                paging.intPageSize = pageSize;
                paging.CurrencyPage = pageIndex;
                paging.intRecordCount = recordCount;
            }
            else
            {
                divPaging.Visible = false;
                divEmpty.Visible = true;
            }
        }
        #endregion
    }
}
