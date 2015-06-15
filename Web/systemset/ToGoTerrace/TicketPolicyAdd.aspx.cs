using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.ToGoTerrace
{
    /// <summary>
    /// 同行平台-机票政策添加页面
    /// </summary>
    /// 柴逸宁
    /// 2011-4-6
    public partial class TicketPolicyAdd : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 机票政策
        /// </summary>
        protected string filePath = string.Empty;
        /// <summary>
        /// 修改时记录ID
        /// </summary>
        protected int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.Model.SiteStructure.TicketPolicy ssModel = new EyouSoft.Model.SiteStructure.TicketPolicy();
            EyouSoft.BLL.SiteStructure.TicketPolicy ssBLL = new EyouSoft.BLL.SiteStructure.TicketPolicy();

            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_同行平台栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_同行平台栏目, false);
                return;
            }
            if (!IsPostBack)
            {
                //修改时加载原有数据
                if (Utils.GetQueryStringValue("type") == "modify")
                {
                    //获取id
                    id = Utils.GetInt(Utils.GetQueryStringValue("tid"));
                    //获取model
                    ssModel = ssBLL.GetTicketPolicy(id, SiteUserInfo.CompanyID);
                    //机票政策内容
                    txt_Contert.Text = ssModel.Content;
                    //机票政策Title
                    txt_Title.Text = ssModel.Title;
                    //附件路劲
                    filePath = ssModel.FilePath;
                }

            }
        }
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="Y">bool 保存or保存并继续添加 </param>
        private void Save()
        {
            //初始化Model
            EyouSoft.Model.SiteStructure.TicketPolicy ssModel = new EyouSoft.Model.SiteStructure.TicketPolicy();
            //初始化BLL
            EyouSoft.BLL.SiteStructure.TicketPolicy ssBLL = new EyouSoft.BLL.SiteStructure.TicketPolicy();
            if (txt_Title.Text.Trim() == "")//验证标题不能为空
            {
                MessageBox.ResponseScript(this, Utils.ShowMsg("标题不能为空！"));
                lit_Title.Text = "标题不能为空！";
                return;
            }
            ///////////修改状态填充原有数据
            //判断操作
            if (Utils.GetQueryStringValue("type") == "modify")//修改操作
            {
                //获取id
                id = Utils.GetInt(Utils.GetFormValue("tid"));
                //获取Model
                ssModel = ssBLL.GetTicketPolicy(id, SiteUserInfo.CompanyID);
            }
            /////////////////////
            ////////////////////model赋值
            ssModel.CompanyId = SiteUserInfo.CompanyID;
            //内容
            ssModel.Content = txt_Contert.Text;
            //判断上传控件个数
            if (Request.Files.Count > 0)
            {
                //上传文件名
                string oldfilename = string.Empty;
                //上传文件路劲
                string filepath = string.Empty;
                //上传文件
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["workAgree"], "SupplierControlFile", out filepath, out oldfilename);
                if (result)
                {
                    //附件路径赋值
                    ssModel.FilePath = filepath;
                }
            }
            ssModel.Title = Utils.GetFormValue("txt_Title");
            bool res = false;//数据 保存是否成功，默认保存失败
            if (id > 0)
            {
                res = ssBLL.UpdateTicketPolicy(ssModel);//修改

            }
            else
            {
                res = ssBLL.AddTicketPolicy(ssModel);//添加
            }

            if (res)
            {
                /////////////////////////保存成功
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();{2}", "保存成功!", Utils.GetQueryStringValue("iframeId"), id > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/systemset/ToGoTerrace/TickePoliyList.aspx';"));
                ////////////////////////////////////////////
            }
            else
            {
                MessageBox.ResponseScript(this, ";alert('保存失败!');");
            }
        }
        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void linkbtnSave_Click(object sender, EventArgs e)
        {
            Save();//保存
        }

    }
}
