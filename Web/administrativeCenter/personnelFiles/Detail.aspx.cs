using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.administrativeCenter.personnelFiles
{
    /// <summary>
    /// 功能：行政中心-人事档案详细查看
    /// 开发人：孙川
    /// 日期：2011-01-17
    /// </summary>
    public partial class Detail : Eyousoft.Common.Page.BackPage
    {
        protected string Birthplace = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            int personnelID = Utils.GetInt(Utils.GetQueryStringValue("personnelID"),-1);
            //绑定
            if (!IsPostBack && personnelID != -1)
            {
                EyouSoft.BLL.AdminCenterStructure.PersonnelInfo bllPersonnel = new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo();
                EyouSoft.Model.AdminCenterStructure.PersonnelInfo modelPersonnel = bllPersonnel.GetModel(CurrentUserCompanyID, personnelID);
                this.lblPersonnelTitle.Text = modelPersonnel.UserName;
                this.lblFileNo.Text = modelPersonnel.ArchiveNo;
                this.lblName.Text = modelPersonnel.UserName;
                this.lblSex.Text = modelPersonnel.ContactSex.ToString();
                this.imgPersonnelPhoto.ImageUrl = modelPersonnel.PhotoPath;
                this.lblCardID.Text = modelPersonnel.CardId;
                this.lblBirthday.Text = modelPersonnel.BirthDate == null ? "" : string.Format("{0:yyyy-MM-dd}",modelPersonnel.BirthDate);
                this.lblDepartment.Text = GetDepartmentByID(modelPersonnel.DepartmentId);          //  所属部门
                this.lblPosition.Text = GetDutyNameByID(modelPersonnel.DutyId);
                this.lblType.Text = modelPersonnel.PersonalType.ToString();
                this.lblState.Text = modelPersonnel.IsLeave ? "离职" : "在职";
                this.lblEntryDate.Text = string.Format("{0:yyyy-MM-dd}", modelPersonnel.EntryDate) ;
                this.lblWorkerLife.Text = GetWorkYear(modelPersonnel.EntryDate) + "年";
                this.lblNational.Text = modelPersonnel.National;
                Birthplace =GetProvinceAndCity(modelPersonnel.Birthplace);
                this.lblPolitical.Text = modelPersonnel.Politic;
                this.lblMarriageState.Text = modelPersonnel.IsMarried ? "已婚" : "未婚";
                this.lblTel.Text = modelPersonnel.ContactTel;
                this.lblMobile.Text = modelPersonnel.ContactMobile;
                this.lblLeftDate.Text = modelPersonnel.LeaveDate == null ? "" : string.Format("{0:yyyy-MM-dd}",modelPersonnel.LeaveDate);
                this.lblQQ.Text = modelPersonnel.QQ;
                this.lblMSN.Text = modelPersonnel.MSN;
                this.lblEmail.Text = modelPersonnel.Email;
                this.lblAddress.Text = modelPersonnel.ContactAddress;
                this.lblRemark.Text = modelPersonnel.Remark;
                this.rptRecord.DataSource = modelPersonnel.HistoryList;
                this.rptRecord.DataBind();
                this.rptEducationInfo.DataSource = modelPersonnel.SchoolList;
                this.rptEducationInfo.DataBind();
            }
        }


        /// <summary>
        /// 得到部门名称
        /// </summary>
        public string GetDepartmentByID(object DepartmentId)
        {
            string result = "";
            if (DepartmentId != null && DepartmentId.ToString() != "")
            {
                string[] ids = DepartmentId.ToString().Split(',');
                EyouSoft.BLL.CompanyStructure.Department bllDepartment = new EyouSoft.BLL.CompanyStructure.Department();
                if (ids != null && ids.Length > 0)
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        result += bllDepartment.GetModel(Utils.GetInt(ids[i])).DepartName + ",";
                    }
                    result = result.Substring(0, result.Length - 1);
                }
            }
            return result;
        }

        /// <summary>
        /// 得到籍贯
        /// </summary>
        public string GetProvinceAndCity(string Birthplace)
        {
            string returnStr = "";
            string[] arrBirthplace = Birthplace.Split(',');
            if (arrBirthplace != null && arrBirthplace.Length > 1)
            {
                EyouSoft.BLL.CompanyStructure.City bllCity = new EyouSoft.BLL.CompanyStructure.City();
                EyouSoft.BLL.CompanyStructure.Province bllProvince = new EyouSoft.BLL.CompanyStructure.Province();
                if (arrBirthplace[0] != "")
                {
                    EyouSoft.Model.CompanyStructure.Province modelProvince = bllProvince.GetModel(Utils.GetInt(arrBirthplace[0]));
                    if (modelProvince != null)
                    {
                        returnStr += modelProvince.ProvinceName + "省";
                    }
                }
                if (arrBirthplace[1] != "")
                {
                    EyouSoft.Model.CompanyStructure.City modelCity = bllCity.GetModel(Utils.GetInt(arrBirthplace[1]));
                    if (modelCity != null)
                    {
                        returnStr += modelCity.CityName + "市";
                    }
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 得到工龄
        /// </summary>
        protected string GetWorkYear(DateTime? EntryDate)
        {
            string returnStr = "";
            if (EntryDate != null)
            {
                double result = (DateTime.Now - EntryDate.Value).TotalDays / 365;
                returnStr = (Math.Round(Utils.GetDecimal(result.ToString()) * 10) / 10).ToString();
            }
            return returnStr;
        }

        /// <summary>
        /// 得到职务名称
        /// </summary>
        protected string GetDutyNameByID(int? dutyID)
        {
            string strReturn = "";
            int id = 0;
            if (dutyID != null)
            {
                id = (int)dutyID;
            }
            EyouSoft.BLL.AdminCenterStructure.DutyManager bllDutyManager = new EyouSoft.BLL.AdminCenterStructure.DutyManager();
            EyouSoft.Model.AdminCenterStructure.DutyManager modelDutyManager = bllDutyManager.GetModel(CurrentUserCompanyID, id);
            if (modelDutyManager != null)
            {
                strReturn = modelDutyManager.JobName;
            }
            return strReturn;
        }
    }
}
