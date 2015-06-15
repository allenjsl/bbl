using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using Common.Enum;

namespace Web.UserCenter.Message
{
    /// <summary>
    /// 留言详细信息页
    /// 功能：查看留言
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-22 
    /// </summary>
    public partial class MsgInfo : BackPage
    {
        //声明留言bll对象
        private EyouSoft.BLL.CompanyStructure.CustomerMessageReply bll = new EyouSoft.BLL.CompanyStructure.CustomerMessageReply();
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断栏目权限
            if (!CheckGrant(TravelPermission.个人中心_留言板_栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.个人中心_留言板_栏目, false);
                return;
            }
            if (!IsPostBack)
            {
                int id = Utils.GetInt(Utils.GetQueryStringValue("id"));
                if (id != 0)
                {
                    DataInit(id);
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="id">记录ID</param>
        private void DataInit(int id)
        {

            EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel model = bll.GetReplyByMessageId(SiteUserInfo.CompanyID, id, false);
            if (model != null)
            {
                this.lblTitle.Text = model.MessageInfo.MessageTitle;
                this.lblMsgDate.Text = model.MessageInfo.MessageTime == null ? "" : Convert.ToDateTime(model.MessageInfo.MessageTime).ToString("yyyy-MM-dd");
                this.lblMsgMan.Text = model.MessageInfo.MessagePersonName;
                this.lblMsg.Text = model.MessageInfo.MessageContent;
                this.lblState.Text = model.MessageInfo.ReplyState.ToString();
                //回复内容
                this.txtReplyMsg.Text = model.ReplyContent;
                //回复人
                this.txtReplyMan.Text = model.ReplyPersonName == null ? "": model.ReplyPersonName;
                //回复时间
                this.txtReplyDate.Text = model.ReplyTime == null ? "" : Convert.ToDateTime(model.ReplyTime).ToString("yyyy-MM-dd");
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = Utils.GetInt(Utils.GetQueryStringValue("id"));
            EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel model = bll.GetReplyByMessageId(SiteUserInfo.CompanyID, id, false);
            if (model != null)
            {
                bool result = false;
                model.MessageId = id;
                model.ReplyPersonId = SiteUserInfo.ID;
                model.ReplyContent = this.txtReplyMsg.Text;
                model.ReplyPersonName = this.txtReplyMan.Text.Trim();
                model.ReplyTime = Utils.GetDateTime(this.txtReplyDate.Text);
                if (model.MessageInfo.ReplyState == EyouSoft.Model.EnumType.CompanyStructure.ReplyState.已回复)
                {
                    result = bll.UpdateReply(SiteUserInfo.CompanyID, model);
                }
                else
                {
                    result = bll.AddReply(SiteUserInfo.CompanyID, model);
                }
                if (result)
                {
                    EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('回复成功!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.window.location.href='/UserCenter/Message/MessageBoard.aspx';");
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('回复失败!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.window.location.href='/UserCenter/Message/MessageBoard.aspx';");
                }

            }
        }
    }
}
