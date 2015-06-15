using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SupplierControl.Insurance
{
    /// <summary>
    /// 供应商管理 添加保险
    /// 李晓欢
    /// 2011-3-8
    /// </summary>
    public partial class InsuranceAdd : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        //操作类型 修改or  添加
        protected string type = string.Empty;
        //定义全局变量 初始化保险编号
        protected int tid = 0;
        //保险业务逻辑类和实体类
        protected EyouSoft.Model.SupplierStructure.SupplierInsurance Insurancemodel = null;
        EyouSoft.BLL.SupplierStructure.SupplierInsurance Insurancebll = null;

        protected bool show = false;//是否查看
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //实例化业务逻辑类和实体类
             Insurancemodel= new EyouSoft.Model.SupplierStructure.SupplierInsurance();
             Insurancebll = new EyouSoft.BLL.SupplierStructure.SupplierInsurance();

            //省份和城市初始化
            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            this.ucProvince1.IsFav = true;
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            this.ucCity1.IsFav = true;

            if (!IsPostBack)
            {
                bindProandCity();
                if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_栏目))
                {
                    //操作类型 修改or添加
                    type = Utils.GetQueryStringValue("type");
                    switch (type)
                    {
                        case "modify":
                            if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_修改))
                            {
                                bind();
                            }
                            else
                            {
                                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_保险_修改, false);
                            }
                            break;
                        case "show":

                            show = true;
                            bind();

                            break;
                        default:
                            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_新增))
                            {
                                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_保险_新增, false);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 初使化省份城市
        /// </summary>
        private void bindProandCity()
        {
            this.ucProvince1.ProvinceId = Insurancemodel.ProvinceId;
            this.ucCity1.CityId = Insurancemodel.CityId;
            this.ucCity1.ProvinceId = Insurancemodel.ProvinceId;
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
                //获取保险信息
                Insurancemodel = Insurancebll.GetModel(tid, SiteUserInfo.CompanyID);
                this.ucProvince1.ProvinceId = Insurancemodel.ProvinceId;
                this.ucCity1.CityId = Insurancemodel.CityId;
                this.ucCity1.ProvinceId = Insurancemodel.ProvinceId;
            }
        }

        //弹窗设置
        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            tid = Utils.GetInt(Utils.GetFormValue("tid"));
            if (tid > 0)
            {
                //获取保险信息
                Insurancemodel = Insurancebll.GetModel(tid, SiteUserInfo.CompanyID);
            }
            else
            {
                Insurancemodel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.保险;
            }
            //省份编号
            int ProvinceId = this.ucProvince1.ProvinceId;
            string proname = "";
            EyouSoft.Model.CompanyStructure.Province proviceModel = new EyouSoft.BLL.CompanyStructure.Province().GetModel(ProvinceId);
            if (proviceModel != null)
            {
                proname = proviceModel.ProvinceName;
            }

            //城市编号
            int CityId = this.ucCity1.CityId;
            string Cityname = "";
            EyouSoft.Model.CompanyStructure.City cityModel = new EyouSoft.BLL.CompanyStructure.City().GetModel(CityId);
            if (cityModel != null)
            {
                Cityname = cityModel.CityName;
            }

            //单位名称
            string Unitsname = Utils.GetFormValue("Txtunitsnname");
            //单位地址
            string Unitaddress = Utils.GetFormValue("TxtAddress");

            //合作协议
            if (Request.Files.Count > 0)
            {
                string litmsg = "";
                if (!EyouSoft.Common.Function.UploadFile.CheckFileType(Request.Files, "workAgree", new[] { ".gif", ".jpeg", ".jpg", ".png", ".xls", ".doc", ".docx", ".rar", ".txt" }, null, out litmsg))
                {
                    MessageBox.ResponseScript(this, ";alert('" + litmsg + "');");
                    return;
                }

                string filepath = string.Empty;
                string oldfilename = string.Empty;
                if (EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["workAgree"], "SupplierControlFile", out filepath, out oldfilename))
                {
                    if (filepath.Trim() != "" && oldfilename.Trim() != "")
                    {
                        Insurancemodel.AgreementFile = filepath;
                    }
                }
            }

            //备注
            string TxtRemarks = Utils.GetFormValue("TxtRemarks");
            //当前公司编号
            int CompanyId = this.SiteUserInfo.CompanyID;
            //当前操作员编号
            int OperatorId = this.SiteUserInfo.ID;
            //联系人信息
            Insurancemodel.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
            string[] accmanname = Utils.GetFormValues("inname");
            string[] accmandate = Utils.GetFormValues("indate");
            string[] accmanphone = Utils.GetFormValues("inphone");
            string[] accmanmobile = Utils.GetFormValues("inmobile");
            string[] accmanqq = Utils.GetFormValues("inqq");
            string[] accmanemail = Utils.GetFormValues("inemail");
            string[] accmanefax = Utils.GetFormValues("inefax");
            for (int i = 0; i < accmanname.Length; i++)
            {
                EyouSoft.Model.CompanyStructure.SupplierContact scmodel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                scmodel.ContactName = accmanname[i];
                scmodel.JobTitle = accmandate[i];
                scmodel.ContactTel = accmanphone[i];
                scmodel.ContactMobile = accmanmobile[i];
                scmodel.QQ = accmanqq[i];
                scmodel.Email = accmanemail[i];
                scmodel.ContactFax = accmanefax[i];
                scmodel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.保险;
                Insurancemodel.SupplierContact.Add(scmodel);
            }
            Insurancemodel.ProvinceId = ProvinceId;
            Insurancemodel.ProvinceName = proname;
            Insurancemodel.CityId = CityId;
            Insurancemodel.CityName = Cityname;
            Insurancemodel.UnitName = Unitsname;
            Insurancemodel.UnitAddress = Unitaddress;
            Insurancemodel.Remark = TxtRemarks;
            Insurancemodel.CompanyId = CompanyId;
            Insurancemodel.OperatorId = OperatorId;
            Insurancemodel.IssueTime = System.DateTime.Now;

            bool res = false;
            if (tid > 0)
            {
                //修改保险
                res = Insurancebll.Update(Insurancemodel);
            }
            else
            {
                //添加保险
                res = Insurancebll.Add(Insurancemodel);
            }

            if (res)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide(); {2}", "保存成功!", Utils.GetQueryStringValue("iframeId"), tid > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/SupplierControl/Insurance/Insurancelist.aspx';"));
            }
            else
            {
                MessageBox.ResponseScript(this, ";alert('保存失败!');");
            }
        }
    }
}
