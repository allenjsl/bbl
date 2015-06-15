using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.IO;
using System.Text;
using EyouSoft.Common.Function;

namespace Web.administrativeCenter.personnelFiles
{
    /// <summary>
    /// 功能：行政中心-ajax档案管理列表
    /// 开发人：孙川
    /// 日期：2011-01-13
    /// </summary>
    public partial class AjaxDefaultInfo : Eyousoft.Common.Page.BackPage
    {
        #region 查询参数
        string FileNo = string.Empty;
        string Name = string.Empty;
        int? Sex = null;
        DateTime? BirthdayStart = null;
        DateTime? BirthdayEnd = null;
        int? WorkYearFrom = null;
        int? WorkYearTo = null;
        int? JobPostion = null;                    
        int? WorkerType = null;                   
        string WorkerState = string.Empty;                  
        string MarriageState = string.Empty;              
        #endregion

        #region 分页参数
        int PageIndex = 1;
        int PageSize = 20;
        int CurrentPage = 0;
        int RecordCount;
	    #endregion

        #region 权限参数
        protected bool EditFlag = false;        //修改
        protected bool DeleteFlag = false;      //删除
        #endregion

        IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listPersonnel = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 删除参数
            string Method = Utils.GetQueryStringValue("method");
            int PersonnelID =Utils.GetInt( Utils.GetQueryStringValue("personnelID"),-1);
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
            #endregion
           
