using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using EyouSoft.Common;

namespace Web.TeamPlan
{    /// <summary>
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-13 
    /// </summary>
    public class AjaxFastVersion : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //操作类型
            string type = context.Request.QueryString["type"];
            //计划ID
            string id = context.Request.QueryString["id"];
            //线路区域ID
            string areaId = context.Request.QueryString["areaId"];
            //团号
            string teamNum = context.Request.QueryString["teamNum"];
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
                if (id != null && id != "" && type != null && type.Trim() != "")
                {
                    string str = "";
                    //GetModel
                    //线路库业务逻辑类
                    EyouSoft.BLL.RouteStructure.Route RouteBll = new EyouSoft.BLL.RouteStructure.Route();
                    //线路基本信息实体类
                    EyouSoft.Model.RouteStructure.RouteInfo Routeinfo = RouteBll.GetRouteInfo(Utils.GetInt(id));
                    if (Routeinfo != null)
                    {
                        //快速版
                        if (type == "Fast")
                        {
                            //0天数 1行程安排 2服务标准
                            var obj = new { dayCount = Routeinfo.RouteDays.ToString(), travel = Routeinfo.RouteQuickInfo.QuickPlan, services = Routeinfo.RouteQuickInfo.Service };
                            str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        }
                        //标准版
                        if (type == "Standard")
                        {
                            //0天数  1行程安排  2添加附件  3包含项目 4不包含项目  5购物安排  6儿童安排 7自费项目 8注意事项  9温馨提示 
                            str = "≮dayCount:\"{0}\",travel:{1},filePath:\"{2}\",project:{3},noPro:\"{4}\",buyPlan:\"{5}\",childPlan:\"{6}\",selfPro:\"{7}\",note:\"{8}\",tips:\"{9}\"≯";

                            //行程安排JSON
                            StringBuilder travel = new StringBuilder();
                            //判断是否有行程信息数据 
                            if (Routeinfo.RouteNormalInfo.Plans != null && Routeinfo.RouteNormalInfo.Plans.Count > 0)
                            {
                                travel.Append("[");
                                for (int i = 0; i < Routeinfo.RouteNormalInfo.Plans.Count; i++)
                                {
                                    //添加行程信息数据
                                    travel.Append("{qujian:\"" + Routeinfo.RouteNormalInfo.Plans[i].Interval + "\",jiaotong:\"" + Routeinfo.RouteNormalInfo.Plans[i].Vehicle + "\",zhushu:\"" + Routeinfo.RouteNormalInfo.Plans[i].Hotel + "\",eatOne:\"" + (Routeinfo.RouteNormalInfo.Plans[i].Dinner.Contains("1") ? "1" : "0") + "\",eatTwo:\"" + (Routeinfo.RouteNormalInfo.Plans[i].Dinner.Contains("2") ? "2" : "0") + "\",eatThree:\"" + (Routeinfo.RouteNormalInfo.Plans[i].Dinner.Contains("3") ? "3" : "0") + "\",eatFour:\"" + (Routeinfo.RouteNormalInfo.Plans[i].Dinner.Contains("4") ? "4" : "0") + "\",content:\"" + Routeinfo.RouteNormalInfo.Plans[i].Plan + "\",fileField:\"" + Routeinfo.RouteNormalInfo.Plans[i].FilePath + "\",img:\"" + Routeinfo.RouteNormalInfo.Plans[i].FilePath + "\"},");
                                }
                                //移除最后一个 ，
                                travel.Remove(travel.Length - 1, 1);
                                travel.Append("]");
                            }
                            else
                            {
                                travel.Append("[]");
                            }

                            //包含项目
                            StringBuilder project = new StringBuilder();
                            if (Routeinfo.RouteNormalInfo.Services != null && Routeinfo.RouteNormalInfo.Services.Count > 0)
                            {
                                project.Append("[");
                                for (int i = 0; i < Routeinfo.RouteNormalInfo.Services.Count; i++)
                                {
                                    //添加包含项目数据
                                    project.Append("{selectPro:\"" + Convert.ToInt16(Routeinfo.RouteNormalInfo.Services[i].ServiceType) + "\",standard:\"" + Routeinfo.RouteNormalInfo.Services[i].Service + "\"},");
                                }
                                project.Remove(project.Length - 1, 1);
                                project.Append("]");
                            }
                            else
                            {
                                project.Append("[]");
                            }
                            string attach = "";
                            //如果附件存在 则取第一个显示
                            if (Routeinfo.Attachs != null && Routeinfo.Attachs.Count > 0)
                            {
                                attach = Routeinfo.Attachs[0].Name;
                            }
                            str = GetNewString(string.Format(str, Routeinfo.RouteDays.ToString(), travel.ToString(), attach, project.ToString(), Routeinfo.RouteNormalInfo.BuHanXiangMu, Routeinfo.RouteNormalInfo.GouWuAnPai, Routeinfo.RouteNormalInfo.ErTongAnPai, Routeinfo.RouteNormalInfo.ZiFeiXIangMu, Routeinfo.RouteNormalInfo.ZhuYiShiXiang, Routeinfo.RouteNormalInfo.WenXinTiXing));
                            str = str.Replace('≮', '{');
                            str = str.Replace('≯', '}');

                        }
                        context.Response.Write(str);
                        return;
                    }
                    else
                    {
                        context.Response.Write("");
                        return;
                    }
                }

                //验证团号是否存在
                if (type == "CheckTeamNum" && teamNum != null && teamNum.Trim() != "")
                {
                    //将团号加入数组
                    string[] tourCode = new string[] { teamNum };
                    //获得重复的数据集合
                    IList<string> count = new EyouSoft.BLL.TourStructure.Tour().ExistsTourCodes(userModel.CompanyID, null, tourCode);
                    if (count != null && count.Count > 0)
                    {
                        //如果该团号存在返回false
                        context.Response.Write("NO");
                    }
                    else
                    {
                        //如果团号不存在返回true
                        context.Response.Write("OK");
                    }
                }

                if (type == "GetAreaUser" && areaId != null && areaId.Trim() != "")
                {
                    string str = "";
                    EyouSoft.Model.CompanyStructure.Area model = new EyouSoft.BLL.CompanyStructure.Area().GetModel(Utils.GetInt(areaId));
                    IList<EyouSoft.Model.CompanyStructure.UserArea> list = null;
                    if (model != null)
                    {
                        list = model.AreaUserList;
                        for (int i = 0; i < list.Count; i++)
                        {
                            str += "{uid:\"" + list[i].UserId + "\",uName:\"" + list[i].ContactName + "\"}|||";
                        }
                    }

                    context.Response.Write(str);
                }
            }
        }

        protected string GetNewString(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                val = val.Replace("\n", "##n##");
            }
            else
            {
                val = "";
            }
            return val;
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
