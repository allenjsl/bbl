using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.administrativeCenter.addressList
{
    /// <summary>
    /// 功能：行政中心-通讯录查询
    /// 开发人：孙川
    /// 日期：2011-01-20
    /// </summary>
    public partial class AjaxDefault : Eyousoft.Common.Page.BackPage
    {
        #region 查询参数
        protected string WorkerName = string.Empty;
        protected int? Department;
        #endregion
        
        #region 分页参数
        int PageIndex = 1;
        int PageSize = 20;
        int CurrentPage = 0;
        int RecordCount;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string method=Utils.GetQueryStringValue("Method");

            if (!IsPostBack && method == "")
            {
                WorkerName = Utils.GetQueryStringValue("WorkerName");
                Department = Utils.GetIntNull(Request.QueryString["Department"]);
                PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
                BindData();
            }
            if (method == "GetExcel")
            {
                EyouSoft.BLL.AdminCenterStructure.PersonnelInfo bllAddress = new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo();
                EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo modelAddressSearch = new EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo();
                modelAddressSearch.UserName = WorkerName;
                IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listAddress = bllAddress.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, modelAddressSearch);

                if (RecordCount > 0)
                {
                    listAddress = bllAddress.GetList(RecordCount, 1, ref RecordCount, CurrentUserCompanyID, modelAddressSearch);
                    if (listAddress != null && listAddress.Count > 0)
                    {
                        ToExcel(listAddress);
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
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            EyouSoft.BLL.AdminCenterStructure.PersonnelInfo bllAddress = new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo();
            IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listAddress = bllAddress.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, WorkerName, Department);
            if (listAddress != null && listAddress.Count > 0)
            {
                this.crptAddressList.DataSource = listAddress;
                this.crptAddressList.DataBind();
                this.BindPage();
            }
            else
            {
                this.crptAddressList.EmptyText = "<tr><td colspan=\"8\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
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

        /// <summary>
        /// 导出Excel
        /// </summary>
        private void ToExcel(IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> listPersonnelInfo)
        {
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=PersonnelFile.xls");
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            StringBuilder sb = new StringBuilder();
            sb.Append("<table width=\"100%\" border=\"1\" cellpadding=\"0\" cellspacing=\"1\"><tr>");
            sb.Append("<th width=\"7%\" align=\"center\" ><strong>序号</strong></th>");
            sb.Append("<th width=\"7%\" align=\"center\" ><strong>姓名</strong></th>");
            sb.Append("<th width=\"12%\" align=\"center\" ><strong>部门</strong></th>");
            sb.Append("<th width=\"11%\" align=\"center\" ><strong>电话</strong></th>");
            sb.Append("<th width=\"10%\" align=\"center\" ><strong>手机</strong></th>");
            sb.Append("<th width=\"13%\" align=\"center\" ><strong>E-mail</strong></th>");
            sb.Append("<th width=\"8%\" align=\"center\" >QQ</th>");
            sb.Append("<th width=\"14%\" align=\"center\" >MSN</th></tr>");
            int i = 0;
            foreach (EyouSoft.Model.AdminCenterStructure.PersonnelInfo modelPersonnel in listPersonnelInfo)
            {
                sb.Append("<tr><td  align=\"center\" >" + (++i) + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.UserName + "</td>");
                sb.Append("<td align=\"center\" >" + GetDepartmentString(modelPersonnel.DepartmentList) + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.ContactTel + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.ContactMobile + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.Email + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.QQ + "</td>");
                sb.Append("<td align=\"center\" >" + modelPersonnel.MSN + "</td></tr>");
            }
            if (listPersonnelInfo.Count == 0)
            {
                sb.Append("<tr><td  align=\"center\" colspan=\"8\">没有相关的数据！</td></tr>");
            }
            sb.Append("</table>");

            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 格式化所属部门
        /// </summary>
        protected string GetDepartmentString(object o)
        {
            IList<EyouSoft.Model.CompanyStructure.Department> listDepartment = (IList<EyouSoft.Model.CompanyStructure.Department>)(o);
            string listDepartmentToStr = string.Empty;
            if (listDepartment != null && listDepartment.Count > 0)
            {
                for (int i = 0; i < listDepartment.Count; i++)
                {
                    listDepartmentToStr += listDepartment[i].DepartName + ",";
                }
                listDepartmentToStr = listDepartmentToStr.Substring(0, listDepartmentToStr.Length - 1);
            }
            return listDepartmentToStr;
        }
    }
}
