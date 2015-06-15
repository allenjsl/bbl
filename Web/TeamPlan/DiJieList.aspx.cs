using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.TeamPlan
{
    /// <summary>
    /// 团队计划地接社信息
    /// 功能：显示地接社列表
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-17 
    /// </summary>
    public partial class DiJieList : BackPage
    {
        #region 分页变量
        protected int pageSize = 15;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //要赋值的文本框ID
                string txtName = Utils.GetQueryStringValue("text");
                //要赋值的隐藏域ID
                string txtValue = Utils.GetQueryStringValue("value");
                //查询条件：地接社名
                string djName = Server.UrlDecode(Utils.GetQueryStringValue("djName"));
                //页面关闭时回调JS方法
                string callBackFun = Utils.GetQueryStringValue("callBackFun");
                {
                    if (callBackFun != "")
                    {
                        this.hideCallBackFun.Value = callBackFun;
                    }
                }
                //分页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

                if (txtName != "")
                {
                    this.hideText.Value = txtName;
                }
                if (txtValue != "")
                {
                    this.hideValue.Value = txtValue;
                }
                this.txtDjName.Text = djName;
                //初始化地接社
                DataInit(djName);
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DataInit(string djName)
        {
            EyouSoft.Model.EnumType.CompanyStructure.SupplierType sType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接;

            if (Utils.GetInt(Utils.GetQueryStringValue("sType"), 0) == 5)
            {
                sType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务;
                lbSupplerTypeName.InnerText = "票务名称：";
            }
            if (Utils.GetInt(Utils.GetQueryStringValue("sType"), 0) == 4)
            {
                sType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.保险;
                lbSupplerTypeName.InnerText = "保险名称：";
            }
            if (Utils.GetInt(Utils.GetQueryStringValue("sType"), 0) == 7)
            {
                sType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.购物;
                lbSupplerTypeName.InnerText = "购物名称：";
            }
            if (Utils.GetInt(Utils.GetQueryStringValue("sType"), 0) == 3)
            {
                sType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.景点;
                lbSupplerTypeName.InnerText = "景点名称：";
            }
            if (Utils.GetInt(Utils.GetQueryStringValue("sType"), 0) == 1)
            {
                sType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.酒店;
                lbSupplerTypeName.InnerText = "酒店名称：";
            }
            if (Utils.GetInt(Utils.GetQueryStringValue("sType"), 0) == 9)
            {
                sType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.其他;
                lbSupplerTypeName.InnerText = "其它名称：";
            }
            
            if (Utils.GetInt(Utils.GetQueryStringValue("sType"), 0) == 8)
            {
                sType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.车队;
                lbSupplerTypeName.InnerText = "车队名称：";
            }
            if (Utils.GetInt(Utils.GetQueryStringValue("sType"), 0) == 2)
            {
                sType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.餐馆;
                lbSupplerTypeName.InnerText = "餐馆名称：";
            }

            //声明操作对象
            EyouSoft.BLL.CompanyStructure.CompanySupplier bll = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            //查询地接社数据，获得列表list
            IList<EyouSoft.Model.CompanyStructure.CompanySupplier> list = bll.GetList(pageSize, pageIndex, ref recordCount, sType, 0, 0, djName, this.CurrentUserCompanyID);
            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                //设置分页 
                BindPage();
            }
            else
            {
                //没有数据时隐藏分页控件 并 提示信息
                this.ExportPageInfo1.Visible = false;
                this.lblMsg.Text = "没有找到相关信息!";
            }
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
    }
}
