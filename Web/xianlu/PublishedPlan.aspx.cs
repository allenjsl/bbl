using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Eyousoft.Common;
using EyouSoft.Common.Function;
using System.Text;
using EyouSoft.Common;

namespace Web.xianlu
{
    /// <summary>
    /// 模块名称:线路信息列表发布计划弹出页面
    /// 创建时间:2011-01-12
    /// 创建人:lixh
    /// </summary>
    public partial class PublishedPlan : Eyousoft.Common.Page.BackPage
    {
        public int companyid { get; set; }
        public int xlId = 0;
        public string hdOprator = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("act") == "ExitTourNo")
            {
                Response.Clear();
                EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
                if (bll.ExistsTourCodes(CurrentUserCompanyID, Utils.GetFormValue("tourid"), Utils.GetFormValue("tourCode")).Count == 0)
                {
                    Response.Write("0");
                }
                else
                {
                    Response.Write("1");
                }
                Response.End();
            }
            if (!this.Page.IsPostBack)
            {
                xlId = EyouSoft.Common.Utils.GetInt(Request.QueryString["id"]);

                BindCustomers();
                BindPriceList();
            }
        }

        /// <summary>
        /// 报价标准
        /// </summary>
        public void BindPriceList()
        {
            EyouSoft.BLL.CompanyStructure.CompanyPriceStand price = new EyouSoft.BLL.CompanyStructure.CompanyPriceStand();
            int kkk = 0;
            IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> pricelist = new List<EyouSoft.Model.CompanyStructure.CompanyPriceStand>();
            pricelist = price.GetList(100, 1, ref kkk, CurrentUserCompanyID);
            this.ddl_price.Items.Clear();
            this.ddl_price.Items.Add(new ListItem("请选择报价标准", ""));
            if (pricelist.Count > 0)
            {
                for (int i = 0; i < pricelist.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = pricelist[i].Id + "|" + pricelist[i].PriceStandName;
                    item.Text = pricelist[i].PriceStandName;
                    this.ddl_price.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 绑定客户等级
        /// </summary>
        void BindCustomers()
        {
            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();
            //int kkk = 0;
            list = bll.GetCustomStandByCompanyId(CurrentUserCompanyID);
            rpt_Customer.DataSource = list;
            rpt_Customer.DataBind();
        }

        #region 设置弹出窗体
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion

        void SaveInfo()
        {
            xlId = EyouSoft.Common.Utils.GetInt(Request.QueryString["id"]);
            EyouSoft.BLL.RouteStructure.Route rout = new EyouSoft.BLL.RouteStructure.Route();
            EyouSoft.Model.RouteStructure.RouteInfo model = rout.GetRouteInfo(xlId);
            EyouSoft.BLL.TourStructure.Tour tour = new EyouSoft.BLL.TourStructure.Tour();
            EyouSoft.Model.TourStructure.TourInfo info=new EyouSoft.Model.TourStructure.TourInfo();

            IList<EyouSoft.Model.CompanyStructure.CustomStand> listcus = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bllCom = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();

            int kkk = 0;
            listcus = bllCom.GetList(100, 1, ref kkk, CurrentUserCompanyID);
            info.AreaId = model.AreaId;
            info.Attachs = null;
            info.CompanyId = CurrentUserCompanyID;

            EyouSoft.Model.TourStructure.TourCreateRuleInfo rule = new EyouSoft.Model.TourStructure.TourCreateRuleInfo();
            rule.Cycle = null;
            rule.SDate = null;
            rule.Rule = EyouSoft.Model.EnumType.TourStructure.CreateTourRule.按日期;
            IList<EyouSoft.Model.TourStructure.TourChildrenInfo> childlist = new List<EyouSoft.Model.TourStructure.TourChildrenInfo>();
            string[] childteamnumber = EyouSoft.Common.Utils.GetFormValue("hidToursNumbers").Split(',');
            for (int i = 0; i < childteamnumber.Length; i++)
            {
                EyouSoft.Model.TourStructure.TourChildrenInfo cinfo = new EyouSoft.Model.TourStructure.TourChildrenInfo();
                cinfo.LDate = EyouSoft.Common.Utils.GetDateTime(childteamnumber[i].Split('{')[0]);
                cinfo.TourCode = childteamnumber[i].Split('}')[1];
                childlist.Add(cinfo);
            }

            info.Childrens = childlist;
            
            info.LTraffic = Txt_StartTraffic.Value;
            info.OperatorId = SiteUserInfo.ID;   //用户ID
            
            info.PlanPeopleNumber = EyouSoft.Common.Utils.GetInt(Txt_PreControlNumber.Value);

            info.Coordinator = new EyouSoft.Model.TourStructure.TourCoordinatorInfo();
            info.Coordinator.CoordinatorId = Utils.GetInt(Utils.GetFormValue("sel_oprator"));

            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> pricelist=new List<EyouSoft.Model.TourStructure.TourPriceStandardInfo>();
            for (int k = 0; k < Utils.GetFormValues("ddl_price").Length; k++)
            {
                EyouSoft.Model.TourStructure.TourPriceStandardInfo price = new EyouSoft.Model.TourStructure.TourPriceStandardInfo();
                price.StandardId = Utils.GetInt(Utils.GetFormValues("ddl_price")[k].Split('|')[0]);
                price.StandardName = Utils.GetFormValues("ddl_price")[k].Split('|')[1];
                IList<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo> listLevels = new List<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo>();


                string[] crPrice = Utils.GetFormValues("txt_cr_price");

                for (int i = 0; i < kkk; i++)
                {
                    EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo level = new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo();
                    level.AdultPrice = Utils.GetDecimal(Utils.GetFormValues("txt_cr_price")[i + kkk * k]);
                    level.ChildrenPrice = Utils.GetDecimal(Utils.GetFormValues("txt_rt_price")[i + kkk * k]);
                    level.LevelType = EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.其他;
                    level.LevelName = Utils.GetFormValues("hd_cusStandName")[i];
                    level.LevelId = Utils.GetInt(Utils.GetFormValues("hd_cusStandId")[i]);
                    listLevels.Add(level);
                }
                price.CustomerLevels = listLevels;
                pricelist.Add(price);
            }
            info.PriceStandards = pricelist;
            info.RouteId = Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("ID"));
            info.RouteName = model.RouteName;
            info.RTraffic = Txt_EndTraffic.Value;
            info.Status = EyouSoft.Model.EnumType.TourStructure.TourStatus.正在收客;

            info.TicketStatus = EyouSoft.Model.EnumType.PlanStructure.TicketState.None;

            model = rout.GetRouteInfo(info.RouteId);
            info.TourDays = model.RouteDays;

            info.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;
            if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
            {
                info.ReleaseType = model.ReleaseType;
                info.TourNormalInfo = model.RouteNormalInfo;
            }
            else
            {
                info.ReleaseType = model.ReleaseType;
                info.TourQuickInfo = model.RouteQuickInfo;
            }        

            if (tour.InsertTourInfo(info) > 0)
            {
                Response.Write("<script>alert('添加成功！');window.parent.location.href=window.parent.location.href;</script>");
            }
            else
            {
                Response.Write("<script>alert('添加失败！');window.parent.location.href=window.parent.location.href;</script>");
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            SaveInfo();            
        }
    }
}
