using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.StatisticAnalysis.IncomeAccount
{
    public partial class IncomeAreaPrint : BackPage
    {
        IList<EyouSoft.Model.TourStructure.TourOrder> list = null;
        protected decimal sumMoneny;
        protected decimal hasGetMoney;
        protected void Page_Load(object sender, EventArgs e)
        {
            BindList();
        }

        #region 线路名称和出团日期显示
        /// <summary>
        /// 线路名称和出团日期显示
        /// </summary>
        /// <param name="type">0:线路名称显示与否，1：出团日期显示与否</param>
        /// <param name="orderType">下单类型</param>
        /// <param name="routeName">如果是单项服务，线路名称为空</param>
        /// <returns></returns>
        public string GetRouteName(string type, string orderType, string routeName, string tourDate)
        {
            string ReturnValue = string.Empty;
            if (type == "0")
            {//线路名称显示
                if (!string.IsNullOrEmpty(orderType))
                {
                    if (orderType == EyouSoft.Model.EnumType.TourStructure.OrderType.单项服务.ToString())
                        ReturnValue = orderType;//"单项服务"
                    else
                        ReturnValue = routeName;
                }
            }
            else
            {//出团日期
                if (!string.IsNullOrEmpty(orderType))
                {
                    if (orderType == EyouSoft.Model.EnumType.TourStructure.OrderType.单项服务.ToString())
                        ReturnValue = "";
                    else
                        ReturnValue = tourDate;
                }
            }
            return ReturnValue;
        }
        #endregion

        protected void BindList()
        {
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBLL = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.TourStructure.SearchInfo SearchModel = new EyouSoft.Model.TourStructure.SearchInfo();
            SearchModel.BuyCompanyName = Utils.GetQueryStringValue("Company");
            SearchModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(Utils.GetQueryStringValue("TourType"));
            SearchModel.LeaveDateFrom = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourStarDate"));
            SearchModel.LeaveDateTo = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourEndDate"));
            SearchModel.ComputeOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            SearchModel.AreaId = null;
            SearchModel.SalerId = null;
            if (Utils.GetQueryStringValue("SalserId") != "" && Utils.GetQueryStringValue("SalserId") != "0")
            {
                SearchModel.SalerId = int.Parse(Utils.GetQueryStringValue("SalserId"));
            }
            if (Utils.GetQueryStringValue("RouteArea") != "" && Utils.GetQueryStringValue("RouteArea") != "0")
            {
                SearchModel.AreaId = int.Parse(Utils.GetQueryStringValue("RouteArea"));
            }

            int count = 0;  //统计查询数据的条数
            //取得返回的条数_count
            TourOrderBLL.GetOrderList(1, 1, ref count, SearchModel);
            list = TourOrderBLL.GetOrderList(count, 1, ref count, SearchModel);
            TourOrderBLL.GetFinanceSumByOrder(SearchModel, ref sumMoneny, ref hasGetMoney);
            this.repList.DataSource = list;
            this.repList.DataBind();

        }
    }

}
