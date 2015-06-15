using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using System.Text.RegularExpressions;
using EyouSoft.BLL.TourStructure;
using System.Text;
using EyouSoft.Model.TourStructure;

namespace Web.TeamPlan
{
    /// <summary>
    /// 团队计划添加人
    /// 功能：添加人
    /// 创建人：戴银柱
    /// 创建时间： 2011-02-14 
    /// </summary>
    public partial class TeamPlanAddMan : BackPage
    {
        protected StringBuilder VisitorArr = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //控件初始化
                LoadVisitors1.CurrentPageIframeId = Utils.GetQueryStringValue("iframeid");

                string orderId = Utils.GetQueryStringValue("orderId");
                string tourId = Utils.GetQueryStringValue("tourId");
                #region 配置游客是否必填
                EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting set = null;//配置实体
                set = setBll.GetSetting(CurrentUserCompanyID);
                hd_IsRequiredTraveller.Value = set.IsRequiredTraveller.ToString();
                #endregion
                if (orderId != "")
                {
                    this.hideOrderId.Value = orderId;
                }
                if (tourId != "")
                {
                    DataInit(tourId);
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected void DataInit(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> cus_list = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetTravellers(tourId);

            if (cus_list != null && cus_list.Count > 0)
            {

                string visitorState = string.Empty;
                bool isDoDelete = false;
                EyouSoft.BLL.TourStructure.TourOrder orderBll = new EyouSoft.BLL.TourStructure.TourOrder();

                VisitorArr.Append("[");
                foreach (var item in cus_list)
                {
                    isDoDelete = orderBll.IsDoDelete(item.ID, ref visitorState);
                    if (isDoDelete == true)
                    {
                        visitorState = string.Empty;
                    }

                    VisitorArr.Append("[");
                    VisitorArr.AppendFormat("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}'",
                        item.VisitorName, (int)item.VisitorType, (int)item.CradType, item.CradNumber,
                        (int)item.Sex, item.ContactTel, FormatSpecialServe(item.SpecialServiceInfo), item.ID, visitorState);
                    VisitorArr.Append("],");

                    visitorState = string.Empty;
                }
                VisitorArr.Remove(VisitorArr.Length - 1, 1);
                VisitorArr.Append("]");
            }
            else
            {
                VisitorArr.Append("[]");
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> cus_list = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();

            string[] visitorNameArr = Utils.GetFormValues("txtVisitorName");
            string[] visitorIdArr = Utils.GetFormValues("hidVisitorID");
            string[] visitorTypeArr = Utils.GetFormValues("ddlVisitorType");
            string[] cardTypeArr = Utils.GetFormValues("ddlCardType");
            string[] cardNoArr = Utils.GetFormValues("txtCardNo");
            string[] sexArr = Utils.GetFormValues("ddlSex");
            string[] contactTelArr = Utils.GetFormValues("txtContactTel");
            string[] uri = Utils.GetFormValues("tefu");
            string[] isDeleteArr = Utils.GetFormValues("isdelete");
            string visitorName = null;
            string hidVisitorID = null;
            bool isDelete = false;
            EyouSoft.Model.TourStructure.TourOrderCustomer item = null;
            EyouSoft.Model.TourStructure.CustomerSpecialService css = null;

            EyouSoft.BLL.TourStructure.TourOrder orderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);

            for (int k = 0; k < visitorNameArr.Length; k++)
            {
                item = new EyouSoft.Model.TourStructure.TourOrderCustomer();
                visitorName = visitorNameArr[k];
                hidVisitorID = visitorIdArr[k];
                isDelete = isDeleteArr[k] == "1" ? true : false;
                //获取 游客ID ,判断当前游客信息操作是新增，修改，或者删除
                if (hidVisitorID != "")//修改
                {
                    item.ID = hidVisitorID;
                    if (isDelete == true)//删除
                    {
                        item.IsDelete = true;
                    }
                }
                else//新增
                {
                    item.ID = Guid.NewGuid().ToString();
                    //如果姓名为空，则忽略这条信息
                    if (visitorName == "")
                    {
                        continue;
                    }
                }

                item.VisitorName = visitorName;
                //switch (visitorTypeArr[k])
                //{
                //    case "1":
                //        item.VisitorType = EyouSoft.Model.EnumType.TourStructure.VisitorType.成人;
                //        break;
                //    case "2":
                //        item.VisitorType = EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童;
                //        break;
                //    default:
                //        item.VisitorType = EyouSoft.Model.EnumType.TourStructure.VisitorType.未知;
                //        break;
                //}
                //switch (cardTypeArr[k])
                //{
                //    case "1":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.身份证;
                //        } break;
                //    case "2":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.护照;
                //        } break;
                //    case "3":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.军官证;
                //        } break;
                //    case "4":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.台胞证;
                //        } break;
                //    case "5":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.港澳通行证;
                //        } break;
                //    default:
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.未知;
                //        } break;
                //}
                item.VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)Utils.GetInt(visitorTypeArr[k]);
                item.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)Utils.GetInt(cardTypeArr[k]);
                item.CradNumber = cardNoArr[k];
                switch (sexArr[k])
                {
                    case "2":
                        {
                            item.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.男;
                        } break;
                    case "1":
                        {
                            item.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.女;
                        } break;
                    default:
                        {
                            item.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.未知;
                        } break;
                }
                item.ContactTel = contactTelArr[k];
                css = new EyouSoft.Model.TourStructure.CustomerSpecialService();

                css.Fee = Utils.GetDecimal(Utils.GetFromQueryStringByKey(uri[k], "txtCost"));
                css.IsAdd = Utils.GetFromQueryStringByKey(uri[k], "ddlOperate") == "0" ? true : false;
                css.IssueTime = DateTime.Now;
                css.ProjectName = Utils.GetFromQueryStringByKey(uri[k], "txtItem");
                css.ServiceDetail = Utils.GetFromQueryStringByKey(uri[k], "txtServiceContent");
                css.CustormerId = item.ID;
                item.SpecialServiceInfo = css;
                item.IssueTime = DateTime.Now;
                item.CompanyID = SiteUserInfo.CompanyID;
                cus_list.Add(item);
            }

            //保存游客信息
            if (orderBll.UpdateCustomerList(cus_list, this.hideOrderId.Value))
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('保存成功!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();");
            }
        }

        #region 格式化特服信息字符串
        /// <summary>
        /// 格式化特服信息字符串
        /// </summary>
        /// <param name="Service"></param>
        /// <returns></returns>
        private string FormatSpecialServe(CustomerSpecialService Service)
        {
            if (Service == null)
                return "";
            StringBuilder strService = new StringBuilder();
            if (!string.IsNullOrEmpty(Service.ProjectName))
                strService.AppendFormat("txtItem={0}&", (Service.ProjectName));
            if (!string.IsNullOrEmpty(Service.ServiceDetail))
                strService.AppendFormat("txtServiceContent={0}&", (Service.ServiceDetail));
            strService.AppendFormat("ddlOperate={0}&", Service.IsAdd ? "0" : "1");
            strService.AppendFormat("txtCost={0}&", (Service.Fee.ToString()));

            return strService.ToString().Trim('&');
        }
        #endregion
    }
}
