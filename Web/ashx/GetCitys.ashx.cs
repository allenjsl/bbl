using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using EyouSoft.Common.Function;
using EyouSoft.Common;

namespace Web.UserControl
{
    /// <summary>
    ///Ajax获取省份下的城市
    ///xuty 2011/1/11
    /// </summary>
     public class GetCitys : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            EyouSoft.SSOComponent.Entity.UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            if (!_IsLogin)
            {
                context.Response.Write("{Islogin:false}");
                return;
            }
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat("<option value=\"0\">请选择</option>");
            int ProvinceId = Utils.GetInt(Utils.GetQueryStringValue("provinceId"));
            int CompanyId = Utils.GetInt(Utils.GetQueryStringValue("companyId"));
            int isFav=Utils.GetInt(Utils.GetQueryStringValue("isFav"));
            bool? IsFav=new Nullable<bool>();
            switch (isFav)
            {
                case 1:
                    IsFav = true;
                    break;
                case 0:
                    IsFav = false;
                    break;
            }
            if (ProvinceId != 0)
            {
                EyouSoft.BLL.CompanyStructure.City cBll = new EyouSoft.BLL.CompanyStructure.City();
                IList<EyouSoft.Model.CompanyStructure.City> list = cBll.GetList(CompanyId, ProvinceId, IsFav);

                if (list != null && list.Count > 0)
                {
                    foreach (EyouSoft.Model.CompanyStructure.City c in list)
                    {
                        strBuilder.AppendFormat("<option value=\"{0}\">{1}</option>", c.Id, c.CityName);
                    }
                }
            }
            context.Response.Write(strBuilder.ToString());
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
