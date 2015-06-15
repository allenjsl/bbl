using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.CRM.customerinfos
{
    /// <summary>
    /// 导入客户信息
    /// xuty 2011/1/22
    /// </summary>
    public partial class ImportCustomer : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_客户资料_导入客户))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_客户资料_导入客户, true);
                return;
            }
            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();//客户bll
            string method = Utils.GetFormValue("method");
            if (method == "save")
            {
                #region 导入
                bool result = false;
                string[] customerArr = Utils.GetFormValues("custData");//获取导入的客户
                if (customerArr != null && customerArr.Length > 0)
                {
                    IList<EyouSoft.Model.CompanyStructure.CustomerInfo> custList = new List<EyouSoft.Model.CompanyStructure.CustomerInfo>();
                    foreach (string cust in customerArr)
                    {
                        string[] custArr = cust.Split(',');//获取每个客户的信息
                        //单位名称
                        int sname = Utils.GetQueryStringValue("sname") == "-1" ? -1 : Utils.GetInt(Utils.GetQueryStringValue("sname"));
                        //许可证号
                        int slice = Utils.GetQueryStringValue("slice") == "-1" ? -1 : Utils.GetInt(Utils.GetQueryStringValue("slice"));
                        //地址
                        int sadd = Utils.GetQueryStringValue("sadd") == "-1" ? -1 : Utils.GetInt(Utils.GetQueryStringValue("sadd"));
                        //邮编
                        int scode = Utils.GetQueryStringValue("scode") == "-1" ? -1 : Utils.GetInt(Utils.GetQueryStringValue("scode"));
                        //传真
                        int sfax = Utils.GetQueryStringValue("sfax") == "-1" ? -1 : Utils.GetInt(Utils.GetQueryStringValue("sfax"));
                        //主要联系人
                        int scon = Utils.GetQueryStringValue("scon") == "-1" ? -1 : Utils.GetInt(Utils.GetQueryStringValue("scon"));
                        //电话
                        int stel = Utils.GetQueryStringValue("stel") == "-1" ? -1 : Utils.GetInt(Utils.GetQueryStringValue("stel"));
                        //手机
                        int smob = Utils.GetQueryStringValue("smob") == "-1" ? -1 : Utils.GetInt(Utils.GetQueryStringValue("smob"));

                        EyouSoft.Model.CompanyStructure.CustomerInfo custModel = new EyouSoft.Model.CompanyStructure.CustomerInfo
                        {

                            Name = sname != -1 && custArr.Length >= sname ? custArr[sname] : "",
                            Licence = slice != -1 && custArr.Length >= slice ? custArr[slice] : "",
                            Adress = sadd != -1 && custArr.Length >= sadd ? custArr[sadd] : "",
                            PostalCode = scode != -1 && custArr.Length >= scode ? custArr[scode] : "",
                            ContactName = scon != -1 && custArr.Length >= scon ? custArr[scon] : "",
                            Phone = stel != -1 && custArr.Length >= stel ? custArr[stel] : "",
                            Mobile = smob != -1 && custArr.Length >= smob ? custArr[smob] : "",
                            Fax = sfax != -1 && custArr.Length >= sfax ? custArr[sfax] : "",
                            IsDelete = false,
                            IsEnable = true,

                            //Name = custArr[0],
                            //Licence = custArr[1],
                            //Adress = custArr[2],
                            //PostalCode = custArr[3],
                            //ContactName = custArr[5],
                            //Phone = custArr[6],
                            //Mobile = custArr[7],
                            //Fax = custArr[4],
                            //IsDelete = false,
                            //IsEnable = true,
                            CompanyId = CurrentUserCompanyID
                        };
                        custList.Add(custModel);
                    }
                    result = custBll.AddCustomerMore(custList);
                }
                Utils.ResponseMeg(result, result ? "导入成功！" : "导入失败！");
                return;
                #endregion
            }
        }
    }
}
