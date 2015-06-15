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
using System.Collections.Generic;
using EyouSoft.Common.Function;
namespace Web.SupplierControl.AreaConnect
{
    /// <summary>
    /// 功能：地接新增修改
    /// 作者：dj
    /// </summary>
    public partial class AreaAdd : Eyousoft.Common.Page.BackPage
    {
        #region form信息

        protected string type = string.Empty;
        protected int tid = 0;
        protected bool show = false;

        protected EyouSoft.Model.CompanyStructure.CompanySupplier csModel = null;
        EyouSoft.BLL.CompanyStructure.CompanySupplier csBll = null;


        #endregion

        /// <summary>
        /// 地接的增加和修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            csBll = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            csModel = new EyouSoft.Model.CompanyStructure.CompanySupplier();
            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            this.ucProvince1.IsFav = true;
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            this.ucCity1.IsFav = true;
            if (IsPostBack)
            {
                Save();
            }
            else
            {
                type = Utils.GetQueryStringValue("type");
                switch (type)
                {
                    case "modify":
                        if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_修改))
                        {
                            bind();
                        }
                        else
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_地接_修改, false);
                        }
                        break;
                    case "show":

                        show = true;
                        bind();

                        break;
                    default:
                        if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_新增))
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_地接_新增, false);
                        }
                        break;
                }
                bindProandCity();
            }
        }

        /// <summary>
        /// 初使化省份城市
        /// </summary>
        private void bindProandCity()
        {
            this.ucProvince1.ProvinceId = csModel.ProvinceId;

            this.ucCity1.CityId = csModel.CityId;

            this.ucCity1.ProvinceId = csModel.ProvinceId;

        }

        /// <summary>
        /// 绑定要修改数据
        /// </summary>
        /// <param name="tid"></param>
        private void bind()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                csModel = csBll.GetModel(tid, this.CurrentUserCompanyID);
            }
        }

        /// <summary>
        /// 保存或修改信息
        /// </summary>
        private void Save()
        {
            tid = Utils.GetInt(Utils.GetFormValue("tid"));
            if (tid > 0)
            {
                csModel = csBll.GetModel(tid, this.CurrentUserCompanyID);
            }
            else
            {
                csModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接;
            }
            //省份编号
            csModel.ProvinceId = this.ucProvince1.ProvinceId;
            //身份名称
            csModel.ProvinceName = Utils.GetFormValue("proname");
            //城市名称
            csModel.CityName = Utils.GetFormValue("cityname");
            //城市编号
            csModel.CityId = this.ucCity1.CityId;
            //单位名称
            csModel.UnitName = Utils.GetFormValue("unionname");
            if (csModel.UnitName.Length == 1)
            {
                csModel.UnitName += " ";
            }
            //地址
            csModel.UnitAddress = Utils.GetFormValue("txtAddress");
            //合作协议
            if (Request.Files.Count > 0)
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["workAgree"], "SupplierControlFile", out filepath, out oldfilename);
                if (result)
                {
                    csModel.AgreementFile = filepath;
                }
            }
            //许可证号
            csModel.LicenseKey = Utils.GetFormValue("txtLicense");
            //返佣
            csModel.Commission = Utils.GetDecimal(Utils.GetFormValue("txtReturnSet"));
            //备注
            csModel.Remark = Utils.GetFormValue("remark");
            //公司编号
            csModel.CompanyId = this.SiteUserInfo.CompanyID;
            //操作人
            csModel.OperatorId = this.SiteUserInfo.ID;
            //供应商联系人
            csModel.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();

            string[] accmanId = Utils.GetFormValues("hidcontactid");//联系人ID
            string[] accmanname = Utils.GetFormValues("inname");
            string[] accmandate = Utils.GetFormValues("indate");
            string[] accmanphone = Utils.GetFormValues("inphone");
            string[] accmanmobile = Utils.GetFormValues("inmobile");
            string[] accmanfax = Utils.GetFormValues("infax");
            string[] accmanqq = Utils.GetFormValues("inqq");
            string[] accmanUserName = Utils.GetFormValues("inusername");//用户名
            string[] accmanPwd = Utils.GetFormValues("inpwd");//密码
            string[] accmanUserId = Utils.GetFormValues("inusernameid");//用户ID

            for (int i = 0; i < accmanname.Length; i++)
            {
                EyouSoft.Model.CompanyStructure.SupplierContact scModel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                scModel.ContactName = accmanname[i];
                scModel.JobTitle = accmandate[i];
                scModel.ContactTel = accmanphone[i];
                scModel.ContactMobile = accmanmobile[i];
                scModel.QQ = accmanqq[i];
                scModel.ContactFax = accmanfax[i];
                scModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接;

                scModel.Id = Utils.GetInt(accmanId[i]);//联系人ID
                //判断对应联系人的用户名是否为空
                if (!string.IsNullOrEmpty(accmanUserName[i]))//不为空
                {
                    scModel.UserAccount = new EyouSoft.Model.CompanyStructure.UserAccount()
                    {
                        ID = Utils.GetInt(accmanUserId[i]),
                        UserName = accmanUserName[i],
                        PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord()
                        {
                            NoEncryptPassword = accmanPwd[i]
                        },
                        TourCompanyId = tid

                    };
                }
                else//为空
                {
                    //将当前联系人的用户信息初始化为空
                    scModel.UserAccount = null;
                }
                
                csModel.SupplierContact.Add(scModel);
            }

            //判断用户信息中 是否存在用户名已被使用
            bool isUserNameExist = false;
            System.Text.StringBuilder strbUserName = new System.Text.StringBuilder();
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);

            foreach (EyouSoft.Model.CompanyStructure.SupplierContact scModel in csModel.SupplierContact)
            {
                //判断当前用户信息 ，是否是 新增的用户信息
                if (scModel.UserAccount != null && scModel.UserAccount.ID == 0)//是
                {
                    //判断用户名 是否已经存在
                    if (userBll.IsExists(0, scModel.UserAccount.UserName, CurrentUserCompanyID) == true)//是
                    {
                        //修改状态
                        isUserNameExist = true;
                        //添加重复的用户名到strbUsername
                        strbUserName.Append(scModel.UserAccount.UserName).Append(",");
                    }
                }
            }

            //去除strbUserName中最后一个多余的【,】逗号
            if (strbUserName.Length > 0)
            {
                strbUserName.Remove(strbUserName.Length - 1, 1);
            }

            bool res = false;
            if (tid > 0)
            {
                res = csBll.UpdateSupplierInfo(csModel);
            }
            else
            {
                res = csBll.AddSupplierInfo(csModel);
            }

            string msg = "";//提示信息

            if (isUserNameExist == true)//如果存在用户名重复
            {
                msg = "地接信息保存成功，新增的用户名中，（"+strbUserName.ToString()+"）已被使用，请更换其他用户名";
            }
            else
            {
                msg = "地接信息保存成功";
            }

            if (res)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();{2}", msg, Utils.GetQueryStringValue("iframeId"), tid > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/SupplierControl/AreaConnect/AreaConnect.aspx';"));
            }
            else
            {
                MessageBox.ResponseScript(this, ";alert('保存失败!');");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }

    }
}
