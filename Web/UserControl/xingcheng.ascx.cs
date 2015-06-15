/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：行程安排
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\UserControl\xingcheng.ascx
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-14 11:10:31
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
using EyouSoft.Common;

namespace Web.UserControl
{
    public partial class xingcheng : System.Web.UI.UserControl
    {
        public string daysControlName { get; set; }
        public List<EyouSoft.Model.TourStructure.TourPlanInfo> listplan;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //listplan = new List<EyouSoft.Model.TourStructure.TourPlanInfo>();
                //rpt_xingcheng.DataSource = listplan;
                //rpt_xingcheng.DataBind();
            }
        }

        public void Bind(List<EyouSoft.Model.TourStructure.TourPlanInfo> list)
        {
            listplan = list;
            rpt_xingcheng.DataSource = list;
            rpt_xingcheng.DataBind();
        }

        /// <summary>
        /// 获取行程
        /// </summary>
        /// <returns></returns>
         public List<EyouSoft.Model.TourStructure.TourPlanInfo> GetValues()
         {
             List<EyouSoft.Model.TourStructure.TourPlanInfo> list = new List<EyouSoft.Model.TourStructure.TourPlanInfo>();
            for( int i=0;i< Utils.GetFormValues("txt_qujian").Length;i++)
            {
                EyouSoft.Model.TourStructure.TourPlanInfo xc = new EyouSoft.Model.TourStructure.TourPlanInfo();
                xc.Interval = Utils.GetFormValues("txt_qujian")[i];
                xc.Vehicle=Utils.GetFormValues("txt_jiaotong")[i];
                xc.Hotel = Utils.GetFormValues("txt_zhushu")[i];
                string hd_eat = Utils.GetFormValues("eat")[i];
                //if (hd_eat.Contains("1,"))
                //{
                //    xc.eatOne = 1;
                //}
                //else
                //{
                //    xc.eatOne = 0;
                //}
                //if (hd_eat.Contains("2,"))
                //{
                //    xc.eatTwo = 1;
                //}
                //else
                //{
                //    xc.eatTwo = 0;
                //}
                //if (hd_eat.Contains("3,"))
                //{
                //    xc.eatThree = 1;
                //}
                //else
                //{
                //    xc.eatThree = 0;
                //}
                //if (hd_eat.Contains("4,"))
                //{
                //    xc.eatFour = 1;
                //}
                //else
                //{
                //    xc.eatFour = 0;
                //}
                xc.Dinner = hd_eat;

                xc.Plan = Utils.GetFormValues("txt_xiancheng")[i];
                //xc.img = Utils.GetFormValues("hd_fileUrl")[i];
                //xc.Img = Request.Files[i];
                string []t ={".jpg",".jpeg",".gif",".bmp",".png"};
                string fileUrl="";
                string oldfile = "";
                string msg="";
                if (!EyouSoft.Common.Function.UploadFile.CheckFileType(Request.Files, "file_xc_img", t, null, out msg))
                {
                    EyouSoft.Common.Function.MessageBox.Show(Page, msg);
                    return null;
                }
                //for (int j = 0; j < Request.Files.Count; j++)
                {
                    if (Request.Files.GetKey(i).ToString().ToUpper() == "file_xc_img".ToUpper())
                    {
                        EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files[i], "File",out fileUrl,out oldfile);
                    }
                }
                if (Request.Files[i].FileName == "" && Utils.GetFormValues("hd_fileUrl")[i] != "")
                {
                    xc.FilePath = Utils.GetFormValues("hd_fileUrl")[i];
                }
                else
                {
                    xc.FilePath = fileUrl;
                }
                list.Add(xc);
            }
            return list;
        }

         protected void rpt_xingcheng_ItemDataBound(object sender, RepeaterItemEventArgs e)
         {
             EyouSoft.Model.TourStructure.TourPlanInfo model = e.Item.DataItem as
                 EyouSoft.Model.TourStructure.TourPlanInfo;
             Panel pfile = e.Item.FindControl("pnlFile") as Panel;
             HyperLink hlink = e.Item.FindControl("hlink_FileName") as HyperLink;
             if (Utils.GetString(model.FilePath, "") != "")
             {
                 pfile.Visible = true;
                 hlink.NavigateUrl = model.FilePath;
             }
         }
    }
}
namespace Web
{
    /// <summary>
    /// 行程
    /// </summary>
    public class XingCheng
    {
        public string qujian { get; set; }
        public string jiaotong { get; set; }
        public string zhushu { get; set; }
        public int eatOne { get; set; }
        public int eatTwo { get; set; }
        public int eatThree { get; set; }
        public int eatFour { get; set; }
        public string content { get; set; }
        public HttpPostedFile Img { get; set; }
        public string img { get; set; }
        public XingCheng() { }
        public XingCheng(string qujian, string jiaotong, string zhushu, int eat1, int eat2, int eat3, int eat4, string content, HttpPostedFile Img)
        {
            this.qujian = qujian;
            this.jiaotong = jiaotong;
            this.zhushu = zhushu;
            this.eatOne = eat1;
            this.eatTwo = eat2;
            this.eatThree = eat3;
            this.eatFour = eat4;
            this.content = content;
            this.Img = Img;
        }
        public XingCheng(string qujian, string jiaotong, string zhushu, int eat1, int eat2, int eat3, int eat4, string content, string Img)
        {
            this.qujian = qujian;
            this.jiaotong = jiaotong;
            this.zhushu = zhushu;
            this.eatOne = eat1;
            this.eatTwo = eat2;
            this.eatThree = eat3;
            this.eatFour = eat4;
            this.content = content;
            this.img = Img;
        }
    }
}