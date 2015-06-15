using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
namespace Web.CRM.customerinfos
{
    /// <summary>
    /// 客户资料
    /// xuty 2011/1/17
    /// </summary>
    public partial class CustomerList : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex; 
        protected int recordCount;
        protected int pageSize = 20;
        protected bool IsDelete;//是否有删除权限
        protected bool IsStop;//是否有停用权限
        protected bool IsUpdate;//是否有更新权限
        protected bool IsAdd;//是否有新增权限
        protected bool IsImportIn;//是否有导入客户权限
        protected bool IsImportOut;//是否有导出客户权限
        protected int i = 0;//客户资料编号
        protected string Orderbyid = string.Empty;//排序编号

        protected void Page_Load(object sender, EventArgs e)
        {
            BindSellerlist();
            if (!IsPostBack)
            {
                //初始化绑定部门员工列表
                EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser();
                IList<EyouSoft.Model.CompanyStructure.CompanyUser> userList = userBll.GetCompanyUser(CurrentUserCompanyID);
                if (userList != null && userList.Count > 0)
                {
                    foreach (EyouSoft.Model.CompanyStructure.CompanyUser userItem in userList)
                    {
                        ddlSalePeople.Items.Add(new ListItem(userItem.PersonInfo.ContactName, userItem.ID.ToString()));
                    }
                }
                ddlSalePeople.Items.Insert(0, new ListItem("请选择", ""));
            }
           
            #region 判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_客户资料_栏目, true);
                return;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_停用帐号))
            {
                IsStop = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_删除客户))
            {
                IsDelete = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_修改客户))
            {
                IsUpdate = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_新增客户))
            {
                IsAdd = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_导入客户))
            {
                IsImportIn = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_导出客户))
            {
                IsImportOut = true;
            }
            #endregion

            i = Utils.GetInt(Utils.GetFormValue("ids"));

            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();//客户bll
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);//页码
            bool result = false;//操作结果
            #region 当前操作
            string method = Utils.GetFormValue("method");
            if (method == "del")
            {
                //删除 获取客户信息所有的编号集合
                string ids = Utils.GetFormValue("ids");
                string[] idlist = ids.Split(',');
                if (idlist.Length > 0)
                {
                    for (int k = 0; k < idlist.Length - 1; k++)
                    {
                        result = custBll.DeleteCustomer(Utils.GetInt(idlist[k]));
                    }
                }
                Utils.ResponseMeg(result, result ? "删除成功！" : "删除失败！");
                return;
            }
            if (method == "stop")
            {   //停用
                string ids = Utils.GetFormValue("ids");
                result = custBll.StopIt(Utils.GetInt(ids));
                Utils.ResponseMeg(result, result ? "已停用！" : "停用失败！");
                return;
            }
            if (method == "start")
            {   //启用
                string ids = Utils.GetFormValue("ids");
                result = custBll.StartId(Utils.GetInt(ids));
                Utils.ResponseMeg(result, result ? "已启用！" : "启用失败！");
                return;
            }
            #endregion

            #region 查询参数
            string contacter = Utils.InputText(Request.QueryString["contacter"] == null ? "" : Server.UrlDecode(Request.QueryString["contacter"]));//联系人
            int[] provinceIds = Utils.GetIntArray(Utils.GetQueryStringValue("province"), ",");//省份
            int[] cityIds = Utils.GetIntArray(Utils.GetQueryStringValue("city"),",");//城市
            string saler = Utils.InputText(Request.QueryString["saler"] == null ? "" : Server.UrlDecode(Request.QueryString["saler"]));//责任销售
            string companyName = Utils.InputText(Request.QueryString["saler"] == null ? "" : Server.UrlDecode(Request.QueryString["companyName"]));//单位名称
            string telephone=Utils.GetQueryStringValue("telephone");
            #endregion

            int pageCount = 1;//页数
            IList<EyouSoft.Model.CompanyStructure.CustomerInfo> list = null;//客户资料集合

            #region 导出客户Excel
            //导出Excel
            if (Utils.GetQueryStringValue("method") == "downexcel")
            {
                string strPageSize = Utils.GetQueryStringValue("recordcount");
                pageSize = Utils.GetInt(strPageSize == "0" ? "1" : strPageSize);//导出游客时获取全部的查询数据
                list = custBll.SearchCustomerList(pageSize, pageIndex, CurrentUserCompanyID, provinceIds, cityIds, companyName, contacter, saler, ref recordCount, ref pageCount, telephone);
                DownLoadExcel(list);
                return;
            }
            #endregion

            #region 绑定客户列表
            //客户资料排序实体
            EyouSoft.Model.CompanyStructure.MCustomerSeachInfo Seachinfo = new EyouSoft.Model.CompanyStructure.MCustomerSeachInfo();           

            //排序方式
            int order = Utils.GetInt(Utils.GetQueryStringValue("order"));
            if (order == 0)
            {
                //绑定客户列表
                list = custBll.SearchCustomerList(pageSize, pageIndex, CurrentUserCompanyID, provinceIds, cityIds, companyName, contacter, saler, ref recordCount, ref pageCount, telephone);
                if (list != null && list.Count > 0)
                {
                    rptCustomer.DataSource = list;
                    rptCustomer.DataBind();
                    BindExportPage();
                }
                else
                {
                    rptCustomer.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无客户资料信息！</td></tr>";
                    ExportPageInfo1.Visible = false;
                }
            }
            else
            {
                #region 标题排序
                switch (order)
                {
                    case 1:
                        Orderbyid = "1";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.客户名称;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 2:
                        Orderbyid = "2";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.客户名称;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 3:
                        Orderbyid = "3";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人名称;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 4:
                        Orderbyid = "4";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人名称;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 5:
                        Orderbyid = "5";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人电话;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 6:
                        Orderbyid = "6";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人电话;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 7:
                        Orderbyid = "7";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人手机;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 8:
                        Orderbyid = "8";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人手机;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 9:
                        Orderbyid = "9";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人传真;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 10:
                        Orderbyid = "10";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人传真;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 11:
                        Orderbyid = "11";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.责任销售;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 12:
                        Orderbyid = "12";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.责任销售;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 13:
                        Orderbyid = "13";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.交易次数;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 14:
                        Orderbyid = "14";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.交易次数;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 17:
                        Orderbyid = "17";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.结算方式;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 18:
                        Orderbyid = "18";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.结算方式;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 19:
                        Orderbyid = "19";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.省份;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 20:
                        Orderbyid = "20";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.省份;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 21:
                        Orderbyid = "21";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.地址;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.ASC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                    case 22:
                        Orderbyid = "22";
                        Seachinfo.OrderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.地址;
                        Seachinfo.OrderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;
                        list = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomers(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, Seachinfo);
                        break;
                }
                #endregion 
                if (list != null && list.Count > 0)
                {
                    rptCustomer.DataSource = list;
                    rptCustomer.DataBind();
                    BindExportPage();
                }
                else
                {
                    rptCustomer.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无客户资料信息！</td></tr>";
                    ExportPageInfo1.Visible = false;
                }
            }
            #endregion

            //恢复查询关键字
            txtCompanyName.Value = companyName;//公司名
            txtContact.Value = contacter;//联系人
            this.ddlSalePeople.SelectedValue = saler; //责任销售人
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams.Add(Request.QueryString);
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;

            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams.Add(Request.QueryString);
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
            
        }
        #endregion

        #region 导出客户资料方法
        /// <summary>
        /// 导出客户列表
        /// </summary>
        /// <param name="list"></param>
        protected void DownLoadExcel(IList<EyouSoft.Model.CompanyStructure.CustomerInfo> list)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("所在地\t单位名称\t联系人\t电话\t手机\t传真\t地址\t责任销售\t交易情况\n");
            foreach (EyouSoft.Model.CompanyStructure.CustomerInfo d in list)
            {
                strBuilder.AppendFormat("{0}-{9}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\n", d.ProvinceName, d.Name, d.ContactName, d.Phone, d.Mobile, d.Fax, d.Adress, d.Saler, d.TradeNum, d.CityName);
            }
            Response.Clear();
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Write(strBuilder.ToString());
            Response.End();
        }
        #endregion

        #region 绑定责任销售员
        protected void BindSellerlist()
        {
            //初始化绑定部门员工列表
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> userList = userBll.GetCompanyUser(CurrentUserCompanyID);
            if (userList != null && userList.Count > 0)
            {
                foreach (EyouSoft.Model.CompanyStructure.CompanyUser userItem in userList)
                {
                    this.ddl_Oprator.Items.Add(new ListItem(userItem.PersonInfo.ContactName, userItem.ID.ToString()));
                }
            }
            this.ddl_Oprator.Items.Insert(0, new ListItem("请选择", ""));        
        }
        #endregion

        #region 责任销售批量修改
        /// <summary>
        ///责任销售批量修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //销售员编号
            int Sellerid = Utils.GetInt(Utils.GetFormValue(this.ddl_Oprator.UniqueID));
            if (Sellerid <= 0)
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, Utils.ShowMsg("请选择责任销售员!"));
                Response.Redirect("/CRM/customerinfos/CustomerList.aspx");
                return;
            }
            //销售员名字
            string SellerName =Utils.GetFormValue(this.SellerName.UniqueID);
            if (SellerName == "" && string.IsNullOrEmpty(SellerName))
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, Utils.ShowMsg("请选择责任销售员!"));
                Response.Redirect("/CRM/customerinfos/CustomerList.aspx");
                return;
            }
            //客户资料集合编号
            string CustomerIds = Utils.GetFormValue(this.Customerids.UniqueID);
            if (!string.IsNullOrEmpty(CustomerIds) && CustomerIds != "")
            {
                IList<string> list = CustomerIds.Split(',').ToList();
                IList<int> ids = null;
                if (list != null && list.Count > 0)
                {
                    ids = new List<int>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        ids.Add(Convert.ToInt32(list[i].ToString()));
                    }
                }
                bool result = false;
                result = new EyouSoft.BLL.CompanyStructure.Customer().BatchSpecifiedSeller(SiteUserInfo.CompanyID, ids, Sellerid, SellerName);
                Utils.ShowAndRedirect(result ? "批量修改成功!" : "批量修改失败!", "/CRM/customerinfos/CustomerList.aspx");
            }                      
        }
        #endregion
    }

}
