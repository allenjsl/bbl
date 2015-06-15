using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.UserCenter.WorkAwake
{
    /// <summary>
    /// 订单提醒-事务提醒-个人中心
    /// </summary>
    /// 汪奇志 2012-04-09
    public partial class DingDanTiXing : Eyousoft.Common.Page.BackPage
    {
        #region attributes
        /// <summary>
        /// page size
        /// </summary>
        protected int PageSize = 20;
        /// <summary>
        /// page index 
        /// </summary>
        protected int PageIndex = 1;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_订单提醒栏目)) Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_事务提醒_订单提醒栏目, true);

            InitTiXing();
        }

        #region private memebers
        /// <summary>
        /// init tixing
        /// </summary>
        void InitTiXing()
        {
            var searchInfo=new EyouSoft.Model.TourStructure.MDingDanTiXingInfo();
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int recordCount = 0;

            var items = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetDingDanTiXing(CurrentUserCompanyID, PageSize, PageIndex, ref recordCount, searchInfo);

            if (items != null && items.Count > 0)
            {
                rpt.DataSource = items;
                rpt.DataBind();

                divPaging.Visible = true;
                divEmpty.Visible = false;

                paging.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                paging.UrlParams.Add(Request.QueryString);
                paging.intPageSize = PageSize;
                paging.CurrencyPage = PageIndex;
                paging.intRecordCount = recordCount;
            }
            else
            {
                divPaging.Visible = false;
                divEmpty.Visible = true;
            }
        }
        #endregion

        #region protected members
        /// <summary>
        /// rtp_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rtp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var info = (EyouSoft.Model.TourStructure.MDingDanTiXingInfo)e.Item.DataItem;

            Literal ltrOrderStatus = (Literal)e.Item.FindControl("ltrOrderStatus");

            string s = "未处理";
            if (info.OrderStatus == EyouSoft.Model.EnumType.TourStructure.OrderState.已留位)
            {
                s = string.Format("留位到<br/><span style=\"color:#ff0000\">{0}</span>", info.LiuWeiShiJian);
            }

            ltrOrderStatus.Text = s;
        }
        #endregion
    }
}
