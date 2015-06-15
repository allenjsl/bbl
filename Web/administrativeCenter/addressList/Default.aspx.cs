using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.administrativeCenter.addressList
{
    /// <summary>
    /// 功能：行政中心-内部通讯录
    /// 开发人：孙川
    /// 日期：2011-01-13
    /// </summary>
    public partial class Default : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_内部通讯录_栏目))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_内部通讯录_栏目, true);
                }
                DepartmentInit();
            }
        }

        /// <summary>
        /// 初始话部门
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
    }
}
