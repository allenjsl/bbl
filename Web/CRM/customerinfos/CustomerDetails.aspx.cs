using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.CRM.customerinfos
{
    /// <summary>
    ///  模块名称：客户资料查看页
    ///  功能说明：显示客户部分信息，不能修改
    /// </summary>
    /// 创建人：DYZ
    /// 创建时间：2011-05-30


    public partial class CustomerDetails : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //客户实体ID
                int customerID = Utils.GetInt(Utils.GetQueryStringValue("cId"));
                //初始化
                DataInit(customerID);
            }
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        protected void DataInit(int customerID)
        {
            EyouSoft.Model.CompanyStructure.CustomerInfo customerModel = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(customerID);
            if (customerModel != null)
            {
                this.lblCompanyName.Text = customerModel.Name;
                this.rptList.DataSource = customerModel.CustomerContactList;
                this.rptList.DataBind();
            }
            else
            {
                Response.Clear();
                Response.Write("该客户信息未找到!");
                Response.End();
            }
        }

    }
}
