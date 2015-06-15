using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EyouSoft.Common;
using System.Text;

namespace Web.print.normal
{
    /// <summary>
    /// 芭比来-团队报价单
    /// 修改人：戴银柱
    /// 修改时间： 2011-02-17
    /// </summary>
    public partial class TourQuotePrint : Eyousoft.Common.Page.BackPage
    {
        protected DateTime LeaveTime;
        protected int RecordSum;
        protected int RecordSumS;
        protected int tdCount = 0;
        protected string style = "padding: 0px; margin: 0px;border-bottom: none;border-right:none";
        protected void Page_Load(object sender, EventArgs e)
        {
            string tourId = Utils.GetQueryStringValue("tourId");
            if (tourId != "")
            {
                DataInit(tourId);
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }
        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="tourId"></param>
        private void DataInit(string tourId)
        {
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourBaseInfo model = bll.GetTourInfo(tourId);
            EyouSoft.Model.TourStructure.TourTeamInfo teamModel = null;
            if (model != null && model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
            {
                teamModel = (EyouSoft.Model.TourStructure.TourTeamInfo)model;
                if (teamModel != null)
                {
                    //组团社
                    this.txtGroups.Text = teamModel.BuyerCName;
                    //联系人
                    this.txtContact.Text = "";
                    //电话
                    this.txtPhone.Text = "";
                    //预计出团日期
                    this.txtOutDate.Text = teamModel.LDate.ToString("yyyy-MM-dd");
                    //人数
                    string number = string.Empty;
                    string money = string.Empty;
                    getPepoleNum(SiteUserInfo.CompanyID, teamModel.PlanPeopleNumber, teamModel.TourTeamUnit, ref number, ref money);
                    this.txtCount.Text = number;
                    this.txtBaoJia.Text = money;
                    //传真
                    this.txtFax.Text = "";
                    //线路名称
                    this.txtAreaName.Text = teamModel.RouteName;
                    //出团日期
                    this.LeaveTime = teamModel.LDate;
                    //地接总价
                    this.lblAllPrice.Text = teamModel.TotalAmount.ToString("0.00");
                    //判断时候有报价ID
                    if (teamModel.QuoteId > 0)
                    {
                        //获得报价model
                        EyouSoft.Model.RouteStructure.QuoteTeamInfo routeModel = new EyouSoft.BLL.RouteStructure.Quote().GetQuoteInfo(teamModel.QuoteId);
                        if (routeModel != null)
                        {
                            //获得客户要求列表
                            if (routeModel.Requirements != null && routeModel.Requirements.Count > 0)
                            {
                                //绑定具体要求列表
                                this.rptRequire.DataSource = routeModel.Requirements;
                                tdCount = routeModel.Requirements.Count;
                                this.rptRequire.DataBind();
                                this.RecordSum = routeModel.Requirements.Count;
                            }
                            else
                            {
                                this.pnlContent.Visible = false;
                                style = "padding: 0px; margin: 0px;border-bottom: none;";
                                this.pnlSpecific.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        //如果不存在报价 那么隐藏具体要求
                        this.pnlContent.Visible = false;
                        style = "padding: 0px; margin: 0px;border-bottom: none;";
                        this.pnlSpecific.Visible = false;
                    }

                    //行程安排 数据绑定
                    if (teamModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                    {
                        if (teamModel.TourNormalInfo != null && teamModel.TourNormalInfo.Plans != null)
                        {
                            this.rptTravel.DataSource = teamModel.TourNormalInfo.Plans;
                            this.rptTravel.DataBind();
                            tdCount = teamModel.TourNormalInfo.Plans.Count;
                            this.pnlSecond.Visible = false;
                        }
                    }
                    else
                    {
                        if (teamModel.TourQuickInfo != null)
                            this.lblSecond.Text = teamModel.TourQuickInfo.QuickPlan;
                        this.pnlFrist.Visible = false;
                    }

                    //地接信息 数据绑定
                    if (teamModel.Services != null && teamModel.Services.Count > 0)
                    {
                        this.RecordSumS = teamModel.Services.Count;
                        this.rptDjList.DataSource = teamModel.Services;
                        this.rptDjList.DataBind();

                    }


                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="recordSum"></param>
        /// <param name="TdCount"></param>
        /// <returns></returns>
        protected string IsOutTrOrTd(int itemIndex, int recordSum, int TdCount)
        {
            //先判断当前itemIndex是否是最后一条数据
            if ((itemIndex + 1) == recordSum)
            {
                System.Text.StringBuilder strb = new System.Text.StringBuilder();
                //判断当前itemInex是否已经到一行的末尾(一行显示4个Td)
                if (((itemIndex + 1) % TdCount) == 0)
                {
                    strb.Append("</tr>");
                }
                else
                {
                    int leaveTdCount = (TdCount - ((itemIndex + 1) % TdCount)) * 2;
                    for (int i = 0; i < leaveTdCount; i++)
                    {
                        if (i + 1 == leaveTdCount)
                        {
                            strb.Append("<td align='center' class='td_r_b_border'>&nbsp;</td>");
                        }
                        else
                        {
                            strb.Append("<td align='center' class='td_b_border'>&nbsp;</td>");
                        }

                    }
                    strb.Append("</tr>");
                }

                return strb.ToString();
            }
            //判断当前itemInex是否已经到一行的末尾(一行显示4个Td)
            else if (((itemIndex + 1) % TdCount) == 0)
            {
                return "</tr><tr>";
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dinner"></param>
        /// <returns></returns>
        protected string GetDinnerByValue(string dinner)
        {
            string str = "";
            if (dinner.Trim().Length > 0)
            {
                string[] list = dinner.Split(',');
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list[i].Trim() != "")
                    {
                        switch (list[i])
                        {
                            case "2": str += "早餐,"; break;
                            case "3": str += "中餐,"; break;
                            case "4": str += "晚餐,"; break;
                            default: break;
                        }
                    }
                }
            }
            str = str.TrimEnd(',');
            return str;
        }
        #region 返回人数显示
        /// <summary>
        /// 返回人数结算价显示
        /// </summary>
        /// <param name="companyID">公司编号</param>
        /// <param name="count">总人数</param>
        /// <param name="TourTeamUnit">团队计划人数及单价信息</param>
        /// <param name="number">返回人数string</param>
        /// <param name="money">返回结算价string</param>
        public void getPepoleNum(int companyID, int count, EyouSoft.Model.TourStructure.MTourTeamUnitInfo TourTeamUnit, ref string number, ref string money)
        {
            EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople NumConfig = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetTeamNumberOfPeople(companyID);
            if (NumConfig == EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople.PartNumber && TourTeamUnit != null)
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("{0}(成人)+{1}(儿童)+{2}(全陪)", TourTeamUnit.NumberCr, TourTeamUnit.NumberEt, TourTeamUnit.NumberQp);
                number = str.ToString();
                str = new StringBuilder();
                str.AppendFormat("成人单价合计：{0}、儿童单价合计：{1}、全陪单价合计：{2}", TourTeamUnit.UnitAmountCr, TourTeamUnit.UnitAmountEt, TourTeamUnit.UnitAmountQp);
                money = str.ToString();
                str = null; 
                
            }
            else
            {
                number = "总人数：" + count.ToString();
                money = string.Empty;
            }
        }
        #endregion
    }
}
