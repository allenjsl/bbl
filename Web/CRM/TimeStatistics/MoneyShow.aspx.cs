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

namespace Web.CRM.TimeStatistics
{
    /// <summary>
    ///页面功能： 拖欠款总金额
    ///Author:dj
    ///Date:2011-1-21
    /// </summary>
    public partial class MoneyShow : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        protected int len = 0;

        EyouSoft.BLL.TourStructure.TourOrder toBLL = null;
        EyouSoft.Model.TourStructure.SearchInfo sModel = new EyouSoft.Model.TourStructure.SearchInfo();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_销售分析_帐龄分析栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_销售分析_帐龄分析栏目, false);
            }
            toBLL = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            if (!IsPostBack)
            {
                bind();
            }
        }
        private void bind()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int area = Utils.GetInt(Utils.GetQueryStringValue("areaid"), 0);
            if (area > 0)
            {
                sModel.AreaId = area;
            }
            sModel.CompanyId = 0;
            sModel.LeaveDateFrom = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("stime"));
            sModel.LeaveDateTo = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("etime"));
            sModel.SalerId = Utils.GetIntNull(Utils.GetQueryStringValue("tid"));
            int tourtype = Utils.GetInt(Utils.GetQueryStringValue("type"), -1);
            if (tourtype > -1)
            {
                sModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)tourtype;
            }
            IList<EyouSoft.Model.TourStructure.TourOrder> list = null;
            list = toBLL.GetOrderList(pageSize, pageIndex, ref recordCount, sModel);
            len = list == null ? 0 : list.Count;
            this.rptList.DataSource = list;
            this.rptList.DataBind();
            BindPage();
        }

        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
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
    }
}
