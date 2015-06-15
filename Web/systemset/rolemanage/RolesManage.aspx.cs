using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using Web.UserControl.ucSystemSet;
namespace Web.systemset.rolemanage
{   
    /// <summary>
    /// 角色管理
    /// xuty 2011/01/15
    /// </summary>
    public partial class RolesManage : Eyousoft.Common.Page.BackPage
    {
        protected int itemIndex = 1;
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 40;
        protected int itemIndex2 = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_角色管理_角色管理栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_角色管理_角色管理栏目, true);
                return;
            }
            string roleIds = Utils.GetQueryStringValue("roleIds");//报价标准Id
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex2 = (pageIndex - 1) * pageSize + 1;
            EyouSoft.BLL.CompanyStructure.SysRoleManage roleBll = new EyouSoft.BLL.CompanyStructure.SysRoleManage();//初始化角色bll
            //报价Id不为空执行删除操作
            if (roleIds != "")
            {  
                int[] roleIdArr=roleIds.TrimEnd(',').Split(',').Select(i=>Utils.GetInt(i)).ToArray();
                bool result = roleBll.Delete(CurrentUserCompanyID, roleIdArr);
                MessageBox.ShowAndRedirect(this,result?"删除成功！":"删除失败！","/systemset/rolemanage/RolesManage.aspx");
            }
            //绑定角色列表
            IList<EyouSoft.Model.CompanyStructure.SysRoleManage> list = roleBll.GetList(pageSize,pageIndex,ref recordCount,CurrentUserCompanyID);
            if (list != null && list.Count > 0)
            {
           
                rptRoles.DataSource = list;
                rptRoles.DataBind();
                BindExportPage();
            }
            else
            {
                rptRoles.EmptyText = "<tr><td colspan='6' align='center'>对不起，暂无角色信息！</td></tr>";
                this.ExportPageInfo1.Visible = false;
               
            }
        }
        /// <summary>
        /// 绑定列表时获取换行操作
        /// </summary>
        /// <returns></returns>
        protected string GetListTr()
        {
            string strHtml = "";
            if (itemIndex == 1)
                strHtml= "<tr class='oddTr'>";
            else if (itemIndex % 4 == 1)
                strHtml= "</tr><tr class='oddTr'>";
             else if (itemIndex % 2 == 1)
                strHtml= "</tr><tr class='evenTr'>";
            itemIndex++;
            return strHtml; 
        }

        /// <summary>
        /// 获取绑定列表的最后一项
        /// </summary>
        /// <returns></returns>
        protected string GetLastTr()
        {
            if (recordCount == 0)
                return "";
            if (itemIndex % 1 == 0)
                return "<td colspan='4'>&nbsp;</td></tr>";
            return "</tr>";
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
