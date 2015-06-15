using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SupplierControl.Shopping
{
    public partial class ShoppingAdd : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 购物新增修改
        /// 作者：柴逸宁
        /// 2011-3-9
        /// </summary>

        #region 变量


        protected string type = string.Empty;//操作类型
        protected int tid = 0;//id
        protected bool show = false;//是否查看

        protected EyouSoft.BLL.SupplierStructure.SupplierShopping csBll = null;
        protected EyouSoft.Model.SupplierStructure.SupplierShopping ssModel = null;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            csBll = new EyouSoft.BLL.SupplierStructure.SupplierShopping();
            ssModel = new EyouSoft.Model.SupplierStructure.SupplierShopping();
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_购物_栏目, false);
                return;

            }
            ucProvince1.CompanyId = CurrentUserCompanyID;
            ucProvince1.IsFav = true;
            ucCity1.CompanyId = CurrentUserCompanyID;
            ucCity1.IsFav = true;

            if (Page.IsPostBack)
            {
                Save();
            }
            else
            {
                type = Utils.GetQueryStringValue("type");
                switch (type)
                {
                    case "modify":
                        if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_修改))
                        {
                            bind();
                        }
                        else
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_购物_修改, false);
                        }
                        break;
                    case "show":

                        show = true;
                        bind();

                        break;
                    default:
                        if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_新增))
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_购物_新增, false);
                        }
                        break;
                }
                bindProandCity();
            }
        }


        #region 保存修改数据
        private void Save()
        {
            tid = Utils.GetInt(Utils.GetFormValue("tid"));
            if (tid > 0)
            {
                ssModel = csBll.GetModel(tid);

            }
            else
            {
                ssModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.购物;
            }
            if (string.IsNullOrEmpty(ssModel.AgreementFile))
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
            ssModel.CityName = Utils.GetFormValue("cityname");
            ssModel.ProvinceName = Utils.GetFormValue("proname");
            ssModel.ProvinceId = ucProvince1.ProvinceId;
            ssModel.CityId = ucCity1.CityId;
            ssModel.UnitName = Utils.GetFormValue("unionname");
            ssModel.SaleProduct = Utils.GetFormValue("goods");
            ssModel.UnitAddress = Utils.GetFormValue("txtAddress");
            ssModel.GuideWord = Utils.GetFormValue("tourGuids");
            ssModel.Remark = Utils.GetFormValue("remark");
            ssModel.SaleProduct = Utils.GetFormValue("goodsName");
            if (Request.Files.Count > 0)
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["workAgree"], "SupplierControlFile", out filepath, out oldfilename);
                if (result)
                {
                    ssModel.AgreementFile = filepath;
                }
            }
            ssModel.CompanyId = this.SiteUserInfo.CompanyID;
            ssModel.OperatorId = this.SiteUserInfo.ID;
            ssModel.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
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
                scModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.购物;
                ssModel.SupplierContact.Add(scModel);
            }
            bool res = false;
            if (tid > 0)
            {
                res = csBll.UpdateShopping(ssModel);
            }
            else
            {
                res = csBll.AddShopping(ssModel);
            }

            if (res)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();{2}", "保存成功!", Utils.GetQueryStringValue("iframeId"), tid > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/SupplierControl/Shopping/Default.aspx';"));
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
        #endregion


        #region 初始化省份城市
        protected void bindProandCity()
        {
            this.ucProvince1.ProvinceId = ssModel.ProvinceId;

            this.ucCity1.CityId = ssModel.CityId;

            this.ucCity1.ProvinceId = ssModel.ProvinceId;
        }
        #endregion
        #region 初始化数据
        private void bind()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                //获取model
                ssModel = csBll.GetModel(tid);
            }
            else
            {
                ssModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.购物;
            }
        }
        #endregion
    }
}
