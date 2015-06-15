using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Shop.T1
{
    /// <summary>
    /// 机票政策明细
    /// </summary>
    /// 汪奇志 2012-04-03
    public partial class JiPiaoZhengCeMingXi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitMingXi();
        }

        #region private members
        /// <summary>
        /// init mingxi
        /// </summary>
        void InitMingXi()
        {
            int zhengCeBH = Utils.GetInt(Utils.GetQueryStringValue("id"));
            EyouSoft.BLL.SiteStructure.TicketPolicy ssBLL = new EyouSoft.BLL.SiteStructure.TicketPolicy();
            var info = new EyouSoft.BLL.SiteStructure.TicketPolicy().GetTicketPolicy(zhengCeBH, Master.CompanyId);
            if (info != null)
            {
                lblContent.Text = info.Content;
                lblTitle.Text = info.Title;
                if (!string.IsNullOrEmpty(info.FilePath))
                {
                    linkFilePath.NavigateUrl = info.FilePath;
                    linkFilePath.Visible = true;
                }
            }
        }
        #endregion
    }
}
