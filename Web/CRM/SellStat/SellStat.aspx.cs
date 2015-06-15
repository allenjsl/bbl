using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using System.Text;
using EyouSoft.Model.StatisticStructure;

namespace Web.CRM.SellStat
{
    /// <summary>
    /// 销售统计
    /// </summary>
    /// 柴逸宁
    /// 2011-04-20

    /// <summary>
    /// 模块名称:客户关系管理 销售统计列表样式调整 列表添加序号一列
    /// 修改时间:2011-05-30
    /// 修改人：李晓欢
    /// </summary>

    public partial class SellStat : BackPage
    {

        #region 变量
        /// <summary>
        /// 当前页数
        /// </summary>
        protected int pageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        protected int recordCount;
        /// <summary>
        /// 每页显示条数
        /// </summary>
        protected int pageSize = 20;
        /// <summary>
        /// 导出权限
        /// </summary>
        protected bool IsImportOut;
        /// <summary>
        /// 查询实体
        /// </summary>
        private EyouSoft.Model.StatisticStructure.MQuerySalesStatistics searchInfo = null;
        /// <summary>
        /// 查询条件-组团社Id
        /// </summary>
        protected string hd_teamId = string.Empty;
        /// <summary>
        /// 查询条件-组团社Name
        /// </summary>
        protected string txt_teamName = string.Empty;

        protected StringBuilder sbSalesClerk = null;

