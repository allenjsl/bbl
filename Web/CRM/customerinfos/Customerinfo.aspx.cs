using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.CRM.customerinfos
{
    /// <summary>
    /// 客户关系管理 客户资料 联系人信息
    /// 2011-04-18
    /// 李晓欢
    /// </summary>
    public partial class Customerinfo : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                PageIndex = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("Page"), 1);
                EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();//客户资料bll
                EyouSoft.Model.CompanyStructure.CustomerInfo custModel = null;//客户资料实体
                int custId = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("custId"));
                if (custId > 0)
                {
                    custModel = new EyouSoft.Model.CompanyStructure.CustomerInfo();
                    custModel = custBll.GetCustomerModel(custId);
                    //获取联系人集合
                    IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> custList = custModel.CustomerContactList;
                    if (custList != null && custList.Count > 0)
                    {
                        this.CloseNumberList.DataSource = custList;
                        this.CloseNumberList.DataBind();
                        BIndPage();
                    }
                    else
                    {
                        this.Close_ExportPageInfo1.Visible = false;
                    }
                }
            }
        }

        #region 分页控件
        protected void BIndPage()
        {
            this.Close_ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.Close_ExportPageInfo1.UrlParams = Request.QueryString;
            this.Close_ExportPageInfo1.intPageSize = PageSize;
            this.Close_ExportPageInfo1.CurrencyPage = PageIndex;
            this.Close_ExportPageInfo1.intRecordCount = RecordCount;
        }
        #endregion

        #region 设置弹窗
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion
    }
}
 