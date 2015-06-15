using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Collections.Generic;
using EyouSoft.Common;

namespace Web.UserCenter.Notice
{
    /// <summary>
    /// 文档更新
    /// 功能：人个中心-公告通知
    /// 创建人：dj
    /// 创建时间：2011-1-24   
    /// </summary>
    public partial class Notice : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        EyouSoft.BLL.CompanyStructure.News nBll;
        protected int len = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_公告通知_查看公告))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_公告通知_查看公告, true);
            }
            nBll = new EyouSoft.BLL.CompanyStructure.News();
            if (!IsPostBack)
            {
                DataInit();
            }
        }

        #region 数据初使化
        private void DataInit()
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"),1);
            IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> list = null;
            list = nBll.GetAcceptNews(pageSize, pageIndex, ref recordCount,SiteUserInfo.ID,CurrentUserCompanyID);
            len = list.Count;
            
            this.rptList.DataSource = list;
            this.rptList.DataBind();
            //设置分页
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
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion
    }
}
