using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Shop.T1
{
    /// <summary>
    /// t1 intr pages
    /// </summary>
    /// 汪奇志 2012-04-03
    public partial class Intr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.Model.SiteStructure.SiteBasicConfig info = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(Master.CompanyId);

            if (info != null)
            {
                //获取显示类型
                string mod = Utils.GetQueryStringValue("mod");
                //定义显示内容
                string content = string.Empty;
                //定义小标题
                string title = string.Empty;
                
                switch (mod)
                {
                    case "Introduction"://企业介绍
                        content = info.Introduction;
                        title = "企业简介";
                        break;
                    case "KeyRoute"://主营路线
                        content = info.MainRoute;
                        title = "主营路线";
                        break;
                    case "Schooling"://企业文化
                        content = info.CorporateCulture;
                        title = "企业文化";
                        break;
                    case "Link"://联系我们
                        content = info.SiteIntro;
                        title = "联系我们";
                        break;
                    default://其他为企业介绍
                        content = info.Introduction;
                        title = "公司介绍";
                        break;
                }

                ltrTitle.Text = title;
                ltrContent.Text = content;
            }
        }
    }
}
