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

namespace Web.CRM.ProfitStatistical
{
    /// <summary>
    /// 页面功能：利润统计--团队数详细列表页
    /// Author:dj
    /// Date:2011-01-20
    /// </summary>
    public partial class TeamShow : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 8;
        protected int pageIndex = 1;
        protected int recordCount;
        protected int len = 0;
        EyouSoft.Model.StatisticStructure.QueryEarningsStatistic qesModel = new EyouSoft.Model.StatisticStructure.QueryEarningsStatistic();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_销售分析_利润统计栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_销售分析_利润统计栏目, false);
            }
            if (!IsPostBack)
            {
                bind();
            }
        }
        /// <summary>
        /// 初使化列表
        /// </summary>
        private void bind()
        {
            EyouSoft.BLL.TourStructure.Tour TourBll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo, true);
            //查询实体附值
            qesModel.AreaId = Utils.GetInt(Utils.GetQueryStringValue("areaid"), 0);
            qesModel.CheckDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("cetime"));
            qesModel.CheckDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("cstime"));
            qesModel.CompanyId = CurrentUserCompanyID;
            qesModel.DepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("depid"),",");
            qesModel.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("letime"));
            qesModel.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lstime"));
            qesModel.OrderIndex = 0;
            qesModel.SaleIds = Utils.GetIntArray(Utils.GetQueryStringValue("saleman"),",");
            qesModel.CurrYear =Utils.GetInt(Utils.GetQueryStringValue("year"));
            qesModel.CurrMonth = Utils.GetInt(Utils.GetQueryStringValue("month"));
            int type = Utils.GetInt(Utils.GetQueryStringValue("type"), -1);
            if (type > -1)
            {
                qesModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)type;
            }
            int atype = Utils.GetInt(Utils.GetQueryStringValue("atype"), -1);
            if (atype == 2)
            {
                qesModel.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.单项服务;
            }

            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            IList<EyouSoft.Model.TourStructure.LBLYTJTours> list = null;
            list = TourBll.GetToursLYTJ(this.SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, qesModel);
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
