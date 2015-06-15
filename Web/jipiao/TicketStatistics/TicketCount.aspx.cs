using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.jipiao.TicketStatistics
{
    /// <summary>
    /// 出票统计 出票量
    /// 功能：显示出票量列表
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-25 
    /// </summary>
    public partial class TicketCount : BackPage
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        //需要合并的行数
        protected int rowSpan = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //编号ID
                int id = Utils.GetInt(Utils.GetQueryStringValue("id"));
                //开始日期
                DateTime? beginDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("beginDate"));
                //借宿日期
                DateTime? endDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endDate"));
                //航空公司ID 多个时以 , 隔开
                string areaId = Utils.GetQueryStringValue("areaId");
                //部门ID 多个时以 , 隔开
                string departId = Utils.GetQueryStringValue("departId");
                //出团日期开始
                DateTime? leaDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateS"));
                //出团日期结束
                DateTime? leaDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateE"));
                //出票处名称
                string officeName = Utils.GetQueryStringValue("officeName");

                //当前页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                //if (id != 0)
                {
                    //页面初始化
                    DataInit(id, departId, areaId, beginDate, endDate, leaDateStart, leaDateEnd, officeName);
                }
            }
        }

        //标记该页面为弹窗打开
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }


        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departIds"></param>
        /// <param name="areaIds"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="leaDateStart"></param>
        /// <param name="leaDateEnd"></param>
        /// <param name="officeName"></param>
        protected void DataInit(int id, string departIds, string areaIds, DateTime? beginDate, DateTime? endDate, DateTime? leaDateStart
            , DateTime? leaDateEnd, string officeName)
        {
            //部门ID 的集合
            int[] departIdList = JiPiao_TuiList.GetIntArrByStr(departIds);
            //航空公司 ID 集合
            int[] areaIdList = JiPiao_TuiList.GetIntArrByStr(areaIds);

            //声明查询Model
            EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti searchModel = new EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti();
            //查询Model 赋值
            searchModel.CompanyId = SiteUserInfo.CompanyID;
            searchModel.DepartIds = departIdList;
            searchModel.AirLineIds = areaIdList;
            searchModel.StartTicketOutTime = beginDate;
            searchModel.EndTicketOutTime = endDate;
            searchModel.OfficeId = id;
            searchModel.LeaveDateStart = leaDateStart;
            searchModel.LeaveDateEnd = leaDateEnd;
            searchModel.OfficeName = officeName;

            //声明机票 bll 操作对象
            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket(SiteUserInfo);
            //声明出票量集合对象

            IList<EyouSoft.Model.PlanStructure.TicketOutStatisticInfo> list = bll.GetTicketOutStatisticList(pageSize, pageIndex, ref recordCount, searchModel);
            //判断list内数据条数大于0
            if (list != null && list.Count > 0)
            {
                //绑定控件
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                //设置分页
                BindPage();
                //隐藏提示
                this.lblMsg.Visible = false;
            }
            else
            {
                //隐藏分页
                this.ExportPageInfo1.Visible = false;
                //显示提示
                this.lblMsg.Visible = true;
            }
        }

        /// <summary>
        /// 获得航班信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected string GetTdByAreaCompany(object list)
        {
            string str = "";
            if (list != null)
            {
                IList<EyouSoft.Model.PlanStructure.TicketFlight> ticketFlightList = (List<EyouSoft.Model.PlanStructure.TicketFlight>)list;
                //设置合并行数量

                if (ticketFlightList.Count > 0)
                {
                    rowSpan = ticketFlightList.Count;
                    for (int i = 0; i < ticketFlightList.Count; i++)
                    {
                        //拼接HTML

                        str += "<tr><td align=\"center\" width=\"67\" class=\"FlightTD\">" + ticketFlightList[i].AireLine.ToString() + "</td><td class=\"FlightTD\" align=\"center\" width=\"67\">" + ticketFlightList[i].FligthSegment + "</td><td class=\"FlightTD\" align=\"center\" width=\"67\">" + ticketFlightList[i].DepartureTime.ToString("yyyy-MM-dd") + "</td><td class=\"FlightTD\" align=\"center\" width=\"67\">" + ticketFlightList[i].TicketTime + "</td></tr>";

                    }
                }
            }
            return str;
        }


        /// <summary>
        /// 获得成人 儿童的票款 信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected string GetTdTicketInfo(object list)
        {
            string str = "";
            if (list != null)
            {
                //票款集合
                IList<EyouSoft.Model.PlanStructure.TicketKindInfo> ticketKindList = (List<EyouSoft.Model.PlanStructure.TicketKindInfo>)list;
                if (ticketKindList != null)
                {
                    if (ticketKindList.Count >= 2)
                    {
                        str = "<td align=\"center\" >" + Utils.FilterEndOfTheZeroString(ticketKindList[0].Price.ToString("0.00")) + "/" + Utils.FilterEndOfTheZeroString(ticketKindList[1].Price.ToString("0.00")) + "</td><td align=\"center\" >" + Utils.FilterEndOfTheZeroString(ticketKindList[0].OilFee.ToString("0.00")) + "/" + Utils.FilterEndOfTheZeroString(ticketKindList[1].OilFee.ToString("0.00")) + "</td><td align=\"center\" >" + Utils.FilterEndOfTheZeroString(ticketKindList[0].AgencyPrice.ToString("0.00")) + "/" + Utils.FilterEndOfTheZeroString(ticketKindList[1].AgencyPrice.ToString("0.00")) + "</td><td align=\"center\" >" + Utils.FilterEndOfTheZeroString(ticketKindList[0].TotalMoney.ToString("0.00")) + "/" + Utils.FilterEndOfTheZeroString(ticketKindList[1].TotalMoney.ToString("0.00")) + "</td><td align=\"center\" >" + Utils.FilterEndOfTheZeroString(ticketKindList[0].PeopleCount.ToString("0.00")) + "/" + Utils.FilterEndOfTheZeroString(ticketKindList[1].PeopleCount.ToString("0.00")) + "</td>";
                    }
                }
            }
            return str;
        }

        #region 设置分页
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion




    }
}
