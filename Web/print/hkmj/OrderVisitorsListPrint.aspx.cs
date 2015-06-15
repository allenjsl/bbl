using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Model.TourStructure;

namespace Web.print.hkmj
{
    public partial class OrderVisitorsListPrint : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dt = Utils.GetDateTime(Utils.GetQueryStringValue("date"));
                int trafficID = Utils.GetInt(Utils.GetQueryStringValue("tfID"));
                if (trafficID > 0 && dt != null)
                {
                    InitVistorsList(trafficID, dt);
                    InitTraffIcModel(trafficID, dt);
                }

            }
        }

        /// <summary>
        /// 绑定游客信息
        /// </summary>
        protected void InitVistorsList(int traffIcID, DateTime dt)
        {
            int adultNum = 0;
            int childrenNum = 0;
            IList<EyouSoft.Model.TourStructure.SongJiJiHuaBiao> List = new EyouSoft.BLL.TourStructure.TourOrder(this.SiteUserInfo).GetSongJiJiHuaBiao(traffIcID, dt);
            if (List != null && List.Count > 0)
            {
                adultNum = List.Sum(p => p.ChenRenShu);
                childrenNum = List.Sum(p => p.ErTongShu);
                this.repVisitorsList.DataSource = List;
                this.repVisitorsList.DataBind();
            }
            this.litAdultNum.Text = adultNum.ToString();
            this.litChildRenNum.Text = childrenNum.ToString();
            this.litCountNum.Text = (adultNum + childrenNum).ToString();
        }

        protected string GetTourOrderAmountPlusInfo(object amountplus)
        {
            string remarkStr = string.Empty;
            EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo info = (EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo)amountplus;
            if (info != null)
            {
                remarkStr= info.Remark;
            }
            return remarkStr;
        }

        /// <summary>
        /// 获取交通实体
        /// </summary>
        /// <param name="trafficID"></param>
        protected void InitTraffIcModel(int trafficID, DateTime dt)
        {
            EyouSoft.Model.PlanStructure.TrafficInfo traffModel = new EyouSoft.BLL.PlanStruture.PlanTrffic(SiteUserInfo).GetTrafficModel(trafficID);
            if (traffModel != null)
            {
                this.litTrafficName.Text = traffModel.TrafficName;
                this.litLeaveDate.Text = dt.ToString("yyyy-MM-dd");
            }
        }

        protected string GetCustomerList(object o)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            IList<TourOrderCustomer> list = (List<TourOrderCustomer>)o;
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sb.Append("姓名：[" + list[i].VisitorName + "]&nbsp联系电话：[" + list[i].ContactTel + "]<br/>证件：[" + list[i].CradType + ":" + list[i].CradNumber + "]<br/>");
                }
            }
            return sb.ToString();
        }
    }
}
