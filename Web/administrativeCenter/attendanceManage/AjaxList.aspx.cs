using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.administrativeCenter.attendanceManage
{
    /// <summary>
    /// 功能：行政中心-ajax考勤汇总列表和个人考勤列表
    /// 开发人：孙川
    /// 日期：2011-01-18
    /// </summary>
    public partial class AjaxList : Eyousoft.Common.Page.BackPage
    {
        #region 页面参数
        protected int Year;
        protected int Month;
        protected int DepartmentID;
        protected string WorkerNum=string.Empty;
        protected string Name=string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = Utils.GetQueryStringValue("method");
            if (!IsPostBack)
            {
                Year = Utils.GetInt(Request.QueryString["Year"], DateTime.Now.Year);
                Month = Utils.GetInt(Request.QueryString["Month"],DateTime.Now.Month);
                DepartmentID = Utils.GetInt(Request.QueryString["DepartmentID"], -1);
                WorkerNum = Utils.GetQueryStringValue("WorkerNum");
                Name = Utils.GetQueryStringValue("Name");
                if (method == "PersonalList")
                {
                    BindDataPersonalList();
                }
                if (method == "CollectAllList")
                {
                    BindDateAllList();
                }
            }
        }
        #region 个人考勤列表
        /// <summary>
        /// 得到个人考勤列表数据
        /// </summary>
        private void BindDataPersonalList()
        {
            EyouSoft.BLL.AdminCenterStructure.AttendanceInfo bllAttendanceInfo = new EyouSoft.BLL.AdminCenterStructure.AttendanceInfo();
            EyouSoft.Model.AdminCenterStructure.SearchInfo modelSearchInfo=new EyouSoft.Model.AdminCenterStructure.SearchInfo();
            modelSearchInfo.Year=Year;
            modelSearchInfo.Month=Month;
            modelSearchInfo.DepartMentId=DepartmentID;
            modelSearchInfo.ArchiveNo=WorkerNum;
            modelSearchInfo.StaffName= Name;

            IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listPersonnelInfo = bllAttendanceInfo.GetList(CurrentUserCompanyID, modelSearchInfo);
            if (listPersonnelInfo != null && listPersonnelInfo.Count > 0)
            {
                this.crptPersonalList.DataSource = listPersonnelInfo;
                this.crptPersonalList.DataBind();
            }
            else
            {
                this.crptPersonalList.EmptyText = "<table width=\"800\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#000000\" style=\"border-collapse:collapse; line-height:16px;\"><tr><td colspan=\"7\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr></table>";
            }
        }

        /// <summary>
        /// 得到当月考勤统计
        /// </summary>
        public string GetMonthAttendance(int StaffNo)
        {
            EyouSoft.BLL.AdminCenterStructure.AttendanceInfo bllAttendanceInfo = new EyouSoft.BLL.AdminCenterStructure.AttendanceInfo();
            EyouSoft.Model.AdminCenterStructure.AttendanceAbout modelAttendanceAbout = bllAttendanceInfo.GetAttendanceAbout(CurrentUserCompanyID, StaffNo, Year, Month);
            if (modelAttendanceAbout != null)
            {
                return "准点<strong>" + modelAttendanceAbout.Punctuality + "</strong>天,"
                    + "迟到<strong>" + modelAttendanceAbout.Late + "</strong>天,"
                    + "早退<strong>" + modelAttendanceAbout.LeaveEarly + "</strong>天,"
                    + "旷工<strong>" + modelAttendanceAbout.Absenteeism + "</strong>天,"
                    + "休假<strong>" + modelAttendanceAbout.Vacation + "</strong>天,"
                    + "请假<strong>" + modelAttendanceAbout.AskLeave + "</strong>天,"
                    + "加班<strong>" + modelAttendanceAbout.OverTime + "</strong>小时,"
                    + "外出<strong>" + modelAttendanceAbout.Out + "</strong>天，"
                    + "出团<strong>" + modelAttendanceAbout.Group + "</strong>天";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 格式化部门名称
        /// </summary>
        protected string GetDepartmentByList(object DepartmentList)
        {
            IList<EyouSoft.Model.CompanyStructure.Department> lists = null;
            string result = "";
            if (DepartmentList != null)
            {
                lists = (List<EyouSoft.Model.CompanyStructure.Department>)DepartmentList;
            }
            if (lists != null && lists.Count > 0)
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    result += lists[i].DepartName + ",";
                }
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        /// <summary>
        /// 得到当月的考勤详细信息
        /// </summary>
        protected string GetAttendanceDetails(object ListAttendanceInfo,int StaffNo)
        {
            

            IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> listAttendance = null;
            IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> tmp = null;
            if (ListAttendanceInfo != null)
            {
                listAttendance = (IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo>)ListAttendanceInfo;
            }
            if (listAttendance != null && listAttendance.Count > 0)     //查询符合条件的
            {
                tmp = listAttendance.Where(Item => Item.StaffNo == StaffNo).ToList();
            }
            listAttendance = (List<EyouSoft.Model.AdminCenterStructure.AttendanceInfo>)(ListAttendanceInfo);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width=\"800\" border=\"1\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#000000\" style=\"border-collapse:collapse; line-height:16px;\">");
            int monthDays = GetMonth(Year, Month);
            if (tmp != null && tmp.Count > 0)
            {
                sb.Append("<tr>");
                for (int i = 0; i < monthDays; i++)
                {
                    sb.Append("<td width=\"17\" align=\"center\">" + (i + 1) + "</td>");
                }
                sb.Append("</tr>");
                sb.Append("<tr>");
                for (int i = 0; i < monthDays; i++)
                {
                    sb.Append("<td width=\"17\" align=\"center\">" + GetStatus(tmp, new DateTime(Year, Month, i + 1)) + "</td>");
                }
                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr><td colspan=\"" + monthDays + "\">没有考勤记录</td></tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        /// <summary>
        /// 格式化考勤状态
        /// </summary>
        private string GetStatus(IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> listAttendance, DateTime dt)
        {
            string result = "";
            if (listAttendance != null)
            {
                for (int i = 0; i < listAttendance.Count; i++)
                {
                    if (dt.ToString() == listAttendance[i].AddDate.ToString())
                    {
                        result += listAttendance[i].WorkStatus;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 得到当前月的天数
        /// </summary>
        private int GetMonth(int Year, int Month)
        {
            int Days = 0;
            switch (Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    Days = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    Days = 30;
                    break;
                case 2:
                    if (DateTime.IsLeapYear(Year))
                    {
                        Days = 29;
                    }
                    else
                    {
                        Days = 28;
                    }
                    break;
                default: break;
            }
            return Days;
        }
        #endregion


        #region 考勤汇总表
        /// <summary>
        /// 得到考勤汇总表数据
        /// </summary>
        private void BindDateAllList()
        {
            EyouSoft.BLL.AdminCenterStructure.AttendanceInfo bllAttendanceInfo = new EyouSoft.BLL.AdminCenterStructure.AttendanceInfo();
            EyouSoft.Model.AdminCenterStructure.SearchInfo modelSearchInfo = new EyouSoft.Model.AdminCenterStructure.SearchInfo();
            modelSearchInfo.Year = Year;
            modelSearchInfo.Month = Month;
            modelSearchInfo.DepartMentId = DepartmentID;
            modelSearchInfo.ArchiveNo = WorkerNum;
            modelSearchInfo.StaffName = Name;

            IList<EyouSoft.Model.AdminCenterStructure.AttendanceByDepartment> listAttendanceByDepartment = bllAttendanceInfo.GetAttendanceByDepartmentList(CurrentUserCompanyID, modelSearchInfo);
            if (listAttendanceByDepartment != null && listAttendanceByDepartment.Count > 0)
            {
                bool flag=true;
                for (int i = 0; i < listAttendanceByDepartment.Count; i++)
                {
                    if (listAttendanceByDepartment[i].PersonList.Count != 0)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    this.crptCollectAllList.EmptyText = "<table width=\"800\" border=\"1\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#000000\" style=\"border-collapse:collapse; line-height:16px;\"><tr><td colspan=\"34\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr></table>";
                }
                else
                {
                    this.crptCollectAllList.DataSource = listAttendanceByDepartment;
                    this.crptCollectAllList.DataBind();
                }
            }
        }

        /// <summary>
        /// 得到title
        /// </summary>
        protected string GetAttendanceAllDepartTitle()
        { 
            StringBuilder sb = new StringBuilder();
            int monthDays = GetMonth(Year, Month);
            sb.Append("<tr>");
            for (int i = 0; i < monthDays; i++)
            {
                if (i==0)
                {
                    sb.Append("<td width=\"50\" align=\"center\" >部门</td>");
                    sb.Append("<td width=\"53\" align=\"center\" >编号</td>");
                    sb.Append("<td width=\"53\" align=\"center\">姓名</td>");
                }
                sb.Append("<td width=\"16\" align=\"center\">" + (i + 1) + "</td>");
            }
            sb.Append("</tr>");
            return sb.ToString();
        }


        /// <summary>
        /// 得到当月的部门考勤详细信息
        /// </summary>
        /// <param name="ListAttendanceInfo"></param>
        /// <param name="DepartName">部门</param>
        /// <returns>格式化数据</returns>
        protected string GetAttendanceAllDepart(object ListAttendanceInfo,string DepartName)
        {
            IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listPersonel = null;
            IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> tmp = null;
            if(ListAttendanceInfo!=null)
            {
                listPersonel = (List<EyouSoft.Model.AdminCenterStructure.PersonnelInfo>)(ListAttendanceInfo);
            }
            StringBuilder sb = new StringBuilder();
            int monthDays = GetMonth(Year, Month);
            if (listPersonel != null && listPersonel.Count > 0)
            {
                
                for (int j = 0; j < listPersonel.Count; j++)
                {
                    sb.Append("<tr>");
                    if (j == 0)
                    {
                        sb.Append("<td width=\"53\" align=\"center\" rowspan=\"" + listPersonel.Count + "\">"
                            + DepartName + "</td>");
                    }
                    if (listPersonel[j].AttendanceList != null && listPersonel[j].AttendanceList.Count > 0)    
                    {
                        tmp = listPersonel[j].AttendanceList.Where(Item => Item.StaffNo == listPersonel[j].Id).ToList();
                    }

                    for (int i = 0; i < monthDays; i++)
                    {
                        if (i == 0)
                        {
                            sb.Append("<td width=\"50\" align=\"center\">" + listPersonel[j].ArchiveNo + "</td>");
                            sb.Append("<td width=\"53\" align=\"center\">" + listPersonel[j].UserName + "</td>");
                        }
                        sb.Append("<td width=\"16\" align=\"center\">"
                            + GetStatus(tmp, new DateTime(Year, Month, i + 1)) + "</td>");
                    }
                    sb.Append("</tr>");
                }
            }
            return sb.ToString();
        }
        #endregion

    }
}
