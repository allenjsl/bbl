using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.administrativeCenter.attendanceManage
{
    /// <summary>
    /// 功能：行政中心-ajax考勤管理列表
    /// 开发人：孙川
    /// 日期：2011-01-18
    /// </summary>
    public partial class AjaxAttendanceList : Eyousoft.Common.Page.BackPage
    {
        #region 查询数据
        string StaffNo ; //员工编号
        string StaffName = string.Empty;      //姓名
        int DepartmentId ; //部门
        #endregion

        #region 分页
        int PageIndex = 1;
        int PageSize = 20;         
        int CurrentPage = 0;        //当前页
        int RecordCount;            //总条数
        #endregion

        #region 权限参数
        protected bool EditFlag = false;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_考勤管理_考勤登记))
                {
                    EditFlag = true;
                }
                StaffNo = Utils.GetQueryStringValue("WorkerNum");
                StaffName = Utils.GetQueryStringValue("Name");
                DepartmentId = Utils.GetInt(Request.QueryString["Department"]);
                PageIndex = Utils.GetInt(Request.QueryString["Page"]);
                BindData();
            }

        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            EyouSoft.BLL.AdminCenterStructure.AttendanceInfo bllAttendance = new EyouSoft.BLL.AdminCenterStructure.AttendanceInfo();
            IList<EyouSoft.Model.AdminCenterStructure.AttendanceAbout> listAttendanceAbout = bllAttendance.GetList(PageSize, PageIndex, ref RecordCount, StaffNo, StaffName, DepartmentId, CurrentUserCompanyID);
            if (listAttendanceAbout != null && listAttendanceAbout.Count > 0)
            {
                this.crptAttendanceList.DataSource = listAttendanceAbout;
                this.crptAttendanceList.DataBind();
                this.BindPage();
            }
            else
            {
                this.crptAttendanceList.EmptyText = "<tr><td colspan=\"7\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
                this.ExporPageInfoSelect1.Visible = false;
            }
        }

        /// <summary>
        /// 设置分页控件参数
        /// </summary>
        private void BindPage()
        {
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.HrefType = Adpost.Common.ExporPage.HrefTypeEnum.JsHref;
            this.ExporPageInfoSelect1.AttributesEventAdd("onclick", "AttDefault.LoadData(this);", 1);
            this.ExporPageInfoSelect1.AttributesEventAdd("onchange", "AttDefault.LoadData(this);", 0);
        }

        /// <summary>
        /// 序号
        /// </summary>
        protected int GetCount()
        {
            return ++CurrentPage + (PageIndex - 1) * PageSize;
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
