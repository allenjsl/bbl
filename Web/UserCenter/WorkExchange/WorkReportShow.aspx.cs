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

namespace Web.UserCenter.WorkExchange
{
    /// <summary>
    /// 页面：个人中心--工作汇报查看
    /// 功能：查看
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class WorkReportShow : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected EyouSoft.Model.PersonalCenterStructure.WorkReport wrModel = new EyouSoft.Model.PersonalCenterStructure.WorkReport();
        protected int tid = 0;//主键ID
        EyouSoft.BLL.PersonalCenterStructure.WorkReport wrBll = null;
        EyouSoft.BLL.CompanyStructure.Department dBll = null;
        protected bool isshow = false;//是否能查看
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_工作交流_工作汇报栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_工作交流_工作汇报栏目, true);
            }
            wrBll = new EyouSoft.BLL.PersonalCenterStructure.WorkReport(SiteUserInfo);
            dBll = new EyouSoft.BLL.CompanyStructure.Department();
            if (!IsPostBack)
            {
                Bind();
            }
        }

        /// <summary>
        /// 查看数据绑定
        /// </summary>
        private void Bind()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"), 0);
            if (tid > 0)
            {
                wrModel = wrBll.GetModel(tid);
                if (wrModel.OperatorId == SiteUserInfo.ID || dBll.JudgePermission(wrModel.OperatorId, SiteUserInfo.ID) > 0)
                {
                    isshow = true;
                }
            }
        }
    }
}
