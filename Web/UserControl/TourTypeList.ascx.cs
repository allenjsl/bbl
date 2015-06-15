using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl
{
    /// <summary>
    /// 页面功能：团队类型控件
    /// Author:liuym
    /// Date:2011-01-28
    /// </summary>
    public partial class TourTypeList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //绑定团队类型
            string[] typeList = Enum.GetNames(typeof(EyouSoft.Model.EnumType.TourStructure.TourType));
            if (typeList != null && typeList.Length > 0)
            {
                foreach (string str in typeList)
                {
                    this.ddlTourType.Items.Add(new ListItem(str, ((int)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), str)).ToString()));
                }
            }
            this.ddlTourType.Items.Insert(0, new ListItem("-请选择-", "-1"));
            if (TourType > -1)
                this.ddlTourType.Items.FindByValue(TourType.ToString()).Selected = true;
            //释放资源
            typeList = null;
        }
        /// <summary>
        /// 团队状态
        /// </summary>
        public int TourType
        {
            get;
            set;
        }
    }
}