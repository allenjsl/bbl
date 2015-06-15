using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.UserCenter.DjArrangeMengs
{
    public partial class ArrangeMengsList : Eyousoft.Common.Page.AreaConnectPage
    {
        /// <summary>
        /// 个人中心 地接安排
        /// 2011-04-06
        /// 李晓欢
        /// </summary>
        
        #region 变量
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        protected int len = 0; //总记录数
        #endregion

        //线路名称
        protected string RouteName = string.Empty;
        //出团时间
        protected DateTime? Starttime;
        //天数
        protected int? TourDays;

        //地接安排业务逻辑层和实体类
        EyouSoft.Model.TourStructure.TourInfo Tourinfo = null;
        EyouSoft.BLL.TourStructure.Tour Tourbll = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Tourbll = new EyouSoft.BLL.TourStructure.Tour();
            Tourinfo = new EyouSoft.Model.TourStructure.TourInfo();

            if (!IsPostBack)
            {
                //当前登录账户判断
                if (SiteUserInfo.ContactInfo.UserType != EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.地接用户)
                {
                    Response.Clear();
                    Response.Write("对不起，你不是地接用户没有权限查看!");                    
                    Response.End();
                }
                DateInit();
            }
        }

        #region 数据初始化
        protected void DateInit()
        {
            //初使化查询条件
            PageIndex = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("page"), 1);
            //线路名称
            RouteName = EyouSoft.Common.Utils.GetQueryStringValue("_RouteName");
            //出团时间
            Starttime = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("_Stratime"));
            //出游天数
            TourDays = EyouSoft.Common.Utils.GetIntNull(EyouSoft.Common.Utils.GetQueryStringValue("_Tourdays"));
            //地接安排查询实体
            EyouSoft.Model.TourStructure.TourSearchInfo Search=new EyouSoft.Model.TourStructure.TourSearchInfo ();
            //判断线路名称是否是空
            if (RouteName == null)
            {
                Search.RouteName = null;
            }
            else
            {
                Search.RouteName = RouteName;
            }
            //判断出团时间是否是空
            if (Starttime == null)
            {
                Search.SDate = null;
                Search.EDate = null;
            }
            else
            {
                Search.SDate = Starttime;
                Search.EDate = Starttime;
            }
            //判断出游天数是否是空
            if (TourDays == null)
            {
                Search.TourDays = null;
            }
            else
            {
                Search.TourDays = TourDays;
            }
            //地接安排列表实体
            IList<EyouSoft.Model.TourStructure.LBDJAPTourInfo> list = null;
            list = Tourbll.GetToursDJAP(SiteUserInfo.CompanyID, SiteUserInfo.LocalAgencyCompanyInfo.CompanyId, PageSize, PageIndex, ref RecordCount, Search);
            if (list != null && list.Count > 0)
            {
                this.len = list.Count;
                //数据绑定
                this.Replist.DataSource = list;
                this.Replist.DataBind();
                //设置分页
                BindPage();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
            }
        }
        #endregion

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
        #endregion

        #region 获取地接行程打印单
        protected string GetPrintUrl(string tourId)
        {
            string Url = string.Empty;
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour tourBll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourBaseInfo tourModel = tourBll.GetTourInfo(tourId);
            //声明bll对象
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();

            //判断该计划是团队计划
            if (tourModel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
            {
                EyouSoft.Model.TourStructure.TourTeamInfo teamModel = (EyouSoft.Model.TourStructure.TourTeamInfo)tourModel;
                //行程单
                if (teamModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    //团队计划快速发布行程单
                    Url = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划快速发布行程单);
                }
                else
                {
                    //团队计划标准版发布行程单
                    Url = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划标准发布行程单);
                }
            }

            //判断该计划是散拼计划
            if (tourModel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
            {
                EyouSoft.Model.TourStructure.TourInfo teamModel = (EyouSoft.Model.TourStructure.TourInfo)tourModel;
                if (teamModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                {
                    //散拼计划快速发布行程单
                    Url = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划标准发布行程单);
                }
                if (teamModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    //散拼计划标准发布行程单
                    Url = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划快速发布行程单);
                }
            }
            return Url;
        }
        #endregion



    }
}
