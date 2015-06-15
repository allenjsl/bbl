using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Common
{
    /// <summary>
    /// 省份城市选择
    /// </summary>
    /// 汪奇志 2012-02-29
    public partial class ProvinceAndCity : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var items = new EyouSoft.BLL.CompanyStructure.Province().GetProvinceInfo(CurrentUserCompanyID);

            string s = "var province={0};";
            if (items != null && items.Count > 0)
            {
                RegisterScript(string.Format(s, Newtonsoft.Json.JsonConvert.SerializeObject(items)));
            }
            else
            {
                RegisterScript(string.Format(s, null));
            }

        }
    }
}
