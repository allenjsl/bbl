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
using System.Web.UI.MobileControls;
using System.Collections.Generic;

namespace Web.SupplierControl.AreaConnect
{
    /// <summary>
    /// 导入地接信息
    /// dj 2011/1/22
    /// </summary>
    public partial class AreaLoadExcel : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_导入))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_地接_导入, false);
            }

            if (!IsPostBack)
            {
                string act = Utils.GetQueryStringValue("act");
                if (act == "load")
                {
                    loadXls();
                }
            }
        }

        private void loadXls()
        {
            string data = Utils.GetFormValue("dataxls");
            string[] s = data.Split(';');
            EyouSoft.BLL.CompanyStructure.CompanySupplier csBll = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            
            List<EyouSoft.Model.CompanyStructure.CompanySupplier> list = new List<EyouSoft.Model.CompanyStructure.CompanySupplier>();
            for (int i = 0; i < s.Length; i++)
            {
                string[] smodel = s[i].Split(',');
                if (smodel.Length == 6 && !string.IsNullOrEmpty(smodel[0]) && !string.IsNullOrEmpty(smodel[1]) && !string.IsNullOrEmpty(smodel[2]))
                {
                    EyouSoft.Model.CompanyStructure.CompanySupplier csModel = new EyouSoft.Model.CompanyStructure.CompanySupplier();
                    csModel.ProvinceName = HttpUtility.UrlDecode(smodel[0]);
                    csModel.CityName = HttpUtility.UrlDecode(smodel[1]);
                    csModel.UnitName = HttpUtility.UrlDecode(smodel[2]);
                    csModel.UnitAddress = HttpUtility.UrlDecode(smodel[3]);
                    csModel.Commission = Utils.GetDecimal(HttpUtility.UrlDecode(smodel[4]));
                    csModel.Remark = HttpUtility.UrlDecode(smodel[5]);
                    csModel.OperatorId = SiteUserInfo.ID;
                    csModel.IssueTime = DateTime.Now;
                    csModel.CompanyId = CurrentUserCompanyID;
                    csModel.AgreementFile = string.Empty;
                    csModel.IsDelete = false;
                    csModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接;
                    csModel.TradeNum = 0;
                    csModel.UnitPolicy = string.Empty;
                    list.Add(csModel);
                }
            }
            bool res = false;
            res = csBll.ImportExcelData(list);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();


        }

        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }


    }
}
