using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.jipiao.Traffic
{
    /// <summary>
    /// 交通管理 交通列表
    /// 2012-09-07 李晓欢
    /// </summary>
    public partial class trafficList : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTrfficList(); 
            }
        }

        protected void BindTrfficList()
        {
            PageIndex = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("Page"), 0);
            EyouSoft.Model.PlanStructure.TrafficSearch searchModel = new EyouSoft.Model.PlanStructure.TrafficSearch();
            string trfficName = EyouSoft.Common.Utils.GetQueryStringValue("trfficName");
            if (!string.IsNullOrEmpty(trfficName))
            {
                searchModel.TrafficName = trfficName;
            }
            IList<EyouSoft.Model.PlanStructure.TrafficInfo> List = new EyouSoft.BLL.PlanStruture.PlanTrffic(SiteUserInfo).GetTrafficList(searchModel, PageSize, PageIndex, this.SiteUserInfo.CompanyID, ref RecordCount);
            if (List != null && List.Count > 0)
            {
                this.labMsg.Visible = false;
                this.repTrfficList.DataSource = List;
                this.repTrfficList.DataBind();
                bindPage();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
                this.labMsg.Visible = true;
                this.labMsg.Text = "暂无数据!";
            }
        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="LineType">线路类型</param>
        /// <param name="LineName">线路名称</param>
        /// <param name="StarTime">开始发布时间</param>
        /// <param name="EndTime">结束发布时间</param>
        /// <param name="DateNumber">出团天数</param>
        /// <param name="Author">发布人</param>
        protected void bindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
    }
}
