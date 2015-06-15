using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Web.UserControl
{
    public partial class UCSelectDepartment : System.Web.UI.UserControl
    {
        /// <summary>
        /// 设置图标
        /// </summary>
        public string SetPicture
        {
            get;
            set;
        } 

        /// <summary>
        /// 获取选择人部门名称 多人用","分开
        /// </summary>
        public string GetDepartmentName
        {
            get
            {
                return txt_Department.Value;
            }
            set
            {
                txt_Department.Value = value;
            }
        }
        /// <summary>
        /// 获取选择人部门名称Lbel(多人用","分开)
        /// </summary>
        public string GetDepartment_lblName
        {
            get
            {
                return lblShowOperName.Text;
            }
            set
            {
                lblShowOperName.Text = "(" + value + ")";
            }
        }

        /// <summary>
        /// 获取部门ID 多人用","分开
        /// </summary>
        public string GetDepartId
        {
            get
            {
                return hfDepartId.Value;
            }
            set
            {
                hfDepartId.Value = value;
            }
        }


        /// <summary>
        /// 是否显示文本框后的Label
        /// </summary>
        [Bindable(false)]
        public bool IsShowTextLabel
        {
            get
            {
                return lblShowOperName.Visible;
            }
            set
            {
                this.lblShowOperName.Visible = value;
            }
        }

        /// <summary>
        /// 是否显示文本框
        /// </summary>
        [Bindable(true)]
        public bool IsShowText
        {
            get
            {
                return txt_Department.Visible;
            }
            set
            {
                this.txt_Department.Visible = value;
            }
        }

        /// <summary>
        /// 是否多选 One多选
        /// </summary>
        public string SelNum
        {
            get
            {
                return LabelSelNum.Value;
            }
            set
            {
                LabelSelNum.Value = value;
            }
        }
        /// <summary>
        /// 样式
        /// </summary>
        public string Style { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.imgPath.ImageUrl = SetPicture;

                if (!string.IsNullOrEmpty(Style))
                {
                    txt_Department.Attributes.Add("style", Style);
                }
            }
        }
    }
}