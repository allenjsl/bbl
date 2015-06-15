using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EyouSoft.Common;

namespace Web.GroupEnd
{
    public partial class NoticeShow : Eyousoft.Common.Page.FrontPage
    {
        #region 变量

        protected int tid;
        protected EyouSoft.Model.CompanyStructure.News nModel = new EyouSoft.Model.CompanyStructure.News();
        EyouSoft.BLL.CompanyStructure.News nBll = null;


        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            nBll = new EyouSoft.BLL.CompanyStructure.News();
            if (!IsPostBack)
            {
                DataInit();
            }
        }

        private void DataInit()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                nModel = nBll.GetModel(tid);
                nBll.SetClicks(tid);
            }
        }
    }
}
