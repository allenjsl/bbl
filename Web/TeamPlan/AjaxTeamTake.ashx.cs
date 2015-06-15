using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyouSoft.Common;

namespace Web.TeamPlan
{

    public class AjaxTeamTake : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request.QueryString["type"];
            string id = context.Request.QueryString["id"];
            //登录公司ID
            EyouSoft.SSOComponent.Entity.UserInfo userModel = EyouSoft.Security.Membership.UserProvider.GetUser();
            //如果公司ID为0表示没有公司登录 并返回false
            if (userModel == null)
            {
                context.Response.Write("{Islogin:false}");
                return;
            }
            if (type != null)
            {
                EyouSoft.BLL.PlanStruture.TravelAgency travelbll = new EyouSoft.BLL.PlanStruture.TravelAgency();
                if (type == "Delete")
                {
                    if (id != null && id != "")
                    {
                        bool b = travelbll.DelTravelAgency(id);
                        if (b)
                        {
                            context.Response.Write("OK");
                        }
                        else
                        {
                            context.Response.Write("Error");
                        }
                    }
                }
                if (type == "GetInfo")
                {
                    //声明操作对象
                   EyouSoft.BLL.CompanyStructure.CompanySupplier bll = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
                   EyouSoft.Model.CompanyStructure.CompanySupplier companyModel = bll.GetModel(Utils.GetInt(id), userModel.CompanyID);
                   string returnStr ="{0}||{1}||{2}||{3}";
                   if (companyModel != null)
                   {
                       if (companyModel.SupplierContact != null && companyModel.SupplierContact.Count > 0)
                       {
                           returnStr = string.Format(returnStr, companyModel.SupplierContact[0].ContactName, companyModel.SupplierContact[0].ContactTel, companyModel.Commission.ToString("0.00"),companyModel.LicenseKey);
                       }
                       else
                       {
                           returnStr = string.Format(returnStr, "", "", companyModel.Commission.ToString("0.00"), companyModel.LicenseKey);
                       }
                   }
                    context.Response.Write(returnStr);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
