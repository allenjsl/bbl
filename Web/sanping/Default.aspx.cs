/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) TravelSky 2011
/// 模块名称：Default.aspx.cs
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\Default.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-12 16:04:21
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// ////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.BLL.TourStructure;
using EyouSoft.Model.TourStructure;
using EyouSoft.Model.EnumType;

using Common.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Web.sanping
{
    /// <summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011.6.10
    /// 修改备注：增加一个业务部、计调部的查询
    /// </summary>

    public partial class Default : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 全局行号
        /// </summary>
        public int i = 0;
        /// <summary>
        /// 当前页
        /// </summary>
        public int pageIndex = 1;
        /// <summary>
        /// 每页行数
        /// </summary>
        protected int pageSize = 20;
        /// <summary>
        /// 城市ID状态.空值:页面显示请选择省份,否则为空
        /// </summary>
        protected string cityState = "";
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

            if (!IsPostBack)
            {
                #region ajax获取省份城市
                if (Utils.GetQueryStringValue("act") == "getprovince")
                {
                    Response.Clear();
                    EyouSoft.BLL.CompanyStructure.Province pBll = new EyouSoft.BLL.CompanyStructure.Province();
                    IList<EyouSoft.Model.CompanyStructure.Province> listProvince = pBll.GetHasFavCityProvince(SiteUserInfo.CompanyID);
                    Response.Write(JsonConvert.SerializeObject(listProvince));
                    Response.End();
                }
                if (Utils.GetQueryStringValue("act") == "getcity")
                {
                    Response.Clear();
                    EyouSoft.BLL.CompanyStructure.City pBll = new EyouSoft.BLL.CompanyStructure.City();
                    IList<EyouSoft.Model.CompanyStructure.City> listCity = pBll.GetList(SiteUserInfo.CompanyID, Utils.GetInt(Utils.GetQueryStringValue("provinceId")), true);
                    Response.Write(JsonConvert.SerializeObject(listCity));
                    Response.End();
                }
                #endregion
                #region ajax修改状态 by txb 2011.8.4

                if (Request.QueryString["act"] == "changeStatus")
                {
                    EyouSoft.BLL.TourStructure.Tour bll = new Tour(SiteUserInfo);
                    int result = bll.SetTourRouteStatus((TourStructure.TourRouteStatus)Utils.GetInt(Utils.GetFormValue("status")), Utils.GetFormValue("tourid").Split(','));
                    if (result > 0)
                    { Response.Write("yes"); }
                    else
                    {
                        Response.Write("no");
                    }
                    Response.End();
                }
                #endregion
                #region 绑定信息、列表
                if (Request.QueryString["act"] == "updatenum")
                {
                    EyouSoft.BLL.TourStructure.Tour bll = new Tour();
                    if (bll.SetTourVirtualPeopleNumber(Request["tourid"], Utils.GetInt(Request["num"])) > 0)
                    {
                        Response.Clear();
                        Response.Write("yes");
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write("no");
                    }
                    Response.End();
                    return;
                }
                if (CheckGrant(TravelPermission.散拼计划_散拼计划_栏目))
                {
                    //Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_栏目, false);
                }
                else
                {
                    Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_栏目, false);
                }
                selectXianlu1.userId = SiteUserInfo.ID;
                selectXianlu1.curCompany = CurrentUserCompanyID;
                selectXianlu1.Url = Request.Url.ToString();
                BindList();
                #endregion
                //城市ID状态
                cityState = Utils.GetQueryStringValue("CityID");
                int cityId = 0;
                int proId = 0;
                if (Utils.GetQueryStringValue("provinceId") == "")
                {
                    //默认为浙江省
                    proId = 67;
                    //取得浙江省下第一个城市
                    EyouSoft.BLL.CompanyStructure.City pBll = new EyouSoft.BLL.CompanyStructure.City();
                    IList<EyouSoft.Model.CompanyStructure.City> listCity = pBll.GetList(SiteUserInfo.CompanyID, proId, true);
                    if (listCity != null && listCity.Count > 0)
                    {
                        cityId = listCity[0].Id;
                    }
                }
                else
                {
                    cityId = Utils.GetInt(Utils.GetQueryStringValue("CityID"));
                    proId = Utils.GetInt(Utils.GetQueryStringValue("provinceId"));
                }
                //城市ID

                CityDataInit(proId, cityId);
                string act = Utils.GetQueryStringValue("act");
                if (act.Length > 0)
                {
                    switch (act)
                    {
                        case "HandStatus":
                            string hs = Utils.GetQueryStringValue("hs");
                            if (hs.Length > 0)
                            {
                                switch (hs)
                                {
                                    case "mk":
                                        HandStatusUp(TourStructure.HandStatus.手动客满);
                                        break;
                                    case "ts":
                                        HandStatusUp(TourStructure.HandStatus.手动停收);
                                        break;
                                    default:
                                        HandStatusUp(TourStructure.HandStatus.无);
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
        }

        #region 获得常用城市
        /// <summary>
        /// 获得常用城市
        /// </summary>
        /// 修改:田想兵 2011.5.24
        /// 修改：弹出选择
        /// <param name="selectIndex"></param>
        protected void CityDataInit(int provinceId, int cityId)
        {
            if (provinceId != 0)
            {
                EyouSoft.BLL.CompanyStructure.Province pBll = new EyouSoft.BLL.CompanyStructure.Province();
                EyouSoft.Model.CompanyStructure.Province pModel = pBll.GetModel(provinceId);
                if (pModel != null)
                {
                    lt_province.Text = pModel.ProvinceName;
                    if (cityId != 0)
                    {
                        EyouSoft.BLL.CompanyStructure.City cBll = new EyouSoft.BLL.CompanyStructure.City();
                        EyouSoft.Model.CompanyStructure.City cModel = cBll.GetModel(cityId);
                        //if (cModel != null)
                        //{
                        //    lt_city.Text = cModel.CityName;
                        //}
                    }
                }

            }
        }
        #endregion

        /// <summary>
        /// 绑定列表
        /// </summary>
        void BindList()
        {
            int Days;
            Tour tour = new Tour(SiteUserInfo);
            #region 查询条件
            TourSearchInfo tsi = new TourSearchInfo();
            //多选计调员id
            string jdid = Utils.GetQueryStringValue("jdIdList");
            if (jdid.Length > 0)
            {
                string[] jdStr = jdid.Split(',');
                int[] jdIdList = new int[jdStr.Length];
                for (int i = 0; i < jdIdList.Length; i++)
                {
                    jdIdList[i] = Utils.GetInt(jdStr[i]);
                }
                tsi.Coordinators = jdIdList;
                selectOperator1.OperId = jdid;
                selectOperator1.OperName = Utils.GetQueryStringValue("jdNameList");
            }
            /////////////////////

            //多选销售员id
            string xsid = Utils.GetQueryStringValue("czIdList");
            if (xsid.Length > 0)
            {
                string[] xsStr = xsid.Split(',');
                int[] xsIdList = new int[xsStr.Length];
                for (int i = 0; i < xsIdList.Length; i++)
                {
                    xsIdList[i] = Utils.GetInt(xsStr[i]);
                }
                tsi.Sellers = xsIdList;
                selectOperator2.OperId = xsid;
                selectOperator2.OperName = Utils.GetQueryStringValue("czNameList");
            }
            /////////////////////
            string cusStatus = Utils.GetQueryStringValue("cusStatus");
            if (cusStatus != "" && cusStatus != "-1")
            {
                tsi.TourStatus = (TourStructure.TourStatus)Utils.GetInt(cusStatus);
                ddl_cusStatus.SelectedItem.Selected = false;
                ddl_cusStatus.Items.FindByValue(cusStatus).Selected = true;
            }
            if (Utils.GetQueryStringValue("days") != "")
            {
                Days = EyouSoft.Common.Utils.GetInt(Utils.GetQueryStringValue("days"));
                tsi.TourDays = Days;
                txt_days.Text = Days.ToString();
            }
            if (Utils.GetQueryStringValue("begin") != "")
            {
                tsi.SDate = Utils.GetDateTime(Request.QueryString["begin"]);
                txt_beginDate.Text = Request.QueryString["begin"];
            }
            if (Utils.GetQueryStringValue("end") != "")
            {
                tsi.EDate = Utils.GetDateTime(Utils.GetQueryStringValue("end"));
                txt_endDate.Text = Utils.GetQueryStringValue("end");
            }

            string orderStatus = Utils.GetQueryStringValue("oderStatus");
            if (orderStatus != "-1" && orderStatus != "")
            {
                tsi.OrderStatus = (TourStructure.OrderState)Utils.GetInt(orderStatus);
                ddl_orderStatus.SelectedItem.Selected = false;
                ddl_orderStatus.Items.FindByValue(orderStatus).Selected = true;
            }

            string RouteName = Utils.GetQueryStringValue("xianlu");
            if (RouteName != "")
            {
                tsi.RouteName = RouteName;
                txt_xianlu.Text = RouteName;
            }
            string TourCode = Utils.GetQueryStringValue("team");
            if (TourCode != "")
            {
                tsi.TourCode = TourCode;
                this.txt_team.Text = TourCode;
            }
            string OrderName = Utils.GetQueryStringValue("orderName");
            if (OrderName != null)
            {
                tsi.YouKeName = OrderName;
                this.txt_Name.Text = OrderName;
            }
            string xlid = Utils.GetQueryStringValue("xlid");
            if (xlid != "")
                tsi.AreaId = Utils.GetInt(xlid);

            tsi.TourCityId = Utils.GetIntNull(Utils.GetQueryStringValue("CityID"));
            int order = Utils.GetInt(Utils.GetQueryStringValue("order"));
            switch (order)
            {
                case 1: { tsi.SortType = EyouSoft.Model.EnumType.TourStructure.TourSortType.出团日期升序; } break;
                case 2: { tsi.SortType = EyouSoft.Model.EnumType.TourStructure.TourSortType.出团日期降序; } break;
                case 3: { tsi.SortType = EyouSoft.Model.EnumType.TourStructure.TourSortType.收客状态升序; } break;
                case 4: { tsi.SortType = EyouSoft.Model.EnumType.TourStructure.TourSortType.收客状态降序; } break;
                default: tsi.SortType = EyouSoft.Model.EnumType.TourStructure.TourSortType.默认; break;
            }
            #endregion
            int count = 0;
            IList<TourBaseInfo> list = tour.GetTours(CurrentUserCompanyID, pageSize, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count, tsi);

            rpt_list.DataSource = list;
            rpt_list.DataBind();
            #region 分页
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion
        }
        /// <summary>
        /// 剩余数计算表达式(计划-实收)
        /// </summary>
        /// <param name="jh">计划</param>
        /// <param name="ss">实收</param>
        /// <param name="lw"></param>
        /// <param name="wql"></param>
        /// <returns></returns>
        public int ShengYu(string jh, string ss, string lw, string wql)
        {
            return EyouSoft.Common.Utils.GetInt(jh) - EyouSoft.Common.Utils.GetInt(ss);// -EyouSoft.Common.Utils.GetInt(lw) - EyouSoft.Common.Utils.GetInt(wql);
        }

        public EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo getPrice(string tourId)
        {
            EyouSoft.BLL.TourStructure.Tour bl = new Tour(SiteUserInfo);
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> list = bl.GetPriceStandards(tourId);
            if (list.Count > 0)
            {
                return list[0].CustomerLevels.FirstOrDefault();
            }
            else
            { return null; }
        }

        #region 根据计划编号取第一条报价的同行价  成人价
        public string GetTHAdultPrice(string Tourid)
        {
            string prices = "";
            EyouSoft.BLL.TourStructure.Tour bl = new Tour(SiteUserInfo);
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> list = bl.GetPriceStandards(Tourid);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list[0].CustomerLevels.Count; i++)
                {
                    if (list[0].CustomerLevels[i].LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.同行)
                    {
                        prices = Utils.FilterEndOfTheZeroString(list[0].CustomerLevels[i].AdultPrice.ToString("0.00"));
                        break;
                    }
                }
            }
            return prices;
        }
        #endregion

        #region 根据计划编号取第一条报价的同行价  儿童价
        public string GetTHChildRenPrice(string tourid)
        {
            string prices = "";
            EyouSoft.BLL.TourStructure.Tour bl = new Tour(SiteUserInfo);
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> list = bl.GetPriceStandards(tourid);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list[0].CustomerLevels.Count; i++)
                {
                    if (list[0].CustomerLevels[i].LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.同行)
                    {
                        prices = Utils.FilterEndOfTheZeroString(list[0].CustomerLevels[i].ChildrenPrice.ToString("0.00"));
                        break;
                    }
                }
            }
            return prices;
        }
        #endregion

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            //计调员列表
            string jdIdList = selectOperator1.OperId;
            string jdNameList = selectOperator1.OperName;
            //销售元列表
            string czIdList = selectOperator2.OperId;
            string czNameList = selectOperator2.OperName;
            Response.Redirect("default.aspx?orderName=" + txt_Name.Text + "&begin=" + txt_beginDate.Text + "&end=" + txt_endDate.Text + "&days=" + txt_days.Text + "&team=" + txt_team.Text + "&xianlu=" + txt_xianlu.Text + "&cusStatus=" + ddl_cusStatus.SelectedValue + "&oderStatus=" + ddl_orderStatus.SelectedValue + "&xlid=" + Utils.GetQueryStringValue("xlid") + "&jdIdList=" + jdIdList + "&czIdList=" + czIdList + "&jdNameList=" + jdNameList + "&czNameList=" + czNameList + "&cityID=" + Utils.GetFormValue("hd_city") + "&ProvinceId=" + Utils.GetFormValue("hd_province"));

        }
        /// <summary>
        /// 价格查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpt_list_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            TourBaseInfo drv = e.Item.DataItem as TourBaseInfo;
            string tourId = drv.TourId;
            EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo p = getPrice(tourId);
            if (p != null)
            {
                (e.Item.FindControl("Lable1") as Label).Text = Utils.FilterEndOfTheZeroDecimal(p.AdultPrice);
                (e.Item.FindControl("Lable2") as Label).Text = Utils.FilterEndOfTheZeroDecimal(p.ChildrenPrice);
            }
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            string[] delId = Utils.GetFormValues("checkbox");

            EyouSoft.BLL.TourStructure.Tour bll = new Tour();
            string errorTourCode = "";
            for (int j = 0; j < delId.Length; j++)
            {
                EyouSoft.Model.TourStructure.TourBaseInfo m = bll.GetTourInfo(delId[j]);
                if (m != null)
                {
                    if (!Utils.PlanIsUpdateOrDelete(m.Status.ToString()))
                    {
                        errorTourCode += m.TourCode + ",";
                        continue;
                    }
                    else
                        bll.Delete(delId[j]);
                }
            }
            if (errorTourCode.Length > 0)
            {
                errorTourCode = errorTourCode.TrimEnd(',');
                Response.Write("<script>alert('团号[" + errorTourCode + "]已提交财务，不能对它操作!');location.href=location.href;</script>");
            }
            else
                Response.Redirect(Request.Url.ToString());
            //BindList();

        }
        /// <summary>
        /// 获取行程单URL
        /// </summary>
        /// <param name="tourid"></param>
        /// <param name="releaseType"></param>
        /// <returns></returns>
        public string getUrl(string tourid, int releaseType)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            if (releaseType == 0)
            {
                return bll.GetPrintPath(CurrentUserCompanyID, CompanyStructure.PrintTemplateType.散拼计划标准发布行程单);
            }
            else
            {
                return bll.GetPrintPath(CurrentUserCompanyID, CompanyStructure.PrintTemplateType.散拼计划快速发布行程单);
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="ticketState"></param>
        /// <param name="tourState"></param>
        /// <returns></returns>
        public string getStatus(EyouSoft.Model.EnumType.PlanStructure.TicketState ticketState, EyouSoft.Model.EnumType.TourStructure.TourStatus tourState)
        {
            if (ticketState == PlanStructure.TicketState.机票申请)
            {
                return "机票申请";
            }
            else
            {
                if (ticketState == PlanStructure.TicketState.审核通过)
                {
                    return "未出票";
                }
                else
                {
                    return tourState.ToString();
                }
            }
        }

        private void HandStatusUp(EyouSoft.Model.EnumType.TourStructure.HandStatus HandStatus)
        {
            Tour bll = new Tour();
            string touids = Utils.GetQueryStringValue("touids");
            string[] touid = touids.Split(',');
            if (bll.SetHandStatus(HandStatus, touid) == 1)
            {
                Response.Write("1");

            }
            else
            {
                Response.Write("设置失败！");
            }
            Response.End();
        }
    }
}
