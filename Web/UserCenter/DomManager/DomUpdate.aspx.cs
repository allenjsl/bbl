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
using EyouSoft.Common.Function;

namespace Web.UserCenter.DomManager
{
    /// <summary>
    /// 文档更新
    /// 功能：人个中心-更新文档
    /// 创建人：dj
    /// 创建时间：2011-1-14    
    /// </summary>
    public partial class DomUpdate : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected string type = string.Empty;
        protected int tid;
        protected EyouSoft.Model.PersonalCenterStructure.PersonDocument pdModel = null;
        protected EyouSoft.BLL.PersonalCenterStructure.PersonDocument pdBll = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            pdModel = new EyouSoft.Model.PersonalCenterStructure.PersonDocument();
            pdBll = new EyouSoft.BLL.PersonalCenterStructure.PersonDocument(SiteUserInfo);
            type = Utils.GetQueryStringValue("type");
            if (IsPostBack)
            {
                Save();
            }
            else
            {
                DomBind();
            }
        }

        /// <summary>
        /// 修改文档数据绑定
        /// </summary>
        private void DomBind()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            pdModel = pdBll.GetModel(tid);
            //pdModel = new EyouSoft.Model.PersonalCenterStructure.PersonDocument();
        }

        /// <summary>
        /// 修改
        /// </summary>
        private void Save()
        {
            bool res = false;
            tid = Utils.GetInt(Utils.GetFormValue("domId"));
            if (tid > 0)
            {
                pdModel = pdBll.GetModel(tid);
                pdModel.DocumentName = Utils.GetFormValue("domname");
                if (Request.Files.Count > 0)
                {
                    string filepath = string.Empty;
                    string oldfilename = string.Empty;
                    bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["upfile"], "UserCenterFile", out filepath, out oldfilename);
                    if (result)
                    {
                        pdModel.FilePath = filepath;
                    }
                }
                res = pdBll.Update(pdModel);
            }

            if (res)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location.reload();", "修改成功", Utils.GetQueryStringValue("iframeId")));
            }
            else
            {
                MessageBox.ShowAndClose(this, "修改失败!");
            }

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
