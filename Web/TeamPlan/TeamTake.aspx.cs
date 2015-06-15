using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Common.Enum;


namespace Web.TeamPlan
{
    /// <summary>
    /// 团队计划地接
    /// 功能：团队计划地接信息
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-13 
    /// </summary>

    public partial class TeamTake : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion

        protected bool isA = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //计划类型: 快速 or 标准
                string planType = Utils.GetQueryStringValue("planType");
                //操作类型 : 散拼 or 团队
                string type = Utils.GetQueryStringValue("type");
                //修改记录的ID
                string id = Utils.GetQueryStringValue("id");
                //计划ID
                string planId = Utils.GetQueryStringValue("planId");
                //地接ID
                string travelId = Utils.GetQueryStringValue("travelId");
                //分页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                //操作类型
                string doType = Utils.GetQueryStringValue("doType");
                switch (doType)
                {
                    case "Update":
                        this.hideDoType.Value = "Update";
                        break;
                    case "View":
                        this.hideDoType.Value = "View";
                        break;
                    default:
                        this.hideDoType.Value = "Add";
                        break;
                }
                if (id != "")
                {
                    this.hideId.Value = id;
                }
                if (planId != "")
                {
                    this.hidePlanId.Value = planId;
                }
                if (travelId != "")
                {
                    this.hideDiJieID.Value = travelId;
                }
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

