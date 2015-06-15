using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Model.EnumType;

namespace Web.Shop.T1
{
    /// <summary>
    /// 天天发计划
    /// </summary>
    /// 汪奇志 2012-04-03
    public partial class TianTianFa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitEverydayTours();
        }

        #region private members
        /// <summary>
        /// init everyday tours
        /// </summary>
        void InitEverydayTours()
        {
            int pageIndex = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("Page"),1);
            int recordCount=0;
            int pageSize = 10;

            var items = new EyouSoft.BLL.TourStructure.TourEveryday().GetTourEverydays(Master.CompanyId, pageSize, pageIndex, ref recordCount, null);
            if (items != null && items.Count > 0)
            {
                rpt.DataSource = items;
                rpt.DataBind();

                divPaging.Visible = true;
                divEmpty.Visible = false;

                paging.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                paging.UrlParams.Add(Request.QueryString);
                paging.intPageSize = pageSize;
                paging.CurrencyPage = pageIndex;
                paging.intRecordCount = recordCount;
            }
            else
            {
                divPaging.Visible = false;
                divEmpty.Visible = true;
            }
        }
        #endregion

        #region protected members
        /// <summary>
        /// 获得散拼计划的打印单
        /// </summary>
        /// <param name="tourid"></param>
        /// <param name="releaseType"></param>
        /// <returns></returns>
        protected string getUrl(string tourid, int releaseType)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            string s = string.Empty;
            if (releaseType == 0)
            {
                s = bll.GetPrintPath(Master.CompanyId, CompanyStructure.PrintTemplateType.组团线路标准发布行程单);
            }
            else
            {
                s = bll.GetPrintPath(Master.CompanyId, CompanyStructure.PrintTemplateType.组团线路快速发布行程单);
            }

            s += string.Format("?tourid={0}&action=tourevery", tourid);

            return s;
        }

        /// <summary>
        /// 获得附件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected string GetAttachs(object item)
        {
            string s = "<a href=\"javascript:void(0);\">无附件</a>";

            IList<EyouSoft.Model.TourStructure.TourAttachInfo> attachs = (List<EyouSoft.Model.TourStructure.TourAttachInfo>)item;
            if (attachs != null && attachs.Count > 0 && !string.IsNullOrEmpty(attachs[0].FilePath))
            {
                s = "<a href=\"" + attachs[0].FilePath + "\" target=\"_blank\">点击下载</a>";
            }

            return s;
        }
        #endregion
    }
}
