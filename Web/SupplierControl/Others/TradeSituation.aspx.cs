using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Collections;

namespace Web.SupplierControl.Others
{
    public partial class TradeSituation : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 供应商管理-其他
        /// 作者：柴逸宁
        /// </summary>
        /// 时间：2011-03-09
        #region 分页变量
        protected int pageSize = 4;
        protected int pageIndex = 1;
        protected int recordCount;
        protected int len = 0;
        #endregion
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_栏目)) 
        //    {
        //        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_其它_栏目, false);

        //    }
        //    if (!IsPostBack)
        //    {
        //        int tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
        //        dind(tid);
        //    }
        //}

        //private void dind(int tid)
        //{
        //   //初始化条件
        //    pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
        //    IList<EyouSoft.Model.TourStructure.LBGYSTours> list = null;//地接的list
        //    //EyouSoft.BLL.TourStructure.Tour tBll = new EyouSoft.BLL.TourStructure.Tour();
        //    //list = tBll.GetToursGYS(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, tid);
        //    //len = list == null ? 0 : list.Count;
        //    this.repList.DataSource = list;
        //    this.repList.DataBind();
        //    //设置分页
        //    BindPage();

        //}

        //#region 绑定分页控件
        ///// <summary>
        ///// 绑定分页控件
        ///// </summary>
        //protected void BindPage()
        //{
        //    this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
        //    this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
        //    this.ExporPageInfoSelect1.intPageSize = pageSize;
        //    this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
        //    this.ExporPageInfoSelect1.intRecordCount = recordCount;
        //}
        //#endregion

    }
}
