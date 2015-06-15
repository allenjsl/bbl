using System;
using System.Collections.Generic;
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

namespace Web.print.normal
{
    /// <summary>
    /// 芭比来-送团单
    /// 功能：芭比来-送团单
    /// 创建人：戴银柱
    /// </summary>
    /// 修改时间：2011-6-30
    /// 修改人：柴逸宁
    /// 修改内容：修改集合时间输出格式
    public partial class OffGroupPrint : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tourId = Utils.GetQueryStringValue("tourId");
                if (tourId != "")
                {
                    DataInit(tourId);
                }
            }

            string type = Utils.GetQueryStringValue("type");
            if (type == "save")
            {
                EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
                string tourId = Utils.GetFormValue(this.hideTourID.UniqueID);
                EyouSoft.Model.TourStructure.TourBaseInfo model = bll.GetTourInfo(tourId);
                if (model != null)
                {
                    string txtNumFrist = Utils.GetFormValue(txtNum1.UniqueID);
                    string txtNumSecound = Utils.GetFormValue(txtNum2.UniqueID);
                    string txtNumThird = Utils.GetFormValue(txtNum3.UniqueID);
                    string txtNumFourth = Utils.GetFormValue(txtNum4.UniqueID);

                    IList<string> list = new List<string>();
                    list.Add(txtNumFrist);
                    list.Add(txtNumSecound);
                    list.Add(txtNumThird);
                    bool result = bll.SetTourGuides(tourId, list, txtNumFourth);

                    Response.Clear();
                    if (result)
                    {
                        Response.Write("Ok");
                    }
                    else
                    {
                        Response.Write("Nk");
                    }
                    Response.End();
                }


            }
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="tourId"></param>
        protected void DataInit(string tourId)
        {
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourBaseInfo model = bll.GetTourInfo(tourId);
            if (model != null)
            {
                this.hideTourID.Value = tourId;

                //绑定地接社信息
                this.rptDjList.DataSource = bll.GetTourLocalAgencys(tourId);
                this.rptDjList.DataBind();
                //绑定旅客信息

                this.rptCustomer.DataSource = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetTravellers(tourId).Where(x => x.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList(); ;
                this.rptCustomer.DataBind();

                //出团日期
                this.txtOutDate.Text = model.LDate.ToString("yyyy-MM-dd");
                //出发交通
                this.lblBenginDate.Text = model.LTraffic;
                //回程交通
                this.lblBackDate.Text = model.RTraffic;
                //线路名称
                this.lblAreaName.Text = model.RouteName;
                //人数
                this.lblCount.Text = model.PlanPeopleNumber.ToString();
                //计划类型
                this.lblTourType.Text = model.TourType.ToString();

                if (model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
                {
                    EyouSoft.Model.TourStructure.TourSingleInfo tsModel = (EyouSoft.Model.TourStructure.TourSingleInfo)model;
                    //this.lblRemarks.Text =tsModel.
                }
                if (model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
                {
                    EyouSoft.Model.TourStructure.TourInfo tModel = (EyouSoft.Model.TourStructure.TourInfo)model;
                    if (tModel.TourNormalInfo != null)
                    {
                        //内部信息
                        this.lblRemarks.Text = tModel.TourNormalInfo.NeiBuXingXi;

                    }
                    //标志
                    this.txtNum4.Value = tModel.GatheringSign;
                    //string gather = Utils.GetDateTimeNullable(tModel.GatheringTime) == null ? string.Empty : Utils.GetDateTimeNullable(tModel.GatheringTime).ToString();
                    //this.txtGather.Text = gather == string.Empty ? "" : (Utils.GetDateTime(gather, DateTime.Now)).ToString("yyyy-MM-dd hh 点");
                    this.txtGather.Text = tModel.GatheringTime;
                    IList<string> txtList = bll.GetTourGuides(tourId);
                    if (txtList != null)
                    {
                        if (txtList.Count > 0)
                        {
                            this.txtNum1.Value = txtList[0];
                        }
                        if (txtList.Count > 1)
                        {
                            this.txtNum2.Value = txtList[1];
                        }
                        if (txtList.Count > 2)
                        {
                            this.txtNum3.Value = txtList[2];
                        }

                    }
                }
                if (model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                {
                    EyouSoft.Model.TourStructure.TourTeamInfo ttModel = (EyouSoft.Model.TourStructure.TourTeamInfo)model;

                    #region 人数And结算价

                    TourQuotePrint priantTQP = new TourQuotePrint();
                    string number = string.Empty;
                    string money = string.Empty;
                    priantTQP.getPepoleNum(SiteUserInfo.CompanyID, model.PlanPeopleNumber, ttModel.TourTeamUnit, ref number, ref money);
                    this.lblCount.Text = number;

                    #endregion
                    //内部信息
                    if (ttModel.TourNormalInfo != null)
                    {
                        this.lblRemarks.Text = ttModel.TourNormalInfo.NeiBuXingXi;
                    }

                    //标志
                    this.txtNum4.Value = ttModel.GatheringSign;
                    //string gather = Utils.GetDateTimeNullable(ttModel.GatheringTime) == null ? string.Empty : Utils.GetDateTimeNullable(ttModel.GatheringTime).ToString();
                    //this.txtGather.Text = gather == string.Empty ? "" : (Utils.GetDateTime(gather, DateTime.Now)).ToString("yyyy-MM-dd hh 点");
                    this.txtGather.Text = ttModel.GatheringTime;
                    IList<string> txtList = bll.GetTourGuides(tourId);
                    if (txtList != null)
                    {
                        if (txtList.Count > 0)
                        {
                            this.txtNum1.Value = txtList[0];
                        }
                        if (txtList.Count > 1)
                        {
                            this.txtNum2.Value = txtList[1];
                        }
                        if (txtList.Count > 2)
                        {
                            this.txtNum3.Value = txtList[2];
                        }

                    }

                }


            }
        }

        protected string GetOtherInfo(object orderId)
        {
            string str = "";
            if (orderId != null)
            {
                string orderID = orderId.ToString();
                EyouSoft.Model.CompanyStructure.CustomerInfo model = new EyouSoft.BLL.TourStructure.TourOrder().GetCustomerInfo(orderID);

                EyouSoft.Model.CompanyStructure.CustomerContactInfo otherSideContactInfo = new EyouSoft.BLL.TourStructure.TourOrder().GetOrderOtherSideContactInfo(orderID);

                if (model != null && otherSideContactInfo != null)
                {
                    str = "<td align=\"center\"  class=\"td_r_b_border\">" + model.Name + "</td><td align=\"center\"  class=\"td_r_b_border\">" + otherSideContactInfo.Name + "</td><td align=\"center\" class=\"td_b_border\">" + otherSideContactInfo.Mobile + "</td>";
                }
                else
                {
                    str = "<td align=\"center\"  class=\"td_r_b_border\"></td><td align=\"center\"  class=\"td_r_b_border\"></td><td align=\"center\"  class=\"td_b_border\"></td>";
                }
            }
            else
            {
                str = "<td align=\"center\" class=\"td_r_b_border\"></td><td class=\"td_r_b_border\" align=\"center\" ></td><td align=\"center\"  class=\"td_b_border\"></td>";
            }
            return str;
        }
    }
}
