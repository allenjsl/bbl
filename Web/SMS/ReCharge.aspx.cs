﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SMS
{   
    /// <summary>
    /// 短信充值
    /// xuty 2011/1/21
    /// </summary>
    public partial class ReCharge : Eyousoft.Common.Page.BackPage
    {   
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.短信中心_短信中心_栏目, true);
                return;
            }
            EyouSoft.BLL.SMSStructure.Account accountBll = new EyouSoft.BLL.SMSStructure.Account();
            string method = Utils.GetFormValue("method");
            if (method == "recharge")
            {
                #region 充值
                bool isTrue = false;
                EyouSoft.Model.SMSStructure.PayMoneyInfo Model = new EyouSoft.Model.SMSStructure.PayMoneyInfo();
                Model.CompanyID = SiteUserInfo.CompanyID;
                Model.CompanyName = SiteUserInfo.CompanyName;
                Model.IsChecked = 0;
                Model.OperatorTime = DateTime.Now;
                Model.PayTime =Utils.GetDateTime(Utils.InputText(txtRechargeDate.Value),DateTime.Now);
                Model.PayMoney = Utils.GetDecimal( Utils.InputText(txtRechargeMoney.Value));
                Model.OperatorName = SiteUserInfo.ContactInfo.ContactName;
                Model.OperatorID = SiteUserInfo.ID;
                Model.OperatorMobile= SiteUserInfo.ContactInfo.ContactMobile;
                Model.OperatorTel = SiteUserInfo.ContactInfo.ContactTel;
                isTrue = accountBll.InsertPayMoney(Model);
             
                Utils.ResponseMeg(isTrue, isTrue?"充值成功，请等待审核":"充值失败");
                return;
                #endregion
            }
            //初始化信息
            txtCompanyName.Value = SiteUserInfo.CompanyName;//客户名称
            txtUserName.Value = SiteUserInfo.ContactInfo.ContactName;//充值人
            txtRechargeDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");//充值时间
        }
    }
}
