using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using Web.systemset.systemlog;

namespace Web.SMS
{   
    /// <summary>
    /// 自动填充短语弹出框
    /// xuty 2011/1/24
    /// </summary>
    public partial class SmsDialog : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex=1;
        protected int recordCount;
        protected int pageSize = 20;
        protected int itemIndex;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.短信中心_短信中心_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.短信中心_短信中心_栏目, true);
                return;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex = (pageIndex - 1) * pageSize + 1;
            EyouSoft.BLL.SMSStructure.CommonWords commonBll = new EyouSoft.BLL.SMSStructure.CommonWords();
            IList<EyouSoft.Model.SMSStructure.CommonWords> list = commonBll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, "", 0);
            //绑定短语
            if (list != null && list.Count > 0)
            {
                rptSms.DataSource = list;
                rptSms.DataBind();
                BindExportPage();
            }
            else
            {
                rptSms.EmptyText = "<tr><td colspan='4' align='center'>对不起，暂无短语信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion
    }
}
