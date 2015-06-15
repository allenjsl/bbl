using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Function;

namespace Web.administrativeCenter.trainingPlan
{
    public partial class SeeDetail : Eyousoft.Common.Page.BackPage
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
            if (!IsPostBack)
            {
                int TrainPlanID=EyouSoft.Common.Utils.GetInt(Request.QueryString["TrainPlanID"]);
                EyouSoft.BLL.AdminCenterStructure.TrainPlan bllTrainPlan = new EyouSoft.BLL.AdminCenterStructure.TrainPlan();
                EyouSoft.Model.AdminCenterStructure.TrainPlan modelTrainPlan = bllTrainPlan.GetModel(CurrentUserCompanyID, TrainPlanID);
                if (modelTrainPlan != null)
                {
                    this.lbl_PlanTitle.Text = modelTrainPlan.PlanTitle;
                    this.lbl_PlanContent.Text = modelTrainPlan.PlanContent;
                    this.lbl_OperatorName.Text = modelTrainPlan.OperatorName;
                    this.lbl_IssueTime.Text = string.Format("{0:yyyy-MM-dd}", modelTrainPlan.IssueTime);
                    this.lbl_AcceptPersonnel.Text = GetAcceptList(modelTrainPlan.AcceptList);
                }
                else
                {
                    MessageBox.ShowAndRedirect(this.Page, "没有该数据！", "/administrativeCenter/trainingPlan/Default.aspx");
                }
            }
        }

        /// <summary>
        /// 格式化接受人数据
        /// </summary>
        private string GetAcceptList(IList<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts> listTrainPlanAccepts)
        {
            string strDepartment = "";
            string strPersonnnel = "";
            string strResult = "";
            bool all = false;
            if (listTrainPlanAccepts != null && listTrainPlanAccepts.Count > 0)
            {
                foreach (EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts tmp in listTrainPlanAccepts)
                {
                    if (tmp.AcceptType == EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.所有)
                    {
                        strResult = "所有人";
                        all = true;
                        break;
                    }
                }
                if (!all)//如果选择所有人，则部门和人员就不显示
                {
                    foreach (EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts tmp in listTrainPlanAccepts)
                    {
                        if (tmp.AcceptType == EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.指定部门)
                        {
                            strDepartment += tmp.AcceptName + ",";
                        }
                        if (tmp.AcceptType == EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.指定人)
                        {
                            strPersonnnel += tmp.AcceptName + ",";
                        }
                    }
                    if (strDepartment.Length > 1)
                    {
                        strDepartment = strDepartment.Substring(0, strDepartment.Length - 1) + "。";
                        strResult+="部门:" + strDepartment ;
                    }
                    if (strPersonnnel.Length > 1)
                    {
                        strPersonnnel = strPersonnnel.Substring(0, strPersonnnel.Length - 1) + "。";
                        strResult += "人员：" + strPersonnnel;
                    }
                }
            }
            return strResult;
        }
    }
}
