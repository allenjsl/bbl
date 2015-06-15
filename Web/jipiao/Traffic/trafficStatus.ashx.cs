using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyouSoft.SSOComponent.Entity;
using EyouSoft.Common;

namespace Web.jipiao.Traffic
{
    public class trafficStatus : IHttpHandler
    {

        /// <summary>
        /// 修改交通状态 李晓欢 2012-09-07
        /// </summary> 
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            if (!_IsLogin)
            {
                return;
            }

            string type = EyouSoft.Common.Utils.GetQueryStringValue("type");
            if (type == "status")
            {
                int id = Utils.GetInt(Utils.GetQueryStringValue("ID"));
                int status = Utils.GetInt(Utils.GetQueryStringValue("status"));
                if (id >= 0)
                {
                    bool result = new EyouSoft.BLL.PlanStruture.PlanTrffic().UpdateChangeSatus(id, (EyouSoft.Model.EnumType.PlanStructure.TrafficStatus)status);
                    if (result)
                    {
                        HttpContext.Current.Response.Write("{\"ret\":\"1\",\"msg\":\"设置成功!\"}");
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("{\"ret\":\"0\",\"msg\":\"设置失败!\"}");
                    }
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
