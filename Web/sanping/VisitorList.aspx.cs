using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EyouSoft.Common;

namespace Web.sanping
{
    /// <summary>
    /// 创建人：田想兵
    /// 创建日期：11-08-8
    /// 备注：订票游客信息显示
    /// </summary>
    public partial class VisitorList : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /*if (Utils.GetQueryStringValue("tourId") != "")
                {
                    string tourId = Utils.GetQueryStringValue("tourId");
                    EyouSoft.BLL.TourStructure.TourOrder orderbll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                    IList<EyouSoft.Model.TourStructure.TourOrderCustomer> listCus = orderbll.GetTravellers(tourId);
                    EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();

                    IList<string> CustomerList = bll.CustomerList(tourId);
                    var list = from v in listCus
                               from k in CustomerList
                               where v.ID == k
                               select v;
                    rpt_list.DataSource = list.ToList();
                    rpt_list.DataBind();
                    rpt_list.EmptyText = "<tr><td colspan='4' align='center'>没有任何记录</td></tr>";

                }
            }*/

                //request query ticketIssueId:机票出票编号
                string ticketIssueId = Utils.GetQueryStringValue("ticketIssueId");
                rpt_list.EmptyText = "<tr><td colspan='4' align='center'>没有任何记录</td></tr>";

                if (!string.IsNullOrEmpty(ticketIssueId))
                {
                    EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();
                    var info = bll.GetTicketOutListModel(ticketIssueId);

                    if (info != null && info.CustomerInfoList != null && info.CustomerInfoList.Count > 0)
                    {
                        rpt_list.DataSource = info.CustomerInfoList;
                        rpt_list.DataBind();
                    }
                }

            }
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
