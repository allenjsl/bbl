using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
namespace Web.systemset.basicinfo
{   
    /// <summary>
    /// 系统设置-线路区域
    /// xuty 2011/1/13
    /// </summary>
    public partial class RouteArea : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int itemIndex;//编号
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_基础设置_线路区域栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_基础设置_线路区域栏目, true);
                return;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex = (pageIndex - 1) * pageSize + 1;
            int areaId = Utils.GetInt(Utils.GetQueryStringValue("areaId"));
            EyouSoft.BLL.CompanyStructure.Area areaBll = new EyouSoft.BLL.CompanyStructure.Area();//初始化areabll
            if (areaId != 0)//删除区域
            {
                if (Utils.GetQueryStringValue("method") == "ispublish")//验证该区域是否有线路存在
                {
                   Utils.ResponseMeg(areaBll.IsAreaPublish(areaId, CurrentUserCompanyID),"");
                   return;
                }
                bool result = areaBll.Delete(areaId);
                MessageBox.ShowAndRedirect(this, result ? "删除成功" : "删除失败", "/systemset/basicinfo/RouteArea.aspx");
                return;
            }
            //绑定线路区域
            IList<EyouSoft.Model.CompanyStructure.Area> areaList = areaBll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID);
            if (areaList != null && areaList.Count > 0)
            {
                rptRouteArea.DataSource = areaList;
                rptRouteArea.DataBind();
                BindExportPage();
            }
            else
            {
                rptRouteArea.EmptyText = "<tr><td colspan='6' align='center'>对不起，暂无线路区域信息！</td></tr>";
                this.ExportPageInfo1.Visible = false;
            }
        }
        /// <summary>
        /// 获取区域下的计调人
        /// </summary>
        /// <param name="usersObj"></param>
        /// <returns></returns>
        protected string GetUsers(object usersObj)
        {
            if (usersObj != null)
            {   
                StringBuilder strBuilder=new StringBuilder();
                IList<EyouSoft.Model.CompanyStructure.UserArea> userList = usersObj as IList<EyouSoft.Model.CompanyStructure.UserArea>;
                foreach(EyouSoft.Model.CompanyStructure.UserArea user in userList)
                {
                    strBuilder.AppendFormat("{0},", user.ContactName);
                }
                return strBuilder.ToString().TrimEnd(',');
            }
            return "";
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
