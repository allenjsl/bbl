using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Common.Enum;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 财务管理：杂费收入
    /// 功能：显示杂费收入列表
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-20
    /// 修改人：柴逸宁
    /// 修改时间：2011-06-21
    /// 修改内容：金额栏添加金额的合计
    public partial class Income : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 20;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        #region 总计参数
        protected decimal totalAmount = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            //判断权限
            if (!CheckGrant(TravelPermission.财务管理_杂费收入_栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.财务管理_杂费收入_栏目, false);
                return;
            }

            if (!CheckGrant(TravelPermission.财务管理_杂费收入_收入登记))
            {
                this.pnlAdd.Visible = false;
            }
            if (!IsPostBack)
            {
                //
                string incomePro = Server.UrlDecode(Utils.GetQueryStringValue("incomePro"));
                string incomeMan = Server.UrlDecode(Utils.GetQueryStringValue("incomeMan"));
                string incomeTime = Utils.GetQueryStringValue("incomeTime");

                this.txtIncomePro.Text = incomePro;
                this.txtIncomeMan.Text = incomeMan;
                this.txtIncomeTime.Text = incomeTime;
                //当前页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                //查询
                DataInit(incomePro, incomeMan, incomeTime);
            }
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        protected void DataInit(string incomePro, string incomeMan, string incomeTime)
        {

            EyouSoft.Model.FinanceStructure.OtherCostQuery searchModel = new EyouSoft.Model.FinanceStructure.OtherCostQuery();
            searchModel.CompanyId = SiteUserInfo.CompanyID;
            searchModel.ItemName = incomePro;
            searchModel.Payee = incomeMan;
            searchModel.StartTime = Utils.GetDateTimeNullable(incomeTime);
            searchModel.EndTime = Utils.GetDateTimeNullable(incomeTime);
            searchModel.TourCode = Utils.GetQueryStringValue("tcode");
            searchModel.LSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            searchModel.LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            EyouSoft.BLL.FinanceStructure.OtherCost bll = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo);

            IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> list = bll.GetOtherIncomeList(searchModel, pageSize, pageIndex, ref recordCount);
            #region 合计
            bll.GetOtherIncomeList(searchModel, ref totalAmount);
            #endregion
            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                BindPage();
            }
            else
            {
                this.ExportPageInfo1.Visible = false;
                this.lblMsg.Text = "未找到相关数据!";
            }


            list = null;
        }

        #region 设置分页
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion
    }
}
