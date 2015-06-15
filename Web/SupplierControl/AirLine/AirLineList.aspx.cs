using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.SupplierControl.AirLine
{
    /// <summary>
    /// 航空公司列表页
    /// </summary>
    /// 柴逸宁
    /// 2011.4.18
    public partial class AirLineList : Eyousoft.Common.Page.BackPage
    {

        #region 变量
        /// <summary>
        /// 每页显示记录条数
        /// </summary>
        private int pageSize = 20;
        /// <summary>
        /// 当前第几页
        /// </summary>
        private int pageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        private int recordCount;
        /// <summary>
        /// 查询条件
        /// </summary>
        protected string unionName = string.Empty;

      
        /// <summary>
        /// 记录条数
        /// </summary>
        protected int len = 0;//列表长度


        //权限变量
        /// <summary>
        /// 新增权限
        /// </summary>
        protected bool grantadd = false;
        /// <summary>
        /// 修改权限
        /// </summary>
        protected bool grantmodify = false;
        /// <summary>
        /// 删除权限
        /// </summary>
        protected bool grantdel = false;
        /// <summary>
        /// 导入权限
        /// </summary>
        protected bool grantload = false;
        /// <summary>
        /// 导出权限
        /// </summary>
        protected bool grantto = false;



        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //获取操作
            string type = Utils.GetQueryStringValue("type");
            //获取权限

            grantadd = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_航空公司新增);
            grantdel = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_航空公司删除);
            grantload = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_航空公司导入);
            grantmodify = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_航空公司修改);
            grantto = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_航空公司导出);

            if (!IsPostBack)
            {
                switch (type)
                {

                    case "del":
                        //删除
                        if (grantdel)
                        {
                            AreaDel();
                        }
                        break;
                    case "toexcel":
                        //导出excel
                        if (grantto)
                        {
                            CreateExcel("area" + DateTime.Now.ToShortDateString());
                        }
                        break;

                    default:
                        //绑定数据

                        Bind();

                        break;

                }

            }
        }
        /// <summary>
        /// 初始化页面
        /// </summary>
        private void Bind()
        {
            //BLL
            EyouSoft.BLL.CompanyStructure.CompanySupplier csBLL = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            //初始化条件
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //公司名称
            unionName = EyouSoft.Common.Utils.GetQueryStringValue("unionName");
            //绑定数据
            IList<EyouSoft.Model.SupplierStructure.SupplierOther> list = null;
            list = csBLL.GetOtherList(pageSize, pageIndex, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.航空公司, unionName, CurrentUserCompanyID);

            if (!(list == null))
            {
                len = list.Count;
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                list = null;
            }
            //设置分页
            BindPage();
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
        /// 删除数据
        /// </summary>
        private void AreaDel()
        {
            EyouSoft.BLL.CompanyStructure.CompanySupplier csBLL = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            string[] stid = Utils.GetFormValue("tid").Split(',');
            int[] tid = new int[stid.Length];
            for (int i = 0; i < stid.Length; i++)
            {
                tid[i] = Utils.GetInt(stid[i]);
            }
            bool res = false;
            //删除
            res = csBLL.DeleteSupplierInfo(tid);
            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
        }


        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierOther> list = null;
            EyouSoft.BLL.CompanyStructure.CompanySupplier csBLL = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            //导出条件
            unionName = EyouSoft.Common.Utils.GetQueryStringValue("unionName");
            //获取一条记录，同事获取总记录条数
            list = csBLL.GetOtherList(1, 1, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.航空公司, unionName, CurrentUserCompanyID);
            //判断记录条数
            if (recordCount != 0)
            {
                //记录条数>0 获取所有记录
                list = csBLL.GetOtherList(recordCount, 1, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.航空公司, unionName, CurrentUserCompanyID);


            }

            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", "所在地", "单位名称", "联系人", "电话", "传真");
            foreach (EyouSoft.Model.SupplierStructure.SupplierOther cs in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n",
                    cs.ProvinceName + " " + cs.CityName,
                    cs.UnitName,
                    cs.SupplierContact == null ? "" : cs.SupplierContact[0].ContactName,
                    cs.SupplierContact == null ? "" : cs.SupplierContact[0].ContactTel,
                    cs.SupplierContact == null ? "" : cs.SupplierContact[0].ContactFax,
                    cs.TradeNum);
            }
            Response.Write(sb.ToString());
            Response.End();
        }


    }
}
