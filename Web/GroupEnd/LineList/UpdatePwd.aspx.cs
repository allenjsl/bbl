using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.GroupEnd.LineList
{
    /// <summary>
    /// 页面：修改密码
    /// 功能：修改密码
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class UpdatePwd : Eyousoft.Common.Page.FrontPage
    {
        EyouSoft.BLL.CompanyStructure.CompanyUser cuBll = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            cuBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);
            if (IsPostBack)
            {
                InitBindPwd();
            }
        }
        #region 保存用户修改的信息
        protected void InitBindPwd()
        {
            //旧密码
            string PwdOld = Utils.GetFormValue("oldpas");
            //新密码
            string PwdNew = Utils.GetFormValue("newpas");
            //确认密码
            string PwdSubmit = Utils.GetFormValue("newpas2");
            if (PwdOld != SiteUserInfo.PassWordInfo.NoEncryptPassword)
            {
                SetErrorMsg(false,"旧密码错误！");
            }
            if (PwdNew != PwdSubmit)
            {
                SetErrorMsg(false, "两次密码不同！");
            }
            bool res = cuBll.UpdatePassWord(SiteUserInfo.ID, new EyouSoft.Model.CompanyStructure.PassWord(PwdNew));
            SetErrorMsg(res, res ? "修改成功！" : "修改失败！");
        }
        #endregion

        #region 提示操作信息方法
        /// <summary>
        /// 提示提交信息
        /// </summary>
        /// <param name="IsSuccess">true 执行成功 flase 执行失败</param>
        /// <param name="Msg">提示信息</param>
        private void SetErrorMsg(bool isSuccess, string msg)
        {
            var s = "{{\"isSuccess\":\"{0}\",\"errMsg\":\"{1}\"}}";
            Response.Clear();
            Response.Write(string.Format(s, isSuccess, msg));
            Response.End();
        }
        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
