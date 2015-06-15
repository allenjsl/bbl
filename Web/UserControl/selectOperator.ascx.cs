using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ComponentModel;

namespace Web.UserControl
{
    /// <summary>
    ///选择操作人
    ///修改记录:
    ///1、2011-1-12 曹胡生 创建
    /// </summary>
    public partial class selectOperator : System.Web.UI.UserControl
    {
        /// <summary>
        /// 文本框样式
        /// </summary>
        public string TextClass { get; set; }

        /// <summary>
        /// 选择人姓名
        /// </summary>
        public string OperName
        {
            get
            {
                return txt_op_Name.Text;
            }
            set
            {
                txt_op_Name.Text = value;
            }
        }

        /// <summary>
        /// 选择人姓名
        /// </summary>
        public string OperLblName
        {
            get
            {
                return lblShowOperName.Text;
            }
            set
            {
                lblShowOperName.Text = "("+value+")";
            }
        }

        /// <summary>
        /// 控件宽度
        /// </summary>

        private string width = "620px";
        public string Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// 控件高度
        /// </summary>
        private string height = "460px";
        public string Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return lbl_show_title.Text;
            }
            set
            {
                if (value.IndexOf("：") > 0)
                {
                    this.lbl_show_title.Text = value;
                }
                else
                {
                    this.lbl_show_title.Text = value + "：";
                }
            }
        }

        /// <summary>
        /// 是否显示文本框前的Label
        /// </summary>
        [Bindable(true)]
        public bool IsShowLabel
        {
            get
            {
                return lbl_show_title.Visible;
            }
            set
            {
                this.lbl_show_title.Visible = value;
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
                return txt_op_Name.Visible;
            }
            set
            {
                this.txt_op_Name.Visible = value;
            }
        }


        /// <summary>
        /// 选择人ID
        /// </summary>
        public string OperId
        {
            get
            {
                return hd_op_id.Value;
            }
            set
            {
                hd_op_id.Value = value;
            }
        }


        /// <summary>
        /// 回调方法
        /// </summary>
 
        public string callBack
        {
            get;
            set;
        }
        /// <summary>
        ///  是否开启空值验证
        /// </summary>
        [Bindable(false)]
        public bool IsValid
        {
            get;
            set;
        }
        private string _validmsg = "*请选择销售员！";
        /// <summary>
        ///  是否开启空值验证
        /// </summary>
        public string ValidMsg
        {
            get { return _validmsg; }
            set { _validmsg = value; }
        }


        protected bool _IsMultiSelect = true;
        /// <summary>
        ///  是否多选，默认为多选
        /// </summary>
        [Bindable(false)]
        public bool IsMulutSelec
        {
            set
            {
                _IsMultiSelect = value;
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
                this.lbl_show_title.Text = Title;
                if (IsValid)
                {
                    txt_op_Name.Attributes.Add("valid", "required");
                    txt_op_Name.Attributes.Add("errmsg", ValidMsg);
                }

                if (!string.IsNullOrEmpty(Style))
                {
                    txt_op_Name.Attributes.Add("style", Style);
                }
            }
        }
    }
}