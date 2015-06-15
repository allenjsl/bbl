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
    /// 交通添加
    /// 李晓欢 2012-09-10
    /// </summary>
    public partial class trafficAdd : Eyousoft.Common.Page.BackPage
    {
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
                if (type == "update")
                {
                    int trfficID = Utils.GetInt(Utils.GetQueryStringValue("trfficID"));
                    if (trfficID > 0)
                    {
                        GetTrafficModel(trfficID);
                    }
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        /// <summary>
        /// 获取交通实体
        /// </summary>
        /// <param name="trfficID">交通编号</param>
        private void GetTrafficModel(int trfficID)
        {
            EyouSoft.Model.PlanStructure.TrafficInfo model = new EyouSoft.BLL.PlanStruture.PlanTrffic().GetTrafficModel(trfficID);
            if (model != null)
            {
                this.txtTrfficName.Value = model.TrafficName;
                this.txtChildprices.Value = Utils.GetDecimal(model.ChildPrices.ToString()).ToString("0.00");
            }
        }


        private string PageSave()
        {
            string ret = string.Empty;
            EyouSoft.Model.PlanStructure.TrafficInfo model = new EyouSoft.Model.PlanStructure.TrafficInfo();
            model.ChildPrices = Utils.GetDecimal(Utils.GetFormValue(this.txtChildprices.UniqueID), 0);
            model.CompanyId = this.SiteUserInfo.CompanyID;
            model.InsueTime = DateTime.Now;
            model.IsDelete = false;
            model.Operater = this.SiteUserInfo.UserName;
            model.OperaterId = this.SiteUserInfo.ID;
            model.Status = EyouSoft.Model.EnumType.PlanStructure.TrafficStatus.正常;
            model.TrafficDays = 0;
            model.TrafficName = Utils.GetFormValue(this.txtTrfficName.UniqueID);
            bool result = false;
            string type = Utils.GetQueryStringValue("type");
            if (!string.IsNullOrEmpty(type))
            {
                if (type == "update" && !string.IsNullOrEmpty(Utils.GetQueryStringValue("trfficID")))
                {
                    model.TrafficId = Utils.GetInt(Utils.GetQueryStringValue("trfficID"));
                    result = new EyouSoft.BLL.PlanStruture.PlanTrffic().UpdatePlanTraffic(model);
                    if (result)
                    {
                        ret = "{\"ret\":\"1\",\"msg\":\"修改成功!\"}";
                    }
                    else
                    {
                        ret = "{\"ret\":\"0\",\"msg\":\"修改失败!\"}";
                    }
                }
                else
                {
                    result = new EyouSoft.BLL.PlanStruture.PlanTrffic().AddPlanTraffic(model);
                    if (result)
                    {
                        ret = "{\"ret\":\"1\",\"msg\":\"添加成功!\"}";
                    }
                    else
                    {
                        ret = "{\"ret\":\"0\",\"msg\":\"添加失败!\"}";
                    }
                }
            }


            return ret;
        }
    }
}
