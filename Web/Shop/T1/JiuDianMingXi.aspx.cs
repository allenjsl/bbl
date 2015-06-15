using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Shop.T1
{
    /// <summary>
    /// 酒店明细
    /// </summary>
    /// 汪奇志 2012-04-03
    public partial class JiuDianMingXi : System.Web.UI.Page
    {
        #region attributes
        /// <summary>
        /// 酒店图片信息
        /// </summary>
        protected string photos = "暂无图片";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            InitMingXi();
        }

        #region private members
        /// <summary>
        /// init jiudian mingxi
        /// </summary>
        void InitMingXi()
        {
            int jiuDianBH = Utils.GetInt(Utils.GetQueryStringValue("hotelid"));
            var info = new EyouSoft.BLL.SupplierStructure.SupplierHotel().GetHotelInfo(jiuDianBH);
            
            if (info != null)
            {
                if (!string.IsNullOrEmpty(info.UnitName)) lblHotelName.Text = info.UnitName;
                lblHotelStar.Text = "三星以下";
                if (info.Star != 0 && info.Star != EyouSoft.Model.EnumType.SupplierStructure.HotelStar._3星以下) lblHotelStar.Text = info.Star.ToString();
                if (!string.IsNullOrEmpty(info.UnitAddress)) lblHotelAddress.Text = info.UnitAddress;
                lblHotelInfo.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(info.Introduce);

                if (info.SupplierPic != null && info.SupplierPic.Count > 0)
                {
                    photos = string.Empty;
                    foreach (var pic in info.SupplierPic)
                    {
                        photos += "<img src='" + pic.PicPath + "'style='margin-bottom:5px' width='86px' height='90px' />&nbsp;&nbsp;";
                    }
                }       

                if (info.RoomTypes != null && info.RoomTypes.Count > 0) InitRooms(info.RoomTypes);
            }
        }

        /// <summary>
        /// 房型价格数据绑定
        /// </summary>
        /// <param name="rooms">room集合</param>
        void InitRooms(IList<EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo> rooms)
        {
            rpt.DataSource = rooms;
            rpt.DataBind();
        }
        #endregion
    }
}
