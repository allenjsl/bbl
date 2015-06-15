using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Function;
using EyouSoft.Common;

namespace Web.GroupEnd.SystemSetting
{
    /// <summary>
    /// 设置公司信息
    /// 创建人：李晓欢
    /// </summary>
    public partial class CompanyInfo : Eyousoft.Common.Page.FrontPage
    {        
        EyouSoft.Model.CompanyStructure.CustomerInfo CustomerInfo = null;
        EyouSoft.BLL.CompanyStructure.Customer Customer = new EyouSoft.BLL.CompanyStructure.Customer();

        protected void Page_Load(object sender, EventArgs e)
        {
            //省份
            ProvinceList1.CompanyId = SiteUserInfo.CompanyID;
            ProvinceList1.IsFav = true;
           
            //城市
            CityList1.CompanyId = SiteUserInfo.CompanyID;
            CityList1.IsFav = true;
           
            if (Utils.GetInt(Utils.GetQueryStringValue("issave"))== 1)
            {
                this.InitBindCompany();
            }
            else
            {
                #region 初始化公司信息
                if(!this.Page.IsPostBack)
                {                 
                    CustomerInfo=Customer.GetCustomerModel(SiteUserInfo.TourCompany.TourCompanyId);
                    if (CustomerInfo != null)
                    {
                        this.ProvinceList1.ProvinceId = CustomerInfo.ProviceId;
                        this.CityList1.ProvinceId = CustomerInfo.ProviceId;
                        CityList1.CityId = CustomerInfo.CityId;
                        //公司名称
                        this.Txt_UnitsName.Value = CustomerInfo.Name;
                        //许可证
                        this.Txt_LicenseNumber.Value = CustomerInfo.Licence;
                        //公司地址
                        this.Txt_CompanyAddress.Value = CustomerInfo.Adress;
                        //邮编
                        this.Txt_Code.Value = CustomerInfo.PostalCode;
                        //开户账号
                        this.Txt_AccountNumber.Value = CustomerInfo.BankAccount;
                        //返佣类型
                        if (this.DDlReBateType.Items.FindByValue(((int)CustomerInfo.CommissionType).ToString()) != null)
                        {
                            this.DDlReBateType.Items.FindByValue(((int)CustomerInfo.CommissionType).ToString()).Selected = true;
                        }
                        //主要联系人
                        this.Txt_Contact.Value = CustomerInfo.ContactName;
                        //电话
                        this.Txt_Phone.Value = CustomerInfo.Phone;
                        //手机
                        this.Txt_Mobile.Value = CustomerInfo.Mobile;
                        //传真
                        this.Txt_Fox.Value = CustomerInfo.Fax;
                        //备注
                        this.Txt_Remirke.Value = CustomerInfo.Remark;
                    }                   
                }
                CustomerInfo = null;
                #endregion
            }
        }

        #region 保存公司信息
        protected void InitBindCompany()
        {
            CustomerInfo =new EyouSoft.Model.CompanyStructure.CustomerInfo();
            //省份
            int Province = this.ProvinceList1.ProvinceId;
            //城市
            int CityID = this.CityList1.CityId;
            //单位名称
            string UnitsName = Utils.GetString(Utils.GetFormValue(Txt_UnitsName.UniqueID.Trim()), "");
            if (Utils.InputText(UnitsName) == "")
            {
                MessageBox.Show(this, "请填写单位名称!");
                return;
            }
            //许可证号
            string LicenseNumber = Utils.GetString(Utils.GetFormValue(Txt_LicenseNumber.UniqueID), "");
            //公司地址
            string CompanyAddress = Utils.GetString(Utils.GetFormValue(Txt_CompanyAddress.UniqueID), "");
            //邮编
            string Code = Utils.GetString(Utils.GetFormValue(Txt_Code.UniqueID), "");
            //银行账号
            string AccountNumber = Utils.GetString(Utils.GetFormValue(Txt_AccountNumber.UniqueID), "");
            //返佣类型
            string ReBateType = Utils.GetString(Utils.GetFormValue(DDlReBateType.UniqueID), "");
            //主要联系人
            string Contact =Utils.GetString(Utils.GetFormValue(Txt_Contact.UniqueID), "");
            if (Utils.InputText(Contact) == "")
            {
                MessageBox.Show(this, "请填写主要联系人!");
                return;
            }
            //电话
            string Tel = Utils.GetString(Utils.GetFormValue(Txt_Phone.UniqueID), "");
            if (Utils.InputText(Tel) == "")
            {
                MessageBox.Show(this, "请填写联系电话号码!");
                return;
            }
            //手机
            string Moblie = Utils.GetString(Utils.GetFormValue(Txt_Mobile.UniqueID), "");
            //传真
            string Fox = Utils.GetString(Utils.GetFormValue(Txt_Fox.UniqueID), "");
            //备注
            string Remirke = Utils.GetString(Utils.GetFormValue(Txt_Remirke.UniqueID), "");

            CustomerInfo.ProviceId = Province;
            CustomerInfo.CityId = CityID;
            CustomerInfo.Name = UnitsName;
            CustomerInfo.Licence = LicenseNumber;
            CustomerInfo.Adress = CompanyAddress;
            CustomerInfo.PostalCode = Code;
            CustomerInfo.BankAccount = AccountNumber;
            CustomerInfo.CommissionType = (EyouSoft.Model.EnumType.CompanyStructure.CommissionType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CommissionType), ReBateType);
            CustomerInfo.ContactName = Contact;
            CustomerInfo.Phone = Tel;
            CustomerInfo.Mobile = Moblie;
            CustomerInfo.Fax = Fox;
            CustomerInfo.Remark = Remirke;
            CustomerInfo.Id = SiteUserInfo.TourCompany.TourCompanyId;

            //更新组团端公司信息
            if (Customer.UpdateSampleCustomer(CustomerInfo))
            {
                SetErrorMsg(true, "公司信息设置成功!", "/GroupEnd/SystemSetting/CompanyInfo.aspx");
            }
            else {
                SetErrorMsg(false, "公司信息设置失败!", "/GroupEnd/SystemSetting/CompanyInfo.aspx");
            }
        }
        #endregion

        #region 提示操作信息方法
        /// <summary>
        /// 提示提交信息
        /// </summary>
        /// <param name="IsSuccess">true 执行成功 flase 执行失败</param>
        /// <param name="Msg">提示信息</param>
        private void SetErrorMsg(bool isSuccess, string msg, string url)
        {
            var s = "{{isSuccess:{0},errMsg:'{1}'}}";
            Response.Clear();
            Response.Write(string.Format(s, isSuccess.ToString().ToLower(), msg, url));
            Response.End();
        }
        #endregion
    }
}
