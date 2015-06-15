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
using Eyousoft.Common.Page;
using Common.Enum;

namespace Web
{
    /// <summary>
    /// 默认跳转到个人中心-事务提醒页面
    /// </summary>
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断当前域名是否是专线系统为组团用户配置的域名地址，
            //如果是的话，则跳转到该专线系统下的组团登录地址。
            EyouSoft.BLL.SysStructure.SystemDomain bll = new EyouSoft.BLL.SysStructure.SystemDomain();

            EyouSoft.Model.SysStructure.SystemDomain domain = bll.GetDomain(Request.Url.Host.ToLower());

            if (domain != null)
            {
                string desurl = domain.Url;//组团登录地址
                //如果组团登录地址不为空
                if (!string.IsNullOrEmpty(desurl))//是
                {
                    //跳转到目标URL
                    Response.Redirect(desurl, true);
                    return;
                }
            }

            //默认跳转到 个人中心 提醒页面.
            string url = "/UserCenter/WorkAwake/AppectAwake.aspx";

            Response.Redirect(url, true);
        }
    }
}
