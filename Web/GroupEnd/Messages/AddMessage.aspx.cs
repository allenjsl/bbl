using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;

namespace Web.GroupEnd.Messages
{
    /// <summary>
    /// 组团 留言
    /// 功能：添加留言信息
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-23 
    /// </summary>
    public partial class AddMessage : FrontPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lblMsgDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.lblMsgMan.Text = SiteUserInfo.ContactInfo.ContactName;
            }
        }

        //标记为弹窗打开
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        //添加留言事件
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //留言标题
            string msgTitle = this.txtTitle.Text;
            //留言内容
            string msgContent = this.txtMsg.Text;

            //留言操作对象
            EyouSoft.BLL.CompanyStructure.CustomerMessageReply bll = new EyouSoft.BLL.CompanyStructure.CustomerMessageReply();
            //声明新的留言model
            EyouSoft.Model.CompanyStructure.CustomerMessageModel model = new EyouSoft.Model.CompanyStructure.CustomerMessageModel();

            #region 属性赋值
            model.CompanyId = SiteUserInfo.CompanyID;
            model.MessageCompanyId = SiteUserInfo.TourCompany.TourCompanyId;
            model.MessageContent = msgContent;
            model.MessageTitle = msgTitle;
            model.MessageTime = DateTime.Now;
            model.ReplyState = EyouSoft.Model.EnumType.CompanyStructure.ReplyState.未回复;
            model.MessagePersonId = SiteUserInfo.ID;
            model.MessagePersonName = SiteUserInfo.UserName;
            #endregion
 
            //数据操作
            if (bll.AddMessage(model))
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('留言成功!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.window.location.href='/GroupEnd/Messages/MessageBoard.aspx'");
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('留言失败!');window.location.href='/GroupEnd/Messages/AddMessage.aspx'");
            }
        }
    }
}
