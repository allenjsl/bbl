using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;


namespace Web.caiwuguanli
{
    public partial class PlusSubchange : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //EyouSoft.BLL.TourStructure.TourOrder bllorder = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                //EyouSoft.Model.TourStructure.TourOrder order= bllorder.GetOrderModel(SiteUserInfo.CompanyID, Utils.GetQueryStringValue("id"));
                //if (order != null)
                //{
                //    if (order.AmountPlus != null)
                //    {
                //        lt_add.Text = order.AmountPlus.AddAmount.ToString("#,##0.00");
                //        lt_sub.Text = order.AmountPlus.ReduceAmount.ToString("#,##0.00");
                //        lt_remark.Text = order.AmountPlus.Remark;
                //    }
                //}
                lt_add.Text = Utils.GetDecimal(Utils.GetQueryStringValue("add")).ToString("#,##0.00");
                lt_sub.Text = Utils.GetDecimal(Utils.GetQueryStringValue("sub")).ToString("#,##0.00");
                lt_remark.Text = Utils.GetQueryStringValue("remark");
            }
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

    }
}
