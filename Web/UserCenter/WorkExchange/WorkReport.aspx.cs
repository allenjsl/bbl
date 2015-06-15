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
    /// 页面：个人中心--工作交流-工作汇报
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class WorkReport : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;

        //搜索变量

        protected EyouSoft.Model.PersonalCenterStructure.QueryWorkReport qwrModel = new EyouSoft.Model.PersonalCenterStructure.QueryWorkReport();
        protected string title = string.Empty;
        protected string reportUser = string.Empty;
        protected string stime = string.Empty;
        protected string etime = string.Empty;
        protected string dep = string.Empty;

        protected int len = 0;

        EyouSoft.BLL.PersonalCenterStructure.WorkReport wrBll = null;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_工作交流_工作汇报栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_工作交流_工作汇报栏目, true);
            }

            wrBll = new EyouSoft.BLL.PersonalCenterStructure.WorkReport(SiteUserInfo);
            string type = Utils.GetQueryStringValue("type");
            if (!IsPostBack)
            {
                switch (type)
                {
                    case "reportdel":
                        ReportDel();
                        break;
                    default:
                        DataInit();
                        break;
                }
                BindDepart();
            }
        }
        /// <summary>
        /// 删除报告
        /// </summary>
        private void ReportDel()
        {
            string[] tid = Utils.GetFormValue("tid").Split(',');
            bool result = false;
            result = wrBll.Delete(tid);
            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", result ? 1 : -1));
            Response.End();
        }

        private void DataInit()
        {
            pageIndex = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);

            qwrModel.Title = Utils.GetQueryStringValue("title");
            qwrModel.DepartmentId = Utils.GetInt(Utils.GetQueryStringValue("dep"));
            qwrModel.OperatorName = Utils.GetQueryStringValue("people");
            qwrModel.CreateSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("stime"));
            qwrModel.CreateEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("etime"));

            IList<EyouSoft.Model.PersonalCenterStructure.WorkReport> list = null;
            list = wrBll.GetList(pageSize, pageIndex, ref recordCount, qwrModel);

            len = list==null?0:list.Count;
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


        protected void BindDepart()
        {
            EyouSoft.BLL.CompanyStructure.Department depBll = new EyouSoft.BLL.CompanyStructure.Department();
            IList<EyouSoft.Model.CompanyStructure.Department> deplist = depBll.GetAllDept(this.CurrentUserCompanyID);

            this.ddldep.DataValueField = "Id";
            this.ddldep.DataTextField = "DepartName";
            this.ddldep.DataSource = deplist;
            this.ddldep.DataBind();
            this.ddldep.Items.Insert(0, new ListItem("请选择","0"));
            this.ddldep.SelectedValue = Convert.ToString(qwrModel.DepartmentId);

        }
    }
}
