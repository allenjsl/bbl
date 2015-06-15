using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Shop.T1
{
    /// <summary>
    /// reg
    /// </summary>
    /// 汪奇志 2012-04-03
    public partial class Reg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitProvince();
            }
        }

        /// <summary>
        /// init province
        /// </summary>
        protected void InitProvince()
        {
            txtProvince.Items.Clear();
            txtProvince.Items.Add(new ListItem("-请选择-", "0"));
            var items = new EyouSoft.BLL.CompanyStructure.Province().GetList(Master.CompanyId);
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    txtProvince.Items.Add(new ListItem(item.ProvinceName, item.Id.ToString()));
                }
            }
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        protected void lkBtnSave_Click(object sender, EventArgs e)
        {
            #region 验证
            if (string.IsNullOrEmpty(Utils.GetFormValue(this.txtProvince.UniqueID)))
            {
                Response.Write("<script>alert('请选择省份!');location.href=location.href</script>");
                return;
            }

            if (string.IsNullOrEmpty(Utils.GetFormValue("txtCity")))
            {
                Response.Write("<script>alert('请选择城市!');location.href=location.href</script>");
                return;
            }

            if (string.IsNullOrEmpty(this.txtCompanyName.Text.Trim()))
            {
                Response.Write("<script>alert('请请输入单位名称!');location.href=location.href</script>");
                return;
            }
            if (string.IsNullOrEmpty(this.txtUserName.Text.Trim()))
            {
                Response.Write("<script>alert('请输入用户名!');location.href=location.href</script>");
                return;
            }
            if (string.IsNullOrEmpty(this.txtUserPwd.Text.Trim()))
            {
                Response.Write("<script>alert('请输入密码!');location.href=location.href</script>");
                return;
            }
            #endregion

            //声明新的对象
            EyouSoft.Model.CompanyStructure.CustomerInfo custModel = new EyouSoft.Model.CompanyStructure.CustomerInfo();
            //联系人集合
            IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> list = new List<EyouSoft.Model.CompanyStructure.CustomerContactInfo>();
            //声明联系人
            EyouSoft.Model.CompanyStructure.CustomerContactInfo custInfo = new EyouSoft.Model.CompanyStructure.CustomerContactInfo();
            //联系人账户信息赋值
            custInfo.UserAccount = new EyouSoft.Model.CompanyStructure.UserAccount();
            custInfo.UserAccount.CompanyId = Master.CompanyId;
            custInfo.UserAccount.UserName = txtUserName.Text;
            custInfo.Sex = "0";
            custInfo.UserAccount.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord { NoEncryptPassword = txtUserPwd.Text };
            list.Add(custInfo);

            #region Model赋值
            custModel.Name = this.txtCompanyName.Text.Trim();//单位名称
            custModel.Licence = this.txtLicense.Text.Trim();//许可证号
            custModel.Adress = this.txtAddress.Text.Trim();//地址
            custModel.PostalCode = this.txtPostalCode.Text.Trim();//邮编
            custModel.ContactName = this.txtContact.Text.Trim();//主要联系人
            custModel.Phone = this.txtTel.Text.Trim();//电话
            custModel.Mobile = this.txtPhone.Text;//手机
            custModel.Fax = this.txtFax.Text.Trim();//传真
            custModel.ProviceId = Utils.GetInt(Utils.GetFormValue(this.txtProvince.UniqueID));            //省份ID
            custModel.CityId = Utils.GetInt(Utils.GetFormValue("txtCity"));             //城市ID
            custModel.CustomerContactList = list;
            custModel.CompanyId = Master.CompanyId;                                 //所属公司
            custModel.IssueTime = DateTime.Now;

            custModel.IsEnable = false;
            custModel.IsDelete = false;
            #endregion

            int count = new EyouSoft.BLL.CompanyStructure.Customer().AddCustomer(custModel);
            if (count > 0)
            {
                Response.Write("<script>alert('注册成功');location.href=location.href;</script>");
            }
            else
            {
                Response.Write("<script>alert('注册失败');location.href=location.href;</script>");
            }
        }
    }
}
