using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Model.EnumType;
using System.Text;

namespace Web.GroupEnd
{
    /// <summary>
    /// 模块名称：望海组团景点
    /// 功能说明：望海组团景点列表
    /// 创建人：李晓欢
    /// 创建时间：2011-05-24
    /// </summary>
    public partial class SightRouting : System.Web.UI.Page
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        protected int companyId = 0;

        //登录公司ID
        EyouSoft.SSOComponent.Entity.UserInfo userModel = EyouSoft.Security.Membership.UserProvider.GetUser();
        protected string StrSinglePic = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //设置公司ID
            //判断 当前域名是否是专线系统为组团配置的域名
            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());

            if (!IsPostBack)
            {
                //登录公司编号
                companyId = domain.CompanyId;

                //获取页面上pageindex的值
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                //设置专线介绍用户控件的公司ID
                this.WhLineControl1.CompanyId = companyId;
                //设置导航用户控件的公司ID
                this.WhHeadControl1.CompanyId = companyId;
                //设置友情链接公司ID
                this.WhBottomControl1.CompanyId = companyId;
                //设置导航
                this.WhHeadControl1.NavIndex = 6;

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

                //绑定省份下拉框 
                BindPro();
                //绑定景点列表
                BindSpotList();
            }
        }

        /// <summary>
        /// 组团景点列表
        /// </summary>
        protected void BindSpotList()
        {
            //景点业务逻辑层
            EyouSoft.Model.SupplierStructure.SupplierQuery sightSearchInfo = new EyouSoft.Model.SupplierStructure.SupplierQuery();
            //省份
            sightSearchInfo.ProvinceId = Utils.GetInt(Utils.GetQueryStringValue("ProvinceId"));
            //城市
            sightSearchInfo.CityId = Utils.GetInt(Utils.GetQueryStringValue("Cityid"));
            //景点名称
            sightSearchInfo.UnitName = Utils.GetQueryStringValue("SoptName");

            IList<EyouSoft.Model.SupplierStructure.SupplierSpot> spotlist = new EyouSoft.BLL.SupplierStructure.SupplierSpot().GetList(pageSize, pageIndex, ref recordCount, companyId, sightSearchInfo);
            if (spotlist != null && spotlist.Count > 0)
            {
                //显示分页控件
                this.ExporPageInfoSelect1.Visible = true;
                //绑定数据源
                this.rptTourList.DataSource = spotlist;
                this.rptTourList.DataBind();
                BindPage();
                //隐藏无数据提示控件
                this.lblMsg.Visible = false;
            }
            else
            {
                //隐藏分页控件
                this.ExporPageInfoSelect1.Visible = false;
                this.lblMsg.Text = "未找到相关景点信息!";
                //显示无数据提示控件
                this.lblMsg.Visible = true;
            }
            //初始化查询条件
            SearchBind(sightSearchInfo, companyId);
        }

        #region 赋值给查询控件
        protected void SearchBind(EyouSoft.Model.SupplierStructure.SupplierQuery SearchModel, int Companyid)
        {
            if (SearchModel != null)
            {
                //省份下拉框
                this.ddlPro.SelectedValue = Convert.ToString(SearchModel.ProvinceId);
                EyouSoft.BLL.CompanyStructure.City cityBll = new EyouSoft.BLL.CompanyStructure.City();
                if (ddlPro.SelectedValue != "0")
                {
                    IList<EyouSoft.Model.CompanyStructure.City> list = cityBll.GetList(companyId, (int?)Convert.ToInt32(SearchModel.ProvinceId), null);
                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            ListItem item = new ListItem();
                            item.Text = list[i].CityName;
                            item.Value = Convert.ToString(list[i].Id);
                            this.ddlCity.Items.Add(item);
                        }
                    }
                }

                //城市下拉框
                this.ddlCity.SelectedValue = Convert.ToString(SearchModel.CityId);
                //景点名称
                this.SpotName.Text = SearchModel.UnitName;
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

        #region 星级过滤掉"_"字符
        /// <summary>
        /// 星级过滤掉"_"字符
        /// </summary>
        /// <param name="star"></param>
        /// <returns></returns>
        protected string DisponseStar(int star)
        {
            string GetStar = "";
            switch (star)
            {
                case 1:
                    GetStar = "1星";
                    break;
                case 2:
                    GetStar = "2星";
                    break;
                case 3:
                    GetStar = "3星";
                    break;
                case 4:
                    GetStar = "4星";
                    break;
                case 5:
                    GetStar = "5星";
                    break;
                default:
                    GetStar = "无星级";
                    break;
            }
            return GetStar;
        }
        #endregion

        #region 获取景点图片
        /// <summary>
        /// 获取景点图片
        /// </summary>
        /// <param name="id">供应商编号</param>
        /// <returns>景点第一张图片</returns>
        protected string GetSinglePic(int id)
        {
            EyouSoft.Model.SupplierStructure.SupplierSpot spotmodel = new EyouSoft.BLL.SupplierStructure.SupplierSpot().GetModel(id);
            IList<EyouSoft.Model.SupplierStructure.SupplierPic> pic = spotmodel.SupplierPic;
            if (pic != null && pic.Count > 0)
            {
                for (int i = 0; i < pic.Count; i++)
                {
                    if (pic[0].PicPath == "")
                    {
                        StrSinglePic = "暂无图片";
                    }
                    else
                    {
                        StrSinglePic = "<img src=\"" + pic[0].PicPath + "\" alt=\"\" width=\"60px\" height=\"50px\" border=\"0\" />";
                    }
                }

            }
            else
            {
                StrSinglePic = "暂无图片";
            }
            return StrSinglePic;
        }
        #endregion

        #region 绑定省市
        //绑定省
        protected void BindPro()
        {
            //清空省份的下拉框
            this.ddlPro.Items.Clear();
            //添加默认选择项"-请选择-"
            this.ddlPro.Items.Add(new ListItem("-请选择-", "0"));
            //省份BLL
            EyouSoft.BLL.CompanyStructure.Province proBll = new EyouSoft.BLL.CompanyStructure.Province();
            //根据公司ID取得相应结果集list
            IList<EyouSoft.Model.CompanyStructure.Province> list = proBll.GetList(companyId);
            if (list != null && list.Count > 0)
            {
                //循环将值添加到下拉框
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = list[i].ProvinceName;
                    item.Value = Convert.ToString(list[i].Id);
                    this.ddlPro.Items.Add(item);
                }
            }
        }
        //绑定城市-前台的JS方法 省份选择的change方法调用
        [System.Web.Services.WebMethod()]
        public static string BindCity(string proId, string companyId)
        {
            StringBuilder strBul = new StringBuilder();
            if (proId != "0")
            {
                //城市BLL
                EyouSoft.BLL.CompanyStructure.City cityBll = new EyouSoft.BLL.CompanyStructure.City();
                //根据选择的省份ID，公司ID取得的结果集list 
                IList<EyouSoft.Model.CompanyStructure.City> list = cityBll.GetList(Convert.ToInt32(companyId), (int?)Convert.ToInt32(proId), null);
                if (list != null && list.Count > 0)
                {
                    //遍历list将值添加到strBul
                    foreach (EyouSoft.Model.CompanyStructure.City city in list)
                    {
                        //最终要返回的值,下拉框的HTML
                        strBul.Append("<option value=\"");
                        strBul.Append(city.Id);
                        strBul.Append("\">");
                        strBul.Append(city.CityName);
                        strBul.Append("</option>");
                    }
                }
            }


            return strBul.ToString();
        }
        #endregion
    }
}
