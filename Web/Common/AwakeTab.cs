using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Common.Enum;
using System.Text;
using EyouSoft.SSOComponent.Entity;

/// acthor dj
/// date 2011-02-17
/// 功能 头部tab
namespace Web.Common
{
    /// <summary>
    /// 头部TAB
    /// </summary>
    public class AwakeTab
    {
        /// <summary>
        /// 提醒TAB
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="currectpage">当前权限页面</param>
        /// <returns>tab拼装的html</returns>
        public static string createAwakeTab(UserInfo user, int currectpage)
        {
            StringBuilder sb = new StringBuilder();
            if (user != null)
            {
                if (user.PermissionList.Contains(1))
                {
                    sb.Append(createOneTab("收款提醒", "/UserCenter/WorkAwake/AppectAwake.aspx", currectpage == 1));
                }
                if (user.PermissionList.Contains(2))
                {
                    sb.Append(createOneTab("付款提醒", "/UserCenter/WorkAwake/PayAwake.aspx", currectpage == 2));
                }
                if (user.PermissionList.Contains(3))
                {
                    sb.Append(createOneTab("出团提醒", "/UserCenter/WorkAwake/OutAwake.aspx", currectpage == 3));
                }
                if (user.PermissionList.Contains(4))
                {
                    sb.Append(createOneTab("回团提醒", "/UserCenter/WorkAwake/BackAwake.aspx", currectpage == 4));
                }
                if (user.PermissionList.Contains(5))
                {
                    sb.Append(createOneTab("劳动合同到期提醒", "/UserCenter/WorkAwake/DueAwake.aspx", currectpage == 5));
                }
                if (user.PermissionList.Contains((int)global::Common.Enum.TravelPermission.个人中心_事务提醒_订单提醒栏目))
                {
                    sb.Append(createOneTab("订单提醒", "/UserCenter/WorkAwake/DingDanTiXing.aspx", currectpage == (int)global::Common.Enum.TravelPermission.个人中心_事务提醒_订单提醒栏目));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 交流tab
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="currectpage">当前权限页面</param>
        /// <returns>tab拼装的html</returns>
        public static string createExchangeTab(UserInfo user, int currectpage)
        {
            StringBuilder sb = new StringBuilder();
            if (user != null)
            {
                if (user.PermissionList.Contains(9))
                {
                    sb.Append(createOneTab("工作汇报", "/UserCenter/WorkExchange/WorkReport.aspx", currectpage == 9));
                }
                if (user.PermissionList.Contains(10))
                {
                    sb.Append(createOneTab("工作计划", "/UserCenter/WorkExchange/WorkPlan.aspx", currectpage == 10));
                }
                if (user.PermissionList.Contains(11))
                {
                    sb.Append(createOneTab("工作交流", "/UserCenter/WorkExchange/Exchange.aspx", currectpage == 11));
                }
            }
            return sb.ToString();
        }

        public static string createSale(UserInfo user, int currectpage)
        {
            string cls = "xtnav-on";
            StringBuilder sb = new StringBuilder();
            if (user != null)
            {
                if (user.PermissionList.Contains(127))
                {
                    sb.Append(createOneTab("人次统计", "/CRM/PersonStatistics/AreaList.aspx", currectpage == 127,cls));
                }
                if (user.PermissionList.Contains(128))
                {
                    sb.Append(createOneTab("利润统计", "/CRM/ProfitStatistical/AreaList.aspx", currectpage == 128, cls));
                }
                if (user.PermissionList.Contains(129))
                {
                    sb.Append(createOneTab("帐龄分析表", "/CRM/TimeStatistics/AreaList.aspx", currectpage == 129, cls));
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 单条tabHTML
        /// </summary>
        /// <param name="content">tab内容</param>
        /// <param name="url">tab链接</param>
        /// <param name="isselect">是否选中</param>
        /// <returns></returns>
        private static string createOneTab(string content, string url, bool isselect)
        {
            return string.Format("<td width=\"108\" align=\"center\" {0} ><a href=\"{1}\">{2}</a></td>", isselect ? "class=\"grzxnav-on\"" : "", url, content);
        }

        private static string createOneTab(string content, string url, bool isselect, string cls)
        {
            return string.Format("<td width=\"108\" align=\"center\" {0} ><a href=\"{1}\">{2}</a></td>", isselect ? "class=\""+cls+"\"" : "", url, content);
        }
    }
}
