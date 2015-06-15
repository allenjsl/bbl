using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.jipiao.Traffic
{
    /// <summary>
    /// 交通出票统计
    /// 李晓欢 2012-09-17
    /// </summary>
    public partial class trafficStatisList : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 15;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        protected int CompanyID = 1;//当前登录的公司编号
        protected int lenght = 0; //列表count
        #endregion

        #region 查询条件定义
        protected string Datetime = string.Empty;
        protected string ticketendtime = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (EyouSoft.Common.Utils.GetQueryStringValue("action") == "toexcel")
                {
                    CreateExcel("trafficStatis" + DateTime.Now.ToShortDateString());
                }
                InitStatisList();
            }
        }

        protected void InitStatisList()
        {
            Datetime = EyouSoft.Common.Utils.GetQueryStringValue("DateTime");
            ticketendtime = EyouSoft.Common.Utils.GetQueryStringValue("endtime");
            EyouSoft.Model.PlanStructure.JiaoTongChuPiaoSearch search = new EyouSoft.Model.PlanStructure.JiaoTongChuPiaoSearch();
            search.StartTime = EyouSoft.Common.Utils.GetDateTimeNullable(Datetime);
            search.EndTime = EyouSoft.Common.Utils.GetDateTimeNullable(ticketendtime);
            IList<EyouSoft.Model.PlanStructure.JiaoTongChuPiao> list = new EyouSoft.BLL.PlanStruture.PlanTrffic().GetJiaoTongChuPiao(this.SiteUserInfo.CompanyID, search);
            if (list != null && list.Count > 0)
            {
                this.prtticketlist.DataSource = list;
                this.prtticketlist.DataBind();
                BindPage();
                lenght = list.Count;

                #region 设置总计
                //总票数
                this.lblAllTickets.Text = list.Sum(p => p.ChuPiaoShu).ToString();
                this.labDaiLiFeiCount.Text = list.Sum(p => p.AgencyPrice).ToString("0.00");
                #endregion

                this.lblMsg.Visible = false;
            }
            else
            {
                this.lblMsg.Visible = true;
                this.ExportPageInfo1.Visible = false;
            }
        }

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            Datetime = EyouSoft.Common.Utils.GetQueryStringValue("DateTime");
            ticketendtime = EyouSoft.Common.Utils.GetQueryStringValue("endtime");

            //列表数据绑定
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            EyouSoft.Model.PlanStructure.JiaoTongChuPiaoSearch search = new EyouSoft.Model.PlanStructure.JiaoTongChuPiaoSearch();
            search.StartTime = EyouSoft.Common.Utils.GetDateTimeNullable(Datetime);
            search.EndTime = EyouSoft.Common.Utils.GetDateTimeNullable(ticketendtime);
            IList<EyouSoft.Model.PlanStructure.JiaoTongChuPiao> list = new EyouSoft.BLL.PlanStruture.PlanTrffic().GetJiaoTongChuPiao(this.SiteUserInfo.CompanyID, search);
            if (list != null && list.Count > 0)
            {
                //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendFormat("{0}\t{1}\t{2}\n", "交通名称", "出票数", "代理费");
                foreach (EyouSoft.Model.PlanStructure.JiaoTongChuPiao cp in list)
                {
                    sb.AppendFormat("{0}\t{1}\t{2}\n", cp.TrafficName, cp.ChuPiaoShu, cp.AgencyPrice.ToString("0.00"));
                }
                sb.AppendFormat("{0}\t{1}\t{2}\n", "总计", list.Sum(p => p.ChuPiaoShu).ToString(), list.Sum(p => p.AgencyPrice).ToString("0.00"));
                Response.Write(sb.ToString());
                Response.End();
            }

        }
        #endregion


        #region 绑定分页
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
