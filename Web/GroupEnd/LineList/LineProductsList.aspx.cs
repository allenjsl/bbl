using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using EyouSoft.Common.Function;
using System.Data;
using EyouSoft.Common;

namespace Web.GroupEnd
{
    /// <summary>
    /// 模块名称:组团端线路计划列表(包括团队查询,地接社信息，报名)
    /// 创建时间:2011-01-17
    /// 创建人:lixh
    /// </summary>
    public partial class LineProductsList : Eyousoft.Common.Page.FrontPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        protected int CompanyID = 1;//当前登录的公司编号
        /// <summary>
        /// 城市ID状态.空值:页面显示请选择省份,否则为空
        /// </summary>
        protected string cityState = "";
        /// <summary>
        /// 团队展示方式
        /// </summary>
        EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType TourDisplayType = EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团;
        #endregion      

        protected void Page_Load(object sender, EventArgs e)
        {

            InitTourDisplayType();

            if (!this.Page.IsPostBack)
            {
                InitBindLineType();

                //分页
                PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

                #region 定义变量接受参数
                //线路区域编号
                int? AreaID = Utils.GetIntNull(Utils.GetQueryStringValue("AreaID"));                
                //线路名称
                string AreaName = Utils.GetQueryStringValue("AreaName");
                //开始发布时间
                DateTime? StartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("StarTime"));
                //结束发布时间
                DateTime? EndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("EndTime"));
                #endregion

                #region 初始化变量参数
                if (AreaID > 0)
                {
                    if (this.DDl_LineType.Items.FindByValue(AreaID.ToString()) != null)
                        this.DDl_LineType.Items.FindByValue(AreaID.ToString()).Selected = true;
                }
                if (AreaName != "")
                {
                    this.xl_XianlName.Value = AreaName.ToString();
                }
                if (StartTime != null)
                {
                    this.BeginTime.Value = Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd");                  
                }
                if (EndTime != null)
                {
                    this.EndTime.Value = Convert.ToDateTime(EndTime).ToString("yyyy-MM-dd");                    
                }
                #endregion
                BindInitLineProducts(AreaID, AreaName, StartTime, EndTime);
                #region 初始化省份城市

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
                #endregion 
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
        #region 绑定线路区域
        protected void InitBindLineType()
        {
            //清空下拉框选项
            this.DDl_LineType.Items.Clear();
            this.DDl_LineType.Items.Add(new ListItem("--请选择线路区域--", ""));
            IList<EyouSoft.Model.CompanyStructure.Area> areaList = new EyouSoft.BLL.CompanyStructure.Area().GetAreaList(SiteUserInfo.ID);
            if (areaList != null && areaList.Count > 0)
            {
                //将数据添加至下拉框
                for (int i = 0; i < areaList.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = areaList[i].Id.ToString();
                    item.Text = areaList[i].AreaName;
                    this.DDl_LineType.Items.Add(item);
                }
            }
            //释放资源
            areaList = null;
        }
        #endregion

        #region 线路列表信息
        /// <summary>
        /// 绑定线路列表
        /// </summary>
        protected void BindInitLineProducts(int? LineTypeID, string LineName, DateTime? StarTime, DateTime? EndTime)
        {
            EyouSoft.Model.TourStructure.TourSearchInfo ModelTourSearch = new EyouSoft.Model.TourStructure.TourSearchInfo();
            //出团结束时间
            ModelTourSearch.EDate = EndTime;
            //出团开始时间
            ModelTourSearch.SDate = StarTime;
            //线路名称
            ModelTourSearch.RouteName = LineName;
            //线路区域编号
            ModelTourSearch.AreaId = LineTypeID;

            ModelTourSearch.Areas = SiteUserInfo.Areas;

            //线路筛选
            if (Request.QueryString["xlid"] != "" && Request.QueryString["xlid"] != null)
            {
                ModelTourSearch.AreaId = Utils.GetInt(Utils.GetQueryStringValue("xlid"));
            }

            //获取首页团号
            if (!string.IsNullOrEmpty(Utils.GetQueryStringValue("tourid")))
            {               
                ModelTourSearch.TourId = Utils.GetQueryStringValue("tourid");
            }
            ModelTourSearch.TourCityId = Utils.GetIntNull(Utils.GetQueryStringValue("cityID"));

            if (ModelTourSearch.SDate.HasValue || ModelTourSearch.EDate.HasValue) TourDisplayType = EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团;

            //组团端计划列表
            IList<EyouSoft.Model.TourStructure.LBZTTours> LBZTToursList = new List<EyouSoft.Model.TourStructure.LBZTTours>();
            //计划中心业务类
            EyouSoft.BLL.TourStructure.Tour BllTour = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo, false);
            //获取组团端计划列表
            LBZTToursList = BllTour.GetToursZTD(SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, ModelTourSearch, TourDisplayType);
            if (LBZTToursList.Count > 0)
            {
                this.LineProductLists.DataSource = LBZTToursList;
                this.LineProductLists.DataBind();
                BinPage();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
            }

