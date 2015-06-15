using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.xianlu
{
    /// <summary>
    /// 模块名称:线路信息列表发布人弹出页面
    /// 创建时间:2011-01-12
    /// 创建人:lixh
    /// </summary>
    public partial class Published : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                InitBindAuthor();
            }
        }

        #region 绑定发布人信息 
        protected void InitBindAuthor()
        {
            EyouSoft.Model.CompanyStructure.ContactPersonInfo Model_ContactPersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo();
            EyouSoft.BLL.CompanyStructure.CompanyUser Bll_CompanyUser = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);

            int UserID = EyouSoft.Common.Utils.GetInt(Request.QueryString["ID"]);
            if (!string.IsNullOrEmpty(ID.ToString()) && UserID > 0)
            {
                Model_ContactPersonInfo = Bll_CompanyUser.GetUserBasicInfo(UserID);
                if (Model_ContactPersonInfo != null)
                {
                    //联系人
                    this.Lab_Contact.Text = Model_ContactPersonInfo.ContactName.ToString();
                    //联系电话
                    this.Lab_Mobile.Text = Model_ContactPersonInfo.ContactTel.ToString();
                    //传真
                    this.Lab_Fox.Text = Model_ContactPersonInfo.ContactFax.ToString();
                    //手机
                    this.Lab_Phone.Text = Model_ContactPersonInfo.ContactMobile.ToString();
                    //QQ
                    this.Lab_QQ.Text = Model_ContactPersonInfo.QQ.ToString();
                    //MsN
                    this.Lab_Msn.Text = Model_ContactPersonInfo.MSN.ToString();
                    //Email
                    this.Lab_Email.Text = Model_ContactPersonInfo.ContactEmail.ToString();
                }
            }
            Model_ContactPersonInfo = null;
            Bll_CompanyUser = null;
        }
        #endregion

        #region 设置弹窗页面
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion
    }
}
