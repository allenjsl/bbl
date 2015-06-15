using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using System.Text;

namespace Web.SupplierControl.SightManager
{
    /// <summary>
    /// 创建:万俊
    /// 功能:景点列表
    /// </summary>
    public partial class SightList : BackPage
    {
        EyouSoft.BLL.SupplierStructure.SupplierSpot sightBll = new EyouSoft.BLL.SupplierStructure.SupplierSpot();
        #region 设置分页变量
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 15;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断登陆账号是否有权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_景点_栏目, true);
                return;
            }
            if (!IsPostBack)
            {
                #region 根据登陆公司ID获取省市列表
                this.ucProvince1.CompanyId = CurrentUserCompanyID;
                this.ucProvince1.IsFav = true;
                this.ucCity1.CompanyId = CurrentUserCompanyID;
                this.ucCity1.IsFav = true;
                #endregion
                string act = Utils.GetQueryStringValue("act");              //判断是否为导出操作
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);     //取得页面上的pageindex值.
                if (act == "toexcel")
                {
                    CreateExcel("sight" + DateTime.Now.ToShortDateString());        //调用创建excel的方法
                }
                //页面初始化方法
                PageInit();
            }


        }
        #region 绑定页面数据
        protected void PageInit()
        {
            EyouSoft.Model.SupplierStructure.SupplierQuery sightSearchInfo = new EyouSoft.Model.SupplierStructure.SupplierQuery();
            IList<EyouSoft.Model.SupplierStructure.SupplierSpot> sights = new List<EyouSoft.Model.SupplierStructure.SupplierSpot>();

            #region 控件赋值
            string sight_name =  Server.UrlDecode(Utils.GetQueryStringValue("sight_name"));
            int cityId =  Utils.GetInt(Utils.GetQueryStringValue("cityId"));   
            int provinceId = Utils.GetInt(Utils.GetQueryStringValue("proid"));

            this.ucProvince1.ProvinceId = provinceId;
            this.ucCity1.ProvinceId = provinceId;
            this.ucCity1.CityId = cityId;
            this.sight_name.Value = sight_name;
            #endregion 
            #region 设置景点的查询条件值
            sightSearchInfo.CityId = cityId;
            sightSearchInfo.ProvinceId = provinceId;
            sightSearchInfo.UnitName = sight_name;
            #endregion
            sights = sightBll.GetList(pageSize, pageIndex, ref recordCount, SiteUserInfo.CompanyID, sightSearchInfo);
            this.rptList.DataSource = sights;
            this.rptList.DataBind();
            BindExportPage();           //绑定分页控件


        }
        #endregion
        #region 获得联系人信息
        /// <summary>
        /// 获得联系人信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected string GetContactInfo(object list)
        {
            string str = "";

            IList<EyouSoft.Model.CompanyStructure.SupplierContact> contactList = (List<EyouSoft.Model.CompanyStructure.SupplierContact>)list;
            if (contactList != null && contactList.Count > 0)
            {
                str = "<td align=\"center\">" + contactList[0].ContactName + "</td><td align=\"center\">" + contactList[0].ContactTel + "</td><td align=\"center\">" + contactList[0].ContactFax + "</td>";
            }
            else
            {
                str = "<td align=\"center\"></td><td align=\"center\"></td><td align=\"center\"></td>";
            }


            return str;
        }
        #endregion


        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams.Add(Request.QueryString);
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

        ///// <summary>
        ///// 导出Excel(全字段导出版)
        ///// </summary>
        //public void CreateExcel(string FileName)
        //{
        //    int province = this.ucProvince1.ProvinceId;
        //    int city = this.ucCity1.CityId;
        //    IList<EyouSoft.Model.SupplierStructure.SupplierSpot> list = null;
        //    EyouSoft.Model.SupplierStructure.SupplierQuery searchInfo = new EyouSoft.Model.SupplierStructure.SupplierQuery();
        //    searchInfo.UnitName = this.sight_name.Value;
        //    searchInfo.CityId = city;
        //    searchInfo.ProvinceId = province;
        //    list = sightBll.GetList(1, 1, ref recordCount, SiteUserInfo.CompanyID, searchInfo);
        //    if (recordCount > 0)
        //    {
        //        list = sightBll.GetList(recordCount, 1, ref recordCount, SiteUserInfo.CompanyID, searchInfo);
        //    }
        //    Response.Clear();
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
        //    Response.ContentEncoding = System.Text.Encoding.Default;
        //    Response.ContentType = "application/ms-excel";

        //    //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\n", "省份", "城市", "景点名称", "星级", "地址", "导游词", "联系人姓名", "联系人职务", "联系人电话", "联系人手机", "联系人QQ", "联系人-Email", "散客价", "团队价", "政策", "备注");
        //    foreach (EyouSoft.Model.SupplierStructure.SupplierSpot sight in list)
        //    {
        //        sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\n",
        //            sight.ProvinceName,
        //            sight.CityName,
        //            sight.UnitName,
        //            sight.Start,
        //            sight.UnitAddress,
        //            sight.TourGuide,
        //            sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].ContactName,
        //            sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].JobTitle,
        //            sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].ContactTel,
        //            sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].ContactMobile,
        //            sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].QQ,
        //            sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].Email,
        //            sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].ContactFax,
        //            sight.TravelerPrice,
        //            sight.TeamPrice,
        //            sight.UnitPolicy,
        //            sight.Remark);
        //    }
        //    Response.Write(sb.ToString());
        //    Response.End();
        //}
        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            int province = this.ucProvince1.ProvinceId;
            int city = this.ucCity1.CityId;
            IList<EyouSoft.Model.SupplierStructure.SupplierSpot> list = null;
            EyouSoft.Model.SupplierStructure.SupplierQuery searchInfo = new EyouSoft.Model.SupplierStructure.SupplierQuery();
            searchInfo.UnitName = this.sight_name.Value;
            searchInfo.CityId = city;
            searchInfo.ProvinceId = province;
            //用gerList方法取得总记录的条数
            list = sightBll.GetList(1, 1, ref recordCount, SiteUserInfo.CompanyID, searchInfo);
            if (recordCount > 0)
            {   //用上面list取得的记录条数作为每页显示条数的参数来取得相应的数据
                list = sightBll.GetList(recordCount, 1, ref recordCount, SiteUserInfo.CompanyID, searchInfo);
            }
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\n", "所在地", "景点名称", "星级", "散客价", "团队价", "联系人", "手机", "传真", "政策", "交易次数");
            foreach (EyouSoft.Model.SupplierStructure.SupplierSpot sight in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\n",
                    sight.ProvinceName+"_"+sight.CityName,
                    sight.UnitName,
                    sight.Start,
                    sight.TravelerPrice,
                    sight.TeamPrice,
                    sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].ContactName,
                    sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].ContactMobile,
                    sight.SupplierContact.Count == 0 ? "" : sight.SupplierContact[0].ContactFax,
                    sight.UnitPolicy,
                    sight.TradeNum);
            }
            Response.Write(sb.ToString());
            Response.End();
        }
        #region 权限判断
        protected bool TravelCheck(string type)
        {
            bool result = false;
            switch (type)
            {
                case "add":
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_新增);
                    break;
                case "modify":
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_修改);
                    break;
                case "toExcel":
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_导出);
                    break;
                case "load":
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_导入);
                    break;
                case "del":
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_删除);
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        #endregion
        /// <summary>
        /// 星级过滤掉"_"字符
        /// </summary>
        /// <param name="star"></param>
        /// <returns></returns>
        protected string DisponseStar(string star)
        {
            if (star.Trim() != "" || star != null)
            {
                star = star.Substring(1, star.Length - 1);
                return star;
            }
            else
            {
                return "";
            }

        }
    }

   

}