                if (type == "team")
                {
                    //团队计划权限判断
                    if (CheckGrant(TravelPermission.团队计划_团队计划_栏目))
                    {
                        if (CheckGrant(TravelPermission.团队计划_团队计划_安排地接))
                        {
                            this.Page.Title = "团队计划_安排地接";
                        }
                        else
                        {
                            Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_安排地接, false);
                        }
                    }
                    else
                    {
                        Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_栏目, false);
                    }

                }
                else
                {

                    //散拼计划权限判断
                    if (CheckGrant(TravelPermission.散拼计划_散拼计划_栏目))
                    {
                        if (CheckGrant(TravelPermission.散拼计划_散拼计划_安排地接))
                        {
                            this.Page.Title = "散拼计划_安排地接";
                        }
                        else
                        {
                            Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_安排地接, false);
                        }
                    }
                    else
                    {
                        Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_栏目, false);
                    }
                }

                //获得当前A类 or B类
                EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll

                EyouSoft.Model.CompanyStructure.CompanyFieldSetting set = setBll.GetSetting(SiteUserInfo.CompanyID);
                if (set != null)
                {
                    if (set.PriceComponent == EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.分项报价)
                    {
                        isA = true;
                        hideTypeAorB.Value = "B";
                        this.pnlProjectA.Visible = false;
                        this.pnlProjectB.Visible = true;
                    }
                    else
                    {
                        isA = false;
                        hideTypeAorB.Value = "A";
                        this.pnlProjectA.Visible = true;
                        this.pnlProjectB.Visible = false;
                    }
                }
                // 初始化
                DataListInit(planId);
                //下拉框初始化
                DdlProjectListInit();
                if (id != "" && planType != "")
                {
                    DataModelInti(id, planType);
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
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
                if (baseModel != null)
                {
                    if (baseModel.Status == EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束 || baseModel.Status == EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算)
                    {
                        this.pnlEdit.Visible = false;
                        this.hideIsEidt.Value = "No";
                    }
                }

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
                    this.txtBeginTime.Text = infoModel.LDate.ToString("yyyy-MM-dd");
                    this.txtEndTime.Text = infoModel.RDate.ToString("yyyy-MM-dd");
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
                    this.txtBeginTime.Text = teamModel.LDate.ToString("yyyy-MM-dd");
                    this.txtEndTime.Text = teamModel.RDate.ToString("yyyy-MM-dd");
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

        protected void DataModelInti(string id, string planType)
        {
            EyouSoft.BLL.PlanStruture.TravelAgency travelbll = new EyouSoft.BLL.PlanStruture.TravelAgency();
            //travelbll.GetTravelModel("");
            EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo travelModel = travelbll.GetTravelModel(id);

            if (travelModel != null)
            {

                //地接社名称
                this.txtDiJieName.Text = travelModel.LocalTravelAgency;
                //地接社编号
                this.hideDiJieID.Value = travelModel.TravelAgencyID.ToString();
                //获得地接社的返利

                EyouSoft.Model.CompanyStructure.CompanySupplier djModel = new EyouSoft.BLL.CompanyStructure.CompanySupplier().GetModel(travelModel.TravelAgencyID, SiteUserInfo.CompanyID);
                if (djModel != null)
                {
                    this.hideCommission.Value = djModel.Commission.ToString("0.00");
                }

                //联系人
                this.txtContact.Text = travelModel.Contacter;
                //电话
                this.txtPhone.Text = travelModel.ContactTel;
                //接团时间
                this.txtBeginTime.Text = travelModel.ReceiveTime.ToString("yyyy-MM-dd");
                //送团时间
                this.txtEndTime.Text = travelModel.DeliverTime.ToString("yyyy-MM-dd");
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
                this.txtTeamPrice.Text = travelModel.Fee.ToString("0.00");
                //返款
                this.txtBackPrice.Text = travelModel.Commission.ToString("0.00");
                //结算费用
                this.txtSettlePrice.Text = travelModel.Settlement.ToString("0.00");
                //支付方式
                DdlPayListInit(Convert.ToInt32(travelModel.PayType).ToString());
                //地接社须知
                this.txtNotes.Text = travelModel.Notice;
                //备注
                this.txtRemarks.Text = travelModel.Remark;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //地接社名称
            string diJieName = Utils.GetFormValue(this.txtDiJieName.UniqueID);
            //地接社ID
            string diJieId = Utils.GetFormValue(this.hideDiJieID.UniqueID);
            //联系人
            string contact = Utils.GetFormValue(this.txtContact.UniqueID);
            //电话
            string phone = Utils.GetFormValue(this.txtPhone.UniqueID);
            //接团时间
            DateTime? benginTime = Utils.GetDateTimeNullable(this.txtBeginTime.Text);
            //送团时间
            DateTime? endTime = Utils.GetDateTimeNullable(this.txtEndTime.Text);
            //接待行程
            string[] travel = Utils.GetFormValues("cbxTravel");

            #region 价格项目1
            //项目
            string[] sltProjectArray = Utils.GetFormValues("sltProject");
            //具体要求
            string[] Claim = Utils.GetFormValues("txtClaim");
            //价格项目
            string[] proPrice = Utils.GetFormValues("txtProPrice");
            #endregion

            #region 价格项目2
            //价格项目
            string[] pricePro = Utils.GetFormValues("txtPricePro");
            //备注
            string[] bak = Utils.GetFormValues("txtBak");
            //单人价格
            string[] onePrice = Utils.GetFormValues("txtOnePrice");
            //人数
            string[] peopleCount = Utils.GetFormValues("txtPeoCount");
            #endregion

            //团款
            decimal teamPrice = Utils.GetDecimal(this.txtTeamPrice.Text, 0);
            //反款
            decimal backPrice = Utils.GetDecimal(this.txtBackPrice.Text, 0);
            //结算费用
            decimal settlePrice = Utils.GetDecimal(this.txtSettlePrice.Text, 0);
            //支付类型
            string payType = Utils.GetFormValue(this.dllPayType.UniqueID);
            //地接社需知
            string notes = this.txtNotes.Text.Trim();
            //备注
            string remarks = this.txtRemarks.Text.Trim();

            #region 验证
            //地接社名 不能为空
            if (diJieName.TrimEnd() == "")
            {
                Utils.ShowAndRedirect("请选择地接社!", Request.Url.ToString());
                return;
            }
            //结算费用 不能为空
            if (this.txtSettlePrice.Text.Trim() == "")
            {
                Utils.ShowAndRedirect("请输入结算费用!", Request.Url.ToString());
                return;
            }


            EyouSoft.BLL.PlanStruture.TravelAgency travelbll = new EyouSoft.BLL.PlanStruture.TravelAgency();
            //声明对象model
            EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo travelModel = null;
            //判断如果是修改 则getmodel 反之 new 一个新的对象
            if (this.hideDoType.Value == "Update")
            {
                travelModel = travelbll.GetTravelModel(this.hideId.Value);
            }
            else
            {
                travelModel = new EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo();
            }
            if (travelModel != null)
            {
                //地接ID
                travelModel.TravelAgencyID = Utils.GetInt(this.hideDiJieID.Value);
                //地接名称
                travelModel.LocalTravelAgency = diJieName;
                //联系人
                travelModel.Contacter = contact;
                //联系电话
                travelModel.ContactTel = phone;
                //接团时间
                travelModel.ReceiveTime = Convert.ToDateTime(benginTime);
                //送团时间
                travelModel.DeliverTime = Convert.ToDateTime(endTime);
                //行程接待
                IList<EyouSoft.Model.PlanStructure.LocalAgencyTourPlanInfo> planList = new List<EyouSoft.Model.PlanStructure.LocalAgencyTourPlanInfo>();
                if (this.hidePlanType.Value == "Quick")
                {
                    this.pnlJieDai.Visible = false;
                }
                else
                {
                    if (travel != null && travel.Length > 0)
                    {
                        for (int i = 0; i < travel.Length; i++)
                        {
                            EyouSoft.Model.PlanStructure.LocalAgencyTourPlanInfo localTourModel = new EyouSoft.Model.PlanStructure.LocalAgencyTourPlanInfo();
                            localTourModel.Days = (i + 1);
                            localTourModel.LocalTravelId = this.hideDiJieID.Value;
                            planList.Add(localTourModel);
                        }
                    }

                }
                //价格组成
                if (hideTypeAorB.Value == "B")//B类
                {
                    if (sltProjectArray != null && Claim != null && proPrice != null && sltProjectArray.Count() == Claim.Count() && Claim.Count() == proPrice.Count()) //判断数组count 是否一致
                    {
                        IList<EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo> tList = new List<EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo>();
                        for (int i = 0; i < sltProjectArray.Count(); i++)
                        {
                            //声明价格组成model
                            EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo tModel = new EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo();
                            //B类实体赋值
                            tModel.PriceTpyeAorB = EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.分项报价;
                            tModel.PriceTpyeId = (EyouSoft.Model.EnumType.TourStructure.ServiceType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), sltProjectArray[i]);
                            tModel.Remark = Claim[i];
                            tModel.ReferenceID = this.hideDiJieID.Value;
                            tModel.Price = Utils.GetDecimal(proPrice[i]);
                            // tModel.ID = travelModel.ID;
                            tList.Add(tModel);
                        }
                        travelModel.TravelAgencyPriceInfoList = tList;
                    }
                }
                else  //A类
                {
                    if (pricePro != null && bak != null && onePrice != null && peopleCount != null && pricePro.Count() == bak.Count() && pricePro.Count() == onePrice.Count() && pricePro.Count() == peopleCount.Count())
                    {
                        IList<EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo> tList = new List<EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo>();
                        for (int i = 0; i < pricePro.Count(); i++)
                        {
                            //声明价格组成model
                            EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo tModel = new EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo();
                            //A类实体赋值
                            tModel.PriceTpyeAorB = EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.统一报价;
                            tModel.Title = pricePro[i];
                            tModel.Remark = bak[i];
                            tModel.Price = Utils.GetDecimal(onePrice[i]);
                            tModel.PeopleCount = Utils.GetInt(peopleCount[i]);
                            tList.Add(tModel);
                        }
                        travelModel.TravelAgencyPriceInfoList = tList;
                    }
                }
                //团款
                travelModel.Fee = teamPrice;
                //反款
                travelModel.Commission = backPrice;
                //结算费用
                travelModel.Settlement = settlePrice;
                //支付类型
                travelModel.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType), payType);
                //地接社需知
                travelModel.Notice = notes;
                //行程接待
                travelModel.LocalAgencyTourPlanInfoList = planList;
                //备注
                travelModel.Remark = remarks;
                //公司ID
                travelModel.CompanyId = SiteUserInfo.CompanyID;

                travelModel.TourId = this.hidePlanId.Value;

                travelModel.OperateTime = DateTime.Now;

                travelModel.OperatorID = SiteUserInfo.ID;

                travelModel.Operator = SiteUserInfo.ContactInfo.ContactName;

                if (this.hideDoType.Value == "Update")
                {
                    travelModel.OperateTime = DateTime.Now;
                    if (travelbll.UpdateTravelAgency(travelModel))
                    {
                        Utils.ShowAndRedirect("修改成功!", Request.Url.ToString());
                        return;
                    }
                    else
                    {
                        Utils.ShowAndRedirect("修改失败!", Request.Url.ToString());
                        return;
                    }
                }
                else
                {
                    //add model
                    if (travelbll.AddTravel(travelModel))
                    {
                        Utils.ShowAndRedirect("新增成功!", Request.Url.ToString());
                        return;
                    }
                    else
                    {
                        Utils.ShowAndRedirect("新增失败!", Request.Url.ToString());
                        return;
                    }
                }


            }

            #endregion

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


        #region 支付方式 下拉框 列表初始化
        /// <summary>
        /// 支付方式 下拉框 列表初始化
        /// </summary>
        /// <param name="index"></param>
        protected void DdlPayListInit(string index)
        {
            this.dllPayType.Items.Clear();
            this.dllPayType.Items.Add(new ListItem("--请选择--", "-1"));
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType));
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = list[i].Value;
                    item.Text = list[i].Text;
                    this.dllPayType.Items.Add(item);
                }
            }
            if (index != "")
            {
                this.dllPayType.SelectedValue = index;
            }
            list = null;
        }
        #endregion

        #region 价格组成： 项目下拉框
        /// <summary>
        /// 价格组成： 项目下拉框
        /// </summary>
        protected void DdlProjectListInit()
        {
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType));
            string project = "";
            if (list != null && list.Count > 0)
            {
                project += "{value:\"-1\",text:\"--请选择--\"}|";
                for (int i = 0; i < list.Count; i++)
                {
                    project += "{value:\"" + list[i].Value + "\",text:\"" + list[i].Text + "\"}|";
                }
            }
            project = project.TrimEnd('|');
            this.hideProjectList.Value = project;
        }
        #endregion
    }

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
}
