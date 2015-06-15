using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.Shop.T1
{
    /// <summary>
    /// 酒店
    /// </summary>
    /// 汪奇志 2012-04-03
    public partial class JiuDian : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitHotels();
            BindStars();
            InitProvince();
        }

        #region private members
        /// <summary>
        /// init hotels
        /// </summary>
        void InitHotels()
        {
            int pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int recordCount = 0;
            int pageSize = 10;
            EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo searchInfo = new EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo();

            searchInfo.ProvinceId = Utils.GetIntNull(Utils.GetQueryStringValue("provinceid"));
            searchInfo.CityId = Utils.GetIntNull(Utils.GetQueryStringValue("cityid"));
            searchInfo.Name = Server.UrlDecode(Utils.GetQueryStringValue("name"));
            searchInfo.Star = (EyouSoft.Model.EnumType.SupplierStructure.HotelStar?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.SupplierStructure.HotelStar), Utils.GetQueryStringValue("star"), null);

            if (searchInfo.ProvinceId.HasValue && searchInfo.ProvinceId.Value == 0) searchInfo.ProvinceId = null;
            if (searchInfo.CityId.HasValue && searchInfo.CityId.Value == 0) searchInfo.CityId = null;

            var items = new EyouSoft.BLL.SupplierStructure.SupplierHotel().GetSiteHotels(Master.CompanyId, pageSize, pageIndex, ref recordCount, searchInfo);
            
            if (items != null && items.Count > 0)
            {
                rpt.DataSource = items;
                rpt.DataBind();

                divPaging.Visible = true;
                divEmpty.Visible = false;

                paging.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                paging.UrlParams.Add(Request.QueryString);
                paging.intPageSize = pageSize;
                paging.CurrencyPage = pageIndex;
                paging.intRecordCount = recordCount;
            }
            else
            {
                divPaging.Visible = false;
                divEmpty.Visible = true;
            }
        }

        /// <summary>
        /// 绑定星级
        /// </summary>
        void BindStars()
        {
            txtStar.Items.Clear();
            txtStar.Items.Add(new ListItem("-请选择-", "0"));
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.SupplierStructure.HotelStar));
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var item = new ListItem(list[i].Text, list[i].Value);

                    if (list[i].Value == "1") item.Text = "三星以下";

                    txtStar.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// init province
        /// </summary>
        protected void InitProvince()
        {
            txtProvince.Items.Clear();
            txtProvince.Items.Add(new ListItem("-请选择-", "0"));
            var items = new EyouSoft.BLL.CompanyStructure.Province().GetList(Master.CompanyId);
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    txtProvince.Items.Add(new ListItem(item.ProvinceName, item.Id.ToString()));
                }
            }
        }
        #endregion

        #region protected members
        /// <summary>
        /// 获取酒店星级字符串
        /// </summary>
        /// <param name="obj">星级枚举值</param>
        /// <returns></returns>
        protected string GetStarString(int obj)
        {
            string s = string.Empty;
            EyouSoft.Model.EnumType.SupplierStructure.HotelStar star = (EyouSoft.Model.EnumType.SupplierStructure.HotelStar)obj;

            if (obj == 0)
            {
                s = "三星以下";
            }else if (star == EyouSoft.Model.EnumType.SupplierStructure.HotelStar._3星以下)
            {
                s = "三星以下";
            }
            else
            {
                s = star.ToString();
            }

            return s;
        }
        #endregion
    }
}
