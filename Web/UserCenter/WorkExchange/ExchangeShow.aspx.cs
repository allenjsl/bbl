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
using System.Text;
using System.Collections.Generic;

namespace Web.UserCenter.WorkExchange
{
    /// <summary>
    /// 页面：个人中心--工作交流-交流中心
    /// 功能：查看
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class ExchangeShow : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int tid = 0; //主键
        protected EyouSoft.Model.PersonalCenterStructure.WorkExchange exModel = new EyouSoft.Model.PersonalCenterStructure.WorkExchange();
        EyouSoft.BLL.PersonalCenterStructure.WorkExchange weBll = null; //Bll实体
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_工作交流_交流专区栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_工作交流_交流专区栏目, true);
            }
            weBll = new EyouSoft.BLL.PersonalCenterStructure.WorkExchange(SiteUserInfo);//BLl实体初使化
            if (!IsPostBack)
            {
                if (Utils.GetQueryStringValue("type") == "repeat")
                {
                    Check();//交流回复
                }
                else
                {
                    Bind();//绑定初使数据
                }
            }
        }


        /// <summary>
        /// 回复
        /// </summary>
        private void Check()
        {
            bool res = false;//回复是否成功
            tid = Utils.GetInt(Utils.GetFormValue("tid"));//主键ID
            if (tid > 0)
            {
                EyouSoft.Model.PersonalCenterStructure.WorkExchangeReply werModel = new EyouSoft.Model.PersonalCenterStructure.WorkExchangeReply();
                werModel.ExchangeId = tid;
                werModel.IsAnonymous = Utils.GetInt(Utils.GetFormValue("isannoy")) == 1 ? true : false;
                werModel.OperatorId = SiteUserInfo.ID;
                if (werModel.IsAnonymous)
                {
                    werModel.OperatorName = "匿名";
                }
                else
                {
                    werModel.OperatorName = SiteUserInfo.ContactInfo.ContactName;
                }
                werModel.ReplyTime = DateTime.Now;
                werModel.Description = Utils.GetFormValue("connent");
                res = weBll.AddReply(werModel);
            }
            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
        }

        /// <summary>
        /// 绑定交流实体
        /// </summary>
        private void Bind()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            #region 实体附值
            if (tid > 0)
            {
                exModel = weBll.GetModel(tid);
                weBll.SetClicks(tid);
            }
            #endregion
        }
    }
}
