using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EyouSoft.Common;

namespace Web.CRM.ProfitStatistical
{

    public partial class TimeList : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int len = 0;

        EyouSoft.BLL.StatisticStructure.EarningsStatistic esBll = null;
        protected EyouSoft.Model.StatisticStructure.QueryEarningsStatistic qesModel = new EyouSoft.Model.StatisticStructure.QueryEarningsStatistic();//查询实体

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_销售分析_利润统计栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_销售分析_利润统计栏目, true);
            }

            esBll = new EyouSoft.BLL.StatisticStructure.EarningsStatistic(SiteUserInfo);
            string act = Utils.GetQueryStringValue("act");
            if (!IsPostBack)
            {
                switch (act)
                {
                    case "toexcel":
                        CreateExcel("pro_time_" + DateTime.Now.ToShortDateString());
                        break;
                    case "toprint":
                        PrintInit();
                        break;
                    default:
                        DataInit();
                        break;
                }
            }
        }
        private void PrintInit()
        {
            SelectInit();
            IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> list = null;
            list = esBll.GetEarningsTimeStatistic(qesModel);
            len = list == null ? 0 : list.Count;
            this.rptList.DataSource = list;
            this.rptList.DataBind();
        }

        /// <summary>
        /// 查询实体初使化
        /// </summary>
        private void SelectInit()
        {
            qesModel.CheckDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("cetime"));
            qesModel.CheckDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("cstime"));
            qesModel.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lstime"));
            qesModel.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("letime"));
            int tourtype = Utils.GetInt(Utils.GetQueryStringValue("type"), -1);
            if (tourtype != -1)
            {
                qesModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)tourtype;
            }
            qesModel.AreaId = Utils.GetInt(Utils.GetQueryStringValue("areaid"));
            qesModel.OrderIndex = 6;
            qesModel.CompanyId = CurrentUserCompanyID;
            qesModel.SaleIds = Utils.GetIntArray(Utils.GetQueryStringValue("saleman"), ",");
            qesModel.DepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("depid"), ",");

            this.department.GetDepartId = Utils.GetQueryStringValue("depid");
            this.department.GetDepartmentName = Utils.GetQueryStringValue("depname");
            this.saleman.OperId = Utils.GetQueryStringValue("saleman");
            this.saleman.OperName = Utils.GetQueryStringValue("salemanname");
            this.RouteAreaList1.RouteAreaId = Utils.GetInt(Utils.GetQueryStringValue("areaid"));
            this.TourTypeList1.TourType = tourtype;

            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                qesModel.ComputeOrderType = computerOrderType.Value;
            }
        }

        private void DataInit()
        {
            SelectInit();
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> list = null;
            list = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic>(pageIndex, pageSize, out recordCount, esBll.GetEarningsTimeStatistic(qesModel));
            #region 合计
            int teamNum = 0;
            decimal inMoney = 0;
            decimal outMoney = 0;
            decimal teamGross = 0;
            decimal pepoleGross = 0;
            decimal profitallot = 0;
            decimal comProfit = 0;
            int peopleNum = 0;
            int propleSum = 0;

            if (list != null && list.Count > 0)
            {
                foreach (var v in list)
                {
                    peopleNum += v.TourPeopleNum;
                    teamNum += v.TourNum;
                    inMoney += v.GrossIncome;
                    outMoney += v.GrossOut;
                    teamGross += v.TourGross;
                    profitallot += v.TourShare;
                    comProfit += v.CompanyShare;
                    propleSum += v.TourPeopleNum;
                }
            }
            if (peopleNum == 0)
                pepoleGross = 0;
            else
                pepoleGross = teamGross / peopleNum;
            lt_teamNum.Text = teamNum.ToString();
            lt_InMoney.Text = inMoney.ToString("###,##0.00");
            lt_outMoney.Text = outMoney.ToString("###,##0.00");
            lt_teamgross_profit.Text = teamGross.ToString("###,##0.00");
            lt_pepolegross_profit.Text = pepoleGross.ToString("###,##0.00");
            lt_profitallot.Text = profitallot.ToString("###,##0.00");
            lt_comProfit.Text = comProfit.ToString("###,##0.00");
            lt_peopleSum.Text = propleSum.ToString();

            if (inMoney != 0)
            {
                lt_lirunlv.Text = (Convert.ToDouble(comProfit) / Convert.ToDouble(inMoney)).ToString("0.00%");
            }
            else
            {
                lt_lirunlv.Text = "0.00%";
            }
            #endregion
            len = list == null ? 0 : list.Count;
            this.rptList.DataSource = list;
            this.rptList.DataBind();
            //设置分页
            BindPage();
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            SelectInit();

            IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> list = null;
            list = esBll.GetEarningsTimeStatistic(qesModel);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\n", "时间", "团队数", "人数", "总收入", "总支出", "团队毛利", "人均毛利", "利润分配", "公司利润", "利润率");
            foreach (EyouSoft.Model.StatisticStructure.EarningsTimeStatistic easmod in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\n",
                    easmod.CurrYear + "年" + easmod.CurrMonth + "月",
                    easmod.TourNum,
                    easmod.TourPeopleNum,
                    easmod.GrossIncome,
                    easmod.GrossOut,
                    easmod.TourGross,
                    easmod.PeopleGross,
                    easmod.TourShare,
                    easmod.CompanyShare,
                    easmod.LiRunLv
                    );
            }
            Response.Write(sb.ToString());
            Response.End();
        }
        #endregion



        #region 线路区域统计图Flash
        public string GetCartogramFlashXml()
        {
            SelectInit();

            IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> list = null;
            list = esBll.GetEarningsTimeStatistic(qesModel);

            StringBuilder strXml = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                strXml.Append(@"<graph formatNumber='0' formatNumberScale='0' decimalPrecision='0' xAxisName='利润时间' yAxisName='人'  yaxisminvalue='0' yaxismaxvalue='0' >");
                StringBuilder strCategory = new StringBuilder("<categories>");
                //StringBuilder strDataSet = new StringBuilder("<dataset seriesname='团量' color='#AFD8F8' >");
                StringBuilder strDataSet1 = new StringBuilder("<dataset seriesname='收入' color='#FF8E46' >");
                StringBuilder payout = new StringBuilder("<dataset seriesname='支出' color='#0066FF' >");
                StringBuilder share = new StringBuilder("<dataset seriesname='利润' color='#B22222' >");

                for (int i = 0; i < list.Count; i++)
                {
                    //线路名称
                    strCategory.AppendFormat(@"<category name='" + list[i].CurrYear + "年" + list[i].CurrMonth + "月" + "' hoverText='" + list[i].CurrYear + "年" + list[i].CurrMonth + "月" + "'/>");
                    //团量
                    //strDataSet.AppendFormat(@"<set value='{0}' />", list[i].TourNum.ToString());
                    //收入
                    strDataSet1.AppendFormat(@"<set value='{0}' />", (list[i].GrossIncome / 10000).ToString("0.00"));
                    //支出
                    payout.AppendFormat(@"<set value='{0}' />", (list[i].GrossOut / 10000).ToString("0.00"));

                    //利润
                    share.AppendFormat(@"<set value='{0}' />", (list[i].CompanyShare / 10000).ToString("0.00"));
                }
                strCategory.Append("</categories>");
                //strDataSet.Append("</dataset>");
                strDataSet1.Append("</dataset>");
                payout.Append("</dataset>");
                share.Append("</dataset>");
                strXml.Append(strCategory.ToString());
                //strXml.Append(strDataSet.ToString());
                strXml.Append(strDataSet1.ToString());
                strXml.Append(payout.ToString());
                strXml.Append(share.ToString());
                strXml.Append(@"</graph>");
            }
            return strXml.ToString();
        }
        #endregion
    }
}
