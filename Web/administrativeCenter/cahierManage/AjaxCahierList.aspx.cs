using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.administrativeCenter.cahierManage
{

    public partial class AjaxCahierList : Eyousoft.Common.Page.BackPage
    {

        #region 分页参数
        int PageIndex = 1;
        int PageSize = 20;
        int CurrentPage = 0;
        int RecordCount;
        #endregion

        #region 权限参数
        protected bool EditFlag = false;
        protected bool DeleteFlag = false;
        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = Utils.GetQueryStringValue("Method");
            int MeetingID = Utils.GetInt(Request.QueryString["MeetingID"], -1);
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);

            if (!IsPostBack && method == "")
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_会议记录_删除记录))
                {
                    DeleteFlag = true;
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_会议记录_修改记录))
                {
                    EditFlag = true;
                }
                string MeetingNum = Utils.GetQueryStringValue("MeetingNum");
                string MeetingTheme = Utils.GetQueryStringValue("MeetingTheme");
                DateTime? MeetingDateBegin = Utils.GetDateTimeNullable(Request.QueryString["MeetingDateBegin"]);
                DateTime? MeetingDateEnd = Utils.GetDateTimeNullable(Request.QueryString["MeetingDateEnd"]);
                EyouSoft.BLL.AdminCenterStructure.MeetingInfo bllMeetingInfo = new EyouSoft.BLL.AdminCenterStructure.MeetingInfo();
                IList<EyouSoft.Model.AdminCenterStructure.MeetingInfo> listMeetingInfo = bllMeetingInfo.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, MeetingNum,MeetingTheme, MeetingDateBegin, MeetingDateEnd);
                if (listMeetingInfo != null && listMeetingInfo.Count > 0)
                {
                    this.crptCahierList.DataSource = listMeetingInfo;
                    this.crptCahierList.DataBind();
                    this.BindPage();
                }
                else
                {
                    this.crptCahierList.EmptyText = "<tr><td colspan=\"8\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
                    this.ExporPageInfoSelect1.Visible = false;
                }
            }

            if (method == "DeleteMeeting" && MeetingID != -1)//删除
            {
                EyouSoft.BLL.AdminCenterStructure.MeetingInfo bllMeetingInfo = new EyouSoft.BLL.AdminCenterStructure.MeetingInfo();
                if (bllMeetingInfo.Delete(CurrentUserCompanyID, MeetingID))
                {
                    Response.Clear();
                    Response.Write("True");
                    Response.End();
                }
                else
                {
                    Response.Clear();
                    Response.Write("False");
                    Response.End();
                }
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
            this.ExporPageInfoSelect1.AttributesEventAdd("onclick", "Default.LoadData(this);", 1);
            this.ExporPageInfoSelect1.AttributesEventAdd("onchange", "Default.LoadData(this);", 0);
        }

        /// <summary>
        /// 序号
        /// </summary>
        protected int GetCount()
        {
            return ++CurrentPage + (PageIndex - 1) * PageSize;
        }

        /// <summary>
        /// 格式话时间
        /// </summary>
        public string GetFormartDate(string startDate, string endDate)
        {
            string result = "";
            if (startDate != "1900-01-01")
            {
                result += startDate;
            }
            if (startDate != "1900-01-01" && endDate != "1900-01-01")
            {
                result += "至";
            }
            if (endDate != "1900-01-01")
            {
                result += endDate;
            }
            return result;
        }
    }
}
