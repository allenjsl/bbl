using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.Common
{
    /// <summary>
    /// 页面功能：添加特服
    /// Author:liuym
    /// Date:2011-01-13
    /// </summary>
    public partial class SpecialService : BackPage
    {
        #region Private Members
        protected string EditId = string.Empty;//修改ID
        protected string ServiceType = string.Empty;//服务类型
        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageType = PageType.boxyPage;
            if (Request.QueryString["EditId"] != null)
            {
                EditId = Request.QueryString["EditId"];
            }
            if (Request.QueryString["Type"] != null)
            {
                ServiceType = Request.QueryString["Type"];
            }
            if(!Page.IsPostBack)
            {
                if(EditId!="")
                {
                    ////初始化表单的值
                    //txtCost.Value = "";
                    //txtItem.Value = "";
                    //txtServiceContent.Value = "";
                    //ddlOperate.SelectedValue = int.Parse("1").ToString();
                }
            }
        }
        #endregion

        #region  保存特服
        protected void linkBtnSave_Click(object sender, EventArgs e)
        {
            ////服务标题
            //string ServeItem = Utils.GetText(txtItem.Value.Trim(), 50);
            ////服务内容
            //string ServeContent = Utils.GetText(txtServiceContent.Value.Trim(), 400);
            ////服务类型
            //string ServeType = ddlOperate.SelectedValue.ToString();
            ////费用
            //decimal ServeCost = Utils.GetDecimal(txtCost.Value.ToString().Trim());          
            //调用保存的方法
           
        }
        #endregion
    }
}
