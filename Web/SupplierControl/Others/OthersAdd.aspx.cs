using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SupplierControl.Others
{
    /// <summary>
    ///功能：其他新增修改
    ///作者：柴逸宁
    /// </summary>
    public partial class OthersAdd : Eyousoft.Common.Page.BackPage
    {

        #region form信息

        protected string type = string.Empty;//操作类型
        protected int tid = 0;//非零为修改ID，0表示添加
        protected bool show = false;//type查看

        protected EyouSoft.Model.SupplierStructure.SupplierOther csModel = null;
        EyouSoft.BLL.SupplierStructure.SupplierOther ssBLl = null;


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ssBLl = new EyouSoft.BLL.SupplierStructure.SupplierOther();
            csModel = new EyouSoft.Model.SupplierStructure.SupplierOther();

            this.ucProvince1.CompanyId = CurrentUserCompanyID;//用户所在公司ID
            this.ucProvince1.IsFav = true;//是否为常用城市
            this.ucCity1.CompanyId = CurrentUserCompanyID;//用户所在公司ID
            this.ucCity1.IsFav = true;//是否为常用城市

            if (!IsPostBack)
            {
                type = Utils.GetQueryStringValue("type");//获取操作（修改或添加）
                switch (type)
                {
                    case "modify"://如果是修改
                        if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_修改))//判断是否有修改权限
                        {
                            bind();//获取原有数据
                        }
                        else
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_其它_修改, false);//无权限输出
                        }
                        break;
                    case "show":
                       
                            show = true;
                            bind();//获取原有数据
                     
                            break;
                    default:
                        if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_新增))//判断是否有新增权限
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_其它_新增, false);//无权限输出
                        }
                        break;
                }
                bindProandCity();//初始化城市
            }
            else
            {
                Save();//保存或修改操作
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
                csModel = ssBLl.GetModel(tid, CurrentUserCompanyID);
            }
        }
        /// <summary>
        /// 保存或修改信息
        /// </summary>
        private void Save()
        {
            tid = Utils.GetInt(Utils.GetFormValue("tid"));//获取表操作值
            if (tid > 0)//判断添加或修改
            {
                csModel = ssBLl.GetModel(tid, CurrentUserCompanyID);//更具表单ID于公司ID获取供应商数据
            }
            else
            {
                csModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.其他;//
            }

            if (string.IsNullOrEmpty(csModel.AgreementFile))
            {
                string[] allowExtensions = new string[] { ".jpeg", ".jpg", ".bmp", ".gif", ".pdf", ".xls", ".xlsx", ".doc", ".docx" ,".png",".txt",".rar"};
                string msg = string.Empty;
                bool nameForm = EyouSoft.Common.Function.UploadFile.CheckFileType(Request.Files, "workAgree", allowExtensions, null, out msg);
                if (!nameForm)
                {
                    lstMsg.Text = msg;
                    return;
                }
            }

            csModel.ProvinceId = this.ucProvince1.ProvinceId;
            csModel.ProvinceName = Utils.GetFormValue("proname");//省份
            csModel.CityName = Utils.GetFormValue("cityname");//城市
            csModel.CityId = this.ucCity1.CityId;
            csModel.UnitName = Utils.GetFormValue("unionname");//公司名称
            csModel.UnitAddress = Utils.GetFormValue("txtAddress");//公司地址
            csModel.CompanyId = this.SiteUserInfo.CompanyID;
            csModel.OperatorId = this.SiteUserInfo.ID;
            csModel.Remark = Utils.GetFormValue("remark");
            if (Request.Files.Count > 0)//协议上传
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["workAgree"], "SupplierControlFile", out filepath, out oldfilename);
                if (result)
                {
                    csModel.AgreementFile = filepath;//协议名
                }

            }
            csModel.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
            string[] accmanname = Utils.GetFormValues("inname");
            string[] accmandate = Utils.GetFormValues("indate");
            string[] accmanphone = Utils.GetFormValues("inphone");
            string[] accmanmobile = Utils.GetFormValues("inmobile");
            string[] accmanfax = Utils.GetFormValues("infax");
            string[] accmanqq = Utils.GetFormValues("inqq");
            string[] accmanemail = Utils.GetFormValues("inemail");
            for (int i = 0; i < accmanname.Length; i++)
            {
                EyouSoft.Model.CompanyStructure.SupplierContact scModel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                scModel.ContactName = accmanname[i];
                scModel.JobTitle = accmandate[i];
                scModel.ContactTel = accmanphone[i];
                scModel.ContactMobile = accmanmobile[i];
                scModel.QQ = accmanqq[i];
                scModel.Email = accmanemail[i];
                scModel.ContactFax = accmanfax[i];
                scModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.其他;
                csModel.SupplierContact.Add(scModel);
            }
            bool res = false;
            if (tid > 0)
            {
                res = ssBLl.UpdateSupplierInfo(csModel);

            }
            else
            {
                res = ssBLl.AddSupplierInfo(csModel);
            }

            if (res )
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();{2}", "保存成功!", Utils.GetQueryStringValue("iframeId"), tid > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/SupplierControl/Others/Default.aspx';"));
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
