using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SupplierControl.Restaurants
{
    /// <summary>
    /// 供应商管理—餐馆
    /// 李晓欢
    /// 2011-3-8
    /// </summary>
    public partial class Restaurants : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        //省份编号
        protected int? province = 0;
        //城市编号
        protected int? city = 0;
        //菜系
        protected string CuisineValue = string.Empty;
        //餐馆名称
        protected string TxtUnitsName = string.Empty;
        //列表count
        protected int len = 0;
        //操作类型 删除or导出excel
        protected string action = string.Empty;
        #endregion

        //餐馆业务逻辑类和实体类
        protected EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo Restaurantinfo = null;
        EyouSoft.BLL.SupplierStructure.SupplierRestaurant Restaurantbll = null;        

        //权限变量
        protected bool grantadd = false;//新增
        protected bool grantmodify = false;//修改
        protected bool grantdel = false;//删除
        protected bool grantload = false;//导入
        protected bool grantto = false;//导出

        protected void Page_Load(object sender, EventArgs e)
        {
            //实例化餐馆业务逻辑类和实体类
            Restaurantbll = new EyouSoft.BLL.SupplierStructure.SupplierRestaurant();
            Restaurantinfo = new EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo();            

            //权限判断
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_餐馆_栏目, true);
            }

            grantadd = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_新增);
            grantdel = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_删除);
            grantmodify = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_修改);
            grantto = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_导出);
            grantload = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_导入);

            //初始化省份和城市
            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            this.ucProvince1.IsFav = true;
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            this.ucCity1.IsFav = true;

            if (!this.Page.IsPostBack)
            {
                //操作权限 删除or导出excel
                action = Utils.GetQueryStringValue("action");
                switch (action)
                {
                    case "toexcel":
                        {
                            if (grantto)
                            {
                                CreateExcel("Restaurant" + DateTime.Now.ToShortDateString());
                            }
                        }
                        break;
                    case "Restaurantdel":
                        {
                            if (grantdel)
                            {
                                RestaurantDel();
                            }
                        }
                        break;
                    default:
                        DataInit();
                        break;
                }
            }
        }

        #region 删除
        /// <summary>
        /// 删除数据
        /// </summary>
        private void RestaurantDel()
        {
            string[] stid = Utils.GetFormValue("tid").Split(',');
            int[] tid = new int[stid.Length];
            for (int i = 0; i < stid.Length; i++)
            {
                tid[i] = Utils.GetInt(stid[i]);
            }
            bool res = false;
            //删除酒店信息
            res = Restaurantbll.DeleteRestaurantInfo(tid);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
        }
        #endregion

        #region 初始化
        protected void DataInit()
        {
            //初使化条件
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //省份
            province = Utils.GetIntNull(Utils.GetQueryStringValue("province"));
            //城市
            city = Utils.GetIntNull(Utils.GetQueryStringValue("city"));
            //单位名称
            TxtUnitsName = Utils.GetQueryStringValue("UnitsName");
            //菜系
            CuisineValue = Utils.GetQueryStringValue("CuisineValue");

            //查询条件初始化 判断
            EyouSoft.Model.SupplierStructure.SupplierRestaurantSearchInfo RestaurantSearch = new EyouSoft.Model.SupplierStructure.SupplierRestaurantSearchInfo();            
            RestaurantSearch.Cuisine = CuisineValue;
            RestaurantSearch.Name = TxtUnitsName;
            if (province == null || province <= 0)
                RestaurantSearch.ProvinceId = null;
            else
                RestaurantSearch.ProvinceId = Utils.GetIntNull(province.ToString());

            if (city == null || city <= 0)
                RestaurantSearch.CityId = null;
            else
                RestaurantSearch.CityId = Utils.GetIntNull(city.ToString());

            IList<EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo> list = null;
            list = Restaurantbll.GetRestaurants(SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, RestaurantSearch);
            if (list.Count > 0 && list != null)
            {
                len = list.Count;
                //列表数据绑定           
                this.replist.DataSource = list;
                this.replist.DataBind();
                //设置分页
                BindPage();                
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
            }
            //初始化查询省份和城市
            this.ucProvince1.ProvinceId = Utils.GetInt(province.ToString());
            this.ucCity1.CityId = Utils.GetInt(city.ToString());
            this.ucCity1.ProvinceId = Utils.GetInt(province.ToString());
            list = null;
        }
        #endregion

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            //省份
            province = Utils.GetInt(Utils.GetQueryStringValue("province"));
            //城市
            city = Utils.GetInt(Utils.GetQueryStringValue("city"));
            //单位名称
            TxtUnitsName = Utils.GetQueryStringValue("UnitsName");
            //菜系
            CuisineValue = Utils.GetQueryStringValue("CuisineValue");
            //列表数据绑定
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            EyouSoft.Model.SupplierStructure.SupplierRestaurantSearchInfo RestaurantSearch = new EyouSoft.Model.SupplierStructure.SupplierRestaurantSearchInfo();
            RestaurantSearch.Cuisine = CuisineValue;
            RestaurantSearch.Name = TxtUnitsName;
            if (province == null || province <= 0)
                RestaurantSearch.ProvinceId = null;
            else
                RestaurantSearch.ProvinceId = Utils.GetIntNull(province.ToString());
            if (city == null || city <= 0)
                RestaurantSearch.CityId = null;
            else
                RestaurantSearch.CityId = Utils.GetIntNull(city.ToString());

            //总记录数
            int count = Utils.GetInt(Utils.GetFormValue("hidRecordCount"));
            if (count == 0) count = 100;

            IList<EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo> list = null;
            list = Restaurantbll.GetRestaurants(SiteUserInfo.CompanyID, count, 1, ref RecordCount, RestaurantSearch);

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", "所在地", "单位名称", "菜系", "联系人", "电话", "传真", "交易情况");
            foreach (EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo sr in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n",
                    sr.ProvinceName + "" + sr.CityName, sr.UnitName,sr.Cuisine,
                    sr.SupplierContact == null ? "" : sr.SupplierContact[0].ContactName,
                    sr.SupplierContact == null ? "" : sr.SupplierContact[0].ContactTel,
                    sr.SupplierContact == null ? "" : sr.SupplierContact[0].ContactFax, sr.TradeNum);
            }
            Response.Write(sb.ToString());
            Response.End();
            list = null;
        }
        #endregion


        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
        #endregion
    }
}
