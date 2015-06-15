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
    /// 设置票状态
    /// 李晓欢 2012-09-14
    /// </summary>
    public partial class updateStatus : Eyousoft.Common.Page.BackPage
    {
        protected string ticketStatus = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Utils.GetQueryStringValue("type");
            if (!string.IsNullOrEmpty(type) && type == "update")
            {
                Response.Clear();
                Response.Write(pricesUpdateStatus());
                Response.End();
            }
            if (!IsPostBack)
            {
                int trafficId = Utils.GetInt(Utils.GetQueryStringValue("tfId"));
                string Id = Utils.GetQueryStringValue("Id");
                if (trafficId > 0 && !string.IsNullOrEmpty(Id))
                {
                    EyouSoft.Model.PlanStructure.TrafficPricesInfo pricesInfo = new EyouSoft.BLL.PlanStruture.PlanTrffic().GetTrafficPrices(Id, trafficId);
                    if (pricesInfo != null)
                    {
                        ticketStatus = ((int)pricesInfo.Status).ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <returns></returns>
        public string pricesUpdateStatus()
        {
            string msg = string.Empty;
            string id = Utils.GetQueryStringValue("Id");
            int TrafficId = Utils.GetInt(Utils.GetQueryStringValue("tfId"));
            if (!string.IsNullOrEmpty(id))
            {
                int status = Utils.GetInt(Utils.GetFormValue("radStatus"));
                bool ret = new EyouSoft.BLL.PlanStruture.PlanTrffic().UpdatePricesStatus(id, TrafficId, (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)status);
                if (ret)
                {
                    msg = "{\"ret\":\"1\",\"msg\":\"状态设置成功!\"}";
                }
                else
                {
                    msg = "{\"ret\":\"0\",\"msg\":\"状态设置失败!\"}";
                }
            }
            return msg;
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

    }
}
