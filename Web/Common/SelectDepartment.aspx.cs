using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Common
{
    /// <summary>
    /// 功能：行政中心-选择部门
    /// 开发人：孙川
    /// 日期：2011-01-14
    /// </summary>
    public partial class SelectDepartment : Eyousoft.Common.Page.BackPage
    {
        protected IList<EyouSoft.Model.CompanyStructure.Department> deplist = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            if (!IsPostBack)
            {
                DepartInit();
            }
        }

        private void DepartInit()
        {
            EyouSoft.BLL.CompanyStructure.Department depBll = new EyouSoft.BLL.CompanyStructure.Department();
            deplist = depBll.GetAllDept(this.CurrentUserCompanyID);
        }

        
    }
}
