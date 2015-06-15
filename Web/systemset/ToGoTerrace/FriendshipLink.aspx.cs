using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.systemset.ToGoTerrace
{
    /// <summary>
    /// 友情链接-列表页面
    /// </summary>
    /// 柴逸宁
    /// 2011-4-8
    public partial class FriendshipLink : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        /// <summary>
        /// 每页显示数
        /// </summary>
        private int pageSize = 20;
        /// <summary>
        /// 当前页码
        /// </summary>
        private int pageIndex = 1;
        /// <summary>
        /// 记录条数
        /// </summary>
        private int recordCount;
        /// <summary>
        /// 友情链接LIST
        /// </summary>
        protected IList<EyouSoft.Model.SiteStructure.SiteFriendLink> list = null;
        /// <summary>
        /// 记录条数
        /// </summary>
        protected int len = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_同行平台栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_同行平台栏目, false);
                return;
            }

            if (!IsPostBack)
            {
                //判断删除操作
                if (Utils.GetQueryStringValue("act") == "areadel")
                {
                    AreaDel();
                }
                //绑定
                Bind();
            }
        }

        private void Bind()
        {
            //友情链接BLL
            EyouSoft.BLL.SiteStructure.SiteFriendLink ssBLL = new EyouSoft.BLL.SiteStructure.SiteFriendLink();
            //友情链接Model
            EyouSoft.Model.SiteStructure.SiteFriendLink ssModel = new EyouSoft.Model.SiteStructure.SiteFriendLink();
            //获取友情链接列表
            list = ssBLL.GetSiteFriendLink(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount);
            //判断列表条数
            if (list != null && list.Count > 0)
            {
                rptList.DataSource = list;
                rptList.DataBind();
                this.lblMsg.Visible = false;
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
                this.lblMsg.Visible = true;
            }
            BindPage();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        private void AreaDel()
        {
            //友情链接BLL
            EyouSoft.BLL.SiteStructure.SiteFriendLink ssBLL = new EyouSoft.BLL.SiteStructure.SiteFriendLink();
            //删除记录ID数组
            string[] stid = Utils.GetFormValue("tid").Split(',');
            int[] tid = new int[stid.Length];
            //循环添加删除ID
            for (int i = 0; i < stid.Length; i++)
            {
                tid[i] = Utils.GetInt(stid[i]);
            }
            //默认删除失败
            bool res = false;
            //删除记录
            res = ssBLL.DelFriendLink(tid[0], SiteUserInfo.CompanyID);
            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
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
    }
}
