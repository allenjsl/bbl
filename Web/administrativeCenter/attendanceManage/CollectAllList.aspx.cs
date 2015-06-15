using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.administrativeCenter.attendanceManage
{
    /// <summary>
    /// 功能：行政中心-考勤管理-考勤汇总表
    /// 开发人：孙川
    /// 日期：2011-01-19
    /// </summary>
    public partial class CollectAllList : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.DepartmentInit();
            }
        }

        /// <summary>
        /// 初始话部门和月份
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

            this.dpYear.Items.Clear();
            int year = DateTime.Now.Year;
            for (int i =0; i <10; i++)
            {
                this.dpYear.Items.Add(new ListItem((year - i) + "年", (year - i).ToString()));
            }

            this.dpMonth.Value = DateTime.Now.Month.ToString();
        }
    }
}
