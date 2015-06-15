using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using Common.Enum;

namespace Web.StatisticAnalysis.ProfitStatistic
{
    /// <summary>
    /// 页面功能：利润统计--团队数详细列表页
    /// Author:liuym
    /// Date:2011-01-20
    /// </summary>
    public partial class GetTourDetailList : BackPage
    {
        #region Private Members
        protected int PageSize = 10;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected string TourType = string.Empty;//团队类型      
        protected DateTime? LeaveTourStartDate = null;//出团开始日期
        protected DateTime? LeaveTourEndDate = null;//出团结束日期
        protected DateTime? CheckStartDate = null;//核算开始日期
        protected DateTime? CheckEndDate = null;//核算结束日期
        protected int AreaId = 0;//线路区域编号
        protected string DepartId = string.Empty;//部门编号
        protected int Year = 0;//年份
        protected int Month = 0;//月份
        IList<EyouSoft.Model.TourStructure.LBLYTJTours> list = null;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                #region 权限验证
                if (!CheckGrant(TravelPermission.统计分析_利润统计_利润统计栏目))
                {
                    Utils.ResponseNoPermit(TravelPermission.统计分析_利润统计_利润统计栏目, true);
                    return;
                }
                #endregion
               //初始化数据
                IntiTourPerList();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }
        #region 初始化数据
        private void IntiTourPerList()
        {
            EyouSoft.BLL.TourStructure.Tour TourBll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //查询实体
            //默认获取当年的数据
            DateTime? CurrStartTime = new DateTime(DateTime.Now.Year,1, 1);
            DateTime? CurrEndTime = new DateTime(DateTime.Now.Year,12, 31);
            EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model = new EyouSoft.Model.StatisticStructure.QueryEarningsStatistic();
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            if (PageIndex < 1) PageIndex = 1;
            TourType = Utils.GetQueryStringValue("TourType");
            if (Utils.GetQueryStringValue("areaid") == "0")
            {
                TourType = "2";
            }
            if (Request.QueryString["IsByTime"] != null && Request.QueryString["IsByTime"] == "1")
            {
                LeaveTourStartDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveTourStartDate"], CurrStartTime);
                LeaveTourEndDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveTourEndDate"], CurrEndTime);
            }
            else
            {
                LeaveTourStartDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveTourStartDate"]);
                LeaveTourEndDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveTourEndDate"]);
            }
            
            CheckStartDate = Utils.GetDateTimeNullable(Request.QueryString["CheckStartTime"]);
            CheckEndDate = Utils.GetDateTimeNullable(Request.QueryString["CheckEndTime"]);
            AreaId = Utils.GetInt(Utils.GetQueryStringValue("AreaId"), 0);

            int _areaid = Utils.GetInt(Utils.GetQueryStringValue("RouteAreaName"));
            if (_areaid > 0) AreaId = _areaid;

            DepartId = Utils.GetQueryStringValue("DepartId");
            Year = Utils.GetInt(Request.QueryString["CuYear"],0);//年份
            Month = Utils.GetInt(Request.QueryString["CuMoth"],0);//月份

            if(TourType!=""&& int.Parse(TourType)>-1)
            {
                model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(TourType);
            }

            model.OrderIndex = 0;
            model.LeaveDateStart = LeaveTourStartDate;
            model.LeaveDateEnd = LeaveTourEndDate;
            model.CheckDateStart = CheckStartDate;
            model.CheckDateEnd = CheckEndDate;
            model.AreaId = AreaId;
            model.CurrYear = Year;
            model.CurrMonth = Month;
            model.DepartIds = Utils.GetIntArray(DepartId,",");
            model.SaleIds = Utils.GetIntArray(Utils.GetQueryStringValue("OperatorId"), ",");
            list = TourBll.GetToursLYTJ(this.SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, model);

            int pageCount = RecordCount / PageSize;
            if (RecordCount % PageSize > 0) pageCount++;
            if (PageIndex > pageCount) PageIndex = pageCount;
            if (PageIndex < 1) PageIndex = 1;

            if (list != null && list.Count > 0)
            {
                this.crp_GetTourDetailList.DataSource = list;
                this.crp_GetTourDetailList.DataBind();
                BindPage();
                this.tbl_ExportPage.Visible = true;
            }
            else
            {
                this.crp_GetTourDetailList.EmptyText = "<tr><td colspan='8' bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";
                this.tbl_ExportPage.Visible = false;
            }
            //释放资源
            list = null;
            model = null;
            TourBll = null;           
        }
        #endregion

        #region 设置分页
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intRecordCount = RecordCount;
        }
        #endregion
    }
}
