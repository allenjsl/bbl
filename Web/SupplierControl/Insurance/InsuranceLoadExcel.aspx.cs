using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Collections.Generic;

namespace Web.SupplierControl.Insurance
{
    /// <summary>
    /// 导入供应商管理保险信息
    /// 李晓欢
    /// </summary>
    public partial class InsuranceLoadExcel : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_导入))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_保险_导入, false);
            }
            if (!this.Page.IsPostBack)
            {
                string action = Utils.GetQueryStringValue("action");
                if (action == "load")
                {
                    loadXls();
                }
            }
        }
        private void loadXls()
        {
            string data = Utils.GetFormValue("dataxls");
            string[] s = data.Split(';');
            EyouSoft.BLL.SupplierStructure.SupplierInsurance insurancebll = new EyouSoft.BLL.SupplierStructure.SupplierInsurance();
            IList<EyouSoft.Model.SupplierStructure.SupplierInsurance> insurancemodel = new List<EyouSoft.Model.SupplierStructure.SupplierInsurance>();

            for (int i = 0; i < s.Length; i++)
            {
                string[] smodel = s[i].Split(',');
                if (smodel.Length == 11 && !string.IsNullOrEmpty(smodel[0]) && !string.IsNullOrEmpty(smodel[1]) && !string.IsNullOrEmpty(smodel[2]))
                {
                    EyouSoft.Model.SupplierStructure.SupplierInsurance slmodel = new EyouSoft.Model.SupplierStructure.SupplierInsurance();
                    slmodel.ProvinceName = HttpUtility.UrlDecode(smodel[0]);
                    slmodel.CityName = HttpUtility.UrlDecode(smodel[1]);
                    slmodel.UnitName = HttpUtility.UrlDecode(smodel[2]);
                    slmodel.UnitAddress = HttpUtility.UrlDecode(smodel[3]);

                    slmodel.SupplierContact=new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                    EyouSoft.Model.CompanyStructure.SupplierContact scmodel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    scmodel.ContactName = HttpUtility.UrlDecode(smodel[4]);
                    scmodel.JobTitle = HttpUtility.UrlDecode(smodel[5]);
                    scmodel.ContactTel = HttpUtility.UrlDecode(smodel[6]);
                    scmodel.ContactMobile = HttpUtility.UrlDecode(smodel[7]);
                    scmodel.QQ = HttpUtility.UrlDecode(smodel[8]);
                    scmodel.Email = HttpUtility.UrlDecode(smodel[9]);
                    scmodel.CompanyId = SiteUserInfo.CompanyID;                   
                    slmodel.SupplierContact.Add(scmodel);

                    slmodel.Remark = HttpUtility.UrlDecode(smodel[10]);
                    slmodel.OperatorId = SiteUserInfo.ID;
                    slmodel.IssueTime = DateTime.Now;
                    slmodel.CompanyId = CurrentUserCompanyID;
                    slmodel.AgreementFile = string.Empty;
                    slmodel.IsDelete = false;
                    slmodel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.保险;
                    slmodel.TradeNum = 0;
                    insurancemodel.Add(slmodel);
                }
            }
            int res = 0;
            res = insurancebll.AddList(insurancemodel);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res > 0 ? 1 : -1));
            Response.End();
        }

        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }
    }
}
