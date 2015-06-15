using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using EyouSoft.Common;

namespace Web.SupplierControl.Hotels
{
    /// <summary>
    /// 导入供应商管理酒店信息
    /// 李晓欢
    /// </summary>
    public partial class HotelLoadExcel :Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_导入))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_酒店_导入, false);
            }
            if(!this.Page.IsPostBack)
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
            EyouSoft.BLL.SupplierStructure.SupplierHotel hotelbll = new EyouSoft.BLL.SupplierStructure.SupplierHotel();
            IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> hotelinfo = new List<EyouSoft.Model.SupplierStructure.SupplierHotelInfo>();
            for (int i = 0; i < s.Length; i++)
            {
                string[] smodel = s[i].Split(',');
                if (smodel.Length == 18 && !string.IsNullOrEmpty(smodel[0]) && !string.IsNullOrEmpty(smodel[1]) && !string.IsNullOrEmpty(smodel[2]))
                {
                    EyouSoft.Model.SupplierStructure.SupplierHotelInfo modelhotel = new EyouSoft.Model.SupplierStructure.SupplierHotelInfo();
                    modelhotel.ProvinceName = HttpUtility.UrlDecode(smodel[0]);
                    modelhotel.CityName = HttpUtility.UrlDecode(smodel[1]);
                    modelhotel.UnitName = HttpUtility.UrlDecode(smodel[2]);
                    modelhotel.Star = Utils.GetEnumValue<EyouSoft.Model.EnumType.SupplierStructure.HotelStar>(HttpUtility.UrlDecode(smodel[3]), EyouSoft.Model.EnumType.SupplierStructure.HotelStar.挂3);
                    modelhotel.UnitAddress = HttpUtility.UrlDecode(smodel[4]);
                    modelhotel.Introduce = HttpUtility.UrlDecode(smodel[5]);
                    modelhotel.TourGuide = HttpUtility.UrlDecode(smodel[6]);

                    modelhotel.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                    EyouSoft.Model.CompanyStructure.SupplierContact scmodel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    scmodel.ContactName = HttpUtility.UrlDecode(smodel[7]);
                    scmodel.JobTitle = HttpUtility.UrlDecode(smodel[8]);
                    scmodel.ContactTel = HttpUtility.UrlDecode(smodel[9]);
                    scmodel.ContactMobile = HttpUtility.UrlDecode(smodel[10]);
                    scmodel.QQ = HttpUtility.UrlDecode(smodel[11]);
                    scmodel.Email = HttpUtility.UrlDecode(smodel[12]);
                    scmodel.CompanyId = SiteUserInfo.CompanyID;                    
                    modelhotel.SupplierContact.Add(scmodel);

                    modelhotel.OperatorId = SiteUserInfo.ID;
                    modelhotel.IssueTime = System.DateTime.Now;
                    modelhotel.CompanyId = SiteUserInfo.CompanyID;
                    modelhotel.TradeNum = 0;
                    modelhotel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.酒店;
                    modelhotel.SupplierPic = null;

                    modelhotel.RoomTypes = new List<EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo>();
                    EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo roomtype = new EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo();
                    roomtype.Name = HttpUtility.UrlDecode(smodel[13]);
                    roomtype.SellingPrice = Utils.GetDecimal(HttpUtility.UrlDecode(smodel[14]));
                    roomtype.AccountingPrice = Utils.GetDecimal(HttpUtility.UrlDecode(smodel[15]));
                    roomtype.IsBreakfast = HttpUtility.UrlDecode(smodel[16]) == "是" ? true : false;
                    modelhotel.RoomTypes.Add(roomtype);
                    modelhotel.Remark = HttpUtility.UrlDecode(smodel[17]);

                    hotelinfo.Add(modelhotel);
                }
            }           
            int res = 0;
            res = hotelbll.InsertHotels(hotelinfo);

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
