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
    /// 下载报价及行程
    /// 柴逸宁
    /// </summary>
    public partial class DownloadMoney : Eyousoft.Common.Page.FrontPage
    {
        #region 变量

        protected int len = 0;//列表长度

        protected int pageSize = 20;
        protected int pageIndex;
        protected int recordCount;
        //权限变量
        protected bool grantadd = false;//新增
        protected bool grantmodify = false;//修改
        protected bool grantdel = false;//删除


        //BLL

        //Model

        protected EyouSoft.Model.TourStructure.QuoteAttach tsModel = null;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            tsModel = new EyouSoft.Model.TourStructure.QuoteAttach();//初始化model
            bind();//绑定

        }

        #region 绑定列表数据

        protected void bind()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);//第几页
            EyouSoft.BLL.TourStructure.QuoteAttach tsBLL = new EyouSoft.BLL.TourStructure.QuoteAttach();//初始化bll
            EyouSoft.Model.TourStructure.QuoteAttach SearchInfo = null;//初始化model
            IList<EyouSoft.Model.TourStructure.QuoteAttach> GetInquireList;//初始化list
            GetInquireList = tsBLL.GetQuoteList(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, SearchInfo);//绑定列表
            //绑定
            retList.DataSource = GetInquireList;
            retList.DataBind();
            //分页
            BindPage();
        }
        #endregion


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
