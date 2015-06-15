using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.GroupEnd.wh
{
    /// <summary>
    /// 模块名称：望海组团景点
    /// 功能说明：望海组团景点详细页
    /// 创建人：李晓欢
    /// 创建时间：2011-05-25
    /// </summary>
    public partial class SightDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //供应商景点实体类
                EyouSoft.Model.SupplierStructure.SupplierSpot Spot = null;
                //获取供应商id
                string id = EyouSoft.Common.Utils.GetQueryStringValue("Supplierid");
                if (id != "" && !string.IsNullOrEmpty(id))
                {
                    //根据供应商编号获取景点实体
                    Spot = new EyouSoft.BLL.SupplierStructure.SupplierSpot().GetModel(EyouSoft.Common.Utils.GetInt(id));
                    if (Spot != null)
                    {
                        //景点名称
                        this.litSpotName.Text = Spot.UnitName;
                        //地址
                        this.litAddress.Text = Spot.UnitAddress;
                        //所在城市
                        this.LitProc.Text = Spot.ProvinceName;
                        this.LitCity.Text = Spot.CityName;
                        //景点图片
                        IList<EyouSoft.Model.SupplierStructure.SupplierPic> piclist = Spot.SupplierPic;
                        if (piclist != null && piclist.Count > 0)
                        {                            
                            this.RepPicList.DataSource = piclist;
                            this.RepPicList.DataBind();
                        }
                        else
                        {
                            this.RepPicList.Visible = false;
                            this.litmsg.Text = "暂无景点图片";
                        }
                        //景点介绍
                        this.litTourGuide.Text = Spot.TourGuide;
                        //散客价
                        this.litTravelerPrice.Text = Spot.TravelerPrice.ToString("0.00");
                        //团队价
                        this.litTeamprices.Text = Spot.TeamPrice.ToString("0.00");
                        piclist = null;
                    }
                    Spot = null;                    
                }
            }
        }
    }
}
