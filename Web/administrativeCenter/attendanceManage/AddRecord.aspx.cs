using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.administrativeCenter.attendanceManage
{
    /// <summary>
    /// 功能：行政中心-考勤登记和批量考勤
    /// 开发人：孙川
    /// 日期：2011-01-19
    /// </summary>
    public partial class AddRecord : Eyousoft.Common.Page.BackPage
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
            int WorkerID = Utils.GetInt(Request.QueryString["WorkerID"], -1);
            string Method = Utils.GetFormValue("hidMethod");

            if (WorkerID != -1 && !IsPostBack)            //考勤登记
            {
                this.hidWorkerID.Value = WorkerID.ToString();
                this.spanMany.Visible = false;
                this.spanState1.Visible = true;
                this.spanState2.Visible = true;
            }
            if (WorkerID == -1 && !IsPostBack)             //批量考勤
            {
                this.spanMany.Visible = true;
                this.spanState1.Visible = false;
                this.spanState2.Visible = false;
            }

            if (Method != "")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_考勤管理_考勤登记))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_考勤管理_考勤登记, true);
                }
                bool result = false;
                DateTime AttStartDate = Utils.GetDateTime(Request.Form["txt_AttStartDate"]);
                string Radio = Utils.GetFormValue("radio1");            //准点 迟到 旷工 休假 请假
                EyouSoft.BLL.AdminCenterStructure.AttendanceInfo bllAttendanceInfo = new EyouSoft.BLL.AdminCenterStructure.AttendanceInfo();
                EyouSoft.Model.AdminCenterStructure.AttendanceInfo modelAttendanceInfo = new EyouSoft.Model.AdminCenterStructure.AttendanceInfo();

                if (WorkerID != -1 && Method == "Save")//考勤登记
                {
                    #region 考勤登记  
                    string msg = string.Empty;
                    string[] Checkboxs = Utils.GetFormValues("checkbox1");      //加班 早退 外出 出团

                    #region 加班 请假
                    string LeaveWhy = Utils.GetFormValue("txt_LeaveWhy");
                    DateTime? LeaveForStartDate = Utils.GetDateTimeNullable(Request.Form["txt_LeaveForStartDate"]);
                    DateTime? LeaveForEndDate = Utils.GetDateTimeNullable(Request.Form["txt_LeaveForEndDate"]);
                    int LeaveDayNum = Utils.GetInt(Request.Form["txt_LeaveDayNum"]);

                    string WorkOverTimeContent = Utils.GetFormValue("txt_WorkOverTimeContent");
                    DateTime? WorkOverTimeDateStart = Utils.GetDateTimeNullable(Request.Form["txt_WorkOverTimeDateStart"]);
                    DateTime? WorkOverTimeDateEnd = Utils.GetDateTimeNullable(Request.Form["txt_WorkOverTimeDateEnd"]);
                    int WorkOverTimeNum = Utils.GetInt(Request.Form["txt_WorkOverTimeNum"]);
                    #endregion

                    #region 验证必填项
                    if (AttStartDate == null)
                    {
                        msg += "考勤时间不能为空！<br/>";
                    }
                    if (Radio != "" && Radio == "5")
                    {
                        if (LeaveForStartDate == null)
                        {
                            msg += "请假起始时间不能为空!<br/>";
                        }
                        if (LeaveForEndDate == null)
                        {
                            msg += "请假结束时间不能为空！<br/>";
                        }
                    }
                    if (Checkboxs != null && Checkboxs.Length > 0)
                    {
                        foreach (string value in Checkboxs)
                        {
                            if (value == "8")
                            {
                                if (WorkOverTimeDateStart == null)
                                {
                                    msg += "加班起始时间不能为空!<br/>";
                                }
                                if (WorkOverTimeDateEnd == null)
                                {
                                    msg += "加班结束时间不能为空！<br/>";
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(msg))
                    {
                        MessageBox.Show(this.Page, msg);
                        return;
                    }
                    #endregion
                    
                    modelAttendanceInfo.CompanyId = CurrentUserCompanyID;
                    modelAttendanceInfo.OperatorId = SiteUserInfo.ID;

                    modelAttendanceInfo.StaffNo = WorkerID;                 //考勤ID
                    modelAttendanceInfo.AddDate = AttStartDate;
                    if (Radio != "")
                    {
                        modelAttendanceInfo.WorkStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus)Utils.GetInt(Radio);
                        if (Radio == "7")
                        {

                            modelAttendanceInfo.Reason = LeaveWhy;
                            modelAttendanceInfo.BeginDate = LeaveForStartDate;
                            modelAttendanceInfo.EndDate = LeaveForEndDate;
                            modelAttendanceInfo.OutTime = LeaveDayNum;
                        }
                        result = bllAttendanceInfo.Add(modelAttendanceInfo);
                        if (!result)
                        {
                            MessageBox.Show(this, "考勤失败！");
                            return;
                        }
                    }
                    if (Checkboxs != null && Checkboxs.Length > 0)
                    {
                        for (int i = 0; i < Checkboxs.Length; i++)
                        {
                            modelAttendanceInfo.WorkStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus)Utils.GetInt(Checkboxs[i]);
                            if (Checkboxs[i] == "8")
                            {
                                modelAttendanceInfo.Reason = WorkOverTimeContent;
                                modelAttendanceInfo.BeginDate = WorkOverTimeDateStart;
                                modelAttendanceInfo.EndDate = WorkOverTimeDateEnd;
                                modelAttendanceInfo.OutTime = WorkOverTimeNum;
                            }
                            result = bllAttendanceInfo.Add(modelAttendanceInfo);
                            if (!result)
                            {
                                MessageBox.Show(this, "考勤失败！");
                                return;
                            }
                        }
                    }
                    if (result)
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "考勤成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/attendanceManage/Default.aspx"));
                    }
                    #endregion
                }
                else if (WorkerID == -1 && Method == "Save")//批量考勤
                {
                    #region 批量考勤
                    string msg = string.Empty;
                    DateTime AttEndDate = Utils.GetDateTime(Request.Form["txt_AttEndDate"]);
                    string Name = Utils.GetFormValue("txt_Name");
                    string ID = Utils.GetFormValue("hiddenID");

                    double num = AttEndDate.Subtract(AttStartDate).TotalDays;       //考勤天数
                    string[] ids = ID.Split(',');                                   //考勤人数

                    DateTime tempTime;

                    if (AttStartDate == null)
                    {
                        msg += "考勤开始时间不能为空！<br/>";
                    }
                    if (AttEndDate == null)
                    {
                        msg += "考勤结束时间不能为空！<br/>";
                    }
                    if (Name == "")
                    {
                        msg += "考勤选择人员不能为空！";
                    }
                    if (AttStartDate == null || AttEndDate == null || Name == "")
                    {
                        MessageBox.Show(this.Page,msg);
                        return;
                    }
                    
                    modelAttendanceInfo.CompanyId = CurrentUserCompanyID;
                    modelAttendanceInfo.OperatorId = SiteUserInfo.ID;
                    modelAttendanceInfo.AddDate = AttStartDate;

                    if (Radio != "")
                    {
                        #region 单选情况: 准点 迟到 旷工 休假 请假
                        modelAttendanceInfo.WorkStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus)Utils.GetInt(Radio);
                        tempTime=AttStartDate;
                        for (int i = 0; i < num; i++)
                        {
                            modelAttendanceInfo.AddDate = tempTime;
                            for (int n = 0; n < ids.Length; n++)
                            {
                                modelAttendanceInfo.StaffNo = Utils.GetIntNull(ids[n]);
                                result = bllAttendanceInfo.Add(modelAttendanceInfo);
                                if (!result)
                                {
                                    MessageBox.Show(this, "考勤失败！");
                                    return;
                                }
                            }
                            tempTime = tempTime.AddDays(1);
                        }
                        #endregion
                    }
                    if (result)
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "批量考勤成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/attendanceManage/Default.aspx"));
                    }
                    #endregion
                }
            }            
        }
    }
}
