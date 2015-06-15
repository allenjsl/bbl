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
using System.Web.Services.Description;
using EyouSoft.Common.Function;

namespace Web.UserCenter.UserInfo
{
    /// <summary>
    /// 页面功能：个人中心--个人信息
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class UserInfo : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected EyouSoft.Model.CompanyStructure.CompanyUser user = new EyouSoft.Model.CompanyStructure.CompanyUser();
        EyouSoft.BLL.CompanyStructure.CompanyUser userbll = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            userbll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);
            if (!IsPostBack)
            {
                Datainit();
            }
            else
            {
                Save();
                Datainit();
            }
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        private void Save()
        {
            string _wrong = string.Empty;

            user = userbll.GetUserInfo(this.SiteUserInfo.ID);
            user.PersonInfo.ContactName = Utils.GetFormValue("name");
            user.PersonInfo.ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Utils.GetInt(Utils.GetFormValue("sex"),0);
            user.PersonInfo.ContactFax = Utils.GetFormValue("fax");
            //电话号码 
            if (Utils.IsPhone(Utils.GetFormValue("phone").Trim()) || Utils.GetFormValue("phone").Trim()=="")
            {
                user.PersonInfo.ContactTel = Utils.GetFormValue("phone");
            }
            else
            {
                _wrong = "请输入正确的电话号码！";
            }
            //手机
            if (Utils.IsMobile(Utils.GetFormValue("mobile").Trim()) || Utils.GetFormValue("mobile").Trim() == "")
            {
                user.PersonInfo.ContactMobile = Utils.GetFormValue("mobile");
            }
            else
            {
                _wrong += "请输入正确的手机号码！";
            }
            user.PersonInfo.QQ = Utils.GetFormValue("QQ").Trim();
            user.PersonInfo.MSN = Utils.GetFormValue("msn").Trim();
            user.PersonInfo.ContactEmail = Utils.GetFormValue("email").Trim();

            if (string.IsNullOrEmpty(_wrong))
            {
                bool res = userbll.Update(user);



                if (user.PassWordInfo.NoEncryptPassword != Utils.GetFormValue("psd"))
                {
                    userbll.UpdatePassWord(user.ID, new EyouSoft.Model.CompanyStructure.PassWord(Utils.GetFormValue("psd")));
                }


                if (res)
                {
                    MessageBox.ResponseScript(this, "; alert('保存成功');location.href='/UserCenter/UserInfo/UserInfo.aspx';");
                }
                else
                {
                    MessageBox.ShowAndReturnBack(this, "操作失败",1);
                }
            }
            else
            {
                MessageBox.ShowAndReturnBack(this, _wrong, 1);
            }
            
        }

        /// <summary>
        /// 初使化用户信息
        /// </summary>
        private void Datainit()
        {
            user = userbll.GetUserInfo(this.SiteUserInfo.ID);
        }     
    }
}
