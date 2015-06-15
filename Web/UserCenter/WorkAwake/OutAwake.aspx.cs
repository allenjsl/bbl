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

namespace Web.UserCenter.WorkAwake
{
    /// <summary>
    /// 页面功能：个人中心--出团提醒
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class OutAwake : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int len = 0;
        protected int day;
        EyouSoft.BLL.PersonalCenterStructure.TranRemind trBll = null;
        EyouSoft.BLL.CompanyStructure.CompanySetting csBll = null;

        //权限
        protected bool setday = false;
        #endregion
        /// <summary>
        /// 列表数据
        /// </summary>
        private IList<EyouSoft.Model.PersonalCenterStructure.LeaveTourRemind> list = new List<EyouSoft.Model.PersonalCenterStructure.LeaveTourRemind>();
        protected void Page_Load(object sender, EventArgs e)
        {
            setday = CheckGrant(global::Common.Enum.TravelPermission.系统设置_系统配置_系统设置栏目);
            trBll = new EyouSoft.BLL.PersonalCenterStructure.TranRemind(SiteUserInfo);
            csBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            if (!IsPostBack)
            {
                string type = Utils.GetQueryStringValue("type");
                switch (type)
                {
                    case "duedayset":
                        SetDay();
                        break;
                    default:
                        DataInit();
                        break;
                }

            }
        }

        private void SetDay()
        {
            if (setday)
            {


                day = Utils.GetInt(Utils.GetFormValue("num"), 1);
                bool res = csBll.SetLeaveTourReminderDays(CurrentUserCompanyID, day);


                Response.Clear();
                Response.Write("{\"ret\":" + (res ? "1" : "-1") + "}");
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write("{\"ret\":-2}");
                Response.End();
            }

        }

        /// <summary>
        /// 初使化数据
        /// </summary>
        private void DataInit()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            day = csBll.GetLeaveTourReminderDays(CurrentUserCompanyID);
            list = trBll.GetLeaveTourRemind(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, day);
            len = list == null ? 0 : list.Count;
            this.rptList.DataSource = list;
            this.rptList.DataBind();
            BindPage();
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
        }
        #endregion

        /// <summary>
        /// 绑定组团社信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptls = e.Item.FindControl("rptAgencyInfo") as Repeater;
            if (list[e.Item.ItemIndex].AgencyInfo != null)
            {
                rptls.DataSource = list[e.Item.ItemIndex].AgencyInfo;
                rptls.DataBind();
            }
        }
    }
}
