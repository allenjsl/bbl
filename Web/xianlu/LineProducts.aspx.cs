using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EyouSoft.Common.Function;
using Common.Enum;
using EyouSoft.Common;

namespace Web.line
{
    /// <summary>
    /// 模块名称:线路产品列表(包括线路查询,修改线路,复制线路,删除线路信息,发布线路计划,我要报价等模块)
    /// 创建时间:2011-01-12 
    /// 创建人:lixh
    /// </summary>
    public partial class LineProducts : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers        
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CheckGrant(TravelPermission.线路产品库_线路产品库_栏目))
            {
                //新增权限
                if (!CheckGrant(TravelPermission.线路产品库_线路产品库_新增线路))
                {
                    this.AddLine.Visible = false;
                }
                //修改 复制权限
                if (!CheckGrant(TravelPermission.线路产品库_线路产品库_修改线路))
                {
                    this.updateLine.Visible = false;
                    this.CopyLine.Visible = false;
                }
                //删除权限
                if (!CheckGrant(TravelPermission.线路产品库_线路产品库_删除线路))
                {
                    this.penDelete.Visible = false;
                }
            }
            
            if (!this.Page.IsPostBack)
            {
                BindInitLineType();

                #region 定义变量接受参数
                //线路区域
                int? AreaID = Utils.GetIntNull(Utils.GetQueryStringValue("LineType"));
                //线路名称
                string LineName = Utils.GetQueryStringValue("LineName");
                //开始发布时间
                DateTime? StartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("StarTime"));
                //结束发布时间
                DateTime? EndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("EndTime"));
                //旅游天数
                int? DateNumber = Utils.GetIntNull(Utils.GetQueryStringValue("DateNumber"));
                //发布人
                string Author = Utils.GetQueryStringValue("Author");
                //分页
                PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                #endregion

                #region 初始化变量参数
                if (AreaID > 0)
                {
                    if (this.xl_ddlXianluType.Items.FindByValue(AreaID.ToString()) != null)
                        this.xl_ddlXianluType.Items.FindByValue(AreaID.ToString()).Selected = true;
                }              

                if (LineName != "")
                {
                    this.xl_XianlName.Value = LineName.ToString();
                }
                if (DateNumber.ToString() != "")
                {
                    this.xl_xianlDateNumber.Value = DateNumber.ToString();
                }
                if (StartTime != null)
                {
                    this.xl_XianlStarTime.Value = Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd");
                }
                if (EndTime != null)
                {
                    this.xl_XianlEndTime.Value = Convert.ToDateTime(EndTime).ToString("yyyy-MM-dd");
                }
                if (Author != "")
                {
                    this.xl_XianlAuthor.Value = Author;
                }
                #endregion
                BindInitLineProducts(AreaID, LineName, StartTime, EndTime, DateNumber, Author);
                
                #region 初始化所有的线路
                selectXianlu1.userId = SiteUserInfo.ID;
                selectXianlu1.curCompany = CurrentUserCompanyID;                
                #endregion

            }

            #region 删除线路信息
            string action = Utils.GetQueryStringValue("action");
            if (action == "delete")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["RouteId"].ToString()))
                {
                    int[] RouteId = Request.QueryString["RouteId"].Split(',').Select(i=>Utils.GetInt(i)).ToArray();
                    if (RouteId.Length > 0)
                        Dele(RouteId);        
                }
            }
            #endregion
        }

        #region 绑定线路区域
        protected void BindInitLineType()
        {
            //清空下拉框选项
            this.xl_ddlXianluType.Items.Clear();
            this.xl_ddlXianluType.Items.Add(new ListItem("--请选择线路区域--", ""));
            IList<EyouSoft.Model.CompanyStructure.Area> areaList = new EyouSoft.BLL.CompanyStructure.Area().GetAreaList(SiteUserInfo.ID);
            if (areaList != null && areaList.Count > 0)
            {
                //将数据添加至下拉框
                for (int i = 0; i < areaList.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = areaList[i].Id.ToString();
                    item.Text = areaList[i].AreaName;
                    this.xl_ddlXianluType.Items.Add(item);
                }
            }            
            //释放资源
            areaList = null;
        }
        #endregion

        #region 线路列表信息
        /// <summary>
        /// 绑定线路列表
        /// </summary>
        protected void BindInitLineProducts(int? AreaID,string LineName, DateTime? StartTime, DateTime? EndTime, int? DateNumber, string Author)
        {         
            EyouSoft.Model.RouteStructure.RouteSearchInfo ModelRouteSearch =new EyouSoft.Model.RouteStructure.RouteSearchInfo ();
            //线路区域编号
            ModelRouteSearch.AreaId = AreaID;
            //线路名称
            ModelRouteSearch.RouteName = LineName;
            //发布截止时间
            ModelRouteSearch.REDate = EndTime;
            //线路天数
            ModelRouteSearch.RouteDays = DateNumber;
            //发布起始时间
            ModelRouteSearch.RSDate = StartTime;
            //发布人姓名
            ModelRouteSearch.OperatorName = Author;
            
            //线路筛选
            if (Request.QueryString["xlid"] != "" && Request.QueryString["xlid"] != null)
            {
                ModelRouteSearch.AreaId = Utils.GetInt(Utils.GetQueryStringValue("xlid"));
            }            
            //线路库业务逻辑类
            EyouSoft.BLL.RouteStructure.Route RouteList = new EyouSoft.BLL.RouteStructure.Route(SiteUserInfo);
            IList<EyouSoft.Model.RouteStructure.RouteBaseInfo> RouteLists = RouteList.GetRoutes(SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, ModelRouteSearch);
            if (RouteLists != null && RouteLists.Count > 0)
            {
                this.LineProductList.DataSource = RouteLists;
                this.LineProductList.DataBind();
                BinPage();
            }
            else
            {
                this.LinePro_ExportPageInfo1.Visible = false;
            }           
            RouteList = null;
            RouteLists = null;
            ModelRouteSearch = null;
        }
        #endregion

        #region 设置分页
        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="LineType">线路类型</param>
        /// <param name="LineName">线路名称</param>
        /// <param name="StarTime">开始发布时间</param>
        /// <param name="EndTime">结束发布时间</param>
        /// <param name="DateNumber">出团天数</param>
        /// <param name="Author">发布人</param>
        protected void BinPage()
        {
            this.LinePro_ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.LinePro_ExportPageInfo1.UrlParams = Request.QueryString;
            this.LinePro_ExportPageInfo1.intPageSize = PageSize;
            this.LinePro_ExportPageInfo1.CurrencyPage = PageIndex;
            this.LinePro_ExportPageInfo1.intRecordCount = RecordCount;
        }

        #endregion

        #region 单项删除线路信息
        protected void Dele(int[] Routeid)
        {
            bool IsTrue = false;
            EyouSoft.BLL.RouteStructure.Route Route = new EyouSoft.BLL.RouteStructure.Route(SiteUserInfo);
            if (Routeid.Length >= 0)
            {
                Route.Delete(Routeid);
                IsTrue = true;
            }
            if (IsTrue)
            {
                Response.Clear();
                Response.Write("1");
                Response.End();
            }
            Route = null;
        }
        #endregion

        #region 打印行程单
        protected string ReturnUrl(string enumobj)
        { 
          EyouSoft.BLL.CompanyStructure.CompanySetting CompanyBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
          if (enumobj == "Normal")
          {
              return CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.线路标准发布行程单);
          }
          else
          {
              return CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.线路快速发布行程单);
          }
        }
        #endregion

        #region 根据线路区域编号取线路区域名称
        protected string GetAreaName(string AreaID)
        {            
            EyouSoft.BLL.CompanyStructure.Area Area = new EyouSoft.BLL.CompanyStructure.Area();
            EyouSoft.Model.CompanyStructure.Area AreaM = new EyouSoft.Model.CompanyStructure.Area();
            AreaM = Area.GetModel(Convert.ToInt32(AreaID));
            if (AreaM != null)
            {
                return AreaM.AreaName.ToString();
            }
            else {
                return "";
            }           
        }
       #endregion
    }
}
