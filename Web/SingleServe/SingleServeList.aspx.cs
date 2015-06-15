using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using Common.Enum;
using EyouSoft.Common;

namespace Web.SingleServe
{
    /// <summary>
    /// 页面功能：单项服务列表
    /// Author:liuym
    /// Date:2011-01-12
    /// </summary>
    public partial class SingleServeList : BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 判断权限
            if (!CheckGrant(TravelPermission.单项服务_单项服务_栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.单项服务_单项服务_栏目, true);
                return;
            }
            #endregion

            DateTime? startDate = Utils.GetDateTimeNullable(Request.QueryString["OrderStartTime"]);
            DateTime? endDate = Utils.GetDateTimeNullable(Request.QueryString["OrderEndTime"]);

            if (!IsPostBack)
            {
                #region 表单初始化
                this.txtOrderNo.Value = Utils.GetQueryStringValue("OrderNo") == "" ? "" : Utils.GetQueryStringValue("OrderNo");
                this.txtCustomerCompany.Value = Utils.GetQueryStringValue("CustomerCompany") == "" ? "" : Utils.GetQueryStringValue("CustomerCompany");
                this.txtOrderStartTime.Value = startDate.HasValue ? startDate.Value.ToString("yyyy-MM-dd") : "";
                this.txtOrderEndTime.Value = endDate.HasValue ? startDate.Value.ToString("yyyy-MM-dd") : "";
                #endregion
            }
        }
    }
}
