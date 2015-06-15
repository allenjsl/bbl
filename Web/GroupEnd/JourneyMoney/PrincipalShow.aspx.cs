using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.GroupEnd.JourneyMoney
{
    /// <summary>
    /// 行程报价-下载报价及行程-负责人查看
    /// 柴逸宁
    /// </summary>
    public partial class PrincipalShow : System.Web.UI.Page
    {

       protected EyouSoft.Model.CompanyStructure.ContactPersonInfo csModel = null;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            csModel = new EyouSoft.Model.CompanyStructure.ContactPersonInfo();
            EyouSoft.BLL.CompanyStructure.CompanyUser csBLL = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            int OperatorID = Utils.GetInt(Utils.GetQueryStringValue("OperatorID"));
            csModel=csBLL.GetUserBasicInfo(OperatorID);

        }
    }
}
