using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.basicinfo
{
    /// <summary>
    /// 品牌编辑
    /// xuty 2011/1/24
    /// </summary>
    public partial class BrandEdit : Eyousoft.Common.Page.BackPage
    {
        protected string brandName = string.Empty;//品牌名称
        protected string logInUrl = string.Empty;//内品牌
        protected string logOutUrl = string.Empty;//外品牌
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_基础设置_品牌管理栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_基础设置_品牌管理栏目, false);
                return;
            }
            brandName = Utils.GetFormValue("txtBrand");//获取报价
            int brandId = Utils.GetInt(Utils.GetQueryStringValue("brandId"));//品牌Id
            string method = Utils.GetFormValue("hidMethod");//获取当前操作(保存/继续)
            string showMess = "数据保存完成";//提示消息
            EyouSoft.Model.CompanyStructure.CompanyBrand brandModel = null;
            EyouSoft.BLL.CompanyStructure.CompanyBrand brandBll = new EyouSoft.BLL.CompanyStructure.CompanyBrand();//初始化bll
            //无操作方式则为初次加载数据
            if (method == "")
            {
                #region 初始化数据
                if (brandId != 0)
                {
                    brandModel = brandBll.GetModel(brandId);
                    if (brandModel != null)
                    {
                        brandName = brandModel.BrandName;
                        logInUrl = !string.IsNullOrEmpty(brandModel.Logo1) ? string.Format("<div id='file' style='float:left'><a href='{0}' target='_blank'>查看图片<a><img src='../../images/closebox2.gif' id='del_file'/></div>", brandModel.Logo1) : "暂无内Logo";
                        if (brandModel.Logo1 != "")
                        {
                            hid_file.Value = brandModel.Logo1;
                        }
                    }
                    return;
                }
                #endregion
            }
            else
            {
                #region 保存数据
                if (brandName == "")
                {
                    MessageBox.Show(this, "品牌名称不为空！");
                    return;
                }
                bool result = false;
                brandModel = new EyouSoft.Model.CompanyStructure.CompanyBrand();
                string logoPath = string.Empty;
                string oldName = string.Empty;
                HttpPostedFile inLogo = Request.Files["inLogo"];//内Logo

                if (inLogo != null)
                    result = UploadFile.FileUpLoad(inLogo, "systemset", out logoPath, out oldName);//上传内logo
                else
                    result = true;
                if (result)//如果成功则上传内logo
                {
                    if (inLogo != null)
                        if (logoPath != "")
                        {
                            hid_file.Value = logoPath;
                        }
                    if (hid_file.Value == "")
                    {
                        brandModel.Logo1 = null;
                    }
                    else
                    {
                        brandModel.Logo1 = hid_file.Value;
                    }


                    brandModel.BrandName = brandName;
                    brandModel.CompanyId = CurrentUserCompanyID;
                    brandModel.OperatorId = 0;
                    brandModel.IssueTime = DateTime.Now;
                }
                if (result)//如果logo上传成功则执行保存
                {
                    if (brandId != 0)
                    {
                        brandModel.Id = brandId;
                        result = brandBll.Update(brandModel);//修改品牌
                    }
                    else
                    {
                        result = brandBll.Add(brandModel);//添加品牌
                    }
                }
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                //继续添加则刷新页面,否则关闭当前窗口
                if (method == "continue")
                {
                    MessageBox.ShowAndRedirect(this, showMess, "BrandEdit.aspx");
                }
                else
                {
                    MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/systemset/basicinfo/BrandManage.aspx';window.parent.Boxy.getIframeDialog('{1}').hide();", showMess, Utils.GetQueryStringValue("iframeId")));
                }
                #endregion
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
