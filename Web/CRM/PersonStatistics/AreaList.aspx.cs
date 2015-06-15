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
using EyouSoft.Model.StatisticStructure;

namespace Web.CRM.PersonStatistics
{
    /// <summary>
    /// 页面功能：线路产品库营销分析--区域统计
    /// Author:dj
    /// Date:2011-01-14
    /// 
    /// 修改内容:线路产品库营销分析--区域统计列表上面添加人数汇总
    /// 修改人:李晓欢
    /// 修改时间：2011-05-30
    /// </summary>
    public partial class AreaList : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        EyouSoft.BLL.StatisticStructure.InayatStatistic isBll = null;
        protected string type = string.Empty;

        protected int len = 0;
        protected EyouSoft.Model.StatisticStructure.QueryInayatStatistic QueryModel = new EyouSoft.Model.StatisticStructure.QueryInayatStatistic();//查询实体

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            type = Utils.GetQueryStringValue("type");
            if (type == "xianlu")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.线路产品库_线路产品库_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.线路产品库_线路产品库_栏目, true);
                }
                this.Title = "按区域统计_营销分析_线路产品库";
            }
            else
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_销售分析_人次统计栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_销售分析_人次统计栏目, true);
                }
                this.Title="人次统计_销售分析_客户关系管理";
            }
            this.SDepartment1.SetPicture = "/images/sanping_04.gif";
            //导出
            
            isBll = new EyouSoft.BLL.StatisticStructure.InayatStatistic(this.SiteUserInfo);
            string act = Utils.GetQueryStringValue("act");
            if (!IsPostBack)
            {
                switch (act)
                {
                    case "toexcel":
                        CreateExcel("people_area_" + DateTime.Now.ToShortDateString());
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

        /// <summary>
        /// 打印初使化
        /// </summary>
        private void PrintInit()
        {

            SelectInit();
            IList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic> areaList = null;
            areaList = isBll.GetInayaAreatStatistic(QueryModel);            
            len = areaList == null ? 0 : areaList.Count;
            if (areaList != null && areaList.Count > 0)
            {
                this.rptList.DataSource = areaList;
                this.rptList.DataBind();
            }
        }
        /// <summary>
        /// 查询实体初使化
        /// </summary>
        private void SelectInit()
        {
            QueryModel.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("stime"));
            QueryModel.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("etime"));
            QueryModel.CompanyId = CurrentUserCompanyID;//当前用户公司ID
            QueryModel.DepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("depid"), ",");
            QueryModel.SaleIds = Utils.GetIntArray(Utils.GetQueryStringValue("operid"), ",");
            QueryModel.OrderIndex = 0;
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                QueryModel.ComputeOrderType = computerOrderType.Value;
            }
        }

        /// <summary>
        /// 列表初使化
        /// </summary>
        private void DataInit()
        {
            int peolplecount = 0;
            SelectInit();
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            var items = isBll.GetInayaAreatStatistic(QueryModel);
            IList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic> areaList = null;
            areaList = EyouSoft.Common.Function.SelfExportPage.GetList(pageIndex, pageSize, out recordCount,items);
            if (areaList != null && areaList.Count > 0)
            {
                len = areaList == null ? 0 : areaList.Count;
                this.rptList.DataSource = areaList;
                this.rptList.DataBind();
                BindPage();
            }

            if (items != null && items.Count > 0)
            {
                foreach (var v in items)
                {
                    peolplecount += v.PeopleCount;
                }
            }
            this.litPeopleCount.Text = peolplecount.ToString();

            this.SDepartment1.GetDepartId = Utils.GetQueryStringValue("depid");
            this.SDepartment1.GetDepartmentName = Utils.GetQueryStringValue("depname");
            this.SOperator1.OperId = Utils.GetQueryStringValue("operid");
            this.SOperator1.OperName = Utils.GetQueryStringValue("opername");
            
        }

        #region 获取计调员
        public string GetAccouter(IList<StatisticOperator> list)
        {
            if (list != null && list.Count > 0)
                return Utils.GetListConverToStr(list);
            else
                return "";
        }
        #endregion    

        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            SelectInit();

            IList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic> list = null;
            list = isBll.GetInayaAreatStatistic(QueryModel);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\n", "线路区域", "销售员", "计调员", "人数");
            foreach (EyouSoft.Model.StatisticStructure.InayaAreatStatistic ismod in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\n",
                    ismod.AreaName,
                    GetAccouter(ismod.SalesClerk),
                    GetAccouter(ismod.Logistics),
                    ismod.PeopleCount);
            }
            Response.Write(sb.ToString());
            Response.End();
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


        #region 线路区域统计图Flash
        public string GetCartogramFlashXml()
        {
            SelectInit();

            IList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic> list = null;
            list = isBll.GetInayaAreatStatistic(QueryModel);

            StringBuilder strXml = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                strXml.Append(@"<graph formatNumber='0' formatNumberScale='0' decimalPrecision='0' xAxisName='线路区域' yAxisName='人'  yaxisminvalue='0' yaxismaxvalue='0' >");
                StringBuilder strCategory = new StringBuilder("<categories>");
                StringBuilder strDataSet = new StringBuilder("<dataset seriesname='人数' color='#AFD8F8' >");

                for (int i = 0; i < list.Count; i++)
                {
                    strCategory.Append(@"<category name='" + list[i].AreaName.ToString() + "' hoverText='" + list[i].AreaName.ToString() + "'/>");
                    //人数
                    strDataSet.Append(@"<set value='" + list[i].PeopleCount + "' />");
                }
                strCategory.Append("</categories>");
                strDataSet.Append("</dataset>");
                strXml.Append(strCategory.ToString());
                strXml.Append(strDataSet.ToString());
                strXml.Append(@"</graph>");
            }
            return strXml.ToString();
        }
        #endregion

    }
}
