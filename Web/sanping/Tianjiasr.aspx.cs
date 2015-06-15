/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：SanPing_JiPiaoAdd.aspx.cs
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\SanPing_JiPiaoAdd.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-18 10:09:43
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
using Common.Enum;

namespace Web.sanping
{
    public partial class Tianjiasr : Eyousoft.Common.Page.BackPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //操作类型
                string type = Utils.GetQueryStringValue("type");
                string id = Utils.GetQueryStringValue("id");
                //团队计划
                if (type == "team")
                {
                    //团队计划：团队结算权限判断
                    if (CheckGrant(TravelPermission.团队计划_团队计划_栏目))
                    {
                        if (!CheckGrant(TravelPermission.团队计划_团队计划_团队结算))
                        {
                            Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_团队结算, false);
                        }
                    }
                    else
                    {
                        Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_栏目, false);
                    }

                }
                else
                    //财务管理
                    if (type == "account")
                    {
                        //财务管理：权限判断
                        if (CheckGrant(TravelPermission.财务管理_团队核算_栏目))
                        {
                            if (!CheckGrant(TravelPermission.财务管理_团队核算_其它收入添加))
                            {
                                Utils.ResponseNoPermit(TravelPermission.财务管理_团队核算_其它收入添加, false);
                            }
                        }
                        else
                        {
                            Utils.ResponseNoPermit(TravelPermission.财务管理_团队核算_栏目, false);
                        }
                    }
                    else
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
                        //杂费收入
                        else
                        {
                            if (CheckGrant(TravelPermission.财务管理_杂费收入_栏目))
                            {
                                if (!CheckGrant(TravelPermission.财务管理_杂费收入_收入登记))
                                {
                                    Utils.ResponseNoPermit(TravelPermission.财务管理_杂费收入_收入登记, false);
                                }
                            }
                            else
                            {
                                Utils.ResponseNoPermit(TravelPermission.财务管理_杂费收入_栏目, false);
                            }
                        }

                if (id != "")
                {
                    BindInfo(id);
                }
                else
                {
                    DdlPayTypeInit("");
                }
            }
        }
        /// <summary>
        /// 绑定信息
        /// </summary>
        /// <param name="id"></param>
        void BindInfo(string id)
        {
            EyouSoft.BLL.FinanceStructure.OtherCost bll = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo);
            EyouSoft.Model.FinanceStructure.OtherIncomeInfo model = bll.GetOtherIncomeInfo(id);
            if (model != null)
            {
                txt_date.Text = model.PayTime.ToString("yyyy-MM-dd");
                txt_com.Value = model.CustromCName;
                txt_ziqu.Text = model.Item;
                txt_sendUser.Text = model.Payee;
                SiteUserInfo.ID = model.OperatorId;
                txt_Remarks.Value = model.Remark;
                txt_money.Value = Utils.FilterEndOfTheZeroDecimal(model.TotalAmount);
                this.txtAddMoney.Value = model.AddAmount.ToString("0.00");
                this.txtCutMoney.Value = model.ReduceAmount.ToString("0.00");
                DdlPayTypeInit(((int)model.PayType).ToString());
                if (model.Status) ddlStatus.SelectedIndex = 0;
                else ddlStatus.SelectedIndex = 1;
            }
        }
        /// <summary>
        /// 弹窗初始
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
                base.OnPreInit(e);
            }
        }

        #region 新增、修改
        /// <summary>
        /// 按钮保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtn_Submit_Click(object sender, EventArgs e)
        {

            EyouSoft.BLL.FinanceStructure.OtherCost bll = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo);
            EyouSoft.Model.FinanceStructure.OtherIncomeInfo model = null;
            #region 获取初始数据
            if (Utils.GetQueryStringValue("id") != "")
            {
                model = bll.GetOtherIncomeInfo(Request.QueryString["id"]);
                if (model == null)
                {
                    Utils.ShowAndRedirect("无该条记录信息!", Request.Url.ToString());
                    return;
                }
            }
            else
            {
                model = new EyouSoft.Model.FinanceStructure.OtherIncomeInfo();
            }
            model.CompanyId = CurrentUserCompanyID;
            model.CustromCName = txt_com.Value;
            model.Item = txt_ziqu.Text;
            model.OperatorId = SiteUserInfo.ID;
            model.Remark = txt_Remarks.Value;
            model.CreateTime = DateTime.Now;
            model.PayTime = Utils.GetDateTime(this.txt_date.Text);
            model.Amount = Utils.GetDecimal(txt_money.Value);
            model.Payee = this.txt_sendUser.Text.Trim();
            model.AddAmount = Utils.GetDecimal(this.txtAddMoney.Value);
            model.ReduceAmount = Utils.GetDecimal(this.txtCutMoney.Value);
            model.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetFormValue(this.ddlPayType.UniqueID));
            model.TotalAmount = model.Amount + model.AddAmount - model.ReduceAmount;
            model.Status = Utils.GetFormValue(this.ddlStatus.UniqueID).Trim() == "True";
            #endregion
            #region 提交数据库
            if (Utils.GetQueryStringValue("id") != "")
            {
                model.IncomeId = Request.QueryString["id"];
                bll.UpdateOtherIncome(model);
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('修改成功!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.window.location.reload();");
            }
            else
            {
                model.TourId = Request.QueryString["tourid"];
                bll.AddOtherIncome(model);
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('添加成功!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.window.location.reload();");
            }
            #endregion
        }
        #endregion

        #region 初始化支出方式
        /// <summary>
        /// 支付方式初始化
        /// </summary>
        /// <param name="selectIndex"></param>
        protected void DdlPayTypeInit(string selectIndex)
        {
            this.ddlPayType.Items.Clear();
            this.ddlPayType.Items.Add(new ListItem("--请选择--", "0"));
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

    }
}
