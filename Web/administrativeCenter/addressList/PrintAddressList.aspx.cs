using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.administrativeCenter.addressList
{
    /// <summary>
    /// 功能：行政中心-内部通讯录-打印
    /// 开发人：孙川
    /// 日期：2011-01-13
    /// </summary>
    public partial class PrintAddressList : Eyousoft.Common.Page.BackPage
    {
        protected string WorkerName = string.Empty;
        protected int Department;
        protected int i = 0;

        #region 分页参数
        int PageIndex = 1;
        int PageSize = 20;
        int RecordCount;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EyouSoft.BLL.AdminCenterStructure.PersonnelInfo bllAddress = new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo();
                EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo modelAddressSearch = new EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo();
                modelAddressSearch.UserName = WorkerName;
                IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listAddress = bllAddress.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, modelAddressSearch);


                if (RecordCount == 0)
                {
                    RecordCount = 1;
                }
                listAddress = bllAddress.GetList(RecordCount, 1, ref RecordCount, CurrentUserCompanyID, modelAddressSearch);



                if (listAddress != null && listAddress.Count > 0)
                {
                    this.crptPrintAddressList.DataSource = listAddress;
                    this.crptPrintAddressList.DataBind();
                }
                else
                {
                    this.crptPrintAddressList.EmptyText = "<tr><td colspan=\"8\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
                }
            }
        }

        /// <summary>
        /// 格式化所属部门
        /// </summary>
        protected string GetDepartmentString(object o)
        {
            IList<EyouSoft.Model.CompanyStructure.Department> listDepartment = (IList<EyouSoft.Model.CompanyStructure.Department>)(o);
            string listDepartmentToStr = string.Empty;
            if (listDepartment != null && listDepartment.Count > 0)
            {
                for (int i = 0; i < listDepartment.Count; i++)
                {
                    listDepartmentToStr += listDepartment[i].DepartName + ",";
                }
                listDepartmentToStr = listDepartmentToStr.Substring(0, listDepartmentToStr.Length - 1);
            }
            return listDepartmentToStr;
        }
    }
}
