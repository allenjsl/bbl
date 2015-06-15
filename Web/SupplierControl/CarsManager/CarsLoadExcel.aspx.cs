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
using Eyousoft.Common.Page;


namespace Web.SupplierControl.CarsManager
{
    /// <summary>
    /// 创建:万俊
    /// 功能:车队导入数据
    /// </summary>
    public partial class CarsLoadExcel : BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
            //取得数据源
            string data = Utils.GetFormValue("dataxls");
            //对数据源分割处理
            string[] s = data.Split(';');
            EyouSoft.BLL.SupplierStructure.SupplierCarTeam carTeamBll = new EyouSoft.BLL.SupplierStructure.SupplierCarTeam();

            IList<EyouSoft.Model.SupplierStructure.SupplierCarTeam> list = new List<EyouSoft.Model.SupplierStructure.SupplierCarTeam>();
            //为list集合赋值
            for (int i = 0; i < s.Length; i++)
            {
                string[] smodel = s[i].Split(',');

                if (smodel.Length == 17 && !string.IsNullOrEmpty(smodel[0]) && !string.IsNullOrEmpty(smodel[1]) && !string.IsNullOrEmpty(smodel[2]))
                {
                    EyouSoft.Model.SupplierStructure.SupplierCarTeam carteamInfo = new EyouSoft.Model.SupplierStructure.SupplierCarTeam();
                    EyouSoft.Model.CompanyStructure.SupplierContact contact = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    EyouSoft.Model.SupplierStructure.SupplierCarInfo carInfo = new EyouSoft.Model.SupplierStructure.SupplierCarInfo(); IList<EyouSoft.Model.SupplierStructure.SupplierCarInfo> carInfos = new List<EyouSoft.Model.SupplierStructure.SupplierCarInfo>();
                    IList<EyouSoft.Model.CompanyStructure.SupplierContact> contacts = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                    carteamInfo.ProvinceName = HttpUtility.UrlDecode(smodel[0]);
                    carteamInfo.CityName = HttpUtility.UrlDecode(smodel[1]);
                    carteamInfo.UnitName = HttpUtility.UrlDecode(smodel[2]);
                    carteamInfo.UnitAddress = HttpUtility.UrlDecode(smodel[3]);
                    carInfo.GuideWord = HttpUtility.UrlDecode(smodel[4]);
                    contact.ContactName = HttpUtility.UrlDecode(smodel[5]);
                    contact.JobTitle = HttpUtility.UrlDecode(smodel[6]);
                    contact.ContactTel = HttpUtility.UrlDecode(smodel[7]);
                    contact.ContactMobile = HttpUtility.UrlDecode(smodel[8]);
                    contact.QQ = HttpUtility.UrlDecode(smodel[9]);
                    contact.Email = HttpUtility.UrlDecode(smodel[10]);
                    carInfo.CarType = HttpUtility.UrlDecode(smodel[11]);
                    carInfo.CarNumber = HttpUtility.UrlDecode(smodel[12]);
                    carInfo.Price = Utils.GetDecimal(HttpUtility.UrlDecode(smodel[13]));
                    carInfo.DriverName = HttpUtility.UrlDecode(smodel[14]);
                    carInfo.DriverPhone = HttpUtility.UrlDecode(smodel[15]);
                    carteamInfo.Remark = HttpUtility.UrlDecode(smodel[16]);
                    carteamInfo.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.车队;
                    carteamInfo.TradeNum = 0;
                    carteamInfo.CompanyId = SiteUserInfo.CompanyID;
                    carInfos.Add(carInfo);
                    contacts.Add(contact);
                    carteamInfo.CarsInfo = carInfos;
                    carteamInfo.SupplierContact = contacts;
                    list.Add(carteamInfo);
                }
            }
            int res = 0;
            //导入从excel获得数据
            res = carTeamBll.InsertCarTeams(list);
            bool result=false;
            if (res > 0)
            {
                result = true;
            }

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", result ? 1 : -1));
            Response.End();


        }
        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }
    }
}
