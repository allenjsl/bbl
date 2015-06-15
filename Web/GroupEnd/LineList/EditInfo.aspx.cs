using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Function;
using EyouSoft.Common;

namespace Web.GroupEnd.Orders
{
    /// <summary>
    /// 页面：信息编辑
    /// 功能：信息编辑
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class EditInfo : Eyousoft.Common.Page.FrontPage
    {
        EyouSoft.BLL.CompanyStructure.Customer cBll = null;
        EyouSoft.BLL.CompanyStructure.CompanyUser csBll = null;
        protected EyouSoft.Model.CompanyStructure.CompanyUser cModel = new EyouSoft.Model.CompanyStructure.CompanyUser();
        protected EyouSoft.Model.CompanyStructure.CustomerConfig cptModel = new EyouSoft.Model.CompanyStructure.CustomerConfig();
        protected void Page_Load(object sender, EventArgs e)
        {
            csBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);
            cBll = new EyouSoft.BLL.CompanyStructure.Customer();
            if (!IsPostBack)
            {
                cominit();//初使化修改信息
            }
            else
            {
                InitSave();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        #region 保存用户信息
        protected void InitSave()
        {
            cominit();
            cModel.PersonInfo.ContactTel = Utils.GetFormValue("CompanyTEl");
            cModel.PersonInfo.ContactMobile = Utils.GetFormValue("CompanyPhone");

            //页头
            if (Request.Files["PrintTop"] != null)
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["PrintTop"], "zutuanFile", out filepath, out oldfilename);
                if (result)
                {
                    cptModel.PageHeadFile = filepath;
                }
            }
            //模板页
            if (Request.Files["PrintTemplate"] != null)
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["PrintTemplate"], "zutuanFile", out filepath, out oldfilename);
                if (result)
                {
                    cptModel.TemplateFile = filepath;
                }
            }
            //页脚
            if (Request.Files["PrintFooter"] != null)
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["PrintFooter"], "zutuanFile", out filepath, out oldfilename);
                if (result)
                {
                    cptModel.PageFootFile = filepath;
                }
            }
            //公章
            if (Request.Files["PrintCachet"] != null)
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["PrintCachet"], "zutuanFile", out filepath, out oldfilename);
                if (result)
                {
                    cptModel.CustomerStamp = filepath;
                }
            }

            bool res = cBll.UpDateSampleCompanyUserInfo(cModel);

            res = cBll.UpdateSampleCustomerConfig(cptModel);
            if (res)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location.reload();", "保存成功", Utils.GetQueryStringValue("iframeId")));
            }
        }
        #endregion

        /// <summary>
        /// 初使化用户信息
        /// </summary>
        private void cominit()
        {
            cModel = csBll.GetUserInfo(SiteUserInfo.ID);
            cptModel = cBll.GetCustomerConfigModel(CurrentUserCompanyID);
            if (cModel == null)
                cModel = new EyouSoft.Model.CompanyStructure.CompanyUser();
            if (cptModel == null)
                cptModel = new EyouSoft.Model.CompanyStructure.CustomerConfig();
        }
    }
}
