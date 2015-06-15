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
    /// 返佣金额列表
    /// </summary>
    /// 创建人：柴逸宁
    /// 创建时间：2001.6.7
    public partial class MonerList : Eyousoft.Common.Page.BackPage
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
        /// 是否现返金额
        /// </summary>
        protected bool isnow = false;
        /// <summary>
        /// 客户单位编号
        /// </summary>
        protected int customerid = 0;
        /// <summary>
        /// 客户单位操作员编号
        /// </summary>
        protected int contactid = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //获取操作类型
                string setcont = Utils.GetFormValue("GetCont");
                if (setcont.Length <= 0)
                {
                    #region 页面列表初始化
                    iframeId.Value = Utils.GetQueryStringValue("iframeId");
                    //实例化枚举
                    EyouSoft.Model.EnumType.CompanyStructure.CommissionType type = new EyouSoft.Model.EnumType.CompanyStructure.CommissionType();
                    //获取状态
                    int types = Utils.GetInt(Utils.GetQueryStringValue("type"));
                    //判断状态正确
                    if (types > 0)
                    {
                        #region 页面显示设定
                        //判断状态
                        type =
                            types == (int)EyouSoft.Model.EnumType.CompanyStructure.CommissionType.后返 ?
                            EyouSoft.Model.EnumType.CompanyStructure.CommissionType.后返 :
                            types == (int)EyouSoft.Model.EnumType.CompanyStructure.CommissionType.现返 ?
                            EyouSoft.Model.EnumType.CompanyStructure.CommissionType.现返 : EyouSoft.Model.EnumType.CompanyStructure.CommissionType.未确定;
                        //是否显示操作赋值
                        isnow = !(type == EyouSoft.Model.EnumType.CompanyStructure.CommissionType.后返);
                        #endregion
                        //获取客户单位编号
                        customerid = Utils.GetInt(Utils.GetQueryStringValue("customerid"));
                        //客户单位操作员编号
                        contactid = Utils.GetInt(Utils.GetQueryStringValue("contactid"));
                        if (customerid > 0 && contactid > 0)
                        {
                            //BLL
                            EyouSoft.BLL.StatisticStructure.BCommissionStat bll = new EyouSoft.BLL.StatisticStructure.BCommissionStat();
                            //qModel
                            MCommissionStatSeachInfo qModel = new MCommissionStatSeachInfo();
                            #region 查询实体
                            //单位名称
                            qModel.CustomerName = Utils.GetQueryStringValue("CustomerName");
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
                            }
                            //出团时间-终
                            DateTime? LeaveDateEnd = null;
                            if (Utils.GetQueryStringValue("LeaveDateEnd").Length > 0)
                            {
                                LeaveDateEnd = Utils.GetDateTime(Utils.GetQueryStringValue("LeaveDateEnd"));
                            }
                            //下单时间-终
                            DateTime? OrderDateEnd = null;
                            if (Utils.GetQueryStringValue("OrderDateEnd").Length > 0)
                            {
                                OrderDateEnd = Utils.GetDateTime(Utils.GetQueryStringValue("OrderDateEnd"));
                            }
                            //下单时间-始
                            DateTime? OrderDateStart = null;
                            if (Utils.GetQueryStringValue("OrderDateStart").Length > 0)
                            {
                                OrderDateStart = Utils.GetDateTime(Utils.GetQueryStringValue("OrderDateStart"));
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
                            IList<MCommissionDetailInfo> ls = new List<MCommissionDetailInfo>();
                            ls = bll.GetCommissionDetails(pageSize, pageIndex, ref recordCount, SiteUserInfo.CompanyID, customerid, contactid, type, qModel);
                            //绑定列表
                            rptList.DataSource = ls;
                            rptList.DataBind();
                            //绑定分页
                            BindPage();
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 提交支付信息
                    //切割数据
                    string[] the = setcont.Split(';');
                    int len = the.Length;
                    string[][] result = new string[len][];
                    StringBuilder msg = new StringBuilder();
                    //循环支付
                    for (int i = 0; i < len; i++)
                    {
                        result[i] = the[i].Split(',');
                        //支付实体
                        EyouSoft.Model.StatisticStructure.MPayCommissionInfo info = new MPayCommissionInfo();
                        //BLL
                        EyouSoft.BLL.StatisticStructure.BCommissionStat bll = new EyouSoft.BLL.StatisticStructure.BCommissionStat();
                        #region 支付实体赋值
                        //计划编号
                        info.TourId = result[i][0];
                        //支付金额
                        info.Amount = Utils.GetDecimal(result[i][1]);
                        //订单编号
                        info.OrderId = result[i][2];
                        //客户单位编号
                        info.BuyerCompanyId = Utils.GetInt(result[i][3]);
                        //公司编号
                        info.CompanyId = SiteUserInfo.CompanyID;
                        //支付时间
                        info.PayTime = DateTime.Now;
                        //支付人编号
                        info.PayerId = SiteUserInfo.ID;
                        #endregion
                        //执行支付
                        int ret = bll.PayCommission(info);
                        //int ret = 1;
                        //拼接支付结果
                        if (ret == 1)
                        {
                            msg.AppendFormat("{0} 支付成功！\\n", result[i][5]);
                        }
                        else
                        {
                            msg.AppendFormat("{0} 支付失败！\\n", result[i][5]);
                        }

                    }
                    //提示支付结果
                    Utils.ShowMsgAndCloseBoxy(msg.ToString(), Utils.GetFormValue("iframeId"), false);
                    #endregion
                }


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

        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }

    }
}
