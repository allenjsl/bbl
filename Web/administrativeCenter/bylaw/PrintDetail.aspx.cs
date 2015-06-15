using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.administrativeCenter.bylaw
{
    /// <summary>
    /// 功能：行政中心-规章制度  打印
    /// 开发人：孙川
    /// 日期：2011-01-20
    /// </summary>
    public partial class PrintDetail : Eyousoft.Common.Page.BackPage
    {
        protected string DutyNo = string.Empty;
        protected string DutyTitle = string.Empty;
        protected string Content = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            int dutyID=Utils.GetInt(Request.QueryString["DutyID"],-1);
            if (!IsPostBack)
            {
                if (dutyID != -1)
                {
                    EyouSoft.BLL.AdminCenterStructure.RuleInfo bllRuleInfo = new EyouSoft.BLL.AdminCenterStructure.RuleInfo();
                    EyouSoft.Model.AdminCenterStructure.RuleInfo modelRuleInfo = bllRuleInfo.GetModel(CurrentUserCompanyID,dutyID);
                    if (modelRuleInfo != null)
                    {
                        DutyNo = modelRuleInfo.RoleNo;
                        DutyTitle = modelRuleInfo.Title;
                        Content = modelRuleInfo.RoleContent;
                        this.noData.Visible = false;
                    }
                    else
                    {
                        this.noData.Visible = true;
                    }
                }
                else
                {
                    this.noData.Visible = true;
                }
            }
        }
    }
}
