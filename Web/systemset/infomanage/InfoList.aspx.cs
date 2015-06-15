using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.infomanage
{  
    /// <summary>
    /// 信息管理列表
    /// xuty 2011/1/15
    /// </summary>
    public partial class InfoList : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int itemIndex;
        protected void Page_Load(object sender, EventArgs e)
        {  
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_信息管理_信息管理栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_信息管理_信息管理栏目, true);
                return;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex = (pageIndex - 1) * pageSize + 1;
            string method = Utils.GetQueryStringValue("method");
            string ids = Utils.GetQueryStringValue("ids");//获取员工Id
            EyouSoft.BLL.CompanyStructure.News newsBll = new EyouSoft.BLL.CompanyStructure.News();//初始化newsBll
            //删除操作
            if (method == "del")
            {
                ids = ids.TrimEnd(',');
                bool result=newsBll.Delete(ids.Split(',').Select(i=>Utils.GetInt(i)).ToArray());
                MessageBox.ShowAndRedirect(this, "删除成功！","/systemset/infomanage/InfoList.aspx");
                return;
            }
            //绑定信息列表
            IList<EyouSoft.Model.CompanyStructure.News> list = newsBll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID);
            if (list != null && list.Count > 0)
            {
                rptInfo.DataSource = list;
                rptInfo.DataBind();
                BindExportPage();
            }
            else
            {
                rptInfo.EmptyText = "<tr><td colspan='7' align='center'>对不起，暂无信息！</td></tr>";
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
