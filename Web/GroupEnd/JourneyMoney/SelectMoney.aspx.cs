using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.GroupEnd.JourneyMoney
{
    /// <summary>
    /// 行程报价-我的询价
    /// 柴逸宁
    /// </summary>
    public partial class SelectMoney : Eyousoft.Common.Page.FrontPage
    {

        #region 变量

        protected int len = 0;//列表长度

        protected int pageSize = 20;
        protected int pageIndex;
        protected int recordCount;


        //BLL

        //Model

        protected EyouSoft.Model.TourStructure.LineInquireQuoteInfo tsModel = null;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            tsModel = new EyouSoft.Model.TourStructure.LineInquireQuoteInfo();
            string act = string.Empty;
            act = EyouSoft.Common.Utils.GetQueryStringValue("act");

            if (!IsPostBack)
            {
                switch (act)
                {
                    case "del":

                        AreaDel();//删除操作

                        break;
                    default:
                        bind();
                        break;
                }
            }




        }

        #region 绑定列表数据

        protected void bind()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            EyouSoft.BLL.TourStructure.LineInquireQuoteInfo tsBLL = new EyouSoft.BLL.TourStructure.LineInquireQuoteInfo();
            EyouSoft.Model.TourStructure.LineInquireQuoteSearchInfo SearchInfo = null;

            IList<EyouSoft.Model.TourStructure.LineInquireQuoteInfo> GetInquireList;
            GetInquireList = tsBLL.GetInquireList(SiteUserInfo.TourCompany.TourCompanyId, pageSize, pageIndex, ref recordCount, true, SearchInfo);
            //绑定
            retList.DataSource = GetInquireList;
            retList.DataBind();
            GetInquireList = null;
            BindPage();
        }
        #endregion
        /// <summary>
        /// 删除数据
        /// </summary>
        private void AreaDel()
        {
            EyouSoft.BLL.TourStructure.LineInquireQuoteInfo tsBLL = new EyouSoft.BLL.TourStructure.LineInquireQuoteInfo();
            string[] stid = Utils.GetFormValue("tid").Split(',');
            int[] tid = new int[stid.Length];
            for (int i = 0; i < stid.Length; i++)
            {
                tid[i] = Utils.GetInt(stid[i]);
            }
            bool res = false;
            res = tsBLL.DelInquire(SiteUserInfo.CompanyID, tid);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
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
