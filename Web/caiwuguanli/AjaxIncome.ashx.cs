using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyouSoft.Common;
using Common.Enum;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 功能：AJAX修改杂费收入、支出
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-20
    /// 修改人：柴逸宁
    /// 功能：AJAX修改杂费收入、支出状态
    /// 修改时间：2011-08-09
    public class AjaxIncome : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentType = "text/plain";
            string id = context.Request.QueryString["id"];
            string type = context.Request.QueryString["type"];
            string tourId = context.Request.QueryString["tourId"];
            string verifyType = context.Request.QueryString["verifyType"];//审核类型 in:收入 out:支出
            //登录公司ID
            EyouSoft.SSOComponent.Entity.UserInfo userModel = EyouSoft.Security.Membership.UserProvider.GetUser();
            //如果公司ID为0表示没有公司登录 并返回false
            if (userModel == null)
            {
                context.Response.Write("{Islogin:false}");
                return;
            }

            if (!string.IsNullOrEmpty(verifyType))
            {
                if (verifyType == "in" && !userModel.PermissionList.Contains((int)TravelPermission.财务管理_杂费收入_收入审核))
                {
                    context.Response.Write("{\"retcode\":1000}");
                    return;
                }

                if (verifyType == "out" && !userModel.PermissionList.Contains((int)TravelPermission.财务管理_杂费支出_支出审核))
                {
                    context.Response.Write("{\"retcode\":1000}");
                    return;
                }
            }            

            EyouSoft.BLL.FinanceStructure.OtherCost bll = null;
            string cat = Utils.GetQueryStringValue("cat");
            if (cat.Length > 0)
            {
                bll = new EyouSoft.BLL.FinanceStructure.OtherCost(userModel);
                switch (cat)
                {
                    case "status":
                        int SetOtherCostStatusint = 0;
                        string status = Utils.GetQueryStringValue("status");

                        string otherCostId = Utils.GetQueryStringValue("IncomeId");
                        switch (status)
                        {
                            case "True":
                                SetOtherCostStatusint = bll.SetOtherCostStatus(false, otherCostId);
                                break;
                            case "False":
                                SetOtherCostStatusint = bll.SetOtherCostStatus(true, otherCostId);
                                break;
                            default:
                                context.Response.Write("Error");
                                break;
                        }
                        if (SetOtherCostStatusint == 1)
                        {
                            context.Response.Write("OK");
                        }
                        else
                        {
                            context.Response.Write("Error");
                        }
                        break;
                    default:
                        context.Response.Write("Error");
                        break;
                }
                context.Response.End();
            }
            if (id != null && id.Trim() != "" && type != null && type.Trim() != "")
            {
                //REMOVE MODEL
                bll = new EyouSoft.BLL.FinanceStructure.OtherCost(userModel);
                string[] idArray = new string[] { id };
                switch (type)
                {
                    //杂费收入
                    case "income":
                        if (bll.DeleteOtherIncome(tourId, idArray) > 0)
                        {
                            context.Response.Write("OK");
                            return;
                        }
                        else
                        {
                            context.Response.Write("Error");
                            return;
                        }
                    //杂费支出
                    case "expend":
                        if (bll.DeleteOtherOut(tourId, idArray) > 0)
                        {
                            context.Response.Write("OK");
                            return;
                        }
                        else
                        {
                            context.Response.Write("Error");
                            return;
                        }
                }
                context.Response.Write("Error");
            }
            else
            {
                context.Response.Write("Error");
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
