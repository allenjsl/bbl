using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.GroupEnd.News
{
    /// <summary>
    /// 最新动态-查看
    /// 柴逸宁
    /// </summary>

    public partial class Newsshow : Eyousoft.Common.Page.FrontPage
    {
        #region 变量

        protected int tid;
        protected EyouSoft.Model.CompanyStructure.News nModel = new EyouSoft.Model.CompanyStructure.News();
        EyouSoft.BLL.CompanyStructure.News nBll = null;


        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_公告通知_查看公告))
            //{
            //    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_公告通知_查看公告, true);
            //}
            nBll = new EyouSoft.BLL.CompanyStructure.News();
            if (!IsPostBack)
            {
                DataInit();
            }
        }

        private void DataInit()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                nModel = nBll.GetModel(tid);
                nBll.SetClicks(tid);
            }
        }
    }

}
