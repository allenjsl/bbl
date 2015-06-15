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

using EyouSoft.Common;
using Eyousoft.Common.Page;
using System.Collections.Generic;

namespace Web.jipiao
{
    /// <summary>
    /// 机票管理退票页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    public partial class JiPiao_tuipiao : BackPage
    {
        public string CardType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.机票管理_机票管理_退票操作))
            {
                EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.机票管理_机票管理_退票操作, false);
                return;
            }

            if (!CheckGrant(global::Common.Enum.TravelPermission.机票管理_机票管理_取消退票))//没取消退票权限隐藏取消退票操作按钮
            {
                lbtnQuXiaoTuiPiao.Visible = false;
            }

            if (!IsPostBack)
            {
                onInit();
            }
        }

        //数据初始化
        private void onInit()
        {
            //退票状态绑定
            TuiPiaoStateBind();
            //数据证件类型绑定
            CusCardTypeBind();
            int id = Utils.GetInt(Utils.GetQueryStringValue("id"));
            //根据id得到详细数据

            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();
            EyouSoft.Model.TourStructure.CustomerRefund model = bll.GetRefundTicketModel(id);

            #region 数据初始化
            if (model != null)
            {
                //线路名称
                this.lblLineName.Text = model.RouteName;
                //团号
                this.lblTuanHao.Text = model.TourNo;
                //人数
                this.lblPersonCount.Text = "1";
                //退票须知
                this.txtTuiPiaoMust.Text = model.RefundNote;
                //退回金额
                this.txtTuiMoney.Text = Utils.FilterEndOfTheZeroString(model.RefundAmount.ToString());
                if (model.IsRefund == EyouSoft.Model.EnumType.PlanStructure.TicketRefundSate.已退票)
                {
                    this.ddlJiPiaoState.SelectedValue = "4";
                    this.ddlJiPiaoState.Enabled = false;
                }
                else
                {
                    this.ddlJiPiaoState.SelectedValue = "1";
                }
                #region 航段与退票名单绑定
                //获得该游客退票信息
                this.rptList.DataSource = new EyouSoft.BLL.PlanStruture.PlaneTicket().GetRefundTicketFlights(model.Id);
                this.rptList.DataBind();

                this.txtCusName.Value = model.VisitorName;
                this.txtCardNumber.Value = model.CradNumber;
                CardType = model.CradType.ToString();
                #endregion
            }
            #endregion

        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected void lbtnSave_Click(object sender, EventArgs e)
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
            int ticketID = Utils.GetInt(Utils.GetQueryStringValue("id"));
            //EyouSoft.Model.PlanStructure.TicketRefundModel refundModel = new EyouSoft.Model.PlanStructure.TicketRefundModel();
            EyouSoft.Model.PlanStructure.MRefundTicketInfo mRefundTicketInfo = new EyouSoft.Model.PlanStructure.MRefundTicketInfo();
            //退票须知
            mRefundTicketInfo.Remark = this.txtTuiPiaoMust.Text;
            // refundModel.Remark = this.txtTuiPiaoMust.Text;
            //退票状态,如果没有回传,说明是已退票
            if (String.IsNullOrEmpty(Utils.GetFormValue("ddlJiPiaoState")))
            {
                mRefundTicketInfo.Status = EyouSoft.Model.EnumType.PlanStructure.TicketRefundSate.已退票;
                //refundModel.State = EyouSoft.Model.EnumType.PlanStructure.TicketState.已退票;
            }
            else
            {
                mRefundTicketInfo.Status = (EyouSoft.Model.EnumType.PlanStructure.TicketRefundSate)Utils.GetInt(Utils.GetFormValue("ddlJiPiaoState"));
            }
            //退回金额
            mRefundTicketInfo.RefundAmount = Utils.GetDecimal(this.txtTuiMoney.Text);
            //refundModel.ReturnMoney = Utils.GetDecimal(this.txtTuiMoney.Text);
            mRefundTicketInfo.TicketListId = ticketID;
            mRefundTicketInfo.OperatorId = SiteUserInfo.ID;
            //refundModel.TicketId = ticketID;
            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();

            if (bll.RefundTicket(mRefundTicketInfo))
            {
                printSuccMsg("退票成功!");
            }
            else
            {
                printFaiMsg("退票失败!");
            }
        }

        //输出成功消息
        private void printSuccMsg(string msg)
        {
            EyouSoft.Common.Function.MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();{2}", msg, Utils.GetQueryStringValue("iframeId"), "window.parent.location.reload();"));
        }

        //输出失败消息
        private void printFaiMsg(string msg)
        {
            EyouSoft.Common.Function.MessageBox.Show(this, msg);
        }

        private void TuiPiaoStateBind()
        {
            System.Collections.Generic.List<EnumObj> list = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketRefundSate));
            this.ddlJiPiaoState.DataTextField = "Text";
            this.ddlJiPiaoState.DataValueField = "Value";
            this.ddlJiPiaoState.DataSource = list;
            this.ddlJiPiaoState.DataBind();
            this.ddlJiPiaoState.Items.Insert(0, new ListItem("请选择", ""));
        }

        private void CusCardTypeBind()
        {
            string cardType = "";
            System.Collections.Generic.List<EnumObj> list = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.CradType));
            if (list != null && list.Count > 0)
            {
                cardType += "{value:\"-1\",text:\"--请选择--\"}|";
                for (int i = 0; i < list.Count; i++)
                {
                    cardType += "{value:\"" + list[i].Value + "\",text:\"" + list[i].Text + "\"}|";
                }
                cardType = cardType.TrimEnd('|');
            }
            this.hd_cardType.Value = cardType;
        }

        /// <summary>
        /// 取消退票按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnQuXiaoTuiPiao_Click(object sender, EventArgs e)
        {
            int jiPiaoGuanLiId = Utils.GetInt(Utils.GetQueryStringValue("id"));

            if (jiPiaoGuanLiId < 1) { RegisterAlertAndReloadScript("未知异常"); return; }

            if (new EyouSoft.BLL.PlanStruture.PlaneTicket().QuXiaoTuiPiao(CurrentUserCompanyID, jiPiaoGuanLiId) == 1)
            {
                RegisterScript(string.Format("alert(\"取消退票成功\");parent.Boxy.getIframeDialog('{0}').hide();window.parent.location.href=window.parent.location.href;", Utils.GetQueryStringValue("iframeId")));
            }
            else
            {
                RegisterAlertAndReloadScript("取消退票失败，请重试。");
            }
        }
    }
}
