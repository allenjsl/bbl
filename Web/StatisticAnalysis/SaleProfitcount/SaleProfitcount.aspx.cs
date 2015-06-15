using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EyouSoft.Common;
using System.Collections.Generic;
using System.IO;

namespace Web.StatisticAnalysis.SaleProfitAccount
{
    public partial class SaleProfitcount : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 20;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        IList<EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJiInfo> List = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindList();
            }
            #region 导出报表
            if (Utils.GetQueryStringValue("isExport") == "1")
            {
                ToXls();
            }
            #endregion
        }
        /// <summary>
        /// 绑定销售利润列表
        /// </summary>
        private void BindList()
        {
            //出团开始时间
            string leaveSdate = Utils.GetQueryStringValue("leavesdate");
            //出团结束时间
            string leaveEdate = Utils.GetQueryStringValue("leaveedate");
            //部门名称
            string department = Utils.GetQueryStringValue("department");
            //部门ID
            string departid = Utils.GetQueryStringValue("departid");
            //销售员id
            string sellerid = Utils.InputText(Utils.GetQueryStringValue("sellerid"));
            //销售员name
            string sellername = Utils.InputText(Utils.GetQueryStringValue("seller"));
            //线路区域id
            string routeArea = Utils.GetQueryStringValue("routearea");
            //省份id
            string province = Utils.GetQueryStringValue("province");
            //城市集合id
            string citys = Utils.GetQueryStringValue("city").Trim();
            this.UCSelectDepartment1.GetDepartmentName = department;
            this.UCSelectDepartment1.GetDepartId = departid;
            this.RouteAreaList1.RouteAreaId = Utils.GetInt(routeArea);
            this.SelectOperator.OperName = sellername;
            this.SelectOperator.OperId = sellerid;

            EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJinSearchInfo searchModel = new EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJinSearchInfo();
            searchModel.AreaId = routeArea == "0" ? null : Utils.GetIntNull(routeArea);//线路区域

            int[] provinceIds = Utils.GetIntArray(province, ",");//省份
            int[] cityIds = Utils.GetIntArray(citys, ",");//城市
            int[] DeptIds = Utils.GetIntArray(departid, ",");//部门
            int[] sellIds = Utils.GetIntArray(sellerid, ",");//销售员
            searchModel.CityIds = cityIds;
            searchModel.CTETime = Utils.GetDateTimeNullable(leaveEdate);
            searchModel.CTSTime = Utils.GetDateTimeNullable(leaveSdate);
            searchModel.DeptIds = DeptIds;
            searchModel.ProvinceIds = provinceIds;
            searchModel.XiaoShouYuanIds = sellIds;
            EyouSoft.BLL.StatisticStructure.EarningsStatistic bll = new EyouSoft.BLL.StatisticStructure.EarningsStatistic(SiteUserInfo);

            List = bll.GetTuanDuiLiRunTongJi(this.SiteUserInfo.CompanyID, searchModel, string.Empty);
            IList<EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJiInfo> list = List;
            if (List != null && List.Count > 0)
            {
                recordCount = list.Count;
                list = list.Skip(pageSize * (Utils.GetInt(Utils.GetQueryStringValue("page") == "" ? "1" : Utils.GetQueryStringValue("page")) - 1)).Take(pageSize).ToList();
                this.rptlist.DataSource = list;
                this.rptlist.DataBind();
            }
            else
            {
                this.lbemptydata.Visible = true;
                this.lbemptydata.Text = "<tr class='even'><td colspan='6' align='center'>暂无数据!</td></tr>";
                this.TrTotal.Visible = false;
            }
            #region 设置总计
            //团队数
            string TeamNum = List.Sum(p => p.TuanDuiShu).ToString();
            this.lbTeamNum.InnerText = TeamNum;
            //人数 
            int PeopleNum = List.Sum(p => p.RenShu);
            this.lbPeopleNum.InnerText = PeopleNum.ToString();
            //收入 
            decimal ShouRu = List.Sum(p => p.ShouRuJinE);
            this.lbShouRu.InnerText = ShouRu.ToString("c2");
            //利润
            decimal Profit = List.Sum(p => p.LiRun);
            this.lbProfit.InnerText = Profit.ToString("c2");
            //平均利润
            if (PeopleNum == 0)
            {
                lbAvgProfit.InnerText = "0";
            }
            else
            {
                this.lbAvgProfit.InnerText = (Profit / PeopleNum).ToString("c2");

            }
            #endregion

            #region 分页
            ExportPageInfo1.intPageSize = pageSize;
            ExportPageInfo1.intRecordCount = recordCount;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion
        }

        #region 导出Excel
        private void ToXls()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.统计分析_销售利润统计_销售利润统计栏目)) ResponseToXls(string.Empty);

            var items = List;

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            if (items != null && items.Count > 0)
            {
                s.Append("销售员\t团队数\t人数\t收入\t利润\t人均利润\n");

                foreach (var item in items)
                {
                    s.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n", item.XiaoShouYuanName, item.TuanDuiShu.ToString(), item.RenShu.ToString(), item.ShouRuJinE.ToString("C2"), item.LiRun.ToString("C2"), item.RenJunLiRun.ToString("c2"));
                }
            }

            ResponseToXls(s.ToString());
        }
        #endregion
    }
}
