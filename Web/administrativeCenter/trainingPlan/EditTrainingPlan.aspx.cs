using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.administrativeCenter.trainingPlan
{
    public partial class EditTrainingPlan : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 弹出窗类型
        /// </summary>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        #region 页面参数
        protected string PlanTitle = string.Empty;          
        protected string PlanContent = string.Empty;

        #region 发送对象
        protected bool ChbAllPersonnel = false;
        protected bool ChbDepartment = false;
        protected bool ChbPersonnel = false;
        #endregion
        
        protected string Publisher = string.Empty;
        protected DateTime? PublishTime = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int TrainPlanID = Utils.GetInt(Request.QueryString["TrainPlanID"], -1);
            string Method = Utils.GetFormValue("hidMethod");
            
            this.selectDepartment1.SetPicture = "/images/sanping_04.gif";
            Publisher =this.SiteUserInfo.UserName;              //默认当前帐号，可以修改
            PublishTime = DateTime.Now;                         //默认当前时间

            if (!IsPostBack && TrainPlanID != -1)   //初始化
            {
                EyouSoft.BLL.AdminCenterStructure.TrainPlan bllTrainPlan = new EyouSoft.BLL.AdminCenterStructure.TrainPlan();
                EyouSoft.Model.AdminCenterStructure.TrainPlan modelTrainPlan = bllTrainPlan.GetModel(CurrentUserCompanyID, TrainPlanID);
                if (modelTrainPlan != null)
                {
                    this.hidTrainPlanID.Value = modelTrainPlan.Id.ToString();
                    PlanTitle = modelTrainPlan.PlanTitle;
                    PlanContent = modelTrainPlan.PlanContent;
                    AcceptListInit(modelTrainPlan.AcceptList);
                    Publisher = modelTrainPlan.OperatorName;
                    PublishTime = modelTrainPlan.IssueTime;
                }
                else
                {
                    MessageBox.ShowAndRedirect(this.Page, "没有该条数据", "/administrativeCenter/trainingPlan/Default.aspx");
                }
            }

            if (Method == "save")
            {
                if (TrainPlanID == -1)
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_培训计划_新增计划))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_培训计划_新增计划, true);
                    }
                }
                else
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_培训计划_修改计划))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_培训计划_修改计划, true);
                    }
                }
                EyouSoft.BLL.AdminCenterStructure.TrainPlan bllTrainPlan = new EyouSoft.BLL.AdminCenterStructure.TrainPlan();
                EyouSoft.Model.AdminCenterStructure.TrainPlan ModelTrainPlan = new EyouSoft.Model.AdminCenterStructure.TrainPlan();
                ModelTrainPlan.CompanyId = CurrentUserCompanyID;
                ModelTrainPlan.PlanTitle = Utils.GetFormValue("txt_PlanTitle");
                ModelTrainPlan.PlanContent = Utils.EditInputText(Request.Form["txt_PlanContent"]);
                ModelTrainPlan.AcceptList = this.GetListTrainPlanAccepts();
                ModelTrainPlan.IssueTime = Utils.GetDateTimeNullable(Request.Form["txt_PublishTime"]);
                ModelTrainPlan.OperatorName = Utils.GetFormValue("txt_Publisher");
                if (ModelTrainPlan.OperatorName == this.SiteUserInfo.UserName)      
                {
                    ModelTrainPlan.OperatorId = this.SiteUserInfo.ID;
                }

                if (TrainPlanID == -1)         //添加
                {
                    if (bllTrainPlan.Add(ModelTrainPlan))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "保存成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/trainingPlan/Default.aspx"));
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "保存失败！");
                    }
                }

                if (TrainPlanID != -1)          //修改
                {
                    ModelTrainPlan.Id = TrainPlanID;
                    if (bllTrainPlan.Update(ModelTrainPlan))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "修改成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/trainingPlan/Default.aspx"));
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "修改失败！");
                    }
                }
            }
        }

        /// <summary>
        /// 初始话发送对象的值
        /// </summary>
        private void AcceptListInit(IList<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts> listAccepts)
        {
            string DepartmentIDs = string.Empty;
            string DepartmentNames = string.Empty;
            string PersonnelIDs = string.Empty;
            string PersonnelNames = string.Empty;
            if (listAccepts != null && listAccepts.Count > 0)
            {
                foreach (EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts tmpTrainPlan in listAccepts)
                {
                    if (tmpTrainPlan.AcceptType == EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.所有)
                    {
                        ChbAllPersonnel = true;
                    }
                    else if (tmpTrainPlan.AcceptType == EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.指定部门)
                    {
                        if (!ChbDepartment)
                        {
                            ChbDepartment = true;
                        }
                        DepartmentIDs += tmpTrainPlan.AcceptId + ",";
                        DepartmentNames += tmpTrainPlan.AcceptName + ",";
                    }
                    else if (tmpTrainPlan.AcceptType == EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.指定人)
                    {
                        if (!ChbPersonnel)
                        {
                            ChbPersonnel = true;
                        }
                        PersonnelIDs += tmpTrainPlan.AcceptId + ",";
                        PersonnelNames += tmpTrainPlan.AcceptName + ",";
                    }
                }
                if (DepartmentIDs.Length > 1)
                {
                    DepartmentIDs = DepartmentIDs.Substring(0, DepartmentIDs.Length - 1);
                    DepartmentNames = DepartmentNames.Substring(0, DepartmentNames.Length - 1);
                    this.selectDepartment1.GetDepartId = DepartmentIDs;
                    this.selectDepartment1.GetDepartment_lblName = DepartmentNames;
                }
                if (PersonnelIDs.Length > 1)
                {
                    PersonnelIDs = PersonnelIDs.Substring(0, PersonnelIDs.Length - 1);
                    PersonnelNames = PersonnelNames.Substring(0, PersonnelNames.Length - 1);
                    this.selectOperator1.OperId = PersonnelIDs;
                    this.selectOperator1.OperLblName = PersonnelNames;
                }
            }
        }

        /// <summary>
        /// 得到发送对象
        /// </summary>
        private IList<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts> GetListTrainPlanAccepts()
        {
            IList<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts> listTrainPlanAccepts = new List<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts>();
            EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts modelTrainPlanAccepts = null;
            string allPersonnel = Utils.GetFormValue("chb_All");
            string departmentIDs = this.selectDepartment1.GetDepartId;
            string personnelIDs = this.selectOperator1.OperId;
            string[] arrDepartment = null;
            string[] arrPersonnel = null;
            if (departmentIDs.Length > 0)
            {
                arrDepartment = departmentIDs.Split(',');
            }
            if (personnelIDs.Length > 0)
            {
                arrPersonnel = personnelIDs.Split(',');
            }
            if (allPersonnel == "0")
            {
                modelTrainPlanAccepts = new EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts();
                modelTrainPlanAccepts.AcceptId = 0;
                modelTrainPlanAccepts.AcceptType = (EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType)Utils.GetInt(allPersonnel);
                listTrainPlanAccepts.Add(modelTrainPlanAccepts);
            }

            if (arrDepartment != null && arrDepartment.Length > 0)
            {
                for (int i = 0; i < arrDepartment.Length; i++)
                {
                    modelTrainPlanAccepts = new EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts();
                    modelTrainPlanAccepts.AcceptId = Utils.GetInt(arrDepartment[i]);
                    modelTrainPlanAccepts.AcceptType = EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.指定部门;
                    listTrainPlanAccepts.Add(modelTrainPlanAccepts);
                }
            }
            if (arrPersonnel != null && arrPersonnel.Length > 0)
            {
                for (int i = 0; i < arrPersonnel.Length; i++)
                {
                    modelTrainPlanAccepts = new EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts();
                    modelTrainPlanAccepts.AcceptId = Utils.GetInt(arrPersonnel[i]);
                    modelTrainPlanAccepts.AcceptType = EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.指定人;
                    listTrainPlanAccepts.Add(modelTrainPlanAccepts);
                }
            }
            return listTrainPlanAccepts;
        }
    }
}
