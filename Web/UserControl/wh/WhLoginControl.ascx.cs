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
    /// </summary>
    public partial class WhLoginControl : System.Web.UI.UserControl
    {
        private string _reguri = "/GroupEnd/wh/UserReg.aspx";
        /// <summary>
        /// 注册页面地址
        /// </summary>
        public string RegUri
        {
            get { return _reguri; }
            set { _reguri = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetLoginPanel();            
        }

        #region private members
        /// <summary>
        /// 登录面板设置
        /// </summary>
        void SetLoginPanel()
        {
            EyouSoft.SSOComponent.Entity.UserInfo info = EyouSoft.Security.Membership.UserProvider.GetUser();
            if (info == null || info.UserType != EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户)
            {
                divUnLogin.Visible = true;
                divLogin.Attributes.Add("style", "display:none");
            }
            else
            {
                divUnLogin.Visible = false;
                divLogin.Attributes.Add("style", "display:block");
            }
        }
        #endregion
    }
}