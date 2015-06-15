using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.SSOComponent.Entity;

namespace EyouSoft.BLL
{
    /// <summary>
    /// 用户信息扩展方法
    /// </summary>
    public static class UserInfoExtension
    {
        /// <summary>
        /// 获取当前用户打印配置信息
        /// </summary>
        public static EyouSoft.Model.CompanyStructure.CompanyPrintTemplate GetDeptPrint(this UserInfo user)
        {
            //if (new EyouSoft.BLL.CompanyStructure.Department().GetDeptPrint(user.ID, user.CompanyID) == null)
            //    return new EyouSoft.BLL.CompanyStructure.CompanySetting().GetCompanyPrintFile(user.CompanyID);
            //return new EyouSoft.BLL.CompanyStructure.Department().GetDeptPrint(user.ID, user.CompanyID);

            EyouSoft.Model.CompanyStructure.CompanyPrintTemplate deptModel = new EyouSoft.BLL.CompanyStructure.Department().GetDeptPrint(user.ID, user.CompanyID);
            EyouSoft.Model.CompanyStructure.CompanyPrintTemplate companyModel = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetCompanyPrintFile(user.CompanyID);

            //EyouSoft.Model.CompanyStructure.CompanyPrintTemplate newModel = new EyouSoft.Model.CompanyStructure.CompanyPrintTemplate();

            if(deptModel.PageHeadFile == "")
                deptModel.PageHeadFile = companyModel.PageHeadFile;
            if(deptModel.PageFootFile == "")
                deptModel.PageFootFile = companyModel.PageFootFile;
            if(deptModel.TemplateFile == "")
                deptModel.TemplateFile = companyModel.TemplateFile;
            if(deptModel.DepartStamp == "")
                deptModel.DepartStamp = companyModel.DepartStamp;

            return deptModel;
        }
    }
}