            ModelTourSearch = null;
            LBZTToursList = null;
            BllTour = null;
        }
        #endregion

        #region 设置分页
        /// <summary>
        /// 设置分页
        /// </summary>
        protected void BinPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
        #endregion       

        #region 绑定成人价和儿童价
        /// <summary>
        /// 获取成人价
        /// </summary>
        /// <param name="tourId">团号</param>
        /// <returns></returns>
        protected string GetAdultPrice(string tourId)
        {
            decimal Adult = 0;
            decimal Children = 0;
            EyouSoft.BLL.TourStructure.Tour tourbll = new EyouSoft.BLL.TourStructure.Tour();
            tourbll.GetTourPrice(tourId, SiteUserInfo.TourCompany.CustomerLevel, out Adult, out Children);
            return EyouSoft.Common.Utils.FilterEndOfTheZeroString(Adult.ToString());
        }
        /// <summary>
        /// 获取儿童价
        /// </summary>
        /// <param name="tourId">团号</param>
        /// <returns></returns>
        protected string GetChildrenPrice(string tourId)
        {
            decimal Adult = 0;
            decimal Children = 0;
            EyouSoft.BLL.TourStructure.Tour tourbll = new EyouSoft.BLL.TourStructure.Tour();
            tourbll.GetTourPrice(tourId, SiteUserInfo.TourCompany.CustomerLevel, out Adult, out Children);
            return EyouSoft.Common.Utils.FilterEndOfTheZeroString(Children.ToString());
        }
        #endregion

        #region 处理页面上的剩余人数显示方式
        protected string DisponseLastPeople(int count)
        { 
            string theCount="";
            if(count>9)
            {
                theCount=">9";
            }else
	        {
                theCount = Convert.ToString(count);
	        }

            return theCount;
        }

        #endregion
        #region 打印行程单
        protected string ReturnUrl(string enumobj)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting CompanyBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            if (enumobj == "Normal")
            {
                return CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路标准发布行程单);
            }
            else
            {
                return CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路快速发布行程单);
            }
        }
        #endregion

        #region private members
        /// <summary>
        /// 初始化团队展示方式 默认按系统配置 当request params isdetails==1时，始终按明细团展示
        /// </summary>
        void InitTourDisplayType()
        {
            TourDisplayType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetSiteTourDisplayType(SiteUserInfo.CompanyID);
            if (Utils.GetQueryStringValue("isdetails") == "1") TourDisplayType = EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团;
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
            
            var tourInfo = (EyouSoft.Model.TourStructure.LBZTTours)e.Item.DataItem;
            string lDate = string.Empty;
            if (TourDisplayType == EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团)
            {
                lDate = string.Format("<span>{0}</span>", tourInfo.LDate.ToString("yyyy-MM-dd"));
                //lDate = string.Format("<span><a href=\"javascript:void(0)\" data_selector=\"tour_date\" data_tourid=\"{0}\" data_leavedate=\"{1}\">{1}</a></span>", tourInfo.TourId, tourInfo.LDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                lDate = string.Format("<span title=\"点击可查看全部发班计划\"><a href=\"javascript:void(0)\" data_selector=\"tour_date\" data_tourid=\"{0}\" data_leavedate=\"{1}\">{1}</a></span>", tourInfo.TourId, tourInfo.LDate.ToString("yyyy-MM-dd"));
            }

            Literal ltrLDate = (Literal)e.Item.FindControl("ltrLDate");

            ltrLDate.Text = lDate;

        }
        #endregion
    }

}
