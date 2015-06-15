using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EyouSoft.Common;
using System.Web.UI.MobileControls;
using System.Collections.Generic;

namespace Web.SupplierControl.Shopping
{
    /// <summary>
    /// 购物-导入信息
    /// 作者：柴逸宁
    /// 2011-10
    /// </summary>
    public partial class ShoppingLoadExcel : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_购物_栏目, false);
                return;

            }
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_导入))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_购物_导入, false);
            }
            if (!IsPostBack)
            {
                string act = Utils.GetQueryStringValue("act");
                if (act == "load")
                {
                    loadXls();
                }
            }
        }

        private void loadXls()
        {
            string data = Utils.GetFormValue("dataxls");
            string[] s = data.Split(';');
            EyouSoft.BLL.SupplierStructure.SupplierShopping ssBLL = new EyouSoft.BLL.SupplierStructure.SupplierShopping();

            List<EyouSoft.Model.SupplierStructure.SupplierShopping> list = new List<EyouSoft.Model.SupplierStructure.SupplierShopping>();
            for (int i = 0; i < s.Length; i++)
            {
                string[] smodel = s[i].Split(',');
                if (smodel.Length == 13 && !string.IsNullOrEmpty(smodel[0]) && !string.IsNullOrEmpty(smodel[1]) && !string.IsNullOrEmpty(smodel[2]))
                {
                    EyouSoft.Model.SupplierStructure.SupplierShopping ssModel = new EyouSoft.Model.SupplierStructure.SupplierShopping();
                    ssModel.ProvinceName = HttpUtility.UrlDecode(smodel[0]);//省份
                    ssModel.CityName = HttpUtility.UrlDecode(smodel[1]);//城市
                    ssModel.UnitName = HttpUtility.UrlDecode(smodel[2]);//单位名称
                    ssModel.SaleProduct = HttpUtility.UrlDecode(smodel[3]);//销售产品
                    ssModel.GuideWord = HttpUtility.UrlDecode(smodel[4]);//导游词
                    ssModel.UnitAddress = HttpUtility.UrlDecode(smodel[5]);//地址

                    ssModel.OperatorId = SiteUserInfo.ID;//操作人
                    ssModel.IssueTime = DateTime.Now;//添加时间
                    ssModel.CompanyId = CurrentUserCompanyID;//公司编号
                    ssModel.AgreementFile = string.Empty;//协议
                    ssModel.IsDelete = false;//是否删除
                    ssModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.购物;//单位类型
                    ssModel.TradeNum = 0;//交易次数
                    //联系人
                    ssModel.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                    EyouSoft.Model.CompanyStructure.SupplierContact scModel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    scModel.ContactName = HttpUtility.UrlDecode(smodel[6]);//姓名
                    scModel.JobTitle = HttpUtility.UrlDecode(smodel[7]);//职务
                    scModel.ContactTel = HttpUtility.UrlDecode(smodel[8]);//电话
                    scModel.ContactMobile = HttpUtility.UrlDecode(smodel[9]);//手机
                    scModel.QQ = HttpUtility.UrlDecode(smodel[10]);//QQ
                    scModel.Email = HttpUtility.UrlDecode(smodel[11]);//email

                    ssModel.SupplierContact.Add(scModel);

                    ssModel.Remark = HttpUtility.UrlDecode(smodel[12]);


                    list.Add(ssModel);
                }
            }


            bool res = ssBLL.ImportExcelData(list);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
        }

        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }
    }
}
