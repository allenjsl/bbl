using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.sanping
{
    public partial class chengrenj : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 价格等级
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> sinfo = null;
        /// <summary>
        /// 全局行号
        /// </summary>
        public int i = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomers();
            }
        }

        /// <summary>
        /// 绑定客户等级
        /// </summary>
        void BindCustomers()
        {
            //List<Customers> list = new List<Customers>();
            //list.Add(new Customers(3, "客户等级"));
            //rpt_Customer.DataSource = list;
            //rpt_Customer.DataBind();

            EyouSoft.BLL.TourStructure.Tour tour = new EyouSoft.BLL.TourStructure.Tour();
            EyouSoft.Model.TourStructure.TourInfo binfo = new EyouSoft.Model.TourStructure.TourInfo();
            binfo = (EyouSoft.Model.TourStructure.TourInfo)tour.GetTourInfo(Request.QueryString["tourId"]);
            EyouSoft.BLL.TourStructure.Tour bl = new EyouSoft.BLL.TourStructure.Tour();
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> listStand = binfo.PriceStandards;

            IList<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo> listCust = listStand.First().CustomerLevels;
            


            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();
            
            list = bll.GetCustomStandByCompanyId(CurrentUserCompanyID);
            int kkk = list.Count;
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> plist = binfo.PriceStandards;
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

        }
        #region 设置弹出窗体
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion
    }
}
