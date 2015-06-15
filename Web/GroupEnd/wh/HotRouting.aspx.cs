using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Model.EnumType;

namespace Web.GroupEnd
{
    /// <summary>
    /// 创建:万 俊
    /// 功能:组团端_登陆页_热点线路
    /// </summary>
    public partial class HotRouting : System.Web.UI.Page
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        protected int companyId = 0;

        protected string prices = "";
        System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> PriceList = null;
        EyouSoft.Model.TourStructure.TourInfo model = null;
        //登录公司ID
        EyouSoft.SSOComponent.Entity.UserInfo userModel = EyouSoft.Security.Membership.UserProvider.GetUser();

        protected void Page_Load(object sender, EventArgs e)
        {
            //设置公司ID
            //判断 当前域名是否是专线系统为组团配置的域名
            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
            if (!IsPostBack)
            {
                companyId = domain.CompanyId;
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
                if (areaId == 0)
                {
                    areaId = null;
                    DdlAreaInit("");
                }
                else
                {
                    DdlAreaInit(areaId.ToString());
                }
                //城市ID
                int? cityId = Utils.GetInt(Utils.GetQueryStringValue("CityID"));
                if (cityId == 0)
                {
                    cityId = null;
                    CityDataInit("");
                }
                else
                {
                    CityDataInit(cityId.ToString());
                }

                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);     //取得页面上的pageindex值.
                //初始化
                DataInit(areaName, beginDate, endDate, areaId, cityId);
                //设置专线介绍用户控件的公司ID
                this.WhLineControl1.CompanyId = companyId;
                //设置导航用户控件的公司ID
                this.WhHeadControl1.CompanyId = companyId;
                //设置友情链接公司ID
                this.WhBottomControl1.CompanyId = companyId;
                //设置导航
                this.WhHeadControl1.NavIndex = 3;
            }

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

            //操作类型
            string type = Utils.GetQueryStringValue("type");
            if (type != "" && type == "isLogin")
            {
                Response.Clear();
                Response.Write(CheckLogin().ToString());
                Response.End();
            }

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
            DdlAreaInit(areaId.ToString());
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(null, false);
            //声明查询model
            EyouSoft.Model.TourStructure.TourSearchInfo searchModel = new EyouSoft.Model.TourStructure.TourSearchInfo();
            //查询model 属性赋值
            searchModel.AreaId = areaId;
            searchModel.SDate = beginDate;
            searchModel.EDate = endDate;
            searchModel.RouteName = areaName;
            if (cityId != 0)
            {
                searchModel.TourCityId = cityId;
            }

            //根据查询条件取得的结果集list
            IList<EyouSoft.Model.TourStructure.LBZTTours> list = bll.GetToursSiteHot(companyId, pageSize, pageIndex, ref recordCount, searchModel, CompanyStructure.TourDisplayType.明细团);
            if (list != null && list.Count > 0)
            {
                //显示分页控件
                this.ExporPageInfoSelect1.Visible = true;
                //将list赋给repeater的数据源
                this.rptTourList.DataSource = list;
                //绑定数据
                this.rptTourList.DataBind();
                //绑定分页数据
                BindPage();
                //隐藏无数据提示控件
                this.lblMsg.Visible = false;
            }
            else
            {
                //隐藏分页控件
                this.ExporPageInfoSelect1.Visible = false;
                this.lblMsg.Text = "未找到相关线路信息!";
                //显示无数据提示控件
                this.lblMsg.Visible = true;
            }

        }
        #endregion

        #region 线路区域初始化
        /// <summary>
        /// 线路区域初始化
        /// </summary>
        /// <param name="selectValue">设置选中项</param>
        protected void DdlAreaInit(string selectValue)
        {
            //清空下拉框值
            this.ddlArea.Items.Clear();
            //添加默认行
            this.ddlArea.Items.Add(new ListItem("-选择线路区域-", "0"));
            //获得线路区域集合
            IList<EyouSoft.Model.CompanyStructure.Area> areaList = new EyouSoft.BLL.CompanyStructure.Area(null, false).GetAreaByCompanyId(companyId);
            if (areaList != null && areaList.Count > 0)
            {
                //将数据添加至下拉框
                for (int i = 0; i < areaList.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = areaList[i].Id.ToString();
                    item.Text = areaList[i].AreaName;
                    this.ddlArea.Items.Add(item);
                }
                //设置选中行
                if (selectValue != "")
                {
                    this.ddlArea.SelectedValue = selectValue;
                }
            }
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
            string path = string.Empty;
            if (releaseType == 0)
            {
                path = bll.GetPrintPath(companyId, CompanyStructure.PrintTemplateType.散拼计划标准发布行程单);
                bll = null;
                return path;
            }
            else
            {
                path = bll.GetPrintPath(companyId, CompanyStructure.PrintTemplateType.散拼计划快速发布行程单);
                bll = null;
                return path;
            }

        }
        #endregion

        #region 判断用户是否登录
        /// <summary>
        /// 判断是否有用户登录
        /// </summary>
        /// <returns></returns>
        protected bool CheckLogin()
        {
            //登录公司ID
            EyouSoft.SSOComponent.Entity.UserInfo userModel = EyouSoft.Security.Membership.UserProvider.GetUser();
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

        #region 获得常用城市
        protected void CityDataInit(string selectIndex)
        {
            EyouSoft.BLL.CompanyStructure.City cityBll = new EyouSoft.BLL.CompanyStructure.City();
            IList<EyouSoft.Model.CompanyStructure.City> list = cityBll.GetList(companyId, null, true);
            if (list != null && list.Count > 0)
            {
                this.ddlOutCity.Items.Clear();
                this.ddlOutCity.Items.Add(new ListItem("-选择出港城市-", "0"));
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = list[i].Id.ToString();
                    item.Text = list[i].CityName;
                    if (selectIndex.Trim() != "" && selectIndex == list[i].Id.ToString())
                    {
                        item.Selected = true;
                    }
                    this.ddlOutCity.Items.Add(item);
                }
            }
        }
        #endregion
    }
}
