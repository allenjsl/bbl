/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) TravelSky 2011
/// 模块名称：团款支出登记(添加支出明细)
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\Default.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-19 17:04:21
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
    /// 登记 
    /// by 田想兵 2011.1.19
    /// </summary>
    public partial class dengji : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页面初始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记, false);
                }
                DdlPayTypeInit("");
                BindInfo();
            }
            #region ajax修改
            if (Utils.GetQueryStringValue("act") == "update")
            {
                Response.Clear();

                EyouSoft.BLL.FinanceStructure.OutRegister regist = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
                EyouSoft.Model.FinanceStructure.OutRegisterInfo model = regist.GetOutRegisterList(new EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo() { ReceiveId = Utils.GetQueryStringValue("id") }).SingleOrDefault(x => x.RegisterId == Utils.GetFormValue("registid"));
                model.BillNo = Utils.GetFormValue("billno");
                model.IsBill = Utils.GetFormValue("isbill") == "1" ? true : false;
                model.PaymentAmount = Utils.GetDecimal(Utils.GetFormValue("payMoney"));
                model.PaymentDate = Utils.GetDateTime(Utils.GetFormValue("payDate"));
                model.PaymentType = (EyouSoft.Model.EnumType.TourStructure.RefundType)(Utils.GetInt(Utils.GetFormValue("payType")));
                model.Remark = Utils.GetFormValue("desc");
                model.StaffName = Utils.GetFormValue("staffname");
                model.ReceiveCompanyId = Utils.GetInt(Utils.GetFormValue("comId"));
                model.ReceiveCompanyName = Utils.GetFormValue("comName");
                if (!string.IsNullOrEmpty(Utils.GetFormValue("IsJidiao")))
                {
                    model.ReceiveType = Utils.GetEnumValue<EyouSoft.Model.EnumType.FinanceStructure.OutPlanType>(Utils.GetFormValue("jidiaoleibie"), (EyouSoft.Model.EnumType.FinanceStructure.OutPlanType)Utils.GetInt(Utils.GetFormValue("jidiaoleibie")));
                }
                else
                {
                    model.ReceiveType = Utils.GetEnumValue<EyouSoft.Model.EnumType.FinanceStructure.OutPlanType>(Utils.GetFormValue("jidiaoleibie"), EyouSoft.Model.EnumType.FinanceStructure.OutPlanType.单项服务供应商安排);
                }

                int j = regist.UpdateOutRegister(model);
                if (j > 0)
                {
                    Response.Write("修改成功!");
                }
                else if (j == -1)
                {
                    Response.Write("计调项目支出所有登记金额不能大于计调项目支出金额!");
                }
                else
                {
                    Response.Write("修改失败!");
                }

                Response.End();
            }
            #endregion

        }
        /// <summary>
        /// 绑定信息
        /// </summary>
        void BindInfo()
        {
            EyouSoft.BLL.FinanceStructure.OutRegister regist = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
            IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> list = regist.GetOutRegisterList(new EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo() { ReceiveId = Utils.GetQueryStringValue("id") });
            rpt_list.DataSource = list;
            rpt_list.DataBind();
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
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <param name="selectIndex"></param>
        /// <param name="ddl"></param>
        protected void DdlPayTypeInit(string selectIndex, DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("--请选择--", ""));
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType));
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = list[i].Value;
                    item.Text = list[i].Text;
                    ddl.Items.Add(item);
                }
            }
            if (selectIndex != "")
            {
                ddl.SelectedValue = selectIndex;
            }
        }
        #endregion
        /// <summary>
        /// 提交数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            EyouSoft.BLL.FinanceStructure.OutRegister regist = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
            EyouSoft.Model.FinanceStructure.OutRegisterInfo model = new EyouSoft.Model.FinanceStructure.OutRegisterInfo();
            #region 获取页面数据
            model.BillAmount = Utils.GetDecimal(txt_fukuan.Value);
            model.BillNo = Utils.GetFormValue("txt_code");
            model.CheckerId = 0;
            model.CompanyId = CurrentUserCompanyID;
            model.IsBill = Utils.GetInt(Utils.GetFormValue("sel_is")) == 1 ? true : false;
            model.IsChecked = false;
            model.IsPay = false;
            model.IssueTime = DateTime.Now;
            model.ItemId = Utils.GetQueryStringValue("tourid");
            model.OperatorId = SiteUserInfo.ID;
            model.PaymentAmount = Utils.GetDecimal(txt_fukuan.Value);
            model.PaymentDate = Utils.GetDateTime(Utils.GetFormValue("txt_payDate"));
            model.PaymentType = (EyouSoft.Model.EnumType.TourStructure.RefundType)(Utils.GetInt(Utils.GetFormValue(ddlPayType.UniqueID)));
            model.ReceiveCompanyId = Utils.GetInt(Utils.GetQueryStringValue("comid"));
            model.ReceiveCompanyName = Utils.GetQueryStringValue("com");
            model.ReceiveId = Utils.GetQueryStringValue("id");
            model.ReceiveType = (EyouSoft.Model.EnumType.FinanceStructure.OutPlanType)Utils.GetInt(Utils.GetQueryStringValue("type"));
            model.RegisterType = EyouSoft.Model.EnumType.FinanceStructure.OutRegisterType.团队;
            model.Remark = Utils.GetFormValue("txt_desc");
            model.StaffName = Utils.GetFormValue("txt_Lxr");
            model.StaffNo = 0;
            decimal outdecimal = 0;
            decimal mm = regist.GetExpenseRegisterAmount(null, Utils.GetQueryStringValue("id"), (EyouSoft.Model.EnumType.FinanceStructure.OutPlanType)Utils.GetInt(Utils.GetQueryStringValue("type")), out outdecimal);
            if (outdecimal < mm + model.PaymentAmount)
            {
                EyouSoft.Common.Function.MessageBox.Show(this.Page, "总登记金额已超出应付金额!");
                DdlPayTypeInit("");
                BindInfo();
                return;
            }
            #endregion
            int i = regist.AddOutRegister(model);
            if (i == 1)
            {
                //成功
                Response.Write("<script>alert('登记成功!');location.href=location.href;</script>");
            }
            else
            {
                //成功
                Response.Write("<script>alert('登记失败!');</script>");
            }
        }

        /// <summary>
        /// 支付方式和是否开票绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpt_list_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.Model.FinanceStructure.OutRegisterInfo model = e.Item.DataItem as EyouSoft.Model.FinanceStructure.OutRegisterInfo;
            DropDownList ddl = e.Item.FindControl("sel_PayType") as DropDownList;
            DdlPayTypeInit(((int)model.PaymentType).ToString(), ddl);
            DropDownList sel_isbill = e.Item.FindControl("sel_isbill") as DropDownList;
            sel_isbill.Items.Add(new ListItem("是", "1"));
            sel_isbill.Items.Add(new ListItem("否", "2"));
            sel_isbill.Items.FindByValue(model.IsBill == true ? "1" : "2").Selected = true;
        }
    }
}
