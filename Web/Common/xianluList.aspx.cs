/// /////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：xianluList.aspx.cs
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\Common\xianluList.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-12 17:10:14
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// /////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Common
{
    public partial class xianluList : Eyousoft.Common.Page.BackPage
    {
        public int i = 0;
        public int k = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //selectXianlu1.Url = "?txtname=" + Request.QueryString["txtname"] + "&publishtype=" + Utils.GetQueryStringValue("publishtype") + "&hdid=" + Request.QueryString["hdid"] + "&callback=" + Request.QueryString["callback"] + "";
            selectXianlu1.Url = Request.Url.ToString();
            if (!IsPostBack)
            {
                selectXianlu1.userId = SiteUserInfo.ID;
                selectXianlu1.curCompany = CurrentUserCompanyID;                
                Bind();
            }

        }
        void Bind() {
            EyouSoft.BLL.RouteStructure.Route bll = new EyouSoft.BLL.RouteStructure.Route(SiteUserInfo);
            int count= 0;
            EyouSoft.Model.RouteStructure.RouteSearchInfo search=new EyouSoft.Model.RouteStructure.RouteSearchInfo();
            if (Utils.GetQueryStringValue("xlid") != "")
            {
                search.AreaId = Utils.GetInt(Utils.GetQueryStringValue("xlid"));
            }
            search.RSDate = Utils.GetDateTimeNullable(Utils.GetFormValue(txt_date.UniqueID));
            search.REDate = Utils.GetDateTimeNullable(Utils.GetFormValue(txt_endDate.UniqueID));
            string xlname=Utils.GetFormValue(txt_xianluName.UniqueID);
            if (xlname != "")
            {
                search.RouteName = xlname;
            }
            IList<EyouSoft.Model.RouteStructure.RouteBaseInfo> list =new 
            List<EyouSoft.Model.RouteStructure.RouteBaseInfo>();
            if (Utils.GetQueryStringValue("publishtype") != "")
            {
                ///快速
                if (Utils.GetInt(Utils.GetQueryStringValue("publishtype")) == 1)
                {
                    list = bll.GetQuickRoutes(CurrentUserCompanyID, 20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, search);
                }
                if (Utils.GetInt(Utils.GetQueryStringValue("publishtype")) == 2)
                {
                    list = bll.GetNormalRoutes(CurrentUserCompanyID, 20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, search);
                }
                if (Utils.GetInt(Utils.GetQueryStringValue("publishtype")) == 3)
                {
                    list = bll.GetRoutes(CurrentUserCompanyID, 20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, search);
                }
            }
            rptList.DataSource = list;
            rptList.DataBind();
            ExportPageInfo1.intPageSize =20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Bind();
        }
    }
}
