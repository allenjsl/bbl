using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SupplierControl.AirLine
{
    /// <summary>
    /// 航空公司添加修改显示页面
    /// </summary>
    /// 柴逸宁
    /// 2011.4.18
    public partial class AirLineAdd : Eyousoft.Common.Page.BackPage
    {
        #region form信息
        /// <summary>
        /// 操作类型
        /// </summary>
        private string type = string.Empty;//操作类型
        protected int tid = 0;//非零为修改ID，0表示添加
        protected bool show = false;//type查看

        /// <summary>
        /// 其他Model
        /// </summary>
        protected EyouSoft.Model.SupplierStructure.SupplierOther csModel = null;


        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限


            this.ucProvince1.CompanyId = CurrentUserCompanyID;//用户所在公司ID
            this.ucProvince1.IsFav = true;//是否为常用城市
            this.ucCity1.CompanyId = CurrentUserCompanyID;//用户所在公司ID
            this.ucCity1.IsFav = true;//是否为常用城市
            //实例化
            csModel = new EyouSoft.Model.SupplierStructure.SupplierOther();
            //实例化

            if (!IsPostBack)
            {
                type = Utils.GetQueryStringValue("type");
                switch (type)
                {
                    case "modify":
                        //判断权限
                        if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_航空公司修改))
                        {
                            bind();//获取原有数据
                        }
                        break;
                    case "show":
                        if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_航空公司栏目))
                        {
                            show = true;
                            bind();//获取原有数据
                        }
                        break;
                }
                bindProandCity();//初始化城市
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
            EyouSoft.BLL.SupplierStructure.SupplierOther ssBLl = new EyouSoft.BLL.SupplierStructure.SupplierOther();
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                csModel = ssBLl.GetModel(tid, CurrentUserCompanyID);
                txtAddress.Text = csModel.UnitAddress;
                txtNnionname.Text = csModel.UnitName;
                remark.Value = csModel.Remark;
            }
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void linkbtnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            EyouSoft.BLL.SupplierStructure.SupplierOther ssBLl = new EyouSoft.BLL.SupplierStructure.SupplierOther();
            tid = Utils.GetInt(Utils.GetFormValue("tid"));//获取表操作值
            if (tid > 0)//判断添加或修改
            {
                csModel = ssBLl.GetModel(tid, CurrentUserCompanyID);//更具表单ID于公司ID获取供应商数据
            }
            else
            {
                csModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.航空公司;
            }

            if (string.IsNullOrEmpty(csModel.AgreementFile))
            {
                string[] allowExtensions = new string[] { ".jpeg", ".jpg", ".bmp", ".gif", ".pdf", ".xls", ".xlsx", ".doc", ".docx", ".png", ".txt", ".rar" };
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
            csModel.UnitName = txtNnionname.Text;//公司名称
            csModel.UnitAddress = txtAddress.Text;//公司地址
            csModel.CompanyId = this.SiteUserInfo.CompanyID;
            csModel.OperatorId = this.SiteUserInfo.ID;
            csModel.Remark = Utils.InputText(Utils.GetFormValue("remark"));
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

            if (res)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();{2}", "保存成功!", Utils.GetQueryStringValue("iframeId"), tid > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/SupplierControl/AirLine/AirLineList.aspx';"));
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
