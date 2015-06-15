using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Webmaster
{
    /// <summary>
    /// system list page
    /// </summary>
    /// Author:汪奇志 2011-04-15
    public partial class systems : WebmasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.InitSyss();
        }

        #region private members
        /// <summary>
        /// init systems
        /// </summary>
        private void InitSyss()
        {
            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            IList<EyouSoft.Model.SysStructure.MLBSysInfo> items = bll.GetSyss(null);
            bll = null;

            if (items != null && items.Count > 0)
            {
                this.rptSys.DataSource = items;
                this.rptSys.DataBind();
                this.phNotFound.Visible = false;
            }
            else
            {
                this.phNotFound.Visible = true;
            }
        }
        #endregion

        #region rptSys_ItemDataBound
        /// <summary>
        /// rptSys_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptSys_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex < 0) return;

            Literal ltrDomain = (Literal)e.Item.FindControl("ltrDomain");
            EyouSoft.Model.SysStructure.MLBSysInfo sysInfo = (EyouSoft.Model.SysStructure.MLBSysInfo)e.Item.DataItem;

            if (ltrDomain != null && sysInfo != null && sysInfo.Domains != null && sysInfo.Domains.Count > 0)
            {
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                s.AppendFormat("&nbsp;&nbsp;{0}",sysInfo.Domains[0].Domain);

                for (int i = 1; i < sysInfo.Domains.Count;i++ )
                {
                    s.AppendFormat("<br />&nbsp;&nbsp;{0}", sysInfo.Domains[i].Domain);
                }

                ltrDomain.Text = s.ToString();
            }

        }
        #endregion
    }
}
