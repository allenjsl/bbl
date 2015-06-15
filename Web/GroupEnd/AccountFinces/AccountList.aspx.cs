using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.GroupEnd.AccountFinces
{
    /// <summary>
    /// 描述:组团端财管管理列表页面
    /// 修改记录:
    /// 1. 2011-02-25 AM 曹胡生 创建
    /// </summary>
    public partial class AccountList : Eyousoft.Common.Page.FrontPage
    {
        #region 分页参数
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                #region 接受参数
                //团号
                string TourNo = EyouSoft.Common.Utils.GetQueryStringValue("TourCode");
                //线路名称
                string LineName = EyouSoft.Common.Utils.GetQueryStringValue("LineName");
                //下单人
                string OrderOper = EyouSoft.Common.Utils.GetQueryStringValue("Operator");
                //对方销售员
                string Sales = EyouSoft.Common.Utils.GetQueryStringValue("Salesperson");
                //状态
                string State = EyouSoft.Common.Utils.GetQueryStringValue("State");
                //当前页
                PageIndex = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("page"), 1);
                #endregion

                #region 初始化变量参数
                this.Txt_TourCode.Value = TourNo;
                this.Txt_LineName.Value = LineName;
                //此处为下单人
                this.Txt_operator.Value = OrderOper;
                this.Txt_salesperson.Value = Sales;
                this.Ddl_State.SelectedValue = State;
                #endregion
                BindAccountList(TourNo, LineName, OrderOper, Sales, State);
            }
        }

        #region 绑定财务信息列表
        protected void BindAccountList(string TourNo, string LineName, string OrderOper, string Sales, string State)
        {
            EyouSoft.Model.TourStructure.TourFinanceSearchInfo searchModel = new EyouSoft.Model.TourStructure.TourFinanceSearchInfo();
            searchModel.TourNo = TourNo;
            searchModel.RouteName = LineName;
            searchModel.OperatorName = OrderOper;
            searchModel.SalerName = Sales;
            if (State != "-1" && !string.IsNullOrEmpty(State))
            {
                try
                {
                    if (State == "1")
                    {
                        searchModel.IsSettle = true;
                    }
                    else
                    {

                        searchModel.IsSettle = false;
                    }
                }
                catch
                {

                }
            }
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
            System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrder> list = TourOrderBll.GetTourOrderFinanceList(PageSize, PageIndex, ref RecordCount, SiteUserInfo.TourCompany.TourCompanyId, searchModel);
            if (list != null && list.Count > 0)
            {
                this.ctp_AccountList.DataSource = list;
                this.ctp_AccountList.DataBind();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
                this.ctp_AccountList.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='11' align='center'>暂时没有数据！</td></tr>";
            }
            BindPage();
        }
        #endregion

        //过滤小数点后的多余0
        public string FilterEndOfTheZeroDecimal(object o)
        {
            return EyouSoft.Common.Utils.FilterEndOfTheZeroString(o.ToString());
        }

        #region 设置分页
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
        #endregion

        //获得计调员名字
        public string GetOperatorName(object o)
        {
            System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOperator> list = null;
            try
            {
                list = (System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOperator>)o;
                string name = "";
                foreach (EyouSoft.Model.TourStructure.TourOperator model in list)
                {
                    name += model.ContactName + ",";
                }
                if (name != "")
                {
                    name = name.Substring(0, name.Length - 1);
                }
                return name;
            }
            catch
            {
                return "";
            }
            finally
            {
                list = null;
            }
        }
    }
}
