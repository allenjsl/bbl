using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.systemset.ToGoTerrace
{
    /// <summary>
    /// 同行平台-基础设置
    /// </summary>
    /// 创建人:柴逸宁
    /// 修改人：柴逸宁
    /// 修改时间：2011.5.24
    /// 修改说明 ：增加增加主营线路字段
    ///           增加企业文化字段
    public partial class BaseManage : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        /// <summary>
        /// 标题
        /// </summary>
        protected string SiteTitle = string.Empty;
        /// <summary>
        /// Meta
        /// </summary>
        protected string SiteMeta = string.Empty;
        /// <summary>
        /// 版权
        /// </summary>
        protected string Copyright = string.Empty;
        /// <summary>
        /// 设为首页
        /// </summary>
        protected bool IsSetHome = true;
        /// <summary>
        /// 设为收藏
        /// </summary>
        protected bool IsSetFavorite = true;
        /// <summary>
        /// 附件路径
        /// </summary>
        protected string logoPath = string.Empty;
        /// <summary>
        /// 同行平台-基础设置-Model
        /// </summary>
        protected EyouSoft.BLL.SiteStructure.SiteBasicConfig ssBLL = new EyouSoft.BLL.SiteStructure.SiteBasicConfig();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //同行平台-基础设置-BLL

            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_同行平台栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_同行平台栏目, false);
                return;
            }

            if (!IsPostBack)
            {
                EyouSoft.Model.SiteStructure.SiteBasicConfig ssModel = ssBLL.GetSiteBasicConfig(SiteUserInfo.CompanyID);
                if (ssModel != null)
                {
                    //logo图片路径
                    a_imgHave.HRef = ssModel.LogoPath;
                    //网站title
                    SiteTitle = ssModel.SiteTitle;
                    //Meta
                    SiteMeta = ssModel.SiteMeta;
                    //版权说明
                    txt_Copyright.Text = ssModel.Copyright;
                    //是否设置首页
                    IsSetHome = ssModel.IsSetHome;
                    //是否收藏
                    IsSetFavorite = ssModel.IsSetFavorite;
                    //专线介绍
                    txt_Introduce.Text = ssModel.SiteIntro;
                    //公司介绍
                    txt_gsjs.Text = ssModel.Introduction;
                    //主营线路
                    txtKeyRoute.Text = ssModel.MainRoute;
                    //企业文化
                    txtSchooling.Text = ssModel.CorporateCulture;
                    //联系我们（左）
                    txtLianXiFangShi.Text = ssModel.LianXiFangShi;

                    //判断是否有logo图片
                    if (ssModel.LogoPath.Trim() != "")
                    {
                        this.a_imgHave.Visible = true;
                        this.delimg.Visible = true;
                        this.a_imgNone.Visible = false;
                    }
                    else
                    {
                        this.a_imgHave.Visible = false;
                        this.delimg.Visible = false;
                        this.a_imgNone.Visible = true;
                    }
                }
            }
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //初始化Model并获取公司基本信息
            EyouSoft.Model.SiteStructure.SiteBasicConfig ssModel = ssBLL.GetSiteBasicConfig(SiteUserInfo.CompanyID);
            //判断是否存在基本信息
            if (ssModel == null)
            {
                ssModel = new EyouSoft.Model.SiteStructure.SiteBasicConfig();//不存在基本信息初始化
            }
            if (Request.Files.Count > 0)//判断上传控件个数
            {
                //验证loge格式               
                string[] allowExtensions = new string[] { ".jpeg", ".jpg", ".bmp", ".gif", ".pdf" };//能够上传的格式
                string msg = string.Empty;//提示语
                bool nameForm = EyouSoft.Common.Function.UploadFile.CheckFileType(Request.Files, "workAgree", allowExtensions, null, out msg);//进行上传文件的格式判断
                if (!nameForm)//验证格式不正确
                {
                    lstMsg.Text = msg;//赋值错误信息
                    txt_gsjs.Text = ssModel.Introduction;
                    return;
                }
                //上传
                string filepath = string.Empty;//上传文件路径
                string oldfilename = string.Empty;//上传文件名
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["workAgree"], "SupplierControlFile", out filepath, out oldfilename);//上传文件
                if (result)
                {
                    ssModel.LogoPath = filepath;
                }

            }

            ////////////基础数据model赋值
            //公司编号
            ssModel.CompanyId = SiteUserInfo.CompanyID;
            //网站Title
            ssModel.SiteTitle = Utils.EditInputText(Utils.GetFormValue("txt_Title"));
            //Meta
            ssModel.SiteMeta = Utils.EditInputText(Utils.GetFormValue("txt_Meta"));
            //公司介绍
            ssModel.Introduction = Utils.EditInputText(txt_gsjs.Text);
            //版权说明
            ssModel.Copyright = Utils.EditInputText(txt_Copyright.Text);
            //主营线路
            ssModel.MainRoute = Utils.EditInputText(txtKeyRoute.Text);
            //企业文化
            ssModel.CorporateCulture = Utils.EditInputText(txtSchooling.Text);
            //是否首页
            ssModel.IsSetHome = Utils.GetFormValue("radioIndex") == "1" ? true : false;
            //是否收藏
            ssModel.IsSetFavorite = Utils.GetFormValue("radio") == "1" ? true : false;
            //专线介绍
            ssModel.SiteIntro = Utils.EditInputText(this.txt_Introduce.Text);
            //联系我们（左） 暂未做脚本过滤
            ssModel.LianXiFangShi = this.txtLianXiFangShi.Text;
            //添加更新信息
            if (ssBLL.UpdateSiteInfo(ssModel))
            {
                Response.Write("<script>alert('添加成功!');location.href='BaseManage.aspx';</script>");
            }//添加-同行平台-基础设置
            else
            {
                Response.Write("<script>alert('添加失败!');");
                //EyouSoft.Common.Function.MessageBox.ResponseScript(this, ";alert('保存失败!')");

            }

        }
    }
}
