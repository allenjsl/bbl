using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.GroupEnd.Orders
{
    /// <summary>
    /// 线路联系人信息
    /// 创建时间:2011-01-19 
    /// 创建人:lixh
    /// </summary>
    public partial class LineInfo : Eyousoft.Common.Page.FrontPage
    {
        protected int CompanyID = 1; //当前登录公司编号
        protected void Page_Load(object sender, EventArgs e)
        {
            EyouSoft.Model.TourStructure.LBZTTours model = new EyouSoft.Model.TourStructure.LBZTTours();
            EyouSoft.BLL.TourStructure.Tour tour = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourCoordinatorInfo con =new EyouSoft.Model.TourStructure.TourCoordinatorInfo();
            EyouSoft.Model.TourStructure.TourBaseInfo baseInfo =new EyouSoft.Model.TourStructure.TourBaseInfo();

            if (!this.Page.IsPostBack)
            {
                string TourID = EyouSoft.Common.Utils.GetQueryStringValue("TourID");
                if (!string.IsNullOrEmpty(TourID))
                {  
                   
                    //tour.
                   // Model_ContactPersonInfo = Bll_CompanyUser.GetUserBasicInfo(SiteUserInfo.CompanyID);
                    //if (Model_ContactPersonInfo != null)
                    //{
                    //    //this.Lab_Auothor.Text = Model_ContactPersonInfo.ContactName;
                    //   // this.Lab_Phone.Text = Model_ContactPersonInfo.ContactTel;
                    //    //this.Lab_QQ.Text = Model_ContactPersonInfo.QQ;
                    //}
                }
            }
        }

        #region 设置弹窗页面
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion
    }

}
