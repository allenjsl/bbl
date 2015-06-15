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
using System.Collections.Generic;
using EyouSoft.Common.Function;

namespace Web.UserCenter.UserInfo
{
    /// <summary>
    /// 页面：个人中心-工作计划
    /// 功能：新增和修改
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class WorkPlanAdd : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected string type = string.Empty;
        protected EyouSoft.Model.PersonalCenterStructure.WorkPlan wpModel = new EyouSoft.Model.PersonalCenterStructure.WorkPlan();
        EyouSoft.BLL.PersonalCenterStructure.WorkPlan wpBll = null;
        EyouSoft.BLL.CompanyStructure.Department dBll = null;
        protected int tid = 0;//主键ID

        protected bool isDirector = false;//是否上级主管
        protected bool isManager = false;//是否总经理

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            wpBll = new EyouSoft.BLL.PersonalCenterStructure.WorkPlan(SiteUserInfo);
            dBll = new EyouSoft.BLL.CompanyStructure.Department();
            if (!IsPostBack)
            {
                type = EyouSoft.Common.Utils.GetQueryStringValue("type");
                switch (type)
                {
                    case "modify":
                        Bind();//修改数据初使绑定
                        this.Title = "工作计划修改_个人中心";
                        break;
                    default:
                        AddBind();//新增数据初使化
                        this.Title = "工作计划新增_个人中心";
                        break;
                }
            }
            else
            {
                Save();
            }
        }
        //权限判断
        private void GrantInit()
        {
            int perm = 0;//0普通人员 1部门主管或上级部门总管 2一级部门主管
            if (wpModel.OperatorId != SiteUserInfo.ID)
            {
                perm = dBll.JudgePermission(wpModel.OperatorId, SiteUserInfo.ID);
            }
            //是否上级主管
            isDirector = perm > 0 ? true : false;
            //是否总经理
            isManager = perm == 2 ? true : false;
        }

        /// <summary>
        /// 初使化新增数据
        /// </summary>
        private void AddBind()
        {
            wpModel.OperatorName = this.SiteUserInfo.ContactInfo.ContactName;
            wpModel.CreateTime = DateTime.Now;
            wpModel.ActualDate = DateTime.Now;
            wpModel.ExpectedDate = DateTime.Now;
            wpModel.LastTime = DateTime.MaxValue;
            wpModel.Status = EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState.进行中;
        }

        /// <summary>
        /// 保存新增修改信息
        /// </summary>
        private void Save()
        {
            bool res = false;//是否成功
            tid = EyouSoft.Common.Utils.GetInt(Utils.GetFormValue("tid"));//主键ID，是0为新增
            if (tid > 0)
            {
                wpModel = wpBll.GetModel(tid);
            }
            else
            {
                wpModel.OperatorName = this.SiteUserInfo.ContactInfo.ContactName;
                wpModel.CompanyId = this.CurrentUserCompanyID;
                wpModel.OperatorId = this.SiteUserInfo.ID;
                wpModel.CreateTime = DateTime.Now;
            }
            wpModel.PlanNO = Utils.GetFormValue("num");//计划编号
            wpModel.Title = Utils.GetFormValue("title");//计划标题
            wpModel.Description = Utils.EditInputText(Request.Form["description"]);//计划内容
            wpModel.LastTime = DateTime.Now;//最后修改时间
            //附件
            if (Request.Files.Count > 0)
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["upfile"], "UserCenterFile", out filepath, out oldfilename);
                if (result)
                {
                    wpModel.FilePath = filepath;
                }
            }
            wpModel.Remark = Utils.GetFormValue("remark");//计划说明
            wpModel.ExpectedDate = Utils.GetDateTime(Utils.GetFormValue("expectedDate"));//计划预计完成时间
            wpModel.ActualDate = Utils.GetDateTime(Utils.GetFormValue("actualDate"));//计划实际完成时间
            wpModel.Status = (EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState)int.Parse(Utils.GetFormValue("status"));//计划状态


            #region 接收人
            wpModel.AcceptList = new List<EyouSoft.Model.PersonalCenterStructure.WorkPlanAccept>();
            string[] sid = this.SOperator1.OperId.Split(',');
            //string[] snmae = this.SOperator1.OperLblName.Split(',');
            for(int i =0;i<sid.Length;i++)
            {
                EyouSoft.Model.PersonalCenterStructure.WorkPlanAccept wpActModel = new EyouSoft.Model.PersonalCenterStructure.WorkPlanAccept();
                int id = Utils.GetInt(sid[i]);
                if (id > 0)
                {
                    wpActModel.AccetpId = id;
                    //wpActModel.AccetpName = snmae[i];
                    wpModel.AcceptList.Add(wpActModel);
                }
            }
            #endregion

            #region 上级部门主管和经理部评语
            GrantInit();
            //上级部门
            if (isDirector)
            {
                wpModel.DepartmentComment = Utils.GetFormValue("departmentComment");
            }
            //总经理
            if (isManager)
            {
                wpModel.ManagerComment = Utils.GetFormValue("managerComment");
            }

            #endregion

            if (tid > 0)
            {
                res = wpBll.Update(wpModel);
            }
            else
            {
                res = wpBll.Add(wpModel);
            }

            if (res)
            {
                string conti = Utils.GetFormValue("continue");
                if (conti == "continue")
                {
                    MessageBox.ShowAndRedirect(this, "操作成功！", "WorkPlanAdd.aspx");
                }
                else
                {
                    MessageBox.ShowAndRedirect(this, "操作成功！", "WorkPlan.aspx");
                }
            }
            else
            {
                MessageBox.ShowAndReturnBack(this, "操作失败!", 1);
            }

        }


        /// <summary>
        /// 初使化修改数据
        /// </summary>
        private void Bind()
        {
            tid = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("tid"), 0);
            wpModel = wpBll.GetModel(tid);

            GrantInit();

            if (wpModel.AcceptList.Count > 0)
            {
                string[] sid = new string[wpModel.AcceptList.Count];
                string[] sname = new string[wpModel.AcceptList.Count];
                for (int i = 0; i < wpModel.AcceptList.Count; i++)
                {
                    sid[i] = wpModel.AcceptList[i].AccetpId.ToString();
                    sname[i] = wpModel.AcceptList[i].AccetpName;
                }
                this.SOperator1.OperId = string.Join(",", sid);
                this.SOperator1.OperLblName = string.Join(",", sname);
            }
        }
    }
}
