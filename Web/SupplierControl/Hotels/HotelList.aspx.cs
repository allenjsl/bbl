using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SupplierControl.Hotels
{
    /// <summary>
    /// 供应商管理：酒店管理
    /// 李晓欢
    /// 2011-3-8
    /// </summary>
    public partial class HotelList :Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数

        //查询条件设置
        protected int? province = 0;
        protected int? city = 0;
        protected int HotelStartValue = 0;
        protected string TxtHotelName = string.Empty;
        protected int len = 0;
        //操作类型变量 删除 导出excel
        protected string action = string.Empty;
        #endregion


        //权限变量
        protected bool grantadd = false;//新增
        protected bool grantmodify = false;//修改
        protected bool grantdel = false;//删除
        protected bool grantload = false;//导入
        protected bool grantto = false;//导出

        //酒店业务逻辑类和实体类
        protected EyouSoft.BLL.SupplierStructure.SupplierHotel HotelBll = null;
        EyouSoft.Model.SupplierStructure.SupplierHotelInfo HotelModle = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //实例化酒店业务逻辑类和实体类
            HotelModle = new EyouSoft.Model.SupplierStructure.SupplierHotelInfo();
            HotelBll = new EyouSoft.BLL.SupplierStructure.SupplierHotel();

            //权限判断
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_酒店_栏目, true);
            }

            grantadd = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_新增);
            grantdel = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_删除);
            grantmodify = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_修改);
            grantto = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_导出);
            grantload = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_导入);

            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            this.ucProvince1.IsFav = true;
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            this.ucCity1.IsFav = true;

            if(!this.Page.IsPostBack)
            {
                BindHotelStart();

                //操作类型变量 删除 导出excel
                 action = Utils.GetQueryStringValue("action");
                 switch (action)
                 {
                     case "toexcel":
                         {
                             if (grantto)
                             {
                                 CreateExcel("Hotel" + DateTime.Now.ToShortDateString());
                             }
                         }
                         break;
                     case "hoteldel":
                         {
                             if (grantdel)
                             {
                                 HotelDel();
                             }
                         }
                         break;
                     default:
                         DataInit();
                         break;
                 }
            }
        }

        #region 绑定酒店星级
        protected void BindHotelStart()
        {
            //清空下拉框选项
            this.HotelStartList.Items.Clear();
            this.HotelStartList.Items.Add(new ListItem("--请选择酒店星级--", "-1"));
            List<EnumObj> HotelStart = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.SupplierStructure.HotelStar));
            if (HotelStart.Count > 0 && HotelStart != null)
            {
                for (int i = 0; i < HotelStart.Count; i++)
                {
                    ListItem item = new ListItem();
                    switch (HotelStart[i].Value)
                    {
                        case "1": item.Text = "3星以下"; break;
                        case "2": item.Text = "挂3"; break;
                        case "3": item.Text = "准3"; break;
                        case "4": item.Text = "挂4"; break;
                        case "5": item.Text = "准4"; break;
                        case "6": item.Text = "挂5"; break;
                        case "7": item.Text = "准5"; break;
                        default: break;
                    }
                    item.Value = HotelStart[i].Value;
                    this.HotelStartList.Items.Add(item);
                }
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除数据
        /// </summary>
        private void HotelDel()
        {
            string[] stid = Utils.GetFormValue("tid").Split(',');
            int[] tid = new int[stid.Length];
            for (int i = 0; i < stid.Length; i++)
            {
                tid[i] = Utils.GetInt(stid[i]);
            }
            bool res = false;
            //删除酒店信息
            res = HotelBll.DeleteHotelInfo(tid);

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
            province = Utils.GetIntNull(Utils.GetQueryStringValue("province"));
            city = Utils.GetIntNull(Utils.GetQueryStringValue("city"));
            TxtHotelName = Utils.GetQueryStringValue("HotelName");
            HotelStartValue = Utils.GetInt(Utils.GetQueryStringValue("HotelStartValue"));
            if (this.HotelStartList.Items.FindByValue(HotelStartValue.ToString()) != null)
            {
                this.HotelStartList.Items.FindByValue(HotelStartValue.ToString()).Selected = true;
            }

            //查询条件初始化
            EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo Searchinfo = new EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo();
            Searchinfo.Name = TxtHotelName;

            if (city == null || city <= 0)
                Searchinfo.CityId = null;
            else
                Searchinfo.CityId = city;

            if (province == null || province <= 0)
                Searchinfo.ProvinceId = null;
            else
                Searchinfo.ProvinceId = province;

            if (HotelStartValue == 0)
                Searchinfo.Star = null;
            else
                Searchinfo.Star = (EyouSoft.Model.EnumType.SupplierStructure.HotelStar)Enum.Parse(typeof(EyouSoft.Model.EnumType.SupplierStructure.HotelStar), HotelStartValue.ToString());
           
            IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> list = null;
            list = HotelBll.GetHotels(SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, Searchinfo);
            if (list.Count > 0 && list != null)
            {
                //统计列表
                len = list.Count;
                //列表数据绑定           
                this.RepList.DataSource = list;
                this.RepList.DataBind();
                //设置分页
                BindPage();               
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
            }
            //初始化查询省份城市
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
            province = Utils.GetInt(Utils.GetQueryStringValue("province"));
            city = Utils.GetInt(Utils.GetQueryStringValue("city"));
            TxtHotelName = Utils.GetQueryStringValue("HotelName");
            HotelStartValue = Utils.GetInt(Utils.GetQueryStringValue("HotelStartValue"));
            //列表数据绑定
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";
            
            EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo Searchinfo = new EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo();
            if (city == 0)
                Searchinfo.CityId = null;
            else
                Searchinfo.CityId = city;

            Searchinfo.Name = TxtHotelName;

            if (province == 0)
                Searchinfo.ProvinceId = null;
            else
                Searchinfo.ProvinceId = province;
            if (HotelStartValue == 0)
                Searchinfo.Star = null;
            else
                Searchinfo.Star = (EyouSoft.Model.EnumType.SupplierStructure.HotelStar)Enum.Parse(typeof(EyouSoft.Model.EnumType.SupplierStructure.HotelStar), HotelStartValue.ToString());

            //总记录数
            int Count = Utils.GetInt(Utils.GetFormValue("hidRecordCount"));
            if (Count == 0)
            {
                Count = 100;
            }

            IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> list = null;
            list = HotelBll.GetHotels(SiteUserInfo.CompanyID, Count, 1, ref RecordCount, Searchinfo);
            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n","所在地","单位名称","星级","联系人","电话","传真","交易情况");
            foreach (EyouSoft.Model.SupplierStructure.SupplierHotelInfo sh in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n",
                 sh.ProvinceName + " " + sh.CityName, sh.UnitName, sh.Star,
                    sh.SupplierContact == null ? "" : sh.SupplierContact[0].ContactName,
                    sh.SupplierContact == null ? "" : sh.SupplierContact[0].ContactTel,
                    sh.SupplierContact == null ? "" : sh.SupplierContact[0].ContactFax,
                    sh.TradeNum);
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

        #region 酒店星级转化
        protected string GetStarValue(string StarValue)
        {
            string Star = "";
            switch(StarValue)
            {
                case "_3星以下": Star = "3星以下"; break;
                case "挂3": Star = "挂3星"; break;
                case "准3": Star = "准3星"; break;
                case "挂4": Star = "挂4星"; break;
                case "准4": Star = "准4星"; break;
                case "挂5": Star = "挂5星"; break;
                case "准5": Star = "准5星"; break;
                case "0": Star = "无星级"; break;
                default: break;
            }
            return Star;
        }
        #endregion
    }
}
