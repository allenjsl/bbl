using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.GroupEnd.wh
{
    /// <summary>
    /// 模块名称:酒店详细信息
    /// 功能说明:显示酒店的详细信息
    /// </summary>
    /// 创建人:万俊
    /// 创建时间:2011-5-25
    public partial class HotelInfo : System.Web.UI.Page
    {
        /// <summary>
        /// 输出酒店图片的HTML
        /// </summary>
        protected string photos = string.Empty;
        /// <summary>
        /// 公司编号
        /// </summary>
        protected int companyId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //设置公司ID
            //判断 当前域名是否是专线系统为组团配置的域名
            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
            companyId = domain.CompanyId;
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
            //设定导航页被选中项
            this.WhHeadControl1.NavIndex = 4;
            //设置导航用户控件的公司ID 
            this.WhHeadControl1.CompanyId = companyId;
            //设置专线介绍用户控件的公司ID
            this.WhLineControl1.CompanyId = companyId;
            //设置友情链接公司ID
            this.WhBottomControl1.CompanyId = companyId;
            if (!IsPostBack)
            {

                //获取QueryString的值
                string hotelId = Utils.GetQueryStringValue("hotelId");
                if (hotelId == "" || hotelId == null)
                {
                    photos = "暂无图片";
                    return;
                }
                else
                {
                    ViewHotelInfo(hotelId);
                }
            }

        }
        #region 呈现酒店详细信息
        /// <summary>
        /// 呈现酒店的详细信息
        /// </summary>
        /// <param name="hotelId">酒店ID</param>
        private void ViewHotelInfo(string hotelId)
        {
            //酒店实体
            EyouSoft.Model.SupplierStructure.SupplierHotelInfo hotelInfo;
            //酒店BLL
            EyouSoft.BLL.SupplierStructure.SupplierHotel hotelBll = new EyouSoft.BLL.SupplierStructure.SupplierHotel();
            //根据ID获取酒店详细信息
            hotelInfo = hotelBll.GetHotelInfo(Utils.GetInt(hotelId));
            //判断取得的酒店是否为null
            if (hotelInfo != null)
            {
                //酒店名称
                if (hotelInfo.UnitName.Trim() != "")
                {
                    lblHotelName.Text = hotelInfo.UnitName;
                }
                //酒店星级
                if (hotelInfo.Star != 0)
                {
                    lblHotelStar.Text = ((EyouSoft.Model.EnumType.SupplierStructure.HotelStar)hotelInfo.Star).ToString();
                }

                //酒店地址
                if (hotelInfo.UnitAddress.Trim() != "")
                {
                    lblHotelAddress.Text = hotelInfo.UnitAddress;

                }
                //酒店图片
                if (hotelInfo.SupplierPic != null && hotelInfo.SupplierPic.Count > 0)
                {
                    foreach (EyouSoft.Model.SupplierStructure.SupplierPic pic in hotelInfo.SupplierPic)
                    {

                        photos += "<img src='" + pic.PicPath + "'style='margin-bottom:5px' width='86px' height='90px' />&nbsp;&nbsp;";

                    }
                }
                else
                {
                    photos = "暂无图片";
                }
                //酒店简介
                lblHotelInfo.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(hotelInfo.Introduce);
                //房型价格
                if (hotelInfo.RoomTypes != null && hotelInfo.RoomTypes.Count > 0)
                {
                    BindRoom(hotelInfo.RoomTypes);
                }

            }
            else
            {
                photos = "暂无图片";
            }

        }
        #endregion
        #region 房型价格数据绑定
        /// <summary>
        /// 房型价格数据绑定
        /// </summary>
        /// <param name="rooms">room集合</param>
        private void BindRoom(IList<EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo> rooms)
        {
            this.repRoom.DataSource = rooms;
            this.repRoom.DataBind();
        }
        #endregion
    }
}
