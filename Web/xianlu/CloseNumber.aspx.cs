using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.xianlu
{
    /// <summary>
    /// 模块名称:线路信息列表收客数弹出页面
    /// 创建时间:2011-01-12
    /// 创建人:lixh
    /// 
    /// 模块名称:线路信息列表收客数列表添加人数汇总
    /// 修改时间:2011-05-30
    /// 修改人：李晓欢
    /// </summary>
    public partial class CloseNumber : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        protected int length = 0; //列表总记录数
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.Page.IsPostBack)
            {
                InitBindCloseNumberList();
                this.GetPeoPleCount();
            }
        }

        #region 绑定收客数列表信息
        protected void InitBindCloseNumberList()
        {
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int RouteID = EyouSoft.Common.Utils.GetInt(Utils.GetQueryStringValue("ID"));
            if (!string.IsNullOrEmpty(RouteID.ToString()) && RouteID >0)
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrder = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                IList<EyouSoft.Model.TourStructure.TourOrder> TourOrderList = TourOrder.GetOrderListByRouteId(PageSize, PageIndex, ref RecordCount, RouteID);
                if (TourOrderList != null && TourOrderList.Count > 0)
                {
                    length = TourOrderList.Count;
                    this.CloseNumberList.DataSource = TourOrderList;
                    this.CloseNumberList.DataBind();
                    BIndPage();
                }
                else
                {
                    this.Close_ExportPageInfo1.Visible = false;
                }
                TourOrder = null;
                TourOrderList = null;
            }         
        }
        #endregion

        #region 收客数人数汇总
        protected void GetPeoPleCount()
        {
            int RouteID = EyouSoft.Common.Utils.GetInt(Utils.GetQueryStringValue("ID"));
            if (!string.IsNullOrEmpty(RouteID.ToString()) && RouteID > 0)
            {
                EyouSoft.Model.TourStructure.MRouteSKSCollectInfo RouteSKSCount = new EyouSoft.BLL.TourStructure.Tour().GetRouteSKSCollectInfo(SiteUserInfo.CompanyID, RouteID);
                if (RouteSKSCount != null)
                {
                    //成人数汇总
                    this.PeopleCount.Text = RouteSKSCount.AdultNumber.ToString();
                    //儿童数汇总
                    this.ChildCount.Text = RouteSKSCount.ChildrenNumber.ToString();
                }
            }
        }
        #endregion

        #region 绑定总人数
        protected int PelpeoCount(string ChildNumber, string ChengrenNumber)
        {
            return EyouSoft.Common.Utils.GetInt(ChildNumber) + EyouSoft.Common.Utils.GetInt(ChengrenNumber);
        }
        #endregion


        #region 分页控件
        protected void BIndPage()
        {
            this.Close_ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.Close_ExportPageInfo1.UrlParams = Request.QueryString;
            this.Close_ExportPageInfo1.intPageSize = PageSize;
            this.Close_ExportPageInfo1.CurrencyPage = PageIndex;
            this.Close_ExportPageInfo1.intRecordCount = RecordCount;
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
