using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.CRM.customerinfos
{   
    /// <summary>
    /// 客户交易情况
    /// xuty 2011/1/20
    /// </summary>
    /// 修改：邵权江 2012-1-5 添加出团时间查询
    public partial class CustomerTrade : Eyousoft.Common.Page.BackPage
    {
        protected int PageSize = 20;
        protected int PageIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("istoxls") == "1")
            {
                ToXls();
            }

            InitMingXi();
        }

        #region private members
        /// <summary>
        /// init mingxi
        /// </summary>
        private void InitMingXi()
        {
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            if (PageIndex < 1) PageIndex = 1;

            int recordCount = 0;
            int keHuId = Utils.GetInt(Utils.GetQueryStringValue("cid"));
            var chaXun = new EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiChaXunInfo();

            chaXun.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            chaXun.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));

            //chaXun.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), Utils.GetQueryStringValue("TourType"), null);

            string tourtype = Utils.GetQueryStringValue("TourType");
            if (tourtype == "0")
            {
                chaXun.TourTypes = new EyouSoft.Model.EnumType.TourStructure.TourType[] { EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划 };
            }
            else if(tourtype=="1")
            {
                chaXun.TourTypes = new EyouSoft.Model.EnumType.TourStructure.TourType[] { EyouSoft.Model.EnumType.TourStructure.TourType.团队计划 };
            }
            else if (tourtype == "0,1")
            {
                chaXun.TourTypes = new EyouSoft.Model.EnumType.TourStructure.TourType[] { EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, EyouSoft.Model.EnumType.TourStructure.TourType.团队计划 };
            }

            chaXun.QueryAmountType = EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.未收款;
            chaXun.QueryAmountOperator = (EyouSoft.Model.EnumType.FinanceStructure.QueryOperator)Utils.GetInt(Utils.GetQueryStringValue("queryAmountOperator"));
            chaXun.QueryAmount = Utils.GetDecimal(Utils.GetQueryStringValue("queryAmount"));
            if (chaXun.AreaId.HasValue && chaXun.AreaId.Value < 1) chaXun.AreaId = null;

            var items = new EyouSoft.BLL.TourStructure.TourOrder().GetKeHuJiaoYiMingXi(PageSize, PageIndex, ref recordCount, CurrentUserCompanyID, keHuId, chaXun);

            int pageCount = recordCount / PageSize;
            if (recordCount % PageSize > 0) pageCount++;
            if (PageIndex > pageCount) PageIndex = pageCount;
            if (PageIndex < 1) PageIndex = 1;

            if (items != null && items.Count > 0)
            {
                ExportPageInfo1.Visible = true;
                phHeJi.Visible = true;

                rptTrade.DataSource = items;
                rptTrade.DataBind();

                ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                ExportPageInfo1.UrlParams = Request.QueryString;
                ExportPageInfo1.intPageSize = PageSize;
                ExportPageInfo1.CurrencyPage = PageIndex;
                ExportPageInfo1.intRecordCount = recordCount;

                //底部合计
                int renCi;
                decimal jiaoYiJinE, yiShouJinE;
                new EyouSoft.BLL.TourStructure.TourOrder().GetKeHuJiaoYiMingXiHuiZong(CurrentUserCompanyID, keHuId, out renCi, out jiaoYiJinE, out yiShouJinE, chaXun);

                ltrRenCiHeJi.Text = renCi.ToString();
                ltrJiaoYiJinEHeJi.Text = jiaoYiJinE.ToString("C2");
                ltrYiShouJinEHeJi.Text = yiShouJinE.ToString("C2");
                ltrWeiShouJinEHeJi.Text = (jiaoYiJinE - yiShouJinE).ToString("C2");
            }
            else
            {
                rptTrade.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无交易信息！</td></tr>";
                ExportPageInfo1.Visible = false;
                phHeJi.Visible = false;
            }

            RegisterScript(string.Format("var recordCount={0};", recordCount));
        }

        /// <summary>
        /// to xls
        /// </summary>
        private void ToXls()
        {            
            int pageSize = Utils.GetInt(Utils.GetQueryStringValue("recordcount"));

            if (pageSize < 1) ResponseToXls(string.Empty);

            int recordCount = 0;
            int keHuId = Utils.GetInt(Utils.GetQueryStringValue("cid"));
            var chaXun = new EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiChaXunInfo();

            chaXun.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            chaXun.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            //chaXun.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), Utils.GetQueryStringValue("TourType"), null);

            string tourtype = Utils.GetQueryStringValue("TourType");
            if (tourtype == "0")
            {
                chaXun.TourTypes = new EyouSoft.Model.EnumType.TourStructure.TourType[] { EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划 };
            }
            else if (tourtype == "1")
            {
                chaXun.TourTypes = new EyouSoft.Model.EnumType.TourStructure.TourType[] { EyouSoft.Model.EnumType.TourStructure.TourType.团队计划 };
            }
            else if (tourtype == "0,1")
            {
                chaXun.TourTypes = new EyouSoft.Model.EnumType.TourStructure.TourType[] { EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, EyouSoft.Model.EnumType.TourStructure.TourType.团队计划 };
            }

            var items = new EyouSoft.BLL.TourStructure.TourOrder().GetKeHuJiaoYiMingXi(pageSize, 1, ref recordCount, CurrentUserCompanyID, keHuId, chaXun);

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            if (items != null && items.Count > 0)
            {
                s.AppendFormat("团号\t线路名称\t订单号\t对方操作员\t对方团号\t报团人次\t交易金额\t已收金额\t欠款金额\n");

                foreach (var item in items)
                {
                    s.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\n", item.TourCode, item.RouteName, item.OrderCode,item.BuyerContactName, item.BuyerTourCode, item.RenCi, item.JiaoYiJInE.ToString("C2"), item.YiShouJinE.ToString("C2"), item.WeiShouJinE.ToString("C2"));
                }
            }

            ResponseToXls(s.ToString());
        }
        #endregion
    }
}
