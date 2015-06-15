using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.UserCenter.UserInfo
{
    /// <summary>
    /// 页面：个人中心--工作汇报
    /// 功能：新增和修改
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class WorkReportAdd : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected string type = string.Empty;
        protected EyouSoft.Model.PersonalCenterStructure.WorkReport wrModel = new EyouSoft.Model.PersonalCenterStructure.WorkReport();
        protected int tid = 0;//主键ID
        EyouSoft.BLL.PersonalCenterStructure.WorkReport wrBll = null;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            wrBll = new EyouSoft.BLL.PersonalCenterStructure.WorkReport(SiteUserInfo);
            type = EyouSoft.Common.Utils.GetQueryStringValue("type");
            if (IsPostBack)
            {
                Save();
            }
            else
            {
                switch (type)
                {
                    case "modify":
                        Bind();
                        this.Title = "汇报修改_工作汇报_个人中心";
                        break;
                    case "check":
                        Check();
                        break;
                    default:
                        BindAdd();
                        this.Title = "汇报新增_工作汇报_个人中心";
                        break;

                }
            }
        }

        /// <summary>
        /// 新增初使化
        /// </summary>
        private void BindAdd()
        {
            wrModel.ReportingTime = DateTime.Now;
            wrModel.OperatorName = this.SiteUserInfo.ContactInfo.ContactName;
            wrModel.DepartmentName = this.SiteUserInfo.DepartName;
            wrModel.OperatorId = SiteUserInfo.ID;
        }

        /// <summary>
        /// 审核工作计划
        /// </summary>
        private void Check()
        {
            bool res = false;
            tid = Utils.GetInt(Utils.GetFormValue("tid"));
            if (tid > 0)
            {
                wrModel = wrBll.GetModel(tid);
                if (wrModel.OperatorId != SiteUserInfo.ID)
                {   
                    int sta = Utils.GetInt(Utils.GetFormValue("status"),0);
                    string comment = Utils.GetFormValue("comment");
                    res = wrBll.SetChecked(tid, (EyouSoft.Model.EnumType.PersonalCenterStructure.CheckState)sta, comment);
                }
            }
            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}",res?1:-1));
            Response.End();
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        private void Save()
        {
            tid = EyouSoft.Common.Utils.GetInt(Utils.GetFormValue("tid"));
            if (tid > 0)//修改得到实体
            {
                wrModel = wrBll.GetModel(tid);
            }
            else//新增初使化
            {
                wrModel.ReportingTime = DateTime.Now;
                wrModel.OperatorName = this.SiteUserInfo.ContactInfo.ContactName;
                wrModel.DepartmentId = SiteUserInfo.DepartId;
                wrModel.DepartmentName = this.SiteUserInfo.DepartName;
                wrModel.OperatorId = this.SiteUserInfo.ID;
                wrModel.CompanyId = this.CurrentUserCompanyID;
                wrModel.Status = EyouSoft.Model.EnumType.PersonalCenterStructure.CheckState.未审核;
                wrModel.DepartmentId = this.SiteUserInfo.DepartId;
            }

            wrModel.Title = Utils.GetFormValue("title");
            wrModel.Description = Utils.EditInputText(Request.Form["description"]);

            if (Request.Files.Count > 0)
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["upfile"], "UserCenterFile", out filepath, out oldfilename);
                if (result)
                {
                    wrModel.FilePath = filepath;
                }
            }
            bool res = false;
            if (tid > 0)
            {
                //修改
                res = wrBll.Update(wrModel);
            }
            else
            {
                //新增
                res = wrBll.Add(wrModel);
            }

            

            if (res)
            {
                string conti = Utils.GetFormValue("continue");
                if (conti == "continue")
                {
                    MessageBox.ShowAndRedirect(this, "操作成功！", "WorkReportAdd.aspx");
                }
                else
                {
                    MessageBox.ShowAndRedirect(this, "操作成功！", "WorkReport.aspx");
                }
            }
            else
            {
                MessageBox.ShowAndReturnBack(this, "操作失败!", 1);
            }
        }

        /// <summary>
        /// 修改数据绑定
        /// </summary>
        private void Bind()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"), 0);
            wrModel = wrBll.GetModel(tid);
        }
    }
}
