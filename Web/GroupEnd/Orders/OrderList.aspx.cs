using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Function;

namespace Web.GroupEnd.Orders
{
    /// <summary>
    /// 模块名称:组团端订单列表(包括订单查询,线路负责人信息,地接社信息，报名，打印等模块)
    /// 创建时间:2011-01-19
    /// 创建人:lixh
    /// 修改人:曹胡生
    /// </summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011-06-21
    /// 修改内容：
    /// 1、联系人点击查看联系人信息
    /// 2、去掉地接社信息列
    /// 3、点击线路名称，要可以查看行程
    public partial class OrderList : Eyousoft.Common.Page.FrontPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数

        #endregion
        private string OrderPrintPath = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting CompanyBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            OrderPrintPath = CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团订单打印名单);
            this.selectXianlu1.Url = "/GroupEnd/Orders/OrderList.aspx";

            #region 定义变量接受参数
            //操作
            string act = EyouSoft.Common.Utils.GetQueryStringValue("act");
            //继续留位id
            string ID = EyouSoft.Common.Utils.GetQueryStringValue("ID");
            //线路区域
            string OrderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            //线路名称
            string LineName = EyouSoft.Common.Utils.GetQueryStringValue("LineName");
            //出团开始时间
            DateTime? StartTime = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("BeginTime"));
            //出团结束时间
            DateTime? EndTime = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("EndTime"));
            //订单下单开始时间
            DateTime? OrderStartDate = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("OrderStartDate"));
            //订单下单结束时间
            DateTime? OrderEndDate = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("OrderEndDate"));
            //线路区域ID
            int? xlid = EyouSoft.Common.Utils.GetIntNull(EyouSoft.Common.Utils.GetQueryStringValue("xlid"));
            #endregion
            #region 初始化变量参数
            if (OrderID != null)
            {
                this.TxtOrderID.Value = OrderID;
            }
            if (LineName != "")
            {
                this.xl_XianlName.Value = LineName.ToString();
            }
            if (StartTime != null)
            {
                this.BeginTime.Value = Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd");
            }
            if (EndTime != null)
            {
                this.EndTime.Value = Convert.ToDateTime(EndTime).ToString("yyyy-MM-dd");
            }
            if (OrderStartDate != null)
            {
                this.inputOrderStartDate.Value = Convert.ToDateTime(OrderStartDate).ToString("yyyy-MM-dd");
            }
            if (OrderEndDate != null)
            {
                this.inputOrderEndDate.Value = Convert.ToDateTime(OrderEndDate).ToString("yyyy-MM-dd");
            }
            #endregion
            if (act == "updateContinue")
            {
                //订单信息业务逻辑类
                EyouSoft.BLL.TourStructure.TourOrder BllTourOrder = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                //订单信息实体类
                EyouSoft.Model.TourStructure.TourOrder TourOrderModel = BllTourOrder.GetOrderModel(SiteUserInfo.CompanyID, ID);
                TourOrderModel.LastDate = DateTime.Now;
                TourOrderModel.OrderState = EyouSoft.Model.EnumType.TourStructure.OrderState.未处理;
                string msg = string.Empty;
                switch (BllTourOrder.Update(TourOrderModel))
                {
                    case 0:
                        {
                            TourOrderModel = null;
                            msg = "留位失败";
                            break;
                        }
                    case 1:
                        {
                            TourOrderModel = null;

                            msg = "留位成功";
                            break;
                        }
                    case 2:
                        {
                            TourOrderModel = null;
                            msg = "该团队的订单总人数+当前订单人数大于团队计划人数总和";
                            break;
                        }
                    case 3:
                        {
                            TourOrderModel = null;
                            msg = "该客户所欠金额大于最高欠款金额";
                            break;
                        }
                }
                MessageBox.ShowAndRedirect(this, msg, "/GroupEnd/Orders/OrderList.aspx");
            }
            else
            {

                BindLineInfo(OrderID, LineName, StartTime, EndTime, OrderStartDate, OrderEndDate, xlid);
            }
        }

        #region 线路列表信息
        /// <summary>
        /// 绑定订单信息列表
        /// </summary>
        protected void BindLineInfo(string OrderID, string LineName, DateTime? StarTime, DateTime? EndTime, DateTime? OrderStartDate, DateTime? OrderEndDate, int? xlid)
        {

            EyouSoft.Model.TourStructure.TourOrderSearchInfo TourOrderSearchInfo = new EyouSoft.Model.TourStructure.TourOrderSearchInfo();
            //当前页数
            PageIndex = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("Page"), 1);
            //订单编号
            TourOrderSearchInfo.OrderNo = OrderID;
            //线路名称
            TourOrderSearchInfo.RouteName = LineName;
            //出团开始日期
            TourOrderSearchInfo.LeaveDateFrom = StarTime;
            //出团结束日期
            TourOrderSearchInfo.LeaveDateTo = EndTime;
            //订单开始时间
            TourOrderSearchInfo.CreateDateFrom = OrderStartDate;
            //订单结束时间
            TourOrderSearchInfo.CreateDateTo = OrderEndDate;
            //线路区域ID
            TourOrderSearchInfo.AreaId = xlid;
            //订单信息业务逻辑类
            EyouSoft.BLL.TourStructure.TourOrder BllTourOrder = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            //订单信息实体类
            IList<EyouSoft.Model.TourStructure.TourOrder> TourOrderlist = new List<EyouSoft.Model.TourStructure.TourOrder>();
            //获取组团端订单中心订单信息集合
            TourOrderlist = BllTourOrder.GetTourOrderList(PageSize, PageIndex, ref RecordCount, SiteUserInfo.TourCompany.TourCompanyId, TourOrderSearchInfo);
            if (TourOrderlist != null && TourOrderlist.Count > 0)
            {
                this.rep_List.DataSource = TourOrderlist;
                this.rep_List.DataBind();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
                this.rep_List.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='11' align='center'>暂时没有数据！</td></tr>";
            }
            TourOrderSearchInfo = null;
            BllTourOrder = null;
            TourOrderlist = null;
            BinPage();

        }
        #endregion

        #region 绑定总人数
        protected int PelpeoCount(string ChildNumber, string ChengrenNumber)
        {
            return EyouSoft.Common.Utils.GetInt(ChildNumber) + EyouSoft.Common.Utils.GetInt(ChengrenNumber);
        }
        #endregion

        #region 设置分页
        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="LineType">线路类型</param>
        /// <param name="LineName">线路名称</param>
        /// <param name="StarTime">开始发布时间</param>
        /// <param name="EndTime">结束发布时间</param>
        /// <param name="DateNumber">出团天数</param>
        /// <param name="Author">发布人</param>
        protected void BinPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
        #endregion

        protected void rep_List_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string[] data = e.CommandArgument.ToString().Split(',');
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
            if (e.CommandName == "Del")
            {
                if (TourOrderBll.Delete(data[0], SiteUserInfo.CompanyID, Convert.ToInt32(data[1])))
                {
                    EyouSoft.Common.Function.MessageBox.ShowAndRedirect(this.Page, "删除成功!", "/GroupEnd/Orders/OrderList.aspx");
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.Show(this.Page, "删除失败!");
                }
            }
        }

        protected void rep_List_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EyouSoft.Model.TourStructure.TourOrder model = (EyouSoft.Model.TourStructure.TourOrder)e.Item.DataItem;
                if (model.OrderType == EyouSoft.Model.EnumType.TourStructure.OrderType.组团下单)
                {

                    if (model.OrderState == EyouSoft.Model.EnumType.TourStructure.OrderState.未处理)
                    {
                        ((LinkButton)e.Item.FindControl("lbtnCancel")).Visible = true;
                        ((LinkButton)e.Item.FindControl("lbtnCancel")).CommandArgument = model.ID + "," + model.RouteId.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(OrderPrintPath))
                {
                    ((HyperLink)e.Item.FindControl("hyMinDan")).NavigateUrl = OrderPrintPath + "?orderid=" + model.ID;
                }
            }
        }

        //过滤小数点后的多余0
        public string FilterEndOfTheZeroDecimal(object o)
        {
            return EyouSoft.Common.Utils.FilterEndOfTheZeroString(o.ToString());
        }

        //格式化联系人列表
        public string FormatContactInfo(object o)
        {
            System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOperator> list = (System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOperator>)o;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (EyouSoft.Model.TourStructure.TourOperator tourOperator in list)
            {
                if (tourOperator.ContactTel == "")
                {
                    str.AppendFormat("【{0}】", tourOperator.ContactName);
                }
                else
                {
                    str.AppendFormat("【{0},{1}】", tourOperator.ContactName, tourOperator.ContactTel);
                }                
            }
            return str.ToString();
        }
    }
}
