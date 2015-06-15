using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 财务管理：杂费收入、支出
    /// 功能：修改杂费收入、支出
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-20
    public partial class IncomeDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string id = Utils.GetQueryStringValue("id");
                //类型
                string type = Utils.GetQueryStringValue("type");
                if (id != "")
                {
                    DataInit(id);
                }
                if (type == "expend")
                {
                    this.lblDateTime.Text = "支出日期";
                    this.lblPeople.Text = "付款人";
                    this.lblPro.Text = "支出项目";
                }
            }
        }

        /// <summary>
        /// 修改、添加保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string proName = this.txtProName.Text.Trim();
            string company = this.txtCompany.Text.Trim();
            string people = this.txtPeople.Text.Trim();
            string dateTime = this.txtDateTime.Text.Trim();
            string price = this.txtPrice.Text.Trim();
            string remarks = this.txtRemarks.Text.Trim();

            if (proName == "" && company == "" && people == "" && dateTime == "" && price == "" && remarks == "")
            {
                Page.RegisterStartupScript("提示", Utils.ShowMsg("请输入值!"));
                return;
            }
            Page.RegisterStartupScript("提示", Utils.ShowMsg("保存成功!"));
        }

        protected void DataInit(string id)
        { 
            //GETMODEL
            this.txtProName.Text = "1";
            this.txtCompany.Text = "1";
            this.txtPeople.Text = "1";
            this.txtDateTime.Text = "1";
            this.txtPrice.Text = "1";
            this.txtRemarks.Text = "1";
        }
    }
}
