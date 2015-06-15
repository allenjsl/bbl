/// /////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：xianluInfo.ashx.cs
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\ashx\xianluInfo.ashx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-13 10:52:42
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// /////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using EyouSoft.SSOComponent.Entity;
namespace Web.ashx
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class xianluInfo : IHttpHandler
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
            if (context.Request.QueryString["type"] == "1")
            {
                BindPublish(context, userInfo);
            }
            else
            {
                BindQuickPublish(context, userInfo);
            }
        }
        void BindPublish(HttpContext context, UserInfo userInfo)
        {
           
            int companyId = EyouSoft.Common.Utils.GetInt(context.Request["companyId"]);
            
            EyouSoft.BLL.RouteStructure.Route bll = new EyouSoft.BLL.RouteStructure.Route();
            EyouSoft.Model.RouteStructure.RouteInfo model = bll.GetRouteInfo(EyouSoft.Common.Utils.GetInt(context.Request["id"]));
           
            xianlu xl = new xianlu();
            xl.xlArea = model.AreaId;
            xl.xlDays = model.RouteDays;
            xl.xlFuwu = "";
            xl.xlId = model.RouteId;
            xl.xlName = model.RouteName;
            xl.xclist = new List<XingCheng>();
            xl.projectList=model.RouteNormalInfo.Services;
            xl.noProject = model.RouteNormalInfo.BuHanXiangMu;
            xl.buy = model.RouteNormalInfo.GouWuAnPai;
            xl.child = model.RouteNormalInfo.ErTongAnPai;
            xl.Notes = model.RouteNormalInfo.ZhuYiShiXiang;
            xl.owner = model.RouteNormalInfo.ZiFeiXIangMu;
            xl.Reminder = model.RouteNormalInfo.WenXinTiXing;
            foreach (var l in model.RouteNormalInfo.Plans)
            {
                xl.xclist.Add(new XingCheng(l.Interval, l.Vehicle, l.Hotel, l.Dinner.Contains("1") ? 1 : 0, l.Dinner.Contains("2") ? 1 : 0, l.Dinner.Contains("3") ? 1 : 0, l.Dinner.Contains("4") ? 1 : 0, l.Plan, l.FilePath));
                
            }
            context.Response.Write(JsonConvert.SerializeObject(xl));
        }
        /// <summary>
        /// 快速发布
        /// </summary>
        void BindQuickPublish(HttpContext context, UserInfo userInfo)
        {
            int companyId = EyouSoft.Common.Utils.GetInt(context.Request["companyId"]);
            
            EyouSoft.BLL.RouteStructure.Route bll = new EyouSoft.BLL.RouteStructure.Route();
            EyouSoft.Model.RouteStructure.RouteInfo model = bll.GetRouteInfo(EyouSoft.Common.Utils.GetInt(context.Request["id"]));

            xianlu xl = new xianlu();
            xl.xlArea = model.AreaId;
            xl.xlDays = model.RouteDays;
            xl.xlFuwu = model.RouteQuickInfo.Service;
            xl.xlId = model.RouteId;
            xl.xlName = model.RouteName;
            xl.xlXianchen =model.RouteQuickInfo.QuickPlan;
            context.Response.Write(JsonConvert.SerializeObject(xl)); 
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


    /// <summary>
    /// 线路
    /// </summary>
    public class xianlu
    {
        public int xlId
        { get; set; }
        public string xlName
        { get; set; }
        public int xlDays
        { get; set; }
        public string xlXianchen
        { get; set; }
        public string xlFuwu
        { get; set; }
        public int xlArea
        { get; set; }
        public IList<XingCheng> xclist
        { get; set; }
        public IList<EyouSoft.Model.TourStructure.TourServiceInfo> projectList
        {get;set;}
        public string noProject
        { get; set; }
        public string buy
        { get; set; }
        public string child
        { get; set; }
        public string owner
        { get; set; }
        public string Notes { get; set; }
        public string Reminder { get; set; }
    }
}
