using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.jipiao
{

    /// <summary>
    /// 退票统计列表
    /// 2011-04-20 戴银柱
    /// </summary>
    public partial class JiPiao_TuiList : Eyousoft.Common.Page.BackPage
    {

        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;

        /// <summary>
        /// 退回金额合计
        /// </summary>
        protected decimal RefundAmount;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //权限判断
            if (!CheckGrant(global::Common.Enum.TravelPermission.机票管理_机票管理_退票统计))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.机票管理_机票管理_退票统计, true);
            }

            if (!IsPostBack)
            {
                //团号
                string tourNo = Utils.GetQueryStringValue("teamNum");
                //线路
                string routeName = Server.UrlDecode(Utils.GetQueryStringValue("areaLine"));
                //出团日期开始
                DateTime? outStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("outStartDate"));
                //出团日期结束
                DateTime? outEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("outEndDate"));
                //航段
                string airSegment = Server.UrlDecode(Utils.GetQueryStringValue("airSegment"));
                //航空公司
                int airCompany = Utils.GetInt(Utils.GetQueryStringValue("airCompany"));
                //组团社ID
                int supplierId = Utils.GetInt(Utils.GetQueryStringValue("supplierId"));
                //组团社名
                string supplierName = Server.UrlDecode(Utils.GetQueryStringValue("supplierName"));
                //部门编号集合
                string strDepId = Utils.GetQueryStringValue("depId");
                //部门名称集合
                string strDepName = Utils.GetQueryStringValue("depName");
                //操作员编号集合
                string strOpId = Utils.GetQueryStringValue("opId");
                //操作员名称集合
                string strOpName = Utils.GetQueryStringValue("opName");

                //当前页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);


                #region 设置控件的值
                this.txtTeamNum.Text = tourNo;
                this.txtAreaLine.Text = routeName;
                this.txtOutDate.Text = outStartDate == null ? "" : Convert.ToDateTime(outStartDate).ToString("yyyy-MM-dd");
                txtLeaveDateEnd.Text = outEndDate == null ? string.Empty : Convert.ToDateTime(outEndDate).ToString("yyyy-MM-dd");
                this.txtAirSegment.Text = airSegment;
                AirCompanyInit(airCompany.ToString());
                this.hideSupplierId.Value = supplierId.ToString();
                this.txtGroupsName.Text = supplierName;
                SelectOperator.OperName = strOpName;
                SelectOperator.OperId = strOpId;
                UCselectDepart.GetDepartmentName = strDepName;
                UCselectDepart.GetDepartId = strDepId;
                #endregion

                //初始化方法
                DataInit(tourNo, routeName, outStartDate, outEndDate, airCompany, supplierId, supplierName, airSegment,
                         strDepId, strOpId);
            }
        }

        protected void DataInit(string tourNo, string routeName, DateTime? outStartDate, DateTime? outEndDate, int airCompany
            , int supplieriId, string supplierName, string airSegment, string strDepIds, string strOpIds)
        {
            EyouSoft.Model.StatisticStructure.MQueryRefundStatistic searchModel = new EyouSoft.Model.StatisticStructure.MQueryRefundStatistic();
            if (airCompany == 0)
            {
                searchModel.AireLine = null;
            }
            else
            {
                searchModel.AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)airCompany;
            }

            searchModel.BuyCompanyId = supplieriId;
            searchModel.BuyCompanyName = supplierName;
            searchModel.CompanyId = SiteUserInfo.CompanyID;
            searchModel.FligthSegment = airSegment;
            searchModel.LeaveDateEnd = outEndDate;
            searchModel.LeaveDateStart = outStartDate;
            searchModel.RouteName = routeName;
            searchModel.TourNo = tourNo;
            searchModel.DepIds = GetIntArrByStr(strDepIds);
            searchModel.OperatorIds = GetIntArrByStr(strOpIds);
            IList<EyouSoft.Model.StatisticStructure.MRefundStatistic> list = new EyouSoft.BLL.StatisticStructure.BRefundStatistic(SiteUserInfo, true).GetRefundStatistic(pageSize, pageIndex, ref recordCount, searchModel);

            if (list != null && list.Count > 0)
            {
                //计算
                new EyouSoft.BLL.StatisticStructure.BRefundStatistic(SiteUserInfo, true).GetSumRefundStatistic(
                    ref RefundAmount, searchModel);
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                this.lblMsg.Visible = false;
            }
            else
            {
                RefundAmount = 0M;
                this.lblMsg.Visible = true;
                this.ExporPageInfoSelect1.Visible = false;
            }
            BindPage();
        }

        protected string GetHtmlByList(object list, DateTime RegisterTime)
        {
            string str = "";
            if (list == null)
            {
                str = "<tr><td height=\"30\" width=\"110px\" style=\"border-right:solid 1px #fff;border-bottom:solid 0px #fff;\" >" + RegisterTime.ToString("yyyy-MM-dd") + "</td> <td style=\"border-bottom:solid 0px #fff\" width=\"100px\"></td></tr>";
            }
            else
            {

                IList<EyouSoft.Model.PlanStructure.TicketFlight> refundFlightList = (List<EyouSoft.Model.PlanStructure.TicketFlight>)list;
                if (refundFlightList != null && refundFlightList.Count > 0)
                {
                    for (int i = 0; i < refundFlightList.Count; i++)
                    {
                        str += "<tr><td height=\"30\" width=\"78px\" class=\"FlightTD\">" + refundFlightList[i].DepartureTime.ToString("yyyy-MM-dd") + "</td> <td class=\"FlightTD\" width=\"80px\">" + refundFlightList[i].FligthSegment + "</td> <td class=\"FlightTD\" width=\"100px\">" + refundFlightList[i].TicketTime + "</td><td class=\"FlightTD\" width=\"103px\">" + refundFlightList[i].AireLine.ToString() + "</td><td class=\"FlightTD\" width=\"50px\">" + Utils.FilterEndOfTheZeroString(refundFlightList[i].Discount.ToString("0.00")) + "%</td></tr>";
                    }

                }
                else
                {
                    str += "<tr><td height=\"30\" width=\"78px\" class=\"FlightTD\"></td> <td class=\"FlightTD\" width=\"80px\"></td> <td class=\"FlightTD\" width=\"100px\"></td><td class=\"FlightTD\" width=\"103px\"></td><td class=\"FlightTD\" width=\"50px\"></td></tr>";
                }
            }
            return str;
        }


        /// <summary>
        /// 绑定航空公司
        /// </summary>
        /// <param name="selectVal"></param>
        protected void AirCompanyInit(string selectVal)
        {
            //清空下拉框的值
            this.ddlAirCompany.Items.Clear();
            //添加默认项
            this.ddlAirCompany.Items.Add(new ListItem("--请选择--", "0"));
            //获得航空公司枚举值的list
            IList<EyouSoft.Common.EnumObj> list = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.FlightCompany));

            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = list[i].Value;
                    item.Text = list[i].Text;
                    if (list[i].Value == selectVal)
                    {
                        item.Selected = true;
                    }
                    this.ddlAirCompany.Items.Add(item);
                }
            }
        }

        #region 设置分页
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        protected string getPiaoNum(object o)
        {
            IList<EyouSoft.Model.PlanStructure.TicketFlight> list = (List<EyouSoft.Model.PlanStructure.TicketFlight>)o;
            if (list != null && list.Count > 0)
            {
                return string.Join(",", list.Select(x => x.TicketNum).ToArray());
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据半角逗号分割的字符串生成int数组
        /// </summary>
        /// <param name="strIds">逗号分割的字符串</param>
        /// <returns></returns>
        internal static int[] GetIntArrByStr(string strIds)
        {
            if (string.IsNullOrEmpty(strIds))
                return null;

            var r = new List<int>();
            string[] strTmp = strIds.Split(',');

            foreach (var s in strTmp)
            {
                if (Utils.GetInt(s) > 0)
                {
                    r.Add(Utils.GetInt(s));
                }
            }
            return r.ToArray();
        }
    }



}
