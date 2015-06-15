using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SupplierControl.SightManager
{
    /// <summary>
    /// 创建:万俊
    /// 功能:导入景点数据
    /// </summary>
    public partial class SightLoadExcel : BackPage
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
        //导入方法
        private void loadXls()
        {
            //取得数据源
            string data = Utils.GetFormValue("dataxls");
            //分割数据
            string[] s = data.Split(';');
            EyouSoft.BLL.SupplierStructure.SupplierSpot sightBll = new EyouSoft.BLL.SupplierStructure.SupplierSpot();

            IList<EyouSoft.Model.SupplierStructure.SupplierSpot> list = new List<EyouSoft.Model.SupplierStructure.SupplierSpot>();
            for (int i = 0; i < s.Length; i++)
            {
                string[] smodel = s[i].Split(',');
                if (smodel.Length == 16 && !string.IsNullOrEmpty(smodel[0]) && !string.IsNullOrEmpty(smodel[1]) && !string.IsNullOrEmpty(smodel[2]))
                {
                    EyouSoft.Model.SupplierStructure.SupplierSpot sight = new EyouSoft.Model.SupplierStructure.SupplierSpot();
                    IList<EyouSoft.Model.CompanyStructure.SupplierContact> contacts = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                    EyouSoft.Model.CompanyStructure.SupplierContact contact = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    sight.ProvinceName =HttpUtility.UrlDecode(smodel[0]);
                    sight.CityName = HttpUtility.UrlDecode(smodel[1]);
                    sight.UnitName = HttpUtility.UrlDecode(smodel[2]);
                    sight.Start = (EyouSoft.Model.EnumType.SupplierStructure.ScenicSpotStar)ChangeStar(HttpUtility.UrlDecode(smodel[3]));
                    sight.UnitAddress = HttpUtility.UrlDecode(smodel[4]);
                    sight.TourGuide = HttpUtility.UrlDecode(smodel[5]);
                    contact.ContactName = HttpUtility.UrlDecode(smodel[6]);
                    contact.JobTitle = HttpUtility.UrlDecode(smodel[7]);
                    contact.ContactTel = HttpUtility.UrlDecode(smodel[8]);
                    contact.ContactMobile = HttpUtility.UrlDecode(smodel[9]);
                    contact.QQ = HttpUtility.UrlDecode(smodel[10]);
                    contact.Email = HttpUtility.UrlDecode(smodel[11]);
                    sight.TravelerPrice =Utils.GetDecimal(HttpUtility.UrlDecode(smodel[12]));
                    sight.TeamPrice =Utils.GetDecimal( HttpUtility.UrlDecode(smodel[13]));
                    sight.UnitPolicy = HttpUtility.UrlDecode(smodel[14]);
                    sight.Remark = HttpUtility.UrlDecode(smodel[15]);
                    sight.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.景点;
                    sight.CompanyId = SiteUserInfo.CompanyID;
                    if (contact.ContactMobile != "" && contact.ContactName != "")
                    {
                        contacts.Add(contact);
                    }
                    sight.SupplierContact = contacts;
                    list.Add(sight);
                }
            }
            bool res = false;

            res = sightBll.Add(list);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();


        }
        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }

        /// <summary>
        /// 转换星级
        /// </summary>
        /// <param name="star"></param>
        /// <returns></returns>
        protected int ChangeStar(string star)
        {
            int result = 0;
            if (star == "1星" || star == "_1星" || star=="1星级" || star=="_1星级" || star=="一星级")
            {
                result = 1;
            }
            else if (star == "2星" || star == "_2星" || star == "2星级" || star == "_2星级" || star == "二星级")
            {
                result = 2;
            }
            else if (star == "3星" || star == "_3星" || star == "3星级" || star == "_3星级" || star == "三星级")
            {
                result = 3;
            }
            else if (star == "4星" || star == "_4星" || star == "4星级" || star == "_4星级" || star == "四星级") 
            {
                result = 4;
            }
            else if (star == "5星" || star == "_5星" || star == "5星级" || star == "_5星级" || star == "五星级")
            {
                result = 5;   
            }
            return result;
        }
    }
}
