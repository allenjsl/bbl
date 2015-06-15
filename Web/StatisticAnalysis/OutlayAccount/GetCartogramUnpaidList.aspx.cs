using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.StatisticAnalysis.OutlayAccount
{
    /// <summary>
    /// 页面功能：支出对账单--部门统计-未收
    /// Author:liuym
    /// Date:2011-01-20
    /// </summary>
    public partial class GetUnpaidList : BackPage
    {

        /// <summary>
        /// OnPreInit事件 设置模式窗体属性
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }
        #region
        protected int PageSize = 10;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected string TourType = string.Empty;//团队类型
        protected string RouteName = string.Empty;//线路区域名称
        protected string OperatorId = string.Empty;//操作员ID
        protected DateTime? StartDate = null;//出团开始日期
        protected DateTime? EndDate = null;//出团结束日期
        IList<object> list = null;
        #endregion

        #region
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitCartogramUnpayList();
            }
        }
        #endregion

        #region 初始化未收数据
        private void InitCartogramUnpayList()
        {
            #region 获取表单值
            TourType = Utils.GetQueryStringValue("TourType");
            StartDate = Utils.GetDateTime("StartDate");
            EndDate = Utils.GetDateTime("StartDate");
            TourType = Utils.GetQueryStringValue("TourType");
            RouteName = Utils.GetQueryStringValue("RouteName");
            OperatorId = Utils.GetQueryStringValue("OperatorId");
            #endregion

            #region 实体赋值

            #endregion
            //调用底层方法

            if (list != null && list.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_CartogramUnpayList.DataSource = list;
                this.crp_CartogramUnpayList.DataBind();
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_CartogramUnpayList.EmptyText = "<tr><td colspan='8' height='40px' class='even'>暂时没有数据！</td></tr>";
            }
        }
        #endregion
    }
}
