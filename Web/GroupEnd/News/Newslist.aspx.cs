using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Model.CompanyStructure;
using EyouSoft.Common;

namespace Web.GroupEnd.News
{
    /// <summary>
    ///组团端-最新动态
    ///柴逸宁
    ///11.3.18
    /// </summary>
    public partial class Newslist : Eyousoft.Common.Page.FrontPage
    {
        #region 变量

        protected int len = 0;//列表长度

        //查询条件
        protected string Titles = string.Empty;
        protected string OperateName = string.Empty;
        protected string IssueTime = string.Empty;


        protected int pageSize = 20;
        protected int pageIndex;
        protected int recordCount;




        protected IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> newslist = null;//公告信息
        protected IList<NewsAccept> acceptList = null;//发布对象集合
        //BLL
        protected EyouSoft.BLL.CompanyStructure.News nBll = null;
        //Model

        protected EyouSoft.Model.CompanyStructure.News nModel = new EyouSoft.Model.CompanyStructure.News();


        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title += "_组团_" + SiteUserInfo.CompanyName;
            if (!IsPostBack)
            {

                BindProClamationList();

            }
        }

        #region


        #endregion

        #region 绑定最新动态数据

        protected void BindProClamationList()
        {


            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            ///////绑定查询条件

            EyouSoft.Model.PersonalCenterStructure.NoticeNews sModel = new EyouSoft.Model.PersonalCenterStructure.NoticeNews();
            Titles = EyouSoft.Common.Utils.GetQueryStringValue("Titles");
            OperateName = EyouSoft.Common.Utils.GetQueryStringValue("OperateName");
            if (EyouSoft.Common.Utils.GetQueryStringValue("IssueTime") == "")
            {
                sModel.IssueTime = null;

            }
            else
            {
                sModel.IssueTime = EyouSoft.Common.Utils.GetDateTime(EyouSoft.Common.Utils.GetQueryStringValue("IssueTime"));
                IssueTime = sModel.IssueTime.Value.ToString("yyyy-MM-dd");

            }

            sModel.Title = Titles;
            sModel.OperateName = OperateName;
            nBll = new EyouSoft.BLL.CompanyStructure.News();
            newslist = nBll.GetZuTuanAcceptNews(pageSize, pageIndex, ref recordCount, SiteUserInfo.CompanyID, sModel);
            len = newslist.Count();
            rptList.DataSource = newslist;
            rptList.DataBind();
            newslist = null;
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
