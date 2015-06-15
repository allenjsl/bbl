using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Common.Enum;

namespace Web.TeamPlan
{
    /// <summary>
    /// 团队计算
    /// 功能：团队计划结算
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-13 
    /// </summary>
    public partial class TeamSettle : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string type = Utils.GetQueryStringValue("type");
                string planId = Utils.GetQueryStringValue("id");

                if (type != "")
                {
                    this.hideType.Value = type;
                    //设置结算单打印路径
                    
                    //设置
                    //团队计划
                    if (type == "team")
                    {
                        //团队计划：团队结算权限判断
                        if (CheckGrant(TravelPermission.团队计划_团队计划_栏目))
                        {
                            if (CheckGrant(TravelPermission.团队计划_团队计划_团队结算))
                            {
                                this.lblLocation.Text = "团队计划";
                                this.lblLocationT.Text = "团队计划";
                                Page.Title = "团队计划_团队结算";
                                this.hidePrintUrl.Value = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队结算明细单);
                                this.pnlPlan.Visible = true;
                                this.pnlAccount.Visible = false;
                            }
                            else
                            {
                                Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_团队结算, false);
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
                        if (CheckGrant(TravelPermission.财务管理_团队核算_栏目))
                        {
                            this.lblLocation.Text = "财务管理";
                            this.lblLocationT.Text = "财务管理";
                            Page.Title = "财务管理_团队核算";
                            this.pnlPlan.Visible = false;
                            this.pnlAccount.Visible = true;
                            this.pnlTuiHuiCaiWu.Visible = false;
                            //团队复核
                            if (!CheckGrant(TravelPermission.财务管理_团队核算_团队复核))
                            {
                                this.pnlFuHe.Visible = false;
                            }
                            //核算结束
                            if (!CheckGrant(TravelPermission.财务管理_团队核算_核算结束))
                            {
                                this.pnlJieSu.Visible = false;
                            }
                            //退回计调
                            if (!CheckGrant(TravelPermission.财务管理_团队核算_退回计调))
                            {
                                this.pnlJiDiao.Visible = false;
                            }
                            ////添加收入
                            //if (!CheckGrant(TravelPermission.财务管理_团队核算_其它收入添加))
                            //{
                            //    this.pnlSecond.Visible = false;
                            //}
                            ////添加支出
                            //if (!CheckGrant(TravelPermission.财务管理_团队核算_其它支出添加))
                            //{
                            //    this.pnlFouth.Visible = false;
                            //}
                            //添加分配
                            if (!CheckGrant(TravelPermission.财务管理_团队核算_利润分配添加))
                            {
                                this.pnlFouth.Visible = false;
                            }
                        }
                        else
                        {
                            Utils.ResponseNoPermit(TravelPermission.财务管理_团队核算_栏目, false);
                        }
                    }
                    //散拼计划
                    if (type == "san")
                    {
                        //团队计划：团队结算权限判断
                        if (CheckGrant(TravelPermission.散拼计划_散拼计划_栏目))
                        {
                            if (CheckGrant(TravelPermission.散拼计划_散拼计划_团队结算))
                            {
                                this.lblLocation.Text = "散拼计划";
                                this.lblLocationT.Text = "散拼计划";
                                Page.Title = "散拼计划_团队结算";
                                this.pnlPlan.Visible = true;
                                this.pnlAccount.Visible = false;
                                this.hidePrintUrl.Value = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.结算明细单);
                            }
                            else
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

                if (planId != "")
                {
                    EyouSoft.Model.TourStructure.TourBaseInfo model = new EyouSoft.BLL.TourStructure.Tour().GetTourInfo(planId);
                    if (model != null)
                    {
                        //核算结束时隐藏所有操作按钮
                        //计划已提交账务，由散拼或团队计划进入团队核算页面时隐藏所有操作按钮
                        if (model.Status == EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束 ||
                            ((type == "san" || type == "team") && model.Status == EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算))
                        {
                            //复核
                            this.pnlFuHe.Visible = false;
                            //核算结束
                            this.pnlJieSu.Visible = false;
                            //退回计调
                            this.pnlJiDiao.Visible = false;
                            //保存按钮
                            this.pnlSave.Visible = false;
                            //提交财务
                            this.pnlTiJiao.Visible = false;
                            //添加收入
                            this.pnlSecond.Visible = false;
                            //其它支出
                            this.pnlFouth.Visible = false;
                            //利润分配
                            this.pnlFifth.Visible = false;
                        }

                        //判断是否为单项服务
                        if (model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
                        {
                            EyouSoft.Model.TourStructure.TourSingleInfo singleModel = (EyouSoft.Model.TourStructure.TourSingleInfo)model;
                            //单项服务 支出
                            this.rptListThird_S.DataSource = singleModel.Plans;
                            this.rptListThird_S.DataBind();
                        }
                        else
                        {
                            //团队计划 && 散拼计划 支出
                            this.rptListThird.DataSource = new EyouSoft.BLL.PlanStruture.TravelAgency().GetSettleList(planId);
                            this.rptListThird.DataBind();
                        }
                    }

                    this.hidePlanID.Value = planId;
                    //收入
                    this.rptListFrist.DataSource = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetOrderList(SiteUserInfo.CompanyID, planId);
                    this.rptListFrist.DataBind();

                    //其它收入
                    EyouSoft.Model.FinanceStructure.OtherCostQuery otherCostQueryModel = new EyouSoft.Model.FinanceStructure.OtherCostQuery();
                    otherCostQueryModel.TourId = planId;
                    this.rptListSecond.DataSource = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo).GetOtherIncomeList(otherCostQueryModel);
                    this.rptListSecond.DataBind();

                    //其它支出
                    this.rptListForth.DataSource = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo).GetOtherOutList(otherCostQueryModel);
                    this.rptListForth.DataBind();

                    //利润分配
                    this.rptListFifth.DataSource = new EyouSoft.BLL.FinanceStructure.TourProfitSharer(SiteUserInfo).GetTourShareList(planId);
                    this.rptListFifth.DataBind();

                    //团队核算退回财务权限 如果团队状态为核算结束&&从账务管理团队核算页面点击进去的&&有退回账务权限&&//是系统管理员时
                    if (CheckGrant(TravelPermission.财务管理_团队核算_退回财务) && Request.QueryString["Account"] == "yes" && model.Status == EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束 //&& SiteUserInfo.IsAdmin == true
                        )
                    {
                        this.pnlTuiHuiCaiWu.Visible = true;
                    }
                }               
               
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string tourId = Utils.InputText(this.hidePlanID.Value);
            bool isUpdate = true;
            #region 收入
            //订单号
            string[] hideFrist = Utils.GetFormValues("hideFrist");

            if (hideFrist != null && hideFrist.Count() > 0)
            {
                IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> list = new List<EyouSoft.Model.TourStructure.OrderFinanceExpense>();
                for (int i = 0; i < hideFrist.Count(); i++)
                {
                    EyouSoft.Model.TourStructure.OrderFinanceExpense model = new EyouSoft.Model.TourStructure.OrderFinanceExpense();
                    //ID
                    string id = hideFrist[i];
                    //公司名称
                    //string hideFristCompanyName = Utils.GetFormValue("hideFristCompanyName_" + id);
                    //收入金额
                    string hideFristIncome = Utils.GetFormValue("hideFristIncome_" + id);
                    //增加费用
                    string incomeAdd = Utils.GetFormValue("txtIncomeAdd_" + id);
                    //减少费用
                    string incomeReduc = Utils.GetFormValue("txtIncomeReduc_" + id);
                    //备注
                    string incomeRemarks = Utils.GetFormValue("txtIncomeRemarks_" + id);

                    model.OrderId = id;
                    //model.CompanyName = hideFristCompanyName;
                    model.SumPrice = Utils.GetDecimal(hideFristIncome);
                    model.FinanceAddExpense = Utils.GetDecimal(incomeAdd);
                    model.FinanceRedExpense = Utils.GetDecimal(incomeReduc);
                    //计算小计
                    model.FinanceSum = model.SumPrice + model.FinanceAddExpense - model.FinanceRedExpense;
                    model.FinanceRemark = incomeRemarks;
                    list.Add(model);
                }
                isUpdate = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).UpdateFinanceExpense(list, tourId);
            }
            #endregion

