using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Web.jipiao.Traffic
{
    /// <summary>
    /// 交通管理 价格
    /// 李晓欢 2012-09-07
    /// </summary>
    public partial class trafficpricesList : Eyousoft.Common.Page.BackPage
    {
        //当前月
        protected string thisDate = string.Format("new Date(Date.parse('{0:yyyy\\/MM\\/dd}'))", DateTime.Today);
        //下月
        protected string NextDate = string.Format("new Date(Date.parse('{0:yyyy\\/MM\\/dd}'))", DateTime.Today.AddMonths(1));
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Utils.GetQueryStringValue("type");
            if (!string.IsNullOrEmpty(type) && type == "save")
            {
                Response.Clear();
                Response.Write(PageSave());
                Response.End();
            }

            if (!IsPostBack)
            {
                string tfID = Utils.GetQueryStringValue("trfficID");
                if (!string.IsNullOrEmpty(tfID))
                {
                    BindTravelList(Utils.GetInt(tfID));
                    BindPriceList(Utils.GetInt(tfID));
                }
            }
        }

        /// <summary>
        /// 绑定交通行程
        /// </summary>
        /// <param name="tfID"></param>
        protected void BindTravelList(int tfID)
        {
            IList<EyouSoft.Model.PlanStructure.TravelInfo> List = new EyouSoft.BLL.PlanStruture.PlanTrffic().GettravelList(PageSize, PageIndex, this.SiteUserInfo.CompanyID, ref RecordCount, tfID);
            if (List != null && List.Count > 0)
            {
                this.repTravelList.DataSource = List;
                this.repTravelList.DataBind();
            }
        }

        /// <summary>
        /// 绑定价格列表
        /// </summary>
        /// <param name="tfID"></param>
        protected void BindPriceList(int tfID)
        {
            IList<EyouSoft.Model.PlanStructure.TrafficPricesInfo> pricesList = new EyouSoft.BLL.PlanStruture.PlanTrffic().GetPricesList(tfID);
            if (pricesList != null && pricesList.Count > 0)
            {
                IsoDateTimeConverter isoDate = new IsoDateTimeConverter();
                isoDate.DateTimeFormat = "yyyy-MM-dd";
                this.hidChildrenPrices.Value = "{\"data\":" + JsonConvert.SerializeObject(pricesList, isoDate) + "}";
            }
        }


        /// <summary>
        /// 保存价格
        /// </summary>
        protected string PageSave()
        {
            string ret = string.Empty;
            //开始时间
            DateTime sDate = Utils.GetDateTime(Utils.GetFormValue("txtSDate"));
            //结束时间
            DateTime eDate = Utils.GetDateTime(Utils.GetFormValue("txtEDate"));
            //价格
            string[] PricesArr = Utils.GetFormValues("txtprice");
            //数量
            string[] NumArr = Utils.GetFormValues("txtnum");
            if (PricesArr.Length > 0)
            {
                string tfID = Utils.GetQueryStringValue("tfId");
                for (int i = 0; i < PricesArr.Length; i++)
                {
                    EyouSoft.Model.PlanStructure.TrafficPricesInfo model = new EyouSoft.Model.PlanStructure.TrafficPricesInfo();
                    model.TrafficId = Utils.GetInt(tfID);
                    model.InsueTime = DateTime.Now;
                    model.SDateTime = sDate.AddDays(i);
                    model.Status = EyouSoft.Model.EnumType.PlanStructure.TicketStatus.正常;
                    model.TicketNums = Utils.GetInt(NumArr[i]);
                    model.TicketPrices = Utils.GetDecimal(PricesArr[i]);
                    bool result = new EyouSoft.BLL.PlanStruture.PlanTrffic().AddTrafficPrice(model);
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
