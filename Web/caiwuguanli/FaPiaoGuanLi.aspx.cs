using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 财务管理-发票管理
    /// </summary>
    public partial class FaPiaoGuanLi : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_发票管理_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_发票管理_栏目, false);
            }

            InitLB();
        }

        #region private members
        /// <summary>
        /// 初始化发票管理列表
        /// </summary>
        void InitLB()
        {
            int pageSize = 20;
            int pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            int recordCount = 0;
            var searchInfo = new EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiSearchInfo();

            searchInfo.CrmName = Utils.GetQueryStringValue("kehumingcheng");
            searchInfo.CTETime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ctetime"));
            searchInfo.CTSTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ctstime"));
            searchInfo.KPETime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("kpetime"));
            searchInfo.KPRen = Utils.GetQueryStringValue("kaipiaoren");
            searchInfo.KPSTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("kpstime"));

            var bll = new EyouSoft.BLL.FinanceStructure.BFaPiao();
            var items = bll.GetFaPiaoGuanLis(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, searchInfo);

            if (items != null && items.Count > 0)
            {
                phLB.Visible = true;
                phEmpty.Visible = false;

                rptFaPiaoGuanLi.DataSource = items;
                rptFaPiaoGuanLi.DataBind();

                paging.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                paging.UrlParams = Request.QueryString;
                paging.intPageSize = pageSize;
                paging.CurrencyPage = pageIndex;
                paging.intRecordCount = recordCount;

                decimal jiaoYiJinE;
                decimal kaiPiaoJinE;
                bll.GetFaPiaoGuanLisHeJi(CurrentUserCompanyID, searchInfo, out jiaoYiJinE, out kaiPiaoJinE);
                ltrJiaoYiJinEHeJi.Text = jiaoYiJinE.ToString("C2");
                ltrKaiPiaoJinEHeJi.Text = kaiPiaoJinE.ToString("C2");
                ltrWeiKaiPiaoJinEHeJi.Text = (jiaoYiJinE - kaiPiaoJinE).ToString("C2");
            }
            else
            {
                phLB.Visible = false;
                phEmpty.Visible = true;
            }

            bll = null;
        }
        #endregion
    }
}
