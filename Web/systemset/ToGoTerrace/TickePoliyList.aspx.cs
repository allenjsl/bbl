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
    /// 同行平台-机票政策列表页面
    /// </summary>
    /// 柴逸宁
    /// 2011-4-7
    public partial class TickePoliyList : Eyousoft.Common.Page.BackPage
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
        /// 机票政策列表
        /// </summary>
        protected IList<EyouSoft.Model.SiteStructure.TicketPolicy> list = null;
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
                //获取操作
                if (Utils.GetQueryStringValue("act") == "areadel")//删除操作
                {
                    AreaDel();
                }
                bind();//初始化列表
            }


        }
        /// <summary>
        /// 初始化绑定列表
        /// </summary>
        private void bind()
        {
            //机票政策BLL
            EyouSoft.BLL.SiteStructure.TicketPolicy ssBLL = new EyouSoft.BLL.SiteStructure.TicketPolicy();
            //分页页数
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //获取机票政策列表
            list = ssBLL.GetTicketPolicy(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount);
            //绑定数据
            rptList.DataSource = list;
            rptList.DataBind();
            //记录条数
            len = list.Count;
            //设置分页
            BindPage();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        private void AreaDel()
        {
            //初始化BLL
            EyouSoft.BLL.SiteStructure.TicketPolicy ssBLL = new EyouSoft.BLL.SiteStructure.TicketPolicy();
            //获取删除id，数组
            string[] stid = Utils.GetFormValue("tid").Split(',');
            //定义数组
            int[] tid = new int[stid.Length];
            for (int i = 0; i < stid.Length; i++)
            {
                //循环添加删除id
                tid[i] = Utils.GetInt(stid[i]);
            }
            bool res = false;
            res = ssBLL.DelTicketPolicy(tid[0], SiteUserInfo.CompanyID);
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
