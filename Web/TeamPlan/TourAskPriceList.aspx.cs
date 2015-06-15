using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using Common.Enum;

namespace Web.TeamPlan
{
    /// <summary>
    /// 创建:万俊
    /// 功能:组团社询价列表
    /// </summary>
    public partial class TourAskPriceList : BackPage
    {
        #region 分页变量
        protected int pageSize = 15;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        EyouSoft.BLL.TourStructure.LineInquireQuoteInfo lineTourBll = new EyouSoft.BLL.TourStructure.LineInquireQuoteInfo();    //询价报价BLL
        protected void Page_Load(object sender, EventArgs e)
        {
            //询价权限判断
            if (!CheckGrant(TravelPermission.团队计划_组团社询价_栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.团队计划_组团社询价_栏目, true);
            }
            if (!IsPostBack)
            {
                BindQuote();
            }
        }

        #region 数据绑定
        protected void BindQuote()
        {
            EyouSoft.Model.TourStructure.LineInquireQuoteSearchInfo searchInfo = new EyouSoft.Model.TourStructure.LineInquireQuoteSearchInfo();
            searchInfo.RouteName = Utils.GetQueryStringValue("teamName");                           //获取团队名称  
            searchInfo.TourNo = Utils.GetQueryStringValue("teamNum");                               //获取团号
            searchInfo.DayNum = Utils.GetInt(Utils.GetQueryStringValue("dayCount"), 0);               //获取天数
            searchInfo.SDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("begTime"));      //获取出团日期区间_开始
            searchInfo.EDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endTime"));      //获取出团日期区间_结束
            searchInfo.XunTuanSTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("askbegin"));
            searchInfo.XunTuanETime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("askend"));

            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

            IList<EyouSoft.Model.TourStructure.LineInquireQuoteInfo> lineQuotes = lineTourBll.GetInquireList(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, false, searchInfo);
            //人数合计
            int peopleSum = 0;
            lineTourBll.GetInquireListHeJi(SiteUserInfo.CompanyID, searchInfo, out peopleSum);
            peopleCount.Text = peopleSum.ToString();
            this.rpTour.DataSource = lineQuotes;
            this.rpTour.DataBind();
            this.BindPage();
            this.txt_beginDate.Value = Utils.GetQueryStringValue("begTime");
            this.txt_dayCount.Value = Utils.GetQueryStringValue("dayCount");
            this.txt_endDate.Value = Utils.GetQueryStringValue("endTime");
            this.txt_teamName.Value = Utils.GetQueryStringValue("teamName");
            this.txt_teamNum.Value = Utils.GetQueryStringValue("teamNum");
            this.txt_AskBegin.Value = Utils.GetQueryStringValue("askbegin");
            this.txt_AskEnd.Value = Utils.GetQueryStringValue("askend");


        }
        #endregion


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
