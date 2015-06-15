using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EyouSoft.Common;
using Eyousoft.Common.Page;


namespace Web.caiwuguanli
{
    /// <summary>
    /// 机票审核页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    public partial class JiPiaoAudit : BackPage
    {
        #region 分页参数
        protected int pageSize = 20;
        protected int pageIndex = 1;
        int recordCount;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_机票审核_栏目))
            {
                EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_机票审核_栏目, false);
                return;
            }
            if (!IsPostBack)
            {
                onInit();
            }
        }

        private void onInit()
        {
            //页号
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //日期
            string inputDate = Utils.GetQueryStringValue("inputDate").Trim();
            //航段
            string inputGoLine = Utils.GetQueryStringValue("inputGoLine");
            //操作人ID
            string[] operIds = Utils.GetQueryStringValue("oper").Split(',');
            //计调员姓名
            string operName = Utils.GetQueryStringValue("operName").Trim();
            string tourCode = Utils.GetQueryStringValue("tourCode");
            this.selectOperator1.OperId = Utils.GetQueryStringValue("oper");
            this.selectOperator1.OperName = Utils.GetQueryStringValue("operName");
            DateTime? sVerifyTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("sVerifyTime"));
            DateTime? eVerifyTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("eVerifyTime"));
            DateTime? lSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            DateTime? lEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));

            ListBind(operIds, operName, inputGoLine, inputDate, tourCode, sVerifyTime, eVerifyTime, lSDate, lEDate);
        }

        //列表初始化
        protected void ListBind(string[] operIds, string operName, string inputGoLine, string inputDate, string tourCode, DateTime? sVerifyTime, DateTime? eVerifyTime, DateTime? lSDate, DateTime? lEDate)
        {
            int pageCount = 1;
            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket(SiteUserInfo);
            System.Collections.Generic.IList<EyouSoft.Model.PlanStructure.TicketInfo> list = null;
            EyouSoft.Model.PlanStructure.TicketSearchModel SearchModel = new EyouSoft.Model.PlanStructure.TicketSearchModel();
            if (!(string.IsNullOrEmpty(operName)
               && string.IsNullOrEmpty(inputDate)
               && string.IsNullOrEmpty(operName)
               && string.IsNullOrEmpty(inputGoLine)
               && string.IsNullOrEmpty(tourCode)
               && !sVerifyTime.HasValue
               && !eVerifyTime.HasValue
               && !lSDate.HasValue
               && !lEDate.HasValue))
            {
                SearchModel.TicketListOrFinancialList = 1;
                SearchModel.CompanyId = SiteUserInfo.CompanyID;
                SearchModel.Operator = operName;
                SearchModel.DepartureTime = inputDate;
                SearchModel.FligthSegment = inputGoLine;
                SearchModel.TourCode = tourCode;
                SearchModel.SVerifyTime = sVerifyTime;
                SearchModel.EVerifyTime = eVerifyTime;
                SearchModel.LSDate = lSDate;
                SearchModel.LEDate = lEDate;

                //根据条件搜索相关记录
                list = bll.SearchTicketOut(pageSize, pageIndex, SearchModel, ref recordCount, ref pageCount);
            }
            else
            {
                list = bll.GetCheckedTicketList(pageSize, pageIndex, SiteUserInfo.CompanyID, ref recordCount, ref pageCount);
            }
            if (list != null && list.Count > 0)
            {
                repList.DataSource = list;
                repList.DataBind();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
                this.repList.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='12' align='center'>暂时没有数据！</td></tr>";
            }
            BindPage();
        }

        /// <summary>
        /// 分页控件绑定
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }

        /// <summary>
        /// 获得机票的 时间、航段 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
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
                        if (i == (refundFlightList.Count - 1))
                        {
                            str += "<tr><td height=\"30\" width=\"110px\" style=\"border-right:solid 1px #fff;border-bottom:solid 0px #fff;\" >" + refundFlightList[i].DepartureTime.ToString("yyyy-MM-dd") + "</td> <td style=\"border-bottom:solid 0px #fff\" width=\"100px\">" + refundFlightList[i].FligthSegment + "</td></tr>";
                        }
                        else
                        {
                            str += "<tr><td height=\"30\" width=\"110px\" style=\"border-right:solid 1px #fff;border-bottom:solid 1px #fff;\" >" + refundFlightList[i].DepartureTime.ToString("yyyy-MM-dd") + "</td> <td style=\"border-bottom:solid 1px #fff\" width=\"100px\">" + refundFlightList[i].FligthSegment + "</td></tr>";
                        }

                    }

                }
                else
                {
                    str += "<tr><td height=\"30\" width=\"110px\" ></td> <td width=\"100px\"></td></tr>";
                }
            }

            return str;
        }

        #region 机票状态
        protected string TicketState()
        {
            string State = "";
            string Result = Utils.GetQueryStringValue("Result");
            if (Result != "" && !string.IsNullOrEmpty(Result))
            {
                if (Result == "OK")
                {
                    State = "审核通过";
                }
            }
            else
            {
                State = "机票申请";
            }
            return State;
        }
        #endregion
    }
}
