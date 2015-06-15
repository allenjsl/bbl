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

namespace Web.SupplierControl.TicketService
{
    /// <summary>
    /// 功能：票务新增修改
    /// 作者：dj
    /// </summary>
    public partial class TicketAdd : Eyousoft.Common.Page.BackPage
    {
        #region form信息

        protected string type = string.Empty;
        protected int tid = 0;
        protected bool show = false;//是否查看

        protected EyouSoft.Model.CompanyStructure.CompanySupplier csModel = null;
        EyouSoft.BLL.CompanyStructure.CompanySupplier csBll = null;


        #endregion
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
                        if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_票务_修改))
                        {
                            bind();
                        }
                        else
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_票务_修改, false);
                        }
                        break;
                    case "show":
                        show = true;
                        bind();

                        break;
                    default:
                        if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_票务_新增))
                        {
                            Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_票务_新增, false);
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
                csModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务;
            }
            csModel.ProvinceId = this.ucProvince1.ProvinceId;
            csModel.ProvinceName = Utils.GetFormValue("proname");
            csModel.CityName = Utils.GetFormValue("cityname");
            csModel.CityId = this.ucCity1.CityId;
            csModel.UnitName = Utils.GetFormValue("unionname");
            csModel.UnitAddress = Utils.GetFormValue("txtAddress");
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
            csModel.UnitPolicy = Utils.GetFormValue("police");
            csModel.Remark = Utils.GetFormValue("remark");
            csModel.CompanyId = this.SiteUserInfo.CompanyID;
            csModel.OperatorId = this.SiteUserInfo.ID;
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
                scModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务;
                csModel.SupplierContact.Add(scModel);
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

            if (res)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide(); {2}", "保存成功!", Utils.GetQueryStringValue("iframeId"), tid > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/SupplierControl/TicketService/TicketService.aspx';"));
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
