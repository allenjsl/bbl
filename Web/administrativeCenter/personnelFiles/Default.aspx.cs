using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EyouSoft.Common;

namespace Web.administrativeCenter.personnelFiles
{
    /// <summary>
    /// 功能：行政中心-人事档案
    /// 开发人：孙川
    /// 日期：2011-01-13
    /// </summary>
    public partial class Default : Eyousoft.Common.Page.BackPage
    {
        #region 权限参数
        protected bool InsertFlag = false;      //新增
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_人事档案_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_人事档案_栏目, true);
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_人事档案_新增档案))
                {
                    InsertFlag = true;
                }
                DutyInit();
            }
        }

        /// <summary>
        /// 初始话职务信息
        /// </summary>
        private void DutyInit() 
        {
            EyouSoft.BLL.AdminCenterStructure.DutyManager bllDuty = new EyouSoft.BLL.AdminCenterStructure.DutyManager();
            this.ddlJobPostion.Items.Clear();
            this.ddlJobPostion.Items.Add(new ListItem("--请选择--", "0"));

            IList<EyouSoft.Model.AdminCenterStructure.DutyManager> listDuty = bllDuty.GetList(CurrentUserCompanyID);
            if(listDuty!=null&&listDuty.Count!=0)
            {
                foreach (EyouSoft.Model.AdminCenterStructure.DutyManager modelDuty in listDuty)
                {
                    this.ddlJobPostion.Items.Add(new ListItem(modelDuty.JobName, modelDuty.Id.ToString()));
                }
            }
        }
    }
}
