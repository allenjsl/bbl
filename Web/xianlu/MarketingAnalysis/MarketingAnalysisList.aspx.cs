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
using Eyousoft.Common;
using EyouSoft.Common;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Web
{
    /// <summary>
    /// 页面功能：营销分析（区域统计）：查询，导出到Excel，统计图，打印
    /// Author：  liangx
    /// Date：    2010-1-13
    /// ------------------------------------------------------------------------
    /// 修改人：张新兵，修改时间：2011-01-21
    /// </summary>
    public partial class MarketingAnalysisList : Eyousoft.Common.Page.BackPage
    {
        #region Protected Members
        protected int PageIndex = 1;//页码
        protected int PageSize = 20;//每页显示的记录：20条
        protected int RecordCount = 0;//总记录
        protected string Department = string.Empty; //部门
        protected string DepartmentId = string.Empty;//部门ID
        protected string SalesMan = string.Empty;   //销售员
        protected string SalesManId = string.Empty; //销售员id
        protected DateTime? TimeStar;                //开始时间
        protected DateTime? TimeEnd;                 //结束时间
        protected string IsExcel = string.Empty;   //是否导出
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["IsExcel"] != null && Request.QueryString["IsExcel"].ToString() == "1")
            {
                IList<string> list=new List<string>();
              
                ToExcel(this.CustomRepeater1,list);
            }
            string isGetXml = Utils.GetQueryStringValue("getXml");
            if (isGetXml == "1")
            {
                Response.Clear();
                Response.Write(FusionChartsFree());
                Response.End();
            }
           if(!Page.IsPostBack)
           {
               BindAreaList();
           }
        }

        #region 区域统计列表信息
        public void BindAreaList()
        {
            //获取查询参数
            Department = Utils.GetQueryStringValue("Department");//部门
            DepartmentId = Utils.GetQueryStringValue("DepartmentId");//部门ID
            SalesMan = Utils.InputText(Request.QueryString["Xiaosy"]);//销售员
            SalesManId = Utils.InputText(Server.UrlDecode(Request.QueryString["XiaosyId"]));//销售员ID列表
            TimeStar = Utils.GetDateTimeNullable(Request.QueryString["TimeStar"]);//开始时间
            TimeEnd =Utils.GetDateTimeNullable(Request.QueryString["TimeEnd"]);//结束时间
            PageIndex = Utils.GetInt(Request.QueryString["PageIndex"], 1);//页码

            //根据查询参数 初始化页面
            //销售员
            selectOperator2.OperName = SalesMan;
            selectOperator2.OperId = SalesManId;
            //部门
            UCselectDepart1.GetDepartId = DepartmentId;
            UCselectDepart1.GetDepartmentName = Department;

            //init BLL
            EyouSoft.BLL.StatisticStructure.InayatStatistic InayaStaBLL =
                new EyouSoft.BLL.StatisticStructure.InayatStatistic(this.SiteUserInfo);

            //查询Model
            EyouSoft.Model.StatisticStructure.QueryInayatStatistic QueryModel = 
                new EyouSoft.Model.StatisticStructure.QueryInayatStatistic();//查询实体
            QueryModel.DepartIds = Utils.GetIntArray(DepartmentId, ",");//部门
            QueryModel.SaleIds = Utils.GetIntArray(SalesManId, ",");//销售员
            QueryModel.StartTime = TimeStar;//下单时间 ，开始
            QueryModel.EndTime = TimeEnd;//下单时间，结束
            QueryModel.OrderIndex = 0;//排序

            IList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic> PerAreaStaList 
                = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic>(
                            PageIndex, PageSize, out RecordCount, InayaStaBLL.GetInayaAreatStatistic(QueryModel));

            //绑定数据
            this.CustomRepeater1.DataSource = PerAreaStaList;
            this.CustomRepeater1.DataBind();

            // 绑定分页控件

            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.intRecordCount = RecordCount;
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
        }
        #endregion 
        
        #region ExportToExcel
        public void ToExcel(System.Web.UI.Control ctl, IList<string> AreaStatisticeList)
        {

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=区域统计.xls");

            HttpContext.Current.Response.Charset = "UTF-8";

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;

            HttpContext.Current.Response.ContentType = "application/ms-excel";

            ctl.Page.EnableViewState = false;

            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            ctl.RenderControl(hw);
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 区域统计图
        public string FusionChartsFree()
        {
            IList<AreaStatistic> list = new List<AreaStatistic>();//数据源
            list.Add(new AreaStatistic()
            {
                AreaName = "上海线",
                PersonNum = "100",
                Day = 10
            });
            list.Add(new AreaStatistic()
            {
                AreaName = "杭州线",
                PersonNum = "354",
                Day = 50
            });
            //string GetFree = string.Empty;
            StringBuilder strXml = new StringBuilder();
            StringBuilder strCategory = new StringBuilder("<categories>");
            StringBuilder strDataSet = new StringBuilder("<dataset seriesname='人数' color='F0807F' showValue='1'>");
            StringBuilder strDataSetDay = new StringBuilder("<dataset seriesname='人天数' color='B22222' showValue='1'>");
            strXml.Append(@"<graph xAxisName='线路区域' canvasBgColor='F6DFD9' canvasBaseColor='FE6E54' hovercapbgColor='FFECAA' hovercapborder='F47E00' divlinecolor='F47E00' limitsDecimalPrecision='0' divLineDecimalPrecision='0'>");

            for (int i = 0; i < list.Count; i++)
            {
                strCategory.Append(@"<category name='" + list[i].AreaName.ToString() + "' hoverText='" + list[i].AreaName.ToString() + "'/>");
                //人数
                strDataSet.Append(@"<set value='" + list[i].PersonNum.ToString() + "' />");
                //人天数
                strDataSetDay.Append(@"<set value='" + list[i].Day.ToString() + "' />");
            }
            strCategory.Append("</categories>");
            strDataSet.Append("</dataset>");
            strDataSetDay.Append("</dataset>");
            strXml.Append(strCategory.ToString());
            strXml.Append(strDataSet.ToString());
            strXml.Append(strDataSetDay.ToString());
            strXml.Append(@"</graph>");
            return strXml.ToString();
        }
        #endregion
   
        public class AreaStatistic
        {
            public string AreaName { get; set; }//线路区域名称
            public string PersonNum { get; set; }//人数
            public int Day { get; set; }//天数
        }
    }
}
