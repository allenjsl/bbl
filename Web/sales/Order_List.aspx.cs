using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using EyouSoft.Common;
using Eyousoft.Common.Page;
using EyouSoft.BLL.TourStructure;

namespace Web.sales
{
    /// <summary>
    /// 订单列表页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    /// 2、修改记录
    ///    修改时间：2011-5-31
    ///    修改人：柴逸宁
    ///    添加客户单位详细页面链接
    /// 3、修改记录
    ///    修改时间：2011-7-5
    ///    修改人：柴逸宁
    ///    修改内容：改变“不受理”状态文字显示方式
    public partial class OrderList : BackPage
    {
        #region 分页参数
        protected int pageSize = 20;
        protected int pageIndex = 1;
        int recordCount;
        #endregion

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.general;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_订单中心_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_订单中心_栏目, false);
                return;
            }
            if (!IsPostBack)
            {
                //页面初始化
                onInit();
            }
        }

        /// <summary>
        /// 查询参数初始化
        /// </summary>
        private void onInit()
        {
            //订单号
            string orderLot = Utils.GetQueryStringValue("orderLot");
            //团号
            string tuanHao = Utils.GetQueryStringValue("tuanHao");
            //客户单位
            string cusHao = Utils.GetQueryStringValue("cusHao");
            //操作人ID
            int[] operID = ConvertToIntArray(Utils.GetQueryStringValue("operId").Split(','));
            //出团开始日期
            DateTime? chuTuanStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("chuTuanStart"));
            //出团开始日期
            DateTime? chuTuanEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("chuTuanEnd"));
            //下单开始时间
            DateTime? orderStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("orderStart"));
            //下单开始时间
            DateTime? orderEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("orderEnd"));
            int status = Utils.GetInt(Utils.GetQueryStringValue("status"));
            //设置订单选中
            OrderStateInit(status.ToString());

            EyouSoft.Model.EnumType.TourStructure.OrderState? OrderState =null;
            if (status > 0)
                OrderState = (EyouSoft.Model.EnumType.TourStructure.OrderState)status;
            //当前页
            int curPage = Utils.GetInt(Utils.GetQueryStringValue("Page"));
            if (curPage != 0) { pageIndex = curPage; }
            this.inputChuTuanStartDate.Value = chuTuanStartDate == null ? "" : Convert.ToDateTime(chuTuanStartDate).ToString("yyyy-MM-dd");
            this.inputChuTuanEndDate.Value = chuTuanEndDate == null ? "" : Convert.ToDateTime(chuTuanEndDate).ToString("yyyy-MM-dd");
            this.inputOrderLot.Value = orderLot;
            this.inputTuanHao.Value = tuanHao;
            this.inputCus.Value = cusHao;
            this.inputOrderStartDate.Value = orderStartDate == null ? "" : Convert.ToDateTime(orderStartDate).ToString("yyyy-MM-dd");
            this.inputOrderEndDate.Value = orderEndDate == null ? "" : Convert.ToDateTime(orderEndDate).ToString("yyyy-MM-dd");
            this.selectOperator1.OperId = Utils.GetQueryStringValue("operId");
            this.selectOperator1.OperName = Utils.GetQueryStringValue("operName");

            //订单列表数据绑定
            ListBind(orderLot, tuanHao, cusHao, operID, chuTuanStartDate, chuTuanEndDate, orderStartDate, orderEndDate,OrderState, ref recordCount);
        }

        /// <summary>
        /// 订单列表数据绑定
        /// </summary>
        /// <param name="orderLot">订单号</param>
        /// <param name="tuanHao">团号</param>
        /// <param name="cusHao">客户单位</param>
        /// <param name="operIds">操作人ID</param>
        /// <param name="tDate">出团日期</param>
        /// <param name="orderDate">下单时间</param>
        /// <param name="recordCount">总记录数，地址传递</param>
        protected void ListBind(string orderLot, string tuanHao, string cusHao, int[] operId, DateTime? chuTuanStartDate, DateTime? chuTuanEndDate, DateTime? orderStartDate, DateTime? orderEndDate, EyouSoft.Model.EnumType.TourStructure.OrderState? OrderState, ref int recordCount)
        {
            #region　订单列表绑定
            TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(this.SiteUserInfo);
            EyouSoft.Model.TourStructure.OrderCenterSearchInfo searchModel = new EyouSoft.Model.TourStructure.OrderCenterSearchInfo();
            searchModel.OrderNo = orderLot;
            searchModel.TourNo = tuanHao;
            searchModel.CompanyName = cusHao;
            searchModel.OperatorId = operId;
            searchModel.LeaveDateFrom = chuTuanStartDate;
            searchModel.LeaveDateTo = chuTuanEndDate;
            searchModel.CreateDateFrom = orderStartDate;
            searchModel.CreateDateTo = orderEndDate;
            searchModel.OrderState = OrderState;
            searchModel.OrderId = Utils.GetQueryStringValue("orderid");

            System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrder> Ilist = TourOrderBll.GetOrderList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, searchModel);
            if (Ilist != null && Ilist.Count > 0)
            {
                repList.DataSource = Ilist;
                repList.DataBind();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
                this.repList.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='12' align='center'>暂时没有数据！</td></tr>";
            }
            #endregion
            TourOrderBll = null;
            searchModel = null;
            BindPage();
        }

        /// <summary>
        /// 分页控件绑定
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }

        protected void repList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting CompanyBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            EyouSoft.Model.TourStructure.TourOrder model = (EyouSoft.Model.TourStructure.TourOrder)e.Item.DataItem;
            HtmlInputHidden hd_XingChengDan = (HtmlInputHidden)e.Item.FindControl("hd_XingChengDan");
            if (model.TourClassId == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
            {
                if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                {
                    hd_XingChengDan.Value = CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划标准发布行程单) + "?tourid=" + model.TourId;
                }
                else if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    hd_XingChengDan.Value = CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划快速发布行程单) + "?tourid=" + model.TourId;
                }
            }
            else if (model.TourClassId == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
            {
                if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                {
                    hd_XingChengDan.Value = CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划标准发布行程单) + "?tourid=" + model.TourId;
                }
                else if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    hd_XingChengDan.Value = CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划快速发布行程单) + "?tourid=" + model.TourId;
                }
            }
            ((HtmlInputHidden)e.Item.FindControl("hd_PrintMingDan")).Value = CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.订单中心打印名单) + "?orderid=" + model.ID;
            CompanyBll = null;
            model = null;
        }

        //将字符串数组转化成整型数组
        private int[] ConvertToIntArray(string[] source)
        {
            int[] to = new int[source.Length];
            for (int i = 0; i < source.Length; i++)//将全部的数字存到数组里。
            {
                if (!string.IsNullOrEmpty(source[i].ToString()))
                {
                    to[i] = Utils.GetInt(source[i].ToString());
                }
            }
            if (to[0] == 0)
            {
                return null;
            }
            return to;
        }

        #region 确认单
        protected string GetPrintUrl(string TourType,string Tourid,string OrderId)
        {
            string Url = "";
            //声明bll对象
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            if (TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划.ToString())
            {
                Url = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散客确认单) + "?Tourid=" + Tourid + "&orderId=" + OrderId;
            }
            if (TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划.ToString())
            {
                Url = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队确认单) + "?Tourid=" + Tourid + "";
            }
            return Url;
        }

        protected string GetSettledPrintUrl(string TourType, string Tourid, string OrderId)
        {
            string Url = "";
            //声明bll对象
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            Url = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.结算明细单) + "?Tourid=" + Tourid + "&orderId=" + OrderId;          
            return Url;
        }
        #endregion

        /// <summary>
        /// 获得订单状态
        /// </summary>
        /// <param name="selectIndex"></param>
        protected void OrderStateInit(string selectIndex)
        {
            System.Collections.Generic.IList<EyouSoft.Common.EnumObj> stateList = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.OrderState));
            if (stateList != null && stateList.Count > 0)
            {
                this.ddlOrderState.Items.Clear();
                this.ddlOrderState.Items.Add(new ListItem("--请选择--", "0"));
                for (int i = 0; i < stateList.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = stateList[i].Value;
                    item.Text = stateList[i].Text == "不受理" ? "已取消" : stateList[i].Text;
                    //设置选中行
                    if (selectIndex == item.Value)
                    {
                        item.Selected = true;
                    }
                    //添加行
                    this.ddlOrderState.Items.Add(item);
                }
            }
        }
    }
}
