using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Web
{
    /// <summary>
    /// 创建人：张新兵，创建时间：2011-01-14
    /// 登录页面
    /// </summary>
    public partial class login : System.Web.UI.Page
    {
        /// <summary>
        /// 外logo
        /// </summary>
        protected string BigLogo = "";//"/images/login01.jpg";

        /// <summary>
        /// 公司名字
        /// </summary>
        protected string CompanyName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //初始化BigLogo,CompanyName
            BigLogo = "";

            EyouSoft.Model.SysStructure.SystemDomain domain = GetCompanyIdByHost(Request.Url.Host.ToLower());

            if (domain != null)
            {
                EyouSoft.Model.CompanyStructure.CompanyInfo companyInfo =
                    new EyouSoft.BLL.CompanyStructure.CompanyInfo().GetModel(domain.CompanyId, domain.SysId);

                if (companyInfo != null)
                {
                    CompanyName = companyInfo.CompanyName;

                    BigLogo = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetCompanyLogo(domain.CompanyId, EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户);
                }
            }
        }

        /// <summary>
        /// 根据当前URL的域名信息 ，获取对应的公司ID
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private EyouSoft.Model.SysStructure.SystemDomain GetCompanyIdByHost(string host)
        {
            EyouSoft.BLL.SysStructure.SystemDomain bll = new EyouSoft.BLL.SysStructure.SystemDomain();
         
            EyouSoft.Model.SysStructure.SystemDomain domain = bll.GetDomain(host);

            return domain;
        }
    }
}
