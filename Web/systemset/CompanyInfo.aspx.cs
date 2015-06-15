using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;
using EyouSoft.Common.Function;

namespace Web.systemset
{   
    /// <summary>
    /// 设置公司信息
    /// xuty 2011/1/15
    /// </summary>
    public partial class CompanyInfo : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_公司信息_公司信息栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_公司信息_公司信息栏目, true);
                return;
            }
            EyouSoft.BLL.CompanyStructure.CompanyInfo companyBll = new EyouSoft.BLL.CompanyStructure.CompanyInfo();
            EyouSoft.Model.CompanyStructure.CompanyInfo infoModel = null;//公司信息实体
            string method = Utils.GetFormValue("hidMethod");
            if (method == "save")
            {
                #region 保存公司信息
                if (Utils.InputText(txtCompanyName.Value) == "")
                {
                    MessageBox.Show(this, "公司名称不为空");
                    return;
                }
                //保存
               EyouSoft.Model.CompanyStructure.CompanyAccount account = new EyouSoft.Model.CompanyStructure.CompanyAccount();//公司账户
               infoModel = new EyouSoft.Model.CompanyStructure.CompanyInfo();//公司信息实体
               infoModel.CompanyAddress= Utils.InputText(txtAddress.Value);//地址
               infoModel.ContactName= Utils.InputText(txtAdmin.Value);//负责人
               account.BankName= Utils.InputText(txtBank.Value);//开户行 
               account.CompanyId = CurrentUserCompanyID;//公司编号
               infoModel.CompanyZip=Utils.InputText(txtEmail.Value);//邮箱
               account.AccountName = Utils.InputText(txtUserName.Value);//户名
               account.BankNo = Utils.InputText(txtUserNo.Value);//账号
               infoModel.CompanyEnglishName= Utils.InputText(txtEngName.Value);//公司英文名
               infoModel.ContactFax= Utils.InputText(txtFax.Value);//公司传真
               infoModel.License= Utils.InputText(txtLicence.Value);//公司许可证
               infoModel.ContactMobile= Utils.InputText(txtMoible.Value);//公司手机
               infoModel.CompanyName= Utils.InputText(txtCompanyName.Value);//公司名
               infoModel.ContactTel=  Utils.InputText(txtTel.Value);//电话
               infoModel.CompanyType= Utils.InputText(txtType.Value);//旅行社类别
               infoModel.CompanySiteUrl= Utils.InputText(txtWeb.Value);//网站
               infoModel.CompanyAccountList = new List<EyouSoft.Model.CompanyStructure.CompanyAccount>();
               infoModel.CompanyAccountList.Add(account);//添加到账户集合
               infoModel.SystemId = CurrentUserCompanyID;//系统号
               infoModel.Id = CurrentUserCompanyID;//公司号
               bool result = false;
               result=companyBll.Update(infoModel);
               MessageBox.ShowAndRedirect(this, result?"保存成功！":"保存失败！","/systemset/CompanyInfo.aspx");
               #endregion
            }
            else
            {
                #region 初始化公司信息
                //初始化
                infoModel = companyBll.GetModel(CurrentUserCompanyID, CurrentUserCompanyID);
                if (infoModel != null)
                {  
                     EyouSoft.Model.CompanyStructure.CompanyAccount account=null;//公司账户
                     if (infoModel.CompanyAccountList != null && infoModel.CompanyAccountList.Count > 0)
                     {
                         account = infoModel.CompanyAccountList[0];
                     }
                    txtAddress.Value = infoModel.CompanyAddress;//地址
                    txtAdmin.Value = infoModel.ContactName;//负责人
                    txtEmail.Value = infoModel.CompanyZip;//邮箱
                    txtEngName.Value = infoModel.CompanyEnglishName;//公司英文名
                    txtFax.Value = infoModel.ContactFax;//公司传真
                    txtLicence.Value = infoModel.License;//公司许可证
                    txtMoible.Value = infoModel.ContactMobile;//公司手机
                    txtCompanyName.Value = infoModel.CompanyName;//公司名
                    txtTel.Value = infoModel.ContactTel;//电话
                    txtType.Value = infoModel.CompanyType;//旅行社类别
                    if (account != null)
                    {
                        txtBank.Value = account.BankName;//开户行 
                        txtUserName.Value = account.AccountName;//户名
                        txtUserNo.Value = account.BankNo;//账号
                    }
                    txtWeb.Value = infoModel.CompanySiteUrl;//网站
                }
                #endregion
            }
        }
    }
}
