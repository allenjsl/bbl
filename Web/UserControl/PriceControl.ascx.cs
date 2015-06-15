using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.SSOComponent.Entity;

namespace Web.UserControl
{
    /// <summary>
    /// 计划价格组成
    /// 功能：价格组成
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-14
    /// </summary>
    public partial class PriceControl : System.Web.UI.UserControl
    {
        //设置list
        private IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> _setList;

        /// <summary>
        /// 人数配置 by 田想兵2011.7.12
        /// </summary>
        protected int NumConfig = 0;
        public IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> SetList
        {
            get { return _setList; }
            set { _setList = value; }
        }
        //获得list
        private IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> _getList;

        public IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> GetList
        {
            get { return _getList; }
            set { _getList = value; }
        }
        //设置我社报价合计
        private decimal totalAmount;

        public decimal TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }
        /// <summary>
        /// 成人单价
        /// </summary>
        public decimal cr_price { get; set; }
        /// <summary>
        /// 儿童单价
        /// </summary>
        public decimal rt_price { get; set; }
        /// <summary>
        /// 全陪单价
        /// </summary>
        public decimal all_price { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetDataList();
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                GetDataList();
                base.OnLoad(e);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();

            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            EyouSoft.BLL.TourStructure.TourOrder orderbll = new EyouSoft.BLL.TourStructure.TourOrder(userInfo);
            NumConfig = (int)(new EyouSoft.BLL.CompanyStructure.CompanySetting().GetTeamNumberOfPeople(userInfo.CompanyID));
            SetDataList();
            base.OnPreRender(e);
        }

        #region 为GetList属性 赋值
        /// <summary>
        /// 获得页面数据
        /// </summary>
        protected void GetDataList()
        {
            #region 价格组成
            string[] sltPriceArray = Utils.GetFormValues("selectPrice");
            string[] standardArray = Utils.GetFormValues("Standard");
            string[] djPriceArrray = Utils.GetFormValues("txtDjPrice");
            string[] wsPriceArray = Utils.GetFormValues("txtWsPrice");
            string[] oneDjPrice = Utils.GetFormValues("oneDjPrice");
            string[] oneDjCount = Utils.GetFormValues("oneDjCount");
            string[] oneWsPrice = Utils.GetFormValues("oneWsPrice");
            string[] oneWsCount = Utils.GetFormValues("oneWsCount");
            this.TotalAmount = Utils.GetDecimal(Utils.GetFormValue(this.txtAllPrice.UniqueID));
            //this.OnePriceAll = Utils.GetDecimal(Utils.GetFormValue(this.txtOnePriceAll.UniqueID));
            this.cr_price = Utils.GetDecimal(txt_crPrice.Value);
            this.rt_price = Utils.GetDecimal(txt_rtPrice.Value);
            this.all_price = Utils.GetDecimal(txt_allPrice.Value);
            #endregion
            //if (sltPriceArray != null && sltPriceArray.Count() <= 0)
            //{
            //    Response.Write("<script>javascript:window.alert('请输入价格组成信息!')</script>");
            //    return;
            //}

            if (sltPriceArray != null && standardArray != null && djPriceArrray != null && wsPriceArray != null && oneDjPrice != null && oneDjCount != null && oneWsPrice != null && oneWsCount != null)
            {
                if (sltPriceArray.Count() == standardArray.Count() && djPriceArrray.Count() == wsPriceArray.Count() && sltPriceArray.Count() > 0 && oneDjPrice.Count() == oneDjCount.Count() && oneDjCount.Count() == oneWsPrice.Count() && oneWsPrice.Count() == oneWsCount.Count() && oneWsCount.Count() > 0)
                {
                    IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> list = new List<EyouSoft.Model.TourStructure.TourTeamServiceInfo>();
                    for (int i = 0; i < sltPriceArray.Count(); i++)
                    {
                        if (sltPriceArray[i].Trim() != "-1")
                        {
                            EyouSoft.Model.TourStructure.TourTeamServiceInfo model = new EyouSoft.Model.TourStructure.TourTeamServiceInfo();
                            model.ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), sltPriceArray[i]);
                            model.Service = standardArray[i];
                            model.LocalPrice = Utils.GetDecimal(djPriceArrray[i]);
                            model.SelfPrice = Utils.GetDecimal(wsPriceArray[i]);
                            model.LocalUnitPrice = Utils.GetDecimal(oneDjPrice[i]);
                            model.LocalPeopleNumber = Utils.GetInt(oneDjCount[i]);
                            model.SelfUnitPrice = Utils.GetDecimal(oneWsPrice[i]);
                            model.SelfPeopleNumber = Utils.GetInt(oneWsCount[i]);
                            list.Add(model);
                        }
                    }
                    this.GetList = list;
                }
            }


        }
        #endregion

        #region 页面控件赋值
        /// <summary>
        /// 设置rpeate控件数据
        /// </summary>
        protected void SetDataList()
        {
            IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> list = this.SetList;
            string project = "";
            IList<EnumObj> proList = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), new string[] { ((int)EyouSoft.Model.EnumType.TourStructure.ServiceType.签证).ToString() });
            if (proList != null && proList.Count > 0)
            {
                project += "{value:\"-1\",text:\"--请选择--\"}|";
                for (int i = 0; i < proList.Count; i++)
                {
                    project += "{value:\"" + proList[i].Value + "\",text:\"" + proList[i].Text + "\"}|";
                }
                project = project.TrimEnd('|');
            }
            this.hideProList.Value = project;

            if (this.SetList != null && this.SetList.Count > 0)
            {
                this.hidePrice.Value = this.SetList.Count.ToString();
                this.rptList.DataSource = this.SetList;
                this.rptList.DataBind();

            }

            this.txtAllPrice.Value = Utils.FilterEndOfTheZeroDecimal(this.TotalAmount);
            txt_allPrice.Value = Utils.FilterEndOfTheZeroDecimal(this.all_price);
            txt_crPrice.Value = Utils.FilterEndOfTheZeroDecimal(this.cr_price);
            txt_rtPrice.Value = Utils.FilterEndOfTheZeroDecimal(this.rt_price);
            // this.txtOnePriceAll.Value = Utils.FilterEndOfTheZeroDecimal(this.OnePriceAll);
        }
        #endregion

    }
}