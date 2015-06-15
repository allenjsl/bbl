using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 单项服务收款
    /// by 田想兵 2011.2.23
    /// </summary>
    public partial class single_fukuan : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页面初始绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记, false);
                }
                string tourId = Utils.GetQueryStringValue("tourId");
                EyouSoft.BLL.TourStructure.Tour tourBll = new EyouSoft.BLL.TourStructure.Tour(this.SiteUserInfo);
                EyouSoft.Model.TourStructure.TourSingleInfo Model = (EyouSoft.Model.TourStructure.TourSingleInfo)tourBll.GetTourInfo(tourId);
                if (Model != null)
                {
                    rpt_list.DataSource = Model.Plans;
                    rpt_list.DataBind();
                }
            }
        }
    }
}
