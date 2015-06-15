/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：xianluWindow.ascx.cs
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\UserControl\xianluWindow.ascx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-12 16:21:39
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// ////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserControl
{
    public partial class xianluWindow : System.Web.UI.UserControl
    {
        /// <summary>
        /// 文本样式
        /// </summary>
        public string textClass { get; set; }
        /// <summary>
        /// 发布类型,2标准，1快速,3选择所有线路，4组团询价选择
        /// </summary>
        public int publishType { get; set; }
        /// <summary>
        /// 获取值
        /// </summary>
        public string Name
        {
            get
            {
                return txt_xl_Name.Text;
            }
            set {
                txt_xl_Name.Text = value;
            }
        }
        /// <summary>
        /// 只读
        /// </summary>
        public bool isRead
        { get; set; }
        /// <summary>
        /// 获取ID
        /// </summary>
        public string Id
        {
            get
            {
                return hd_xl_id.Value;
            }
            set {
                hd_xl_id.Value = value;
            }
        }
        /// <summary>
        /// 是否返回回调方法
        /// </summary>
        public string callBack
        {
            get;
            set;
        }

        /// <summary>
        /// 指定点击文本框的时候是否弹出线路选择框，默认不弹出
        /// </summary>
        public bool IsClickTextBoxPopUpWindow
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_xl_Name.CssClass = textClass;
                if (publishType != 1 && publishType != 2 && publishType != 3 && publishType != 4)
                {
                    throw  new IndexOutOfRangeException("请设置发布类型!");
                }
                if (isRead)
                {
                    txt_xl_Name.ReadOnly = true;
                }
                if (IsClickTextBoxPopUpWindow == true)
                {
                    txt_xl_Name.Style.Add("cursor", "pointer");
                    if (publishType == 4)
                    {
                        txt_xl_Name.Attributes["onclick"] = ";return openXLwindow('/Common/GroupxianluList.aspx');";
                    }
                    else
                    {
                        txt_xl_Name.Attributes["onclick"] = ";return openXLwindow('/Common/xianluList.aspx');";
                    }
                }
            }            
        }
        public void Bind()
        {
            txt_xl_Name.Text = this.Name;
            hd_xl_id.Value = this.Id;
        }


    }
}