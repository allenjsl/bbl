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
using EyouSoft.Common;
using System.Collections.Generic;
using System.Text;

namespace Web.SupplierControl.AreaConnect
{
    public partial class TradeSituation : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        protected int len = 0;
        protected int tid = 0;
        protected string act = string.Empty;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_地接_栏目, false);
            }
            if (!IsPostBack)
            {
                if (Utils.GetQueryStringValue("tid") != "")
                {
                    tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
                    bind(tid);
                }

                act = Utils.GetQueryStringValue("act");
                //GetToursJPGYS
                if (act == "toexcel")
                {
                    Response.Clear();
                    Response.Write(toexcel(tid));
                    Response.End();
                }


            }
        }

        private string toexcel(int tid)
        {

            IList<EyouSoft.Model.TourStructure.LBGYSTours> list = null;
            EyouSoft.BLL.TourStructure.Tour tBll = new EyouSoft.BLL.TourStructure.Tour();
            list = tBll.GetToursGYS(CurrentUserCompanyID, 1, 1, ref recordCount, tid);
            if (recordCount != 0)
            {
                list = tBll.GetToursGYS(CurrentUserCompanyID, recordCount, 1, ref recordCount, tid);
            }
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + "area" + DateTime.Now.ToShortDateString() + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<table border='1'><tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th><th>{5}</th><th>{6}</th><tr>", "团号", "线路名称", "出团日期", "人数", "计调员", "返利", "结算费用");
            foreach (EyouSoft.Model.TourStructure.LBGYSTours cs in list)
            {
                sb.AppendFormat("<td >{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><tr>",
                    "　" + cs.TourCode + "　",
                    cs.RouteName,
                    cs.LDate.ToString("yyyy-MM-dd"),
                    cs.PlanPeopleNumber,
                    cs.PlanNames,
                    cs.CommissionAmount,
                    cs.SettlementAmount);
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        private void bind(int tid)
        {

            //初使化条件
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchinfo = new EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo();
            DateTime? SDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("date"));
            DateTime? LDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LDate"));
            int? PayStatus = Utils.GetIntNull(Utils.GetQueryStringValue("status"));
            if (SDate != null)
            {
                this.txt_Date.Value = Convert.ToDateTime(SDate).ToString("yyyy-MM-dd");
            }
            searchinfo.SDate = SDate;
            if (LDate != null)
            {
                this.txt_Date1.Value = Convert.ToDateTime(LDate).ToString("yyyy-MM-dd");
            }
            searchinfo.EDate = LDate;
            if (SeleState.Items.FindByValue(PayStatus.ToString()) != null)
            {
                SeleState.Items.FindByValue(PayStatus.ToString()).Selected = true;
            }
            searchinfo.PayStatus = PayStatus;

            IList<EyouSoft.Model.TourStructure.LBGYSTours> list = null;
            EyouSoft.BLL.TourStructure.Tour tBll = new EyouSoft.BLL.TourStructure.Tour();
            list = tBll.GetToursGYS(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, tid, searchinfo);            
            len = list == null ? 0 : list.Count;
            this.repList.DataSource = list;
            this.repList.DataBind();
            //设置合计
            EyouSoft.Model.SupplierStructure.MTimesSummaryDiJieInfo djInfoModel = new EyouSoft.BLL.CompanyStructure.CompanySupplier().GetTimesSummaryDiJie(SiteUserInfo.CompanyID, tid,searchinfo);
            if (djInfoModel != null)
            {
                this.lblCommissionAmount.Text = Utils.FilterEndOfTheZeroString(djInfoModel.CommAmount.ToString("0.00"));
                this.lblNotPayAmount.Text = Utils.FilterEndOfTheZeroString(djInfoModel.NotPayAmount.ToString("0.00"));
                this.lblSettlementAmount.Text = Utils.FilterEndOfTheZeroString(djInfoModel.TotalAmount.ToString("0.00"));
                this.lblPeopleNum.Text = djInfoModel.PeopleNumber.ToString();
            }

            //设置分页
            BindPage();
        }

        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
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

       

    }
}
