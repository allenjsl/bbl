using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.BLL;
using EyouSoft.Model;
using EyouSoft.Common;
using System.Text;

namespace Web.sanping
{
    /// <summary>
    /// 生成标准计划
    /// by 田想兵 2011.3.25
    /// </summary>
    public partial class GenerationQickPlan :Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 报价标准
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> sinfo = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInfo();
            }
        }
        /// <summary>
        /// 绑定信息
        /// </summary>
        void BindInfo()
        {
            EyouSoft.BLL.TourStructure.TourEveryday bl = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
            string tourId = Utils.GetQueryStringValue("tourid");
            string ApplyId = Utils.GetQueryStringValue("ApplyId");
            EyouSoft.Model.TourStructure.TourEverydayInfo model = bl.GetTourEverydayInfo(tourId);

            #region 价格绑定
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> listStand = model.PriceStandards;

            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();

            list = bll.GetCustomStandByCompanyId(CurrentUserCompanyID);
            int kkk = list.Count;

            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> plist = model.PriceStandards;
            for (int i = 0; i < listStand.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    //if(listStand[i].CustomerLevels[i].LevelId == list.sel)
                    if (listStand[i].CustomerLevels.Count > j)
                    {
                        var vn = list.Where(x => x.Id == listStand[i].CustomerLevels[j].LevelId).FirstOrDefault();
                        if (vn != null)
                            listStand[i].CustomerLevels[j].LevelName = vn.CustomStandName;
                        else
                            listStand[i].CustomerLevels.RemoveAt(j);
                    }
                    var xn = listStand[i].CustomerLevels.Where(x => x.LevelId == list[j].Id).FirstOrDefault();
                    if (xn == null)
                        listStand[i].CustomerLevels.Add(new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo() { LevelId = list[j].Id, LevelType = list[j].LevType, LevelName = list[j].CustomStandName, AdultPrice = 0, ChildrenPrice = 0 });
                }
            }
            sinfo = plist;
            #endregion
            hd_area.Value = model.AreaId.ToString();//线路区域
            EyouSoft.Model.TourStructure.TourEverydayApplyInfo applyModel = new EyouSoft.Model.TourStructure.TourEverydayApplyInfo();
            applyModel = bl.GetTourEverydayApplyInfo(ApplyId);
            hd_Ldate.Value = applyModel.LDate.ToShortDateString();
            txt_ykrs.Value = applyModel.PeopleNumber.ToString();
            hd_num.Value = applyModel.PeopleNumber.ToString();
        }

        /// <summary>
        /// 报价标准
        /// </summary>
        public string BindPriceList()
        {
            StringBuilder sb = new StringBuilder();
            EyouSoft.BLL.CompanyStructure.CompanyPriceStand price = new EyouSoft.BLL.CompanyStructure.CompanyPriceStand();
            IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> pricelist = new List<EyouSoft.Model.CompanyStructure.CompanyPriceStand>();
            pricelist = price.GetPriceStandByCompanyId(CurrentUserCompanyID);
            foreach (var v in pricelist)
            {
                sb.Append("<option value=\"" + v.Id.ToString() + "|" + v.PriceStandName + "\">" + v.PriceStandName + "</option>");
            }
            pricelist = null;
            return sb.ToString();
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
                base.OnPreInit(e);
            }
        }
        /// <summary>
        /// 生成计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.TourStructure.TourInfo info = new EyouSoft.Model.TourStructure.TourInfo();
            EyouSoft.BLL.TourStructure.TourEveryday bl = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
            #region 基础数据
            string tourId = Utils.GetQueryStringValue("tourid");
            string ApplyId = Utils.GetQueryStringValue("ApplyId");
            EyouSoft.Model.TourStructure.TourEverydayInfo model = bl.GetTourEverydayInfo(tourId);
            EyouSoft.Model.TourStructure.TourEverydayApplyInfo applyModel = new EyouSoft.Model.TourStructure.TourEverydayApplyInfo();
            applyModel = bl.GetTourEverydayApplyInfo(ApplyId);
            info.AreaId = model.AreaId;
            info.Attachs = model.Attachs;

            string tourcode = Utils.GetFormValue("txt_tuanhao");
            if (tourcode.Trim().Length == 0)
            {
                EyouSoft.Common.Function.MessageBox.Show(this, "团号不能为空!");
                BindInfo();
                return;
            }
            #region 子团
            IList<EyouSoft.Model.TourStructure.TourChildrenInfo> childlist = new
            List<EyouSoft.Model.TourStructure.TourChildrenInfo>();
            childlist.Add(new EyouSoft.Model.TourStructure.TourChildrenInfo() { LDate = applyModel.LDate, TourCode = tourcode });
            info.Childrens = childlist;
            #endregion
            info.CompanyId = CurrentUserCompanyID;
            info.Coordinator = new EyouSoft.Model.TourStructure.TourCoordinatorInfo() { CoordinatorId = Utils.GetInt(Utils.GetFormValue("sel_oprator")) };
            info.CreateRule = new EyouSoft.Model.TourStructure.TourCreateRuleInfo() { Cycle = null, EDate = null, Rule = EyouSoft.Model.EnumType.TourStructure.CreateTourRule.按日期, SDate = applyModel.LDate };
            info.CreateTime = DateTime.Now;
            info.Gather = Utils.GetFormValue("txt_jhfs");
            info.GatheringPlace = Utils.GetFormValue("txt_jhdd");
            info.GatheringSign = Utils.GetFormValue("txt_jhbz");
            info.GatheringTime = Utils.GetFormValue("txt_jhdate");
            info.LDate = applyModel.LDate;
            info.LTraffic = Utils.GetFormValue("txt_qcDate");
            info.OperatorId = SiteUserInfo.ID;
            info.ORouteId = model.RouteId;
            info.PlanPeopleNumber = Utils.GetInt(Utils.GetFormValue("txt_ykrs"));
            info.PriceStandards = model.PriceStandards;
            //info.RDate
            info.ReleaseType = EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick;
            info.RouteId = model.RouteId;
            info.RouteName = model.RouteName;
            info.RTraffic = Utils.GetFormValue("txt_hcDate");
            //info.SellerId
            //info.SellerName

            info.Status = EyouSoft.Model.EnumType.TourStructure.TourStatus.正在收客;
            info.TicketStatus = EyouSoft.Model.EnumType.PlanStructure.TicketState.None;
            info.TourCode = Utils.GetFormValue("txt_tuanhao");
            info.TourDays = model.TourDays;
            //info.TourId
            info.TourQuickInfo = new EyouSoft.Model.TourStructure.TourQuickPrivateInfo() { QuickPlan=model.TourQuickInfo.QuickPlan, Remark= model.TourQuickInfo.Remark, Service=model.TourQuickInfo.Service };
            info.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;
            info.VirtualPeopleNumber = 0;
            #endregion
            int j = bl.BuildTour(info, tourId, ApplyId);
            if (j > 0)
            {
                Response.Write("<script>alert('生成计划成功！');parent.location.href=parent.location.href;</script>");
                BindInfo();
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.Show(this, "生成失败");
                BindInfo();
            }
        }
    }
}
