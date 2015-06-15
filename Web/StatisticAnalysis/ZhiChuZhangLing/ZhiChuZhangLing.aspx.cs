using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.StatisticAnalysis.ZhiChuZhangLing
{
    /// <summary>
    /// 支出账龄分析
    /// </summary>
    /// <remarks>
    /// 原始需求：按供应商统计，一个供应商一行，统计欠款总额，最长拖欠时间，并可按拖欠额及拖欠时间排序。
    /// </remarks>
    /// 汪奇志 2012-02-28
    public partial class ZhiChuZhangLing : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("istoxls") == "1")
            {
                ToXls();
            }

            if (!CheckGrant(global::Common.Enum.TravelPermission.统计分析_支出账龄分析_支出账龄分析栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.统计分析_支出账龄分析_支出账龄分析栏目, false);
            }

            InitZhiChuZhangLingFenXi();
        }

        #region private members
        /// <summary>
        /// 初始化支出账龄分析
        /// </summary>
        private void InitZhiChuZhangLingFenXi()
        {
            int pageSize = 20;
            int pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            int recordCount = 0;

            var chaXun = GetSearchInfo();
            var bll = new EyouSoft.BLL.StatisticStructure.AccountAgeStatistic(SiteUserInfo);
            
            var items = bll.GetZhiChuZhangLing(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, chaXun);

            if (items != null && items.Count > 0)
            {
                phFXB.Visible = true;
                phEmpty.Visible = false;

                rptZhiChuZhangLing.DataSource = items;
                rptZhiChuZhangLing.DataBind();

                ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                ExportPageInfo1.UrlParams = Request.QueryString;
                ExportPageInfo1.intPageSize = pageSize;
                ExportPageInfo1.CurrencyPage = pageIndex;
                ExportPageInfo1.intRecordCount = recordCount;

                decimal weiFuKuanHeJi;
                decimal zongJinEHeJi;
                bll.GetZhiChuZhangLing(CurrentUserCompanyID, chaXun, out weiFuKuanHeJi, out zongJinEHeJi);
                ltrWeiFuKuanHeJi.Text = weiFuKuanHeJi.ToString("C2");
                ltrZongJinEHeJi.Text = zongJinEHeJi.ToString("C2");
            }
            else
            {
                phFXB.Visible = false;
                phEmpty.Visible = true;
            }

            RegisterScript(string.Format("var recordCount={0};", recordCount));
        }

        /// <summary>
        /// to xls
        /// </summary>
        private void ToXls()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.统计分析_支出账龄分析_支出账龄分析栏目)) ResponseToXls(string.Empty);

            int pageSize = Utils.GetInt(Utils.GetQueryStringValue("recordcount"));
            int recordCount = 0;

            if (pageSize < 1) ResponseToXls(string.Empty);

            var chaXun = GetSearchInfo();
            var items = new EyouSoft.BLL.StatisticStructure.AccountAgeStatistic(SiteUserInfo).GetZhiChuZhangLing(pageSize, 1, ref recordCount, CurrentUserCompanyID, chaXun);

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            if (items != null && items.Count > 0)
            {
                s.Append("供应商\t\t交易总额\t\t欠款总额\t\t最早拖欠时间\n");

                foreach (var item in items)
                {
                    s.AppendFormat("[{0}] {1}\t\t{2}\t\t{3}\t\t{4}\n", item.GongYingShangLeiXing, item.GongYingShang, item.JiaoYiZongE.ToString("C2"), item.QianKuanZongE.ToString("C2"), item.EarlyTime.ToString("yyyy-MM-dd"));
                }
            }

            ResponseToXls(s.ToString());
        }

        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <returns></returns>
        EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingChaXunInfo GetSearchInfo()
        {
            var info = new EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingChaXunInfo();

            info.GongYingShang = Utils.GetQueryStringValue("gongyingshang");
            info.SortType = Utils.GetInt(Utils.GetQueryStringValue("sorttype"));
            info.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            info.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            info.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
            info.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");
            info.GongYingShangLeiXing = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), Utils.GetQueryStringValue("gongyingshangleixing"), null);

            return info;
        }
        #endregion

    }
}
