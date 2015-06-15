using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.GroupEnd.Messages
{
    public partial class MessageBoard : FrontPage
    {

        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string msgTitle = Utils.GetQueryStringValue("title");
                this.txtTitle.Text = msgTitle;
                //当前页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                DataInit(msgTitle);
            }
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        private void DataInit(string msgTitle)
        {
            EyouSoft.BLL.CompanyStructure.CustomerMessageReply bll = new EyouSoft.BLL.CompanyStructure.CustomerMessageReply();

            //查询model
            EyouSoft.Model.CompanyStructure.CustomerMessageModel searchModel = new EyouSoft.Model.CompanyStructure.CustomerMessageModel();
            searchModel.MessageTitle = msgTitle;
            searchModel.CompanyId = SiteUserInfo.CompanyID;

            //获得分页数据
            IList<EyouSoft.Model.CompanyStructure.CustomerMessageModel> list = bll.GetMessageList(SiteUserInfo.TourCompany.TourCompanyId, pageSize, pageIndex, ref recordCount,true, searchModel);

            //判断list是否有数据
            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                BindPage();
                this.lblMsg.Visible = false;
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
                this.lblMsg.Visible = true;
            }

            list = null;

        }

        #region 设置分页
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
