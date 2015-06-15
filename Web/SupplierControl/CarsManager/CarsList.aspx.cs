using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using System.Data;
using System.Data.Sql;
using EyouSoft.Common;
using System.Text;

namespace Web.SupplierControl.CarsManager
{
    /// <summary>
    /// 功能:车队列表管理
    /// 创建:万俊
    /// </summary>
    public partial class CarsList : BackPage
    {
        EyouSoft.BLL.SupplierStructure.SupplierCarTeam carTeamManager = new EyouSoft.BLL.SupplierStructure.SupplierCarTeam(); // 车队BLL
        #region 设置分页变量
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 15;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_车队_栏目, true);
                return;
            }

            if (!IsPostBack)
            {
                //当前页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                string act = Utils.GetQueryStringValue("act");
                #region 根据登陆公司ID获取省市列表
                this.ucProvince1.CompanyId = CurrentUserCompanyID;
                this.ucProvince1.IsFav = true;
                this.ucCity1.CompanyId = CurrentUserCompanyID;
                this.ucCity1.IsFav = true;
                #endregion
                //获取车队信息
                BindCarTeamsBySearh();
                if (act == "toexcel")
                {
                    CreateExcel("carteam" + DateTime.Now.ToShortDateString());
                }
            }

        }

        #region 加载车队信息
        protected void BindCarTeamsBySearh()
        {

            //生成车队类
            EyouSoft.Model.SupplierStructure.SupplierCarTeam carTeam = new EyouSoft.Model.SupplierStructure.SupplierCarTeam();
            //生成查询条件方法
            EyouSoft.Model.SupplierStructure.SupplierCarTeamSearchInfo searInfo = new EyouSoft.Model.SupplierStructure.SupplierCarTeamSearchInfo();

            #region 控件赋值
            string carType = Server.UrlDecode(Utils.GetQueryStringValue("cartype"));
            string carName = Server.UrlDecode(Utils.GetQueryStringValue("carname"));
            int cityId = Utils.GetInt(Utils.GetQueryStringValue("cityId"));
            int provinceId = Utils.GetInt(Utils.GetQueryStringValue("proid"));

            this.ucProvince1.ProvinceId = provinceId;
            this.ucCity1.ProvinceId = provinceId;
            this.ucCity1.CityId = cityId;
            this.car_type.Value = carType;
            this.carTeam_name.Value = carName;
            #endregion


            searInfo.CarType = carType;
            searInfo.CityId = cityId;
            searInfo.Name =carName;
            searInfo.ProvinceId = provinceId;
            IList<EyouSoft.Model.SupplierStructure.SupplierCarTeam> carTeams = carTeamManager.GetCarTeams(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, searInfo);
            this.rptList.DataSource = carTeams;
            this.rptList.DataBind();
            BindExportPage();

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
                str = "<td align=\"center\"></td><td align=\"center\"> </td><td align=\"center\"></td>";
            }


            return str;
        }
        #endregion
        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            int province = this.ucProvince1.ProvinceId;
            int city = this.ucCity1.CityId;
            string carTeam_Name = Utils.GetQueryStringValue("carname");
            string carType = Utils.GetQueryStringValue("cartype");
            IList<EyouSoft.Model.SupplierStructure.SupplierCarTeam> list = null;
            EyouSoft.Model.SupplierStructure.SupplierCarTeamSearchInfo searchInfo = new EyouSoft.Model.SupplierStructure.SupplierCarTeamSearchInfo();
            searchInfo.CarType = carType;
            searchInfo.CityId = city;
            searchInfo.Name = carTeam_Name;
            searchInfo.ProvinceId = province;
            list = carTeamManager.GetCarTeams(SiteUserInfo.CompanyID, 1, 1, ref recordCount, searchInfo);
            if (recordCount > 0)
            {
                list = carTeamManager.GetCarTeams(SiteUserInfo.CompanyID, recordCount, 1, ref recordCount, searchInfo);
            }
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", "所在地", "单位名称", "地址", "联系人姓名", "手机", "传真", "交易次数");
            foreach (EyouSoft.Model.SupplierStructure.SupplierCarTeam ct in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n",
                    ct.ProvinceName + " " + ct.CityName,
                    ct.UnitName,
                    ct.UnitAddress,
                    ct.SupplierContact.Count == 0 ? "" : ct.SupplierContact[0].ContactName,
                    ct.SupplierContact.Count == 0 ? "" : ct.SupplierContact[0].ContactMobile,
                    ct.SupplierContact.Count == 0 ? "" : ct.SupplierContact[0].ContactFax,
                    ct.TradeNum);
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
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_新增);
                    break;
                case "modify":
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_修改);
                    break;
                case "toExcel":
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_导入);
                    break;
                case "load":
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_导出);
                    break;
                case "del":
                    result = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_删除);
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        #endregion

    }


}
