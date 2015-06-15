using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.UserCenter.DjArrangeMengs
{
    public partial class TeamTake : Eyousoft.Common.Page.AreaConnectPage
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 地接社列表
                //计划编号
                string tourid = Utils.GetQueryStringValue("Tourid");
                DataListInit(tourid);                
                #endregion

                if (Utils.GetQueryStringValue("action") == "first")
                {
                    DataListInitfirst(tourid);
                }

                //计划类型: 快速 or 标准
                string planType = Utils.GetQueryStringValue("planType");
                //修改记录的ID
                string id = Utils.GetQueryStringValue("id");
                //地接ID
                string travelId = Utils.GetQueryStringValue("travelId");
                //分页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                if (id != "")
                {
                    this.hideId.Value = id;
                }
                //团号
                if (tourid != "")
                {
                    this.hidePlanId.Value = tourid;
                }
                //地接编号
                if (travelId != "")
                {
                    this.hideDiJieID.Value = travelId;
                }
                //发布类型
                if (planType == "Normal")
                {
                    //标准版发布
                    this.hidePlanType.Value = "Normal";
                }
                else
                {
                    //快速版发布
                    this.hidePlanType.Value = "Quick";
                    //快速发布 没有 接待行程 功能
                    this.pnlJieDai.Visible = false;
                }
                //获得当前地接价格组成A类 or B类
                EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting set = setBll.GetSetting(SiteUserInfo.LocalAgencyCompanyInfo.CompanyId);
                if (set != null)
                {
                    if (set.PriceComponent == EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.分项报价)
                    {
                        this.pnlProjectA.Visible = false;
                        this.pnlProjectB.Visible = true;
                    }
                    else
                    {
                        this.pnlProjectA.Visible = true;
                        this.pnlProjectB.Visible = false;
                    }
                }
                #region 初始化地接社
                if (id != "" && planType != "")
                {
                    //再次绑定地接社
                    string planid = Utils.GetQueryStringValue("planId");
                    this.hidePlanId.Value = planid;
                    this.hidePlanType.Value = planType;
                    DataListInit(planid);
                    //getmodel取地接社详细信息
                    DataModelInti(id, planType);
                }                
                #endregion
            }
        }      

        /// <summary>
        /// 地接列表数据绑定
        /// </summary>
        /// <param name="travelId">计划的ID</param>
        /// <param name="planType"></param>
        protected void DataListInit(string tourId)
        {
            if (tourId != "")
            {
                //地接的操作BLL
                EyouSoft.BLL.PlanStruture.TravelAgency travelbll = new EyouSoft.BLL.PlanStruture.TravelAgency();
                //获得该计划下的所有地接的集合
                IList<EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo> list = travelbll.GetList(tourId);
                if (list != null && list.Count > 0)
                {
                    //列表绑定数据
                    this.rptList.DataSource = list;
                    this.rptList.DataBind();
                    BindPage();
                }
                else
                {
                    this.ExportPageInfo1.Visible = false;
                    this.lblMsg.Text = "没有相关数据!";
                }

                EyouSoft.Model.TourStructure.TourBaseInfo baseModel = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo).GetTourInfo(tourId);
                EyouSoft.Model.TourStructure.TourTeamInfo teamModel = null;
                EyouSoft.Model.TourStructure.TourInfo infoModel = null;

                if (baseModel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
                {
                    infoModel = (EyouSoft.Model.TourStructure.TourInfo)baseModel;
                }
                else
                {
                    teamModel = (EyouSoft.Model.TourStructure.TourTeamInfo)baseModel;
                }

                if (infoModel != null)
                {
                    if (infoModel.TourNormalInfo != null && infoModel.TourNormalInfo.Plans != null)
                    {
                        IList<Jiedai> jiedaiList = new List<Jiedai>();
                        DateTime fristDate = infoModel.LDate;
                        for (int j = 0; j < infoModel.TourNormalInfo.Plans.Count; j++)
                        {
                            Jiedai jModel = new Jiedai();
                            jModel.TraveDate = fristDate.AddDays(j).ToString("yyyy-MM-dd");
                            jModel.PlanContent = infoModel.TourNormalInfo.Plans[j].Plan;
                            jiedaiList.Add(jModel);
                        }
                        this.rptJieDai.DataSource = jiedaiList;
                        this.rptJieDai.DataBind();
                    }

                }
                if (teamModel != null)
                {
                    if (teamModel.TourNormalInfo != null && teamModel.TourNormalInfo.Plans != null)
                    {
                        IList<Jiedai> jiedaiList = new List<Jiedai>();
                        DateTime fristDate = teamModel.LDate;
                        for (int j = 0; j < teamModel.TourNormalInfo.Plans.Count; j++)
                        {
                            Jiedai jModel = new Jiedai();
                            jModel.TraveDate = fristDate.AddDays(j).ToString("yyyy-MM-dd");
                            jModel.PlanContent = teamModel.TourNormalInfo.Plans[j].Plan;
                            jiedaiList.Add(jModel);
                        }
                        this.rptJieDai.DataSource = jiedaiList;
                        this.rptJieDai.DataBind();
                    }

                }
            }
        }

        #region 绑定地接社第一条信息
        protected void DataListInitfirst(string tourid)
        {
            if (tourid != null)
            {
                //地接的操作BLL
                EyouSoft.BLL.PlanStruture.TravelAgency travelbll = new EyouSoft.BLL.PlanStruture.TravelAgency();
                EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo travelModel = new EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo();
                //获得该计划下的所有地接的集合
                IList<EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo> list = travelbll.GetList(tourid);
                travelModel = travelbll.GetTravelModel(list[0].ID);
                if (travelModel != null)
                {
                    //地接社名称
                    this.LitDjName.Text = travelModel.LocalTravelAgency;
                    //地接社编号
                    this.hideDiJieID.Value = travelModel.TravelAgencyID.ToString();
                    //获得地接社的返利
                    EyouSoft.Model.CompanyStructure.CompanySupplier djModel = new EyouSoft.BLL.CompanyStructure.CompanySupplier().GetModel(travelModel.TravelAgencyID, SiteUserInfo.CompanyID);
                    if (djModel != null)
                    {
                        this.hideCommission.Value = djModel.Commission.ToString("0.00");
                    }
                    //联系人
                    this.LitName.Text = travelModel.Contacter;
                    //电话
                    this.LitPhone.Text = travelModel.ContactTel;
                    //接团时间
                    this.LitBeginTime.Text = travelModel.ReceiveTime.ToString("yyyy-MM-dd");
                    //送团时间
                    this.LitEndTime.Text = travelModel.DeliverTime.ToString("yyyy-MM-dd");

                    //接待行程
                    IList<EyouSoft.Model.PlanStructure.LocalAgencyTourPlanInfo> tourList = travelModel.LocalAgencyTourPlanInfoList;
                    //设置选中
                    if (travelModel.LocalAgencyTourPlanInfoList != null && travelModel.LocalAgencyTourPlanInfoList.Count > 0)
                    {
                        for (int i = 0; i < travelModel.LocalAgencyTourPlanInfoList.Count; i++)
                        {
                            this.hideXingCheng.Value += travelModel.LocalAgencyTourPlanInfoList[i].Days + ",";
                        }
                    }

                    //判断 价格组成 为A or B
                    IList<EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo> priceList = travelModel.TravelAgencyPriceInfoList;
                    this.rptProjectB.DataSource = priceList;
                    this.rptProjectB.DataBind();

                    this.rptProjectA.DataSource = priceList;
                    this.rptProjectA.DataBind();

                    //团款
                    this.litTeamPrice.Text = travelModel.Fee.ToString("0.00");
                    //返款
                    this.litBackPrice.Text = travelModel.Commission.ToString("0.00");
                    //结算费用
                    this.litSettlePrice.Text = travelModel.Settlement.ToString("0.00");
                    //支付方式
                    this.litPayType.Text = travelModel.PayType.ToString();
                    //地接社须知
                    this.litNotes.Text = travelModel.Notice;
                    //备注
                    this.litRemarks.Text = travelModel.Remark;
                }
            }
        }
        #endregion

        protected void DataModelInti(string id, string planType)
        {
            EyouSoft.BLL.PlanStruture.TravelAgency travelbll = new EyouSoft.BLL.PlanStruture.TravelAgency();
            EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo travelModel = travelbll.GetTravelModel(id);
            if (travelModel != null)
            {
                //地接社名称
                this.LitDjName.Text = travelModel.LocalTravelAgency;
                //地接社编号
                this.hideDiJieID.Value = travelModel.TravelAgencyID.ToString();
                //获得地接社的返利
                EyouSoft.Model.CompanyStructure.CompanySupplier djModel = new EyouSoft.BLL.CompanyStructure.CompanySupplier().GetModel(travelModel.TravelAgencyID, SiteUserInfo.CompanyID);
                if (djModel != null)
                {
                    this.hideCommission.Value = djModel.Commission.ToString("0.00");
                }
                //联系人
                this.LitName.Text = travelModel.Contacter;
                //电话
                this.LitPhone.Text = travelModel.ContactTel;
                //接团时间
                this.LitBeginTime.Text = travelModel.ReceiveTime.ToString("yyyy-MM-dd");
                //送团时间
                this.LitEndTime.Text = travelModel.DeliverTime.ToString("yyyy-MM-dd");

                //接待行程
                IList<EyouSoft.Model.PlanStructure.LocalAgencyTourPlanInfo> tourList = travelModel.LocalAgencyTourPlanInfoList;
                //设置选中
                if (travelModel.LocalAgencyTourPlanInfoList != null && travelModel.LocalAgencyTourPlanInfoList.Count > 0)
                {
                    for (int i = 0; i < travelModel.LocalAgencyTourPlanInfoList.Count; i++)
                    {
                        this.hideXingCheng.Value += travelModel.LocalAgencyTourPlanInfoList[i].Days + ",";
                    }
                }

                //判断 价格组成 为A or B
                IList<EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo> priceList = travelModel.TravelAgencyPriceInfoList;
                this.rptProjectB.DataSource = priceList;
                this.rptProjectB.DataBind();

                this.rptProjectA.DataSource = priceList;
                this.rptProjectA.DataBind();

                //团款
                this.litTeamPrice.Text = travelModel.Fee.ToString("0.00");
                //返款
                this.litBackPrice.Text = travelModel.Commission.ToString("0.00");
                //结算费用
                this.litSettlePrice.Text = travelModel.Settlement.ToString("0.00");
                //支付方式
                this.litPayType.Text = travelModel.PayType.ToString();                
                //地接社须知
                this.litNotes.Text = travelModel.Notice;
                //备注
                this.litRemarks.Text = travelModel.Remark;
            }
        }

        #region 设置分页
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

        #region 行程接待Model
        public class Jiedai
        {
            public Jiedai() { }
            #region model
            private string _traveDate;
            //行程日期
            public string TraveDate
            {
                get { return _traveDate; }
                set { _traveDate = value; }
            }
            private string _planContent;
            //行程内容
            public string PlanContent
            {
                get { return _planContent; }
                set { _planContent = value; }
            }
            #endregion
        }
        #endregion

        #region 设置弹窗
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion
    }
}
