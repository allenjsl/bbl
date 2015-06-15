using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.GroupEnd
{
    public partial class whpolicy : System.Web.UI.Page
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //当前页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
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
                    bind();
                }
                //设置导航
                this.WhHeadControl1.NavIndex = 5;
            }
        }
        private void bind()
        {
            //初使化条件
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //机票政策BLL
            EyouSoft.BLL.SiteStructure.TicketPolicy ssBLL = new EyouSoft.BLL.SiteStructure.TicketPolicy();
            //机票政策List
            IList<EyouSoft.Model.SiteStructure.TicketPolicy> list = null;
            //判断 当前域名是否是专线系统为组团配置的域名
            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
            //获取机票政策列表
            list = ssBLL.GetTicketPolicy(domain.CompanyId, pageSize, pageIndex, ref recordCount);
            //绑定机票政策
            rptList.DataSource = list;
            rptList.DataBind();
            //判断列表是否为空
            if (list != null && list.Count == 0)
            {
                lbl_msg.Visible = true;
            }
            else
            {
                lbl_msg.Visible = false;
            }
            //分页
            BindPage();
        }
        #region 设置分页
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion
    }
}
