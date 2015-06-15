using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;

namespace Web.administrativeCenter.attendanceManage
{
    /// <summary>
    /// 功能：行政中心-查看
    /// 开发人：孙川
    /// 日期：2011-01-19
    /// </summary>
    public partial class LookDetail : Eyousoft.Common.Page.BackPage
    {

        /// <summary>
        /// 弹出窗类型
        /// </summary>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            int StaffID = EyouSoft.Common.Utils.GetInt(Request.QueryString["WorkerID"], -1);
            string method = EyouSoft.Common.Utils.GetQueryStringValue("method");
            int dataYear = EyouSoft.Common.Utils.GetInt(Request.QueryString["dataYear"], -1);
            int dataMonth = EyouSoft.Common.Utils.GetInt(Request.QueryString["dataMonth"], -1);

            if (!IsPostBack && method == "" && StaffID != -1)                       //初始化当前时间
            {
                int MonthDays = GetMonth(DateTime.Now.Year, DateTime.Now.Month);
                this.hiddenWorkerID.Value = StaffID.ToString();
                GetJsonData(StaffID, MonthDays, DateTime.Now.Year, DateTime.Now.Month);
            }

            if (method != "" && dataYear != -1 && dataMonth != -1 && StaffID != -1) //初始化指定时间
            {
                int MonthDays = GetMonth(dataYear, dataMonth);
                GetJsonData(StaffID, MonthDays, dataYear, dataMonth);
                Response.Clear();
                Response.Write(this.hiddenJson.Value);
                Response.End();
            }
        }

        /// <summary>
        /// 得到json数据
        /// </summary>
        private void GetJsonData(int StaffID, int MonthDays,int Year,int Month)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{");

            EyouSoft.BLL.AdminCenterStructure.AttendanceInfo bllAttendanceInfo = new EyouSoft.BLL.AdminCenterStructure.AttendanceInfo();
            IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> listAttendance =null;
            for (int i = 1; i <= MonthDays; i++)
            {
                listAttendance = bllAttendanceInfo.GetList(CurrentUserCompanyID, StaffID, new DateTime(Year, Month, i));
                if (listAttendance != null && listAttendance.Count > 0)
                {
                    string day = Year.ToString() + Month.ToString().PadLeft(2, '0') + i.ToString().PadLeft(2, '0');
                    Json.Append("\"" + i + "\"" + ":\"<a href='javascript:void(0);' onclick='return LookDetail.LookWorker(" + day + "," + StaffID + ");'>" + i + "</a><br/>");
                    for (int j = 0; j < listAttendance.Count; j++)
                    {
                        Json.Append(listAttendance[j].WorkStatus + "&nbsp;");
                        if ((j + 1) % 2 == 0)
                        {
                            Json.Append("<br/>");
                        }
                    }
                    Json.Append("\",");
                }
                else
                {
                    Json.Append("\"" + i + "\"" + ":\"" + i + "\",");
                }
            }
            Json.Remove(Json.ToString().Length - 1, 1);
            Json.Append("}");
            this.hiddenJson.Value = "";
            this.hiddenJson.Value = Json.ToString();
        }

        /// <summary>
        /// 得到当前月的天数
        /// </summary>
        private int GetMonth(int Year,int Month)
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

        ///// <summary>
        ///// 考勤统计
        ///// </summary>
        //private void AttendanceStatistics(int)
        //{
        //    EyouSoft.BLL.AdminCenterStructure.AttendanceInfo bllAttendanceInfo = new EyouSoft.BLL.AdminCenterStructure.AttendanceInfo();
        //    bllAttendanceInfo.GetModel(CurrentUserCompanyID,
        //}
        
    }
}
