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
    public partial class GetTourMoreInfoList : BackPage
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
            TourType = Utils.GetQueryStringValue("TourType");
            if (Utils.GetQueryStringValue("AreaId") == "0")
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
                LeaveTourStartDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveStartDate"]);
                LeaveTourEndDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveEndDate"]);
            }
            
            CheckStartDate = Utils.GetDateTimeNullable(Request.QueryString["CheckStartTime"]);
            CheckEndDate = Utils.GetDateTimeNullable(Request.QueryString["CheckEndTime"]);
            AreaId = Utils.GetInt(Utils.GetQueryStringValue("AreaId"), 0);
            DepartId = Utils.GetQueryStringValue("DepartId");
            Year = Utils.GetInt(Request.QueryString["CuYear"],0);//年份
            Month = Utils.GetInt(Request.QueryString["CuMoth"],0);//月份

            if(TourType!=""&& int.Parse(TourType)>0)
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
            list = TourBll.GetToursLYTJ(this.SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, model);
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

        protected override void OnPreInit(EventArgs e)
        {
            base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }
        protected void crp_GetTourDetailList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.Model.TourStructure.LBLYTJTours model = e.Item.DataItem as EyouSoft.Model.TourStructure.LBLYTJTours;
            if (model != null)
            {
                ControlLibrary.CustomRepeater rptlist1 = e.Item.FindControl("rpt_list1") as ControlLibrary.CustomRepeater;
                ControlLibrary.CustomRepeater rptlist2 = e.Item.FindControl("rpt_list2") as ControlLibrary.CustomRepeater;
                ControlLibrary.CustomRepeater rptlist3 = e.Item.FindControl("rpt_list3") as ControlLibrary.CustomRepeater;
                rptlist1.DataSource = model.Orders;
                rptlist1.DataBind();
                if (model.PlanSingles != null)
                {                    
                    //单项
                    IList<EyouSoft.Model.TourStructure.LBLYTJTourPlanInfo> list = model.PlanSingles;
                    var v = from vn in list
                            where vn.ServiceType == EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通
                            select vn;
                    int sum = 0;
                    if (model.Orders != null)
                    {
                        sum = model.Orders.Sum(x => x.PeopleNumber);
                    }
                    if(v!=null)
                    foreach (var r in v)
                    {
                        r.PeopleNumber = sum;
                    }                    
                    rptlist3.DataSource = v;
                    rptlist3.DataBind();
                    list = null;
                }
                else
                {
                    IList<EyouSoft.Model.TourStructure.LBLYTJTourPlanInfo> tj = model.PlanAgencys;
                    int sum = 0;
                    if (model.Orders != null)
                    {
                        sum = model.Orders.Sum(x => x.PeopleNumber);
                    }
                    if(tj!=null)
                    foreach (var r in tj)
                    {
                        r.PeopleNumber = sum;
                    }
                    rptlist2.DataSource = tj;
                    rptlist2.DataBind();
                    rptlist3.DataSource = model.PlanTickets;
                    rptlist3.DataBind();
                }
            }
        }
    }
}
