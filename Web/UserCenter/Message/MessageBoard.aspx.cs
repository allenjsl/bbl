using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Common.Enum;

namespace Web.UserCenter.Message
{
    /// <summary>
    /// 留言板列表页
    /// 功能：查看留言信息
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-22 
    /// </summary>
    public partial class MessageBoard : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            //判断栏目权限
            if (!CheckGrant(TravelPermission.个人中心_留言板_栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.个人中心_留言板_栏目, false);
                return;
            }
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
            IList<EyouSoft.Model.CompanyStructure.CustomerMessageModel> list = bll.GetMessageList(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, false, searchModel);

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
                this.ExportPageInfo1.Visible = false;
                this.lblMsg.Visible = true;
            }

            list = null;

        }

        #region 设置分页
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.Model.CompanyStructure.CustomerMessageModel cmm= e.Item.DataItem as EyouSoft.Model.CompanyStructure.CustomerMessageModel;
            EyouSoft.Model.CompanyStructure.CustomerInfo cci = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(cmm.MessageCompanyId);
            Literal lt_groupName = e.Item.FindControl("lt_groupName") as Literal;
            Literal lt_Contract = e.Item.FindControl("lt_Contract") as
                Literal;
            if (cci != null)
            {
                lt_groupName.Text = cci.Name;
                lt_Contract.Text = cci.Phone;
            }
        }
    }
}
