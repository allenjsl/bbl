using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EyouSoft.Common;

namespace Web.UserCenter.DomManager
{
    /// <summary>
    /// 文档更新
    /// 功能：人个中心-文档查看
    /// 创建人：dj
    /// 创建时间：2011-1-14    
    /// </summary>
    public partial class DomShow : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected string type = string.Empty;
        protected int tid;
        protected EyouSoft.Model.PersonalCenterStructure.PersonDocument pdModel = null;
        protected EyouSoft.BLL.PersonalCenterStructure.PersonDocument pdBll = null;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_文档管理_下载文档))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_文档管理_下载文档, false);
            }
            pdModel = new EyouSoft.Model.PersonalCenterStructure.PersonDocument();
            pdBll = new EyouSoft.BLL.PersonalCenterStructure.PersonDocument(SiteUserInfo);
            if (!IsPostBack)
            {
                DomBind();
            }
        }
        /// <summary>
        /// 查看文档数据绑定
        /// </summary>
        private void DomBind()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            pdModel = pdBll.GetModel(tid);
            //pdModel = new EyouSoft.Model.PersonalCenterStructure.PersonDocument();
        }
        /// <summary>
        /// 页面初使化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }
    }
}
