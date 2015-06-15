using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.print;
using EyouSoft.Common;
using System.Text;
using Common.Enum;

namespace Web.UserCenter.SendTasks
{
    /// <summary>
    /// 个人中心-送团任务表
    /// 柴逸宁
    /// 11.03.25
    /// </summary>
    /// 修改人:陈志仁   修改时间:2012-02-23
    /// 修改内容:添加出团时间段查询,条件控制修改成成员所在部门可见
    public partial class TasksList : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageSize = 20;
        protected int pageIndex;
        protected int recordCount;

        protected int len = 0;//列表长度

        //查询条件
        protected string LDate = string.Empty;//出团开始日期
        protected string LEDate = string.Empty;//出团结束日期
        protected string RouteName = string.Empty;//线路名称

        //送团单URL
        protected string printUrl = string.Empty;

        //权限变量
        //protected bool grantadd = false;//新增
        //protected bool grantmodify = false;//修改
        //protected bool grantdel = false;//删除
        //protected bool grantload = false;//导入
        //protected bool grantto = false;//导出

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("istoxls") == "1")
            {
                ToXls();
            }

            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_送团任务表_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.个人中心_送团任务表_栏目, true);
            }
            string act = string.Empty;
            act = EyouSoft.Common.Utils.GetQueryStringValue("act");
            if (!IsPostBack)
            {
                bind();
            }


        }

        /// <summary>
        /// to xls
        /// </summary>
        private void ToXls()
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.个人中心_送团任务表_栏目)) ResponseToXls(string.Empty);

            int _pageSize = Utils.GetInt(Utils.GetQueryStringValue("recordcount"));
            int _recordCount = 0;

            if (_pageSize < 1) ResponseToXls(string.Empty);

            LDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LDate")) == null ? "" : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LDate")).Value.ToString("yyyy-MM-dd");
            LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LEdate")) == null ? "" : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LEDate")).Value.ToString("yyyy-MM-dd");
            RouteName = Utils.GetQueryStringValue("RouteName").ToString();
            EyouSoft.Model.TourStructure.TourSentTaskSearch TourSentTaskSearch = new EyouSoft.Model.TourStructure.TourSentTaskSearch();
            TourSentTaskSearch.LDate = Utils.GetDateTimeNullable(LDate);
            TourSentTaskSearch.LEDate = Utils.GetDateTimeNullable(LEDate);
            TourSentTaskSearch.RouteName = RouteName;
            TourSentTaskSearch.CompanyId = CurrentUserCompanyID;
            //第几页
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //用户ID
            int[] UserID = new int[1];
            UserID[0] = SiteUserInfo.ID;
            //初始化列表
            EyouSoft.BLL.TourStructure.Tour tsBLL = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);

            var list = tsBLL.GetMySendTourInfo(_pageSize, 1, ref _recordCount, UserID, TourSentTaskSearch);

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", "出团日期", "集合时间", "去程航班/时间", "回程航班/时间", "  线路名称", "人数", "计调");
            foreach (EyouSoft.Model.TourStructure.TourSentTask cs in list)
            {
                cs.GatheringTime = Utils.GetDateTimeNullable(cs.GatheringTime) == null ? "" : cs.GatheringTime;
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n",
                  cs.LDate == null ? "" : Convert.ToDateTime(cs.LDate).ToString("yyyy-MM-dd"),
                  cs.GatheringTime,
                  cs.LTraffic,
                  cs.RTraffic,
                  cs.RouteName,
                  cs.PlanPeopleNumber,
                  cs.TourCoordinatorInfo[0].Name);
            }

            ResponseToXls(sb.ToString());
        }

        /// <summary>
        /// 打印送团单
        /// </summary>
        private void Print()
        {
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour tourBll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);

            //声明bll对象
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            //打印送团单 路劲
            printUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团单);


        }

        private void bind()
        {
            //查询条件
            LDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LDate")) == null ? "" : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LDate")).Value.ToString("yyyy-MM-dd");
            LEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LEdate")) == null ? "" : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LEDate")).Value.ToString("yyyy-MM-dd");
            RouteName = Utils.GetQueryStringValue("RouteName").ToString();
            EyouSoft.Model.TourStructure.TourSentTaskSearch TourSentTaskSearch = new EyouSoft.Model.TourStructure.TourSentTaskSearch();
            TourSentTaskSearch.LDate = Utils.GetDateTimeNullable(LDate);
            TourSentTaskSearch.LEDate = Utils.GetDateTimeNullable(LEDate);
            TourSentTaskSearch.RouteName = RouteName;
            TourSentTaskSearch.CompanyId = CurrentUserCompanyID;
            //第几页
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //用户ID
            int[] UserID = new int[1];
            UserID[0] = SiteUserInfo.ID;
            //初始化列表
            EyouSoft.BLL.TourStructure.Tour tsBLL = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            IList<EyouSoft.Model.TourStructure.TourSentTask> SendTaskslist = null;
            SendTaskslist = tsBLL.GetMySendTourInfo(pageSize, pageIndex, ref recordCount, UserID, TourSentTaskSearch);
            //绑定
            retList.DataSource = SendTaskslist;
            retList.DataBind();
            Repeater1.DataSource = SendTaskslist;
            Repeater1.DataBind();
            //判断记录条数
            len = retList == null ? 0 : SendTaskslist.Count;
            Print();
            //分页
            BindPage();

            RegisterScript(string.Format("var recordCount={0};", recordCount));
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        #region protected members
        /// <summary>
        /// 获取计调员姓名
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetJiDiaoName(object obj)
        {
            if (obj == null) return string.Empty;

            IList<EyouSoft.Model.TourStructure.TourCoordinatorInfo> items = (IList<EyouSoft.Model.TourStructure.TourCoordinatorInfo>)obj;

            if (items != null && items.Count > 0)
            {
                return items[0].Name;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取计调员手机
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetJiDiaoMobile(object obj)
        {
            if (obj == null) return string.Empty;

            IList<EyouSoft.Model.TourStructure.TourCoordinatorInfo> items = (IList<EyouSoft.Model.TourStructure.TourCoordinatorInfo>)obj;

            if (items != null && items.Count > 0)
            {
                return items[0].Mobile;
            }

            return string.Empty;
        }
        #endregion
    }
}
