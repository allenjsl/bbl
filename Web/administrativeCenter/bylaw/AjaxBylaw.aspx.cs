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
    /// 功能：行政中心-ajax规章制度列表
    /// 开发人：孙川
    /// 日期：2011-01-20
    /// </summary>
    public partial class AjaxBylaw : Eyousoft.Common.Page.BackPage
    {
        string Number = string.Empty;
        string RegentTitle = string.Empty;

        #region 分页参数
        int PageIndex = 1;
        int PageSize = 20;
        int CurrentPage = 0;
        int RecordCount;
        #endregion

        #region 权限参数
        protected bool EditFlag = false;
        protected bool DelateFlag = false;
        #endregion
       
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = Utils.GetQueryStringValue("Method");
            int dutyID = Utils.GetInt(Request.QueryString["DutyID"],-1);
            PageIndex = Utils.GetInt(Request.QueryString["Page"], -1);

            if (!IsPostBack && method == "")
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_规章制度_修改制度))
                {
                    EditFlag = true;
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_规章制度_删除制度))
                {
                    DelateFlag = true;
                }
                Number = Utils.GetQueryStringValue("Number");
                RegentTitle = Utils.GetQueryStringValue("RegentTitle");
                BindData();
            }
            if (method == "DeleteDuty" && dutyID != -1)//删除
            {
                EyouSoft.BLL.AdminCenterStructure.RuleInfo bllRuleInfo = new EyouSoft.BLL.AdminCenterStructure.RuleInfo();
                if (bllRuleInfo.Delete(CurrentUserCompanyID, dutyID))
                {
                    Response.Clear();
                    Response.Write("True");
                    Response.End();
                }
                else
                {
                    Response.Clear();
                    Response.Write("False");
                    Response.End();
                }
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData() 
        {
            EyouSoft.BLL.AdminCenterStructure.RuleInfo bllRuleInfo = new EyouSoft.BLL.AdminCenterStructure.RuleInfo();
            IList<EyouSoft.Model.AdminCenterStructure.RuleInfo> ListRuleInfo = bllRuleInfo.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, Number, RegentTitle);
            if (ListRuleInfo != null && ListRuleInfo.Count > 0)
            {
                this.crptBylawList.DataSource = ListRuleInfo;
                this.crptBylawList.DataBind();
                this.BindPage();
            }
            else
            {
                this.crptBylawList.EmptyText = "<tr><td colspan=\"3\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
                this.ExporPageInfoSelect1.Visible = false;
            }
        }


        /// <summary>
        /// 设置分页控件参数
        /// </summary>
        private void BindPage()
        {
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.HrefType = Adpost.Common.ExporPage.HrefTypeEnum.JsHref;
            this.ExporPageInfoSelect1.AttributesEventAdd("onclick", "Default.LoadData(this);", 1);
            this.ExporPageInfoSelect1.AttributesEventAdd("onchange", "Default.LoadData(this);", 0);
        }

        /// <summary>
        /// 序号
        /// </summary>
        protected int GetCount()
        {
            return ++CurrentPage + (PageIndex - 1) * PageSize;
        }
    }
}
