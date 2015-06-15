using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.GroupEnd.FitHairDay
{
    /// <summary>
    /// 组团端 散客天天发 负责人信息
    /// 创建人：李晓欢
    /// 创建时间:2011-3-22
    /// </summary>
    public partial class Operatorlist : Eyousoft.Common.Page.FrontPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ContacterName = EyouSoft.Common.Utils.GetQueryStringValue("ContacterName");
            if (ContacterName != null)
            {
                this.Lab_Contact.Text = ContacterName;
            }
            string ContacterTelephone = EyouSoft.Common.Utils.GetQueryStringValue("ContacterTelephone");
            if (ContacterTelephone != null)
            {
                this.Lab_Mobile.Text = ContacterTelephone;
            }
            string ContacterFax = EyouSoft.Common.Utils.GetQueryStringValue("ContacterFax");
            if(ContacterFax!=null)
            {
                this.Lab_Fox.Text = ContacterFax;
            }
            string ContacterMobile = EyouSoft.Common.Utils.GetQueryStringValue("ContacterMobile");
            if (ContacterMobile!=null)
            {
                this.Lab_Phone.Text = ContacterMobile;
            }
            string ContacterQQ = EyouSoft.Common.Utils.GetQueryStringValue("ContacterQQ");
            if (ContacterQQ != null)
            {
                this.Lab_QQ.Text = ContacterQQ;
            }
            string ContacterEmail=EyouSoft.Common.Utils.GetQueryStringValue("ContacterEmail");
            if (ContacterEmail != null)
            {
                this.Lab_Email.Text = ContacterEmail;
            }
        }

        #region 设置弹窗页面
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion
    }
}
