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
using System.Collections.Generic;
using EyouSoft.Common;

namespace Web.UserCenter.UserInfo
{
    /// <summary>
    /// 页面：个人中心--工作交流-工作7计划
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class WorkPlan : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected EyouSoft.Model.PersonalCenterStructure.QueryWorkPlan qwpModel = new EyouSoft.Model.PersonalCenterStructure.QueryWorkPlan();
        protected int len = 0;
        EyouSoft.BLL.PersonalCenterStructure.WorkPlan wpbll = null;//工作计划BLL
        EyouSoft.BLL.CompanyStructure.Department dBll = null;//部门BLL

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!CheckGrant(global::Common.Enum.TravelPermission.个人中心_工作交流_工作计划栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_工作交流_工作计划栏目, true);
            }

            wpbll = new EyouSoft.BLL.PersonalCenterStructure.WorkPlan(SiteUserInfo);
            dBll = new EyouSoft.BLL.CompanyStructure.Department();
            string type = Utils.GetQueryStringValue("type");
            if (!IsPostBack)
            {
                switch (type)
                {
                    case "plandel":
                        PlanDel();//计划删除
                        break;
                    default :
                        DataInit();//初使化
                        break;
                }
                DDLBind();
            }
        }

        /// <summary>
        /// 删除计划
        /// </summary>
        private void PlanDel()
        {
            string[] tid = Utils.GetFormValue("tid").Split(',');
            bool result = false;
            result = wpbll.Delete(tid);
            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", result ? 1 : -1));
            Response.End();
        }
        /// <summary>
        /// 初使数据
        /// </summary>
        private void DataInit()
        {
            pageIndex = Utils.GetInt(Request.QueryString["page"], 1);

            qwpModel.Title = Utils.GetQueryStringValue("title");
            qwpModel.OperatorName = Utils.GetQueryStringValue("people");
            int st = Utils.GetInt(Utils.GetQueryStringValue("status"));
            if (st > 0)
            {
                qwpModel.Status = (EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState)st;
            }
            qwpModel.LastSTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("stime"));
            qwpModel.LastETime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("etime"));
            // 数据列表
            IList<EyouSoft.Model.PersonalCenterStructure.WorkPlan> list = null;

            list = wpbll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, SiteUserInfo.ID, qwpModel);
            len = list.Count;
            this.rptList.DataSource = list;
            this.rptList.DataBind();
            //设置分页
            BindPage();
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion
        /// <summary>
        ///  计划状态下拉列表
        /// </summary>
        /// <param name="index"></param>
        protected void DDLBind()
        {
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState));
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = list[i].Value;
                    item.Text = list[i].Text;
                    this.ddlstatus.Items.Add(item);
                }
            }
            this.ddlstatus.SelectedValue = qwpModel.Status==null?"0":Convert.ToInt32(qwpModel.Status).ToString();
        }
        
    }
}
