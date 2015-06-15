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
    /// 景点明细
    /// </summary>
    /// 汪奇志 2012-04-03
    public partial class JingDianMingXi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitMingXi();
        }

        #region private members
        /// <summary>
        /// init jingdian mingxi
        /// </summary>
        void InitMingXi()
        {
            int id = Utils.GetInt(Utils.GetQueryStringValue("id"));
            var info = new EyouSoft.BLL.SupplierStructure.SupplierSpot().GetModel(id);

            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());

            if (domain == null || info == null || info.CompanyId != domain.CompanyId)
            {
                Response.Clear();
                Response.Write("请求异常。");
                Response.End();
            }

            if (info != null)
            {
                this.litSpotName.Text = info.UnitName;
                this.litAddress.Text = info.UnitAddress;
                this.LitProc.Text = info.ProvinceName;
                this.LitCity.Text = info.CityName;   
                this.litTourGuide.Text = info.TourGuide;
                this.litTravelerPrice.Text = info.TravelerPrice.ToString("0.00");
                this.litTeamprices.Text = info.TeamPrice.ToString("0.00");

                if (info.SupplierPic != null && info.SupplierPic.Count > 0)
                {
                    this.rpt.DataSource = info.SupplierPic;
                    this.rpt.DataBind();
                }
                else
                {
                    this.rpt.Visible = false;
                    this.litmsg.Text = "暂无景点图片";
                }
            }
        }
        #endregion
    }
}
