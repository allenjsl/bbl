using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;
using EyouSoft.Model.EnumType;

namespace Web.Shop.T1
{
    /// <summary>
    /// t1 default page
    /// </summary>
    /// 汪奇志 2012-04-01
    public partial class Default : System.Web.UI.Page
    {
        #region attributes
        /// <summary>
        /// page size
        /// </summary>
        protected int  PageSize = 10;
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
        EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType TourDisplayType = CompanyStructure.TourDisplayType.明细团;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region ajax request
            string doType = Utils.GetQueryStringValue("doType");
            switch (doType)
            {
                case "isLogin": IsLogin(); break;
                case "existsUsername": ExistsUsername(); break;
                case "getCity": GetCity(); break;
                case "getTours": GetTours(); break;
            }
            #endregion

            InitPrintPath();
            InitCitys();
            InitAreas();
            InitTours();
        }

        #region ajax request members
        /// <summary>
        /// AJAX请求判断组团端用户是否登录，输出0已登录，输入-1未登录。
        /// </summary>
        void IsLogin()
        {
            string s = "0";
            EyouSoft.SSOComponent.Entity.UserInfo info = EyouSoft.Security.Membership.UserProvider.GetUser();
            if (info == null || info.UserType != EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户) s = "-1";

            ResponseAndClear(s);
        }

        /// <summary>
        /// AJAX请求用户名验证
        /// </summary>
        void ExistsUsername()
        {
            bool result = new EyouSoft.BLL.CompanyStructure.CompanyUser().IsExists(0, Utils.GetFormValue("username"), Master.CompanyId);

            string s = "0";
            if (result)
            {
                s = "-1";
            }

            ResponseAndClear(s);
        }

