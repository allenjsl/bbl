using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.GroupEnd.Orders
{
    /// <summary>
    /// 地接社信息页面
    /// 创建时间:2011-01-19
    /// 创建人：lixh
    /// 修改人:曹胡生
    /// </summary>
    public partial class DijieInfo : Eyousoft.Common.Page.FrontPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.InitBind();
            }
        }

        #region 绑定地接社信息
        protected void InitBind()
        {
            string TourID = EyouSoft.Common.Utils.GetQueryStringValue("TourID");
            IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> TravelAgencyPriceInfoList = null;
            EyouSoft.BLL.TourStructure.Tour TourBll = new EyouSoft.BLL.TourStructure.Tour();
            if (!string.IsNullOrEmpty(TourID))
            {
                TravelAgencyPriceInfoList = TourBll.GetTourLocalAgencys(TourID);
                if (TravelAgencyPriceInfoList.Count > 0)
                {
                    this.DijieList.DataSource = TravelAgencyPriceInfoList;
                    this.DijieList.DataBind();
                }
            }
            TravelAgencyPriceInfoList = null;
            TourBll = null;
        }
        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
