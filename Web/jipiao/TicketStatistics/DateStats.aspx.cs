using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.jipiao.TicketStatistics
{
    /// <summary>
    /// 出票统计-根据日期
    /// 柴逸宁
    /// </summary>
    public partial class DateStats : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        /// <summary>
        /// 每页显示记录条数
        /// </summary>
        private int pageSize = 15;
        /// <summary>
        /// 当前页数
        /// </summary>
        private int pageIndex;
        /// <summary>
        /// 总记录条数
        /// </summary>
        protected int recordCount;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!CheckGrant(global::Common.Enum.TravelPermission.机票管理_出票统计_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.机票管理_出票统计_栏目, true);

            }
            string cat = Utils.GetQueryStringValue("cat");

            if (!IsPostBack)
            {

                DataBindLs();
                switch (cat)
                {
                    case "toexcel":

                        toExcel("area" + DateTime.Now.ToShortDateString());

                        break;

                }
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        private void DataBindLs()
        {
            //获取显示页数
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //用户信息实体
            EyouSoft.SSOComponent.Entity.UserInfo UserModel = new EyouSoft.SSOComponent.Entity.UserInfo();
            //公司编号
            UserModel.CompanyID = SiteUserInfo.CompanyID;
            //出票统计查询实体
            EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti ssModel = new EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti();
            //查询条件
            ssModel.CompanyId = SiteUserInfo.CompanyID;
            string DepartName = Utils.GetQueryStringValue("DepartMents");//获取部门查询条件
            string departId = Utils.GetQueryStringValue("DepartIds");
            string OfficeName = Utils.GetQueryStringValue("OfficeName");//获取航空公司查询条件
            int[] AirLineIds = new int[1];
            AirLineIds[0] = Utils.GetInt(Utils.GetQueryStringValue("areaId"));//获取售票处查询条件
            if (AirLineIds[0] > 0)
            {
                ssModel.AirLineIds = AirLineIds;
            }
            ssModel.DepartIds = JiPiao_TuiList.GetIntArrByStr(departId);
            ssModel.OfficeName = OfficeName;
            ssModel.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateS"));
            ssModel.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateE"));
            //出票统计BLL
            EyouSoft.BLL.StatisticStructure.TicketOutStatistic ssBLL = new EyouSoft.BLL.StatisticStructure.TicketOutStatistic(UserModel);
            //按日期获取出票统计
            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticTime> ssList = ssBLL.GetTicketOutStatisticTime(ssModel);
            if (ssList != null && ssList.Count > 0)
            {
                retList.DataSource = ssList.Skip((pageIndex - 1) * pageSize).Take(pageSize); ;
                retList.DataBind();
                recordCount = ssList.Count;
                BindPage();

                #region 设置总计
                //总票数
                this.lblAllTickets.Text = ssList.Sum(p => p.TicketOutNum).ToString();
                //应付机票款 
                this.lblNeedMoney.Text = ssList.Sum(p => p.TotalAmount).ToString("￥#,##0.00");
                //已付机票款 
                this.lblOverMoney.Text = ssList.Sum(p => p.PayAmount).ToString("￥#,##0.00");
                //未付机票款
                this.lblNoMoney.Text = ssList.Sum(p => p.UnPaidAmount).ToString("￥#,##0.00");
                #endregion

                this.lblMsg.Visible = false;
            }
            else
            {
                this.ExportPageInfo1.Visible = false;
            }

            ssList = null;
            Bindddl(AirLineIds);
            txt_spq.Value = ssModel.OfficeName;
            if (ssModel.LeaveDateStart.HasValue)
                txtLeaveDateStart.Text = ssModel.LeaveDateStart.Value.ToShortDateString();
            if (ssModel.LeaveDateEnd.HasValue)
                txtLeaveDateEnd.Text = ssModel.LeaveDateEnd.Value.ToShortDateString();
            UCselectDepart.GetDepartmentName = DepartName;
            UCselectDepart.GetDepartId = departId;

        }
        /// <summary>
        /// 绑定航空公司列表并初始化查询条件
        /// </summary>
        /// <param name="AirLineIds">查询条件-售票处</param>
        private void Bindddl(int[] AirLineIds)
        {
            IList<EnumObj> proList = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.FlightCompany));

            ddlAirLineIds.Items.Add("--请选择--");
            ddlAirLineIds.DataSource = proList;
            ddlAirLineIds.DataBind();
            ddlAirLineIds.SelectedIndex = AirLineIds[0];
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

        /// <summary>
        /// 导出Excel
        /// </summary>
        public void toExcel(string FileName)
        {

            EyouSoft.SSOComponent.Entity.UserInfo UserModel = new EyouSoft.SSOComponent.Entity.UserInfo();
            UserModel.CompanyID = SiteUserInfo.CompanyID;
            EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti ssModel = new EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti();
            //查询条件
            ssModel.CompanyId = SiteUserInfo.CompanyID;
            string departId = Utils.GetQueryStringValue("DepartIds");
            string OfficeName = Utils.GetQueryStringValue("OfficeName");//获取航空公司查询条件
            int[] AirLineIds = new int[1];
            AirLineIds[0] = Utils.GetInt(Utils.GetQueryStringValue("areaId"));//获取售票处查询条件
            if (AirLineIds[0] > 0)
            {
                ssModel.AirLineIds = AirLineIds;
            }
            ssModel.DepartIds = JiPiao_TuiList.GetIntArrByStr(departId);
            ssModel.OfficeName = OfficeName;
            ssModel.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateS"));
            ssModel.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateE"));
            EyouSoft.BLL.StatisticStructure.TicketOutStatistic ssBLL = new EyouSoft.BLL.StatisticStructure.TicketOutStatistic(UserModel);
            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticTime> ssList = null;
            ssList = ssBLL.GetTicketOutStatisticTime(ssModel);

            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", "月份", "出票量", "应付机票款", "已付机票款", "未付机票款");
            foreach (EyouSoft.Model.StatisticStructure.TicketOutStatisticTime cs in ssList)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n",
                    cs.CurrYear + "年" + cs.CurrMonth + "月",
                    cs.TicketOutNum + "(张)",
                    cs.TotalAmount.ToString("￥#,##0.00 "),
                    cs.PayAmount.ToString("￥#,##0.00 "),
                    cs.UnPaidAmount.ToString("￥#,##0.00 "));
            }
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", "总计", lblAllTickets.Text + "(张)", lblNeedMoney.Text, lblOverMoney.Text, lblNoMoney.Text);
            Response.Write(sb.ToString());
            Response.End();


        }
    }
}
