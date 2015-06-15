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
using System.Text;

namespace Web.SupplierControl.TicketService
{
    public partial class TicketService : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 20;
        protected int pageIndex = 1;
        protected int recordCount;


        protected int province = 0;
        protected int city = 0;
        protected string unionName = string.Empty;
        EyouSoft.BLL.CompanyStructure.CompanySupplier csBll = null;
        protected int len = 0;
        protected string act = string.Empty;


        //权限变量
        protected bool grantadd = false;//新增
        protected bool grantmodify = false;//修改
        protected bool grantdel = false;//删除
        protected bool grantload = false;//导入
        protected bool grantto = false;//导出

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_票务_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_票务_栏目, true);
            }

            //权限附值
            grantadd = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_票务_新增);
            grantmodify = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_票务_修改);
            grantdel = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_票务_删除);
            grantload = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_票务_导入);
            grantto = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_票务_导出);

            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            this.ucProvince1.IsFav = true;
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            this.ucCity1.IsFav = true;

            csBll = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            act = EyouSoft.Common.Utils.GetQueryStringValue("act");
            if (!IsPostBack)
            {
                switch (act)
                {
                    case "toexcel":
                        if (grantto)
                        {
                            CreateExcel("ticket" + DateTime.Now.ToShortDateString());
                        }
                        break;
                    case "ticketdel":
                        if (grantdel)
                        {
                            TicketDel();
                        }
                        break;
                    default:
                        DataInit();
                        break;
                }
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        private void TicketDel()
        {
            string[] stid = Utils.GetFormValue("tid").Split(',');
            int[] tid = new int[stid.Length];
            for (int i = 0; i < stid.Length; i++)
            {
                tid[i] = Utils.GetInt(stid[i]);
            }
            bool res = false;
            res = csBll.DeleteSupplierInfo(tid);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
        }

        /// <summary>
        /// 初使化
        /// </summary>
        private void DataInit()
        {
            //初使化条件
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            province = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("province"));
            city = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("city"));
            unionName = EyouSoft.Common.Utils.GetQueryStringValue("unionName");

            IList<EyouSoft.Model.CompanyStructure.CompanySupplier> list = null;
            list = csBll.GetList(pageSize, pageIndex, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务, province, city, unionName, this.CurrentUserCompanyID);

            len = list.Count;
            this.rptList.DataSource = list;
            this.rptList.DataBind();
            //设置分页
            BindPage();

            //设置 合计
            EyouSoft.Model.CompanyStructure.MSupplierSearchInfo searchModel = new EyouSoft.Model.CompanyStructure.MSupplierSearchInfo();

            if (city != 0)
            {
                searchModel.CityId = city;
            }

            if (province != 0)
            {
                searchModel.ProvinceId = province;
            }

            searchModel.Name = unionName;
 
            this.lblAllCount.Text = new EyouSoft.BLL.CompanyStructure.CompanySupplier().GetTimesGYSSummary(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务, searchModel).ToString();


            this.ucProvince1.ProvinceId = province;
            this.ucCity1.CityId = city;
            this.ucCity1.ProvinceId = province;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            province = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("province"));
            city = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("city"));
            unionName = EyouSoft.Common.Utils.GetQueryStringValue("unionName");
            IList<EyouSoft.Model.CompanyStructure.CompanySupplier> list = null;
            list = csBll.GetList(1, 1, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务, province, city, unionName, this.CurrentUserCompanyID);
            if (recordCount > 0)
            {
                list = csBll.GetList(recordCount, 1, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务, province, city, unionName, this.CurrentUserCompanyID);
            }
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", "所在地", "单位名称", "联系人", "电话", "传真", "交易情况", "政策");
            foreach (EyouSoft.Model.CompanyStructure.CompanySupplier cs in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n",
                    cs.ProvinceName + " " + cs.CityName,
                    cs.UnitName,
                    cs.SupplierContact == null ? "" : cs.SupplierContact[0].ContactName,
                    cs.SupplierContact == null ? "" : cs.SupplierContact[0].ContactTel,
                    cs.SupplierContact == null ? "" : cs.SupplierContact[0].ContactFax,
                    cs.TradeNum, cs.UnitPolicy);
            }
            Response.Write(sb.ToString());
            Response.End();
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
    }
}
