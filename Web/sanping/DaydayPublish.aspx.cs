using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.BLL.TourStructure;
using EyouSoft.Model.TourStructure;
using EyouSoft.Model.EnumType;

using Common.Enum;

namespace Web.sanping
{
    /// <summary>
    /// 散客天天发列表
    /// 田想兵 2011.3.22
    /// </summary>
    public partial class DaydayPublish : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页面变量I
        /// </summary>
        public int i = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 绑定
            if (!IsPostBack)
            {
                if (!CheckGrant(TravelPermission.散拼计划_散客天天发_栏目)){
                    Utils.ResponseNoPermit(TravelPermission.散拼计划_散客天天发_栏目, false);
                }
                selectXianlu1.Url = Request.Url.ToString();
                BindList();
            }
            #endregion
            #region 删除操作
            if (Utils.GetQueryStringValue("act")=="delete")
            {
                EyouSoft.BLL.TourStructure.TourEveryday bll = new TourEveryday(SiteUserInfo);
                int i = bll.DeleteTourEverydayInfo(Utils.GetQueryStringValue("id"));
                if (i > 0)
                {
                    EyouSoft.Common.Function.MessageBox.ShowAndRedirect(this.Page, "删除成功!", "DaydayPublish.aspx");
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.ShowAndRedirect(this.Page, "删除失败!", "DaydayPublish.aspx");
                }
            }
            #endregion
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        public void BindList()
        {
            #region 搜索条件
            EyouSoft.Model.TourStructure.TourEverydaySearchInfo tsi = new EyouSoft.Model.TourStructure.TourEverydaySearchInfo();
            tsi.AreaId = Utils.GetIntNull(Utils.GetQueryStringValue("xlid"));
            tsi.RouteName = Utils.GetQueryStringValue("xianlu");
            tsi.TourDays = Utils.GetIntNull(Utils.GetQueryStringValue("days"));
            txt_days.Text = tsi.TourDays.HasValue? tsi.TourDays.Value.ToString():"";
            txt_xianlu.Text = tsi.RouteName;
            #endregion
            #region 绑定
            EyouSoft.BLL.TourStructure.TourEveryday tour = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
            int count = 0;
            IList<EyouSoft.Model.TourStructure.LBTourEverydayInfo> list = tour.GetTourEverydays(CurrentUserCompanyID, 20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, tsi);

            rpt_list.DataSource = list;
            rpt_list.DataBind();
            #region 分页
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion
            #endregion
        }
        /// <summary>
        /// 查询跳转URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("daydaypublish.aspx?days=" + txt_days.Text + "&xianlu=" + txt_xianlu.Text + "&xlid=" + Utils.GetQueryStringValue("xlid"));

        }
        /// <summary>
        /// 获取行程单URL
        /// </summary>
        /// <param name="tourid"></param>
        /// <param name="releaseType"></param>
        /// <returns></returns>
        public string getUrl(string tourid, int releaseType)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            if (releaseType == 0)
            {
                return bll.GetPrintPath(CurrentUserCompanyID, CompanyStructure.PrintTemplateType.散拼计划标准发布行程单);
            }
            else
            {
                return bll.GetPrintPath(CurrentUserCompanyID, CompanyStructure.PrintTemplateType.散拼计划快速发布行程单);
            }
        }
    }
}
