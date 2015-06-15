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
    /// 功能：行政中心-考勤修改
    /// 开发人：孙川
    /// 日期：2011-01-19
    /// </summary>
    public partial class UpdateRecord : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 弹出窗类型
        /// </summary>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }


        #region 页面变量
        protected DateTime AttStartDate;                 //考勤时间
        protected int radioValue = -1;                  //单选考勤状态
        protected string[] arrCheckboxValue;                //多选考勤状态

        protected string LeaveWhy = string.Empty;                     //请假原因
        protected DateTime? LeaveForStartDate = null;            //请假开始时间
        protected DateTime? LeaveForEndDate = null;              //请假结束时间
        protected string LeaveDayNum = string.Empty;                  //请假天数

        protected string WorkOverTimeContent = string.Empty;              //加班原因
        protected DateTime? WorkOverTimeDateStart = null;            //加班开始时间
        protected DateTime? WorkOverTimeDateEnd = null;             //加班结束时间
        protected string WorkOverTimeNum = string.Empty;                  //加班天数
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int WorkerID = Utils.GetInt(Request.QueryString["WorkerID"], -1);
            string currDateTime=Utils.GetQueryStringValue("DateTime");          //考勤日期
            string Method = Utils.GetFormValue("hidMethod");

            if (!IsPostBack && WorkerID != -1 && Method == "" && currDateTime != "")
            {
                #region 初始化
                int count=0;                //记数
                this.hidWorkerID.Value = WorkerID.ToString();
                int year = Utils.GetInt(currDateTime.Substring(0, 4));
                int month = Utils.GetInt(currDateTime.Substring(4, 2));
                int day = Utils.GetInt(currDateTime.Substring(6, 2));
                EyouSoft.BLL.AdminCenterStructure.AttendanceInfo bllAttendance = new EyouSoft.BLL.AdminCenterStructure.AttendanceInfo();
                IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> listAttendanceInfo = bllAttendance.GetList(CurrentUserCompanyID, WorkerID, new DateTime(year, month, day));
                if (listAttendanceInfo != null && listAttendanceInfo.Count > 0)
                {
                    arrCheckboxValue = new string[listAttendanceInfo.Count];
                    this.crptAttUpdate.DataSource = listAttendanceInfo;
                    this.crptAttUpdate.DataBind();
                    foreach (EyouSoft.Model.AdminCenterStructure.AttendanceInfo modelAttendance in listAttendanceInfo)
                    {
                        AttStartDate = modelAttendance.AddDate;
                        switch ((int)modelAttendance.WorkStatus)
                        {
                            case 0:
                            case 1:
                            case 3:
                            case 4:
                            case 7:
                                radioValue=(int)(modelAttendance.WorkStatus);
                                break;
                            case 2:
                            case 5:
                            case 6:
                            case 8:
                                arrCheckboxValue[count] = ((int)(modelAttendance.WorkStatus)).ToString();
                                count++;
                                break;
                            default: break;
                        }
                        if ((int)(modelAttendance.WorkStatus) == 7)                  //请假
                        {
                            LeaveWhy = modelAttendance.Reason;
                            LeaveForStartDate = modelAttendance.BeginDate;
                            LeaveForEndDate = modelAttendance.EndDate;
                            LeaveDayNum = decimal.Round(modelAttendance.OutTime, 1).ToString();
                        }
                        if ((int)(modelAttendance.WorkStatus) == 8)               //加班
                        {
                            WorkOverTimeContent = modelAttendance.Reason;
                            WorkOverTimeDateStart = modelAttendance.BeginDate;
                            WorkOverTimeDateEnd = modelAttendance.EndDate;
                            WorkOverTimeNum = decimal.Round(modelAttendance.OutTime, 1).ToString();
                        }
                    }
                }
                else
                {
                    this.crptAttUpdate.EmptyText = "<tr><td colspan=\"3\"><div style=\"text-align:center;  margin-top:10px; margin-bottom:10px;\">没有相关的数据!</div></td></tr>";
                }
                #endregion
            }

            if (WorkerID != -1 && Method == "Save")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_考勤管理_考勤登记))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_考勤管理_考勤登记, true);
                }
                #region 保存
                AttStartDate = Utils.GetDateTime(Request.Form["txt_AttStartDate"]);
                radioValue = Utils.GetInt(Request.Form["radio1"]);
                arrCheckboxValue = Utils.GetFormValues("checkbox1");

                EyouSoft.BLL.AdminCenterStructure.AttendanceInfo bllAttendance = new EyouSoft.BLL.AdminCenterStructure.AttendanceInfo();
                
                IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> listAttendance =new List<EyouSoft.Model.AdminCenterStructure.AttendanceInfo>();
                EyouSoft.Model.AdminCenterStructure.AttendanceInfo modelAttendance = null;
                if (AttStartDate.ToString() == "0001-1-1 0:00:00")
                {
                    MessageBox.Show(this, "考勤时间不能为空！");
                    return;
                }
                if (radioValue >=0)                 //单选情况
                {
                    modelAttendance = new EyouSoft.Model.AdminCenterStructure.AttendanceInfo();
                    modelAttendance.StaffNo = WorkerID;
                    modelAttendance.CompanyId = CurrentUserCompanyID;
                    modelAttendance.OperatorId = SiteUserInfo.ID;
                    modelAttendance.AddDate = AttStartDate;
                    modelAttendance.WorkStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus)(radioValue);
                    if (radioValue == 7)
                    {
                        modelAttendance.Reason = Utils.GetFormValue("txt_LeaveWhy");
                        modelAttendance.BeginDate = Utils.GetDateTimeNullable(Request.Form["txt_LeaveForStartDate"]);
                        modelAttendance.EndDate = Utils.GetDateTimeNullable(Request.Form["txt_LeaveForEndDate"]);
                        modelAttendance.OutTime = Utils.GetDecimal(Request.Form["txt_LeaveDayNum"]);
                    }
                    listAttendance.Add(modelAttendance);
                }
                if (arrCheckboxValue != null && arrCheckboxValue.Length > 0)                //多选情况
                {
                    for (int i = 0; i < arrCheckboxValue.Length; i++)
                    {
                        modelAttendance = new EyouSoft.Model.AdminCenterStructure.AttendanceInfo();
                        modelAttendance.StaffNo = WorkerID;
                        modelAttendance.CompanyId = CurrentUserCompanyID;
                        modelAttendance.OperatorId = SiteUserInfo.ID;
                        modelAttendance.AddDate = AttStartDate;
                        modelAttendance.WorkStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus)(Utils.GetInt(arrCheckboxValue[i]));
                        if (arrCheckboxValue[i] == "8")
                        {
                            modelAttendance.Reason = Utils.GetFormValue("txt_WorkOverTimeContent");
                            modelAttendance.BeginDate = Utils.GetDateTimeNullable(Request.Form["txt_WorkOverTimeDateStart"]);
                            modelAttendance.EndDate = Utils.GetDateTimeNullable(Request.Form["txt_WorkOverTimeDateEnd"]);
                            modelAttendance.OutTime = Utils.GetDecimal(Request.Form["txt_WorkOverTimeNum"]);
                        }
                        listAttendance.Add(modelAttendance);
                    }
                }
                if (bllAttendance.Update(CurrentUserCompanyID,listAttendance, AttStartDate))
                {
                    MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.Boxy.getIframeDocument('{2}').location.reload();", "保存成功！", Utils.GetQueryStringValue("iframeId"), Utils.GetQueryStringValue("desid")));
                }
                else
                {
                    MessageBox.Show(this.Page,"修改失败！");
                }
                #endregion
            }
        }

        /// <summary>
        /// 得到备注
        /// </summary>
        public string GetReason(Object WorkStatus, string Reason)
        {
            int attWorkStaus = (int)((EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus)(WorkStatus));
            if (attWorkStaus == 7 || attWorkStaus == 8)
            {
                return Reason;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 初始checkbox多选
        /// </summary>
        public bool GetCheckBoxSelect(string statusValue)
        {
            bool result = false;
            if (arrCheckboxValue != null && arrCheckboxValue.Length > 0)
            {
                foreach (string value in arrCheckboxValue)
                {
                    if (value == statusValue)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
