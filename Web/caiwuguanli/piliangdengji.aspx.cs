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
    /// 批量审核
    /// </summary>
    /// 创建人：柴逸宁
    /// 创建时间：2011-07-4
    public partial class piliangdengji : Eyousoft.Common.Page.BackPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var s = Utils.GetQueryStringValue("gysName");
            if (!IsPostBack)
            {

                DdlPayTypeInit();//绑定支付方式

            }
        }
        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.FinanceStructure.MBatchRegisterExpenseInfo model = new EyouSoft.Model.FinanceStructure.MBatchRegisterExpenseInfo();
            model.CompanyId = CurrentUserCompanyID;
            /*int comType = Utils.GetInt(Utils.GetQueryStringValue("comType"));
            if (comType == 0)
            {
                model.ExpenseType = null;
            }
            else
            {
                model.ExpenseType = (EyouSoft.Model.EnumType.FinanceStructure.OutPlanType)comType;
            }*/
            model.PaymentType = (EyouSoft.Model.EnumType.TourStructure.RefundType)(Utils.GetInt(Utils.GetFormValue("ddlPayType")));
            model.OperatorId = SiteUserInfo.ID;
            model.Payer = txtStaffName.Text;
            model.PayerId = 0;
            model.PaymentTime = Utils.GetDateTime(txtPayDate.Text, DateTime.Now);
            model.Remark = t_desc.Value;
            model.SearchGYSName = Utils.GetQueryStringValue("gysName");
            model.SearchGYSType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), Utils.GetQueryStringValue("comType"), null);

            IList<string> ls = new List<string>();
            string[] tourids = Utils.GetQueryStringValue("tourids").Split(',');
            for (int i = 0; i < tourids.Length; i++)
            {
                ls.Add(tourids[i]);
            }
            model.TourIds = ls;
            EyouSoft.BLL.FinanceStructure.BSpendRegister bll = new EyouSoft.BLL.FinanceStructure.BSpendRegister();

            if (bll.BatchRegisterExpense(model) > 0)
            {
                Utils.ShowMsgAndCloseBoxy("登记成功！", Utils.GetQueryStringValue("IframeId"), true);
            }
            else
            {
                Response.Write("<script>alert('登记失败!');</script>");
            }
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <param name="selectIndex"></param>
        /// <param name="ddl"></param>
        protected void DdlPayTypeInit()
        {
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType));
            if (list != null && list.Count > 0)
            {
                ddlPayType.DataSource = list;
                ddlPayType.DataBind();
            }
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
