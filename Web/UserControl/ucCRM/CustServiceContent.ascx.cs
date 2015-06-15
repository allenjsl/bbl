using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.UserControl.ucCRM
{   
   /// <summary>
   /// 客户服务中质量管理项的回访结果，投诉内容做成的用户控件
   /// xuty 2011/1/19
   /// </summary>
    public partial class CustServiceContent : System.Web.UI.UserControl
    {
        protected string remarkTitle = "投诉意见";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.routeTag1.isRead = true;
        }
        /// <summary>
        /// 行程安排满意度
        /// </summary>
        public int LevelTravel
        {
            get
            {
                return Utils.GetInt(this.rdiTravel.SelectedValue);
            }
            set
            {
                this.rdiTravel.SelectedValue = value.ToString();
            }

        }
        /// <summary>
        /// 餐饮质量满意度
        /// </summary>
        public int LevelFood
        {
            get
            {
                return Utils.GetInt(this.rdiFoodLevel.SelectedValue);
            }
            set
            {
                this.rdiFoodLevel.SelectedValue = value.ToString();
            }

        }
        /// <summary>
        /// 酒店环境满意度
        /// </summary>
        public int LevelHotelCondition
        {
            get
            {
                return Utils.GetInt(this.rdiHotelCondition.SelectedValue);
            }
            set
            {
                this.rdiHotelCondition.SelectedValue = value.ToString();
            }

        }
        /// <summary>
        /// 景点安排满意度
        /// </summary>
        public int LevelLandScape
        {
            get
            {
                return Utils.GetInt(this.rdiLandscape.SelectedValue);
            }
            set
            {
                this.rdiLandscape.SelectedValue = value.ToString();
            }

        }
        /// <summary>
        /// 导游服务满意度
        /// </summary>
        public int LevelGuideService
        {
            get
            {
                return Utils.GetInt(this.rdiGuide.SelectedValue);
            }
            set
            {
                this.rdiGuide.SelectedValue = value.ToString();
            }

        }
        /// <summary>
        /// 购物安排满意度
        /// </summary>
        public int LevelShopping
        {
            get
            {
                return Utils.GetInt(this.rdiShopping.SelectedValue);
            }
            set
            {
                this.rdiShopping.SelectedValue = value.ToString();
            }

        }
        /// <summary>
        /// 车辆安排满意度
        /// </summary>
        public int LevelCar
        {
            get
            {
                return Utils.GetInt(this.rdiCar.SelectedValue);
            }
            set
            {
                this.rdiCar.SelectedValue = value.ToString();
            }

        }
        /// <summary>
        /// 浏览时间满意度
        /// </summary>
        public int LevelBrowseDate
        {
            get
            {
                return  Utils.GetInt(this.rdiBrowsDate.SelectedValue);
            }
            set
            {
                this.rdiBrowsDate.SelectedValue = value.ToString();
            }

        }
        
        /// <summary>
        /// 出团时间
        /// </summary>
        public DateTime LeaveDate
        {
            get
            {
                return Utils.GetDateTime(this.txtLeaveDate.Value,DateTime.Now);
            }
            set
            {
                this.txtLeaveDate.Value = value.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName
        {
            get
            {
                return Utils.InputText(this.routeTag1.Name);
            }
            set
            {
                this.routeTag1.Name = value;
            }
        }
        /// <summary>
        /// 线路Id
        /// </summary>
        public string RouteId
        {
            get
            {
                return this.routeTag1.Id;
            }
            set
            {
                this.routeTag1.Id = value;
            }
        }

        ///// <summary>
        ///// 投诉意见
        ///// </summary>
        //public string Suggest
        //{
        //    get
        //    {
        //        return Utils.InputText(this.txtSuggest.Value);
        //    }
        //    set
        //    {
        //        this.txtSuggest.Value = value;
        //    }
        //}

        /// <summary>
        /// 是否是来访客户编辑(用来判断是否显示投诉意见)
        /// </summary>
        public Boolean IsVisist
        {
            set
            {
                if (value)
                {
                    remarkTitle = "备注";
                }
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get
            {
                return Utils.InputText(txtRemark.Value);
            }
            set
            {
                this.txtRemark.Value = value;
            }
        }
     
        

    }
}