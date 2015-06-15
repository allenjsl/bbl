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
    public partial class EditContract : Eyousoft.Common.Page.BackPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }

        #region 页面参数
        protected string WorkerNO = string.Empty;
        protected string WorkerName = string.Empty;
        protected DateTime BeginDate;
        protected DateTime EndDate;
        protected string Reamrk = string.Empty;
        #endregion  

        protected void Page_Load(object sender, EventArgs e)
        {
            int WorkerID = Utils.GetInt(Request.QueryString["WorkerID"],-1);
            string Method = Utils.GetFormValue("hidMethod");
            if (!IsPostBack && WorkerID != -1 )//初始化
            { 
                EyouSoft.BLL.AdminCenterStructure.ContractInfo bllContractInfo = new EyouSoft.BLL.AdminCenterStructure.ContractInfo();
                EyouSoft.Model.AdminCenterStructure.ContractInfo modelContractInfo = bllContractInfo.GetModel(CurrentUserCompanyID, WorkerID);
                if (modelContractInfo != null)
                {
                    this.hidWorkerID.Value = modelContractInfo.Id.ToString();
                    this.dpState.SelectedIndex =(int)modelContractInfo.ContractStatus;
                    WorkerNO = modelContractInfo.StaffNo;
                    WorkerName = modelContractInfo.StaffName;
                    BeginDate = modelContractInfo.BeginDate;
                    EndDate = modelContractInfo.EndDate;
                    Reamrk = modelContractInfo.Remark;
                }
            }
            if (Method == "save")
            {
                if (Utils.GetInt(this.hidWorkerID.Value, -1) != -1)     //  修改权限
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_修改合同))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_修改合同, true);
                    }
                }
                else
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_新增合同))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_新增合同, true);
                    }
                }
                EyouSoft.BLL.AdminCenterStructure.ContractInfo bllContractInfo = new EyouSoft.BLL.AdminCenterStructure.ContractInfo();
                EyouSoft.Model.AdminCenterStructure.ContractInfo modelContractInfo = new EyouSoft.Model.AdminCenterStructure.ContractInfo();
                modelContractInfo.CompanyId = CurrentUserCompanyID;
                modelContractInfo.StaffNo = Utils.GetFormValue("txt_WorkerNO");
                modelContractInfo.StaffName = Utils.GetFormValue("txt_WorkerName");
                if (Utils.GetDateTime(Request.Form["txt_BeginDate"]) != null)
                {
                    modelContractInfo.BeginDate = Utils.GetDateTime(Request.Form["txt_BeginDate"]);
                }
                if (Utils.GetDateTime(Request.Form["txt_EndDate"]) != null)
                {
                    modelContractInfo.EndDate = Utils.GetDateTime(Request.Form["txt_EndDate"]);
                }
                modelContractInfo.Remark = Utils.GetFormValue("txt_Reamrk");
                modelContractInfo.ContractStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.ContractStatus)this.dpState.SelectedIndex;
                if (Utils.GetInt(this.hidWorkerID.Value, -1) != -1)     //  修改
                {
                    modelContractInfo.Id = Utils.GetInt(this.hidWorkerID.Value);
                    if (bllContractInfo.Update(modelContractInfo))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "修改成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/contractManage/Default.aspx"));
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "修改失败！");
                    }
                }
                else                            //新增
                {
                    if (bllContractInfo.Add(modelContractInfo))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "新增成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/contractManage/Default.aspx"));
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
