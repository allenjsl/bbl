using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
namespace Web.CRM.customerinfos
{   
    /// <summary>
    /// 设置线路区域
    /// xuty 2011/1/24
    /// </summary>
    public partial class SelRouteArea : Eyousoft.Common.Page.BackPage
    {
        protected int itemIndex = 1;
        protected int recordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_分配帐号))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_客户资料_分配帐号, true);
                return;
            }
            EyouSoft.BLL.CompanyStructure.Area areaBll = new EyouSoft.BLL.CompanyStructure.Area();//区域bll
            //绑定线路区域
            rptArea.DataSource = areaBll.GetAreaByCompanyId(CurrentUserCompanyID);
            rptArea.DataBind();
        }
        /// <summary>
        /// 绑定列表时获取换行操作
        /// </summary>
        /// <returns></returns>
        protected string GetListTr()
        {
            string strHtml = "";
            if (itemIndex == 1)
                strHtml = "<tr class='oddTr'>";
            else if (itemIndex % 10 == 1)
                strHtml = "</tr><tr class='oddTr'>";
            else if (itemIndex % 5 == 1)
                strHtml = "</tr><tr class='evenTr'>";
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
            if (recordCount % 5 != 0)
                return string.Format("<td colspan='{0}'></td></tr>", 5 - recordCount % 5);
            return "</tr>";
        }
        /// <summary>
        /// 获取当前信息选中过的那些部门
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        protected string GetAreaChecked(int departId)
        {
            if (departId == 0)
            {
                return "checked=\"checked\"";
            }
            return "";
        }
        
      
    }
}

