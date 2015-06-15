using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.systemlog
{   
    /// <summary>
    /// 系统日志
    /// xuty 2011/1/17
    /// </summary>
    public partial class LogList : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int itemIndex;//序号
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_系统日志_系统日志栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_系统日志_系统日志栏目, true);
                return;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex = (pageIndex - 1) * pageSize + 1;
            EyouSoft.BLL.CompanyStructure.SysHandleLogs logBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();//日志bll
            //获取查询条件
            string operatorId = Utils.GetQueryStringValue("operator");
            string startDate = Utils.GetQueryStringValue("startDate");
            string endDate = Utils.GetQueryStringValue("endDate");
            string departId = Utils.GetQueryStringValue("departId");
            EyouSoft.BLL.CompanyStructure.Department departBll = new EyouSoft.BLL.CompanyStructure.Department();//部门bll
            //绑定部门列表
            IList<EyouSoft.Model.CompanyStructure.Department> departList = departBll.GetAllDept(CurrentUserCompanyID);
            if (departList != null && departList.Count > 0)
            {
                selDeaprt.DataTextField = "DepartName";
                selDeaprt.DataValueField = "Id";
                selDeaprt.DataSource = departList;
                selDeaprt.DataBind();
            }
            selDeaprt.Items.Insert(0, new ListItem("请选择", "0"));
            //绑定操作员列表
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);//初始化bll
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> userlist = userBll.GetCompanyUser(CurrentUserCompanyID);
            if (userlist != null && userlist.Count > 0)
            {
                foreach (EyouSoft.Model.CompanyStructure.CompanyUser user in userlist)
                {
                    selOperator.Items.Add(new ListItem(user.PersonInfo.ContactName, user.ID.ToString()));
                }
            }
            selOperator.Items.Insert(0, new ListItem("请选择", "0"));
            //日志查询试实体
            EyouSoft.Model.CompanyStructure.QueryHandleLog queryLog=new EyouSoft.Model.CompanyStructure.QueryHandleLog();
            queryLog.CompanyId=CurrentUserCompanyID;//公司编号
            queryLog.DepartId=Utils.GetInt(departId);//部门编号
            queryLog.HandEndTime=Utils.GetDateTimeNullable(endDate);//操作结束时间
            queryLog.HandStartTime=Utils.GetDateTimeNullable(startDate);//操作开始时间
            queryLog.OperatorId = Utils.GetInt(operatorId);//操作员
            //绑定日志列表
            IList<EyouSoft.Model.CompanyStructure.SysHandleLogs> list = logBll.GetList(pageSize, pageIndex, ref recordCount, queryLog);
            if (list != null && list.Count > 0)
            {
                rptLog.DataSource = list;
                rptLog.DataBind();
                BindExportPage();
            }
            else
            {
                rptLog.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无日志信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
            //恢复查询关键字
            txtEndDate.Value = endDate;//操作结束时间
            selOperator.Value = operatorId;//操作员
            txtStartDate.Value = startDate;//操作开始时间
            selDeaprt.Value = departId;//部门编号
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams.Add(Request.QueryString);
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion
      }
   }
