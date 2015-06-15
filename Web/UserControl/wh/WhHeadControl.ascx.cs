using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl.wh
{
    /// <summary>
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-11 
    ///  
    /// 模块名称:望海组团导航控件
    /// 功能说明:望海组团导航控件
    /// 修改人：李晓欢 修改时间：2011-05-24 
    /// 修改说明：导航添加景点
    /// </summary>

    public partial class WhHeadControl : System.Web.UI.UserControl
    {
        private int navIndex;

        public int NavIndex
        {
            get { return navIndex; }
            set { navIndex = value; }
        }

        private int companyId;
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        #region 分页变量
        protected int pageSize = 50;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion

        protected int index = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.Model.SiteStructure.SiteBasicConfig siteModel = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(CompanyId);
            if (siteModel != null)
            {
                this.imgLogo.ImageUrl = siteModel.LogoPath;
            }

            if (!IsPostBack)
            {
                #region 设置是否显示

                //声明基础设置实体对象
                EyouSoft.Model.SiteStructure.SiteBasicConfig configModel = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(CompanyId);
                if (configModel != null)
                {
                    //不显示 设为收藏
                    if (!configModel.IsSetFavorite)
                    {
                        this.liAddFav.Visible = false;

                    }
                    //不显示 设为主页
                    if (!configModel.IsSetHome)
                    {
                        this.liSetHome.Visible = false;

                    }
                }

                #endregion
            }

        }
    }
}