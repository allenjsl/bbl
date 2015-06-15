using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using EyouSoft.Common;

namespace Web.GroupEnd.wh
{
    /// <summary>
    /// 创建:万俊
    /// 功能:用户注册
    /// </summary>
    public partial class UserReg : System.Web.UI.Page
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        protected int companyId = 0;
        EyouSoft.Model.SysStructure.SystemDomain domain;
        protected void Page_Load(object sender, EventArgs e)
        {
            //设置公司ID
            //判断 当前域名是否是专线系统为组团配置的域名
            domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
            companyId = domain.CompanyId;
            if (!IsPostBack)
            {
                //设定导航页被选中项
                this.WhHeadControl1.NavIndex = 0;
                //绑定省份下拉框 
                BindPro();
            }
            #region 设置页面title,meta
            //声明基础设置实体对象
            EyouSoft.Model.SiteStructure.SiteBasicConfig configModel = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(domain.CompanyId);
            if (configModel != null)
            {
                //添加Meta
                Literal keywork = new Literal();
                keywork.Text = "<meta name=\"keywords\" content= \"" + configModel.SiteMeta + "\" />";
                this.Page.Header.Controls.Add(keywork);
            }
            #endregion
            //设置导航用户控件的公司ID 
            this.WhHeadControl1.CompanyId = companyId;
            //设置专线介绍用户控件的公司ID
            this.WhLineControl1.CompanyId = companyId;
            //设置友情链接公司ID
            this.WhBottomControl1.CompanyId = companyId;
        }

        #region 绑定省市
        //绑定省
        protected void BindPro()
        {
            //清空省份的下拉框
            this.ddlPro.Items.Clear();
            //添加默认选择项"-请选择-"
            this.ddlPro.Items.Add(new ListItem("-请选择-", ""));
            //省份BLL
            EyouSoft.BLL.CompanyStructure.Province proBll = new EyouSoft.BLL.CompanyStructure.Province();
            //根据公司ID取得相应结果集list
            IList<EyouSoft.Model.CompanyStructure.Province> list = proBll.GetList(companyId);
            //循环将值添加到下拉框
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = list[i].ProvinceName;
                    item.Value = Convert.ToString(list[i].Id);
                    this.ddlPro.Items.Add(item);
                }
            }

        }
        //绑定城市-前台的JS方法 省份选择的change方法调用
        [System.Web.Services.WebMethod()]
        public static string BindCity(string proId, string companyId)
        {
            StringBuilder strBul = new StringBuilder();
            if (proId != "0")
            {
                //城市BLL
                EyouSoft.BLL.CompanyStructure.City cityBll = new EyouSoft.BLL.CompanyStructure.City();
                //根据选择的省份ID，公司ID取得的结果集list 
                IList<EyouSoft.Model.CompanyStructure.City> list = cityBll.GetList(Convert.ToInt32(companyId), (int?)Convert.ToInt32(proId), null);
                if (list != null && list.Count > 0)
                {
                    //遍历list将值添加到strBul
                    foreach (EyouSoft.Model.CompanyStructure.City city in list)
                    {
                        //最终要返回的值,下拉框的HTML
                        strBul.Append("<option value=\"");
                        strBul.Append(city.Id);
                        strBul.Append("\">");
                        strBul.Append(city.CityName);
                        strBul.Append("</option>");
                    }
                }
            }


            return strBul.ToString();
        }
        #endregion


        /// <summary>
        /// 注册用户
        /// </summary>
        protected void lkBtnSave_Click(object sender, EventArgs e)
        {
            #region 验证
            if (Utils.GetFormValue(this.ddlPro.UniqueID) == "" || Utils.GetFormValue(this.ddlPro.UniqueID) == "0")
            {
                Response.Write("<script>alert('请选择省份!');location.href=location.href</script>");
                return;
            }

            if (Utils.GetFormValue(this.ddlCity.UniqueID) == "" || Utils.GetFormValue(this.ddlCity.UniqueID) == "")
            {
                Response.Write("<script>alert('请选择城市!');location.href=location.href</script>");
                return;
            }

            if (this.txtCompanyName.Text.Trim() == "")
            {
                Response.Write("<script>alert('请请输入单位名称!');location.href=location.href</script>");
                return;
            }
            if (this.txtUserName.Text.Trim() == "")
            {
                Response.Write("<script>alert('请输入用户名!');location.href=location.href</script>");
                return;
            }
            if (this.txtUserPwd.Text.Trim() == "")
            {
                Response.Write("<script>alert('请输入密码!');location.href=location.href</script>");
                return;
            }
            #endregion





            //声明BLL
            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();
            //声明新的对象
            EyouSoft.Model.CompanyStructure.CustomerInfo custModel = new EyouSoft.Model.CompanyStructure.CustomerInfo();
            //联系人集合
            IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> list = new List<EyouSoft.Model.CompanyStructure.CustomerContactInfo>();
            //声明联系人
            EyouSoft.Model.CompanyStructure.CustomerContactInfo custInfo = new EyouSoft.Model.CompanyStructure.CustomerContactInfo();
            //联系人账户信息赋值
            custInfo.UserAccount = new EyouSoft.Model.CompanyStructure.UserAccount();
            custInfo.UserAccount.CompanyId = companyId;
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
            custModel.ProviceId = Utils.GetInt(Utils.GetFormValue(this.ddlPro.UniqueID));            //省份ID
            custModel.CityId = Utils.GetInt(Utils.GetFormValue(this.ddlCity.UniqueID));             //城市ID
            custModel.CustomerContactList = list;
            custModel.CompanyId = companyId;                                 //所属公司
            custModel.IssueTime = DateTime.Now;
           
            custModel.IsEnable = false;
            custModel.IsDelete = false;
            #endregion
            int count = custBll.AddCustomer(custModel);
            if (count > 0)
            {
                Response.Write("<script>alert('注册成功');location.href=location.href</script>");
            }
            else
            {
                Response.Write("<script>alert('注册失败')</script>");
            }
        }
        /// <summary>
        /// 检测用户名是否重复
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        [System.Web.Services.WebMethod()]
        public static bool CheckUserName(string userName, int companyId)
        {
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            bool result = userBll.IsExists(0, userName, companyId);
            return result;
        }

    }
}
