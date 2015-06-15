using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.GroupEnd.SystemSetting
{
    /// <summary>
    /// 组团端系统配置
    /// 李晓欢
    /// </summary>
    public partial class DeployManager : Eyousoft.Common.Page.FrontPage
    {
        protected string pageHeader = string.Empty;//页眉
        protected string pageFooter = string.Empty;//页脚
        protected string pageModel = string.Empty;//模板
        protected string departSeal = string.Empty;//部门公章
        protected string PageLogo = string.Empty;//公司logo

        protected void Page_Load(object sender, EventArgs e)
        {
            string method =Utils.GetFormValue("hidMethod");
            EyouSoft.BLL.CompanyStructure.Customer Customer = new EyouSoft.BLL.CompanyStructure.Customer();   
            EyouSoft.Model.CompanyStructure.CustomerConfig CustomerConfig = null;
            if (method == "save")
            {
                //保存
                Customer = new EyouSoft.BLL.CompanyStructure.Customer();
                CustomerConfig = new EyouSoft.Model.CompanyStructure.CustomerConfig();
                string fileName = string.Empty;
                string oldName = string.Empty;
                bool result = true;
                string OldFilePath = "";

                HttpPostedFile fHeader = Request.Files["fileHeader"];
                HttpPostedFile fFooter = Request.Files["fileFooter"];
                HttpPostedFile fSeal = Request.Files["fileSeal"];
                HttpPostedFile fModel = Request.Files["fileModel"];
                HttpPostedFile flogo = Request.Files["fileLogo"];                

                //公司logo
                if (flogo != null && !string.IsNullOrEmpty(flogo.FileName) && flogo.ContentLength > 0)
                {
                    result = UploadFile.FileUpLoad(flogo, "systemset", out fileName, out oldName);
                    CustomerConfig.FilePathLogo = fileName;
                }
                else
                {
                    OldFilePath = Utils.GetFormValue(hidfileLogo.UniqueID);
                    CustomerConfig.FilePathLogo = OldFilePath;
                }  
                //上传页眉
                if (fHeader != null && !string.IsNullOrEmpty(fHeader.FileName) && fHeader.ContentLength>0)
                {
                    result = UploadFile.FileUpLoad(fHeader, "systemset", out fileName, out oldName);                    
                    CustomerConfig.PageHeadFile = fileName;
                }
                else
                {
                    OldFilePath = Utils.GetFormValue(hidFileHeader.UniqueID);
                    CustomerConfig.PageHeadFile = OldFilePath;
                }
                //上传页脚
                if (result && (fFooter != null && !string.IsNullOrEmpty(fFooter.FileName)) && fFooter.ContentLength > 0)
                {
                    result = UploadFile.FileUpLoad(fFooter, "systemset", out fileName, out oldName);
                    CustomerConfig.PageFootFile = fileName;
                }
                else
                {
                    OldFilePath = Utils.GetFormValue(hidFileFooter.UniqueID);
                    CustomerConfig.PageFootFile = OldFilePath;
                }
                //上传模板
                if (result && (fModel != null && !string.IsNullOrEmpty(fModel.FileName)) && fModel.ContentLength > 0)
                {
                    result = UploadFile.FileUpLoad(fModel, "systemset", out fileName, out oldName);
                    CustomerConfig.TemplateFile = fileName;
                }
                else
                {
                    OldFilePath = Utils.GetFormValue(hidFileModel.UniqueID);
                    CustomerConfig.TemplateFile = OldFilePath;
                }
                //上传公章
                if (result && (fSeal != null && !string.IsNullOrEmpty(fSeal.FileName)) && fSeal.ContentLength > 0)
                {
                    result = UploadFile.FileUpLoad(fSeal, "systemset", out fileName, out oldName);
                    CustomerConfig.CustomerStamp = fileName;
                }
                else
                {
                    OldFilePath = Utils.GetFormValue(hidfileSeal.UniqueID);
                    CustomerConfig.CustomerStamp = OldFilePath;
                }                

                 //获得配置信息                
                if(result)
                {
                    CustomerConfig.Id = SiteUserInfo.TourCompany.TourCompanyId;
                    result = Customer.UpdateSampleCustomerConfig(CustomerConfig);
                }
                if (CustomerConfig != null)
                {
                    SetPrintPic(CustomerConfig); 
                }
                MessageBox.ShowAndRedirect(this, result ? "设置成功!" : "设置失败!", "/GroupEnd/SystemSetting/DeployManager.aspx");
                return;
            }

            CustomerConfig = Customer.GetCustomerConfigModel(SiteUserInfo.TourCompany.TourCompanyId);
            if (CustomerConfig != null)
            {
                SetPrintPic(CustomerConfig); 
            }
        }

        /// <summary>
        /// 设置打印模板显示
        /// </summary>
        /// <param name="set"></param>
        protected void SetPrintPic(EyouSoft.Model.CompanyStructure.CustomerConfig CustomerConfig)
        {
            PageLogo = !string.IsNullOrEmpty(CustomerConfig.FilePathLogo) ? string.Format("<a href='{0}' target='_blank'>查看</a>", CustomerConfig.FilePathLogo) : "暂无公司logo";
            hidfileLogo.Value = CustomerConfig.FilePathLogo;
            pageHeader = !string.IsNullOrEmpty(CustomerConfig.PageHeadFile) ? string.Format("<a href='{0}' target='_blank'>查看</a>", CustomerConfig.PageHeadFile) : "暂无页眉";
            hidFileHeader.Value = CustomerConfig.PageHeadFile;
            pageFooter = !string.IsNullOrEmpty(CustomerConfig.PageFootFile) ? string.Format("<a href='{0}' target='_blank'>查看</a>", CustomerConfig.PageFootFile) : "暂无页脚";
            hidFileFooter.Value = CustomerConfig.PageFootFile;
            pageModel = !string.IsNullOrEmpty(CustomerConfig.TemplateFile) ? string.Format("<a href='{0}' target='_blank'>查看</a>", CustomerConfig.TemplateFile) : "暂无模板";
            hidFileModel.Value = CustomerConfig.TemplateFile;
            departSeal = !string.IsNullOrEmpty(CustomerConfig.CustomerStamp) ? string.Format("<a href='{0}' target='_blank'>查看</a>", CustomerConfig.CustomerStamp) : "暂无公章";
            hidfileSeal.Value = CustomerConfig.CustomerStamp;
        }
    }
}
