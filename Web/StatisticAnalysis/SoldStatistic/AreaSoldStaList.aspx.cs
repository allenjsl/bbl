#region 命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Common.Enum;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Model.StatisticStructure;
#endregion

namespace Web.StatisticAnalysis.SoldStatistic
{
    /// <summary>
    /// 统计分析 区域销售统计
    /// </summary>
    /// 开发人员:陈志仁 开发时间:2012-02-24
    public partial class AreaSoldStaList : BackPage
    {
        #region 内部变量        
        protected int PageSize = 30;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        /// <summary>
        /// 年
        /// </summary>
        //protected int Year = DateTime.Now.Year;
        /// <summary>
        /// 月
        /// </summary>
        //protected int Month = DateTime.Now.Month;
        //protected string SalerIds = String.Empty;
        //protected string AreaIds = String.Empty;
        protected IList<EyouSoft.Model.StatisticStructure.AreaStatDepartList> DepartList = null;
        #endregion

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_区域销售统计_区域销售栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_区域销售统计_区域销售栏目, true);
                return;
            }
            #endregion

            if (Utils.GetFormValue("istoxls") == "1")
            {
                ToXls();
            }

            if (!IsPostBack)
            {
                //初始化数据
                InitSearch();
                InitDepartList();
                InitAreaList();

                InitHeJi();
            }
        }
        #endregion

        #region 加载列表
        /// <summary>
        /// 加载查询
        /// </summary>
        private void InitSearch()
        {
            DateTime? st = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("st"));
            DateTime? et = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("et"));
            int tourType = Utils.GetInt(Utils.GetQueryStringValue("tourType"), -1);
            string salerIds = Utils.GetQueryStringValue("SalerId");
            string cityIds = Utils.GetQueryStringValue("cityId");
            string salername = Utils.GetQueryStringValue("salername");

            txtStartDate.Value = st.HasValue ? st.Value.ToShortDateString() : string.Empty;
            txtEndDate.Value = et.HasValue ? et.Value.ToShortDateString() : string.Empty;
            ddlTourType.TourType = tourType;
            this.SelectSalser.OperName = salername;
            this.SelectSalser.OperId = salerIds;
        }
        /// <summary>
        /// 加载列表
        /// </summary>
        private void InitAreaList()
        {
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);//页码
            EyouSoft.Model.StatisticStructure.AreaSoldStatSearch searchInfo = new EyouSoft.Model.StatisticStructure.AreaSoldStatSearch();
            searchInfo.SalerIds = Utils.GetQueryStringValue("SalerId");
            searchInfo.CityIds = Utils.GetQueryStringValue("cityId");
            IList<EyouSoft.Model.StatisticStructure.AreaDepartStat> list = new EyouSoft.BLL.StatisticStructure.SoldStatistic().getAreaStatList(SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, searchInfo);
            if (list != null && list.Count > 0)
            {
                crp_AreaSoldList.DataSource = list;
                crp_AreaSoldList.DataBind();
                BindExportPage();
            }
            else
            {
                crp_AreaSoldList.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
        }
        /// <summary>
        /// 绑定部门列表
        /// </summary>
        private void InitDepartList()
        {
            DepartList = new List<EyouSoft.Model.StatisticStructure.AreaStatDepartList>();
            DateTime? st = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("st"));
            DateTime? et = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("et"));
            int tourType = Utils.GetInt(Utils.GetQueryStringValue("tourType"), -1);

            EyouSoft.Model.EnumType.TourStructure.TourType? tmp = tourType < 0
                                                                      ? null
                                                                      : (EyouSoft.Model.EnumType.TourStructure.TourType?)tourType;

            IList<EyouSoft.Model.StatisticStructure.AreaStatDepartList> list =
                new EyouSoft.BLL.StatisticStructure.SoldStatistic().getDepartList(
                    SiteUserInfo.CompanyID, st, et, tmp);
            System.Text.StringBuilder tmpStr = new System.Text.StringBuilder();

            if (list != null && list.Count > 0)
            {
                if (CheckGrant(TravelPermission.统计分析_区域销售统计_查看全部统计数据))
                {
                    DepartList = list;
                }
                else
                {
                    foreach (EyouSoft.Model.StatisticStructure.AreaStatDepartList item in list)
                    {
                        if (item.DepartId == SiteUserInfo.DepartId) { DepartList.Add(item); break; }
                    }
                }
            }

            if (DepartList != null && DepartList.Count > 0)
            {
                foreach (EyouSoft.Model.StatisticStructure.AreaStatDepartList item in DepartList)
                {
                    tmpStr.AppendFormat("<th width=\"80\" align=\"center\" bgcolor=\"#bddcf4\">{0}</th>", item.DepartName);
                }
            }

            list = null;
            this.ltrDepartList.Text = tmpStr.ToString();
        }
        #endregion

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams.Add(Request.QueryString);
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intRecordCount = RecordCount;
        }
        #endregion

        #region 部门人数信息
        /// <summary>
        /// 加载部门人数信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InitDepartStat(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.Literal ltrDepartStatInfo = (System.Web.UI.WebControls.Literal)e.Item.FindControl("ltrDepartStatInfo");
                //if (DepartList == null)
                //    DepartList = new EyouSoft.BLL.StatisticStructure.SoldStatistic().getDepartList(SiteUserInfo.CompanyID, Year, Month);
                DateTime? st = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("st"));
                DateTime? et = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("et"));
                int tourType = Utils.GetInt(Utils.GetQueryStringValue("tourType"), -1);
                EyouSoft.Model.EnumType.TourStructure.TourType? tmp = tourType < 0
                                                                      ? null
                                                                      : (EyouSoft.Model.EnumType.TourStructure.TourType?)tourType;
                EyouSoft.Model.StatisticStructure.AreaDepartStat row = (EyouSoft.Model.StatisticStructure.AreaDepartStat)e.Item.DataItem;
                IList<EyouSoft.Model.StatisticStructure.AreaDepartStatInfo> list =
                    new EyouSoft.BLL.StatisticStructure.SoldStatistic().getDepartStatList(
                        SiteUserInfo.CompanyID, st, et, row.CityId, row.SaleId, tmp);
                System.Text.StringBuilder tmpStr = new System.Text.StringBuilder();
                if (list.Count > 0)
                {                    
                    foreach (EyouSoft.Model.StatisticStructure.AreaStatDepartList depart in DepartList)
                    {
                        bool IsExists = true;
                        foreach (EyouSoft.Model.StatisticStructure.AreaDepartStatInfo item in list)
                        {
                            if (depart.DepartId == item.DepartId)
                            {
                                tmpStr.AppendFormat("<th width=\"80\" align=\"center\" >{0}</th>",item.TradeNumber);
                                IsExists = false;
                                break;
                            }
                        }
                        if(IsExists)
                            tmpStr.AppendFormat("<th width=\"80\" align=\"center\" >0</th>");
                    }
                }
                else
                {
                    for(int i=0;i<DepartList.Count;i++)
                    {
                        tmpStr.Append("<th width=\"80\" align=\"center\" >0</th>");
                    }
                }
                ltrDepartStatInfo.Text = tmpStr.ToString();
            }
        }