            #region 其它收入
            string[] hideSecond = Utils.GetFormValues("hideSecond");

            if (hideSecond != null && hideSecond.Count() > 0)
            {
                IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> list = new List<EyouSoft.Model.FinanceStructure.OtherIncomeInfo>();
                for (int i = 0; i < hideSecond.Count(); i++)
                {
                    EyouSoft.Model.FinanceStructure.OtherIncomeInfo model = new EyouSoft.Model.FinanceStructure.OtherIncomeInfo();

                    string id = hideSecond[i];
                    //收入金额
                    string hideOtherIncome = Utils.GetFormValue("hideOtherIncome_" + id);
                    //增加费用
                    string otherAdd = Utils.GetFormValue("txtOtherAdd_" + id);
                    //减少费用
                    string otherReduc = Utils.GetFormValue("txtOtherReduc_" + id);
                    //备注
                    string otherRemarks = Utils.GetFormValue("txtOtherRemarks_" + id);

                    model.IncomeId = id;
                    model.Amount = Utils.GetDecimal(hideOtherIncome);
                    model.AddAmount = Utils.GetDecimal(otherAdd);
                    model.ReduceAmount = Utils.GetDecimal(otherReduc);
                    model.TotalAmount = model.Amount + model.AddAmount - model.ReduceAmount;
                    model.Remark = otherRemarks;
                    model.TourId = tourId;
                    list.Add(model);
                }
                int count = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo).UpdateOtherIncome(list);
                if (count <= 0)
                {
                    isUpdate = false;
                }
            }
            #endregion

            #region 团队计划 && 散拼计划 支出
            string[] hideThird = Utils.GetFormValues("hideThird");
            if (hideThird != null && hideThird.Count() > 0)
            {
                IList<EyouSoft.Model.PlanStructure.PaymentList> list = new List<EyouSoft.Model.PlanStructure.PaymentList>();
                for (int i = 0; i < hideThird.Count(); i++)
                {
                    EyouSoft.Model.PlanStructure.PaymentList model = new EyouSoft.Model.PlanStructure.PaymentList();
                    string id = hideThird[i];
                    //金额
                    string hideExpendIncome = Utils.GetFormValue("hideExpendIncome_" + id);
                    //增加费用
                    string expendAdd = Utils.GetFormValue("txtExpendAdd_" + id);
                    //减少费用
                    string expendLess = Utils.GetFormValue("txtExpendLess_" + id);
                    //备注
                    string expendRemarks = Utils.GetFormValue("txtExpendRemarks_" + id);
                    //类型
                    string supplierType = Utils.GetFormValue("hideSupplierType_" + id);
                    model.Id = id;
                    model.PayAmount = Utils.GetDecimal(hideExpendIncome);
                    model.AddAmount = Utils.GetDecimal(expendAdd);
                    model.ReduceAmount = Utils.GetDecimal(expendLess);
                    model.TotalAmount = model.PayAmount + model.AddAmount - model.ReduceAmount;
                    model.FRemark = expendRemarks;
                    model.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), supplierType);
                    list.Add(model);
                }
                bool result = new EyouSoft.BLL.PlanStruture.TravelAgency().UpdateSettle(list, tourId);
                if (!result)
                {
                    isUpdate = false;
                }
            }
            #endregion

            #region 单项服务
            string[] hideThird_S = Utils.GetFormValues("hideThird-S");
            if (hideThird_S != null && hideThird_S.Count() > 0)
            {
                IList<EyouSoft.Model.TourStructure.PlanSingleInfo> list = new List<EyouSoft.Model.TourStructure.PlanSingleInfo>();
                for (int i = 0; i < hideThird_S.Count(); i++)
                {
                    EyouSoft.Model.TourStructure.PlanSingleInfo model = new EyouSoft.Model.TourStructure.PlanSingleInfo();
                    string id = hideThird_S[i];
                    //金额
                    string hideExpendIncome = Utils.GetFormValue("hideExpendIncome-S_" + id);
                    //增加费用
                    string expendAdd = Utils.GetFormValue("txtExpendAdd-S_" + id);
                    //减少费用
                    string expendLess = Utils.GetFormValue("txtExpendLess-S_" + id);
                    //备注
                    string expendRemarks = Utils.GetFormValue("txtExpendRemarks-S_" + id);
                    //类型
                    string serviceType = Utils.GetFormValue("hideSupplierType-S_" + id);
                    model.PlanId = id;
                    model.Amount = Utils.GetDecimal(hideExpendIncome);
                    model.AddAmount = Utils.GetDecimal(expendAdd);
                    model.ReduceAmount = Utils.GetDecimal(expendLess);
                    model.TotalAmount = model.Amount + model.AddAmount - model.ReduceAmount;
                    model.FRemark = expendRemarks;
                    model.ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), serviceType);
                    list.Add(model);
                }
                int result = new EyouSoft.BLL.PlanStruture.PlanSingle().SetSingleAccounting(tourId, list);
                if (result <= 0)
                {
                    isUpdate = false;
                }
            }
            #endregion

            #region 其它支出
            string[] hideForth = Utils.GetFormValues("hideForth");
            if (hideForth != null && hideForth.Count() > 0)
            {
                IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> list = new List<EyouSoft.Model.FinanceStructure.OtherOutInfo>();
                for (int i = 0; i < hideForth.Count(); i++)
                {
                    EyouSoft.Model.FinanceStructure.OtherOutInfo model = new EyouSoft.Model.FinanceStructure.OtherOutInfo();
                    string id = hideForth[i];

                    //支出金额
                    string hideOutIncome = Utils.GetFormValue("hideOutIncome_" + id);
                    //增加费用
                    string outAdd = Utils.GetFormValue("txtOutAdd_" + id);
                    //减少费用
                    string outLess = Utils.GetFormValue("txtOutLess_" + id);
                    //备注
                    string outRemarks = Utils.GetFormValue("txtOutRemarks_" + id);

                    model.OutId = id;
                    model.Amount = Utils.GetDecimal(hideOutIncome);
                    model.AddAmount = Utils.GetDecimal(outAdd);
                    model.ReduceAmount = Utils.GetDecimal(outLess);
                    model.TotalAmount = model.Amount + model.AddAmount - model.ReduceAmount;
                    model.Remark = outRemarks;
                    model.TourId = tourId;
                    list.Add(model);
                }
                int count = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo).UpdateOtherOut(list);
                if (count <= 0)
                {
                    isUpdate = false;
                }
            }
            #endregion

            #region 利润分配
            string[] hideFifth = Utils.GetFormValues("hideFifth");
            if (hideFifth != null && hideFifth.Count() > 0)
            {
                IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list = new List<EyouSoft.Model.FinanceStructure.TourProfitShareInfo>();
                for (int i = 0; i < hideFifth.Count(); i++)
                {
                    EyouSoft.Model.FinanceStructure.TourProfitShareInfo model = new EyouSoft.Model.FinanceStructure.TourProfitShareInfo();

                    string id = hideFifth[i];

                    //分配金额
                    string shareCost = Utils.GetFormValue("txtShareCost_" + id);
                    //备注
                    string profitAll = Utils.GetFormValue("txtProfitRemarks_" + id);

                    model.ShareId = id;
                    model.ShareCost = Utils.GetDecimal(shareCost);
                    model.CompanyId = SiteUserInfo.CompanyID;
                    model.TourId = tourId;
                    model.Remark = profitAll;
                    model.CompanyId = SiteUserInfo.CompanyID;

                    list.Add(model);
                }
                int count = new EyouSoft.BLL.FinanceStructure.TourProfitSharer(SiteUserInfo).UpdateTourShare(list);
                if (count <= 0)
                {
                    isUpdate = false;
                }
            }

            if (isUpdate)
            {
                Utils.ShowAndRedirect("修改成功!", Request.Url.ToString());
            }
            else
            {
                Utils.ShowAndRedirect("修改失败!", Request.Url.ToString());
            }
            #endregion
        }

        public int getType(int type)
        {
            switch (type)
            {
                case 1: return type;
                case 2: return type;
                default: return 0;
            }
        }
    }
}
