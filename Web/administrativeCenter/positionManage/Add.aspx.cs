using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using Common.Enum;


namespace Web.administrativeCenter.positionManage
{
    /// <summary>
    /// 功能：行政中心-职务管理编辑
    /// 开发人：孙川
    /// 日期：2011-01-12
    /// </summary>
    public partial class Add : Eyousoft.Common.Page.BackPage
    {
        protected string JobName = string.Empty;    //职务名称
        protected string JobDes = string.Empty;     //职务说明
        protected string JobRequire = string.Empty; //职务要求
        protected string Remarks = string.Empty;    //备注

        protected int PositionID;                    //ID
        protected string Method = string.Empty;
        protected string ReturnUrl = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.BLL.AdminCenterStructure.DutyManager bllDuty;
            EyouSoft.Model.AdminCenterStructure.DutyManager modelDuty;

            PositionID = Utils.GetInt(Request.QueryString["PositionID"],-1);
            Method = Utils.GetFormValue("hiddenMethod");

            if (!IsPostBack)//绑定要修改的数据
            {
                if (PositionID != -1)
                {
                    if (!CheckGrant(TravelPermission.行政中心_职务管理_修改职务))
                    {
                        Utils.ResponseNoPermit(TravelPermission.行政中心_职务管理_修改职务, true);
                    }
                    this.hiddenID.Value = PositionID.ToString();
                    bllDuty = new EyouSoft.BLL.AdminCenterStructure.DutyManager();
                    modelDuty = bllDuty.GetModel(CurrentUserCompanyID, PositionID);
                    JobName = modelDuty.JobName;
                    JobDes = modelDuty.Help;
                    JobRequire = modelDuty.Requirement;
                    Remarks = modelDuty.Remark;
                }
                else
                {
                    if (!CheckGrant(TravelPermission.行政中心_职务管理_新增职务))
                    {
                        Utils.ResponseNoPermit(TravelPermission.行政中心_职务管理_新增职务, true);
                    }
                }
                
            }

            #region   "保存和修改"
            if (Method != "")
            {
                JobName = Utils.GetFormValue("txt_JobName");
                JobDes = Utils.GetFormValue("txt_JobDes");
                JobRequire = Utils.GetFormValue("txt_JobRequire");
                Remarks = Utils.GetFormValue("txt_Remarks");

                if (JobName != "")
                {
                    bllDuty = new EyouSoft.BLL.AdminCenterStructure.DutyManager();
                    modelDuty = new EyouSoft.Model.AdminCenterStructure.DutyManager();
                    modelDuty.JobName = JobName;
                    modelDuty.Help = JobDes;
                    modelDuty.Requirement = JobRequire;
                    modelDuty.Remark = Remarks;
                    modelDuty.CompanyId = CurrentUserCompanyID;

                    if (PositionID == -1 && Method == "saveandadd")//保存并继续添加
                    {
                        int result = bllDuty.Add(modelDuty);
                        if (result == 1)
                        {
                            MessageBox.ShowAndRedirect(this, "保存成功！", this.Request.Url.ToString());
                        }
                        else if (result == 0)
                        {
                            MessageBox.Show(this.Page, "添加失败！");
                        }
                        else if (result == -1)
                        {
                            MessageBox.Show(this.Page, "该职务已存在，添加失败，请确认！");
                        }
                    }
                    else if (PositionID == -1 && Method == "save")//保存
                    {
                        int result = bllDuty.Add(modelDuty);
                        if (result == 1)
                        {
                            MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "保存成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/positionManage/Default.aspx"));
                        }
                        else if (result == 0)
                        {
                            MessageBox.Show(this.Page, "添加失败！");
                        }
                        else if (result == -1)
                        {
                            MessageBox.Show(this.Page, "该职务已存在，添加失败,请确认！");
                        }
                    }
                    else if (PositionID != -1 && Method == "update")//修改
                    {
                        modelDuty.Id = PositionID;
                        int result = bllDuty.Update(modelDuty);
                        if (result==1)
                        {
                            MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "修改成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/positionManage/Default.aspx"));
                        }
                        else if(result==0)
                        {
                            MessageBox.Show(this.Page, "修改失败！");
                        }
                        else if (result == -1)
                        {
                            MessageBox.Show(this.Page, "该职务已存在，修改失败,请确认！");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this.Page, "职务名称不能为空，请确认！");
                }
            }
            #endregion
        }

        /// <summary>
        /// 弹出窗类型
        /// </summary>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        
    }
}
