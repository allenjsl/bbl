using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.administrativeCenter.personnelFiles
{
    /// <summary>
    /// 功能：行政中心-人事档案列表打印
    /// 开发人：孙川
    /// 日期：2011-01-17
    /// </summary>
    public partial class Print : Eyousoft.Common.Page.BackPage
    {
        #region 查询参数
        string FileNo = string.Empty;
        string Name = string.Empty;
        int? Sex = null;
        DateTime? BirthdayStart = null;
        DateTime? BirthdayEnd = null;
        int? WorkYearFrom = null;
        int? WorkYearTo = null;
        int? JobPostion = null;
        int? WorkerType = null;
        string WorkerState = string.Empty;
        string MarriageState = string.Empty;
        int i = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //绑定
             if (!IsPostBack)
             {
                 FileNo = Utils.GetQueryStringValue("FileNo");           //编号
                 Name = Utils.GetQueryStringValue("Name");
                 Sex = Utils.GetIntNull(Request.QueryString["Sex"]);
                 BirthdayStart = Utils.GetDateTimeNullable(Request.QueryString["BirthdayStart"]);
                 BirthdayEnd = Utils.GetDateTimeNullable(Request.QueryString["BirthdayEnd"]);
                 WorkYearFrom = Utils.GetIntNull(Request.QueryString["WorkLife"]);       //工龄
                 WorkYearTo = Utils.GetIntNull(Request.QueryString["WorkLife"]);       //工龄
                 JobPostion = Utils.GetIntNull(Request.QueryString["JobPostion"]);                    //职务
                 WorkerType = Utils.GetIntNull(Request.QueryString["WorkerType"]);                    //类型
                 WorkerState = Utils.GetQueryStringValue("WorkerState");                  //员工状态
                 MarriageState = Utils.GetQueryStringValue("MarriageState");              //婚姻状况
                 BindData();
             }
        }

        /// <summary>
        /// 绑定个人档案详细信息
        /// </summary>
        protected void BindData() 
        {
            EyouSoft.BLL.AdminCenterStructure.PersonnelInfo bllPersonnel = new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo();
            EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo modelPersonnelSearch = new EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo();
            if (FileNo != "")
            {
                modelPersonnelSearch.ArchiveNo = FileNo;
            }
            if (Name != "")
            {
                modelPersonnelSearch.UserName = Name;
            }
            if (Sex != 0)
            {
                modelPersonnelSearch.ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Sex;
            }
            modelPersonnelSearch.BirthDateFrom = BirthdayStart;
            modelPersonnelSearch.BirthDateTo = BirthdayEnd;
            modelPersonnelSearch.WorkYearFrom = WorkYearFrom;
            modelPersonnelSearch.WorkYearTo = WorkYearTo;
            if (JobPostion != 0)
            {
                modelPersonnelSearch.DutyId = JobPostion;
            }
            if (WorkerType != null && WorkerType != -1)
            {
                modelPersonnelSearch.PersonalType = (EyouSoft.Model.EnumType.AdminCenterStructure.PersonalType)(WorkerType);
            }
            if (WorkerState != "null")
            {
                modelPersonnelSearch.IsLeave = Convert.ToBoolean(WorkerState);
            }
            if (MarriageState != "null")
            {
                modelPersonnelSearch.IsMarried = Convert.ToBoolean(MarriageState);
            }
            int RecordCount = 0;
            IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listPersonnel = bllPersonnel.GetList(10000, 1, ref RecordCount, CurrentUserCompanyID, modelPersonnelSearch);
            if (listPersonnel != null && listPersonnel.Count != 0)
            {
                this.rptDataList.DataSource = listPersonnel;
                this.rptDataList.DataBind();
            }
            else
            {
                this.rptDataList.EmptyText = "<tr><td colspan=\"12\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
            }

        }

        /// <summary>
        /// 序号
        /// </summary>
        protected int GetCount()
        {
            return ++i;
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
