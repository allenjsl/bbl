﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.GroupEnd.FitHairDay
{
    /// <summary>
    /// 组团端 散客天天发  成人价详细信息
    /// 创建时间：2011-03-23
    /// 创建人：李晓欢
    /// </summary>
    public partial class AdultPrices : Eyousoft.Common.Page.FrontPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string MSAdultPrice = EyouSoft.Common.Utils.GetQueryStringValue("MSAdultPrice");
                if (!string.IsNullOrEmpty(MSAdultPrice))
                {
                    this.MsCrprices.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(MSAdultPrice);
                }
                string MSChildrePricen = EyouSoft.Common.Utils.GetQueryStringValue("MSChildrePricen");
                if (!string.IsNullOrEmpty(MSChildrePricen))
                {
                    this.MsetPrices.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(MSChildrePricen);
                }
                string THAdultPrice = EyouSoft.Common.Utils.GetQueryStringValue("THAdultPrice");
                if (!string.IsNullOrEmpty(THAdultPrice))
                {
                    this.ThCrprices.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(THAdultPrice);
                }
                string THChildrenPrice = EyouSoft.Common.Utils.GetQueryStringValue("THChildrenPrice");
                if (!string.IsNullOrEmpty(THChildrenPrice))
                {
                    this.Therprices.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(THChildrenPrice);
                }
            }
        }

        #region 设置弹窗页面
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion
    }
}
