using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.TeamPlan
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    public class AjaxTeamSettle : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request.QueryString["type"];
            //登录公司ID
            EyouSoft.SSOComponent.Entity.UserInfo userModel = EyouSoft.Security.Membership.UserProvider.GetUser();
            //如果公司ID为0表示没有公司登录 并返回false
            if (userModel == null)
            {
                context.Response.Write("{Islogin:false}");
                return;
            }
            if (type != null && type != "")
            {
                string tourId = context.Request.QueryString["tourId"];
                if (tourId != "")
                {
                    int result = 0;                   
                    switch (type)
                    {
                        case "TiJiao": result = new EyouSoft.BLL.TourStructure.Tour().SetStatus(tourId, EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算); break;
                        case "FuHe": result = new EyouSoft.BLL.TourStructure.Tour().SetIsReview(tourId, true); break;
                        case "JieSu": result = new EyouSoft.BLL.TourStructure.Tour().SetStatus(tourId, EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束); break;
                        case "JiDiao": result = new EyouSoft.BLL.TourStructure.Tour().SetStatus(tourId, EyouSoft.Model.EnumType.TourStructure.TourStatus.回团报账); break;
                        case "TuiHuiCaiWu": result = new EyouSoft.BLL.TourStructure.Tour().SetStatus(tourId, EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算); break;
                    }
                    context.Response.Write(result.ToString());
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
