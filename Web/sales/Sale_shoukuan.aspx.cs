using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.sales
{
    public partial class Sale_shoukuan : BackPage
    {
        /// <summary>
        /// 销售列表收款页面
        /// 修改记录：
        /// 1、2011-01-12 曹胡生 创建
        /// </summary>
        protected string shoukuaiList = "";
        //订单ID
        private string OrderID = "";

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 取消收款审核
            if (Utils.GetQueryStringValue("requestType") == "ajax_cancel")
            {
                QuXiaoShouKuanShenHe();
            }
            #endregion

            //判断权限
            //if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_收款登记))
            //{
            //    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_销售收款_收款登记, false);
            //    return;
            //}
            string State = EyouSoft.Common.Utils.GetQueryStringValue("State");
            //订单ID
            OrderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            //编辑状态
            if (State == "EDIT")
            {
                //if(isEdit())
                ShouKuanEdit();
            }
            //审核状态
            else if (State == "PASS")
            {
                //if (isEdit())
                PassKuan();
            }
            //删除状态
            else if (State == "DEL")
            {
                //if (isEdit())
                ShouKuanDel();
            }

            if (!IsPostBack)
            {
                //绑定收款类型
                onInit();
            }
        }
        bool isEdit()
        {

            #region 判断是否提交财务
            EyouSoft.Model.TourStructure.TourBaseInfo m = new EyouSoft.BLL.TourStructure.Tour().GetTourInfo(Utils.GetQueryStringValue("tourid"));
            if (m != null)
            {
                if (!Utils.PlanIsUpdateOrDelete(m.Status.ToString()))
                {
                    Response.Clear();
                    Response.Write("该团已提交财务，不能对它操作!");
                    Response.End();
                    return false;
                }
                else
                    return true;
            }
            else
                return true;
            #endregion
        }
        // 退款数据初始化
        private void onInit()
        {
            ShouKuaiType();
            if (Utils.GetQueryStringValue("act") == "pass")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_收款登记))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款收入_收款登记, false);
                    return;
                }
            }
            else
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_收款登记))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_销售收款_收款登记, false);
                    return;
                }
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(this.SiteUserInfo);
            System.Collections.Generic.IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> Ilist = TourOrderBll.GetReceive(OrderID);
            if (Ilist != null && Ilist.Count > 0)
            {
                foreach (EyouSoft.Model.FinanceStructure.ReceiveRefund receiveModel in Ilist)
                {
                    stringBuilder.Append("<tr class=\"even\">");
                    stringBuilder.Append("<td height=\"30\" align=\"center\">");
                    stringBuilder.AppendFormat("<input id=\"date\"  class=\"searchinput\" name=\"date\" type=\"text\" valid=\"required\" errmsg=\"请写正确的日期!\" onfocus=\"WdatePicker()\" value=\"{0}\"/>", Convert.ToDateTime(receiveModel.RefundDate).ToShortDateString());
                    stringBuilder.Append("</td><td align=\"center\">");
                    stringBuilder.AppendFormat("<input id=\"tuikuaiPerson\" class=\"searchinput\" name=\"shoukuaiPerson\" valid=\"required\" errmsg=\"请写收款人!\" type=\"text\" value=\"{0}\"/>", receiveModel.StaffName);
                    stringBuilder.Append("</td><td align=\"center\">");
                    stringBuilder.AppendFormat("<input id=\"tuikuaiMoney\" class=\"searchinput\" name=\"shoukuaiMoney\" valid=\"required|isMoney\" MaxLength=\"10\" errmsg=\"请写收款金额!|请填写正确的收款金额!\" type=\"text\" value=\"{0}\"/>", Utils.FilterEndOfTheZeroDecimal(receiveModel.RefundMoney));
                    stringBuilder.Append("</td><td align=\"center\">");
                    #region 收款方式
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
                    stringBuilder.AppendFormat("<input id=\"kaipiaoMoney\" class=\"searchinput\" name=\"kaipiaoMoney\" type=\"text\" valid=\"isMoney\" MaxLength=\"10\" errmsg=\"请填写正确的开票金额!\" value=\"{0}\"/>", Utils.FilterEndOfTheZeroDecimal(receiveModel.BillAmount));
                    stringBuilder.Append("</td><td align=\"center\">");

                    stringBuilder.AppendFormat("<textarea rows=\"2\" cols=\"20\" MaxLength=\"500\" name=\"memo\">{0}</textarea>", receiveModel.Remark);
                    stringBuilder.Append("</td><td align=\"center\">");
                    #region 退款记录状态，是否审核,如果已审核，就不显示修改与删除操作。
                    switch (receiveModel.IsCheck)
                    {
                        case true:
                            {
                                stringBuilder.Append("<span style=\"color:red\">已审核</span>");

                                if (Utils.GetQueryStringValue("act") == "pass"&&CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_取消收款审核))
                                {
                                    stringBuilder.AppendFormat("<span style=\"color:red\">&nbsp;&nbsp;<a href=\"javascript:void(0)\" onclick=\"cancelChecked('{0}')\">取消审核</a></span>", receiveModel.Id);
                                }

                                break;
                            }
                        case false:
                            {
                                if (Utils.GetQueryStringValue("act") == "pass")
                                {

                                    if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_收款审核))
                                    {
                                        stringBuilder.AppendFormat("<a id='pass' name='pass' href=\"javascript:void(0)\" onclick=\"passShouKuai('{0}','{1}',$(this))\">审核</a>&nbsp;", receiveModel.Id, OrderID);
                                    }
                                    if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_收款登记))
                                    {
                                        lbtnSave.Visible = false;
                                    }
                                }
                                else
                                {
                                    if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_收款登记))
                                    {
                                        lbtnSave.Visible = false;
                                    }
                                }
                                stringBuilder.AppendFormat("<a id=\"update\" name=\"update\" onclick=\"editShouKuai('{0}','{1}',$(this))\" href=\"javascript:void(0)\">修改</span>&nbsp;", receiveModel.Id, OrderID);
                                stringBuilder.AppendFormat("<a id=\"del\" name=\"del\" onclick=\"delShouKuai('{0}','{1}')\" href=\"javascript:void(0)\">删除</span>", receiveModel.Id, OrderID);
                                break;
                            }
                    }

                    #endregion
                    stringBuilder.Append(" </td></tr>");
                }
                shoukuaiList = stringBuilder.ToString();
                TourOrderBll = null;
                Ilist = null;
                stringBuilder = null;
            }
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
        private bool checkData(ref string msg, DateTime shouKuaiDate, string shouKuaiPerson, decimal shouKuaiMoney, decimal kaiPiaoMoney, EyouSoft.Model.EnumType.TourStructure.RefundType shouKuaiStyle)
        {
            if (shouKuaiDate == DateTime.MinValue)
            {
                msg = "请填写正确的日期";
                return false;
            }
            if (shouKuaiPerson == "")
            {
                msg = "请填写收款人";
                return false;
            }
            if (shouKuaiMoney == -1)
            {
                msg = "请填写有效的收款金额";
                return false;
            }
            if ((int)shouKuaiStyle == -1)
            {
                msg = "请选择收款方式";
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
        private void ShouKuaiType()
        {
            System.Collections.Generic.List<EnumObj> list = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType));
            this.ddlShouKuaiStyle.DataTextField = "Text";
            this.ddlShouKuaiStyle.DataValueField = "Value";
            this.ddlShouKuaiStyle.DataSource = list;
            this.ddlShouKuaiStyle.DataBind();
            this.ddlShouKuaiStyle.Items.Insert(0, new ListItem("请选择", ""));
        }

        //删除收款记录
        private void ShouKuanDel()
        {
            if (Utils.GetQueryStringValue("act") == "pass")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_收款登记))
                {
                    RCWE("-1");
                }
            }
            else
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_收款登记))
                {
                    RCWE("-1");
                }
            }

            string RID = EyouSoft.Common.Utils.GetQueryStringValue("RID");

            if (string.IsNullOrEmpty(RID))
            {
                RCWE("删除成功");
            }

            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
            bool bllRetCode = TourOrderBll.DeleteReceive(this.SiteUserInfo.CompanyID, RID, OrderID, EyouSoft.Model.EnumType.TourStructure.LogRefundSource.销售管理_销售收款);
            TourOrderBll = null;

            if (bllRetCode)
            {
                RCWE("删除成功");
            }
            else
            {
                RCWE("删除失败");
            }
        }

        //编辑收款记录
        private void ShouKuanEdit()
        {
            if (Utils.GetQueryStringValue("act") == "pass")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_收款登记))
                {
                    RCWE("-1");
                }
            }
            else
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_收款登记))
                {
                    RCWE("-1");
                }
            }

            string msg = "";
            #region 获得添加值
            //订单ID
            string orderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            //收款记录id
            string rid = EyouSoft.Common.Utils.GetQueryStringValue("RID").Trim();
            //收款日期
            DateTime shouKuaiDate = Utils.GetDateTime(Utils.GetQueryStringValue("ShouKuaiDate").Trim());
            //收款人
            string shouKuaiPerson = Utils.GetQueryStringValue("ShouKuaiPerson");
            //收款金额
            decimal shouKuaiMoney = Utils.GetDecimal(Utils.GetQueryStringValue("ShouKuaiMoney").Trim(), -1);
            //收款方式
            EyouSoft.Model.EnumType.TourStructure.RefundType shouKuaiStyle = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetQueryStringValue("ShouKuaiStyle"), -1);
            //是否开票
            bool isKaiPiao = Utils.GetQueryStringValue("KaiPiaoStyle").Trim() == "1" ? true : false;
            //开票金额
            decimal kaiPiaoMoney = Utils.GetDecimal(Utils.GetQueryStringValue("KaiPiaoMoney").Trim(), -1);
            //备注
            string memo = Utils.GetQueryStringValue("Memo");
            #endregion
            #region 修改收款记录
            if (checkData(ref msg, shouKuaiDate, shouKuaiPerson, shouKuaiMoney,kaiPiaoMoney,shouKuaiStyle))
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.FinanceStructure.ReceiveRefund ReceiveRefundModel = new EyouSoft.Model.FinanceStructure.ReceiveRefund();

                ReceiveRefundModel.Id = rid;
                ReceiveRefundModel.ItemId = orderID;
                ReceiveRefundModel.ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单;
                ReceiveRefundModel.CompanyID = SiteUserInfo.CompanyID;
                ReceiveRefundModel.OperatorID = SiteUserInfo.ID;
                ReceiveRefundModel.PayCompanyId = 0;
                ReceiveRefundModel.PayCompanyName = "";
                ReceiveRefundModel.RefundDate = shouKuaiDate;
                ReceiveRefundModel.StaffName = shouKuaiPerson;
                ReceiveRefundModel.RefundMoney = shouKuaiMoney;
                ReceiveRefundModel.RefundType = shouKuaiStyle;
                ReceiveRefundModel.Remark = memo;
                ReceiveRefundModel.IsReceive = true;
                ReceiveRefundModel.IsBill = isKaiPiao;
                ReceiveRefundModel.BillAmount = kaiPiaoMoney;

                if (TourOrderBll.UpdateReceive(ReceiveRefundModel, OrderID, EyouSoft.Model.EnumType.TourStructure.LogRefundSource.销售管理_销售收款))
                {
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    //onInit();
                    Response.Write("修改成功");
                    Response.End();
                }
                else
                {
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    //onInit();
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

        //审核收款记录
        private void PassKuan()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_收款审核))
            {
                RCWE("-1");
            }

            string msg = "";
            #region 获得添加值
            //订单ID
            string orderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            //收款记录id
            string rid = EyouSoft.Common.Utils.GetQueryStringValue("RID").Trim();
            //收款日期
            DateTime shouKuaiDate = Utils.GetDateTime(Utils.GetQueryStringValue("ShouKuaiDate").Trim());
            //收款人
            string shouKuaiPerson = Utils.GetQueryStringValue("ShouKuaiPerson");
            //收款金额
            decimal shouKuaiMoney = Utils.GetDecimal(Utils.GetQueryStringValue("ShouKuaiMoney").Trim(), -1);
            //收款方式
            EyouSoft.Model.EnumType.TourStructure.RefundType shouKuaiStyle = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetQueryStringValue("ShouKuaiStyle"), -1);
            //是否开票
            bool isKaiPiao = Utils.GetQueryStringValue("KaiPiaoStyle").Trim() == "1" ? true : false;
            //开票金额
            decimal kaiPiaoMoney = Utils.GetDecimal(Utils.GetQueryStringValue("KaiPiaoMoney").Trim(), -1);
            //备注
            string memo = Utils.GetQueryStringValue("Memo");
            #endregion
            #region 修改收款记录
            if (checkData(ref msg, shouKuaiDate, shouKuaiPerson, shouKuaiMoney,kaiPiaoMoney, shouKuaiStyle))
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.FinanceStructure.ReceiveRefund ReceiveRefundModel = new EyouSoft.Model.FinanceStructure.ReceiveRefund();

                ReceiveRefundModel.Id = rid;
                ReceiveRefundModel.ItemId = orderID;
                ReceiveRefundModel.ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单;
                ReceiveRefundModel.CompanyID = SiteUserInfo.CompanyID;
                ReceiveRefundModel.PayCompanyId = 0;
                ReceiveRefundModel.PayCompanyName = "";

                ReceiveRefundModel.RefundDate = shouKuaiDate;
                ReceiveRefundModel.StaffName = shouKuaiPerson;
                ReceiveRefundModel.RefundMoney = shouKuaiMoney;
                ReceiveRefundModel.RefundType = shouKuaiStyle;
                ReceiveRefundModel.Remark = memo;
                ReceiveRefundModel.IsReceive = true;
                ReceiveRefundModel.IsBill = isKaiPiao;
                ReceiveRefundModel.BillAmount = kaiPiaoMoney;
                ReceiveRefundModel.IsCheck = true;
                ReceiveRefundModel.OperatorID = SiteUserInfo.ID;
                ReceiveRefundModel.CheckerId = SiteUserInfo.ID;
                if (TourOrderBll.UpdateReceive(ReceiveRefundModel, OrderID, EyouSoft.Model.EnumType.TourStructure.LogRefundSource.财务管理_团款收入))
                {
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    //onInit();
                    Response.Write("审核成功");
                    Response.End();
                }
                else
                {
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    //onInit();
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

        //添加收款记录
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            string msg = "";
            #region 获得添加值
            //订单ID
            string orderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            //收款日期
            DateTime shouKuaiDate = Utils.GetDateTime(this.txtShouKuaiDate.Text.Trim());
            //收款人
            string shouKuaiPerson = this.txtShouKuaiPerson.Text.Trim();
            //收款金额
            decimal shouKuaiMoney = Utils.GetDecimal(this.txtShouKuaiMoney.Text.Trim(), -1);
            //收款方式
            EyouSoft.Model.EnumType.TourStructure.RefundType shouKuaiStyle = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetFormValue(ddlShouKuaiStyle.UniqueID), -1);
            //是否开票
            bool isKaiPiao = this.ddlKaiPiaoStyle.SelectedValue == "1" ? true : false;
            //开票金额
            decimal kaiPiaoMoney = Utils.GetDecimal(this.txtKaiPiaoMoney.Text.Trim(), 0);
            //备注
            string memo = Utils.GetQueryStringValue("memo");
            #endregion
            #region 添加收款记录
            if (checkData(ref msg, shouKuaiDate, shouKuaiPerson, shouKuaiMoney,kaiPiaoMoney,shouKuaiStyle))
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.FinanceStructure.ReceiveRefund ReceiveRefundModel = new EyouSoft.Model.FinanceStructure.ReceiveRefund();

                ReceiveRefundModel.Id = System.Guid.NewGuid().ToString();
                ReceiveRefundModel.ItemId = orderID;
                ReceiveRefundModel.ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单;
                ReceiveRefundModel.CompanyID = SiteUserInfo.CompanyID;
                ReceiveRefundModel.OperatorID = SiteUserInfo.ID;
                ReceiveRefundModel.RefundDate = shouKuaiDate;
                ReceiveRefundModel.StaffName = shouKuaiPerson;
                ReceiveRefundModel.RefundMoney = shouKuaiMoney;
                ReceiveRefundModel.RefundType = shouKuaiStyle;
                ReceiveRefundModel.Remark = this.txtMemo.Text;
                ReceiveRefundModel.IsReceive = true;
                ReceiveRefundModel.IsBill = isKaiPiao;
                ReceiveRefundModel.BillAmount = kaiPiaoMoney;
                ReceiveRefundModel.PayCompanyId = 0;
                ReceiveRefundModel.PayCompanyName = "";

                if (TourOrderBll.AddReceive(ReceiveRefundModel, OrderID, EyouSoft.Model.EnumType.TourStructure.LogRefundSource.销售管理_销售收款))
                {
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    #region 添加数据初始化
                    this.txtShouKuaiDate.Text = "";
                    this.txtShouKuaiPerson.Text = "";
                    this.txtShouKuaiMoney.Text = "";
                    ddlKaiPiaoStyle.SelectedValue = "-1";
                    txtKaiPiaoMoney.Text = "";
                    txtMemo.Text = "";
                    #endregion
                    printMsg("添加成功");
                }
                else
                {
                    TourOrderBll = null;
                    ReceiveRefundModel = null;
                    printMsg("添加失败");
                }
                onInit();
            }
            else
            {
                onInit();
                printMsg(msg);
            }
            #endregion
        }

        //输出消息
        private void printMsg(string msg)
        {
            EyouSoft.Common.Function.MessageBox.Show(this, msg);
        }

        /// <summary>
        /// 取消收款审核
        /// </summary>
        void QuXiaoShouKuanShenHe()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_取消收款审核))
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

            if (new EyouSoft.BLL.TourStructure.TourOrder().CancelIncomeChecked(companyId, orderId, checkedId))
            {
                RCWE("0");
            }

            RCWE("-2");
        }

    }
}
