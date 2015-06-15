using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Web.UserControl.wh
{
    public partial class ImagePlayer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
            int companyId = domain.CompanyId;
            int recordCount = 0;
            IList<EyouSoft.Model.SiteStructure.SiteChangePic> items = new EyouSoft.BLL.SiteStructure.SiteChangePic().GetSiteChange(companyId, 6, 1, ref recordCount);

            StringBuilder s = new StringBuilder();
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    s.AppendFormat("imagePlayer.addItem(\"\", \"{0}\", \"{1}\");", (string.IsNullOrEmpty(item.URL) || item.URL.ToLower() == "http://") ? "###" : item.URL, item.FilePath);
                }

                s.AppendFormat("imagePlayer.init(\"imagePlayer\", 710, 290);");
            }

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), s.ToString(), true);
        }
    }
}