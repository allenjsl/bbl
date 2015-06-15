using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Web.sales;
using System.Collections.Generic;
using Newtonsoft.Json;
using EyouSoft.Model.TourStructure;
using System.IO;
using EyouSoft.SSOComponent.Entity;
using EyouSoft.Common;
//田想兵 3.10 获取区域计调员信息
namespace Web.ashx
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class getAreaOprator : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            if (!_IsLogin)
            {
                return;
            }
            int areaId = Utils.GetInt(Utils.GetQueryStringValue("areaId"));
            if (areaId >0)
            {
                EyouSoft.BLL.CompanyStructure.Area bll = new EyouSoft.BLL.CompanyStructure.Area(userInfo);
                EyouSoft.Model.CompanyStructure.Area model = bll.GetModel(areaId);
                IList<EyouSoft.Model.CompanyStructure.UserArea> list = new
                List<EyouSoft.Model.CompanyStructure.UserArea>();
                if (model != null)
                {
                    list = model.AreaUserList;
                }
                context.Response.Write(JsonConvert.SerializeObject(list));
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
