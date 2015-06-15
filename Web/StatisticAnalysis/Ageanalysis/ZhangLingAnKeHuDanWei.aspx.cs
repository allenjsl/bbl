using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Common.Enum;

namespace Web.StatisticAnalysis.Ageanalysis
{
    /// <summary>
    /// 统计分析-账龄分析-按客户单位
    /// </summary>
    /// 汪奇志 2012-02-23
    public partial class ZhangLingAnKeHuDanWei : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("istoxls") == "1")
            {
                ToXls();
            }

            if (!CheckGrant(TravelPermission.统计分析_帐龄分析_帐龄分析栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_帐龄分析_帐龄分析栏目, true);
                return;
            }

            BindZhangLingAnKeHuDanWei();
        }

        #region private members
        /// <summary>
        /// 绑定统计分析账龄分析按客户单位
        /// </summary>
        private void BindZhangLingAnKeHuDanWei()
        {
            int pageSize = 20;
            int pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"),1);
            int recordCount = 0;

            var searchInfo = new EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun();

            searchInfo.KeHuName = Utils.GetQueryStringValue("kehumingcheng");
            searchInfo.SortType = Utils.GetInt(Utils.GetQueryStringValue("sorttype"));
            searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));

            EyouSoft.BLL.StatisticStructure.AccountAgeStatistic bll = new EyouSoft.BLL.StatisticStructure.AccountAgeStatistic(SiteUserInfo);
            var items = bll.GetZhangLingAnKeHuDanWei(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, searchInfo);

            if (items != null && items.Count > 0)
            {
                ph.Visible = true;
                phEmpty.Visible = false;
                rpt.DataSource = items;
                rpt.DataBind();

                decimal weiShouKuanHeJi;
                bll.GetZhangLingAnKeHuDanWei(CurrentUserCompanyID, searchInfo, out weiShouKuanHeJi);

                ltrWeiShouHeJi.Text = weiShouKuanHeJi.ToString("C2");

                this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                this.ExportPageInfo1.UrlParams = Request.QueryString;
                this.ExportPageInfo1.intPageSize = pageSize;
                this.ExportPageInfo1.CurrencyPage = pageIndex;
                this.ExportPageInfo1.intRecordCount = recordCount;
            }
            else
            {
                ph.Visible = false;
                phEmpty.Visible = true;
            }

            RegisterScript(string.Format("var recordCount={0};", recordCount));
        }

        /// <summary>
        /// to xls
        /// </summary>
        private void ToXls()
        {
            if (!CheckGrant(TravelPermission.统计分析_帐龄分析_帐龄分析栏目)) ResponseToXls(string.Empty);

            int pageSize = Utils.GetInt(Utils.GetQueryStringValue("recordcount"));

            if (pageSize < 1) ResponseToXls(string.Empty);

            int recordCount = 0;
            var searchInfo = new EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun();

            searchInfo.KeHuName = Utils.GetQueryStringValue("kehumingcheng");
            searchInfo.SortType = Utils.GetInt(Utils.GetQueryStringValue("sorttype"));
            searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));

            EyouSoft.BLL.StatisticStructure.AccountAgeStatistic bll = new EyouSoft.BLL.StatisticStructure.AccountAgeStatistic(SiteUserInfo);
            var items = bll.GetZhangLingAnKeHuDanWei(pageSize, 1, ref recordCount, CurrentUserCompanyID, searchInfo);

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            if (items != null && items.Count > 0)
            {
                s.Append("客户单位\t拖欠款总额\t最长拖欠时间\n");

                foreach (var item in items)
                {
                    s.AppendFormat("{0}\t{1}\t{2}\n", item.KeHuName, item.WeiShouKuan.ToString("C2"), item.EarlyTime.ToString("yyyy-MM-dd"));
                }
            }

            ResponseToXls(s.ToString());
        }
        #endregion
    }
}