            if (!IsPostBack && Method=="" && PersonnelID==-1)
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_人事档案_修改档案))
                {
                    EditFlag = true;
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_人事档案_删除档案))
                {
                    DeleteFlag = true;
                }
                GetOnSearchValue();
                BindData();
            }
            if (Method == "deletePersonnelInfo" && PersonnelID != -1)//删除
            {
                EyouSoft.BLL.AdminCenterStructure.PersonnelInfo bllPersonnel = new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo();
                if (bllPersonnel.Delete(CurrentUserCompanyID, PersonnelID))
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
            if (Method == "GetExcel" && PersonnelID == -1)  //  导出Excel
            {
                GetOnSearchValue();
                BindData();
                EyouSoft.BLL.AdminCenterStructure.PersonnelInfo bllPersonnel = new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo();
                EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo modelPersonnelSearch = GetPersonnelSearchInfo();

                if (RecordCount > 0)
                {
                    listPersonnel = bllPersonnel.GetList(RecordCount, 1, ref RecordCount, CurrentUserCompanyID, modelPersonnelSearch);
                    if (listPersonnel != null && listPersonnel.Count > 0)
                    {
                        ToExcel(listPersonnel);
                    }
                    else
                    {
                        ToExcel(new List<EyouSoft.Model.AdminCenterStructure.PersonnelInfo>());
                    }
                }
                else
                {
                    ToExcel(new List<EyouSoft.Model.AdminCenterStructure.PersonnelInfo>());
                }
            }
            
        }

        /// <summary>
        /// 得到查询条件
        /// </summary>
        private void GetOnSearchValue()
        {
            FileNo = Utils.GetQueryStringValue("FileNo");           //编号
            Name = Utils.GetQueryStringValue("Name");
            Sex = Utils.GetIntNull(Request.QueryString["Sex"]);
            BirthdayStart = Utils.GetDateTimeNullable(Request.QueryString["BirthdayStart"]);
            BirthdayEnd = Utils.GetDateTimeNullable(Request["BirthdayEnd"]);
            WorkYearFrom = Utils.GetIntNull(Request.QueryString["WorkYearFrom"]);       //工龄
            WorkYearTo = Utils.GetIntNull(Request.QueryString["WorkYearTo"]);       //工龄
            JobPostion = Utils.GetIntNull(Request.QueryString["JobPostion"]);                    //职务
            WorkerType = Utils.GetIntNull(Request.QueryString["WorkerType"]);                    //类型
            WorkerState = Utils.GetQueryStringValue("WorkerState");                  //员工状态
            MarriageState = Utils.GetQueryStringValue("MarriageState");              //婚姻状况
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
        }

        /// <summary>
        /// 得到查询对象
        /// </summary>
        private EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo GetPersonnelSearchInfo()
        {
            EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo modelPersonnelSearch = new EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo();
            if (FileNo != "")
                modelPersonnelSearch.ArchiveNo = FileNo;
            if (Name != "")
                modelPersonnelSearch.UserName = Name;
            if (Sex != 0)
                modelPersonnelSearch.ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Sex;
            modelPersonnelSearch.BirthDateFrom = BirthdayStart;
            modelPersonnelSearch.BirthDateTo = BirthdayEnd;
            modelPersonnelSearch.WorkYearFrom = WorkYearFrom;
            modelPersonnelSearch.WorkYearTo = WorkYearTo;
            if (JobPostion != 0)
                modelPersonnelSearch.DutyId = JobPostion;
            if (WorkerType != null && WorkerType != -1)
                modelPersonnelSearch.PersonalType = (EyouSoft.Model.EnumType.AdminCenterStructure.PersonalType)(WorkerType);
            if (WorkerState != "null")
                modelPersonnelSearch.IsLeave = Convert.ToBoolean(WorkerState);
            if (MarriageState != "null")
                modelPersonnelSearch.IsMarried = Convert.ToBoolean(MarriageState);
            return modelPersonnelSearch;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            EyouSoft.BLL.AdminCenterStructure.PersonnelInfo bllPersonnel = new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo();
            EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo modelPersonnelSearch = GetPersonnelSearchInfo();

            listPersonnel = bllPersonnel.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, modelPersonnelSearch);
            
            if (listPersonnel != null && listPersonnel.Count > 0)
            {
                this.rptData.DataSource = listPersonnel;
                this.rptData.DataBind();
                this.BindPage();
            }
            else
            {
                this.rptData.EmptyText = "<tr><td colspan=\"12\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
                this.ExporPageInfoSelect1.Visible = false;
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
            this.ExporPageInfoSelect1.AttributesEventAdd("onclick", "PDefault.LoadData(this);", 1);
            this.ExporPageInfoSelect1.AttributesEventAdd("onchange", "PDefault.LoadData(this);", 0);
        }

        /// <summary>
        /// 序号
        /// </summary>
        protected int GetCount()
        {
            return ++CurrentPage + (PageIndex - 1) * PageSize;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        private void ToExcel(IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listPersonnelInfo)
        {
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=PersonnelFile.xls");
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");//ystem.Text.Encoding.Default;
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            StringBuilder sb = new StringBuilder();

            sb.Append("<table width=\"100%\" border=\"1\" cellpadding=\"0\" cellspacing=\"1\"><tr>");
            sb.Append("<th width=\"5%\" align=\"center\" ><strong>序号</strong></th>");
            sb.Append("<th width=\"7%\" align=\"center\" ><strong>档案编号</strong></th>");
            sb.Append("<th width=\"7%\" align=\"center\" ><strong>姓名</strong></th>");
            sb.Append("<th width=\"4%\" align=\"center\" ><strong>性别</strong></th>");
            sb.Append("<th width=\"8%\" align=\"center\" ><strong>出生日期</strong></th>");
            sb.Append("<th width=\"9%\" align=\"center\" ><strong>所属部门</strong></th>");
            sb.Append("<th width=\"7%\" align=\"center\" ><strong>职务</strong></th>");
            sb.Append("<th width=\"4%\" align=\"center\" ><strong>工龄</strong></th>");
            sb.Append("<th width=\"11%\" align=\"center\" ><strong>联系电话</strong></th>");
            sb.Append("<th width=\"10%\" align=\"center\" ><strong>手机</strong></th>");
            sb.Append("<th width=\"13%\" align=\"center\" ><strong>E-mail</strong></th>");
            int i = 1;
            foreach (EyouSoft.Model.AdminCenterStructure.PersonnelInfo modelPersonnel in listPersonnelInfo)
            {
                sb.Append("<tr><td  align=\"center\" >" + i++ + "</td>");
                sb.Append("<td align=\"center\"  >" + modelPersonnel.ArchiveNo + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.UserName + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.ContactSex.ToString() + "</td>");
                sb.Append("<td align=\"center\" >" + string.Format("{0:yyyy-MM-dd}", modelPersonnel.BirthDate) + "</td>");
                sb.Append("<td align=\"center\" >" + GetDepartmentByID(modelPersonnel.DepartmentId) + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.DutyName + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.WorkYear + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.ContactTel + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.ContactMobile + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.Email + "</td></tr>");
            }
            if (listPersonnelInfo.Count == 0)
            {
                sb.Append("<tr><td  align=\"center\" colspan=\"11\">没有相关的数据！</td></tr>");
            }
            sb.Append("</table>");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }


        /// <summary>
        /// 得到部门名称
        /// </summary>
        private string GetDepartmentByID(object DepartmentId)
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
        /// 格式化部门名称
        /// </summary>
        protected string GetDepartmentByList(object DepartmentList)
        {
            IList<EyouSoft.Model.CompanyStructure.Department> lists = null;
            string result = "";
            if (DepartmentList != null)
            {
                lists = (List<EyouSoft.Model.CompanyStructure.Department>)DepartmentList;
            }
            if (lists != null && lists.Count > 0)
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    result += lists[i].DepartName + ",";
                }
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        /// <summary>
        /// 得到工龄
        /// </summary>
        protected string GetWorkYear(DateTime? EntryDate)
        {
            string returnStr="";
            if (EntryDate != null)
            {
                double result = (DateTime.Now - EntryDate.Value).TotalDays / 365;
                returnStr = (Math.Round(Utils.GetDecimal(result.ToString()) * 10) / 10).ToString();
            }
            return returnStr;
        }
    }
}