#endregion

        #region 导出EXCEL
        /// <summary>
        /// ToXls
        /// </summary>
        private void ToXls()
        {
            ResponseToXls(Request.Form["hidContent"]);
        }
        #endregion

        /// <summary>
        /// 初始化合计信息
        /// </summary>
        private void InitHeJi()
        {
            EyouSoft.Model.StatisticStructure.AreaSoldStatSearch searchInfo = new EyouSoft.Model.StatisticStructure.AreaSoldStatSearch();
            DateTime? st = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("st"));
            DateTime? et = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("et"));
            int tourType = Utils.GetInt(Utils.GetQueryStringValue("tourType"), -1);
            searchInfo.SalerIds = Utils.GetQueryStringValue("SalerId");
            searchInfo.CityIds = Utils.GetQueryStringValue("cityId");
            EyouSoft.Model.EnumType.TourStructure.TourType? tmp = tourType < 0
                                                                      ? null
                                                                      : (EyouSoft.Model.EnumType.TourStructure.TourType?)tourType;

            EyouSoft.BLL.StatisticStructure.SoldStatistic bll = new EyouSoft.BLL.StatisticStructure.SoldStatistic();

            System.Text.StringBuilder s = new System.Text.StringBuilder();

            //if (DepartList == null)
            //{
            //    DepartList = new EyouSoft.BLL.StatisticStructure.SoldStatistic().getDepartList(SiteUserInfo.CompanyID, Year, Month);
            //}

            if (DepartList != null && DepartList.Count > 0)
            {
                foreach (var item in DepartList)
                {
                    int heJi;
                    bll.GetQuYuXiaoShouTongJiHeJi(
                        CurrentUserCompanyID, st, et, item.DepartId, searchInfo, tmp, out heJi);
                    s.AppendFormat("<td align=\"center\" bgcolor=\"#bddcf4\"><b>{0}</b></td>", heJi);
                }
            }

            ltrHeJi.Text = s.ToString();
        }
    }
}
