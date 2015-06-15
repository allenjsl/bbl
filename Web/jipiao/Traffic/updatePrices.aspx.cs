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
    /// 更新票价格
    /// 李晓欢 2012-09-14
    /// </summary>
    public partial class updatePrices : Eyousoft.Common.Page.BackPage
    {
        protected string status = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Utils.GetQueryStringValue("action");
            if (!string.IsNullOrEmpty(action) && action == "Save")
            {
                Response.Clear();
                Response.Write(PageSave());
                Response.End();
            }

            if (!IsPostBack)
            {
                string type = Utils.GetQueryStringValue("type");
                if (!string.IsNullOrEmpty(type) && type == "update")
                {
                    string id = Utils.GetQueryStringValue("Id");
                    int tfId = Utils.GetInt(Utils.GetQueryStringValue("tfId"));
                    if (!string.IsNullOrEmpty(id) && tfId > 0)
                    {
                        GetTrafficPricesInfo(id, tfId);
                    }
                }
            }
        }

        /// <summary>
        /// 获取价格实体
        /// </summary>
        /// <param name="prices"></param>
        protected void GetTrafficPricesInfo(string pricesId, int trafficId)
        {
            EyouSoft.Model.PlanStructure.TrafficPricesInfo info = new EyouSoft.BLL.PlanStruture.PlanTrffic().GetTrafficPrices(pricesId, trafficId);
            if (info != null)
            {
                this.txtTicketPrices.Value = Utils.GetDecimal(info.TicketPrices.ToString()).ToString("0.00");
                this.txtTicketNums.Value = info.TicketNums.ToString();
                status = ((int)info.Status).ToString();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="pricesId">价格编号</param>
        /// <returns></returns>
        protected string PageSave()
        {
            string msg = string.Empty;
            EyouSoft.Model.PlanStructure.TrafficPricesInfo priceInfo = new EyouSoft.Model.PlanStructure.TrafficPricesInfo();
            priceInfo.InsueTime = DateTime.Now;
            priceInfo.SDateTime = Utils.GetDateTime(Utils.GetQueryStringValue("date"));
            priceInfo.Status = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)Utils.GetInt(Utils.GetFormValue("radStatus"));
            priceInfo.TicketNums = Utils.GetInt(Utils.GetFormValue(this.txtTicketNums.UniqueID));
            priceInfo.TicketPrices = Utils.GetDecimal(Utils.GetFormValue(this.txtTicketPrices.UniqueID));
            priceInfo.TrafficId = Utils.GetInt(Utils.GetQueryStringValue("tfId"));
            if (Utils.GetQueryStringValue("type") == "update")
            {
                priceInfo.PricesID = Utils.GetQueryStringValue("pricesId");
                bool ret = new EyouSoft.BLL.PlanStruture.PlanTrffic().UpdatetrafficPrice(priceInfo);
                if (ret)
                {
                    msg = "{\"msg\":\"修改成功!\"}";
                }
                else
                {
                    msg = "{\"msg\":\"修改失败!\"}";
                }
            }
            else
            {
                bool ret = new EyouSoft.BLL.PlanStruture.PlanTrffic().AddTrafficPrice(priceInfo);
                if (ret)
                {
                    msg = "{\"msg\":\"添加成功!\"}";
                }
                else
                {
                    msg = "{\"msg\":\"添加失败!\"}";
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
