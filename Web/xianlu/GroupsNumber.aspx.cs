using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.xianlu
{
    /// <summary>
    /// 模块名称:线路信息列表上团数弹出页面
    /// 创建时间:2011-01-12
    /// 创建人:李晓欢
    /// 
    /// 修改部分:上团数列表上面添加人数，收入，支出，毛利汇总
    /// 修改人:李晓欢
    /// 修改时间:2011-05-30
    /// </summary>
    public partial class GroupsNumber : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 10;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        protected int length = 0;  //总记录数
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.InitBindGroupNumberList();
                this.GetCount();
            }
        }

        #region 绑定上团数
        protected void InitBindGroupNumberList()
        {           
            PageIndex = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("Page"), 1);
            int RouteID = EyouSoft.Common.Utils.GetInt(Request.QueryString["ID"]);
            if (!string.IsNullOrEmpty(RouteID.ToString()) && RouteID > 0)
            {
                IList<EyouSoft.Model.TourStructure.TourBaseInfo> ToursList = new EyouSoft.BLL.TourStructure.Tour().GetToursByRouteId(SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, RouteID);
                if (ToursList != null && ToursList.Count > 0)
                {
                    length = ToursList.Count;
                    this.GroupsNumberList.DataSource = ToursList;
                    this.GroupsNumberList.DataBind();
                    BindPage();
                }
                else
                {
                    this.Group_ExportPageInfo1.Visible = false;
                }
                ToursList = null;
            }
        }
        #endregion

        #region 上团数汇总
        protected void GetCount()
        { 
            int RouteID = EyouSoft.Common.Utils.GetInt(Request.QueryString["ID"]);
            if (!string.IsNullOrEmpty(RouteID.ToString()) && RouteID > 0)
            {
                EyouSoft.Model.TourStructure.MRouteSTSCollectInfo RouteSTSCount = new EyouSoft.BLL.TourStructure.Tour().GetRouteSTSCollectInfo(SiteUserInfo.CompanyID, RouteID);
                if (RouteSTSCount != null)
                {
                    //人数汇总
                    this.PeopleCount.Text = RouteSTSCount.PeopleNumberShiShou.ToString();
                    //收入汇总
                    this.TotalIncomeCount.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(RouteSTSCount.IncomeAmount.ToString("###,##0.00"));
                    //支出汇总
                    this.TotalExpensesCount.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(RouteSTSCount.OutAmount.ToString("###,##0.00"));
                    //毛利汇总
                    this.GrossProfitCount.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(RouteSTSCount.GrossProfit.ToString("###,##0.00"));
                }
            }
        }
        #endregion

        #region 绑定分页控件
        protected void BindPage()
        {
            this.Group_ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.Group_ExportPageInfo1.UrlParams = Request.QueryString;
            this.Group_ExportPageInfo1.intPageSize = PageSize;
            this.Group_ExportPageInfo1.CurrencyPage = PageIndex;
            this.Group_ExportPageInfo1.intRecordCount = RecordCount;
        }
        #endregion

        #region 设置弹窗
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion
    }
}
