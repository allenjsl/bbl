using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Model.EnumType;
using EyouSoft.Common;

namespace Web.Shop.T1
{
    /// <summary>
    /// t1 master page
    /// </summary>
    /// 汪奇志 2012-04-01
    public partial class Default1 : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId;
        EyouSoft.Model.SiteStructure.SiteBasicConfig SiteInfo = null;

        protected void Page_Load(object sender, EventArgs e)
        {            
            InitHeader();

            //用户控件初始化
            WhLineControl1.CompanyId = CompanyId;
            WhBottomControl1.CompanyId = CompanyId;
        }

        #region Override OnInit
        /// <summary>
        /// override OnInit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());

            if (domain == null)
            {
                Response.Clear();
                Response.Write("域名配置错误。");
                Response.End();
            }

            CompanyId = domain.CompanyId;

            EyouSoft.Model.EnumType.SysStructure.SiteTemplate template = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetSiteTemplateId(CompanyId);
            Utils.ShopTemplateValidate(CompanyId, template);

            SiteInfo = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(CompanyId);

            if (SiteInfo == null)
            {
                Response.Clear();
                Response.Write("同行平台未做任何配置，请转至管理系统-系统设置-同行平台进行配置。");
                Response.End();
            }

            base.OnInit(e);
        }

        /// <summary>
        /// override OnPreRender
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            this.Page.Title = this.Page.Title + "_" + SiteInfo.SiteTitle ;
            ltrMeta.Text = "<meta name=\"keywords\" content= \"" + SiteInfo.SiteMeta + "\" />";

            base.OnPreRender(e);
        }
        #endregion

        #region private members
        /// <summary>
        /// init header
        /// </summary>
        private void InitHeader()
        {
            if (SiteInfo != null)
            {
                imgLogo.ImageUrl = SiteInfo.LogoPath;
                liAddFav.Visible = SiteInfo.IsSetFavorite;
                liSetHome.Visible = SiteInfo.IsSetHome;
            }
        }
        #endregion
    }
}

