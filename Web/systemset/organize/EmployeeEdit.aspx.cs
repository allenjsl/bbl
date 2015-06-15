
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.organize
{   
    /// <summary>
    /// 部门人员编辑
    /// xuty 2011/1/14
    /// </summary>
    public partial class EmployeeEdit : Eyousoft.Common.Page.BackPage
    {
        protected int empId;
        protected string method2 = "";
        protected string pass = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_组织机构_部门人员栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_组织机构_部门人员栏目, false);
                return;
            }
            empId = Utils.GetInt(Utils.GetQueryStringValue("empId"));//获取员工Id
            string method = Utils.GetFormValue("hidMethod");//获取当前操作(保存/继续)
            method2 = Utils.GetQueryStringValue("copy");//是否复制数据
            string showMess = "数据保存成功！";//提示消息
            //如果当前操作无则初始加载(否则保存操作)
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);//初始化bll
            EyouSoft.BLL.CompanyStructure.Department departBll = new EyouSoft.BLL.CompanyStructure.Department();//初始化bll
            if (method == "")
            {
                #region 初始化员工信息
                //所属部门
                IList<EyouSoft.Model.CompanyStructure.Department> departList = departBll.GetAllDept(CurrentUserCompanyID);
                selBdepart.DataTextField = "DepartName";
                selBdepart.DataValueField = "Id";
                selBdepart.DataSource = departList;
                selBdepart.DataBind();
                selBdepart.Items.Insert(0, new ListItem("选择部门", ""));
                //监管部门
                selMdepart.DataTextField = "DepartName";
                selMdepart.DataValueField = "Id";
                selMdepart.DataSource = departList;
                selMdepart.DataBind();
                selMdepart.Items.Insert(0, new ListItem("选择部门", ""));
                if (empId != 0) //如果员工Id不为空则加载数据
                {
                    EyouSoft.Model.CompanyStructure.CompanyUser userModel = userBll.GetUserInfo(empId);
                    if (userModel != null)
                    {
                        txtEmail.Value = userModel.PersonInfo.ContactEmail;
                        txtFax.Value = userModel.PersonInfo.ContactFax;
                        txtIntroduce.Value = userModel.PersonInfo.PeopProfile;
                        txtMoible.Value = userModel.PersonInfo.ContactMobile;
                        txtMSN.Value = userModel.PersonInfo.MSN;
                        txtQQ.Value = userModel.PersonInfo.QQ;
                        txtRemark.Value = userModel.PersonInfo.Remark;
                        txtTel.Value = userModel.PersonInfo.ContactTel;
                        rdiSex.SelectedValue = ((int)userModel.PersonInfo.ContactSex).ToString();
                        selMdepart.Value = userModel.SuperviseDepartId.ToString();
                        selBdepart.Value = userModel.DepartId.ToString();
                        txtDuty.Value = userModel.PersonInfo.JobName;
                        if (method2 != "copy") //如果不是复制则显示用户名,密码,姓名
                        {
                            txtUserName.Value = userModel.UserName;
                            txtUserName.Attributes.Add("readonly", "readonly");
                            txtPass.Value = userModel.PassWordInfo.NoEncryptPassword;
                            pass = userModel.PassWordInfo.NoEncryptPassword;
                            txtName.Value = userModel.PersonInfo.ContactName;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 保存员工信息
                bool result = false;
                //判断用户名是否已经存在
                if (method == "isexist")
                {
                    string uName = Utils.GetFormValue("uName");
                    if (method2 == "copy")
                    {
                        empId = 0;
                    }
                    result = userBll.IsExists(empId,uName, CurrentUserCompanyID);
                    Utils.ResponseMeg(true, result ? "isExist" : "noisExist");
                    return;
                }
                //验证数据完整性
                if (Utils.InputText(txtUserName.Value) == "" || Utils.InputText(txtPass.Value) == "" || Utils.InputText(txtName.Value) == "")
                {
                    MessageBox.Show(this, "数据请填写完整！");
                    return;
                }
                EyouSoft.Model.CompanyStructure.CompanyUser userModel = new EyouSoft.Model.CompanyStructure.CompanyUser();
                //如果员工编号不为空且不是复制操作则修改操作(否则为新增)
                EyouSoft.Model.CompanyStructure.ContactPersonInfo PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo();

                PersonInfo.JobName = Utils.InputText(txtDuty.Value);
                PersonInfo.ContactEmail=Utils.InputText(txtEmail.Value);
                PersonInfo.ContactFax=Utils.InputText(txtFax.Value);
                PersonInfo.PeopProfile=Utils.InputText(txtIntroduce.Value,250);
                PersonInfo.ContactMobile=Utils.InputText(txtMoible.Value);
                PersonInfo.UserType = EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户;
                PersonInfo.MSN=Utils.InputText(txtMSN.Value);
                PersonInfo.ContactName = Utils.InputText(txtName.Value);
                PersonInfo.QQ=Utils.InputText(txtQQ.Value);
                PersonInfo.Remark=Utils.InputText(txtRemark.Value,250);
                PersonInfo.ContactTel=Utils.InputText(txtTel.Value);
                PersonInfo.ContactSex= (EyouSoft.Model.EnumType.CompanyStructure.Sex)Utils.GetInt(rdiSex.SelectedValue);
                userModel.PersonInfo = PersonInfo;
                userModel.IsEnable = true;
                userModel.LastLoginTime = DateTime.Now;
                userModel.CompanyId = CurrentUserCompanyID;
                userModel.DepartId = Utils.GetInt(Utils.GetFormValue(selBdepart.UniqueID));
                userModel.SuperviseDepartName = Utils.GetFormValue("selMName");
                if (userModel.SuperviseDepartName == "选择部门")
                {
                    userModel.SuperviseDepartName = "";
                }
                userModel.DepartName = Utils.GetFormValue("selBName");
                userModel.IssueTime = DateTime.Now;
                userModel.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord { NoEncryptPassword = Utils.InputText(txtPass.Value) };
                userModel.SuperviseDepartId = Utils.GetInt(Utils.GetFormValue(selMdepart.UniqueID));
                userModel.UserName = Utils.InputText(txtUserName.Value);
                if (empId != 0&&method2!="copy")//修改
                {
                    userModel.ID = empId;
                    result = userBll.Update(userModel);
                }
                else
                {
                    result = userBll.Add(userModel);//添加
                }
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                //继续添加则刷新页面,否则关闭当前窗口
                if (method == "continue")
                {
                    MessageBox.ShowAndRedirect(this, showMess, "EmployeeEdit.aspx");
                }
                else
                {
                    MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/systemset/organize/DepartEmployee.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", showMess, Utils.GetQueryStringValue("iframeId")));
                }
                #endregion
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
