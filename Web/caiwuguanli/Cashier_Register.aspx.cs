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
    /// 登记出纳  
    /// by 田想兵 2010.2.18
    /// </summary>
    public partial class dengjiChuLa : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_出纳登帐_到款登记))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_出纳登帐_到款登记, false);
                }
                DdlPayTypeInit("");
            }
        }
        /// <summary>
        /// 窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        #region 初始化支出方式
        /// <summary>
        /// 支付方式初始化
        /// </summary>
        /// <param name="selectIndex"></param>
        protected void DdlPayTypeInit(string selectIndex)
        {
            this.ddlPayType.Items.Clear();
            this.ddlPayType.Items.Add(new ListItem("--请选择--", ""));
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType));
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = list[i].Value;
                    item.Text = list[i].Text;
                    this.ddlPayType.Items.Add(item);
                }
            }
            if (selectIndex != "")
            {
                this.ddlPayType.SelectedValue = selectIndex;
            }
        }
        #endregion
        #region 登记
        protected void lbtn_Submit_Click(object sender, EventArgs e)
        {
            EyouSoft.BLL.FinanceStructure.CashierRegister bll = new EyouSoft.BLL.FinanceStructure.CashierRegister(SiteUserInfo);
            EyouSoft.Model.FinanceStructure.CashierRegisterInfo model = new EyouSoft.Model.FinanceStructure.CashierRegisterInfo();
            model.CompanyId = CurrentUserCompanyID;
            model.Contacter = txt_sendUser.Text;
            model.ContactTel = Utils.GetFormValue("txt_phone");
            model.CreateTime = DateTime.Now;
            model.CustomerId = Utils.GetInt(Utils.GetFormValue("hd_teamId"));
            model.CustomerName = Utils.GetFormValue("txt_teamName");
            model.OperatorId = SiteUserInfo.ID;
            model.PaymentBank = Utils.GetFormValue("txt_bank");
            model.PaymentCount = Utils.GetDecimal(Utils.GetFormValue("txt_money"));
            model.PaymentTime = Utils.GetDateTime(Utils.GetFormValue(txt_date.UniqueID), DateTime.Now);
            model.Remark = Utils.GetFormValue(txt_Remarks.UniqueID);
            
            int k=bll.AddCashierRegister(model);
            if (k > 0)
            {
                Response.Write("<script>alert('登记成功');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.location.reload();</script>");
            }
            else
            {
                Response.Write("<script>alert('登记失败');location.href=location.href;</script>");
            }
        }
        #endregion
    }
}
