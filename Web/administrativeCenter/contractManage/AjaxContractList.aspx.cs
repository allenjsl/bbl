using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.administrativeCenter.contractManage
{
    public partial class AjaxContractList : Eyousoft.Common.Page.BackPage
    {
        #region 分页参数
        int PageIndex = 1;
        int PageSize = 20;
        int CurrentPage = 0;
        int RecordCount;
        #endregion

        #region 删除权限
        protected bool EditFlag = false;
        protected bool DeleteFlag = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
            string method = Utils.GetQueryStringValue("Method");
            int WorkerID = Utils.GetInt(Request.QueryString["WorkerID"],-1);
            

            if (!IsPostBack && method=="")          //查询
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_修改合同))
                {
                    EditFlag = true;
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_删除合同))
                {
                    DeleteFlag = true;
                }
                string WorkerNo = Utils.GetQueryStringValue("WorkerNo");
                string WorkerName = Utils.GetQueryStringValue("WorkerName");
                DateTime? TimeOfSigningStart = Utils.GetDateTimeNullable(Request.QueryString["TimeOfSigningStart"]);
                DateTime? TimeOfSigningEnd = Utils.GetDateTimeNullable(Request.QueryString["TimeOfSigningEnd"]);
                DateTime? ComeDueStart = Utils.GetDateTimeNullable(Request.QueryString["ComeDueStart"]);
                DateTime? ComeDueEnd = Utils.GetDateTimeNullable(Request.QueryString["ComeDueEnd"]);

                EyouSoft.BLL.AdminCenterStructure.ContractInfo bllContractInfo = new EyouSoft.BLL.AdminCenterStructure.ContractInfo();
                EyouSoft.Model.AdminCenterStructure.ContractSearchInfo searchInfo = new EyouSoft.Model.AdminCenterStructure.ContractSearchInfo();
                searchInfo.StaffNo = WorkerNo;
                searchInfo.StaffName = WorkerName;
                searchInfo.BeginFrom = TimeOfSigningStart;
                searchInfo.BeginTo = TimeOfSigningEnd;
                searchInfo.EndFrom = ComeDueStart;
                searchInfo.EndTo = ComeDueEnd;
                IList<EyouSoft.Model.AdminCenterStructure.ContractInfo> listContractInfo = bllContractInfo.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, searchInfo);
                if (listContractInfo != null && listContractInfo.Count > 0)
                {
                    this.crptContractList.DataSource = listContractInfo;
                    this.crptContractList.DataBind();
                    BindPage();
                }
                else
                {
                    this.crptContractList.EmptyText = "<tr><td colspan=\"8\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
                    this.ExporPageInfoSelect1.Visible = false;
                }
            }
            if (method == "DeleteContract" && WorkerID != -1)//删除
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_删除合同))//删除权限判断
                {
                    Response.Clear();
                    Response.Write("NoPermission");
                    Response.End();
                }
                else
                {
                    EyouSoft.BLL.AdminCenterStructure.ContractInfo bllContractInfo = new EyouSoft.BLL.AdminCenterStructure.ContractInfo();
                    if (bllContractInfo.Delete(CurrentUserCompanyID, WorkerID))
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
    }
}
