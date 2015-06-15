using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using Common.Enum;

namespace Web.TeamPlan
{
    /// <summary>
    /// 付款申请
    /// 功能：付款申请
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-08 
    /// </summary>
    public partial class Payment : BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限判断
            string type = Utils.GetQueryStringValue("tourType");
            if (type != "")
            {
                this.hideTourType.Value = type;
                //团队计划
                if (type == "team")
                {
                    //团队计划：团队结算权限判断
                    if (CheckGrant(TravelPermission.团队计划_团队计划_栏目))
                    {
                        if (!CheckGrant(TravelPermission.团队计划_团队计划_团队结算))
                        {
                            Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_团队结算, false);
                            return;
                        }
                    }
                    else
                    {
                        Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_栏目, false);
                    }

                }
                //财务管理
                if (type == "account")
                {
                    //财务管理：权限判断
                    if (!CheckGrant(TravelPermission.财务管理_团队核算_栏目))
                    {
                        Utils.ResponseNoPermit(TravelPermission.财务管理_团队核算_栏目, false);
                        return;
                    }
                }
                //散拼计划
                if (type == "san")
                {
                    //团队计划：团队结算权限判断
                    if (CheckGrant(TravelPermission.散拼计划_散拼计划_栏目))
                    {
                        if (!CheckGrant(TravelPermission.散拼计划_散拼计划_团队结算))
                        {
                            Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_团队结算, false);
                        }
                    }
                    else
                    {
                        Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_栏目, false);
                    }
                }
            }
            #endregion


            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记, false);
                }
                DdlPayTypeInit("");
                BindInfo();
            }

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

        }
        void BindInfo()
        {
            EyouSoft.BLL.FinanceStructure.OutRegister regist = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
            IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> list = regist.GetOutRegisterList(new EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo() { ReceiveId = Utils.GetQueryStringValue("id") });
            rpt_list.DataSource = list;
            rpt_list.DataBind();
        }
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

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            EyouSoft.BLL.FinanceStructure.OutRegister regist = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
            EyouSoft.Model.FinanceStructure.OutRegisterInfo model = new EyouSoft.Model.FinanceStructure.OutRegisterInfo();
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
            //model.ReceiveCompanyId = Utils.GetInt(Utils.GetFormValue("hd_teamId"));
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
