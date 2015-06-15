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
using EyouSoft.Model.PersonalCenterStructure;
using System.Collections.Generic;
using EyouSoft.Common;
using EyouSoft.Common.Function;
namespace Web.UserCenter.UserInfo
{
    /// <summary>
    /// 页面：个人中心--工作交流-交流中心
    /// 功能：新增和修改
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class ExchangeAdd : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected EyouSoft.Model.PersonalCenterStructure.WorkExchange exModel = new EyouSoft.Model.PersonalCenterStructure.WorkExchange();//交流专区实体
        protected string type = string.Empty;// modify：修改 add：新增 default：新增
        protected int tid = 0;//主键ID
        EyouSoft.BLL.PersonalCenterStructure.WorkExchange workbll = null;//工作交流BLL
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            workbll = new EyouSoft.BLL.PersonalCenterStructure.WorkExchange();
            if (!IsPostBack)
            {
                type = EyouSoft.Common.Utils.GetQueryStringValue("type");
                switch (type)
                {
                    case "modify":
                        Bind();
                        this.Title = "交流修改_工作交流_个人中心";
                        break;
                    default:
                        Add();
                        this.Title = "交流新增_工作交流_个人中心";
                        break;
                }
            }
            else
            {
                Save();//新增修改数据保存
            }
        }

        /// <summary>
        /// 新增时初使化
        /// </summary>
        private void Add()
        {
            exModel.OperatorName = this.SiteUserInfo.ContactInfo.ContactName;
            exModel.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 新增修改
        /// </summary>
        private void Save()
        {
            bool res = false;//是否成功
            tid = Utils.GetInt(Utils.GetFormValue("tid"));//主键ID
            if (tid > 0)
            {
                exModel = workbll.GetModel(tid); //修改获取实体
            }
            else //新增初使化数据
            {
                exModel.CompanyId = CurrentUserCompanyID;
                exModel.IsDelete = false;
                exModel.Replys = 0;
                exModel.OperatorId = SiteUserInfo.ID;
                exModel.Clicks = 0;
                exModel.IsDelete = false;
                exModel.CreateTime = DateTime.Now;//创建时间

                //现在需求所有人查看
                exModel.AcceptList = new List<EyouSoft.Model.PersonalCenterStructure.WorkExchangeAccept>();
                EyouSoft.Model.PersonalCenterStructure.WorkExchangeAccept exAct = new EyouSoft.Model.PersonalCenterStructure.WorkExchangeAccept();
                exAct.AcceptId = 0;
                exAct.AcceptType = EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType.所有;
                exModel.AcceptList.Add(exAct);

            }
            exModel.Title = Utils.GetFormValue("title"); //标题
            exModel.Description = Utils.EditInputText(Request.Form["description"]); //内容
            exModel.IsAnonymous = Utils.GetFormValue("isAnonymous") == "on" ? true : false; //是否匿名
            if (exModel.IsAnonymous)
            {
                exModel.OperatorName = "匿名";
            }
            else
            {
                exModel.OperatorName = SiteUserInfo.ContactInfo.ContactName;
            }

            if (tid > 0)
            {
                res = workbll.Update(exModel); //更新
            }
            else
            {
                res = workbll.Add(exModel);//新增
            }
            if (res)
            {
                string conti = Utils.GetFormValue("continue");
                if (conti == "continue")
                {
                    MessageBox.ShowAndRedirect(this, "操作成功！", "ExchangeAdd.aspx");
                }
                else
                {
                    MessageBox.ShowAndRedirect(this, "操作成功！", "Exchange.aspx");
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
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"), 0);
            exModel = workbll.GetModel(tid);
        }
    }
}