        protected StringBuilder sbLogistics = null;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                IsImportOut = CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_销售统计_栏目);
                //权限判断
                if (!IsImportOut)
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_销售统计_栏目, true);
                }
                //线路区域
                string type = Utils.GetQueryStringValue("type");
                if (type == "toexcel")
                {
                    CreateExcel("area" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                }
                //获取操作员
                if (type == "teamSelect")
                {
                    GroupUser();
                }
                else
                {
                    BindArea();
                    //绑定列表
                    BindList();
                    //分页
                    BindPage();
                    //获取操作员
                    DdlUserListInit(Utils.GetInt(Utils.GetQueryStringValue("czyId")));
                }
            }


        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindList()
        {
            //BLL
            EyouSoft.BLL.StatisticStructure.BSalesStatistics bSaleBll;
            //List
            IList<EyouSoft.Model.StatisticStructure.MSalesStatistics> list;
            //查询条件
            SelectQModel(out bSaleBll, out list);
            //获取记录列表
            list = bSaleBll.GetSalesStatistics(pageSize, pageIndex, ref recordCount, searchInfo);
            int AllPeopleNum = 0;
            decimal AllFinanceSum = 0;
            bSaleBll.GetSumSalesStatistics(ref AllPeopleNum, ref AllFinanceSum, searchInfo);
            //判断记录条数
            if (list != null && list.Count > 0)
            {
                rptCustomer.DataSource = list;
                rptCustomer.DataBind();
                lblPeopleNum.Text = AllPeopleNum.ToString();
                lblTotalAmount.Text = Utils.FilterEndOfTheZeroString(AllFinanceSum.ToString());
                this.ExportPageInfo1.intRecordCount = list.Count;
                this.lblMsg.Visible = false;
                this.ExportPageInfo1.Visible = true;
                
            }
            else
            {
                this.ExportPageInfo1.Visible = false;
                this.lblMsg.Visible = true;
            }


        }
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="bSaleBll"></param>
        /// <param name="list"></param>
        private void SelectQModel(out EyouSoft.BLL.StatisticStructure.BSalesStatistics bSaleBll, out IList<EyouSoft.Model.StatisticStructure.MSalesStatistics> list)
        {
            //初始化条件
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //实例化销售统计BLL
            bSaleBll = new EyouSoft.BLL.StatisticStructure.BSalesStatistics(SiteUserInfo);
            //实例化销售统计Model
            searchInfo = new EyouSoft.Model.StatisticStructure.MQuerySalesStatistics();
            //实例化销售统计List
            list = null;
            //查询实体类赋值
            searchInfo.CompanyId = CurrentUserCompanyID;
            //排序方式
            searchInfo.OrderIndex = 1;
            //开始时间
            searchInfo.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("todate"));
            txtToLeaveDate.Text = Utils.GetQueryStringValue("todate");
            //结束时间
            searchInfo.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endate"));
            txtEndLeaveDate.Text = Utils.GetQueryStringValue("endate");
            //线路区域
            int[] areaIds = Utils.GetIntArray(Utils.GetQueryStringValue("ddlLineType"), ",");
            searchInfo.AreaIds = areaIds;

            searchInfo.CityIds = Utils.GetIntArray(Utils.GetQueryStringValue("cityId"), ",");
            searchInfo.ProvinceIds = Utils.GetIntArray(Utils.GetQueryStringValue("province"), ",");
            
            //组团社Id
            searchInfo.CustomerId = Utils.GetInt(Utils.GetQueryStringValue("hd_teamId"));
            //searchInfo.CustomerName = Utils.GetQueryStringValue("hd_teamName");
            hd_teamId = Utils.GetQueryStringValue("hd_teamId");
            txt_teamName = Utils.GetQueryStringValue("hd_teamName");
            //计调员
            int[] jdy = Utils.GetIntArray(Utils.GetQueryStringValue("jdyId"), ",");
            searchInfo.LogisticIds = jdy;
            //销售员
            int[] xsy = Utils.GetIntArray(Utils.GetQueryStringValue("xsyId"), ",");
            searchInfo.SalesClerkIds = xsy;
            //操作员
            int[] czy = Utils.GetIntArray(Utils.GetQueryStringValue("czyId"), ",");
            searchInfo.OperatorId = czy;
            selectOperator1.OperId = Utils.GetQueryStringValue("jdyId");
            selectOperator1.OperName = Utils.GetQueryStringValue("jdyName");
            selectOperator2.OperId = Utils.GetQueryStringValue("xsyId");
            selectOperator2.OperName = Utils.GetQueryStringValue("xsyName");
            EyouSoft.BLL.CompanyStructure.CompanySetting csBLL = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? cot = csBLL.GetComputeOrderType(SiteUserInfo.CompanyID);
            searchInfo.ComputeOrderType = cot.HasValue ? cot.Value : EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单;

            searchInfo.ShangDanSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("sdsdate"));
            searchInfo.ShangDanEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("sdedate"));

        }
        #region 将List数据转换成string类型
        /// <summary>
        /// 将List数据转换成string类型
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected static string GetListConverToStr(IList<EyouSoft.Model.CompanyStructure.City> list)
        {
            string listToStr = "";
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ToString() != "")
                    {
                        listToStr += list[i].Id + ",";
                    }
                }
            }
            return listToStr.TrimEnd(',');
        }
        protected static string GetListConverToStrSalesClerk(IList<EyouSoft.Model.StatisticStructure.StatisticOperator> list)
        {
            string listToStr = "";
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ToString() != "")
                    {
                        listToStr += list[i].OperatorName + ",";
                    }
                }
            }
            return listToStr.TrimEnd(',');
        }
        #endregion


        /// <summary>
        /// 绑定区域
        /// </summary>
        void BindArea()
        {

            EyouSoft.BLL.CompanyStructure.Area area = new EyouSoft.BLL.CompanyStructure.Area(SiteUserInfo);
            int userid = SiteUserInfo.ID;
            IList<EyouSoft.Model.CompanyStructure.Area> list = new List<EyouSoft.Model.CompanyStructure.Area>();
            list = area.GetAreaList(userid);
            ddl_LineType.Items.Add(new ListItem("请选择线路区域", "-1"));
            ddl_LineType.DataSource = list;
            ddl_LineType.DataBind();
            //锁定
            ddl_LineType.Items.FindByValue(Utils.GetInt(Utils.GetQueryStringValue("ddlLineType"), -1).ToString()).Selected = true;
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams.Add(Request.QueryString);
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            //BLL
            EyouSoft.BLL.StatisticStructure.BSalesStatistics bSaleBll;
            //List
            IList<EyouSoft.Model.StatisticStructure.MSalesStatistics> list;
            //查询条件
            SelectQModel(out bSaleBll, out list);
            list = bSaleBll.GetSalesStatistics(1, 1, ref recordCount, searchInfo);
            if (recordCount != 0)
            {
                list = bSaleBll.GetSalesStatistics(recordCount, 1, ref recordCount, searchInfo);
            }
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\n", "线路区域", "线路名称", "旅行社名称", "所属地区", "所属部门", "人数", "金额", "销售员", "计调员", "对方操作人员", "发团日期");
            foreach (EyouSoft.Model.StatisticStructure.MSalesStatistics cs in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\n",
                    cs.AreaName,
                    cs.RouteName,
                    cs.CustomerName,
                    cs.ProvinceAndCity,
                    cs.DepartName,
                    cs.PeopleNum,
                    cs.TotalAmount.ToString("￥0.00"),
                    cs.SalerName,
                    cs.Logistics == null ? "" : cs.Logistics[0].OperatorName,
                    cs.OperatorName,
                    cs.LeaveDate.ToString("yyyy-MM-dd"));
            }
            Response.Write(sb.ToString());
            Response.End();
        }


        /// <summary>
        /// 根据组团ID 获得操作员
        /// </summary>
        protected void DdlUserListInit(int selectIndex)
        {
            this.czy.Items.Clear();
            this.czy.Items.Add(new ListItem("--请选择--", "0"));
            int zId = Utils.GetInt(Utils.GetQueryStringValue("zId"));
            //if (zId != 0)
            //{
            //    EyouSoft.BLL.CompanyStructure.CompanyUser csBLL = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            //    EyouSoft.Model.CompanyStructure.QueryCompanyUser QueryModel = new EyouSoft.Model.CompanyStructure.QueryCompanyUser();
            //    IList<EyouSoft.Model.CompanyStructure.CompanyUser> csList = null;
            //    QueryModel.ZuTuanCompanyId = zId;
            //    QueryModel.CompanyId = SiteUserInfo.CompanyID;
            //    csList = csBLL.GetCompanyUsers(QueryModel);
            //    if (csList != null)
            //    {
            //        for (int i = 0; i < csList.Count; i++)
            //        {
            //            ListItem item = new ListItem();
            //            item.Value = csList[i].ID.ToString();
            //            item.Text = csList[i].PersonInfo.ContactName;
            //            if (csList[i].ID == selectIndex)
            //            {
            //                item.Selected = true;
            //            }
            //            this.czy.Items.Add(item);
            //        }
            //    }
            //}

            if (zId <= 0) return;
            var items = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerContactList(zId);

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item == null || string.IsNullOrEmpty(item.Name)) continue;
                    czy.Items.Add(new ListItem(item.Name, item.ID.ToString()));
                }

                czy.SelectedValue = selectIndex.ToString();
            }

        }


        /// <summary>
        /// 绑定操作员
        /// </summary>
        private void GroupUser()
        {
            int zId = Utils.GetInt(Utils.GetQueryStringValue("zId"));
            //string cityHtml = string.Empty;

            //EyouSoft.BLL.CompanyStructure.CompanyUser csBLL = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            //EyouSoft.Model.CompanyStructure.QueryCompanyUser QueryModel = new EyouSoft.Model.CompanyStructure.QueryCompanyUser();
            //IList<EyouSoft.Model.CompanyStructure.CompanyUser> csList = null;
            //QueryModel.ZuTuanCompanyId = zId;
            //QueryModel.CompanyId = SiteUserInfo.CompanyID;
            //csList = csBLL.GetCompanyUsers(QueryModel);
            //StringBuilder strBuilder = new StringBuilder();
            //strBuilder.AppendFormat("<option value=\"0\"selected=\"selected\">--请选择--</option>");
            //if (csList != null)
            //{
            //    for (int i = 0; i < csList.Count; i++)
            //    {
            //        strBuilder.AppendFormat("<option value=\"" + csList[i].ID + "\">" + csList[i].PersonInfo.ContactName + "</option>");
            //    }
            //}

            //cityHtml = strBuilder.ToString();

            StringBuilder s = new StringBuilder();
            s.Append("<option value=\"0\"selected=\"selected\">--请选择--</option>");
            var items = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerContactList(zId);

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item == null || string.IsNullOrEmpty(item.Name)) continue;
                    s.AppendFormat("<option value=\"{0}\">{1}</option>", item.ID, item.Name);
                }
            }

            //异步执行代码返回
            Response.Clear();
            Response.Write(s.ToString());
            Response.End();
        }

    }
}
