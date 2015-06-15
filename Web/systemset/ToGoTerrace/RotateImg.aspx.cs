using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.UserControl;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.ToGoTerrace
{
    /// <summary>
    /// 同行平台-图片轮转
    /// </summary>
    /// 柴逸宁
    /// 2011-4-7
    public partial class RotateImg : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 已有数据条数包括空数据
        /// </summary>
        protected int con = 0;
        /// <summary>
        /// 轮换图片的实体
        /// </summary>
        private EyouSoft.Model.SiteStructure.SiteChangePic ssModel = null;
        /// <summary>
        /// 轮换图片list
        /// </summary>
        protected IList<EyouSoft.Model.SiteStructure.SiteChangePic> list = null;

        //轮换图片BLL
        EyouSoft.BLL.SiteStructure.SiteChangePic ssBLL = new EyouSoft.BLL.SiteStructure.SiteChangePic();

        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_同行平台栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_同行平台栏目, false);
                return;
            }
            if (!IsPostBack)
            {
                //初始化轮换图片实体
                ssModel = new EyouSoft.Model.SiteStructure.SiteChangePic();

                //获取轮换图片列表
                list = ssBLL.GetSiteChange(SiteUserInfo.CompanyID);
                //获取记录条数
                con = list.Count;
                //循环5次，对５个实体进行操作
                for (int i = 0; i < 5; i++)
                {
                    //判断用户是否存在第ｉ个实体
                    if (list.Count > i)
                    {
                        //根据循环定义textbox
                        TextBox URL = (TextBox)Page.Form.FindControl("txt_URL" + (i + 1).ToString());
                        ///////对已有图片的URL进行赋值
                        //判断图片的连接是否为空
                        if (list[i].URL.Trim() == "")
                        {
                            URL.Text = "http://";//空的情况下，默认添加“http://”
                        }
                        else
                        {
                            URL.Text = list[i].URL;//不为空则获取列表中对应的连接地址
                        }
                    }

                }

            }
            //异步删除图片操作
            if (Request["id"] != null && Request["bid"] != null)//判断是否为删除操作
            {
                //获取删除图片ID
                int id = Utils.GetInt(Request["id"].ToString());
                //获取原有数据
                ssModel = ssBLL.GetSiteChange(id, SiteUserInfo.CompanyID);
                //清空图片路径
                ssModel.FilePath = "";
                //保存数据
                ssBLL.UpdateSiteChangePic(ssModel);

                bool res = false;
                res = ssBLL.UpdateSiteChangePic(ssModel);
                Response.Clear();
                Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
                Response.End();
            }



        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //初始化BLL
            EyouSoft.BLL.SiteStructure.SiteChangePic ssBLL = new EyouSoft.BLL.SiteStructure.SiteChangePic();
            //初始化Model
            ssModel = new EyouSoft.Model.SiteStructure.SiteChangePic();
            //图片路径
            string filepath = string.Empty;
            //操作结果，默认失败
            bool ser = false;
            list = ssBLL.GetSiteChange(SiteUserInfo.CompanyID);

            ////////////////////////////////////////上传
            for (int i = 0; i < 5; i++)
            {
                ///////////////////验证loge格式
                //logo验证格式
                string[] allowExtensions = new string[] { ".jpeg", ".jpg", ".bmp", ".gif", ".pdf" };
                //提示语
                string msg = string.Empty;
                //验证上传文件格式
                bool nameForm = EyouSoft.Common.Function.UploadFile.CheckFileType(Request.Files, "workAgree" + i.ToString(), allowExtensions, null, out msg);
                if (!nameForm)
                {
                    //提示第ｉ个错误提示
                    Literal lstMsg = (Literal)Page.Form.FindControl("lstMsg" + (i + 1).ToString());
                    lstMsg.Text = msg;

                }
                ///////////////////上传
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["workAgree" + i.ToString()], "SupplierControlFile", out filepath, out oldfilename);
                if (result)
                {
                    //获取文件后缀
                    //string a = System.IO.Path.GetExtension(filepath);
                    //图片保存路劲
                    ssModel.FilePath = filepath;
                    //图片连接路径
                    TextBox URL = (TextBox)Page.Form.FindControl("txt_URL" + (i + 1).ToString());
                    ssModel.URL = URL.Text;
                    //专线公司ID
                    ssModel.CompanyId = SiteUserInfo.CompanyID;
                    //上传图片Name
                    ssModel.FileName = oldfilename;//进入页面，不进行任何操作直接保存 Name会清空
                    //上传类型
                    ssModel.ItemType = EyouSoft.Model.EnumType.SiteStructure.ItemType.轮换图片;
                    //主键ID
                    ssModel.Id = Utils.GetInt(Utils.GetFormValue("tid" + (i + 1).ToString()));

                }

                /////////////////////////

                if (ssModel.Id == 0)
                {
                    //添加
                    ser = ssBLL.AddSiteChangePic(ssModel);
                }
                else
                {
                    if (ssModel.FilePath == "")
                    {
                        //修改
                        ssModel.FilePath = list[i].FilePath;


                    }
                    ser = ssBLL.UpdateSiteChangePic(ssModel);
                }
            }
            if (ser)
            {
                Response.Write("<script language=javascript>alert('添加成功!');window.location.href='/systemset/ToGoTerrace/RotateImg.aspx';</script>");
            }
            else
            {
                Response.Write("<script language=javascript>alert('添加失败!')</script>");
            }

        }
    }
}
