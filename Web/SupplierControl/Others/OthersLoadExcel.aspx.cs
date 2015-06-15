using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SupplierControl.Others
{
    public partial class OthersLoadExcel : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 导入其他信息
        /// 作者：柴逸宁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_其它_栏目, false);
                return;

            }
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_导入))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_其它_导入, false);
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
            EyouSoft.BLL.SupplierStructure.SupplierOther ssBLL = new EyouSoft.BLL.SupplierStructure.SupplierOther();

            List<EyouSoft.Model.SupplierStructure.SupplierOther> list = new List<EyouSoft.Model.SupplierStructure.SupplierOther>();
            for (int i = 0; i < s.Length; i++)
            {
                string[] smodel = s[i].Split(',');


                if (smodel.Length == 11 && !string.IsNullOrEmpty(smodel[0]) && !string.IsNullOrEmpty(smodel[1]) && !string.IsNullOrEmpty(smodel[2]))
                {

                    EyouSoft.Model.SupplierStructure.SupplierOther ssModel = new EyouSoft.Model.SupplierStructure.SupplierOther();
                    ssModel.ProvinceName = HttpUtility.UrlDecode(smodel[0]);
                    ssModel.CityName = HttpUtility.UrlDecode(smodel[1]);
                    ssModel.UnitName = HttpUtility.UrlDecode(smodel[2]);
                    ssModel.UnitAddress = HttpUtility.UrlDecode(smodel[3]);

                    ssModel.OperatorId = SiteUserInfo.ID;
                    ssModel.IssueTime = DateTime.Now;
                    ssModel.CompanyId = CurrentUserCompanyID;
                    ssModel.AgreementFile = string.Empty;
                    ssModel.IsDelete = false;
                    ssModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.其他;
                    ssModel.TradeNum = 0;
                    ssModel.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                    EyouSoft.Model.CompanyStructure.SupplierContact scModel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    scModel.ContactName = HttpUtility.UrlDecode(smodel[4]);//姓名
                    scModel.JobTitle = HttpUtility.UrlDecode(smodel[5]);//职务
                    scModel.ContactTel = HttpUtility.UrlDecode(smodel[6]);//电话
                    scModel.ContactMobile = HttpUtility.UrlDecode(smodel[7]);//手机
                    scModel.QQ = HttpUtility.UrlDecode(smodel[8]);//QQ
                    scModel.Email = HttpUtility.UrlDecode(smodel[9]);//email

                    ssModel.SupplierContact.Add(scModel);

                    ssModel.Remark = HttpUtility.UrlDecode(smodel[10]);
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
