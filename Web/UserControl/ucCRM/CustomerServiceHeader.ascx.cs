using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl.ucCRM
{  
    /// <summary>
    /// 客户服务头部导航菜单
    /// xuty 2011/01/18
    /// </summary>
    public partial class CustomerServiceHeader : System.Web.UI.UserControl
    {
        protected bool HasMarket;//是否有市场销售权限
        protected bool HasCare;//是否有客户关怀权限
        protected bool HasVisist;//是否有客户来访权限
        protected bool HasComplaint;//是否有投诉权限
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 判断权限
            Eyousoft.Common.Page.BackPage backPage = this.Page as Eyousoft.Common.Page.BackPage;
           if (backPage.CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_营销活动_栏目))
           {
               HasMarket = true;
           }
           if (backPage.CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户关怀_栏目))
           {
               HasCare = true;
           }
           if (backPage.CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_回访栏目))
           {
               HasVisist = true;
           }
           if (backPage.CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_投诉栏目))
           {
               HasComplaint = true;
           }
            #endregion
        }
        /// <summary>
        /// 菜单选中项
        /// </summary>
        public int TabIndex
        {
            get;
            set;
        }
        //面包屑导航
        public string UseMap
        {
            get;
            set;
        }
    }
}