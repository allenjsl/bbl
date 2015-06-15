using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common.Function;
using System.Text;
using EyouSoft.SSOComponent.Entity;

namespace Web.masterpage
{

    public partial class Print : System.Web.UI.MasterPage
    {
        protected string DepartStamp = string.Empty;
        protected string PageHeadFile = string.Empty;
        protected string PageFootFile = string.Empty;
        protected UserInfo SiteUserInfo = null;
        protected int CurrentUserCompanyID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out SiteUserInfo);


            if (!IsPostBack)
            {
                this.hidDocName.Value = EyouSoft.Common.Utils.GetQueryStringValue("docName");
                this.hidDocName.Value = "个人详细信息";

                if (SiteUserInfo == null)
                {
                    EyouSoft.BLL.SysStructure.SystemDomain bll = new EyouSoft.BLL.SysStructure.SystemDomain();
                    EyouSoft.Model.SysStructure.SystemDomain domain = bll.GetDomain(Request.Url.Host.ToLower());
                    CurrentUserCompanyID = domain.CompanyId;
                    EyouSoft.BLL.CompanyStructure.Customer Customer = new EyouSoft.BLL.CompanyStructure.Customer();
                    EyouSoft.Model.CompanyStructure.CompanyPrintTemplate CustomerConfig = GetTemplateByCompaneyId(CurrentUserCompanyID);

                    if (CustomerConfig != null)
                    {
                        DepartStamp = CustomerConfig.DepartStamp;
                        PageHeadFile = CustomerConfig.PageHeadFile;
                        PageFootFile = CustomerConfig.PageFootFile;
                    }
                }
                else
                {
                    if (SiteUserInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户)
                    {
                        CurrentUserCompanyID = SiteUserInfo.CompanyID;
                        EyouSoft.Model.CompanyStructure.CompanyPrintTemplate modelDepartmentPrint = GetTemplate();

                        if (modelDepartmentPrint != null)
                        {
                            DepartStamp = modelDepartmentPrint.DepartStamp;
                            PageHeadFile = modelDepartmentPrint.PageHeadFile;
                            PageFootFile = modelDepartmentPrint.PageFootFile;
                        }
                    }

                    else if (SiteUserInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户)
                    {
                        CurrentUserCompanyID = SiteUserInfo.TourCompany.TourCompanyId;
                        EyouSoft.BLL.CompanyStructure.Customer Customer = new EyouSoft.BLL.CompanyStructure.Customer();
                        EyouSoft.Model.CompanyStructure.CustomerConfig CustomerConfig = Customer.GetCustomerConfigModel(CurrentUserCompanyID);

                        if (CustomerConfig != null)
                        {
                            DepartStamp = CustomerConfig.CustomerStamp;
                            PageHeadFile = CustomerConfig.PageHeadFile;
                            PageFootFile = CustomerConfig.PageFootFile;
                        }
                    }

                }


            }
            this.ibtnWord.Attributes.Add("onclick", "ReplaceInput();");

        }

        /// <summary>
        /// word导出
        /// </summary>
        protected void ibtnWord_Click(object sender, ImageClickEventArgs e)
        {
            string printHtml = Request.Form["hidPrintHTML"];
            string saveFileName = HttpUtility.UrlEncode(this.hidDocName.Value + ".doc");
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", saveFileName));
            Response.ContentType = "application/ms-word";
            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<html>\n<head>\n<meta http-equiv=Content-Type content=\"text/html; charset=gb2312\">\n<meta name=ProgId content=Word.Document>");
            strHtml.Append("<style>" + "\n" +
                "<!--" + "\n" +
                "BODY { MARGIN: 0px }" + "\n" +
                "TABLE { BORDER-COLLAPSE: collapse }" + "\n" +
                "TD { FONT-SIZE: 12px; WORD-BREAK: break-all; LINE-HEIGHT: 100%; TEXT-DECORATION: none }" + "\n" +
                "BODY { FONT-SIZE: 12px; WORD-BREAK: break-all; TEXT-DECORATION: none;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt }" + "\n" +
                "p.MsoNormal, li.MsoNormal, div.MsoNormal" + "\n" +
                "{mso-style-parent:\"\";" + "\n" +
                "margin:0cm;" + "\n" +
                "margin-bottom:.0001pt;" + "\n" +
                "text-align:justify;" + "\n" +
                "text-justify:inter-ideograph;" +
                "mso-pagination:none;" + "\n" +
                "font-size:10.5pt;" + "\n" +
                "mso-bidi-font-size:12.0pt;" + "\n" +
                "font-family:\"Times New Roman\";" + "\n" +
                "mso-fareast-font-family:宋体;" + "\n" +
                "mso-font-kerning:1.0pt;}" + "\n" +
                "@page" + "\n" +
                "{mso-page-border-surround-header:no;" + "\n" +
                "mso-page-border-surround-footer:no;}" + "\n" +
                "@page Section1" + "\n" +
                "{size:595.3pt 841.9pt;" + "\n" +
                "margin:1.0cm 1.0cm 1.0cm 1.0cm;" + "\n" +
                "mso-header-margin:0cm;" + "\n" +
                "mso-footer-margin:0cm;" + "\n" +
                "mso-paper-source:0;" + "\n" +
                "layout-grid:15.6pt;}" + "\n" +
                "div.Section1" + "\n" +
                "{page:Section1;}" + "\n" +
                ".BlnFnt { FONT-WEIGHT: bold; FONT-SIZE: 14px }" + "\n" +
                "table{min-height:24px;}" + "\n" +
                ".hand{cursor:pointer;}" + "\n" +
                "body{font:100% Verdana,Arial,Helvetica,sans-serif;font-size:12px;margin:0;padding:0; text-align:center;}" + "\n" +
                "#divContent{width:760px; margin:0 auto;text-align:left;}" + "\n" +
                ".underlineTextBox{border:none;border-bottom:1px solid black;text-align:center;}" + "\n" +
                ".nonelineTextBox{border:none;text-align:center;border-color:white;}" +
                "table{border-collapse:collapse;}table td{border-collapse:collapse;}.table_normal2{border:solid #000;border-width:1px 0 0 1px;}.table_normal2 td,.table_normal2 th{border:solid #000;border-width:0 1px 1px 0;}.table_normal{border:solid #000;border-width:1px 0 0 1px;border:1px solid black;margin:0px;padding:0px;}.table_normal .normaltd{border:solid #000;border-width:0 1px 1px 0;border:1px solid black;}.table_noneborder{border:none;}.table_l_border{border-left:1px solid #000;}.table_t_border{border-top:1px solid #000;}.table_r_border{border-right:1px solid #000;}.table_b_border{border-bottom:1px solid #000;}.td_noneborder{border:none;}.td_l_border{border-left:1px solid #000}.td_l_t_border{border-left:1px solid #000;border-top:1px solid #000;}.td_l_r_border{border-left:1px solid #000;border-right:1px solid #000;}.td_l_b_border{border-left:1px solid #000;border-bottom:1px solid #000;}.td_t_border{border-top:1px solid #000;}.td_t_r_border{border-top:1px solid #000;border-right:1px solid #000;}.td_t_b_border{border-top:1px solid #000;border-bottom:1px solid #000;}.td_r_border{border-right:1px solid #000;}.td_r_b_border{border-right:1px solid #000;border-bottom:1px solid #000}.td_b_border{border-bottom:1px solid #000}.Placeholder5{margin:0;padding:0;height:5px;width:100%;}.Placeholder10{margin:0;padding:0;height:10px;width:100%;}.Placeholder15{margin:0;padding:0;height:15px;width:100%;}.Placeholder20{margin:0;padding:0;height:20px;width:100%;}" +
                "-->" + "\n" +
                "</style>");
            strHtml.Append("</head>\n");
            strHtml.Append("<body lang=ZH-CN style='tab-interval:21.0pt;text-justify-trim:punctuation'>\n<div class=Section1 style='layout-grid:15.6pt'>\n");
            //内容开始
            strHtml.Append(printHtml);
            //内容结束
            strHtml.Append("</div>\n</body>\n</html>");    
            //保存现有线路信息到文件
            Random rnd = new Random();
            //获得文件名
            string RouteInfoFileName = DateTime.Now.ToFileTime().ToString() + rnd.Next(1000, 99999).ToString() + ".doc";
            string tmpName = DateTime.Now.ToFileTime().ToString() + rnd.Next(1000, 99999).ToString() + ".doc";
            string WordTemplateFile = "/PrintTemplate/default.dot";
            if (SiteUserInfo == null)
            {
                EyouSoft.Model.CompanyStructure.CompanyPrintTemplate modelDepartmentPrint = GetTemplateByCompaneyId(CurrentUserCompanyID);
                if (modelDepartmentPrint != null)
                {
                    if (modelDepartmentPrint.TemplateFile != "" && System.IO.File.Exists(Server.MapPath(modelDepartmentPrint.TemplateFile)))
                    {
                        WordTemplateFile = modelDepartmentPrint.TemplateFile;
                    }
                    else
                    {
                        WordTemplateFile = "/PrintTemplate/default.dot";
                    }
                }
            }
            else
            {
                if (SiteUserInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户)
                {
                    EyouSoft.Model.CompanyStructure.CompanyPrintTemplate modelDepartmentPrint = GetTemplate();
                    if (modelDepartmentPrint != null)
                    {
                        if (modelDepartmentPrint.TemplateFile != "" && System.IO.File.Exists(Server.MapPath(modelDepartmentPrint.TemplateFile)))
                        {
                            WordTemplateFile = modelDepartmentPrint.TemplateFile;
                        }
                        else
                        {
                            WordTemplateFile = "/PrintTemplate/default.dot";
                        }
                    }
                }
                else if (SiteUserInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户)
                {
                    EyouSoft.Model.CompanyStructure.CustomerConfig modelCustomerConfig = GetCustomerConfigTemplate();
                    if (modelCustomerConfig != null)
                    {
                        if (modelCustomerConfig.TemplateFile != "" && System.IO.File.Exists(Server.MapPath(modelCustomerConfig.TemplateFile)))
                        {
                            WordTemplateFile = modelCustomerConfig.TemplateFile;
                        }
                        else
                        {
                            WordTemplateFile = "/PrintTemplate/default.dot";
                        }
                    }
                }
            }
            StringValidate objFile = new StringValidate();
            objFile.WriteTextToFile(Server.MapPath("/DocTmpFile/" + RouteInfoFileName), strHtml.ToString());
            //保存到WORD文件
            Adpost.Common.Office.InteropWord objWord = new Adpost.Common.Office.InteropWord();//定义对象
            objWord.Add(Server.MapPath(WordTemplateFile));                                    //打开模板
            objWord.InsertWordFile(Server.MapPath("/DocTmpFile/" + RouteInfoFileName));
            objWord.SaveAs(Server.MapPath("/DocTmpFile/") + tmpName);
            objFile.FileDel(Server.MapPath("/DocTmpFile/" + RouteInfoFileName));
            objWord.Dispose();
            Response.Clear();
            Response.Redirect("/DocTmpFile/" + tmpName);
            Response.End();
        }

        /// <summary>
        /// 得到专线用户打印模版变量
        /// </summary>
        private EyouSoft.Model.CompanyStructure.CompanyPrintTemplate GetTemplate()
        {
            EyouSoft.Model.CompanyStructure.CompanyPrintTemplate modelDepartmentPrint = EyouSoft.BLL.UserInfoExtension.GetDeptPrint(SiteUserInfo); 
            return modelDepartmentPrint;
        }

        /// <summary>
        /// 根据公司ID获取打印模版变量
        /// </summary>
        /// 创建：田想兵 时间：2011.5.24
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.CompanyPrintTemplate GetTemplateByCompaneyId(int companyId)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting sbll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            return sbll.GetCompanyPrintFile(companyId);
        }
        /// <summary>
        /// 得到组团用户打印模版变量
        /// </summary>
        private EyouSoft.Model.CompanyStructure.CustomerConfig GetCustomerConfigTemplate()
        {
            EyouSoft.BLL.CompanyStructure.Customer Customer = new EyouSoft.BLL.CompanyStructure.Customer();
            EyouSoft.Model.CompanyStructure.CustomerConfig CustomerConfig = Customer.GetCustomerConfigModel(SiteUserInfo.TourCompany.TourCompanyId);
            return CustomerConfig;
        }
       
    }
}
