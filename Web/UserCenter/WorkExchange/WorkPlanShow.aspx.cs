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

namespace Web.UserCenter.WorkExchange
{
    /// <summary>
    /// 页面：个人中心-工作计划查看
    /// 功能：查看
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class WorkPlanShow : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected EyouSoft.Model.PersonalCenterStructure.WorkPlan wpModel = new EyouSoft.Model.PersonalCenterStructure.WorkPlan();
        EyouSoft.BLL.PersonalCenterStructure.WorkPlan wpBll = null;
        EyouSoft.BLL.CompanyStructure.Department dBll = null;
        int tid=0;
        protected StringBuilder acceptname = new StringBuilder();//接收人
        protected bool isshow = false;//是否有权限查看
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_工作交流_工作计划栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_工作交流_工作计划栏目, true);
            }

            wpBll = new EyouSoft.BLL.PersonalCenterStructure.WorkPlan(SiteUserInfo);
            dBll = new EyouSoft.BLL.CompanyStructure.Department();
            if (!IsPostBack)
            {
                ShowInit();
            }
        }

        /// <summary>
        /// 绑定查看数据
        /// </summary>
        private void ShowInit()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));//计划ID
            if (tid > 0)
            {
                wpModel = wpBll.GetModel(tid);
                if (wpModel != null)
                {
                    //计划作者是否当前用户，是的话有查看权限
                    if (wpModel.OperatorId == SiteUserInfo.ID)
                    {
                        isshow = true;
                    }
                    //接收人附值并判断当前用户是否在接收人列表中
                    if (wpModel.AcceptList != null)
                    {
                        if (wpModel.AcceptList.Count > 0)
                        {
                            bool temp = false;
                            foreach (EyouSoft.Model.PersonalCenterStructure.WorkPlanAccept wpa in wpModel.AcceptList)
                            {
                                if (temp)
                                {
                                    acceptname.Append(",");
                                }
                                acceptname.Append(wpa.AccetpName);
                                temp = true;
                                //当前用户是否是计划的接收人，是的话有查看权限
                                if (wpa.AccetpId == SiteUserInfo.ID)
                                {
                                    isshow = true;
                                }
                            }
                        }
                    }
                    //当前用户是否是计划作者的主管或上级，是的话有查看权限
                    if (!isshow)
                    {
                        isshow = dBll.JudgePermission(wpModel.OperatorId, SiteUserInfo.ID) > 0 ? true : false;
                    }
                }
                
            }
        }
    }
}
