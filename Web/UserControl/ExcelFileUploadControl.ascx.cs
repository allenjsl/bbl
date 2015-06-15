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
    /// 张新兵，20110120
    /// Excel模板文件上传，在服务端通过 ExcelConvert.ashx转换返回二维数组
    /// 注：引用该用户控件后，调用页面 需在页面头部引入jquery.js,swfupload/swfupload.js,swfupload/default.css
    /// </summary>
    public partial class ExcelFileUploadControl : System.Web.UI.UserControl
    {
        protected string uploadSuccessJavaScriptFunCallBack = "function(){}";
        /// <summary>
        /// 设置文件上传成功后，回调的JavaScript函数名称, 
        /// The CallBackFun exec Like This : FunCalllBack(arr),The Property Initial like this: FunCalllBack
        /// </summary>
        [Bindable(true)]
        public string UploadSuccessJavaScriptFunCallBack
        {
            set
            {
                uploadSuccessJavaScriptFunCallBack = value;
            }
        }

        private string _uploadFrom;
        [Bindable(true)]
        public string UploadFrom
        {
            get { return _uploadFrom; }
            set { _uploadFrom = value; }
        }

        /// <summary>
        /// 是否可以上传txt文件
        /// </summary>
        public bool ContainsTxt
        {
            get;
            set;
        }

        ////游客信息模板下载
        //protected void DownLoad_Click(object sender, EventArgs e)
        //{
        //    string filename = "游客信息";
        //    Response.Clear();
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
        //    Response.ContentEncoding = System.Text.Encoding.Default;
        //    Response.ContentType = "application/ms-excel";
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n", "姓名", "类型", "证件名称", "证件号码", "性别", "联系电话");
        //    Response.Write(sb.ToString());
        //    Response.End();
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (this.UploadFrom)
                {
                    case "客户单位": this.litDown.Text = "<a href='/PrintTemplate/客户单位模板.xls' target='_blank'>客户单位模板下载</a>"; break;
                    case "地接": this.litDown.Text = "<a href='/PrintTemplate/地接模板.xls' target='_blank'>地接模板下载</a>"; break;
                    case "售票处": this.litDown.Text = "<a href='/PrintTemplate/售票处模板.xls' target='_blank'>售票处模板下载</a>"; break;
                    case "酒店": this.litDown.Text = "<a href='/PrintTemplate/酒店模板.xls' target='_blank'>酒店模板下载</a>"; break;
                    case "餐馆": this.litDown.Text = "<a href='/PrintTemplate/餐馆模板.xls' target='_blank'>餐馆模板下载</a>"; break;
                    case "车队": this.litDown.Text = "<a href='/PrintTemplate/车队模板.xls' target='_blank'>车队模板下载</a>"; break;
                    case "景点": this.litDown.Text = "<a href='/PrintTemplate/景点模板.xls' target='_blank'>景点模板下载</a>"; break;
                    case "购物": this.litDown.Text = "<a href='/PrintTemplate/购物模板.xls' target='_blank'>购物模板下载</a>"; break;
                    case "保险": this.litDown.Text = "<a href='/PrintTemplate/保险模板.xls' target='_blank'>保险模板下载</a>"; break;
                    case "航空": this.litDown.Text = "<a href='/PrintTemplate/航空模板.xls' target='_blank'>航空公司模板下载</a>"; break;
                    case "其它": this.litDown.Text = "<a href='/PrintTemplate/其它模板.xls' target='_blank'>其它模板下载</a>"; break;
                    default: this.litDown.Text = "<a href='/PrintTemplate/importtemplate.xls'  target='_blank'>游客信息模板下载</a>"; break;
                }
            }

        }
    }
}