using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using EyouSoft.SSOComponent.Entity;
using EyouSoft.Common;

namespace Web.ashx
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// 机票审核后台处理
    /// 修改记录:
    /// 1、2010-01-19 曹胡生 创建
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class JiPiaoAudit : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            if (!_IsLogin)
            {
                return;
            }
            string TicketID = EyouSoft.Common.Utils.GetQueryStringValue("RefundId");
            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();
            if (!string.IsNullOrEmpty(TicketID) && bll.CheckTicket(TicketID, Utils.GetFormValue("remark")))
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("审核通过!");
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("审核失败!");
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
