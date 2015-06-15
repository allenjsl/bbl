using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.CRM.customerinfos
{
    /// <summary>
    /// 客户资料修改
    /// xuty 2011/01/17
    /// </summary>
    /// 
    /// <summary>
    /// 模块名称:客户关系管理
    /// 功能说明：客户关系管理添加结算日期字段
    /// 修改人：李晓欢
    /// 修改时间：2011-05-26
    /// </summary>
    /// 模块名称:客户关系管理
    /// 修改时间：2011-6-27
    /// 修改内容：添加查看详情功能
    /// 修改人：柴逸宁
    public partial class CustomerEdit : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 是否为显示详情
        /// </summary>
        protected bool isshow = false;
        protected int contactNum = 1;//联系人数量
        protected string strContactHtml;//联系人html
        protected bool HasPermit;//是否有权限
        protected bool hasAccountPermit;//是否有分配账号权限
        protected bool IsStartState = true;//当前账户是否是停用状态
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.AccountDaylist.Items.Clear();
                string[] intarry = new string[32];
                for (int i = 0; i < 32; i++)
                {
                    if (i == 0)
                    {
                        intarry[0] = "请选择";
                    }
                    else
                    {
                        intarry[i] = (i).ToString();
                    }
                }
                if (intarry != null && intarry.Length > 0)
                {
                    this.AccountDaylist.DataSource = intarry;
                    this.AccountDaylist.DataBind();
                }
            }
            #region 设置公司ID初始化省份城市列表
            ucProvince1.CompanyId = CurrentUserCompanyID;
            ucProvince1.IsFav = true;
            ucCity1.CompanyId = CurrentUserCompanyID;
            ucCity1.IsFav = true;
            #endregion

            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();//客户资料bll
            EyouSoft.Model.CompanyStructure.CustomerInfo custModel = null;//客户资料实体
            string method = Utils.GetFormValue("hidMethod");//当前操作
            int custId = Utils.GetInt(Utils.GetQueryStringValue("custId"));//客户Id
            string type = Utils.GetQueryStringValue("type");
            if (type == "show")
            {
                isshow = true;
            }
            EyouSoft.BLL.CompanyStructure.CompanyUser user = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            if (method == "save")
            {
                #region 保存客户资料
                custModel = new EyouSoft.Model.CompanyStructure.CustomerInfo();
                custModel.Name = Utils.InputText(txtCompany.Value);//单位名称
                custModel.Licence = Utils.InputText(txtLicense.Value);//许可证号
                custModel.Adress = Utils.InputText(txtAddress.Value);//地址
                custModel.PostalCode = Utils.InputText(txtPostalCode.Value);//邮编
                custModel.BankAccount = Utils.InputText(txtBankCode.Value);//银行账号
                custModel.PreDeposit = Utils.GetDecimal(Utils.InputText(txtBeforeMoney.Value));//预存款
                custModel.MaxDebts = Utils.GetDecimal(Utils.InputText(txtArrears.Value));//最高欠款
                custModel.CommissionType = (EyouSoft.Model.EnumType.CompanyStructure.CommissionType)Utils.GetInt(Utils.InputText(rdiBackType.SelectedValue));//反佣类型
                custModel.CommissionCount = Utils.GetDecimal(Utils.InputText(txtBackMoney.Value));//反佣金额
                custModel.CustomerLev = Utils.GetInt(Utils.GetFormValue(selCustLevel.UniqueID));//客户等级

                custModel.JieSuanType = (EyouSoft.Model.EnumType.CompanyStructure.KHJieSuanType)Utils.GetInt(ddl_checkType.SelectedValue);
                custModel.IsRequiredTourCode = rad_IsShowGroupNo.SelectedValue == "true" ? true : false;

                //ddl_checkType
                //结算日期
                if (Utils.GetFormValue("AccountTypeRadio") == "1")
                {
                    custModel.AccountDayType = EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Month;
                    custModel.AccountDay = Utils.GetInt(Utils.GetFormValue(AccountDaylist.UniqueID));
                }
                else
                {
                    custModel.AccountDayType = EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Week;
                    custModel.AccountDay = Utils.GetInt(Utils.GetFormValue("checkDay"));
                }

                custModel.AccountWay = Utils.InputText(Utils.GetFormValue(AccountType.UniqueID));//结算方式
                custModel.ContactName = Utils.InputText(txtMainContact.Value);//主要联系人
                custModel.Phone = Utils.InputText(txtMainTel.Value);//电话
                custModel.Mobile = Utils.InputText(txtMainMobile.Value);//手机
                custModel.Fax = Utils.InputText(txtMainFax.Value);//传真
                custModel.Remark = Utils.InputText(txtRemark.Value);//备注
                custModel.ProviceId = this.ucProvince1.ProvinceId;
                custModel.CityId = this.ucCity1.CityId;
                custModel.ProvinceName = Utils.GetFormValue("pName");
                if (custModel.ProvinceName == "请选择")
                {
                    custModel.ProvinceName = "";
                }
                custModel.CityName = Utils.GetFormValue("cName");
                if (custModel.CityName == "请选择")
                {
                    custModel.CityName = "";
                }
                custModel.CompanyId = CurrentUserCompanyID;
                custModel.SaleId = Utils.GetInt(Utils.GetFormValue(selDutySaler.UniqueID));
                custModel.Saler = Utils.GetFormValue("hidSaler");
                custModel.IssueTime = DateTime.Now;
                custModel.IsEnable = true;
                custModel.IsDelete = false;

                custModel.BrandId = Utils.GetInt(Utils.GetFormValue(selBrand.UniqueID));
                string showMess = "数据保存成功！";
                string[] contactId = Utils.GetFormValues("contactId");
                string[] custName = Utils.GetFormValues("txtContact");//姓名
                string[] custSex = Utils.GetFormValues("selSex");//性别
                string[] custDepart = Utils.GetFormValues("txtDepart");//部门
                string[] custBirth = Utils.GetFormValues("txtBirth");//生日
                string[] custDuty = Utils.GetFormValues("txtDuty");//职务
                string[] custMobile = Utils.GetFormValues("txtMobile");//手机
                string[] custTel = Utils.GetFormValues("txtTel");//电话
                string[] cusFax = Utils.GetFormValues("txtFax"); //传真
                string[] custQQ = Utils.GetFormValues("txtQQ");//QQ
                string[] custEmail = Utils.GetFormValues("txtEmail");//用户邮箱
                string[] custUserName = Utils.GetFormValues("txtUserName");//用户名
                string[] custUserId = Utils.GetFormValues("userId");//用户ID
                string[] custPwd = Utils.GetFormValues("txtPwd");//密码
                string[] custPwdSure = Utils.GetFormValues("txtPwdSure");//确认密码
                string[] splitStr = new string[] { "|," };
                string[] custAreaId = Utils.GetFormValue("custHidArea").Split(splitStr, StringSplitOptions.None);//线路区域
                string[] custAreaName = Utils.GetFormValue("txtRouteArea").Split(splitStr, StringSplitOptions.None);//线路区域
                IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> custList = new List<EyouSoft.Model.CompanyStructure.CustomerContactInfo>();
                StringBuilder repartUserName = new StringBuilder();
                for (int i = 0, len = custName.Length; i < len; i++)
                {
                    if (i == 0 && string.IsNullOrEmpty(custName[i]) || custName[i] == "")
                        continue;
                    if (String.IsNullOrEmpty(custName[i]))
                    {
                        MessageBox.ShowAndRedirect(this, "请填写完整姓名！", this.Request.Url.ToString());
                        break;
                    }
                    if (custUserId[i] == "0")
                    {
                        if (user.IsExists(0, custUserName[i], CurrentUserCompanyID))
                        {
                            repartUserName.AppendFormat("{0}，", custUserName[i]);
                            //continue;
                        }
                    }
                    EyouSoft.Model.CompanyStructure.CustomerContactInfo custInfo = new EyouSoft.Model.CompanyStructure.CustomerContactInfo();
                    custInfo.BirthDay = Utils.GetDateTime(custBirth[i], DateTime.Now);
                    custInfo.Department = custDepart[i];
                    custInfo.IsDelete = "0";
                    custInfo.Email = string.Empty;
                    custInfo.Job = custDuty[i];
                    custInfo.Mobile = custMobile[i];
                    custInfo.Tel = custTel[i];
                    custInfo.Fax = cusFax[i];
                    custInfo.ID = Utils.GetInt(contactId[i]);
                    custInfo.Name = custName[i];
                    custInfo.qq = custQQ[i];
                    custInfo.Sex = custSex[i];
                    custModel.IssueTime = DateTime.Now;
                    if (!user.IsExists(Utils.GetInt(custUserId[i]), custUserName[i], CurrentUserCompanyID) || Utils.GetInt(custUserId[i]) != 0)
                    {
                        custInfo.UserAccount = new EyouSoft.Model.CompanyStructure.UserAccount();
                        custInfo.UserAccount.CompanyId = CurrentUserCompanyID;
                        custInfo.UserAccount.UserName = custUserName[i];
                        custInfo.UserAccount.UserAreaList = new List<EyouSoft.Model.CompanyStructure.UserArea>();
                        string areaIds = custAreaId[i].Trim('|');
                        string areaNames = custAreaName[i].Trim('|');
                        custInfo.AreaIds = areaIds;
                        custInfo.AreaNames = areaNames;
                        custInfo.UserAccount.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord { NoEncryptPassword = custPwd[i] };
                    }
                    custList.Add(custInfo);
                }
                custModel.CustomerContactList = custList;
                string strRepeat = repartUserName.ToString().TrimEnd('，');

                bool result = false;
                int resultInt = 0;
                if (custId != 0)
                {
                    //修改
                    custModel.Id = custId;
                    resultInt = custBll.UpdateCustomer(custModel);
                    result = resultInt == 1 || resultInt == 9;
                    var splitStr2 = new string[] { "," };
                    string[] delIds = Utils.GetFormValue("delIds").Split(splitStr2, StringSplitOptions.RemoveEmptyEntries);
                    if (delIds.Length > 0)
                    {
                        result = custBll.DeleteCustomerContacter(delIds);
                    }
                }
                else
                {
                    //添加
                    resultInt = custBll.AddCustomer(custModel);
                    result = resultInt == 1 || resultInt == 9;
                }
                if (!result)
                {

                    showMess = "数据保存失败";
                }
                else
                {
                    if (strRepeat != "")
                    {
                        showMess = strRepeat + "账户已经存在将不会添加！";
                    }
                }
                Utils.ShowMsgAndCloseBoxy(showMess, Utils.GetQueryStringValue("iframeId"), true);
                //MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/CRM/customerinfos/CustomerList.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", showMess, Utils.GetQueryStringValue("iframeId")));
                #endregion
            }
            else
            {
                #region 初始化加载客户资料
                EyouSoft.BLL.CompanyStructure.CompanyBrand brandBll = new EyouSoft.BLL.CompanyStructure.CompanyBrand();
                int recordCount = 0;
                //初始化绑定品牌列表
                IList<EyouSoft.Model.CompanyStructure.CompanyBrand> listB = brandBll.GetList(100000, 1, ref recordCount, CurrentUserCompanyID);
                selBrand.DataTextField = "BrandName";
                selBrand.DataValueField = "Id";
                selBrand.DataSource = listB;
                selBrand.DataBind();
                selBrand.Items.Insert(0, new ListItem("请选择", ""));
                //初始化绑定客户等级列表
                EyouSoft.BLL.CompanyStructure.CompanyCustomStand customStandBll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();//初始化custbll
                IList<EyouSoft.Model.CompanyStructure.CustomStand> list = customStandBll.GetList(100000, 1, ref recordCount, CurrentUserCompanyID);
                selCustLevel.DataTextField = "CustomStandName";
                selCustLevel.DataValueField = "Id";
                selCustLevel.DataSource = list;
                selCustLevel.DataBind();
                selCustLevel.Items.Insert(0, new ListItem("请选择", ""));
                //初始化绑定部门员工列表
                EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser();
                IList<EyouSoft.Model.CompanyStructure.CompanyUser> userList = userBll.GetCompanyUser(CurrentUserCompanyID);
                if (userList != null && userList.Count > 0)
                {
                    foreach (EyouSoft.Model.CompanyStructure.CompanyUser userItem in userList)
                    {
                        selDutySaler.Items.Add(new ListItem(userItem.PersonInfo.ContactName, userItem.ID.ToString()));
                    }
                }
                selDutySaler.Items.Insert(0, new ListItem("请选择", ""));
                //判断是否有分配账号权限(隐藏或显示分配账号按钮)
                if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_分配帐号))
                {
                    hasAccountPermit = true;
                }
                if (custId != 0)
                {
                    //初始化数据获取客户资料实体
                    custModel = custBll.GetCustomerModel(custId);
                    if (custModel != null)
                    {
                        IsStartState = custModel.IsEnable;
                        txtCompany.Value = custModel.Name;//单位名称
                        txtLicense.Value = custModel.Licence;//许可证号
                        txtAddress.Value = custModel.Adress;//地址
                        txtPostalCode.Value = custModel.PostalCode;//邮编
                        txtBankCode.Value = custModel.BankAccount;//银行账号
                        txtBeforeMoney.Value = custModel.PreDeposit.ToString("F2");//预存款
                        txtArrears.Value = custModel.MaxDebts.ToString("F2");//最高欠款
                        rdiBackType.SelectedValue = ((int)custModel.CommissionType).ToString();//反佣类型
                        txtBackMoney.Value = custModel.CommissionCount.ToString("F2");//反佣金额
                        selCustLevel.Value = custModel.CustomerLev.ToString();//客户等级


                        ddl_checkType.SelectedValue = ((int)custModel.JieSuanType).ToString();
                        rad_IsShowGroupNo.SelectedValue = (bool)custModel.IsRequiredTourCode ? "true" : "false";

                        //结算日期 按周
                        if (custModel.AccountDayType == EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Week)
                        {
                            this.AccountTypeRadio1.Checked = false;
                            this.AccountTypeRadio2.Checked = true;
                            this.AccountDaylist.Attributes.Add("disabled", "disabled");
                            if (custModel.AccountDay != 0)
                            {
                                switch (custModel.AccountDay)
                                {
                                    case 1:
                                        this.checkDay1.Checked = true;
                                        break;
                                    case 2:
                                        this.checkDay1.Checked = false;
                                        this.checkDay2.Checked = true;
                                        break;
                                    case 3:
                                        this.checkDay1.Checked = false;
                                        this.checkDay3.Checked = true;
                                        break;
                                    case 4:
                                        this.checkDay1.Checked = false;
                                        this.checkDay4.Checked = true;
                                        break;
                                    case 5:
                                        this.checkDay1.Checked = false;
                                        this.checkDay5.Checked = true;
                                        break;
                                    case 6:
                                        this.checkDay1.Checked = false;
                                        this.checkDay6.Checked = true;
                                        break;
                                    case 7:
                                        this.checkDay1.Checked = false;
                                        this.checkDay7.Checked = true;
                                        break;
                                }
                            }

                        }//按每月X几
                        else
                        {
                            this.AccountTypeRadio1.Checked = true;
                            this.AccountTypeRadio2.Checked = false;
                            this.checkDay1.Attributes.Add("disabled", "disabled"); this.checkDay3.Attributes.Add("disabled", "disabled");
                            this.checkDay2.Attributes.Add("disabled", "disabled"); this.checkDay4.Attributes.Add("disabled", "disabled");
                            this.checkDay5.Attributes.Add("disabled", "disabled"); this.checkDay6.Attributes.Add("disabled", "disabled"); this.checkDay7.Attributes.Add("disabled", "disabled");
                            if (custModel.AccountDay != 0)
                            {
                                this.AccountDaylist.Items.FindByValue(custModel.AccountDay.ToString()).Selected = true;
                            }
                        }

                        //结算方式
                        this.AccountType.Value = custModel.AccountWay;
                        txtMainContact.Value = custModel.ContactName;//主要联系人
                        txtMainTel.Value = custModel.Phone;//电话
                        txtMainMobile.Value = custModel.Mobile;//手机
                        txtMainFax.Value = custModel.Fax;//传真
                        txtRemark.Value = custModel.Remark;//备注
                        selBrand.Value = custModel.BrandId.ToString();//品牌
                        selDutySaler.Value = custModel.SaleId.ToString();//责任销售

                        ucProvince1.ProvinceId = custModel.ProviceId;//省份
                        ucCity1.CityId = custModel.CityId;//城市
                        ucCity1.ProvinceId = custModel.ProviceId;
                        //获取联系人集合
                        IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> custList = custModel.CustomerContactList;
                        if (custList != null && custList.Count > 0)
                        {
                            GetContactHtml(custList);
                        }
                        //如果有修改权限则显示保存按钮
                        if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_修改客户))
                        {
                            HasPermit = true;
                        }
                    }

                }
                else
                {   //如果有新增权限则显示保存按钮
                    if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_新增客户))
                    {
                        HasPermit = true;
                    }
                }
                #endregion
            }

        }

        #region 构造联系人信息html
        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <param name="list">联系人集合</param>
        protected void GetContactHtml(IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> list)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (EyouSoft.Model.CompanyStructure.CustomerContactInfo info in list)
            {
                //构造基本信息
                strBuilder.AppendFormat("<tr class=\"even\" id=\"tr1\">" +
              "<th height=\"30\" align=\"center\"><input name=\"txtContact\" type=\"text\"  class=\"searchinput\"  value=\"{0}\"/><input type='hidden' value='{8}' name='contactId'/></th>" +
              "<th align=\"center\"><select name=\"selSex\"><option value=\"0\" {1}>男</option><option value=\"1\" {2}>女</option>" +
              "</select></th><th align=\"center\"><input name=\"txtBirth\" type=\"text\"  class=\"searchinput\" value=\"{3}\" onfocus=\"WdatePicker()\"/></th>" +
              "<td align=\"center\">部门: <input name=\"txtDepart\" type=\"text\"  class=\"jiesuantext\" value=\"{4}\" /><br />职务:" +
              "<input name=\"txtDuty\" type=\"text\"  class=\"jiesuantext\" value=\"{5}\" /></td>" +
              "<th align=\"center\"><input name=\"txtMobile\" type=\"text\"  class=\"searchinput\" style=\"width: 95%\"  value=\"{6}\" /></th>" +
              "<th align=\"center\"><input name=\"txtTel\" type=\"text\"  class=\"searchinput\" style=\"width: 95%\" value=\"{11}\" /></th>" +
              "<th align=\"center\"><input name=\"txtFax\" type=\"text\"  class=\"searchinput\" value=\"{12}\" /></th>" +
              "<th align=\"center\"><input name=\"txtQQ\" type=\"text\"  class=\"searchinput\" value=\"{7}\" /></th>" +
              "<td  align=\"center\" {9} ><a href=\"javascript:;\" onclick=\"return CustEdit.del(this);return false;\"><img src=\"/images/delicon01.gif\" width=\"14\" height=\"14\" /> 删除</a>&nbsp;{10}<input type=\"hidden\" name=\"hidCodeInfo\" value=\"\" /></td>" +
              "</tr>"
              , info.Name
              , info.Sex == "男" ? "selected=\"selected\"" : ""
              , info.Sex == "女" ? "selected=\"selected\"" : ""
              , info.BirthDay.HasValue ? info.BirthDay.Value.ToString("yyyy-MM-dd") : ""
              , info.Department
              , info.Job
              , info.Mobile
              , info.qq
              , info.ID
              , ""
              , hasAccountPermit ? "<br/><a href=\"javascript:;\" onclick=\"return CustEdit.setCode('',this)\" >分配账号</a>" : ""
              , info.Tel
              , info.Fax);
                //如果账户存在则构造联系人账户否则输出空的内容
                if (info.UserAccount != null)
                {

                    strBuilder.AppendFormat("<tr class=\"even\" name='cinfo'> <th height=\"40\" align=\"center\" colspan=\"9\">" +
               "用户名：<input type=\"text\" name=\"txtUserName\" style=\"width:80px\" value=\"{0}\"  readonly='readonly' /><input type='hidden' value='{4}' name='userId'/>密码：<input  type=\"password\" name=\"txtPwd\" style=\"width:90px\" value=\"{1}\"/>" +
               "确认密码：<input type=\"password\" name=\"txtPwdSure\" style=\"width:90px\" value=\"{1}\"/>线路区域：<input type=\"text\" name=\"txtRouteArea\" style=\"width:120px\" value=\"{3}\" readonly='readonly'/><input type=\"hidden\" name=\"custHidArea\" value=\"{2}|\" />" +
               "<a href=\"javascript:;\" onclick=\"return CustEdit.selArea('',this);\"><img src=\"/images/sanping_04.gif\" width=\"28\" height=\"18\" /></a></th></tr>", info.UserAccount.UserName, info.UserAccount.PassWordInfo != null ? info.UserAccount.PassWordInfo.NoEncryptPassword : "", info.AreaIds, info.AreaNames, info.ID);
                }
                else
                {
                    strBuilder.AppendFormat("<tr class=\"even\"  name='cinfo' style=\"display:none\" > <th height=\"40\" align=\"center\" colspan=\"9\">" +
                "用户名：<input type=\"text\" name=\"txtUserName\" style=\"width:80px\"  /><input type='hidden' value='0' name='userId'/>密码：<input  type=\"password\" name=\"txtPwd\" style=\"width:90px\" />" +
                "确认密码：<input type=\"password\" name=\"txtPwdSure\" style=\"width:90px\" />线路区域：<input type=\"text\" name=\"txtRouteArea\" style=\"width:120px\" /><input type=\"hidden\" name=\"custHidArea\" value=\"|\" />" +
                 "<a href=\"javascript:;\" onclick=\"return CustEdit.selArea('',this);\"><img src=\"/images/sanping_04.gif\" width=\"28\" height=\"18\" /></a></th></tr>");
                }

            }
            strContactHtml = strBuilder.ToString();
        }
        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
