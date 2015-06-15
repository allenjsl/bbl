using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using EyouSoft.Common;
using Common;

namespace Web.Common
{
    public partial class AwakePage : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetAjaxHtml();
        }

        private void GetAjaxHtml()
        {
            Response.Clear();
            EyouSoft.BLL.PersonalCenterStructure.TranRemind trBll = new EyouSoft.BLL.PersonalCenterStructure.TranRemind(SiteUserInfo);
            EyouSoft.BLL.CompanyStructure.CompanySetting csBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            StringBuilder sb = new StringBuilder();
            if (SiteUserInfo != null)
            {

                if (CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_收款提醒栏目))
                {
                    EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType rrt = csBll.GetReceiptRemindType(CurrentUserCompanyID);
                    int? sellerId = null;
                    if (rrt == EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.OnlySeller)
                    {
                        sellerId = SiteUserInfo.ID;
                    }
                    int awakenum = 0;
                    trBll.GetReceiptRemind(1, 0, ref awakenum, CurrentUserCompanyID, rrt, sellerId, null);

                    if (awakenum > 0)
                    {
                        sb.AppendFormat("<p><a href=\"javascript:window.opener.location.href='http/UserCenter/WorkAwake/AppectAwake.aspx';window.opener.focus();\";  >您有{0}条收款未处理！</a></p>", awakenum);
                    }
                }

                if (CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_付款提醒栏目))
                {
                    int awakenum = trBll.GetPayRemind(this.CurrentUserCompanyID);
                    if (awakenum > 0)
                    {
                        sb.AppendFormat("<p><a href=\"javascript:window.opener.location.href='http/UserCenter/WorkAwake/PayAwake.aspx';window.opener.focus();\";  >您有{0}条付款未处理！</a></p>", awakenum);
                    }
                }

                if (CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_出团提醒栏目))
                {
                    int day = csBll.GetLeaveTourReminderDays(CurrentUserCompanyID);
                    int awakenum = trBll.GetLeaveTourRemind(this.CurrentUserCompanyID, day);
                    if (awakenum > 0)
                    {
                        sb.AppendFormat("<p><a href=\"javascript:window.opener.location.href='http/UserCenter/WorkAwake/OutAwake.aspx';window.opener.focus();\";  >{0}天内有{1}个团出团！</a></p>", day, awakenum);
                    }
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.个人中心_事务提醒_回团提醒栏目))
                {
                    int day = csBll.GetBackTourReminderDays(this.CurrentUserCompanyID);
                    int awakenum = trBll.GetBackTourRemind(this.CurrentUserCompanyID, day);
                    if (awakenum > 0)
                    {
                        sb.AppendFormat("<p><a href=\"javascript:window.opener.location.href='http/UserCenter/WorkAwake/BackAwake.aspx';window.opener.focus();\";  >{0}天内有{1}个团回团！</a></p>", day, awakenum);
                    }
                }
                if (SiteUserInfo.PermissionList.Contains(5))
                {
                    int day = csBll.GetContractReminderDays(this.CurrentUserCompanyID);
                    int awakenum = trBll.GetTotalContractRemind(day, CurrentUserCompanyID);
                    if (awakenum > 0)
                    {
                        sb.AppendFormat("<p><a href=\"javascript:window.opener.location.href='http/UserCenter/WorkAwake/DueAwake.aspx';window.opener.focus();\";  >{0}天内有{1}个员工劳动合同到期！</a></p>", day, awakenum);
                    }
                }

                if (SiteUserInfo.PermissionList.Contains((int)global::Common.Enum.TravelPermission.个人中心_事务提醒_订单提醒栏目))
                {
                    var searchInfo = new EyouSoft.Model.TourStructure.MDingDanTiXingInfo();
                    int recordCount = 0;

                    var items = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetDingDanTiXing(CurrentUserCompanyID, 1, 1, ref recordCount, searchInfo);

                    if (recordCount > 0)
                    {
                        sb.AppendFormat("<p><a href=\"javascript:window.opener.location.href='http/UserCenter/WorkAwake/DingDanTiXing.aspx';window.opener.focus();\";  >您有<b>{0}</b>个未处理订单！</a></p>", recordCount);
                    }
                }
            }
            Utils.SetCookie(CookieKeyManage.LAST_LOGIN_TIME_COOKIE_KEY, DateTime.Now.ToString("yyyy-M-d-H-m-s"));
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}
