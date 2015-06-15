using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SupplierControl.Restaurants
{
    public partial class RestaurantsLoadExcel : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_导入))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_餐馆_导入, false);
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

            EyouSoft.BLL.SupplierStructure.SupplierRestaurant srbll = new EyouSoft.BLL.SupplierStructure.SupplierRestaurant();
            IList<EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo> srinfo = new List<EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo>();
                      
            for (int i = 0; i < s.Length; i++)
            {
                string[] smodel = s[i].Split(',');
                if (smodel.Length == 14 && !string.IsNullOrEmpty(smodel[0]) && !string.IsNullOrEmpty(smodel[1]) && !string.IsNullOrEmpty(smodel[2]))
                {
                    EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo srModel = new EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo();
                    srModel.ProvinceName = HttpUtility.UrlDecode(smodel[0]);
                    srModel.CityName = HttpUtility.UrlDecode(smodel[1]);
                    srModel.UnitName = HttpUtility.UrlDecode(smodel[2]);
                    srModel.Cuisine = HttpUtility.UrlDecode(smodel[3]);
                    srModel.UnitAddress = HttpUtility.UrlDecode(smodel[4]);
                    srModel.Introduce = HttpUtility.UrlDecode(smodel[5]);
                    srModel.TourGuide = HttpUtility.UrlDecode(smodel[6]);


                    srModel.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                    EyouSoft.Model.CompanyStructure.SupplierContact contect = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    contect.ContactName = HttpUtility.UrlDecode(smodel[7]);
                    contect.JobTitle = HttpUtility.UrlDecode(smodel[8]);
                    contect.ContactTel = HttpUtility.UrlDecode(smodel[9]);
                    contect.ContactMobile = HttpUtility.UrlDecode(smodel[10]);
                    contect.QQ = HttpUtility.UrlDecode(smodel[11]);
                    contect.Email = HttpUtility.UrlDecode(smodel[12]);
                    contect.CompanyId = SiteUserInfo.CompanyID;
                    srModel.SupplierContact.Add(contect);
                    srModel.Remark = HttpUtility.UrlDecode(smodel[13]);

                    srModel.OperatorId = SiteUserInfo.ID;
                    srModel.IssueTime = DateTime.Now;
                    srModel.CompanyId = CurrentUserCompanyID;
                    srModel.AgreementFile = string.Empty;
                    srModel.IsDelete = false;
                    srModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.餐馆;
                    srModel.TradeNum = 0;
                    srModel.Commission = 0;
                    srinfo.Add(srModel);
                }
            }

            int res = 0;
            res = srbll.InsertRestaurants(srinfo);

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
