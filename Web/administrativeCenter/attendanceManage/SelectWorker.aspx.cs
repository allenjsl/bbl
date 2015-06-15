using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using EyouSoft.Common;

namespace Web.administrativeCenter.attendanceManage
{
    /// <summary>
    /// 功能：行政中心_考勤管理_批量考勤_选择考勤人员
    /// 开发人：孙川
    /// 日期：2011-01-19
    /// </summary>
    public partial class SelectWorker : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 弹出窗类型
        /// </summary>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        #region 分页
        int PageIndex = 1;
        int PageSize = 20;
        int CurrentPage = 0;        //当前页
        int RecordCount;            //总条数
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = Utils.GetQueryStringValue("method");
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
            DepartmentInit();

            if (!IsPostBack && method == "")
            {
                BindData(null, "");
            }
            if (method == "OnSearch")   //查询
            {
                int? DepartmentID = Utils.GetIntNull(Request.QueryString["departmentId"]);
                string Name = Utils.GetQueryStringValue("userName");
                BindData(DepartmentID, Name);
                this.txt_WorkerName.Value = Name;
                if (DepartmentID != null && DepartmentID != 0)
                {
                    this.dpDepartment.Value = DepartmentID.ToString();
                }
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void BindData(int? DepartmentID,string Name) 
        {
            EyouSoft.BLL.AdminCenterStructure.PersonnelInfo bllPersonnelInfo=new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo();
            IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listPersonnelInfo = bllPersonnelInfo.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, Name, DepartmentID);
            if (listPersonnelInfo != null && listPersonnelInfo.Count > 0)
            {
                this.dlPersonnelList.DataSource = listPersonnelInfo;
                this.dlPersonnelList.DataBind();
                this.BindPage();
            }
            else
            {
                this.NoData.Visible = true;
                this.ExportPageInfo1.Visible = false;
            }
        }

        /// <summary>
        /// 部门列表初始化
        /// </summary>
        protected void DepartmentInit()
        {
            EyouSoft.BLL.CompanyStructure.Department bllDepartment = new EyouSoft.BLL.CompanyStructure.Department();
            IList<EyouSoft.Model.CompanyStructure.Department> listDepartment = bllDepartment.GetAllDept(CurrentUserCompanyID);
            this.dpDepartment.Items.Clear();
            this.dpDepartment.Items.Add(new ListItem("--请选择--", "0"));
            if (listDepartment != null && listDepartment.Count > 0)
            {
                foreach (EyouSoft.Model.CompanyStructure.Department modelDepartment in listDepartment)
                {
                    this.dpDepartment.Items.Add(new ListItem(modelDepartment.DepartName, modelDepartment.Id.ToString()));
                }
            }
        }

        /// <summary>
        /// 设置分页控件参数
        /// </summary>
        private void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intRecordCount = RecordCount;
        }
    }
}
