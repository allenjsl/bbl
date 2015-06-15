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
    /// 出纳登帐-销账弹出框 
    /// 田想兵
    /// </summary>
    public partial class chunaxiaozhang : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页面变量I
        /// </summary>
        public int i = 0;
        /// <summary>
        /// 页面初始绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_出纳登帐_销售销账))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_出纳登帐_销售销账, false);
                }
                Bind();
            }
        }
        /// <summary>
        /// 列表绑定
        /// </summary>
        void Bind()
        {
            int BuyCompanyId = 0;
            BuyCompanyId = Utils.GetInt(hd_comId.Value);
            int RecordCount = 0;
            EyouSoft.BLL.TourStructure.TourOrder order = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            IList<EyouSoft.Model.TourStructure.TourOrder> list = order.GetOrderList(20, EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1), ref RecordCount, CurrentUserCompanyID, Utils.GetInt(Request.QueryString["id"]), BuyCompanyId);

            rpt_list.DataSource = list;
            rpt_list.DataBind();
            #region 分页
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = RecordCount;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion
        }
        /// <summary>
        /// 销账操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            string[] chk = Utils.GetFormValues("chk");
            EyouSoft.BLL.FinanceStructure.CashierRegister bll = new EyouSoft.BLL.FinanceStructure.CashierRegister(SiteUserInfo);
            IList<EyouSoft.Model.FinanceStructure.CancelRegistInfo> list = new List<EyouSoft.Model.FinanceStructure.CancelRegistInfo>();
            for (int i = 0; i < chk.Length; i++)
            {
                EyouSoft.Model.FinanceStructure.CancelRegistInfo model = new EyouSoft.Model.FinanceStructure.CancelRegistInfo();
                model.CompanyId = Utils.GetInt(Utils.GetFormValues("hd_comId")[Utils.GetInt(chk[i])]);
                model.Money = Utils.GetDecimal(Utils.GetFormValues("txt_money")[Utils.GetInt(chk[i])]);
                model.OrderId = Utils.GetFormValues("hd_orderId")[Utils.GetInt(chk[i])];
                list.Add(model);
            }
            int j = bll.CancelRegist(Utils.GetInt(Request.QueryString["id"]), SiteUserInfo.ID, SiteUserInfo.UserName, list);
            if (j > 0)
            {
                Response.Write("<script>alert('销账成功');location.href=location.href;parent.location.href='chunadz_list.aspx';</script>");
            }
            else
            {
                Response.Write("<script>alert('销账失败');location.href=location.href;</script>");
            }
            Bind();
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Bind();
        }
    }
}
