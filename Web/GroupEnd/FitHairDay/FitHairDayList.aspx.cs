using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Function;
using EyouSoft.Common;

namespace Web.GroupEnd.FitHairDay
{
    /// <summary>
    /// 芭比来组团端 散客天天发
    /// 创建时间：2011-3-22
    /// 创建人：李晓欢
    /// </summary>
    public partial class FitHairDayList : Eyousoft.Common.Page.FrontPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        protected int CompanyID = 1;//当前登录的公司编号
        protected int lenght = 0; //列表count
        #endregion    

        //散客天天发实体类和业务逻辑类
        protected EyouSoft.Model.TourStructure.TourEverydayInfo TourEverydayinfo = null;
        EyouSoft.BLL.TourStructure.TourEveryday TourEverydaybll = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //实例化实体类和业务逻辑类
            TourEverydaybll = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo, false);
            TourEverydayinfo = new EyouSoft.Model.TourStructure.TourEverydayInfo();

            if (!this.Page.IsPostBack)
            {
                #region 定义变量接收参数           
                //线路名称
                string RouteName = Utils.GetQueryStringValue("RouteName");
                //初始化查询条件              
                if (RouteName != null && RouteName != "")
                {
                    this.RouteName.Value = RouteName;
                }
                #endregion
                Bindlist(RouteName);
            }
        }

        #region 绑定列表
        protected void Bindlist(string RouteName)
        {
            //分页
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            //散客天天查询实体
            EyouSoft.Model.TourStructure.TourEverydaySearchInfo TourSearch=new EyouSoft.Model.TourStructure.TourEverydaySearchInfo ();
            //线路区域编号集合
            TourSearch.Areas = SiteUserInfo.Areas;
            //线路名称
            TourSearch.RouteName=RouteName;

            //线路筛选
            if (Request.QueryString["xlid"] != "" && Request.QueryString["xlid"] != null)
            {
                TourSearch.AreaId = Utils.GetInt(Utils.GetQueryStringValue("xlid"));
            }

            IList<EyouSoft.Model.TourStructure.LBTourEverydayInfo> list = null;
            list = TourEverydaybll.GetTourEverydays(SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, TourSearch);
            if (list.Count > 0 && list != null)
            {
                lenght = list.Count;
                this.repeaterlist.DataSource = list;
                this.repeaterlist.DataBind();
                BindPage();              
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
            }
            
        }
        #endregion

        #region 绑定分页
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
        #endregion

        #region 线路区域名称
        protected string GetAreaName(string AreaId)
        {
            string AreaName=string.Empty;
            EyouSoft.Model.CompanyStructure.Area Area = new EyouSoft.BLL.CompanyStructure.Area().GetModel(Convert.ToInt32(AreaId));
            if (Area != null)
            {
                AreaName = Area.AreaName;
            }
            return AreaName;
        }
        #endregion

        public string getUrl(string tourid, int releaseType)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            if (releaseType == 0)
            {
                return bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路标准发布行程单);
            }
            else
            {
                return bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路快速发布行程单);
            }
        }
    }
}
