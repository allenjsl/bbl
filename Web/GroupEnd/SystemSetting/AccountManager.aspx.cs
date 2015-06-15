using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.GroupEnd.SystemSetting
{
    /// <summary>
    /// 组团端账号管理
    /// 李晓欢
    /// </summary>
    public partial class AccountManager : Eyousoft.Common.Page.FrontPage
    {              
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.BLL.CompanyStructure.CompanyUser CompanyUser = null;
            EyouSoft.Model.CompanyStructure.CompanyUser ModelCompanyUser =new EyouSoft.Model.CompanyStructure.CompanyUser ();
            
            string hidMethod=Utils.GetFormValue("hidMethod");
            
            if (hidMethod == "save")
            {
                ModelCompanyUser.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord();
                ModelCompanyUser.PassWordInfo.NoEncryptPassword = this.Txt_PassWord.Text;
                ModelCompanyUser.UserName = Utils.GetFormValue(this.txtUserName.UniqueID);
                ModelCompanyUser.PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo();
                ModelCompanyUser.PersonInfo.ContactName = this.Txt_Name.Value;
                ModelCompanyUser.PersonInfo.ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.Sex), this.ddlSex.SelectedValue);
                ModelCompanyUser.PersonInfo.JobName = this.Txt_Position.Value;
                ModelCompanyUser.PersonInfo.ContactTel = this.Txt_Phone.Value;
                ModelCompanyUser.PersonInfo.ContactFax = this.Txt_Fox.Value;
                ModelCompanyUser.PersonInfo.ContactMobile = this.Txt_Moblie.Value;
                ModelCompanyUser.PersonInfo.QQ = this.Txt_QQNumber.Value;
                ModelCompanyUser.PersonInfo.ContactEmail = this.Txt_Email.Value;
                ModelCompanyUser.PersonInfo.Remark = this.Txt_Remiks.Value;

                bool result = true;
                ModelCompanyUser.ID = SiteUserInfo.ID;
                ModelCompanyUser.TourCompanyId = SiteUserInfo.TourCompany.TourCompanyId;
                CompanyUser = new EyouSoft.BLL.CompanyStructure.CompanyUser();
                result = CompanyUser.UpdateZuTuan(ModelCompanyUser);
                MessageBox.ShowAndRedirect(this, result ? "用户信息修改成功!" : "用户信息修改失败!", "/GroupEnd/SystemSetting/AccountManager.aspx");
                return;
            }
            CompanyUser = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            ModelCompanyUser = CompanyUser.GetUserInfo(SiteUserInfo.ID);
            if (ModelCompanyUser != null)
            {
                this.Txt_UserName.Text = ModelCompanyUser.UserName;
                this.txtUserName.Value = ModelCompanyUser.UserName;
                this.Txt_Name.Value = ModelCompanyUser.PersonInfo.ContactName;
                this.Txt_PassWord.Text = ModelCompanyUser.PassWordInfo.NoEncryptPassword;
                if (this.ddlSex.Items.FindByValue(((int)ModelCompanyUser.PersonInfo.ContactSex).ToString()) != null)
                {
                    this.ddlSex.Items.FindByValue(((int)ModelCompanyUser.PersonInfo.ContactSex).ToString()).Selected = true;
                }
                this.Txt_Position.Value = ModelCompanyUser.PersonInfo.JobName;
                this.Txt_Phone.Value = ModelCompanyUser.PersonInfo.ContactTel;
                this.Txt_Fox.Value = ModelCompanyUser.PersonInfo.ContactFax;
                this.Txt_Moblie.Value = ModelCompanyUser.PersonInfo.ContactMobile;
                this.Txt_QQNumber.Value = ModelCompanyUser.PersonInfo.QQ;
                this.Txt_Email.Value = ModelCompanyUser.PersonInfo.ContactEmail;
                this.Txt_Remiks.Value = ModelCompanyUser.PersonInfo.Remark;
            }

        }
    }
}
