using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl
{
    /// <summary>
    /// 页面功能：线路区域控件
    /// Author:liuym
    /// Date:2011-01-27
    /// </summary>
    public partial class RouteAreaList : System.Web.UI.UserControl
    {
        /// <summary>
        /// 是否显示公司所有的线路区域
        /// </summary>
        public bool IsComAreas { get; set; }

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            Eyousoft.Common.Page.BackPage bakcPageObj = (Eyousoft.Common.Page.BackPage)this.Page;
            if (bakcPageObj != null)
            {
                //绑定线路区域
                EyouSoft.BLL.CompanyStructure.Area AreaBll = new EyouSoft.BLL.CompanyStructure.Area();
                //当前用户ID
                int CurrUserId = bakcPageObj.SiteUserInfo.ID;
                IList<EyouSoft.Model.CompanyStructure.Area> AreaList = null;

                if (!IsComAreas)
                {
                    AreaList = AreaBll.GetAreaList(CurrUserId);
                }
                else
                {
                    AreaList = AreaBll.GetAreaByCompanyId(bakcPageObj.CurrentUserCompanyID);
                }
                
                this.ddlRouteArea.DataTextField = "AreaName";
                this.ddlRouteArea.DataValueField = "Id";
                this.ddlRouteArea.DataSource = AreaList;
                this.ddlRouteArea.DataBind();
                this.ddlRouteArea.Items.Insert(0, new ListItem("-请选择-", "0"));
                //选中状态
                if (RouteAreaId > 0)
                {
                    ListItem item = this.ddlRouteArea.Items.FindByValue(RouteAreaId.ToString());
                    if (item != null)
                        item.Selected = true;
                }
                AreaBll = null;
                AreaList = null;
            }
            base.OnPreRender(e);
        }
        /// <summary>
        /// 线路ID
        /// </summary>
        public int RouteAreaId
        {
            get;
            set;
        }
    }
}