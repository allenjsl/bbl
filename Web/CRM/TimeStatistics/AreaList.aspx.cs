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

namespace Web.CRM.TimeStatistics
{
    /// <summary>
    /// 页面功能：销售分析--帐龄分析表
    /// Author:dj
    /// Date:2011-01-14
    /// </summary>
    public partial class AreaList : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int len = 0;
        EyouSoft.BLL.StatisticStructure.AccountAgeStatistic aasBll = null;
        protected EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic qaaModel = new EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_销售分析_帐龄分析栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_销售分析_帐龄分析栏目, true);
            }
            aasBll = new EyouSoft.BLL.StatisticStructure.AccountAgeStatistic(this.SiteUserInfo);

            string act = Utils.GetQueryStringValue("act");
            if (!IsPostBack)
            {
                switch (act)
                {
                    case "toexcel":
                        ToExcel("Time_area_" + DateTime.Now.ToShortDateString());
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
            SearchInit();
            IList<EyouSoft.Model.StatisticStructure.AccountAgeStatistic> list = null;
            list = aasBll.GetAccountAgeStatistic(qaaModel);
            len = list == null ? 0 : list.Count;
            this.rptList.DataSource = list;
            this.rptList.DataBind();
        }

        private void SearchInit()
        {
            int tourtype = Utils.GetInt(Utils.GetQueryStringValue("type"), -1);
            if (tourtype != -1)
            {
                qaaModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)tourtype;
            }
            qaaModel.CompanyId = CurrentUserCompanyID;
            qaaModel.LeaveDateEnd = Utils.GetDateTime(Utils.GetQueryStringValue("etime"), DateTime.Now);
            qaaModel.LeaveDateStart = Utils.GetDateTime(Utils.GetQueryStringValue("stime"), new DateTime(DateTime.Now.Year, 1, 1));
            qaaModel.OrderIndex = 0;
            qaaModel.SaleIds = Utils.GetIntArray(Utils.GetQueryStringValue("saleman"), ",");
            qaaModel.AreaId = Utils.GetInt(Utils.GetQueryStringValue("areaid"),0);
        }
        /// <summary>
        /// 初使化
        /// </summary>
        private void DataInit()
        {
            SearchInit();
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            IList<EyouSoft.Model.StatisticStructure.AccountAgeStatistic> list = null;
            list = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.AccountAgeStatistic>(pageIndex, pageSize, out recordCount, aasBll.GetAccountAgeStatistic(qaaModel));
            len = list == null ? 0 : list.Count;
            this.rptList.DataSource = list;
            this.rptList.DataBind();
            //设置分页
            BindPage();
            this.TourTypeList1.TourType = qaaModel.TourType == null ? -1 : (int)qaaModel.TourType;
            this.saleman.OperId = Utils.GetQueryStringValue("saleman");
            this.saleman.OperName = Utils.GetQueryStringValue("salemanname");
            this.RouteAreaList1.RouteAreaId = qaaModel.AreaId;
            
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
        /// 导出excel
        /// </summary>
        /// <param name="FileName"></param>
        public void ToExcel(string FileName)
        {
            SearchInit();

            IList<EyouSoft.Model.StatisticStructure.AccountAgeStatistic> list = null;
            list = aasBll.GetAccountAgeStatistic(qaaModel);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\n", "销售员", "拖欠款总额", "最长拖欠时间（天）");
            foreach (EyouSoft.Model.StatisticStructure.AccountAgeStatistic aasmod in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\n",
                    aasmod.SalesClerk.OperatorName,
                    aasmod.ArrearageSum,
                    (DateTime.Now-aasmod.MaxArrearageTime).Days);
            }
            Response.Write(sb.ToString());
            Response.End();
        }
        #endregion

    }
}