        /// <summary>
        /// AJAX请求获取省份下城市信息集合(select options )
        /// </summary>
        void GetCity()
        {
            int provinceId = Utils.GetInt(Utils.GetFormValue("provinceid"));

            StringBuilder s = new StringBuilder();

            var items = new EyouSoft.BLL.CompanyStructure.City().GetList(Master.CompanyId, provinceId, null);
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    s.AppendFormat("<option value=\"{0}\">{1}</option>", item.Id, item.CityName);
                }
            }

            ResponseAndClear(s.ToString());
        }

        /// <summary>
        /// AJAX请求获取指定团队编号所在母团下相关子团信息
        /// </summary>
        void GetTours()
        {
            string tourId = Utils.GetFormValue("tourid");
            DateTime sLDate = new DateTime(Utils.GetInt(Utils.GetFormValue("year"), DateTime.Now.Year), Utils.GetInt(Utils.GetFormValue("month"), DateTime.Now.Month), 1);
            DateTime eLDate = sLDate.AddMonths(1);

            EyouSoft.SSOComponent.Entity.UserInfo userInfo = EyouSoft.Security.Membership.UserProvider.GetUser();
            int levelId = 0;
            if (userInfo != null && userInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户)
            {
                levelId = userInfo.TourCompany.CustomerLevel;
            }

            var items = new EyouSoft.BLL.TourStructure.Tour().GetToursRiLi(Master.CompanyId, tourId, sLDate, eLDate);
            string s = "[]";
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    decimal marketPrice = 0;
                    decimal levelPrice = 0;
                    new EyouSoft.BLL.TourStructure.Tour().GetTourPrice(item.TourId, levelId,out marketPrice,out levelPrice);

                    item.JiaGeCR = levelPrice;
                }

                s = Newtonsoft.Json.JsonConvert.SerializeObject(items);
            }

            ResponseAndClear(s);
        }

        /// <summary>
        /// response clear,response write s,response end.
        /// </summary>
        /// <param name="s"></param>
        void ResponseAndClear(string s)
        {
            Response.Clear();
            Response.Write(s);
            Response.End();
        }
        #endregion

        #region private members
        /// <summary>
        /// init citys
        /// </summary>
        void InitCitys()
        {
            var items = new EyouSoft.BLL.CompanyStructure.City().GetList(Master.CompanyId, null, true);
            
            if (items != null && items.Count > 0)
            {
                rptCitys.DataSource = items;
                rptCitys.DataBind();
            }
        }

        /// <summary>
        /// init areas
        /// </summary>
        void InitAreas()
        {
            var items = new EyouSoft.BLL.CompanyStructure.Area(null, false).GetAreaByCompanyId(Master.CompanyId);

            if (items != null && items.Count > 0)
            {
                rptAreas.DataSource = items;
                rptAreas.DataBind();
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
            searchInfo.SDate = Utils.GetDateTimeNullable("sldate");
            searchInfo.EDate = Utils.GetDateTimeNullable("eldate");
            searchInfo.TourCityId = Utils.GetIntNull(Utils.GetQueryStringValue("cityid"));
            searchInfo.AreaId = Utils.GetIntNull(Utils.GetQueryStringValue("areaid"));

            if (searchInfo.AreaId.HasValue && searchInfo.AreaId.Value == 0) searchInfo.AreaId = null;
            if (searchInfo.TourCityId.HasValue && searchInfo.TourCityId.Value == 0) searchInfo.TourCityId = null;

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

        /// <summary>
        /// 初始化打印链接
        /// </summary>
        /// <returns></returns>
        void InitPrintPath()
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            PrintPathBZ = bll.GetPrintPath(Master.CompanyId, CompanyStructure.PrintTemplateType.组团线路标准发布行程单);
            PrintPathKS = bll.GetPrintPath(Master.CompanyId, CompanyStructure.PrintTemplateType.组团线路快速发布行程单);
            bll = null;
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

            string biaoZhunMingCheng=string.Empty;

            bool isFind = false;
            var userInfo = EyouSoft.Security.Membership.UserProvider.GetUser();
            var tourInfo = (EyouSoft.Model.TourStructure.LBZTTours)e.Item.DataItem;
            var items = new EyouSoft.BLL.TourStructure.Tour().GetPriceStandards(tourInfo.TourId);

            if (items != null && items.Count > 0 && items[0] != null && items[0].CustomerLevels != null && items[0].CustomerLevels.Count > 0)
            {
                biaoZhunMingCheng = items[0].StandardName;

                for (int j = 0; j < items[0].CustomerLevels.Count; j++)
                {
                    if (items[0].CustomerLevels[j].LevelType == CompanyStructure.CustomLevType.门市)
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

            string routeNameLink=string.Format("{0}?tourid={1}",tourInfo.ReleaseType== TourStructure.ReleaseType.Quick?PrintPathKS:PrintPathBZ,tourInfo.TourId);

            string lDate = string.Empty;
            if (TourDisplayType == CompanyStructure.TourDisplayType.明细团)
            {
                lDate = string.Format("<span>{0}</span> | <span>{1}</span>", tourInfo.LDate.ToString("yyyy-MM-dd"), tourInfo.RemainPeopleNumber);
                //lDate = string.Format("<span><a href=\"javascript:void(0)\" data_selector=\"tour_date\" data_tourid=\"{0}\" data_leavedate=\"{1}\">{1}</a></span> | <span>{2}</span>", tourInfo.TourId, tourInfo.LDate.ToString("yyyy-MM-dd"), tourInfo.RemainPeopleNumber);
            }
            else
            {
                lDate = string.Format("<span title=\"点击可查看全部发班计划\"><a href=\"javascript:void(0)\" data_selector=\"tour_date\" data_tourid=\"{0}\" data_leavedate=\"{1}\">{1}</a></span> | <span>{2}</span>", tourInfo.TourId, tourInfo.LDate.ToString("yyyy-MM-dd"), tourInfo.RemainPeopleNumber);
            }

            Literal ltrPrice = (Literal)e.Item.FindControl("ltrPrice");
            Literal ltrPriceFD = (Literal)e.Item.FindControl("ltrPriceFD");
            Literal ltrRouteName = (Literal)e.Item.FindControl("ltrRouteName");
            Literal ltrLDate = (Literal)e.Item.FindControl("ltrLDate");

            ltrPrice.Text = chengRenPrice.ToString("C2") + "/" + erTongPrice.ToString("C2");
            ltrPriceFD.Text = fuDongJiaGe.ToString();
            ltrRouteName.Text = string.Format("<a target=\"_blank\" title=\"{0}\" href=\"{1}\">{2}<a>", tourInfo.RouteName, routeNameLink, EyouSoft.Common.Utils.GetText2(tourInfo.RouteName, 19, true));
            ltrLDate.Text = lDate;

        }
        #endregion
    }
}
