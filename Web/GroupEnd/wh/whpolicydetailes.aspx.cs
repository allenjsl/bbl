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
    /// 机票政策
    /// </summary>
    public partial class whpolicydetailes : System.Web.UI.Page
    {
        protected string filePath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
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
                //if (configModel != null)
                //{
                //    if (configModel.SiteTitle.Trim() == "")
                //    {
                //        this.Title = "机票政策_望海";
                //    }
                //    else
                //    {
                //        this.Title = configModel.SiteTitle;
                //    }
                //    //添加Meta
                //    Literal keywork = new Literal();
                //    keywork.Text = "<meta name=\"keywords\" content= \"" + configModel.SiteMeta + "\" />";
                //    this.Page.Header.Controls.Add(keywork);
                //}
                #endregion


                //初始化列表方法
                Content();
            }
            //设置导航
            this.WhHeadControl1.NavIndex = 5;
        }
        /// <summary>
        /// 初始化列表
        /// </summary>
        private void Content()
        {
            //判断 当前域名是否是专线系统为组团配置的域名
            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
            //获取机票政策id
            int id = Utils.GetInt(Utils.GetQueryStringValue("id"));
            //机票政策BLL
            EyouSoft.BLL.SiteStructure.TicketPolicy ssBLL = new EyouSoft.BLL.SiteStructure.TicketPolicy();
            //机票政策Model
            EyouSoft.Model.SiteStructure.TicketPolicy ssModel = new EyouSoft.Model.SiteStructure.TicketPolicy();
            //获取机票政策实体
            ssModel = ssBLL.GetTicketPolicy(id, domain.CompanyId);
            if (ssModel != null)
            {
                //显示机票政策
                lblContent.Text = ssModel.Content;
                //显示标题
                lblTitle.Text = ssModel.Title;
                //显示时间
                //lblDate.Text=ssModel.
                //获取附件
                if (ssModel.FilePath.Trim() != "")
                {
                    linkFilePath.NavigateUrl = ssModel.FilePath;
                    linkFilePath.Visible = true;
                }
            }

        }
    }
}
