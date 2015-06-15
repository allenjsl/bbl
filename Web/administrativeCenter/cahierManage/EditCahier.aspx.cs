using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.administrativeCenter.cahierManage
{
    /// <summary>
    /// 功能：行政中心-会议记录修改
    /// 开发人：孙川
    /// 日期：2011-01-20
    /// </summary>
    public partial class EditCahier : Eyousoft.Common.Page.BackPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        #region 页面变量
        protected string MeetingNum = string.Empty;
        protected string MeetingSubject = string.Empty;
        protected string JoinPersonnel = string.Empty;
        protected DateTime MeetingTimeBegin = Utils.GetDateTime("1900-01-01");
        protected DateTime MeetingTimeEnd = Utils.GetDateTime("1900-01-01");
        protected string MeetingLocale = string.Empty;
        protected string Remark = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int MeetingID =Utils.GetInt( Request.QueryString["MeetingID"],-1);
            string Method = Utils.GetFormValue("hidMethod");
            if (!IsPostBack && MeetingID != -1)
            {
                EyouSoft.BLL.AdminCenterStructure.MeetingInfo bllMeetingInfo = new EyouSoft.BLL.AdminCenterStructure.MeetingInfo();
                EyouSoft.Model.AdminCenterStructure.MeetingInfo modelMeetingInfo = bllMeetingInfo.GetModel(CurrentUserCompanyID, MeetingID);
                if (modelMeetingInfo != null)
                {
                    this.hidMeetingID.Value = modelMeetingInfo.Id.ToString();
                    MeetingNum = modelMeetingInfo.MetttingNo;
                    MeetingSubject = modelMeetingInfo.Title;
                    JoinPersonnel = modelMeetingInfo.Personal;
                    MeetingTimeBegin = modelMeetingInfo.BeginDate;
                    MeetingTimeEnd = modelMeetingInfo.EndDate;
                    MeetingLocale = modelMeetingInfo.Location;
                    Remark = modelMeetingInfo.Remark;
                }
            }
            if (Method == "save")
            {
                if (this.hidMeetingID.Value != "")
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_会议记录_修改记录))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_会议记录_修改记录, true);
                    }
                }
                else
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_会议记录_新增记录))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_会议记录_新增记录, true);
                    }
                }
                MeetingNum = Utils.GetFormValue("txt_MeetingNum");
                MeetingSubject = Utils.GetFormValue("txt_MeetingSubject");
                JoinPersonnel = Utils.GetFormValue("txt_JoinPersonnel");
                MeetingTimeBegin = Utils.GetDateTime(Request.Form["txt_MeetingTimeBegin"], Utils.GetDateTime("1900-01-01"));
                MeetingTimeEnd = Utils.GetDateTime(Request.Form["txt_MeetingTimeEnd"], Utils.GetDateTime("1900-01-01"));
                MeetingLocale = Utils.GetFormValue("txt_MeetingLocale");
                Remark = Utils.GetFormValue("txt_Remark");

                EyouSoft.BLL.AdminCenterStructure.MeetingInfo bllMeetingInfo = new EyouSoft.BLL.AdminCenterStructure.MeetingInfo();
                EyouSoft.Model.AdminCenterStructure.MeetingInfo modelMeetingInfo = new EyouSoft.Model.AdminCenterStructure.MeetingInfo();
                modelMeetingInfo.CompanyId = CurrentUserCompanyID;
                modelMeetingInfo.MetttingNo = MeetingNum;
                modelMeetingInfo.Title = MeetingSubject;
                modelMeetingInfo.Personal = JoinPersonnel;
                modelMeetingInfo.BeginDate =(MeetingTimeBegin);
                modelMeetingInfo.EndDate = (MeetingTimeEnd);
                modelMeetingInfo.Location = MeetingLocale;
                modelMeetingInfo.Remark = Remark;
                modelMeetingInfo.IssueTime = DateTime.Now;

                if (this.hidMeetingID.Value != "")            //修改
                {
                    
                    modelMeetingInfo.Id = Utils.GetInt(this.hidMeetingID.Value);
                    if (bllMeetingInfo.Update(modelMeetingInfo))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "修改成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/cahierManage/Default.aspx"));
                    }
                    else
                    {
                        MessageBox.Show(this.Page,"修改失败！");
                    }
                }
                else                           //新增
                {
                    if (bllMeetingInfo.Add(modelMeetingInfo))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "新增成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/cahierManage/Default.aspx"));
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "新增失败！");
                    }
                }
            }
        }

       
    }
}
