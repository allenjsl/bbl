/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) TravelSky 2011
/// 模块名称：团支出应收账款
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\Default.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-19 17:04:21
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

namespace Web.caiwuguanli
{
    /// <summary>
    /// 付款
    /// </summary>
    public partial class zctuankuan_fukuan : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记, false);
                }
                Bind();
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        void Bind()
        {
            txt_teamName.Value = "";
            txt_teamNum.Value = "";
            txt_teamStatus.Value = "";
            txt_teamTraffic.Value = "";
            txt_PaidMoney.Value = "";
            string tourId = Utils.GetQueryStringValue("tourId");
            EyouSoft.BLL.PlanStruture.TravelAgency bll = new EyouSoft.BLL.PlanStruture.TravelAgency();
            EyouSoft.BLL.TourStructure.Tour tour = new EyouSoft.BLL.TourStructure.Tour();
            EyouSoft.Model.TourStructure.TourBaseInfo info=  tour.GetTourInfo(tourId);
            txt_teamNum.Value = info.TourCode;
            txt_teamName.Value = info.RouteName;
            txt_teamStatus.Value = info.Status.ToString();
            txt_teamTraffic.Value = Utils.FilterEndOfTheZeroDecimal(info.TotalExpenses);
            //txt_PaidMoney .Value=Utils.FilterEndOfTheZeroDecimal(info.
            //lt_seller.Text = info.SellerName;
            //lt_actor.Text = info.OperatorId.ToString();

            EyouSoft.BLL.TourStructure.TourOrder orderbll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourOrder ordermodel = new EyouSoft.Model.TourStructure.TourOrder();

            IList<EyouSoft.Model.StatisticStructure.StatisticOperator> listSeller = orderbll.GetSalerInfo(tourId);
            IList<string> listOprator = tour.GetTourCoordinators(tourId);

            string sellers = "";
            string oprator = "";
            foreach (var v in listSeller)
            {
                sellers += v.OperatorName+",";
            }
            if (sellers.Length > 0)
            {
                sellers=sellers.TrimEnd(',');
            }
            foreach (var v in listOprator)
            {
                oprator += v + ",";
            }
            if (oprator.Length > 0)
            {
                oprator=oprator.TrimEnd(',');
            }
            lt_seller.Text = sellers;
            lt_actor.Text = oprator;


            IList<EyouSoft.Model.PlanStructure.PaymentList> list = bll.GetSettleList(tourId);
            IList<EyouSoft.Model.PlanStructure.PaymentList> listPiaowu = list.Where(x => x.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务).ToList();
            IList<EyouSoft.Model.PlanStructure.PaymentList> listDiJie = list.Where(x => x.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接).ToList();
            rpt_dijie.DataSource = listDiJie;
            rpt_piaowu.DataSource = listPiaowu;
            rpt_dijie.DataBind();
            rpt_piaowu.DataBind();
        }
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int getType(int type)
        {
            switch (type)
            {
                case 1: {
                    return type;
                } 
                case 2: { return type; }
                default:{
                    return 0;
                }
            }
        }
    }
}
