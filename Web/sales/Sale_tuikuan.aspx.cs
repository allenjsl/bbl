using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.sales
{
    /// <summary>
    /// 销售列表退款页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    public partial class Sale_tuikuan : BackPage
    {
        public string tuikuaiList = "";

        private string OrderID = "";

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            #region 取消审批
            if (Utils.GetQueryStringValue("requestType") == "ajax_cancel")
            {
                QuXiaoTuiKuanShenHe();
            }
            #endregion
            //判断权限
            //if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_退款登记))
            //{
            //    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_销售收款_退款登记, false);
            //    return;
            //}
            string State = EyouSoft.Common.Utils.GetQueryStringValue("State");
            //订单ID
            OrderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            //编辑状态
            if (State == "EDIT")
            {
                TuiKuanEdit();
            }else
            if (State == "PASS")
            {
                TuiKuanPass();
            }
            //删除状态
            else if (State == "DEL")
            {
                TuiKuanDel();
            }
            if (!IsPostBack)
            {
                onInit();
            }
        }

        // 退款数据初始化
        private void onInit()
        {
            TuiKuaiType();

            if (Utils.GetQueryStringValue("act") == "pass")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_退款登记))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款收入_退款登记, false);
                    return;
                    lbtnSave.Visible = false;
                }
            }
            else
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_退款登记))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_销售收款_退款登记, false);
                    return;
                    lbtnSave.Visible = false;
                }
            }
            

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(this.SiteUserInfo);
            System.Collections.Generic.IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> Ilist = TourOrderBll.GetRefund(OrderID);
            if (Ilist != null && Ilist.Count > 0)
            {
                foreach (EyouSoft.Model.FinanceStructure.ReceiveRefund receiveModel in Ilist)
                {
                    stringBuilder.Append("<tr class=\"even\">");
                    stringBuilder.Append("<td height=\"30\" align=\"center\">");
                    stringBuilder.AppendFormat("<input id=\"date\" onfocus=\"WdatePicker()\" class=\"searchinput\" name=\"date\" type=\"text\" value=\"{0}\"/>", Convert.ToDateTime(receiveModel.RefundDate).ToShortDateString());
                    stringBuilder.Append("</td><td align=\"center\">");
                    stringBuilder.AppendFormat("<input id=\"tuikuaiPerson\" class=\"searchinput\" name=\"tuikuaiPerson\" type=\"text\" value=\"{0}\"/>", receiveModel.StaffName);
                    stringBuilder.Append("</td><td align=\"center\">");
                    stringBuilder.AppendFormat("<input id=\"tuikuaiMoney\" class=\"searchinput\" name=\"tuikuaiMoney\" MaxLength=\"10\" valid=\"required|isMoney\" errmsg=\"请写退款金额!|请填写正确的退款金额!\" type=\"text\" value=\"{0}\"/>", Utils.FilterEndOfTheZeroDecimal(receiveModel.RefundMoney));
                    stringBuilder.Append("</td><td align=\"center\">");
                    #region 退款方式
                    switch (receiveModel.RefundType)
                    {
                        case EyouSoft.Model.EnumType.TourStructure.RefundType.财务现收:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" selected=\"selected\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        case EyouSoft.Model.EnumType.TourStructure.RefundType.财务现付:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\" selected=\"selected\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        case EyouSoft.Model.EnumType.TourStructure.RefundType.导游现收:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\" selected=\"selected\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        case EyouSoft.Model.EnumType.TourStructure.RefundType.银行电汇:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\" selected=\"selected\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        case EyouSoft.Model.EnumType.TourStructure.RefundType.转账支票:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\" selected=\"selected\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        case EyouSoft.Model.EnumType.TourStructure.RefundType.导游现付:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\" selected=\"selected\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        case EyouSoft.Model.EnumType.TourStructure.RefundType.签单挂账:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\" selected=\"selected\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        case EyouSoft.Model.EnumType.TourStructure.RefundType.预存款支付:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\" selected=\"selected\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        case EyouSoft.Model.EnumType.TourStructure.RefundType.银行到账_私人:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\" selected=\"selected\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        default:
                            {
                                stringBuilder.Append("<select name=\"ddlShouStyle\" valid=\"required\" errmsg=\"请选择收款方式!\">");
                                stringBuilder.Append("<option value=\"-1\" selected=\"selected\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">财务现收</option>");
                                stringBuilder.Append("<option value=\"2\">财务现付</option>");
                                stringBuilder.Append("<option value=\"3\">导游现收</option>");
                                stringBuilder.Append("<option value=\"4\">银行电汇</option>");
                                stringBuilder.Append("<option value=\"5\">转账支票</option>");
                                stringBuilder.Append("<option value=\"6\">导游现付</option>");
                                stringBuilder.Append("<option value=\"7\">签单挂账</option>");
                                stringBuilder.Append("<option value=\"8\">预存款支付</option>");
                                stringBuilder.Append("<option value=\"9\">银行到账_私人</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                    }
                    #endregion
                    stringBuilder.Append("</td><td align=\"center\">");
                    #region 是否开票
                    switch (receiveModel.IsBill)
                    {
                        case true:
                            {
                                stringBuilder.Append("<select id=\"ddlkaiStyle\" name=\"ddlkaiStyle\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" selected=\"selected\">是</option>");
                                stringBuilder.Append("<option value=\"0\">否</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        case false:
                            {
                                stringBuilder.Append("<select id=\"ddlkaiStyle\" name=\"ddlkaiStyle\">");
                                stringBuilder.Append("<option value=\"-1\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">是</option>");
                                stringBuilder.Append("<option value=\"0\" selected=\"selected\">否</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                        default:
                            {
                                stringBuilder.Append("<select id=\"ddlTuiStyle\" name=\"ddlTuiStyle\">");
                                stringBuilder.Append("<option value=\"-1\" selected=\"selected\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">是</option>");
                                stringBuilder.Append("<option value=\"0\">否</option>");
                                stringBuilder.Append("</select>");
                                break;
                            }
                    }
                    #endregion
                    stringBuilder.Append("</td><td align=\"center\">");
                    stringBuilder.AppendFormat("<input id=\"kaipiaoMoney\" class=\"searchinput\" name=\"kaipiaoMoney\" MaxLength=\"10\" valid=\"isMoney\" errmsg=\"请填写正确的开票金额!\" type=\"text\" value=\"{0}\"/>", Utils.FilterEndOfTheZeroDecimal(receiveModel.BillAmount));
                    stringBuilder.Append("</td><td align=\"center\">");
                    stringBuilder.AppendFormat("<textarea rows=\"2\" cols=\"20\" MaxLength=\"500\" name=\"memo\">{0}</textarea>", receiveModel.Remark);
                    stringBuilder.Append("</td><td align=\"center\">");
                    #region 退款记录状态，是否审核,如果已审核，就不显示修改与删除操作。
                    switch (receiveModel.IsCheck)
                    {
                        case true:
                            {
                                stringBuilder.Append("<span style=\"color:red\">已审核</span>");
                                if (Utils.GetQueryStringValue("act") == "pass" && CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_取消退款审核))
                                {
                                    stringBuilder.AppendFormat("<span style=\"color:red\">&nbsp;&nbsp;<a href=\"javascript:void(0)\" onclick=\"cancelChecked('{0}')\">取消审核</a></span>", receiveModel.Id);
                                }
                                break;
                            }
                        case false:
                            {
                                if (Utils.GetQueryStringValue("act") == "pass")
                                {
                                    if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_退款审核))
                                    {
                                        stringBuilder.AppendFormat("<a id='pass' name='pass' href=\"javascript:void(0)\" onclick=\"passTuiKuai('{0}','{1}',this)\">审核</a>&nbsp;", receiveModel.Id, OrderID);
                                    }
                                    if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_退款登记))
                                    {
                                        lbtnSave.Visible=false;
                                    }
                                }
                                else
                                {
                                    if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_退款登记))
                                    {
                                        lbtnSave.Visible = false;
                                    }
                                }
                                stringBuilder.AppendFormat("<a id=\"update\" name=\"update\" onclick=\"editTuiKuai('{0}','{1}',$(this))\" href=\"javascript:void(0)\">修改&nbsp;</a></span>", receiveModel.Id, OrderID);
                                stringBuilder.AppendFormat("<a id=\"del\" name=\"del\" onclick=\"delTuiKuai('{0}','{1}')\" href=\"javascript:void(0)\">删除</a></span>", receiveModel.Id, OrderID);
                                break;
                            }
                    }
                    #endregion
                    stringBuilder.Append(" </td></tr>");
                }
                tuikuaiList = stringBuilder.ToString();
                stringBuilder = null;
                TourOrderBll = null;
                Ilist = null;
            }
        }

        //输出消息
        private void printMsg(string msg)
        {
            EyouSoft.Common.Function.MessageBox.Show(this.Page, msg);
        }

        /// <summary>
        /// 检查数据输入
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="date"></param>
        /// <param name="operID"></param>
        /// <param name="money"></param>
        /// <param name="shouKuanStyle"></param>
        /// <returns></returns>
        private bool checkData(ref string msg, DateTime tuiKuaiDate, string tuiKuaiPerson, decimal tuiKuaiMoney, decimal kaiPiaoMoney, EyouSoft.Model.EnumType.TourStructure.RefundType tuiKuaiStyle)
        {
            if (tuiKuaiDate == DateTime.MinValue)
            {
                msg = "请填写正确的日期";
                return false;
            }
            if (tuiKuaiPerson == "")
            {
                msg = "请填写退款人";
                return false;
            }
            if (tuiKuaiMoney == -1)
            {
                msg = "请填写有效的退款金额";
                return false;
            }
            if ((int)tuiKuaiStyle == -1)
            {
                msg = "请选择退款方式";
                return false;
            }
            if (kaiPiaoMoney == -1)
            {
                msg = "请输入正确的开票金额";
                return false;
            }
            return true;
        }

        //绑定收款方式
        private void TuiKuaiType()
        {
            System.Collections.Generic.List<EnumObj> list = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType));
            this.ddlTuiKuaiStyle.DataTextField = "Text";
            this.ddlTuiKuaiStyle.DataValueField = "Value";
            this.ddlTuiKuaiStyle.DataSource = list;
            this.ddlTuiKuaiStyle.DataBind();
            this.ddlTuiKuaiStyle.Items.Insert(0, new ListItem("请选择", ""));
        }

        //删除退款记录
        private void TuiKuanDel()
        {
            if (Utils.GetQueryStringValue("act") == "pass")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_退款登记))
                {
                    RCWE("-1");
                }
            }
            else
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_退款登记))
                {
                    RCWE("-1");
                }
            }

            //要删除的退款记录ID
            string RID = EyouSoft.Common.Utils.GetQueryStringValue("RID");
            if (!string.IsNullOrEmpty(RID))
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                if (TourOrderBll.DeleteRefund(CurrentUserCompanyID, RID,OrderID, EyouSoft.Model.EnumType.TourStructure.LogRefundSource.销售管理_销售收款))
                {
                    //onInit();
                    TourOrderBll = null;
                    Response.Write("删除成功");
                    Response.End();
                }
                else
                {
                    //onInit();
                    TourOrderBll = null;
                    Response.Write("删除失败");
                    Response.End();
                }
            }
        }
        //审核
        private void TuiKuanPass()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_退款审核))
            {
                RCWE("-1");
            }

            string msg = "";
            #region 获得Get值
            //当前记录ID
            string rid = EyouSoft.Common.Utils.GetQueryStringValue("RID");
            //订单ID
            string orderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            //退款日期
            DateTime tuiKuaiDate = Utils.GetDateTime(EyouSoft.Common.Utils.GetQueryStringValue("TuiKuaiDate"));
            //退款人
            string tuiKuaiPerson = EyouSoft.Common.Utils.GetQueryStringValue("TuiKuaiPerson");
            //退款金额
            decimal tuiKuaiMoney = Utils.GetDecimal(EyouSoft.Common.Utils.GetQueryStringValue("TuiKuaiMoney"), -1);
            //退款方式
            EyouSoft.Model.EnumType.TourStructure.RefundType tuiKuaiStyle = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetQueryStringValue("TuiKuaiStyle"), -1);
            //是否开票
            bool isKaiPiao = EyouSoft.Common.Utils.GetQueryStringValue("KaiPiaoStyle") == "1" ? true : false;
            //开票金额
            decimal kaiPiaoMoney = Utils.GetDecimal(EyouSoft.Common.Utils.GetQueryStringValue("KaiPiaoMoney"), -1);
            //备注
            string memo = Utils.GetQueryStringValue("Memo");
            #endregion
            #region 修改退款记录
            if (checkData(ref msg, tuiKuaiDate, tuiKuaiPerson, tuiKuaiMoney,kaiPiaoMoney,tuiKuaiStyle))
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                EyouSoft.Model.FinanceStructure.ReceiveRefund ReceiveRefundModel = new EyouSoft.Model.FinanceStructure.ReceiveRefund();

                ReceiveRefundModel.Id = rid;
                ReceiveRefundModel.ItemId = orderID;
                ReceiveRefundModel.ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单;
                ReceiveRefundModel.CompanyID = SiteUserInfo.CompanyID;
                ReceiveRefundModel.PayCompanyId = 0;
                ReceiveRefundModel.PayCompanyName = "";
                ReceiveRefundModel.OperatorID = SiteUserInfo.ID;
                ReceiveRefundModel.RefundDate = tuiKuaiDate;
                ReceiveRefundModel.StaffName = tuiKuaiPerson;
                ReceiveRefundModel.RefundMoney = tuiKuaiMoney;
                ReceiveRefundModel.RefundType = tuiKuaiStyle;
                ReceiveRefundModel.IsBill = isKaiPiao;
                ReceiveRefundModel.BillAmount = kaiPiaoMoney;
                ReceiveRefundModel.Remark = memo;
                ReceiveRefundModel.IsReceive = false;
                ReceiveRefundModel.IsCheck = true;
                ReceiveRefundModel.OperatorID = SiteUserInfo.ID;
                ReceiveRefundModel.CheckerId = SiteUserInfo.ID;
                if (TourOrderBll.UpdateRefund(ReceiveRefundModel, OrderID, EyouSoft.Model.EnumType.TourStructure.LogRefundSource.财务管理_团款收入))
                {
                    //onInit();
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    Response.Write("审核成功");
                    Response.End();
                }
                else
                {
                    //onInit();
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    Response.Write("审核失败");
                    Response.End();
                }

            }
            else
            {
                Response.Write(msg);
                Response.End();
            }
            #endregion
        }
        //修改退款记录
        private void TuiKuanEdit()
        {
            if (Utils.GetQueryStringValue("act") == "pass")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_退款登记))
                {
                    RCWE("-1");
                }
            }
            else
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_退款登记))
                {
                    RCWE("-1");
                }
            }

            string msg = "";
            #region 获得Get值
            //当前记录ID
            string rid = EyouSoft.Common.Utils.GetQueryStringValue("RID");
            //订单ID
            string orderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            //退款日期
            DateTime tuiKuaiDate = Utils.GetDateTime(EyouSoft.Common.Utils.GetQueryStringValue("TuiKuaiDate"));
            //退款人
            string tuiKuaiPerson = EyouSoft.Common.Utils.GetQueryStringValue("TuiKuaiPerson");
            //退款金额
            decimal tuiKuaiMoney = Utils.GetDecimal(EyouSoft.Common.Utils.GetQueryStringValue("TuiKuaiMoney"), -1);
            //退款方式
            EyouSoft.Model.EnumType.TourStructure.RefundType tuiKuaiStyle = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetQueryStringValue("TuiKuaiStyle"), -1);
            //是否开票
            bool isKaiPiao = EyouSoft.Common.Utils.GetQueryStringValue("KaiPiaoStyle") == "1" ? true : false;
            //开票金额
            decimal kaiPiaoMoney = Utils.GetDecimal(EyouSoft.Common.Utils.GetQueryStringValue("KaiPiaoMoney"), -1);
            //备注
            string memo = Utils.GetQueryStringValue("Memo");
            #endregion
            #region 修改退款记录
            if (checkData(ref msg, tuiKuaiDate, tuiKuaiPerson, tuiKuaiMoney,kaiPiaoMoney,tuiKuaiStyle))
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                EyouSoft.Model.FinanceStructure.ReceiveRefund ReceiveRefundModel = new EyouSoft.Model.FinanceStructure.ReceiveRefund();
                ReceiveRefundModel.Id = rid;
                ReceiveRefundModel.ItemId = orderID;
                ReceiveRefundModel.ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单;
                ReceiveRefundModel.CompanyID = SiteUserInfo.CompanyID;
                ReceiveRefundModel.PayCompanyId = 0;
                ReceiveRefundModel.RefundDate = tuiKuaiDate;
                ReceiveRefundModel.StaffName = tuiKuaiPerson;
                ReceiveRefundModel.RefundMoney = tuiKuaiMoney;
                ReceiveRefundModel.RefundType = tuiKuaiStyle;
                ReceiveRefundModel.IsBill = isKaiPiao;
                ReceiveRefundModel.BillAmount = kaiPiaoMoney;
                ReceiveRefundModel.Remark = memo;
                ReceiveRefundModel.IsReceive = false;
                if (TourOrderBll.UpdateRefund(ReceiveRefundModel, OrderID, EyouSoft.Model.EnumType.TourStructure.LogRefundSource.销售管理_销售收款))
                {
                    //onInit();
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    Response.Write("修改成功");
                    Response.End();
                }
                else
                {
                    //onInit();
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    Response.Write("修改失败");
                    Response.End();
                }
            }
            else
            {
                Response.Write(msg);
                Response.End();
            }
            #endregion
        }

        //添加退款记录
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            string msg = "";
            #region 获得添加值
            //订单ID
            string orderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            //退款日期
            DateTime tuiKuaiDate = Utils.GetDateTime(this.txtTuiKuaiDate.Text.Trim());
            //退款人
            string tuiKuaiPerson = this.txtTuiKuaiPerson.Text.Trim();
            //退款金额
            decimal tuiKuaiMoney = Utils.GetDecimal(this.txtTuiKuaiMoney.Text.Trim(), -1);
            //退款方式
            EyouSoft.Model.EnumType.TourStructure.RefundType tuiKuaiStyle = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetFormValue(ddlTuiKuaiStyle.UniqueID), -1);
            //是否开票
            bool isKaiPiao = this.ddlKaiPiaoStyle.SelectedValue == "1" ? true : false;
            //开票金额
            decimal kaiPiaoMoney = Utils.GetDecimal(this.txtKaiPiaoMoney.Text.Trim(),0);
            //备注
            string memo = Utils.GetQueryStringValue("Memo");
            #endregion
            #region 添加退款记录
            if (checkData(ref msg, tuiKuaiDate, tuiKuaiPerson, tuiKuaiMoney,kaiPiaoMoney,tuiKuaiStyle))
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                EyouSoft.Model.FinanceStructure.ReceiveRefund ReceiveRefundModel = new EyouSoft.Model.FinanceStructure.ReceiveRefund();

                ReceiveRefundModel.Id = System.Guid.NewGuid().ToString();
                ReceiveRefundModel.ItemId = orderID;
                ReceiveRefundModel.CompanyID = this.SiteUserInfo.CompanyID;
                ReceiveRefundModel.ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单;
                ReceiveRefundModel.RefundDate = DateTime.Now;
                ReceiveRefundModel.OperatorID = SiteUserInfo.ID;
                ReceiveRefundModel.PayCompanyId = 0;

                ReceiveRefundModel.RefundDate = tuiKuaiDate;
                ReceiveRefundModel.StaffName = tuiKuaiPerson;
                ReceiveRefundModel.RefundMoney = tuiKuaiMoney;
                ReceiveRefundModel.RefundType = tuiKuaiStyle;
                ReceiveRefundModel.IsBill = isKaiPiao;
                ReceiveRefundModel.BillAmount = kaiPiaoMoney;
                ReceiveRefundModel.Remark = this.txtMemo.Text;
                ReceiveRefundModel.IsReceive = false;

                if (TourOrderBll.AddRefund(ReceiveRefundModel, OrderID, EyouSoft.Model.EnumType.TourStructure.LogRefundSource.销售管理_销售收款))
                {
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    #region 添加数据初始化
                    this.txtTuiKuaiDate.Text = "";
                    this.txtTuiKuaiPerson.Text = "";
                    this.txtTuiKuaiMoney.Text = "";
                    ddlKaiPiaoStyle.SelectedValue = "-1";
                    txtKaiPiaoMoney.Text = "";
                    txtMemo.Text = "";
                    #endregion
                    onInit();
                    printMsg("添加成功");
                }
                else
                {
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    onInit();
                    printMsg("添加失败");
                }
            }
            else
            {
                onInit();
                printMsg(msg);
            }
            #endregion
        }

        /// <summary>
        /// 取消退款款审核
        /// </summary>
        void QuXiaoTuiKuanShenHe()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_取消退款审核))
            {
                RCWE("-1");
            }

            string checkedId = Utils.GetFormValue("cancelId");
            string orderId = Utils.GetFormValue("orderId");
            int companyId = SiteUserInfo.CompanyID;
            if (string.IsNullOrEmpty(checkedId) || string.IsNullOrEmpty(orderId))
            {
                RCWE("-1");
            }

            if (new EyouSoft.BLL.TourStructure.TourOrder().CancelIncomeTuiChecked(companyId, orderId, checkedId))
            {
                RCWE("0");
            }

            RCWE("-2");
        }
    }
}
