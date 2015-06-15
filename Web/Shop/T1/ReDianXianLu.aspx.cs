using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using EyouSoft.Common;

namespace Web.Shop.T1
{
    /// <summary>
    /// 热点线路
    /// </summary>
    /// 汪奇志 2012-04-09
    public partial class ReDianXianLu : System.Web.UI.Page
    {
        #region attributes
        /// <summary>
        /// page size
        /// </summary>
        protected int PageSize = 10;
        /// <summary>
        /// page index 
        /// </summary>
        protected int PageIndex = 1;
        /// <summary>
        /// print path 标准
        /// </summary>
        string PrintPathBZ = string.Empty;
        /// <summary>
        /// print path 快速
        /// </summary>
        string PrintPathKS = string.Empty;
        /// <summary>
        /// 团队展示方式
        /// </summary>
        EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType TourDisplayType = EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            InitPrintPath();
            InitAreas();
            InitCitys();
            InitTours();
        }

        #region private members
        /// <summary>
        /// 初始化打印链接
        /// </summary>
        /// <returns></returns>
        void InitPrintPath()
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            PrintPathBZ = bll.GetPrintPath(Master.CompanyId, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路标准发布行程单);
            PrintPathKS = bll.GetPrintPath(Master.CompanyId, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路快速发布行程单);
            bll = null;
        }

        /// <summary>
        /// init citys
        /// </summary>
        void InitCitys()
        {
            var items = new EyouSoft.BLL.CompanyStructure.City().GetList(Master.CompanyId, null, true);
            txtCitys.Items.Clear();
            txtCitys.Items.Add(new ListItem("-选择城市-", "0"));
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    txtCitys.Items.Add(new ListItem(item.CityName, item.Id.ToString()));
                }
            }
        }

        /// <summary>
        /// init areas
        /// </summary>
        void InitAreas()
        {
            var items = new EyouSoft.BLL.CompanyStructure.Area(null, false).GetAreaByCompanyId(Master.CompanyId);
            txtAreas.Items.Clear();
            txtAreas.Items.Add(new ListItem("-选择线路区域-", "0"));

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    txtAreas.Items.Add(new ListItem(item.AreaName, item.Id.ToString()));
                }
            }
        }

        /// <summary>
        /// init tours
        /// </summary>
        void InitTours()
        {
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            int recordCount = 0;
            TourDisplayType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetSiteTourDisplayType(Master.CompanyId);
            var searchInfo = new EyouSoft.Model.TourStructure.TourSearchInfo();

            searchInfo.RouteName = Utils.GetQueryStringValue("routename");
            searchInfo.SDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("sldate"));
            searchInfo.EDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("eldate"));
            searchInfo.TourCityId = Utils.GetIntNull(Utils.GetQueryStringValue("cityid"));
            searchInfo.AreaId = Utils.GetIntNull(Utils.GetQueryStringValue("areaid"));

            if (searchInfo.AreaId.HasValue && searchInfo.AreaId.Value == 0) searchInfo.AreaId = null;
            if (searchInfo.TourCityId.HasValue && searchInfo.TourCityId.Value == 0) searchInfo.TourCityId = null;
            if (searchInfo.SDate.HasValue || searchInfo.EDate.HasValue) TourDisplayType = EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团;

            var items = new EyouSoft.BLL.TourStructure.Tour().GetToursSiteHot(Master.CompanyId, PageSize, PageIndex, ref recordCount, searchInfo, TourDisplayType);

            if (items != null && items.Count > 0)
            {
                rptTours.DataSource = items;
                rptTours.DataBind();

                divPaging.Visible = true;
                divEmpty.Visible = false;

                paging.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                paging.UrlParams.Add(Request.QueryString);
                paging.intPageSize = PageSize;
                paging.CurrencyPage = PageIndex;
                paging.intRecordCount = recordCount;
            }
            else
            {
                divPaging.Visible = false;
                divEmpty.Visible = true;
            }
        }
        #endregion

        #region protected members
        /// <summary>
        /// rtpTours_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rtpTours_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            decimal chengRenPrice = 0;
            decimal erTongPrice = 0;

            decimal chengRenMSPrice = 0;
            decimal erTongMSPrice = 0;

            string biaoZhunMingCheng = string.Empty;

            bool isFind = false;
            var userInfo = EyouSoft.Security.Membership.UserProvider.GetUser();
            var tourInfo = (EyouSoft.Model.TourStructure.LBZTTours)e.Item.DataItem;
            var items = new EyouSoft.BLL.TourStructure.Tour().GetPriceStandards(tourInfo.TourId);

            if (items != null && items.Count > 0 && items[0] != null && items[0].CustomerLevels != null && items[0].CustomerLevels.Count > 0)
            {
                biaoZhunMingCheng = items[0].StandardName;

                for (int j = 0; j < items[0].CustomerLevels.Count; j++)
                {
                    if (items[0].CustomerLevels[j].LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.门市)
                    {
                        chengRenMSPrice = items[0].CustomerLevels[j].AdultPrice;
                        erTongMSPrice = items[0].CustomerLevels[j].ChildrenPrice;
                    }

                    if (userInfo != null && items[0].CustomerLevels[j].LevelId == userInfo.TourCompany.CustomerLevel)
                    {
                        chengRenPrice = items[0].CustomerLevels[j].AdultPrice;
                        erTongPrice = items[0].CustomerLevels[j].ChildrenPrice;
                        isFind = true;
                    }
                }
            }

            if (!isFind)
            {
                chengRenPrice = chengRenMSPrice;
                erTongPrice = erTongMSPrice;
            }

            StringBuilder fuDongJiaGe = new StringBuilder();

            fuDongJiaGe.Append("<tr bgcolor=\"#f6fafd\">");
            fuDongJiaGe.AppendFormat("<td style=\"text-align:center ;height:24px;\">{0}</td>", biaoZhunMingCheng);
            fuDongJiaGe.AppendFormat("<td style=\"text-align:center;;line-height:24px;\">成人价：{0} 儿童价：{1}</td>", chengRenPrice.ToString("C2"), erTongPrice.ToString("C2"));
            fuDongJiaGe.Append("</tr>");

            string routeNameLink = string.Format("{0}?tourid={1}", tourInfo.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick ? PrintPathKS : PrintPathBZ, tourInfo.TourId);

            string lDate = string.Empty;
            if (TourDisplayType == EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团)
            {
                lDate = string.Format("<span>{0}</span> | <span>{1}</span>", tourInfo.LDate.ToString("yyyy-MM-dd"), tourInfo.RemainPeopleNumber);
            }
            else
            {
                lDate = string.Format("<span title=\"点击可查看全部发班计划\"><a href=\"javascript:void(0)\" data_selector=\"tour_date\" data_tourid=\"{0}\" data_leavedate=\"{1}\">{1}</a></span> | <span>{2}</span>", tourInfo.TourId, tourInfo.LDate.ToString("yyyy-MM-dd"), tourInfo.RemainPeopleNumber);
            }

            string routeName = string.Format("<a target=\"_blank\" title=\"{0}\" href=\"{1}\">{2}<a>", tourInfo.RouteName, routeNameLink, EyouSoft.Common.Utils.GetText2(tourInfo.RouteName, 19, true));

            Literal ltrPrice = (Literal)e.Item.FindControl("ltrPrice");
            Literal ltrPriceFD = (Literal)e.Item.FindControl("ltrPriceFD");
            Literal ltrRouteName = (Literal)e.Item.FindControl("ltrRouteName");
            Literal ltrLDate = (Literal)e.Item.FindControl("ltrLDate");

            ltrPrice.Text = chengRenPrice.ToString("C0") + "/" + erTongPrice.ToString("C0");
            ltrPriceFD.Text = fuDongJiaGe.ToString();
            ltrRouteName.Text = routeName;
            ltrLDate.Text = lDate;

        }
        #endregion
    }
}
