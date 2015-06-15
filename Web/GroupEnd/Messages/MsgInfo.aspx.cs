using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.GroupEnd.Messages
{
    /// <summary>
    /// 组团 留言
    /// 功能：查看留言信息
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-23 
    /// </summary>
    public partial class MsgInfo : FrontPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //留言ID
                int id =Utils.GetInt( Utils.GetQueryStringValue("id"));
                if (id != 0)
                {
                    //初始化
                    DataInit(id);
                }
            }
        }

        //标记为弹窗打开
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="id">记录ID</param>
        protected void DataInit(int id)
        {
            //声明留言操作对象
            EyouSoft.BLL.CompanyStructure.CustomerMessageReply bll = new EyouSoft.BLL.CompanyStructure.CustomerMessageReply();
           //声明回复信息对象
            EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel model = bll.GetReplyByMessageId(SiteUserInfo.TourCompany.TourCompanyId, id, true);
            if (model != null && model.MessageInfo != null)
            {
                //标题
                this.lblTitle.Text = model.MessageInfo.MessageTitle;
                //留言时间
                this.lblMsgDate.Text = model.MessageInfo.MessageTime == null ? "" : Convert.ToDateTime(model.MessageInfo.MessageTime).ToString("yyyy-MM-dd");
                //留言人名
                this.lblMsgMan.Text = model.MessageInfo.MessagePersonName;
                //留言内容
                this.lblMsg.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.MessageInfo.MessageContent);
                //留言回复状态
                this.lblState.Text = model.MessageInfo.ReplyState.ToString();
                //回复内容
                this.lblReplyMsg.Text = EyouSoft.Common.Function.StringValidate.TextToHtml( model.ReplyContent);
                //回复人名
                this.lblReplyMan.Text = model.ReplyPersonName;
                //回复时间
                this.lblReplyDate.Text = model.ReplyTime == null ? "" : Convert.ToDateTime(model.ReplyTime).ToString("yyyy-MM-dd");
            }
        }
    }
}
