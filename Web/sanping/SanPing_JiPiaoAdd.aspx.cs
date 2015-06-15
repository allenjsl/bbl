/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：SanPing_JiPiaoAdd.aspx.cs
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\SanPing_JiPiaoAdd.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-12 16:09:43
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// ////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.BLL;
using EyouSoft.Model;
using Common.Enum;

namespace Web.sanping
{
    public partial class SanPing_JiPiaoAdd : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 全局行号
        /// </summary>
        public int i=0;
        public int j = 0;
        /// <summary>
        /// 计算公式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType? config_Agency;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region ajax删除
            if (Utils.GetQueryStringValue("act") == "del")
            {
                Response.Clear();
                EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();
                if (Utils.GetQueryStringValue("id") != "")
                {
                    EyouSoft.Model.TourStructure.TourBaseInfo m = new EyouSoft.BLL.TourStructure.Tour().GetTourInfo(Utils.GetQueryStringValue("tourid") );
                    if (m != null)
                    {
                        if (!Utils.PlanIsUpdateOrDelete(m.Status.ToString()))
                        {
                            Response.Write("-1");
                        }else
                        {
                            //删除操作
                            int i = bll.DeleteTicket(Utils.GetQueryStringValue("id"));
                            Response.Write(i.ToString());
                        }
                    }
                }
                Response.End();
                return;
            }
            #endregion
            #region 绑定
            if (!IsPostBack)
            {

                config_Agency = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetAgencyFee(CurrentUserCompanyID);
                BindList();
                if (Utils.GetInt(Utils.GetQueryStringValue("type")) == 1)
                { 
                    lt_jihua.Text = "散拼计划";
                    if (CheckGrant(TravelPermission.散拼计划_散拼计划_申请机票))
                    {
                        this.Page.Title = "申请机票_散拼计划";
                    }
                    else
                    {
                        Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_申请机票, false);
                    }
                }
                else {
                    lt_jihua.Text = "团队计划";
                    if (CheckGrant(TravelPermission.团队计划_团队计划_申请机票))
                    {
                        this.Page.Title = "申请机票_团队计划";
                    }
                    else
                    {
                        Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_申请机票, false);
                    }
                }
            }
            #endregion
            #region 当前用户
            jipiaoUpdate1.userId = SiteUserInfo.ID;
            jipiaoUpdate1.username = SiteUserInfo.UserName;
            jipiaoUpdate1.cur_companyId = CurrentUserCompanyID;
            #endregion
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        void BindList()
        {
            EyouSoft.BLL.PlanStruture.PlaneTicket plan = new EyouSoft.BLL.PlanStruture.PlaneTicket();
            IList<EyouSoft.Model.PlanStructure.MLBTicketApplyInfo> list =
                plan.GetTicketApplys(CurrentUserCompanyID, Request.QueryString["tourId"]);
            if (list != null && list.Count > 0)
            {
                //成人票款
                this.litFundAdult.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(list.Sum(f => f.FundAdult.TotalMoney).ToString("c2"));
                //儿童票款
                this.LitFunChildren.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(list.Sum(f => f.FundChildren.TotalMoney).ToString("c2"));

                //其它费用 
                if (config_Agency.HasValue && config_Agency.Value == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                {
                    this.LitAgencyAdult.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(list.Sum(n => n.FundAdult.OtherPrice).ToString("c2"));
                    this.LitAgencyChild.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(list.Sum(n => n.FundChildren.OtherPrice).ToString("c2"));
                }
                else
                {
                    this.LitAgencyAdult.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(list.Sum(n => n.FundAdult.AgencyPrice).ToString("c2"));
                    this.LitAgencyChild.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(list.Sum(n => n.FundChildren.AgencyPrice).ToString("c2"));
                }

                //成人数 儿童数
                this.LitAdultCount.Text = list.Sum(n => n.FundAdult.PeopleCount).ToString();
                this.litChildRenCount.Text = list.Sum(n => n.FundChildren.PeopleCount).ToString();
                //总费用
                this.LitTotalCount.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(list.Sum(n => n.TotalAmount).ToString("c2"));
                rpt_list.DataSource = list;
                rpt_list.DataBind();
            }
            else
            {
                this.tr.Visible = false;
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string getStatus(EyouSoft.Model.EnumType.PlanStructure.TicketState status)
        {
            if (status != EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请)
            {
                return status.ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 根据配置获取代理费或其他费用
        /// </summary>
        /// <param name="obj1">代理费</param>
        /// <param name="othermoney">其它费用</param>
        /// <returns></returns>
        public string getOtherMoney(object obj1, object othermoney)
        {
            if (config_Agency.HasValue && config_Agency.Value == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
            {
                return othermoney.ToString();
            }
            else
            {
                return obj1.ToString();
            }
        }

        protected void rpt_list1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.Model.PlanStructure.MLBTicketApplyInfo info = e.Item.DataItem as EyouSoft.Model.PlanStructure.MLBTicketApplyInfo;
            Repeater rpt = e.Item.FindControl("rpt_sList") as Repeater;
            rpt.DataSource = info.TicketFlights;
            rpt.DataBind();
        }
    }
}
