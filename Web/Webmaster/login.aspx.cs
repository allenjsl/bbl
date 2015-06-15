using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Web.Webmaster
{
    /// <summary>
    /// webmaster login page
    /// </summary>
    /// Author:汪奇志 2011-04-15
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //text:000000 MD5:670b14728ad9902aecba32e22fa4f6bd
        }

        /// <summary>
        /// btnLogin click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            StringBuilder error = new StringBuilder();

            var webmasterInfo = new EyouSoft.SSOComponent.Entity.MasterUserInfo()
            {
                Username = Request.Form["t_u"]
            };

            webmasterInfo.PassWordInfo.NoEncryptPassword = Request.Form["t_p"];


            if (string.IsNullOrEmpty(webmasterInfo.Username))
            {
                error.Append("Please enter your login information.\\n");
            }

            if (string.IsNullOrEmpty(webmasterInfo.PassWordInfo.NoEncryptPassword))
            {
                error.Append("Please enter a password.");
            }

            if (!string.IsNullOrEmpty(error.ToString()))
            {
                this.RegisterAlertScript(error.ToString());
                return;
            }

            EyouSoft.Security.Membership.UserProvider userProvider = new EyouSoft.Security.Membership.UserProvider();
            webmasterInfo = userProvider.MasterLogin(webmasterInfo.Username, webmasterInfo.PassWordInfo.MD5Password);

            if (webmasterInfo != null)
            {
                Utils.SetWebmaster(webmasterInfo);

                Response.Redirect("default.aspx");
            }
            else
            {
                this.RegisterAlertScript("Please enter the correct password.");
                return;
            }
        }

        #region private members
        /// <summary>
        /// register alert script
        /// </summary>
        /// <param name="s">msg</param>
        private void RegisterAlertScript(string s)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), string.Format("alert('{0}');", s), true);
        }
        #endregion
    }
}
