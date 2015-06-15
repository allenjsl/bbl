using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.GroupEnd
{
    /// <summary>
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-11 
    /// </summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011-5.24
    /// 修改说明：显示内容控制
    public partial class about_us : System.Web.UI.Page
    {
        /// <summary>
        /// 网站Title以及小标题变量
        /// </summary>
        protected string tit = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.WhHeadControl1.NavIndex = 1;
            if (!IsPostBack)
            {
                //设置公司ID
                //判断 当前域名是否是专线系统为组团配置的域名
                EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
                if (domain != null)
                {
                    //设置友情链接
                    this.WhBottomControl1.CompanyId = domain.CompanyId;
                    //设置专线介绍
                    this.WhLineControl1.CompanyId = domain.CompanyId;
                    //头部控件
                    this.WhHeadControl1.CompanyId = domain.CompanyId;

                    #region 设置页面title,meta
                    //声明基础设置实体对象
                    EyouSoft.Model.SiteStructure.SiteBasicConfig configModel = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(domain.CompanyId);
                    if (configModel != null)
                    {
                        if (configModel.SiteTitle.Trim() == "")
                        {
                            this.Title = "公司介绍_望海";
                        }
                        else
                        {
                            this.Title = configModel.SiteTitle;
                        }
                        //添加Meta
                        Literal keywork = new Literal();
                        keywork.Text = "<meta name=\"keywords\" content= \"" + configModel.SiteMeta + "\" />";
                        this.Page.Header.Controls.Add(keywork);
                        //获取显示类型
                        string mod = Utils.GetQueryStringValue("mod");
                        //定义显示内容
                        string cont = string.Empty;
                        //定义小标题
                        switch (mod)
                        {
                            case "Introduction"://企业介绍
                                cont = configModel.Introduction;
                                tit = "企业简介";
                                break;
                            case "KeyRoute"://主营路线
                                cont = configModel.MainRoute;
                                tit = "主营路线";
                                break;
                            case "Schooling"://企业文化
                                cont = configModel.CorporateCulture;
                                tit = "企业文化";
                                break;
                            case "Link"://联系我们
                                cont = configModel.SiteIntro;
                                tit = "联系我们";
                                break;
                            default://其他为企业介绍
                                cont = configModel.Introduction;
                                tit = "公司介绍";
                                break;
                        }
                        //内容
                        this.lblCompanyInfo.Text = cont;
                        //this.lblCompanyInfo.Text = configModel.Introduction;
                    }
                    #endregion
                }

            }
        }
    }
}
