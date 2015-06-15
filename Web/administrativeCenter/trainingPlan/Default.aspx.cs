using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.administrativeCenter.trainingPlan
{

    public partial class Default : Eyousoft.Common.Page.BackPage
    {
        #region 分页参数
        int PageIndex = 1;
        int PageSize = 20;
        int CurrentPage = 0;
        int RecordCount;
        #endregion

        #region 权限参数
        protected bool EditFlag = false;        //修改
        protected bool DeleteFlag = false;      //删除
        protected bool InsertFlag = false;      //新增
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int TrainPlanID = Utils.GetInt(Request.QueryString["TrainPlanID"], -1);
            string method = Utils.GetQueryStringValue("method");
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);

            if (!IsPostBack && method == "")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_培训计划_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_培训计划_栏目, true);
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_培训计划_新增计划))
                {
                    InsertFlag = true;
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_培训计划_修改计划))
                {
                    EditFlag = true;
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_培训计划_删除计划))
                {
                    DeleteFlag = true;
                }
                BindData();
            }
            if (method == "DeleteTrainPlan" && TrainPlanID != -1)   //删除
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_培训计划_删除计划))
                {
                    Response.Clear();
                    Response.Write("NoPermission");
                    Response.End();
                }
                else
                {
                    EyouSoft.BLL.AdminCenterStructure.TrainPlan bllTrainPlan = new EyouSoft.BLL.AdminCenterStructure.TrainPlan();
                    if (bllTrainPlan.Delete(CurrentUserCompanyID, TrainPlanID))
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
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            EyouSoft.BLL.AdminCenterStructure.TrainPlan bllTrainPlan = new EyouSoft.BLL.AdminCenterStructure.TrainPlan();
            IList<EyouSoft.Model.AdminCenterStructure.TrainPlan> listTrainPlan = bllTrainPlan.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, SiteUserInfo.ID, SiteUserInfo.DepartId);
            if (listTrainPlan != null && listTrainPlan.Count > 0)
            {
                this.crptTrainingPlanList.DataSource = listTrainPlan;
                this.crptTrainingPlanList.DataBind();
                this.BindPage();
            }
            else
            {
                this.crptTrainingPlanList.EmptyText = "<tr><td colspan=\"6\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
                this.ExportPageInfo1.Visible = false;
            }
        }

        /// <summary>
        /// 设置分页控件参数
        /// </summary>
        private void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intRecordCount = RecordCount;
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
