using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Eyousoft.Common.Page;
using EyouSoft.Common;
namespace Web.sales
{
    /// <summary>
    /// 订单退团编辑页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    public partial class Tuituan : BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_订单中心_退团操作))
            {
                EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_订单中心_退团操作, false);
                return;
            }
            if (!IsPostBack)
            {
                OnInit();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        //游客信息初始化
        private void OnInit()
        {
            //游客ID
            string cusID = EyouSoft.Common.Utils.GetQueryStringValue("cusID").Trim();
            this.lblTuanHao.Text = EyouSoft.Common.Utils.GetQueryStringValue("tourNo");
            if (!string.IsNullOrEmpty(cusID))
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.TourStructure.CustomerLeague TuiTuanModel = TourOrderBll.GetLeague(cusID);
                if (TuiTuanModel != null)
                {
                    this.txtMoney.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal((TuiTuanModel.RefundAmount));
                    this.txtReson.Text = TuiTuanModel.RefundReason;
                    TourOrderBll = null;
                    TuiTuanModel = null;
                }
            }
        }

        //游客退团保存
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
            //游客ID
            string cusID = EyouSoft.Common.Utils.GetQueryStringValue("cusID");
            decimal money = EyouSoft.Common.Utils.GetDecimal(this.txtMoney.Text);
            //原因
            string reson = this.txtReson.Text;
            EyouSoft.Model.TourStructure.CustomerLeague CustomerModel = new EyouSoft.Model.TourStructure.CustomerLeague();
            
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
            if (!string.IsNullOrEmpty(cusID))
            {
                EyouSoft.Model.TourStructure.CustomerLeague model = TourOrderBll.GetLeague(cusID);
                if (model != null && model.CustormerId == cusID)
                {
                    CustomerModel.CustormerId = cusID;
                    CustomerModel.OperatorID = SiteUserInfo.ID;
                    CustomerModel.OperatorName = SiteUserInfo.UserName;
                    CustomerModel.RefundAmount = money;
                    CustomerModel.RefundReason = reson;
                    if (TourOrderBll.UpdateLeague(CustomerModel))
                    {
                        printSuccMsg("退团修改成功!");
                    }
                    else
                    {
                       printFaiMsg("退团失败!");
                    }
                }
                else
                {
                    CustomerModel.CustormerId = cusID;
                    CustomerModel.IssueTime = DateTime.Now;
                    CustomerModel.OperatorID = SiteUserInfo.ID;
                    CustomerModel.OperatorName = SiteUserInfo.UserName;
                    CustomerModel.RefundAmount = money;
                    CustomerModel.RefundReason = reson;
                    if (TourOrderBll.AddLeague(CustomerModel))
                    {
                        printSuccMsg("退团成功!");
                    }
                    else
                    {
                        printFaiMsg("退团失败!");
                    }
                }
            }
            CustomerModel = null;
            TourOrderBll = null;
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
    }
}
