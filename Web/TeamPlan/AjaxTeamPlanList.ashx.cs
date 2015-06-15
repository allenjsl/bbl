using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyouSoft.Common;

namespace Web.TeamPlan
{
    /// <summary>
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-13 
    /// </summary>
    public class AjaxTeamPlanList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = Utils.GetQueryStringValue("type");
            //登录公司ID
            EyouSoft.SSOComponent.Entity.UserInfo userModel = EyouSoft.Security.Membership.UserProvider.GetUser();
            //如果公司ID为0表示没有公司登录 并返回false
            if (userModel == null)
            {
                context.Response.Write("{Islogin:false}");
                return;
            }
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bllOrder = new EyouSoft.BLL.TourStructure.Tour();
            if (type != "")
            {
                if (type == "Delete")
                {
                    string idList = Utils.GetQueryStringValue("idList");
                    string[] list = idList.Split(',');
                    if (list.Count() > 0)
                    { 
                        for (int i = 0; i < list.Count(); i++)
                        {
                            if (list[i].Trim() != "")
                            {
                                //删除
                                bllOrder.Delete(list[i]);
                            }
                        }
                    }
                    context.Response.Write("OK");
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
