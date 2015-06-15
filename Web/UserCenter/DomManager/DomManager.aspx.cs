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

namespace Web.UserCenter.DomManager
{
    /// <summary>
    /// 页面功能：个人中心--文档管理
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class DomManager : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int len = 0;
        protected string type = string.Empty;
        EyouSoft.BLL.PersonalCenterStructure.PersonDocument pdBll = null;

        //权限变量
        protected bool grantadd = false;
        protected bool grantdel = false;
        protected bool grantmodify = false;
        protected bool grantload = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_文档管理_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_文档管理_栏目, true);
            }

            grantadd = CheckGrant(global::Common.Enum.TravelPermission.个人中心_文档管理_新增);
            grantdel = CheckGrant(global::Common.Enum.TravelPermission.个人中心_文档管理_删除);
            grantmodify = CheckGrant(global::Common.Enum.TravelPermission.个人中心_文档管理_修改);
            grantload = CheckGrant(global::Common.Enum.TravelPermission.个人中心_文档管理_下载文档);

            pdBll = new EyouSoft.BLL.PersonalCenterStructure.PersonDocument(SiteUserInfo);
            if (!IsPostBack)
            {
                type = Utils.GetQueryStringValue("type");
                switch (type)
                {
                    case "domdel":
                        DomDel();
                        break;
                    default:
                        DataInit();
                        break;
                }
            }
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        private void DomDel()
        {
            string[] tid = Utils.GetFormValue("tid").Split(',');
            bool result = false;
            result = pdBll.Delete(tid);
            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}",result?1:-1));
            Response.End();
        }

        #region 初使化

        private void DataInit()
        {
            pageIndex = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            IList<EyouSoft.Model.PersonalCenterStructure.PersonDocument> list = null;
            list = new List<EyouSoft.Model.PersonalCenterStructure.PersonDocument>();
            list = pdBll.GetList(pageSize, pageIndex, ref recordCount,SiteUserInfo.CompanyID,0);
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
            this.ExporPageInfoSelect1.PageLinkURL = Request.Path + "?";
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion
    }
}
