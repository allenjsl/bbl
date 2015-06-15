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
    /// 出纳登账
    /// by 田想兵 2011.1.21
    /// </summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011-06-21
    /// 修改内容：金额栏添加金额的合计
    public partial class chunadz_list : Eyousoft.Common.Page.BackPage
    {
        #region 合计变量
        /// <summary>
        /// 到款银行
        /// </summary>
        protected decimal paymentCount = 0;
        /// <summary>
        /// 已销账金额
        /// </summary>
        protected decimal totalAmount = 0;
        #endregion
        /// <summary>
        /// 全局行号
        /// </summary>
        public int i = 0;
        /// <summary>
        /// 页面初始绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_出纳登帐_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_出纳登帐_栏目, false);
                }
                BindInfo();
            }
        }
        /// <summary>
        /// 绑定
        /// </summary>
        void BindInfo()
        {
            EyouSoft.BLL.FinanceStructure.CashierRegister bll = new EyouSoft.BLL.FinanceStructure.CashierRegister(SiteUserInfo);
            int sumcount = 0;
            EyouSoft.Model.FinanceStructure.QueryCashierRegisterInfo model = new EyouSoft.Model.FinanceStructure.QueryCashierRegisterInfo();
            model.CompanyId = CurrentUserCompanyID;
            model.CustomerId = Utils.GetInt(Utils.GetQueryStringValue("comid"));
            //model.EndTime = Utils.GetDateTime( txt_date.Value);
            model.PaymentBank = Utils.GetQueryStringValue("bank");

            model.StartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("begin"));

            txt_bank.Value = model.PaymentBank;
            txt_com.Value = Utils.GetQueryStringValue("com");
            hd_comId.Value = Utils.GetQueryStringValue("comid");
            txt_date.Value = Utils.GetQueryStringValue("begin");

            IList<EyouSoft.Model.FinanceStructure.CashierRegisterInfo> ilist = bll.GetList(model, 20, Utils.GetInt(Request.QueryString["page"], 1), ref sumcount);
            rpt_list.DataSource = ilist;
            rpt_list.DataBind();
            //合计
            bll.GetCashierRegisterMoney(model, ref paymentCount, ref totalAmount);
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = sumcount;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);

        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            //BindInfo();
            Response.Redirect("chunadz_list.aspx?bank=" + txt_bank.Value + "&comid=" + hd_comId.Value + "&com=" + txt_com.Value + "&begin=" + txt_date.Value);
        }
        /// <summary>
        /// 销账操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rpt_list_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "update")
            {
                EyouSoft.Model.FinanceStructure.CashierRegisterInfo model = e.Item.DataItem as EyouSoft.Model.FinanceStructure.CashierRegisterInfo;
                EyouSoft.BLL.FinanceStructure.CashierRegister bll = new EyouSoft.BLL.FinanceStructure.CashierRegister(SiteUserInfo);
                IList<EyouSoft.Model.FinanceStructure.CancelRegistInfo> list = new List<EyouSoft.Model.FinanceStructure.CancelRegistInfo>();
                list.Add(new EyouSoft.Model.FinanceStructure.CancelRegistInfo() { Money = model.PaymentCount, CompanyId = CurrentUserCompanyID, OrderId = "" });
                int i = bll.CancelRegist(model.RegisterId, SiteUserInfo.ID, SiteUserInfo.UserName, list);
                if (i > 0)
                {
                    Response.Write("<script>alert(\"销账成功!\");</script>");
                }
            }
        }
    }
}
