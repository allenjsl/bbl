using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Eyousoft.Common.Page;
using System.Drawing;
using EyouSoft.Common;
using System.Collections.Generic;

namespace Web.sales
{
    /// <summary>
    /// 订单退票编辑页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// 2、2011-04-22 万俊 修改
    /// </summary>
    public partial class Tuipiao : BackPage
    {
        private string html;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_订单中心_退票申请))
            {
                EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_订单中心_退票申请, false);
                return;
            }
            if (!IsPostBack)
            {
                OnInit();
            }
        }

        protected void lkBtnSave_Click(object sender, EventArgs e)
        {
            #region 判断是否提交财务
            EyouSoft.Model.TourStructure.TourBaseInfo m = new EyouSoft.BLL.TourStructure.Tour().GetTourInfo(Utils.GetQueryStringValue("tourid"));
            if (m != null)
            {
                if (!Utils.PlanIsUpdateOrDelete(m.Status.ToString()))
                {
                    Response.Write("<script>alert('该团已提交财务，不能对它操作!');location.href=location.href;</script>");
                    return;
                }
            }
            #endregion
            //取得页面上选择的航段集合
            string[] strs = this.planIds.Value.Split('|');
            //游客ID
            string cusID = EyouSoft.Common.Utils.GetQueryStringValue("cusID");
            //退票编号
            string RefundId = EyouSoft.Common.Utils.GetQueryStringValue("RefundId");
            if (!string.IsNullOrEmpty(cusID))
            {

                //退票后重新出票
                bool isChuPiao = false;//this.chkBox.Checked;
                //退票须知
                string must = this.txtTuiPiaoMust.Text;
                EyouSoft.Model.TourStructure.CustomerRefund TuiPiaoModel = new EyouSoft.Model.TourStructure.CustomerRefund();
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();

                //添加退票记录
                if (!string.IsNullOrEmpty(cusID))
                {
                    EyouSoft.Model.TourStructure.CustomerRefund model = TourOrderBll.GetCustomerRefund(RefundId);
                    //编号
                    TuiPiaoModel.Id = System.Guid.NewGuid().ToString();
                    //操作员ID
                    TuiPiaoModel.OperatorID = SiteUserInfo.ID;
                    //操作员名字
                    if (SiteUserInfo.ContactInfo != null)
                    {
                        TuiPiaoModel.OperatorName = SiteUserInfo.ContactInfo.ContactName;
                    }
                    //游客编号
                    TuiPiaoModel.CustormerId = cusID;
                    //退票时间
                    TuiPiaoModel.IssueTime = DateTime.Now;
                    //退票需知
                    TuiPiaoModel.RefundNote = must;
                    //退票状态
                    TuiPiaoModel.IsRefund = EyouSoft.Model.EnumType.PlanStructure.TicketRefundSate.已退票;
                    //已退航段集合_遍历IDS取出
                    IList<EyouSoft.Model.TourStructure.MCustomerRefundFlight> list = new List<EyouSoft.Model.TourStructure.MCustomerRefundFlight>();
                    for (int i = 0; i < strs.Length - 1; i++)
                    {
                        EyouSoft.Model.TourStructure.MCustomerRefundFlight fight = new EyouSoft.Model.TourStructure.MCustomerRefundFlight();
                        fight.FlightId = Utils.GetInt(strs[i]);
                        list.Add(fight);
                    }
                    //游客已退航段信息
                    TuiPiaoModel.CustomerRefundFlights = list;
                    string tourID = "";
                    if (TourOrderBll.AddCustomerRefund(TuiPiaoModel, ref tourID))
                    {
                        if (isChuPiao)
                        {
                            //跳转到申请机票页面
                            EyouSoft.Common.Function.MessageBox.ResponseScript(this.Page, string.Format(";alert('退票成功,系统将转入机票审请页面');window.parent.location='/sanping/SanPing_JiPiaoAdd.aspx?tourid={1}&cusId={2}';window.parent.Boxy.getIframeDialog('{0}').hide();", EyouSoft.Common.Utils.GetQueryStringValue("iframeId"), tourID, cusID));
                        }
                        printSuccMsg("退票保存成功,待审核!");
                    }
                    else
                    {
                        printFaiMsg("退票保存失败!");
                    }
                }
                TuiPiaoModel = null;
                TourOrderBll = null;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        //输出成功消息
        private void printSuccMsg(string msg)
        {
            EyouSoft.Common.Function.MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();{2}", msg, EyouSoft.Common.Utils.GetQueryStringValue("iframeId"), "window.parent.location.reload();"));
        }
        //输出失败消息
        private void printFaiMsg(string msg)
        {
            EyouSoft.Common.Function.MessageBox.Show(this, msg);
        }

        //游客信息初始化
        private void OnInit()
        {
            //游客ID
            string cusID = EyouSoft.Common.Utils.GetQueryStringValue("cusID");
            //退票编号
            string RefundId = EyouSoft.Common.Utils.GetQueryStringValue("RefundId");
            if (!string.IsNullOrEmpty(cusID))
            {
                BindPlanInfo(cusID);
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.TourStructure.CustomerRefund TuiPiaoModel = TourOrderBll.GetCustomerRefund(RefundId);
                this.lblOPer.Text = SiteUserInfo.ContactInfo.ContactName;
                if (TuiPiaoModel != null)
                {
                    this.txtTuiPiaoMust.Text = TuiPiaoModel.RefundNote;
                }
            }
        }
        //绑定航段信息的数据
        private void BindPlanInfo(string cusID)
        {
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
            EyouSoft.Model.TourStructure.MCustomerAllFlight fight = TourOrderBll.GetCustomerAllFlight(cusID);
            //调用取得航段信息集合
            IList<EyouSoft.Model.PlanStructure.TicketFlight> list = DisponseFightList(fight);
            //绑定数据.
            if (list != null && list.Count > 0)
            {
                this.rpTicket.DataSource = list;
                this.rpTicket.DataBind();
                //repeater显示
                this.rpTicket.Visible = true;
                //无数据提示Label隐藏
                this.lblMsg.Visible = false;
            }
            else
            {
                //无数据提示Label显示
                this.lblMsg.Visible = true;
                //赋值
                this.lblMsg.Text = "未查找到相应的航段信息";
                //隐藏repeater
                this.rpTicket.Visible = false;
                //设置label控件的字体颜色
                this.lblMsg.ForeColor = Color.BlueViolet;

            }

        }

        //处理航段list数据
        protected IList<EyouSoft.Model.PlanStructure.TicketFlight> DisponseFightList(EyouSoft.Model.TourStructure.MCustomerAllFlight fight)
        {
            IList<EyouSoft.Model.PlanStructure.TicketFlight> list = new List<EyouSoft.Model.PlanStructure.TicketFlight>();
            //判断查询所得的航段信息是否为null
            if (fight != null)
            {
                //判断航段信息集合的数量是否大于0
                if (fight.TicketFlights.Count > 0)
                {
                    //判断以退航段信息集合的数量是否大于0
                    if (fight.RefundFlights.Count > 0)
                    {
                        //遍历所有的航段
                        foreach (EyouSoft.Model.PlanStructure.MTicketFlightAndState item in fight.TicketFlights)
                        {
                            if (item != null && item.Status == EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票)
                            {
                                //为ticketId字段设置标识符'true','false'
                                item.TicketId = item.TicketId + "_false";
                                foreach (EyouSoft.Model.TourStructure.MCustomerRefundFlight refFight in fight.RefundFlights)
                                {
                                    //如果所有航段中有已退航段则设置ticketid的标识符为true
                                    if (item.ID == refFight.FlightId)
                                    {
                                        item.TicketId = item.TicketId.Split('_')[0];
                                        item.TicketId = item.TicketId + "_true";
                                    }
                                }
                                //将数据添加到集合中
                                list.Add(item);
                            }
                        }
                        return list;
                    }
                    else
                    {
                        //如果该fight对象中的已退航段信息集合的数量为0则直接将所有的ticketid的标识符设置为'false'
                        foreach (EyouSoft.Model.PlanStructure.MTicketFlightAndState item in fight.TicketFlights)
                        {
                            if (item != null && item.Status == EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票)
                            {

                                item.TicketId = item.TicketId + "_false";
                                list.Add(item);
                            }
                        }


                        return list;
                    }
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }
        //根据ticket的状态来呈现checkBox
        public string RefResult(string str, string id)
        {
            if (str != null && str != "")
            {
                //如果标识符为'true',则返回的html的checkbox设置为不可用,已选择.
                if (str.Split('_')[1] == "true")
                {
                    html = "<input type=\"checkbox\" class=\"check\" disabled=\"disabled\" checked=\"checked\" id=select_" + id + " />";
                }
                else
                {
                    html = "<input type=\"checkbox\" class=\"check\" id=select_" + id + " />";
                }
            }

            return html;
        }
    }
}
