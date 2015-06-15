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
    /// 页面功能：个人中心--文档管理新增
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class DomAdd : Eyousoft.Common.Page.BackPage
    {
        protected string type = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_文档管理_新增))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_文档管理_新增, false);
            }

            if (!IsPostBack)
            {

            }
            else
            {
                Save();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            EyouSoft.Model.PersonalCenterStructure.PersonDocument pdModel = new EyouSoft.Model.PersonalCenterStructure.PersonDocument();
            pdModel.DocumentName = Utils.GetFormValue("domname");
            pdModel.CreateTime = DateTime.Now;
            pdModel.OperatorName = SiteUserInfo.ContactInfo.ContactName;
            pdModel.CompanyId = CurrentUserCompanyID;
            pdModel.OperatorId = SiteUserInfo.ID;
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
            EyouSoft.BLL.PersonalCenterStructure.PersonDocument pdBll = new EyouSoft.BLL.PersonalCenterStructure.PersonDocument(SiteUserInfo);
            bool res = false;
            res = pdBll.Add(pdModel);

            string conti = Utils.GetFormValue("continue");

            if (res)
            {
                if (conti == "continue")
                {
                    MessageBox.ShowAndCloseReload(this, "保存成功！");
                }
                else
                {
                    MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location.reload();", "保存成功", Utils.GetQueryStringValue("iframeId")));
                }
            }
            else
            {
                MessageBox.ShowAndReturnBack(this, "保存失败!",1);
            }

        }



        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }
    }
}
