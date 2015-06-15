using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.xianlu
{
    public partial class QuoteInfo : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int QuoteID = Utils.GetInt(Utils.GetQueryStringValue("QuoteID"));
                if (!string.IsNullOrEmpty(QuoteID.ToString()) && QuoteID > 0)
                {
                    EyouSoft.Model.RouteStructure.QuoteTeamInfo ModelRouteTeaminfo = new EyouSoft.Model.RouteStructure.QuoteTeamInfo();
                    EyouSoft.BLL.RouteStructure.Quote BllQuote = new EyouSoft.BLL.RouteStructure.Quote();
                    ModelRouteTeaminfo = BllQuote.GetQuoteInfo(QuoteID);
                    if (ModelRouteTeaminfo != null)
                    {
                        //询价单位
                        this.Txt_Inquiry.Text = ModelRouteTeaminfo.QuoteUnitsName;
                        //联系人
                        this.Txt_Contact.Text = ModelRouteTeaminfo.ContactName;
                        //联系电话
                        this.Txt_TelPhone.Text = ModelRouteTeaminfo.ContactTel;
                        //预计出团时间
                        this.Txt_GroupStarTime.Text = ModelRouteTeaminfo.TmpLeaveDate.ToString("yyyy-MM-dd");
                        //人数
                        this.Txt_Numbers.Text = ModelRouteTeaminfo.PeopleNum.ToString();
                        //客人要求
                        IList<EyouSoft.Model.TourStructure.TourServiceInfo> TourServiceInfoList = ModelRouteTeaminfo.Requirements;                              
                        if (TourServiceInfoList != null && TourServiceInfoList.Count > 0)
                        {
                            this.rptList.DataSource = TourServiceInfoList;
                            this.rptList.DataBind();
                        }                        

                        //价格组成
                        IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> TourTeamServiceInfolist = ModelRouteTeaminfo.Services;
                        if (TourTeamServiceInfolist.Count > 0 && TourTeamServiceInfolist != null)
                        {
                            this.repPrices.DataSource = TourTeamServiceInfolist;
                            this.repPrices.DataBind();
                        }                        

                        //备注信息
                        this.Txt_RemarksBottom.Text = ModelRouteTeaminfo.Remark;

                        TourTeamServiceInfolist = null;
                        TourServiceInfoList = null;
                    }
                }
            }
        }


        #region 设置弹窗
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion
    }
}
