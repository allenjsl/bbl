using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.administrativeCenter.bylaw
{
    /// <summary>
    /// 功能：行政中心-规章制度 新增和修改
    /// 开发人：孙川
    /// 日期：2011-01-20
    /// </summary>
    public partial class EditBylaw : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 弹出窗类型
        /// </summary>

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            file.Visible = false;
        }

        #region 页面变量
        protected string Number = string.Empty;
        protected string RegentTitle = string.Empty;
        protected string RegentContent = string.Empty;
        protected string FileHref = string.Empty;
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            int dutyID = Utils.GetInt(Request.QueryString["DutyID"], -1);    //制度ID
            string method = Utils.GetFormValue("hidMethod");
            if (!IsPostBack && method == "")
            {
                this.hidDutyID.Value = dutyID.ToString();
                if (dutyID != -1)
                {
                    EyouSoft.BLL.AdminCenterStructure.RuleInfo bllRuleInfo = new EyouSoft.BLL.AdminCenterStructure.RuleInfo();
                    EyouSoft.Model.AdminCenterStructure.RuleInfo modelRuleInfo = bllRuleInfo.GetModel(CurrentUserCompanyID, dutyID);
                    if (modelRuleInfo != null)
                    {
                        Number = modelRuleInfo.RoleNo;
                        RegentTitle = modelRuleInfo.Title;
                        this.txt_RegentContent.Value = modelRuleInfo.RoleContent;

                        if (modelRuleInfo.FilePath != "")
                        {
                            FileHref = modelRuleInfo.FilePath;
                            this.hidFileValue.Value = modelRuleInfo.FilePath;
                            file.Visible = true;
                        }
                        else
                        {
                            file.Visible = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "没有该规章制度信息");
                        return;
                    }
                }
            }

            if (method == "save")       //保存
            {
                Number = Utils.GetFormValue("txt_Number");
                RegentTitle = Utils.GetFormValue("txt_RegentTitle");
                RegentContent = Utils.EditInputText(Request.Form["txt_RegentContent"]);
                if (RegentTitle == "")
                {
                    MessageBox.Show(this.Page, "制度标题不能为空！");
                    return;
                }

                HttpPostedFile hpf = this.Request.Files["file_Bylaw"];
                string oldeFile = string.Empty;
                string fileName = string.Empty;
                if (hpf != null && hpf.ContentLength > 0)
                {
                    if (UploadFile.FileUpLoad(hpf, "AdminCenterRuleInfo", out fileName, out oldeFile))
                    {
                        FileHref = fileName;
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "附加上传失败!");
                        return;
                    }
                }
                else
                {
                    FileHref = this.hidFileValue.Value;
                }
                EyouSoft.Model.AdminCenterStructure.RuleInfo modelRuleInfo = new EyouSoft.Model.AdminCenterStructure.RuleInfo();
                EyouSoft.BLL.AdminCenterStructure.RuleInfo bllRuleInfo = new EyouSoft.BLL.AdminCenterStructure.RuleInfo();
                modelRuleInfo.RoleNo = Number;
                modelRuleInfo.Title = RegentTitle;
                modelRuleInfo.RoleContent = RegentContent;
                modelRuleInfo.FilePath = FileHref;
                modelRuleInfo.CompanyId = CurrentUserCompanyID;

                if (dutyID == -1)
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_规章制度_新增制度))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_规章制度_新增制度, true);
                    }
                    if (bllRuleInfo.Add(modelRuleInfo))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "保存成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/bylaw/Default.aspx"));
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "保存失败！");
                    }
                }
                else if (dutyID != -1)
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_规章制度_修改制度))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_规章制度_修改制度, true);
                    }
                    modelRuleInfo.Id = dutyID;
                    if (bllRuleInfo.Update(modelRuleInfo))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "修改成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/bylaw/Default.aspx"));
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "修改失败！");
                    }
                }

            }
        }
    }
}
