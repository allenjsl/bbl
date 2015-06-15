using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Collections.Generic;
using EyouSoft.Common;

namespace Web.UserCenter.UserInfo
{
    /// <summary>
    /// 页面功能：个人中心--工作交流-交流中心
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class Exchange : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int len = 0;
        EyouSoft.BLL.PersonalCenterStructure.WorkExchange exBll = null;//工作交流BLL
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            exBll = new EyouSoft.BLL.PersonalCenterStructure.WorkExchange();
            if (!IsPostBack)
            {
                string type = Utils.GetQueryStringValue("type");
                if (type == "delchange")
                {
                    DelChange();//删除工作交流
                }
                else
                {
                    DataInit();//初使数据绑定
                }
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        private void DelChange()
        {
            string[] tid = Utils.GetFormValue("tid").Split(',');//交流ID主键集合
            bool result = false;
            result = exBll.Delete(tid);
            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", result ? 1 : -1));
            Response.End();
            
        }

        /// <summary>
        /// 初使化
        /// </summary>
        private void DataInit()
        {
            pageIndex = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);//当前页码

            IList<EyouSoft.Model.PersonalCenterStructure.WorkExchange> list = null;
            list = exBll.GetList(pageSize, pageIndex, ref recordCount, this.CurrentUserCompanyID, this.SiteUserInfo.ID);
            
            len = list.Count;
            this.rptList.DataSource = list;
            this.rptList.DataBind();
            //设置分页
            BindPage();
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.Path + "?";
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion
    }
}
