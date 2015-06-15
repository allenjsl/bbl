using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.sanping
{
    public partial class yichuli : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 散客天天发 已处理弹出窗
        /// by 田想兵 3.24
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 绑定列表
                BindList();
                #endregion
            }
        }
        /// <summary>
        /// 弹窗初始
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
                base.OnPreInit(e);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        void BindList()
        {
            EyouSoft.BLL.TourStructure.TourEveryday bll = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
            int count=0;
            IList<EyouSoft.Model.TourStructure.LBTourEverydayHandleInfo> list = bll.GetTourEverydayHandleApplys(CurrentUserCompanyID, Utils.GetQueryStringValue("tourid"), 20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref  count);
            rpt_list.DataSource = list;
            rpt_list.DataBind();
            rpt_list.EmptyText = "<tr class='even'><td colspan='8' align='center'>暂无记录!</td></tr>";
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
        }
    }
}
