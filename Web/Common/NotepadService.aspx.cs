using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.Common
{
    /// <summary>
    /// 服务标准模板
    /// </summary>
    public partial class NotepadService : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        private void Bind()
        {            
            int count= 0;
            EyouSoft.Model.EnumType.CompanyStructure.NotepadServiceType type = (EyouSoft.Model.EnumType.CompanyStructure.NotepadServiceType)
                EyouSoft.Common.Utils.GetInt(Request.QueryString["type"], 1);
            EyouSoft.BLL.CompanyStructure.BNotepadService bll = new EyouSoft.BLL.CompanyStructure.BNotepadService();
            IList<EyouSoft.Model.CompanyStructure.MNotepadServiceInfo> list = 
            bll.GetNotepads(SiteUserInfo.CompanyID, 5, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref count,
                type, new EyouSoft.Model.CompanyStructure.MNotepadServiceSearchInfo());
            rptList.DataSource = list;
            rptList.DataBind();

            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);

        }
    }
}
