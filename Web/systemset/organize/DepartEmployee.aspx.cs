using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.organize
{   
    /// <summary>
    /// 部门员工管理
    /// xuty 2011/1/14
    /// </summary>
    public partial class DepartEmployee : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_组织机构_部门人员栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_组织机构_部门人员栏目, true);
                return;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            string method = Utils.GetQueryStringValue("method");
            string ids = Utils.GetQueryStringValue("ids");//获取员工
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);//初始化bll
            bool result = false;

            #region 当前操作
            if (method=="del")
            {
                ids = ids.TrimEnd(',');
                result = userBll.Remove(CurrentUserCompanyID, ids.Split(','));
                MessageBox.Show(this, result?"删除成功！":"删除失败！");
            }
            if (method == "setState")
            {
                result = userBll.SetEnable(Utils.GetInt(ids), Utils.GetQueryStringValue("hidMethod") == "start");
                Utils.ResponseMeg(result, result?"设置完成！":"设置失败！");
                return;
            }
            #endregion
            //绑定部门人员
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> list = userBll.GetList(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount);
            if (list != null && list.Count > 0)
            {
                rptEmployee.DataSource = list;
                rptEmployee.DataBind();
                BindExportPage();
            }
            else
            {
                rptEmployee.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无部门员工信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
           
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion
    }
   
}
