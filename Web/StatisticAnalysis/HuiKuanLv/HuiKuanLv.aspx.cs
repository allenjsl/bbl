using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.StatisticAnalysis.HuiKuanLv
{
    /// <summary>
    /// 统计分析-回款率分析
    /// </summary>
    /// 汪奇志 2012-02-24
    public partial class HuiKuanLv : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetFormValue("istoxls") == "1")
            {
                ToXls();
            }

            if (!CheckGrant(global::Common.Enum.TravelPermission.统计分析_回款率分析_回款率分析栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.统计分析_回款率分析_回款率分析栏目, false);
            }

            InitHuiKuanLv();
        }

        #region private members
        /// <summary>
        /// init huikuanlv
        /// </summary>
        private void InitHuiKuanLv()
        {
            EyouSoft.Model.StatisticStructure.HuiKuanLvChaXunInfo searchInfo =new EyouSoft.Model.StatisticStructure.HuiKuanLvChaXunInfo();

            searchInfo.KeHuDanWei = Utils.GetQueryStringValue("kehumingcheng");
            searchInfo.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            searchInfo.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            searchInfo.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
            searchInfo.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("sellerids"), ",");
            searchInfo.SFBHWeiShenHe = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetHuiKuanLvSFBHWeiShenHe(CurrentUserCompanyID);
            string departNames = Utils.GetQueryStringValue("departnames");
            string sellerNames = Utils.GetQueryStringValue("sellernames");

            DateTime now = DateTime.Now;

            if (!searchInfo.LSDate.HasValue && !searchInfo.LEDate.HasValue)
            {
                searchInfo.LSDate = new DateTime(now.Year, now.Month, 1);
                searchInfo.LEDate = searchInfo.LSDate.Value.AddMonths(1).AddDays(-1);
            }

            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? tongJiDingDanFangShi = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (tongJiDingDanFangShi.HasValue)
            {
                searchInfo.TongJiDingDanFangShi = tongJiDingDanFangShi.Value;
            }

            EyouSoft.BLL.StatisticStructure.HuiKuanLv bll = new EyouSoft.BLL.StatisticStructure.HuiKuanLv();
            var items = bll.GetHuiKuanLv(CurrentUserCompanyID, now, 13, searchInfo);
            bll = null;

            rptHuiKuanLv.DataSource = items;
            rptHuiKuanLv.DataBind();

            StringBuilder s = new StringBuilder();
            if (searchInfo.LSDate.HasValue && searchInfo.LEDate.HasValue)
            {
                s.AppendFormat("出团时间从{0}至{1}", searchInfo.LSDate.Value.ToString("yyyy-MM-dd"), searchInfo.LEDate.Value.ToString("yyyy-MM-dd"));
            }
            else if (searchInfo.LSDate.HasValue)
            {
                s.AppendFormat("出团时间在{0}以后", searchInfo.LSDate.Value.ToString("yyyy-MM-dd"));
            }
            else if (searchInfo.LEDate.HasValue)
            {
                s.AppendFormat("出团时间在{0}以前", searchInfo.LEDate.Value.ToString("yyyy-MM-dd"));
            }

            if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
            {
                s.AppendFormat("，部门：{0}", departNames);
            }

            if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
            {
                s.AppendFormat("，销售员：{0}", sellerNames);
            }

            if (!string.IsNullOrEmpty(searchInfo.KeHuDanWei))
            {
                s.AppendFormat("，客户单位：{0}", searchInfo.KeHuDanWei);
            }

            s.Append("。回款率信息如下：");

            ltrHuiKuanLvBiaoTi.Text = s.ToString();
        }

        /// <summary>
        /// to xls
        /// </summary>
        private void ToXls()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.统计分析_回款率分析_回款率分析栏目)) ResponseToXls(string.Empty);

            ResponseToXls(Request.Form["txtXlsHTML"],System.Text.Encoding.UTF8);
        }
        #endregion
    }
}
