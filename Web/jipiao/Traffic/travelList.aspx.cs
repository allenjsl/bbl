using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.jipiao.Traffic
{
    /// <summary>
    /// 交通管理 行程
    /// 李晓欢 2012-09-07
    /// </summary>
    public partial class travelList : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Utils.GetQueryStringValue("type");
            if (!string.IsNullOrEmpty(type) && type == "delete")
            {
                string trID = Utils.GetQueryStringValue("ID");
                int tfID = Utils.GetInt(Utils.GetQueryStringValue("tfID"));
                if (!string.IsNullOrEmpty(trID) && tfID > 0)
                {
                    Response.Clear();
                    Response.Write(DeleteTravel(trID, tfID));
                    Response.End();
                }
            }

            if (!IsPostBack)
            {
                string tfID = Utils.GetQueryStringValue("trfficID");
                if (!string.IsNullOrEmpty(tfID))
                {
                    InitTravelList(Utils.GetInt(tfID));
                }
            }


        }

        /// <summary>
        /// 删除行程
        /// </summary>
        /// <param name="travelID">行程编号</param>
        /// <returns></returns>
        protected string DeleteTravel(string travelID, int trafficId)
        {                      
            int[] tIdArr = travelID.Split(',').Select(i => Convert.ToInt32(i)).ToArray();
            bool ret = new EyouSoft.BLL.PlanStruture.PlanTrffic().DeletePlanTravel(trafficId, tIdArr);
            if (ret)
            {
                return "{\"ret\":\"1\",\"msg\":\"删除成功!\"}";
            }
            else
            {
                return "{\"ret\":\"0\",\"msg\":\"删除失败!\"}";
            }
        }

        /// <summary>
        /// 绑定行程列表
        /// </summary>
        /// <param name="tfID"></param>
        protected void InitTravelList(int tfID)
        {
            PageIndex = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("Page"), 0);
            IList<EyouSoft.Model.PlanStructure.TravelInfo> List = new EyouSoft.BLL.PlanStruture.PlanTrffic().GettravelList(PageSize, PageIndex, this.SiteUserInfo.CompanyID, ref RecordCount, tfID);
            if (List != null && List.Count > 0)
            {
                this.repTravelList.DataSource = List;
                this.repTravelList.DataBind();
                bindPage();
            }
            else
            {
                this.labMsg.Visible = true;
                this.ExporPageInfoSelect1.Visible = false;
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
