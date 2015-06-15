using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Model.EnumType;
using Eyousoft.Common.Page;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Web.GroupEnd
{
    /// <summary>
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-10 
    /// </summary>
    public partial class whlogin : System.Web.UI.Page
    {
        #region 分页变量
        protected int pageSize = 20;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        protected int companyId = 0;
        protected string prices = "";
        System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> PriceList = null;
        //登录公司ID
        EyouSoft.SSOComponent.Entity.UserInfo userModel = EyouSoft.Security.Membership.UserProvider.GetUser();

        /// <summary>
        /// 页面初始绑定
        /// </summary>
        /// 修改：田想兵 修改时间：2011.5.24
        /// 修改内容：去掉景点图片
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //杭州:733 宁波:738 嘉兴:735 湖州:734 嘉兴湖州:1145 上海:663 
            int[] citys = { 733, 738, 1145, 663 };
            int cityId = Utils.GetInt(Utils.GetQueryStringValue("cityid"), citys[0]);
            if (!citys.Contains(cityId)) cityId = citys[0];

            if (!IsPostBack)
            {
                //设置公司ID
                //判断 当前域名是否是专线系统为组团配置的域名
                EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
                
                //当前页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                if (domain != null)
                {
                    companyId = domain.CompanyId;

                    //设置景区图片
                    //ImgDataInit(domain.CompanyId);

                    //设置友情链接
                    this.WhBottomControl1.CompanyId = domain.CompanyId;
                    //设置专线介绍
                    this.WhLineControl1.CompanyId = domain.CompanyId;
                    //头部控件
                    this.WhHeadControl1.CompanyId = domain.CompanyId;
                }
                //线路名称
                string areaName = Utils.GetQueryStringValue("AreaName");
                this.txtAreaName.Text = areaName;
                //开始日期
                DateTime? beginDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("BeginDate"));
                if (beginDate != null) { this.txtBeginDate.Text = Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd"); }
                //结束日期
                DateTime? endDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("EndDate"));
                if (endDate != null) { this.txtEndDate.Text = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd"); }
                //线路区域ID
                int? areaId = Utils.GetInt(Utils.GetQueryStringValue("AreaId"));
                AreaInit();

                

                //初始化
                DataInit(areaName, beginDate, endDate, areaId, cityId);

                #region 设置页面title,meta
                //声明基础设置实体对象
                EyouSoft.Model.SiteStructure.SiteBasicConfig configModel = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(domain.CompanyId);
                if (configModel != null)
                {
                    //添加Meta
                    Literal keywork = new Literal();
                    keywork.Text = "<meta name=\"keywords\" content= \"" + configModel.SiteMeta + "\" />";
                    this.Page.Header.Controls.Add(keywork);
                }
                #endregion

                //设置导航
                this.WhHeadControl1.NavIndex = 0;
            }

            //操作类型
            string type = Utils.GetQueryStringValue("type");
            if (type != "" && type == "isLogin")
            {
                //返回是否登录信息
                Response.Clear();
                Response.Write(CheckLogin(userModel).ToString());
                Response.End();
            }

            ClientScript.RegisterClientScriptBlock(GetType(), Guid.NewGuid().ToString(), string.Format("var currentCityId={0};", cityId), true);
        }

        #region 登录账户 取当前登录账户所在组团单位的客户等级的价格信息
        protected string BindPricesList(string Tourid, int CustomerLevel)
        {
            //用来判断登录后是否获得数据
            bool isGetDate = true;
            System.Text.StringBuilder stringPrice = new System.Text.StringBuilder();
            EyouSoft.BLL.TourStructure.Tour tourBLl = new EyouSoft.BLL.TourStructure.Tour();
            PriceList = tourBLl.GetPriceStandards(Tourid);
            stringPrice.Append("<tr bgcolor=\"#dfedfc\">");
            stringPrice.Append("<td height=\"20\" align=\"center\" width=\"40%\" bgcolor=\"#d0eafb\">报价标准</td>");
            stringPrice.Append("<td style=\"text-align:left\" width=\"60%\" bgcolor=\"#d0eafb\">价格</td>");
            for (int i = 0; i < PriceList.Count; i++)
            {
                for (int j = 0; j < PriceList[i].CustomerLevels.Count; j++)
                {
                    if (PriceList[i].CustomerLevels[j].LevelId == CustomerLevel)
                    {
                        stringPrice.Append("<tr>");
                        stringPrice.AppendFormat("<td height=\"20\" width=\"50px\" align=\"center\" bgcolor=\"#f6fafd\">{0}&nbsp;&nbsp;</td>", PriceList[i].StandardName);
                        stringPrice.Append("<td style=\"text-align:left\" bgcolor=\"#f6fafd\">");
                        stringPrice.AppendFormat("成人价：{0}&nbsp;&nbsp;<br/>", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].AdultPrice));
                        stringPrice.AppendFormat("儿童价：{0}", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].ChildrenPrice));
                        stringPrice.Append("</td></tr>");
                        isGetDate = false;
                        break;
                    }
                }

            }

            if (isGetDate)
            {
                for (int i = 0; i < PriceList.Count; i++)
                {
                    for (int j = 0; j < PriceList[i].CustomerLevels.Count; j++)
                    {
                        if (PriceList[i].CustomerLevels[j].LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.门市)
                        {
                            stringPrice.Append("<tr>");
                            stringPrice.AppendFormat("<td height=\"20\" width=\"50px\" align=\"center\" bgcolor=\"#f6fafd\">{0}&nbsp;&nbsp;</td>", PriceList[i].StandardName);
                            stringPrice.Append("<td style=\"text-align:left\" bgcolor=\"#f6fafd\">");
                            stringPrice.AppendFormat("成人价：{0}&nbsp;&nbsp;<br/>", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].AdultPrice));
                            stringPrice.AppendFormat("儿童价：{0}", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].ChildrenPrice));
                            stringPrice.Append("</td></tr>");
                            break;
                        }
                    }
                }
            }
            stringPrice.Append("</tr>");
            prices = stringPrice.ToString();

            return prices;
        }
        #endregion

        #region 未登录用户 取所有报价标准中的门市价的成人价和儿童价
        protected string customlevtypeprice(string tourid)
        {
            if (userModel != null && userModel.ID > 0)
            {
                return BindPricesList(tourid, userModel.TourCompany.CustomerLevel);
            }
            else
            {
                System.Text.StringBuilder stringPrice = new System.Text.StringBuilder();
                EyouSoft.BLL.TourStructure.Tour tourBLl = new EyouSoft.BLL.TourStructure.Tour();
                PriceList = tourBLl.GetPriceStandards(tourid);
                stringPrice.Append("<tr bgcolor=\"#dfedfc\">");
                stringPrice.Append("<td height=\"20\" align=\"center\" width=\"40%\" bgcolor=\"#d0eafb\">报价标准</td>");
                stringPrice.Append("<td style=\"text-align:left\" width=\"60%\" bgcolor=\"#d0eafb\">价格</td>");
                for (int i = 0; i < PriceList.Count; i++)
                {
                    for (int j = 0; j < PriceList[i].CustomerLevels.Count; j++)
                    {
                        if (PriceList[i].CustomerLevels[j].LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.门市)
                        {
                            stringPrice.Append("<tr>");
                            stringPrice.AppendFormat("<td height=\"20\" width=\"50px\" align=\"center\" bgcolor=\"#f6fafd\">{0}&nbsp;&nbsp;</td>", PriceList[i].StandardName);
                            stringPrice.Append("<td style=\"text-align:left\" bgcolor=\"#f6fafd\">");
                            stringPrice.AppendFormat("成人价：{0}&nbsp;&nbsp;<br/>", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].AdultPrice));
                            stringPrice.AppendFormat("儿童价：{0}", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].ChildrenPrice));
                            stringPrice.Append("</td></tr>");
                            break;
                        }
                    }
                }
                stringPrice.Append("</tr>");
                prices = stringPrice.ToString();
                return prices;
            }
        }
        #endregion


        #region 页面初始化
        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="areaName">线路名称</param>
        /// <param name="beginDate">出发时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="areaId">线路区域ID</param>
        /// <param name="cityId">出港城市ID</param>
        protected void DataInit(string areaName, DateTime? beginDate, DateTime? endDate, int? areaId, int? cityId)
        {
            if (areaId == 0)
            {
                areaId = null;
            }
            if (cityId == 0)
            {
                cityId = null;
            }


            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(null, false);
            //声明查询model
            EyouSoft.Model.TourStructure.TourSearchInfo searchModel = new EyouSoft.Model.TourStructure.TourSearchInfo();
            //查询model 属性赋值
            searchModel.AreaId = areaId;
            searchModel.SDate = beginDate;
            searchModel.EDate = endDate;
            searchModel.RouteName = areaName;
            searchModel.TourCityId = cityId;
            IList<EyouSoft.Model.TourStructure.LBZTTours> list = bll.GetToursSiteHot(companyId, pageSize, pageIndex, ref recordCount, searchModel, CompanyStructure.TourDisplayType.明细团);
            if (list != null && list.Count > 0)
            {
                this.rptTourList.DataSource = list;
                this.rptTourList.DataBind();
                BindPage();
                this.lblMsg.Visible = false;
            }
            else
            {
                //如果默认省份的第一个城市无数据，则取消第一城市
                searchModel.TourCityId = null;
                list = bll.GetToursSiteHot(companyId, pageSize, pageIndex, ref recordCount, searchModel, CompanyStructure.TourDisplayType.明细团);
                if (list != null && list.Count > 0)
                {
                    this.rptTourList.DataSource = list;
                    this.rptTourList.DataBind();
                    BindPage();
                    this.lblMsg.Visible = false;
                }
                else
                {
                    this.ExporPageInfoSelect1.Visible = false;
                    this.lblMsg.Text = "未找到相关线路信息!";
                }
            }

        }
        #endregion

        #region 线路区域初始化
        /// <summary>
        /// 线路区域初始化
        /// 创建：田想兵 2011.5.24
        /// 说明：线路区域初始化
        /// </summary>
        protected void AreaInit()
        {
            //获得线路区域集合
            IList<EyouSoft.Model.CompanyStructure.Area> areaList = new EyouSoft.BLL.CompanyStructure.Area(null, false).GetAreaByCompanyId(companyId);
            rpt_area.DataSource = areaList;
            rpt_area.DataBind();
        }
        #endregion

        #region 获得天数差
        /// <summary>
        /// 获得出团日期与当前日期的天数差
        /// </summary>
        /// <param name="beginDate"></param>
        /// <returns></returns>
        protected string GetDays(object beginDate)
        {
            int day = 0;
            DateTime bDate = Utils.GetDateTime(beginDate.ToString(), DateTime.Now);
            DateTime nowDate = DateTime.Now;
            TimeSpan st = bDate.Subtract(nowDate);
            day = st.Days;
            if (day < 0) { day = 0; }
            return day.ToString();
        }
        #endregion

        #region 获得成人、儿童 价格
        /// <summary>
        /// 获得成人 儿童 价格
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        protected string getPeoPlePrice(string tourId)
        {
            string Prices = "";
            //登录
            if (userModel != null && userModel.ID > 0)
            {
                IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> PriceStandard = new EyouSoft.BLL.TourStructure.Tour().GetPriceStandards(tourId);
                if (PriceStandard != null && PriceStandard.Count > 0)
                {
                    if (PriceStandard[0].CustomerLevels != null)
                    {
                        for (int j = 0; j < PriceStandard[0].CustomerLevels.Count; j++)
                        {
                            if (PriceStandard[0].CustomerLevels[j].LevelId == userModel.TourCompany.CustomerLevel)
                            {
                                Prices = Utils.FilterEndOfTheZeroString(PriceStandard[0].CustomerLevels[j].AdultPrice.ToString("0.00")) + "/" + Utils.FilterEndOfTheZeroString(PriceStandard[0].CustomerLevels[j].ChildrenPrice.ToString("0.00"));
                                break;
                            }
                            else
                            {
                                Prices = "";
                            }
                        }
                    }
                }
            }

            //未登录
            if (userModel == null || Prices == "")
            {
                EyouSoft.BLL.TourStructure.Tour bl = new EyouSoft.BLL.TourStructure.Tour();
                IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> list = bl.GetPriceStandards(tourId);
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[0].CustomerLevels != null)
                        {
                            for (int j = 0; j < list[0].CustomerLevels.Count; j++)
                            {
                                if (list[0].CustomerLevels[j].LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.门市)
                                {
                                    Prices = Utils.FilterEndOfTheZeroString(list[0].CustomerLevels[j].AdultPrice.ToString("0.00")) + "/" + Utils.FilterEndOfTheZeroString(list[0].CustomerLevels[j].ChildrenPrice.ToString("0.00"));
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return Prices;
        }
        #endregion

        #region 获得打印单链接
        /// <summary>
        /// 获得散拼计划的打印单
        /// </summary>
        /// <param name="tourid"></param>
        /// <param name="releaseType"></param>
        /// <returns></returns>
        protected string getUrl(string tourid, int releaseType)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            if (releaseType == 0)
            {
                return bll.GetPrintPath(companyId, CompanyStructure.PrintTemplateType.散拼计划标准发布行程单);
            }
            else
            {
                return bll.GetPrintPath(companyId, CompanyStructure.PrintTemplateType.散拼计划快速发布行程单);
            }
        }
        #endregion

        #region 判断用户是否登录
        /// <summary>
        /// 判断是否有用户登录
        /// </summary>
        /// <returns></returns>
        protected bool CheckLogin(EyouSoft.SSOComponent.Entity.UserInfo userModel)
        {
            if (userModel != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 设置分页
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion
    }
}
