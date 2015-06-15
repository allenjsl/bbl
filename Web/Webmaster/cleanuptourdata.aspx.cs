using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Webmaster
{
    /// <summary>
    /// 清除系统业务数据
    /// </summary>
    public partial class _cleanuptourdata : WebmasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { InitSyss(); }
        }

        #region private members
        /// <summary>
        /// 初始化子系统
        /// </summary>
        private void InitSyss()
        {
            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            IList<EyouSoft.Model.SysStructure.MLBSysInfo> items = bll.GetSyss(null);
            bll = null;

            ddlSys.Items.Clear();

            ddlSys.Items.Add(new ListItem("请选择子系统", "0"));

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    ddlSys.Items.Add(new ListItem(item.SysName, item.CompanyId.ToString()));
                }
            }
        }
        #endregion

        protected void btnCleanup_Click1(object sender, EventArgs e)
        {
            string msg = "";
            //出团日期开始
            DateTime lDateStart = EyouSoft.Common.Utils.GetDateTime(txtLDate.Value);
            //出团日期结束
            DateTime lDateEnd = EyouSoft.Common.Utils.GetDateTime(txtRDate.Value);
            //系统公司编号
            int sysID = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue(this.ddlSys.UniqueID));
            if (sysID.Equals(0))
            {
                msg = "请选择子系统!\\n";
            }
            if (lDateStart == DateTime.MinValue || lDateEnd == DateTime.MinValue)
            {
                msg += "请填写完整的出团日期!";
            }
            InitSyss();
            this.ddlSys.SelectedValue = sysID.ToString();
            if (!string.IsNullOrEmpty(msg))
            {
                EyouSoft.Common.Function.MessageBox.Show(this, msg);
                return;
            }
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour();
            this.btnCleanup.Text = "清理";
            bll.ClearTour(sysID, lDateStart, lDateEnd, ref msg);
            if(string.IsNullOrEmpty(msg))
            {
                EyouSoft.Common.Function.MessageBox.Show(this, "清理成功!");
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.Show(this, "清理失败!\\n" + msg);
            }
        }
    }
}
