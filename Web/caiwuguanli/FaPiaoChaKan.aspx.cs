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
    public partial class FaPiaoChaKan : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("dotype") == "delete")
            {
                Delete();
            }

            if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_发票管理_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_发票管理_栏目, false);
            }

            InitLB();
        }

        #region private memebers
        /// <summary>
        /// 删除
        /// </summary>
        void Delete()
        {
            string delId = Utils.GetQueryStringValue("delid");

            if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_发票管理_删除))
            {
                RCWE("-1");
            }

            if (string.IsNullOrEmpty(delId))
            {
                RCWE("-2");
            }

            if (new EyouSoft.BLL.FinanceStructure.BFaPiao().Delete(delId) == 1)
            {
                RCWE("1");
            }

            RCWE("-3");
        }

        /// <summary>
        /// 初始化发票登记列表
        /// </summary>
        void InitLB()
        {
            int pageSize = 12;
            int pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            int recordCount = 0;
            int keHuDanWeiId=Utils.GetInt(Utils.GetQueryStringValue("kehudanweiid"));
            var searchInfo = new EyouSoft.Model.FinanceStructure.MFaPiaoSearchInfo();

           
            searchInfo.KPETime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("kpetime"));
            searchInfo.KPRen = Utils.GetQueryStringValue("kaipiaoren");
            searchInfo.KPSTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("kpstime"));

            var bll = new EyouSoft.BLL.FinanceStructure.BFaPiao();
            var items = bll.GetFaPiaos(CurrentUserCompanyID, keHuDanWeiId, pageSize, pageIndex, ref recordCount, searchInfo);

            if (items != null && items.Count > 0)
            {
                phLB.Visible = true;
                phEmpty.Visible = false;

                rptLB.DataSource = items;
                rptLB.DataBind();

                paging.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                paging.UrlParams = Request.QueryString;
                paging.intPageSize = pageSize;
                paging.CurrencyPage = pageIndex;
                paging.intRecordCount = recordCount;
                decimal kaiPiaoJinE;
                bll.GetFaPiaosHeJi(CurrentUserCompanyID,keHuDanWeiId, searchInfo, out kaiPiaoJinE);
                ltrKaiPiaoJinEHeJi.Text = kaiPiaoJinE.ToString("C2");
            }
            else
            {
                phLB.Visible = false;
                phEmpty.Visible = true;
            }

            bll = null;
        }
        #endregion

        #region protected members
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }
        #endregion
    }
}
