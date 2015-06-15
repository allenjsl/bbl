using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SupplierControl.AirLine
{
    /// <summary>
    /// 航空公司导入页面
    /// </summary>
    /// 柴逸宁
    /// 2011.4.18
    public partial class AirLineExcel : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限

            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_航空公司栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_航空公司栏目, false);
                return;

            }
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_航空公司导入))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_航空公司导入, false);
            }
            if (!IsPostBack)
            {
                //获取操作类型
                string type = Utils.GetQueryStringValue("type");
                if (type == "load")
                {
                    //导入操作
                    loadXls();
                }
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        private void loadXls()
        {
            string data = Utils.GetFormValue("dataxls");
            string[] s = data.Split(';');
            EyouSoft.BLL.SupplierStructure.SupplierOther ssBLL = new EyouSoft.BLL.SupplierStructure.SupplierOther();

            List<EyouSoft.Model.SupplierStructure.SupplierOther> list = new List<EyouSoft.Model.SupplierStructure.SupplierOther>();
            for (int i = 0; i < s.Length; i++)
            {
                string[] smodel = s[i].Split(',');

                //判断输出要求
                if (smodel.Length == 11 && !string.IsNullOrEmpty(smodel[0]) && !string.IsNullOrEmpty(smodel[1]) && !string.IsNullOrEmpty(smodel[2]))
                {
                    //实例化Model
                    EyouSoft.Model.SupplierStructure.SupplierOther ssModel = new EyouSoft.Model.SupplierStructure.SupplierOther();
                    //省份名称
                    ssModel.ProvinceName = HttpUtility.UrlDecode(smodel[0]);
                    //城市名称
                    ssModel.CityName = HttpUtility.UrlDecode(smodel[1]);
                    //单位名称
                    ssModel.UnitName = HttpUtility.UrlDecode(smodel[2]);
                    //地址
                    ssModel.UnitAddress = HttpUtility.UrlDecode(smodel[3]);
                    //操作人
                    ssModel.OperatorId = SiteUserInfo.ID;
                    //添加时间
                    ssModel.IssueTime = DateTime.Now;
                    //公司编号
                    ssModel.CompanyId = CurrentUserCompanyID;
                    //合作协议
                    ssModel.AgreementFile = string.Empty;
                    //是否删除
                    ssModel.IsDelete = false;
                    //单位类型
                    ssModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.航空公司;
                    //交易次数
                    ssModel.TradeNum = 0;
                    //实例化供应商联系人列表
                    ssModel.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                    //实例化供应商Model
                    EyouSoft.Model.CompanyStructure.SupplierContact scModel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    //联系人姓名
                    scModel.ContactName = HttpUtility.UrlDecode(smodel[4]);
                    //联系人职务
                    scModel.JobTitle = HttpUtility.UrlDecode(smodel[5]);
                    //联系人电话
                    scModel.ContactTel = HttpUtility.UrlDecode(smodel[6]);
                    //联系人手机
                    scModel.ContactMobile = HttpUtility.UrlDecode(smodel[7]);
                    //联系人QQ
                    scModel.QQ = HttpUtility.UrlDecode(smodel[8]);
                    //联系人邮箱
                    scModel.Email = HttpUtility.UrlDecode(smodel[9]);//email
                    //联系人列表赋值
                    ssModel.SupplierContact.Add(scModel);
                    //备注
                    ssModel.Remark = HttpUtility.UrlDecode(smodel[10]);
                    //赋值list
                    list.Add(ssModel);

                }
            }
            //导入并判断
            bool res = ssBLL.ImportExcelData(list);
            //输出导入结果
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
