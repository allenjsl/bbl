using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Model.StatisticStructure;
using EyouSoft.Common;
using System.Text;


namespace Web.CRM.ReturnStatistics
{
    /// <summary>
    /// 返佣统计
    /// </summary>
    /// 创建人：柴逸宁
    /// 创建时间：2001.6.7
    public partial class ReturnList : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        /// <summary>
        /// 每页显示条数
        /// </summary>
        private int pageSize = 20;
        /// <summary>
        /// 当前页数
        /// </summary>
        private int pageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        private int recordCount;
        /// <summary>
        /// 查询条件-组团社Id
        /// </summary>
        protected string hd_teamId = string.Empty;
        /// <summary>
        /// 查询条件-组团社Name
        /// </summary>
        protected string txt_teamName = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_返佣统计_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_返佣统计_栏目, true);
                }
                //线路区域
                string type = Utils.GetQueryStringValue("type");
                //获取操作员
                if (type == "teamSelect")
                {
                    GroupUser();
                }
                //BLL
                EyouSoft.BLL.StatisticStructure.BCommissionStat bll = new EyouSoft.BLL.StatisticStructure.BCommissionStat();
                //实例化查询实体
                MCommissionStatSeachInfo qModel = new MCommissionStatSeachInfo();
                #region 查询实体
                //单位名称
                qModel.CustomerName = Utils.GetQueryStringValue("CustomerName");
                //查询条件-单位名称
                txtCustomerName.Text = qModel.CustomerName;
                //组团社Id
                hd_teamId = Utils.GetQueryStringValue("hd_teamId");
                //组团社名称
                txt_teamName = Utils.GetQueryStringValue("txt_teamName");
                //获取对方操作员转化成Int
                int Contact = Utils.GetInt(Utils.GetQueryStringValue("czyId"));
                //定义对方操作员int数组
                int[] ContactId;
                //对方操作员-存在
                if (Contact > 0)
                {
                    ContactId = new int[1];
                    //对方操作员Id赋值
                    ContactId[0] = Contact;
                    hd_ContactId.Value = Contact.ToString();
                }
                else
                {
                    ContactId = null;
                }
                //我社操作员
                string Operator = Utils.GetQueryStringValue("OperatorId");
                //存在我社操作员
                if (Operator.Length > 0)
                {
                    //拆分我社操作员
                    string[] OperatorSTR = Operator.Split(',');
                    //更具拆分情况定义我蛇操作员ID数组
                    int[] OperatorId = new int[OperatorSTR.Length];
                    //循环赋值
                    for (int i = 0; i < OperatorSTR.Length; i++)
                    {
                        OperatorId[i] = Utils.GetInt(OperatorSTR[i]);
                    }
                    //查询实体赋值
                    qModel.OperatorId = OperatorId;
                    selectOperator1.OperId = Operator;
                    selectOperator1.OperName = Utils.GetQueryStringValue("OperatorIdName");
               
                }
                else
                {
                    qModel.OperatorId = null;
                }
                #region 时间类型查询条件赋值
                //出团时间-始
                DateTime? LeaveDateStart = null;
                if (Utils.GetQueryStringValue("LeaveDateStart").Length > 0)
                {
                    LeaveDateStart = Utils.GetDateTime(Utils.GetQueryStringValue("LeaveDateStart"));
                    txtLeaveDateStart.Text = ((DateTime)LeaveDateStart).ToString("yyyy-MM-dd");
                }
                //出团时间-终
                DateTime? LeaveDateEnd = null;
                if (Utils.GetQueryStringValue("LeaveDateEnd").Length > 0)
                {
                    LeaveDateEnd = Utils.GetDateTime(Utils.GetQueryStringValue("LeaveDateEnd"));
                    txtLeaveDateEnd.Text = ((DateTime)LeaveDateEnd).ToString("yyyy-MM-dd");
                }
                //下单时间-终
                DateTime? OrderDateEnd = null;
                if (Utils.GetQueryStringValue("OrderDateEnd").Length > 0)
                {
                    OrderDateEnd = Utils.GetDateTime(Utils.GetQueryStringValue("OrderDateEnd"));
                    txtOrderDateEnd.Text = ((DateTime)OrderDateEnd).ToString("yyyy-MM-dd");
                }
                //下单时间-始
                DateTime? OrderDateStart = null;
                if (Utils.GetQueryStringValue("OrderDateStart").Length > 0)
                {
                    OrderDateStart = Utils.GetDateTime(Utils.GetQueryStringValue("OrderDateStart"));
                    txtOrderDateStart.Text = ((DateTime)OrderDateStart).ToString("yyyy-MM-dd");
                }
                #endregion
                //查询实体赋值
                //对方操作元
                qModel.ContactId = ContactId;
                //出团时间-始
                qModel.LeaveDateStart = LeaveDateStart;
                //出团时间-终
                qModel.LeaveDateEnd = LeaveDateEnd;
                //下单时间-终
                qModel.OrderDateEnd = OrderDateEnd;
                //下单时间-始
                qModel.OrderDateStart = OrderDateStart;
                #endregion
                //IList
                IList<MCommissionStatInfo> ls = new List<MCommissionStatInfo>();
                ls = bll.GetCommissions(pageSize, pageIndex, ref recordCount, SiteUserInfo.CompanyID, qModel);
                //判断总记录条数
                if (recordCount == 0)
                {
                    //数据为空的时候显示提示信息
                    lblMsg.Visible = true;
                }
                else
                {
                    lblMsg.Visible = false;

                }
                //绑定列表
                rptList.DataSource = ls;
                rptList.DataBind();
                //绑定分页
                BindPage();
            }
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
        /// <summary>
        /// 绑定操作员
        /// </summary>
        private void GroupUser()
        {
            int zId = Utils.GetInt(Utils.GetQueryStringValue("zId"));
            string cityHtml = string.Empty;
            EyouSoft.BLL.CompanyStructure.Customer csBLL = new EyouSoft.BLL.CompanyStructure.Customer();
            IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> csList = null;
            csList = csBLL.GetCustomerContactList(zId);
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat("<option value=\"0\"selected=\"selected\">--请选择--</option>");
            if (csList != null)
            {
                for (int i = 0; i < csList.Count; i++)
                {
                    strBuilder.AppendFormat("<option value=\"" + csList[i].ID + "\">" + csList[i].Name + "</option>");
                }
            }

            cityHtml = strBuilder.ToString();
            //异步执行代码返回
            Response.Clear();
            Response.Write(string.Format(cityHtml));
            Response.End();
            return;
        }
    }
}
