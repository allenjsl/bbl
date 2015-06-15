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

namespace Web.ashx
{
    /// <summary>
    /// 张新兵，2011-01-27
    /// 根据日期数组和公司ID,生成对应的团号数组
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GenerateTourNumbers : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse Response = context.Response;
            HttpRequest Request = context.Request;

            //需判断 当前会话数据 是否有效
            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);

            if (_IsLogin == false)
            {
                Response.Clear();
                Response.Write("{Islogin:false}");
                Response.End();
            }

            

            //获取公司ID
            int companyId = EyouSoft.Common.Utils.GetInt(Request.QueryString["companyId"]);
            if (companyId == 0)
            {
                EyouSoft.Common.Utils.ResponseMeg(false, "参数错误");
            }
            //获取日期数组
            string arrStr = EyouSoft.Common.Utils.GetFormValue("datestr");

            string[] dateArr = arrStr.Split(',');

            if (dateArr.Length == 0)
            {
                EyouSoft.Common.Utils.ResponseMeg(false, "参数错误");
            }

            List<DateTime> dateList = new List<DateTime>();
            DateTime tmpDate = DateTime.Now;
            for (int i = 0; i < dateArr.Length; i++)
            {
                if (DateTime.TryParse(dateArr[i], out tmpDate))
                {
                    dateList.Add(tmpDate);
                }
            }

            if (dateList.Count == 0)
            {
                EyouSoft.Common.Utils.ResponseMeg(false, "参数错误");
            }

            EyouSoft.BLL.TourStructure.Tour tourBll = new EyouSoft.BLL.TourStructure.Tour();

            IList<EyouSoft.Model.TourStructure.TourChildrenInfo> childInfos = 
                tourBll.CreateAutoTourCodes(companyId, dateList.ToArray());

            System.Text.StringBuilder strb = new System.Text.StringBuilder();
            StringWriter sw = new StringWriter(strb);

            if (childInfos.Count > 0)
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.WriteStartArray();

                    foreach (TourChildrenInfo childInfo in childInfos)
                    {
                        jsonWriter.WriteStartArray();
                        //int i = 0;

                        jsonWriter.WriteValue(childInfo.LDate.ToString("yyyy-M-d"));
                        jsonWriter.WriteValue(childInfo.TourCode);
                        jsonWriter.WriteEndArray();
                    }


                    jsonWriter.WriteEnd();
                }
            }

            Response.Clear();
            Response.Write(strb.ToString());
            Response.End();
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
