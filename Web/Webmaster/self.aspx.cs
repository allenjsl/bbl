using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Webmaster
{
    /// <summary>
    /// 我的信息管理
    /// </summary>
    /// Author:汪奇志 2011-04-26
    public partial class self : WebmasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.InitSelfInfo();
            }
        }

        #region private members
        /// <summary>
        /// register scripts
        /// </summary>
        /// <param name="script"></param>
        private void RegisterScript(string script)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        /// <summary>
        /// register alert script
        /// </summary>
        /// <param name="s">msg</param>
        private void RegisterAlertScript(string s)
        {
            this.RegisterScript(string.Format("alert('{0}');", s));
        }

        /// <summary>
        /// register alert and Redirect script
        /// </summary>
        /// <param name="s"></param>
        /// <param name="url">IsNullOrEmpty=tru page reload</param>
        private void RegisterAlertAndRedirectScript(string s, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href='{1}';", s, url));
            }
            else
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href=window.location.href;", s));
            }
        }
        /// <summary>
        /// init self info
        /// </summary>
        private void InitSelfInfo()
        {
            EyouSoft.SSOComponent.Entity.MasterUserInfo loginUserInfo = Utils.GetWebmaster();
            if (loginUserInfo != null)
            {
                this.txtUsername.Value = loginUserInfo.Username;
            }
        }
        #endregion

        #region btn click event
        /// <summary>
        /// btnUpdate_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.SysStructure.MWebmasterInfo webmasterInfo = new EyouSoft.Model.SysStructure.MWebmasterInfo();
            EyouSoft.SSOComponent.Entity.MasterUserInfo loginUserInfo = Utils.GetWebmaster();
            if (loginUserInfo != null)
            {
                webmasterInfo.UserId = loginUserInfo.UserId;
                webmasterInfo.Username = EyouSoft.Common.Utils.InputText(this.txtUsername.Value);
                webmasterInfo.Password = new EyouSoft.Model.CompanyStructure.PassWord();
                webmasterInfo.Password.NoEncryptPassword = this.txtPassword.Value;

                if (string.IsNullOrEmpty(webmasterInfo.Username))
                {
                    this.RegisterAlertAndRedirectScript("登录账号不能为空", "");
                    return;
                }

                EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
                if (bll.UpdateWebmasterInfo(webmasterInfo))
                {
                    this.RegisterAlertAndRedirectScript("账号信息更新成功", "");
                }
                else
                {
                    this.RegisterAlertAndRedirectScript("账号信息更新失败", "");
                }
            }
        }
        
        /// <summary>
        /// btnMD5Encrypt_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMD5Encrypt_Click(object sender, EventArgs e)
        {
            string s = EyouSoft.Common.Utils.InputText(this.txtPlaintext.Value);

            if (string.IsNullOrEmpty(s))
            {
                this.RegisterAlertAndRedirectScript("输入不能为空！", "");
                return;
            }

            EyouSoft.Model.CompanyStructure.PassWord p = new EyouSoft.Model.CompanyStructure.PassWord();
            p.NoEncryptPassword = s;

            this.RegisterAlertAndRedirectScript(p.MD5Password, "");
        }
        #endregion
    }
}
