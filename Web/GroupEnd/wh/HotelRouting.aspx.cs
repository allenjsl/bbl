using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.GroupEnd
{
    /// <summary>
    /// 创建:万 俊
    /// 功能:组团端_登陆页_酒店
    /// </summary>
    public partial class HotelRouting : System.Web.UI.Page
    {
        #region 设置分页变量
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 10;
        #endregion
        protected int companyId = 0;  //公司编号
        protected void Page_Load(object sender, EventArgs e)
        {
            //设置公司ID
            //判断 当前域名是否是专线系统为组团配置的域名
            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
            companyId = domain.CompanyId;
            if (!IsPostBack)
            {
                //星级下拉框设置为“-请选择-”
                BindStatrs(0);
                //设定导航页被选中项
                this.WhHeadControl1.NavIndex = 4;
                //绑定省份下拉框 
                BindPro();
                //取得页面上的pageindex值.
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
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
            //设置导航用户控件的公司ID 
            this.WhHeadControl1.CompanyId = companyId;
            //设置专线介绍用户控件的公司ID
            this.WhLineControl1.CompanyId = companyId;
            //设置友情链接公司ID
            this.WhBottomControl1.CompanyId = companyId;
            //绑定页面数据
            InitHotelList();
        }
        #region 绑定页面数据
        protected void InitHotelList()
        {
            EyouSoft.BLL.SupplierStructure.SupplierHotel hotelBll = new EyouSoft.BLL.SupplierStructure.SupplierHotel();
            //搜索条件
            EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo searchInfo = new EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo();
            //省份ID
            searchInfo.ProvinceId = Utils.GetInt(Utils.GetQueryStringValue("ProId"));
            if (searchInfo.ProvinceId == 0)
            {
                searchInfo.ProvinceId = null;
            }
            //城市ID
            searchInfo.CityId = Utils.GetInt(Utils.GetQueryStringValue("CityId"));
            if (searchInfo.CityId == 0)
            {
                searchInfo.CityId = null;
            }
            //酒店名称
            searchInfo.Name = Server.UrlDecode(Utils.GetQueryStringValue("HotelName"));
            //星级
            searchInfo.Star = (EyouSoft.Model.EnumType.SupplierStructure.HotelStar?)Utils.GetInt(Utils.GetQueryStringValue("Star"));
            if (searchInfo.Star == 0)
            {
                searchInfo.Star = null;
            }
            //取得请求结果list
            IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> list = hotelBll.GetSiteHotels(companyId, pageSize, pageIndex, ref recordCount, searchInfo);
            //list赋给repeater的数据源
            rptTourList.DataSource = list;
            if (list != null && list.Count > 0)
            {
                //显示分页控件
                this.ExporPageInfoSelect1.Visible = true;
                //repeater绑定数据
                rptTourList.DataBind();
                //分页绑定
                BindExportPage();
                //无数据提示控件隐藏
                this.lblMsg.Visible = false;
            }
            else
            {
                //隐藏分页控件
                this.ExporPageInfoSelect1.Visible = false;
                this.lblMsg.Text = "未查找到相关的酒店信息";
                //显示无数据提示控件
                this.lblMsg.Visible = true;
            }
            //为查询控件赋值
            BindSearch(searchInfo, companyId);
        }
        #endregion
        #region 绑定星级选择控件
        protected void BindStatrs(int selectIndex)
        {
            //清空星级下拉框的内容
            this.sel_star.Items.Clear();
            //添加-请选择-默认项
            this.sel_star.Items.Add(new ListItem("-请选择-", "0"));
            //取得结果集list
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.SupplierStructure.HotelStar));
            //判断集合是否不为null且有数据
            if (list != null && list.Count > 0)
            {
                //循环赋值给星级下拉框
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = list[i].Value;
                    //处理"_准3以下"时的时候去掉"_"
                    if (list[i].Text.Length > 4)
                    {
                        item.Text = list[i].Text.Substring(1, list[i].Text.Length - 1);
                    }
                    else
                    {
                        item.Text = list[i].Text;
                    }
                    //将item添加到星级菜单中
                    this.sel_star.Items.Add(item);
                }
            }
            //如果页面上的选择项不是"-请选择-",就将页面上选中星级的值指定为下拉框的当前选择项
            if (selectIndex != 0)
            {
                this.sel_star.SelectedValue = Convert.ToString(selectIndex);
            }
        }
        #endregion
        #region 绑定分页控件
        protected void BindExportPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams.Add(Request.QueryString);
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
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
            //循环将值添加到下拉框
            if (list != null && list.Count > 0)
            {
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
        #region 赋值给查询控件
        protected void BindSearch(EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo searchInfo, int companyId)
        {
            if (searchInfo != null)
            {
                //省份下拉框
                this.ddlPro.SelectedValue = Convert.ToString(searchInfo.ProvinceId);
                EyouSoft.BLL.CompanyStructure.City cityBll = new EyouSoft.BLL.CompanyStructure.City();
                if (ddlPro.SelectedValue != "0")
                {
                    IList<EyouSoft.Model.CompanyStructure.City> list = cityBll.GetList(companyId, (int?)Convert.ToInt32(searchInfo.ProvinceId), null);
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
                this.ddlCity.SelectedValue = Convert.ToString(searchInfo.CityId);
                //星级下拉框
                if (searchInfo.Star == null)
                {
                    BindStatrs(0);
                }
                else
                {
                    BindStatrs((int)searchInfo.Star);
                }
                //酒店名称
                this.hotelName.Value = searchInfo.Name;
            }
        }
        #endregion
        #region 绑定图片
        protected string ReturnImgPath(IList<EyouSoft.Model.SupplierStructure.SupplierPic> picList)
        {
            string path = "";
            //判断图片字段是否为null
            if (picList != null && picList.Count>0)
            {

                    path = "<img style='width: 80px; height: 50px' src=" + picList[0].PicPath + " />";
            }
            else
            {
                path = "<label>暂无图片</label>";
            }

            return path;

        }
        #endregion
        #region 绑定房型
        protected string ReturnRoomTypeName(IList<EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo> roomType)
        {
            string roomName = "-";
            //判断房型集合是否为null
            if (roomType != null)
            {
                //判断房型集合的数量是否大于0
                if (roomType.Count > 0)
                {
                    roomName = roomType[0].Name;
                }
            }
            return roomName;
        }
        #endregion
        #region 绑定结算价
        protected string ReturnPrice(IList<EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo> roomType)
        {
            string price = "";
            //判断房型的集合是否为null
            if (roomType != null)
            {
                //判断房型的集合是否大于0
                if (roomType.Count > 0)
                {
                    price = roomType[0].SellingPrice.ToString("f2");
                }
                //处理返回数据
                if (price != "0.00")
                {
                    price = "￥" + price;
                }
                else
                {
                    price = "-暂无-";
                }
            }
            return price;
        }
        #endregion
        #region 页面处理星级
        protected string ReturnStar(string star)
        {
            string reStar = "";
            //处理"_准3以下"时的时候去掉"_"
            if (star.Length > 4)
            {
                reStar = star.Substring(1, star.Length - 1);
            }
            //如果是“0”，返回"无星级"
            else if (star == "0")
            {
                reStar = "无星级";
            }
            //不符合上诉条件返回相应的值
            else
            {
                reStar = star;
            }
            return reStar;
        }
        #endregion
        #region 截断字符
        protected string ReturnIntroduce(string introduce)
        {
            string refIntroduce = "";
            //如果当前值长度大于15则做截断处理
            if (introduce.Length > 15)
            {
                refIntroduce = introduce.Substring(0, 15) + "...";
            }
            //如果当前值为空则赋值“-暂无介绍-”
            else if (introduce == "")
            {
                refIntroduce = "-暂无介绍-";
            }
            //不符合上诉情况则直接赋值
            else
            {
                refIntroduce = introduce;
            }
            return refIntroduce;
        }
        #endregion
    }


}
