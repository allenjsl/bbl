/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) TravelSky 2011
/// 模块名称：团支出审核
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
    /// 团支出审核
    /// </summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011-06-21
    /// 修改内容：金额栏添加金额的合计
    /// 修改人：柴逸宁
    /// 修改时间：2011-07-5
    /// 修改内容：增加批量审核和批量支付功能
    public partial class waitkuan : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 合计参数-付款金额
        /// </summary>
        protected decimal paymentAmount = 0;
        /// <summary>
        /// 页面变量I
        /// </summary>
        public int i = 0;
        /// <summary>
        /// 页面初始化绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetOutRegisterList
            #region 审核
            if (Utils.GetQueryStringValue("act") == "pass")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款审批))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_付款审批, false);
                    return;
                }
                EyouSoft.Model.FinanceStructure.OutRegisterInfo model = new EyouSoft.Model.FinanceStructure.OutRegisterInfo();

                EyouSoft.BLL.FinanceStructure.OutRegister bll = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
                int i = bll.SetCheckedState(true, SiteUserInfo.ID, Utils.GetQueryStringValue("id"));
                if (i == 1)
                {
                    Response.Write("<script>alert('审核成功');location.href='waitkuan.aspx';</script>");
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.Show(this.Page, "审核失败");
                }
            }
            #endregion
            #region 批量审核or批量支付
            if (Utils.GetQueryStringValue("act") == "Allpass" || Utils.GetQueryStringValue("act") == "Allpay")
            {
                string act = Utils.GetQueryStringValue("act");
                EyouSoft.BLL.FinanceStructure.BSpendRegister bll = new EyouSoft.BLL.FinanceStructure.BSpendRegister();
                IList<string> ls = new List<string>();
                string[] ids = Utils.GetFormValue("ids").Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    ls.Add(ids[i]);
                }
                bool res = false;
                if (act == "Allpass")
                {
                    res = bll.BatchApprovalExpense(SiteUserInfo.ID, ls) > 0;
                }
                else
                {
                    res = bll.BatchPayExpense(SiteUserInfo.ID, ls) > 0;
                }
                Response.Clear();
                Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
                Response.End();
            }
            #endregion
            #region 支付
            if (Utils.GetQueryStringValue("act") == "pay")
            {

                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_财务支付))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_财务支付, false);
                    return;
                }
                EyouSoft.Model.FinanceStructure.OutRegisterInfo model = new EyouSoft.Model.FinanceStructure.OutRegisterInfo();

                EyouSoft.BLL.FinanceStructure.OutRegister bll = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
                int i = bll.SetIsPay(true, Utils.GetQueryStringValue("id"));
                if (i == 1)
                {
                    Response.Write("<script>alert('支付成功');location.href='waitkuan.aspx';</script>");
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.Show(this.Page, "支付失败");
                }
            }
            #endregion
            #region 绑定
            if (!IsPostBack)
            {

                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_栏目, false);
                }
                BindInfo();
            }
            #endregion
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="sel"></param>
        void BindUserList(string sel)
        {
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);//初始化bll
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> userlist = userBll.GetCompanyUser(CurrentUserCompanyID);
            if (userlist != null && userlist.Count > 0)
            {
                foreach (EyouSoft.Model.CompanyStructure.CompanyUser user in userlist)
                {
                    selOperator.Items.Add(new ListItem(user.PersonInfo.ContactName, user.ID.ToString()));
                }
            }
            selOperator.Items.Insert(0, new ListItem("请选择", "0"));
            //selOperator.Items.FindByValue(sel).Selected = true;
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        void BindInfo()
        {
            string selValue = Utils.GetFormValue(selOperator.UniqueID);
            selValue = selValue == "" ? "0" : selValue;
            BindUserList(selValue);
            IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> list = new
            List<EyouSoft.Model.FinanceStructure.OutRegisterInfo>();
            EyouSoft.BLL.FinanceStructure.OutRegister bll = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
            EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo searchInfo = new EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo();
            int count = 0;
            #region 查询参数
            searchInfo.CompanyId = CurrentUserCompanyID;

            //searchInfo.FTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("date"));
            txt_Date.Value = Utils.GetQueryStringValue("date");
            searchInfo.RegisterType = EyouSoft.Model.EnumType.FinanceStructure.OutRegisterType.团队;
            searchInfo.TourNo = Utils.GetQueryStringValue("teamNum");
            txt_teamNum.Value = searchInfo.TourNo;
            searchInfo.OperatorId = Utils.GetInt(Utils.GetQueryStringValue("Operator"));
            searchInfo.RegisterStatus = Utils.GetIntNull(Utils.GetQueryStringValue("Status"));

            if (selOperator.Items.FindByValue(searchInfo.OperatorId.ToString()) != null)
            {
                selOperator.Items.FindByValue(searchInfo.OperatorId.ToString()).Selected = true;
            }

            if (seleState.Items.FindByValue(searchInfo.RegisterStatus.ToString()) != null)
            {
                seleState.Items.FindByValue(searchInfo.RegisterStatus.ToString()).Selected = true;
            }

            searchInfo.ReceiveCompanyName = Utils.GetQueryStringValue("com");
            txt_com.Value = searchInfo.ReceiveCompanyName;

            searchInfo.StartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("date"));
            searchInfo.EndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("regedate"));
            #endregion
            list = bll.GetOutRegisterList(searchInfo, 20, Utils.GetInt(Utils.GetQueryStringValue("page")), ref count);            
            rpt_list.DataSource = list;
            rpt_list.DataBind();
            bll.GetOutRegisterList(searchInfo, ref paymentAmount);
            #region 分页
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion
            rpt_list.EmptyText = "<tr><td id=\"EmptyData\" colspan='8' bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            //BindInfo();
            string param = "";
            param += "teamNum=" + txt_teamNum.Value;
            param += "&com=" + txt_com.Value;
            param += "&operator=" + Utils.GetFormValue(selOperator.UniqueID);
            param += "&date=" + txt_Date.Value;
            param += "&Status=" + Utils.GetFormValue(seleState.UniqueID);
            param += "&regedate=" + Utils.GetFormValue("txtRegEDate");
            Response.Redirect("waitkuan.aspx?" + param);
        }
        /// <summary>
        /// 列表项操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rpt_list_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "pay")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_财务支付))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_财务支付, false);
                }
                //支付
                EyouSoft.Model.FinanceStructure.OutRegisterInfo model = e.Item.DataItem as EyouSoft.Model.FinanceStructure.OutRegisterInfo;

                EyouSoft.BLL.FinanceStructure.OutRegister bll = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
                int i = bll.SetIsPay(true, model.RegisterId);
                if (i == 1)
                {
                    BindInfo();
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.Show(this.Page, "支付失败");
                }
            }
            if (e.CommandName == "check")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款审批))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_付款审批, false);
                }
                //审核
                EyouSoft.Model.FinanceStructure.OutRegisterInfo model = e.Item.DataItem as EyouSoft.Model.FinanceStructure.OutRegisterInfo;

                EyouSoft.BLL.FinanceStructure.OutRegister bll = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);
                int i = bll.SetCheckedState(true, SiteUserInfo.ID, model.RegisterId);
                if (i == 1)
                {
                    BindInfo();
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.Show(this.Page, "审核失败");
                }
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCheck_Click(object sender, EventArgs e)
        {
            string[] str = Utils.GetFormValues("chk_sel");
            if (str.Length > 0)
            {
                EyouSoft.BLL.FinanceStructure.OutRegister bll = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);

                int i = bll.SetCheckedState(true, SiteUserInfo.ID, str);
                if (i == 1)
                {
                    Response.Write("<script>alert('审核成功');location.href=location.href;</script>");
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.Show(this.Page, "审核失败");
                }
            }
        }
        /// <summary>
        /// 获取操作URL
        /// </summary>
        /// <param name="ischeck"></param>
        /// <param name="ispay"></param>
        /// <param name="id"></param>
        /// <param name="tourid"></param>
        /// <param name="ReceiveId"></param>
        /// <returns></returns>
        public string getUrl(bool ischeck, bool ispay, string id, string tourid, string ReceiveId)
        {
            if (!ispay)
            {
                if (ischeck)
                {

                    if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_财务支付))
                    {
                        return "";
                    }
                    else
                    {
                        //return "<a href=\"waitkuan.aspx?act=pay&id=" + id + "\">支付</a>";
                        return string.Format("<a class=\"pass\" href=\"waitkuanpass.aspx?act=pass&id={0}&tourid={1}&receiveid={2}\">支付</a>", id, tourid, ReceiveId);
                    }
                }
                else
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款审批))
                    {
                        return "";
                    }
                    else
                    {
                        return "<a class=\"pass\" href=\"waitkuanPass.aspx?act=pass&id=" + id + "&tourid=" + tourid + "&ReceiveId=" + ReceiveId + "\">审批</a>";
                    }
                }
            }
            else
            {
                //return "已支付";
                return string.Format("<a class=\"pass\" href=\"waitkuanpass.aspx?act=pass&id={0}&tourid={1}&receiveid={2}\">已支付</a>", id
                    , tourid, ReceiveId);
            }
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkPay_Click(object sender, EventArgs e)
        {
            string[] str = Utils.GetFormValues("chk_sel");
            if (str.Length > 0)
            {
                EyouSoft.BLL.FinanceStructure.OutRegister bll = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo);

                int i = bll.SetIsPay(true, str);
                if (i == 1)
                {
                    Response.Write("<script>alert('支付成功');location.href=location.href;</script>");
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.Show(this.Page, "支付失败");
                }
            }
        }
    }
}
