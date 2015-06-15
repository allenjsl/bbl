/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) TravelSky 2011
/// 模块名称：退款
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\Default.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-19 15:04:21
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// ////////////////////////////////////////////////////////////////////////////////

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
    /// 退款
    /// </summary>
    public partial class tuikuan : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页面变量K
        /// </summary>
        public int k = 0;
        /// <summary>
        /// 页面初始绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_退款审核))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款收入_退款审核, false);
                }
                Bind();
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        void Bind()
        {
            EyouSoft.BLL.TourStructure.TourOrder bll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> list = bll.GetRefund(Request.QueryString["id"]);
            rpt_list.DataSource = list;
            rpt_list.DataBind();
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }
        /// <summary>
        /// 列表数据项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpt_list_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton lbtn_Audit = e.Item.FindControl("lnk_Audit") as LinkButton;
            lbtn_Audit.Attributes["onclick"] = "return confirm('是否确认删除');";
        }

        /// <summary>
        /// 列表数据项操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rpt_list_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            EyouSoft.BLL.TourStructure.TourOrder bll = new EyouSoft.BLL.TourStructure.TourOrder();

            switch (e.CommandName)
            {
                case "update":
                    {
                        EyouSoft.Model.FinanceStructure.ReceiveRefund model = new EyouSoft.Model.FinanceStructure.ReceiveRefund();
                        model.BillAmount = Utils.GetDecimal(Utils.GetFormValues("hd_skMoney")[e.Item.ItemIndex]);
                        model.CheckerId = SiteUserInfo.ID;
                        model.CompanyID = CurrentUserCompanyID;
                        model.IsBill = Utils.GetFormValues("ddl_piao")[e.Item.ItemIndex] == "1" ? true : false;
                        model.IsCheck = true;
                        model.IsReceive = false;
                        model.IssueTime = Utils.GetDateTime(Utils.GetFormValues("IssueTime")[e.Item.ItemIndex]);
                        model.ItemId = Request.QueryString["id"];
                        model.ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单;
                        model.OperatorID = SiteUserInfo.ID;
                        model.PayCompanyId = Utils.GetInt(Utils.GetFormValues("hd_PayCompanyId")[e.Item.ItemIndex]);
                        model.PayCompanyName = Utils.GetFormValues("hd_PayCompanyName")[e.Item.ItemIndex];
                        model.RefundDate = DateTime.Now;
                        model.RefundMoney = Utils.GetDecimal(Utils.GetFormValues("hd_skMoney")[e.Item.ItemIndex]);
                        model.RefundType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetFormValues("ddl_Type")[e.Item.ItemIndex]);
                        model.Remark = Utils.GetFormValues("txt_mark")[e.Item.ItemIndex];
                        model.StaffName = Utils.GetFormValues("txt_user")[e.Item.ItemIndex];
                        model.StaffNo = 0;
                        bll.UpdateRefund(model, Request.QueryString["id"],EyouSoft.Model.EnumType.TourStructure.LogRefundSource.财务管理_团款收入);

                    } break;//审核
                case "delete":
                    {
                        bll.DeleteRefund(CurrentUserCompanyID, Utils.GetFormValues("hd_id")[e.Item.ItemIndex], Request.QueryString["id"], EyouSoft.Model.EnumType.TourStructure.LogRefundSource.财务管理_团款收入);
                    } break;//删除
            }
            Bind();
        }
        /// <summary>
        /// 提交数据库操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {

            EyouSoft.BLL.TourStructure.TourOrder bll = new EyouSoft.BLL.TourStructure.TourOrder();
            IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> list = new List<EyouSoft.Model.FinanceStructure.ReceiveRefund>();
            string[] str_chk = Utils.GetFormValues("checkbox");
            for (int i = 0; i < str_chk.Length; i++)
            {
                EyouSoft.Model.FinanceStructure.ReceiveRefund model = new EyouSoft.Model.FinanceStructure.ReceiveRefund();
                model.BillAmount = Utils.GetDecimal(Utils.GetFormValues("hd_skMoney")[Utils.GetInt(str_chk[i])]);
                model.CheckerId = SiteUserInfo.ID;
                model.CompanyID = CurrentUserCompanyID;
                model.IsBill = Utils.GetFormValues("ddl_piao")[Utils.GetInt(str_chk[i])] == "1" ? true : false;
                model.IsCheck = true;
                model.IsReceive = false;
                model.IssueTime = Utils.GetDateTime(Utils.GetFormValues("IssueTime")[Utils.GetInt(str_chk[i])]);
                model.ItemId = Request.QueryString["id"];
                model.ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单;
                model.OperatorID = SiteUserInfo.ID;
                model.PayCompanyId = Utils.GetInt(Utils.GetFormValues("hd_PayCompanyId")[Utils.GetInt(str_chk[i])]);
                model.PayCompanyName = Utils.GetFormValues("hd_PayCompanyName")[Utils.GetInt(str_chk[i])];
                model.RefundDate = DateTime.Now;
                model.RefundMoney = Utils.GetDecimal(Utils.GetFormValues("hd_skMoney")[Utils.GetInt(str_chk[i])]);
                model.RefundType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetFormValues("ddl_Type")[Utils.GetInt(str_chk[i])]);
                model.Remark = Utils.GetFormValues("txt_mark")[Utils.GetInt(str_chk[i])];
                model.StaffName = Utils.GetFormValues("txt_user")[Utils.GetInt(str_chk[i])];
                model.StaffNo = 0;
                list.Add(model);
            }
            bll.UpdateReceive(list, Request.QueryString["id"],EyouSoft.Model.EnumType.TourStructure.LogRefundSource.财务管理_团款收入);
        }
    }
}
