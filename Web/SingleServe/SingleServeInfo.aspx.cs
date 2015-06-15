using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;
using EyouSoft.Model.TourStructure;
using Common.Enum;
using System.IO;
using System.Text;
using Newtonsoft.Json.Serialization;
namespace Web.SingleServe
{
    /// <summary>
    /// 页面功能：单项服务新增/修改
    /// Author:liuym
    /// Date:2011-01-12
    /// </summary>
    public partial class SingleServeInfo : BackPage
    {
        #region Private Members
        protected bool IsUpdate = false;//是否修改操作
        protected string EditId = string.Empty;//修改ID
        protected string VisitorArr = string.Empty;//游客信息二维数组
        protected string SupplierJSON = string.Empty;//供应商JSON字符串
        protected string GuestRequestJSON = "{}";//客户要求JSON字符串
        protected EyouSoft.Model.TourStructure.TourSingleInfo EditModel = null;
        #endregion

        /// <summary>
        /// OnPreInit事件 设置模式窗体属性
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected bool IsChecked; //团队状态是5
        protected bool IsChecked1;//团队状态是4

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 下拉框添加点击事件
            ddlSupperlierList.Attributes.Add("onchange", "ChecklocaCustomer(this)");
            #endregion

            #region 修改权限验证
            if (!CheckGrant(TravelPermission.单项服务_单项服务_修改服务))
            {
                Utils.ResponseNoPermit(TravelPermission.单项服务_单项服务_修改服务, true);
                return;
            }
            #endregion

            #region 配置
            EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting set = setBll.GetSetting(CurrentUserCompanyID);//配置实体    
            hd_IsRequiredTraveller.Value = set.IsRequiredTraveller.ToString();//游客是否允许为空配置
            #endregion
            // 绑定项目类型
            BindPriceItem();
            if (Request.QueryString["EditId"] != null)//修改ID
            {
                EditId = Request.QueryString["EditId"];
            }

            #region 新增权限验证
            if (!CheckGrant(TravelPermission.单项服务_单项服务_新增服务))
            {
                Utils.ResponseNoPermit(TravelPermission.单项服务_单项服务_新增服务, true);
                return;
            }
            #endregion
            EyouSoft.BLL.TourStructure.TourOrder tourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(this.SiteUserInfo);

            this.txtOrderNo.Value = tourOrderBll.CreateOrderNo();


            if (!Page.IsPostBack)
            {
                #region 栏目权限验证
                if (!CheckGrant(TravelPermission.单项服务_单项服务_栏目))
                {
                    Utils.ResponseNoPermit(TravelPermission.单项服务_单项服务_栏目, true);
                    return;
                }
                #endregion

                LoadVisitors1.CurrentPageIframeId = Request.QueryString["iframeId"];

                if (EditId != "")
                {
                    EyouSoft.BLL.TourStructure.Tour tourBll = new EyouSoft.BLL.TourStructure.Tour(this.SiteUserInfo);
                    EditModel = (EyouSoft.Model.TourStructure.TourSingleInfo)tourBll.GetTourInfo(EditId);
                    if (EditModel == null)
                        Utils.ShowAndRedirect("未能找到您要修改的记录！", "/SingleServe/SingleServeList.aspx");
                    IsUpdate = true;//确认修改操作
                    IsChecked = ((int)EditModel.Status) == 5;//判断团队状态是否核算结束
                    IsChecked1 = ((int)EditModel.Status) == 4;//判断团队状态是否是财务核算
                    #region 初始化表单值
                    txtContactName.Value = EditModel.ContacterName;//联系人
                    txtCustomerCompany.Value = EditModel.BuyerCName;//客户单位名称
                    this.TxtStartTime.Value = EditModel.LDate.ToString("yyyy-MM-dd"); //委托出团日期
                    this.txtTourCode.Value = EditModel.TourCode; //团号
                    this.hideOldTeamNum.Value = EditModel.TourCode; //团号
                    hfSelectCustId.Value = EditModel.BuyerCId.ToString();//客户单位编号
                    selectOperator1.OperId = EditModel.SellerId.ToString();//销售员编号
                    selectOperator1.OperName = EditModel.SellerName;//销售员名称
                    txtOrderNo.Value = EditModel.OrderCode;//订单号
                    txtPersonNum.Value = EditModel.PlanPeopleNumber.ToString();
                    txtTel.Value = EditModel.ContacterTelephone;//电话
                    txtTotalIncome.Value = Utils.FilterEndOfTheZeroDecimal(EditModel.TotalAmount);//合计收入
                    txtTotalOutlay.Value = Utils.FilterEndOfTheZeroDecimal(EditModel.TotalOutAmount);//合计支出
                    txtGrossProfit.Value = Utils.FilterEndOfTheZeroDecimal(EditModel.GrossProfit);//毛利
                    hfOBuyerId.Value = EditModel.BuyerCId.ToString();
                    #region 绑定游客信息
                    if (EditModel.Customers != null && EditModel.Customers.Count > 0)
                    {
                        StringBuilder VisitorBuilder = new StringBuilder();
                        VisitorBuilder.Append("[");
                        foreach (var item in EditModel.Customers)
                        {
                            VisitorBuilder.Append("[");
                            VisitorBuilder.AppendFormat("'{0}','{1}','{2}','{3}','{4}','{5}','{6}'",
                                item.VisitorName, (int)item.VisitorType, (int)item.CradType, item.CradNumber,
                                (int)item.Sex, item.ContactTel, FormatSpecialServe(item.SpecialServiceInfo), item.ID);
                            VisitorBuilder.Append("],");
                        }
                        VisitorBuilder.Append("]");
                        VisitorArr = VisitorBuilder.ToString().Substring(0, VisitorBuilder.Length - 2) + "]";
                    }
                    else
                    {
                        VisitorArr = "[]";
                    }
                    #endregion

                    #region 客户要求
                    if (EditModel.Services != null && EditModel.Services.Count > 0)
                    {
                        GuestRequestJSON = Newtonsoft.Json.JsonConvert.SerializeObject(EditModel.Services);
                    }
                    else
                    {
                        GuestRequestJSON = "{}";
                    }
                    #endregion

                    #region 供应商安排
                    if (EditModel.Plans != null && EditModel.Plans.Count > 0)
                    {
                        SupplierJSON = Newtonsoft.Json.JsonConvert.SerializeObject(EditModel.Plans);
                    }
                    #endregion

                    #region 附件信息
                    if (!string.IsNullOrEmpty(EditModel.CustomerFilePath))
                    {
                        aFile.HRef = EditModel.CustomerFilePath;
                        aFile.Target = "_blank";
                        hFileInfo.Value = EditModel.CustomerFilePath;
                    }
                    #endregion

                    #endregion
                }
            }

            string type = Utils.GetQueryStringValue("type");
            if (type == "getInfo")
            {
                int cid = Utils.GetInt(Utils.GetQueryStringValue("cid"));
                EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();
                EyouSoft.Model.CompanyStructure.CustomerInfo model = custBll.GetCustomerModel(cid);
                Response.Clear();
                if (model != null)
                {
                    Response.Write(model.ContactName + "|" + model.Phone);
                }
                else
                {
                    Response.Write(" | ");
                }
                Response.End();
            }
        }
        #endregion


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
            strService.AppendFormat("txtItem={0}&", string.IsNullOrEmpty(Service.ProjectName) ? "" : Service.ProjectName);
            strService.AppendFormat("txtServiceContent={0}&", string.IsNullOrEmpty(Service.ServiceDetail) ? "" : Service.ServiceDetail);
            strService.AppendFormat("ddlOperate={0}&", Service.IsAdd ? "0" : "1");
            strService.AppendFormat("txtCost={0}&", Service.Fee.ToString());

            return strService.ToString().Trim('&');
        }
        #endregion

        #region 绑定项目类型
        private void BindPriceItem()
        {

            this.ddlGuestList.Items.Add(new ListItem("-请选择-", ""));
            this.ddlSupperlierList.Items.Add(new ListItem("-请选择-", ""));
            this.ddlSupperlierList.Items.Clear();
            this.ddlGuestList.Items.Clear();
            List<EnumObj> typeList = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType));
            /*if (typeList != null && typeList.Count > 0)
            {
                for (int i = 0; i < typeList.Count; i++)
                {
                    ListItem list_Item = new ListItem();
                    switch (typeList[i].Value)
                    {
                        case "-1":
                            list_Item.Text = "-请选择-";
                            break;
                        case "0":
                            list_Item.Text = "地接";
                            break;
                        case "1":
                            list_Item.Text = "酒店";
                            break;
                        case "2":
                            list_Item.Text = "用餐";
                            break;
                        case "3":
                            list_Item.Text = "景点";
                            break;
                        case "4":
                            list_Item.Text = "保险";
                            break;
                        case "5":
                            list_Item.Text = "大交通";
                            break;
                        case "6":
                            list_Item.Text = "导服";
                            break;
                        case "7":
                            list_Item.Text = "购物";
                            break;
                        case "8":
                            list_Item.Text = "小交通";
                            break;
                        case "9":
                            list_Item.Text = "其它";
                            break;
                        case "10":
                            list_Item.Text = "签证";
                            break;
                    }
                    list_Item.Value = typeList[i].Value;
                    this.ddlGuestList.Items.Add(list_Item);
                    this.ddlSupperlierList.Items.Add(list_Item);
                }
                //释放资源
                typeList = null;
            }*/

            if (typeList != null && typeList.Count > 0)
            {
                foreach (var item in typeList)
                {
                    var listItem = new ListItem(item.Text, item.Value);
                    this.ddlGuestList.Items.Add(listItem);
                    this.ddlSupperlierList.Items.Add(listItem);
                }
            }
        }
        #endregion

        #region 保存
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            string tourCode = Utils.InputText(this.txtTourCode.Value.Trim()); //团号

            #region 判断团号是否重复 by 田想兵 2011.8.30
            //将团号加入数组
            string[] tourCodes = new string[] { tourCode };
            //获得重复的数据集合
            IList<string> count = new EyouSoft.BLL.TourStructure.Tour().ExistsTourCodes(SiteUserInfo.CompanyID, EditId, tourCodes);
            if (count != null && count.Count > 0)
            {
                //如果该团号存在返回false
                Response.Write("<script>alert('该团号已存在！');location.href=location.href;</script>");
                return;
            }
            #endregion

            #region 成员变量
            decimal totalOutlay = 0.0M;
            decimal totalIncome = 0.0M;
            #endregion

            #region 获取参数
            int VisitorNum = Convert.ToInt32(Utils.InputText(txtPersonNum.Value.Trim().ToString()));
            string CustomerCompany = Utils.InputText(txtCustomerCompany.Value.Trim());//客户单位
            string CustomerCompanyId = Utils.InputText(hfSelectCustId.Value.Trim());//客户单位编号
            string OrderNo = Utils.InputText(txtOrderNo.Value.Trim());//订单号
            string SalsId = Utils.InputText(this.hfSaler.Value.Trim());//销售员Id
            string ContactTel = Utils.InputText(txtTel.Value.Trim());//联系电话
            string VisitorAttachment = "";//游客附件
            string ContactName = Utils.InputText(this.txtContactName.Value.Trim());//联系人
            string StartTime = Utils.InputText(this.TxtStartTime.Value.Trim());//委托出团日期
            if (Utils.GetFormValue(txtTotalOutlay.ClientID).Trim() != "")
            {

                totalOutlay = Utils.GetDecimal(Utils.GetFormValue(txtTotalOutlay.ClientID).Trim());
            }
            if (txtTotalIncome.Value.Trim().ToString() != "")
            {
                totalIncome = Utils.GetDecimal(txtTotalIncome.Value.Trim().ToString());
            }
            decimal GrossProfit = Utils.GetDecimal(txtGrossProfit.Value.Trim().ToString());//毛利
            #endregion

            #region 上传附件
            //确定是否有附件
            if (fuiLoadAttachment.PostedFile.ContentLength > 0)
            {
                string FileName = DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss_") + Guid.NewGuid().ToString() + Path.GetExtension(fuiLoadAttachment.PostedFile.FileName);
                if (!Directory.Exists(Server.MapPath("/uploadFiles/SingleServe")))
                {
                    Directory.CreateDirectory(Server.MapPath("/uploadFiles/SingleServe"));
                }
                VisitorAttachment = "/uploadFiles/SingleServe/" + FileName;
                fuiLoadAttachment.PostedFile.SaveAs(Server.MapPath(VisitorAttachment));
            }
            else
            {
                VisitorAttachment = hFileInfo.Value;
            }
            #endregion

            #region 客户要求
            string[] CustomerItemArr = Request.Form.GetValues("ddlGuestList");//服务类别
            string[] CustomerServeDetailArr = Request.Form.GetValues("txtGuestRequest");//具体要求
            string[] CustomerPriceItemArr = Request.Form.GetValues("txtGPrice");//价格
            string[] CusRemarks = Request.Form.GetValues("txtGuestRemark");//备注
            IList<TourSingleServiceInfo> CustomerServeList = null;//客户要求集合
            if (CustomerItemArr != null)
            {
                CustomerServeList = new List<TourSingleServiceInfo>();
                for (int i = 0; i < CustomerItemArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(CustomerItemArr[i].Trim()) || !string.IsNullOrEmpty(CustomerServeDetailArr[i].Trim()) || !string.IsNullOrEmpty(CustomerPriceItemArr[i].Trim()) || !string.IsNullOrEmpty(CusRemarks[i].Trim()))
                    {
                        TourSingleServiceInfo cModel = new TourSingleServiceInfo();
                        if (!string.IsNullOrEmpty(CustomerItemArr[i]))
                            cModel.ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)int.Parse(CustomerItemArr[i].Trim());
                        if (!string.IsNullOrEmpty(CusRemarks[i]))
                            cModel.Remark = Utils.InputText(CusRemarks[i].Trim(), 450);
                        if (!string.IsNullOrEmpty(CustomerServeDetailArr[i]))
                            cModel.Requirement = Utils.InputText(CustomerServeDetailArr[i].Trim(), 450);
                        if (!string.IsNullOrEmpty(CustomerPriceItemArr[i]))
                            cModel.SelfPrice = Utils.GetDecimal(CustomerPriceItemArr[i].Trim(), 0);
                        CustomerServeList.Add(cModel);
                    }
                }
            }
            #endregion

            #region 供应商安排
            string[] SupplierItemArr = Request.Form.GetValues("ddlSupperlierList");//服务类别
            string[] SupplierServeDetailArr = Request.Form.GetValues("txtSupplierRequest");//具体安排
            string[] SupplierPriceItemArr = Request.Form.GetValues("txtAccountCost");//结算费用
            string[] SupName = Request.Form.GetValues("txtSupperName");//供应商名称
            string[] SupRemark = Request.Form.GetValues("txtSupRemark");//备注
            string[] SupPlanIdArr = Request.Form.GetValues("hPlanID");//供应商安排主键编号
            string[] SupperIdArr = Request.Form.GetValues("hSupperID");//供应商编号
            IList<PlanSingleInfo> SupperList = null;//供应商集合
            if (SupplierItemArr != null)
            {
                SupperList = new List<PlanSingleInfo>();
                for (int i = 0; i < SupplierItemArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(SupplierItemArr[i].Trim()) || !string.IsNullOrEmpty(SupplierServeDetailArr[i].Trim()) || !string.IsNullOrEmpty(SupplierPriceItemArr[i].Trim()) || !string.IsNullOrEmpty(SupRemark[i].Trim()))
                    {
                        PlanSingleInfo pModel = new PlanSingleInfo();
                        if (SupPlanIdArr[i] != "0")
                            pModel.PlanId = SupPlanIdArr[i];
                        if (SupperIdArr[i] != "0")
                            pModel.SupplierId = int.Parse(SupperIdArr[i]);
                        if (!string.IsNullOrEmpty(SupplierItemArr[i]))
                            pModel.ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)int.Parse(SupplierItemArr[i].Trim());
                        if (!string.IsNullOrEmpty(SupRemark[i]))
                            pModel.Remark = Utils.InputText(SupRemark[i].Trim(), 450);
                        if (!string.IsNullOrEmpty(SupplierServeDetailArr[i]))
                            pModel.Arrange = Utils.InputText(SupplierServeDetailArr[i].Trim(), 450);
                        if (!string.IsNullOrEmpty(SupName[i]))
                            pModel.SupplierName = Utils.InputText(SupName[i].Trim(), 100);
                        if (!string.IsNullOrEmpty(SupplierPriceItemArr[i]))
                            pModel.Amount = Utils.GetDecimal(SupplierPriceItemArr[i].Trim(), 0);//结算费用
                        pModel.CreateTime = DateTime.Now;
                        SupperList.Add(pModel);
                    }
                }
            }
            #endregion

            #region 游客信息
            string[] VisitorNameArr = Request.Form.GetValues("txtVisitorName");//游客姓名
            string[] VisitorTypeArr = Request.Form.GetValues("ddlVisitorType");//旅客类型
            string[] CardTypeArr = Request.Form.GetValues("ddlCardType");//证件类型
            string[] CardNoArr = Request.Form.GetValues("txtCardNo");//证件号
            string[] ContactTelArr = Request.Form.GetValues("txtContactTel");//联系电话
            string[] SexArr = Request.Form.GetValues("ddlSex");//性别
            string[] ServeItemsArr = Request.Form.GetValues("tefu");//特服
            string[] VisitorIDArr = Request.Form.GetValues("hidVisitorID");//游客主键
            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> VisitorList = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();//游客信息集合
            if (VisitorNameArr != null)
            {
                for (int j = 0; j < VisitorNameArr.Length; j++)
                {
                    TourOrderCustomer tModel = new TourOrderCustomer();

                    if (EditId != "")//修改
                    {
                        if (string.IsNullOrEmpty(VisitorNameArr[j]))
                        {
                            continue;
                        }
                        tModel.IsDelete = false;//修改时 如果姓名为空  就忽略这条信息
                    }
                    else
                    {//新增  
                        if (!string.IsNullOrEmpty(VisitorNameArr[j]))//游客姓名不为空
                        {
                            tModel.IsDelete = false;
                        }
                        else
                        {
                            tModel.IsDelete = true;//新增 如果游客姓名为空，就不存入数据库 
                        }
                    }
                    #region 游客基本信息
                    if (!string.IsNullOrEmpty(VisitorIDArr[j]))
                        tModel.ID = VisitorIDArr[j];
                    else
                        tModel.ID = Guid.NewGuid().ToString();
                    //tModel.TicketStatus = EyouSoft.Model.EnumType.TourStructure.CustomerTicketStatus.正常;
                    tModel.CompanyID = SiteUserInfo.CompanyID;
                    if (!string.IsNullOrEmpty(VisitorNameArr[j]))
                        tModel.VisitorName = Utils.InputText(VisitorNameArr[j], 50);
                    if (!string.IsNullOrEmpty(ContactTelArr[j]))
                        tModel.ContactTel = Utils.InputText(ContactTelArr[j].Trim(), 12);
                    if (!string.IsNullOrEmpty(CardNoArr[j]))
                        tModel.CradNumber = Utils.InputText(CardNoArr[j].Trim(), 30);
                    tModel.CompanyID = this.SiteUserInfo.CompanyID;
                    if (!string.IsNullOrEmpty(CardTypeArr[j]))
                        tModel.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)int.Parse(CardTypeArr[j].Trim());
                    tModel.IssueTime = DateTime.Now;
                    if (!string.IsNullOrEmpty(SexArr[j]))
                        tModel.Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)int.Parse(SexArr[j].Trim());
                    if (!string.IsNullOrEmpty(VisitorTypeArr[j]))
                        tModel.VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)int.Parse(VisitorTypeArr[j].Trim());
                    #endregion

                    #region 游客特服信息
                    if (ServeItemsArr != null && !string.IsNullOrEmpty(ServeItemsArr[j]))
                    {
                        CustomerSpecialService SpecialServiceInfo = new CustomerSpecialService();
                        string[] ServiceItem = ServeItemsArr[j].Split('$');
                        SpecialServiceInfo.CustormerId = tModel.ID;
                        SpecialServiceInfo.IssueTime = DateTime.Now;
                        int index = 0;
                        foreach (var item in ServiceItem)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                switch (index)
                                {
                                    case 0:
                                        SpecialServiceInfo.ProjectName = Utils.InputText(item, 450);
                                        break;
                                    case 1:
                                        SpecialServiceInfo.ServiceDetail = Utils.InputText(item, 450);
                                        break;
                                    case 2:
                                        SpecialServiceInfo.Fee = Utils.GetDecimal(item);
                                        break;
                                    case 3:
                                        SpecialServiceInfo.IsAdd = item == "0" ? true : false;
                                        break;
                                }
                            }
                            index++;
                        }
                        tModel.SpecialServiceInfo = SpecialServiceInfo;
                    }
                    #endregion

                    VisitorList.Add(tModel);
                    tModel = null;
                }
            }
            #endregion

            #region 成员变量
            EyouSoft.Model.TourStructure.TourSingleInfo model = new EyouSoft.Model.TourStructure.TourSingleInfo();
            EyouSoft.BLL.TourStructure.Tour tourBll = new EyouSoft.BLL.TourStructure.Tour(this.SiteUserInfo);
            #endregion

            //实体赋值
            if (model != null)
            {
                model.CreateTime = DateTime.Now;//发布时间                
                model.BuyerCName = CustomerCompany;//客户单位名称
                model.TourCode = tourCode; //团号
                model.LDate = DateTime.Parse(StartTime); //出团日期
                model.BuyerCId = int.Parse(CustomerCompanyId);//客户单位编号
                model.CompanyId = this.SiteUserInfo.CompanyID;//公司ID
                model.Plans = SupperList;//供应商安排集合
                model.Services = CustomerServeList;//客户要求集合
                model.Customers = VisitorList;//游客信息集合
                model.TotalAmount = totalIncome;//收入
                model.TotalOutAmount = totalOutlay;//支出
                model.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.单项服务;//团队类型
                model.VirtualPeopleNumber = 0;//虚拟实收

                model.ContacterMobile = this.SiteUserInfo.ContactInfo.ContactMobile;
                model.ContacterTelephone = this.SiteUserInfo.ContactInfo.ContactTel;
                if (!string.IsNullOrEmpty(VisitorAttachment))
                {
                    model.CustomerDisplayType = EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.附件方式;
                }
                else
                    model.CustomerDisplayType = EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.输入方式;

                if (!string.IsNullOrEmpty(SalsId))
                {
                    if (SalsId.Split(',').Length > 0)
                    {
                        model.SellerId = Utils.GetInt(SalsId.Split(',')[0]);
                        model.SellerName = this.selectOperator1.OperName.Split(',')[0];
                    }
                    else
                    {
                        model.SellerId = Utils.GetInt(SalsId);
                        model.SellerName = this.selectOperator1.OperName;
                    }

                }
                model.PlanPeopleNumber = VisitorNum;//游客人数
                model.GrossProfit = GrossProfit;//毛利
                model.CustomerFilePath = VisitorAttachment;//游客信息附件
                model.ContacterTelephone = ContactTel;//联系电话
                model.ContacterName = ContactName;//联系人
                model.TourDays = 0;
                model.OrderCode = OrderNo;//订单号
                //model.LDate = DateTime.Now;
                model.OperatorId = this.SiteUserInfo.ID;//操作员
                if (EditId != "")
                {//修改操作
                    model.TourId = EditId;  //团队编号  
                    model.OBuyerCId = int.Parse(hfOBuyerId.Value.ToString());//客户单位编号
                    if (tourBll.UpdateSingleTourInfo(model) > 0)
                        EyouSoft.Common.Function.MessageBox.ResponseScript(this, string.Format("alert('修改成功!');window.parent.Boxy.getIframeDialog('{0}').hide();window.parent.location='/SingleServe/SingleServeList.aspx';", Request.QueryString["iframeId"]));
                    else
                        EyouSoft.Common.Function.MessageBox.ResponseScript(this, string.Format("alert('修改失败!');window.parent.Boxy.getIframeDialog('{0}').hide();window.parent.location='/SingleServe/SingleServeList.aspx';", Request.QueryString["iframeId"]));
                }
                else
                {//新增操作
                    if (tourBll.InsertSingleTourInfo(model) > 0)
                        EyouSoft.Common.Function.MessageBox.ResponseScript(this, string.Format("alert('新增成功!');window.parent.Boxy.getIframeDialog('{0}').hide();window.parent.location='/SingleServe/SingleServeList.aspx';", Request.QueryString["iframeId"]));
                    else
                        EyouSoft.Common.Function.MessageBox.ResponseScript(this, string.Format("alert('修改失败!');window.parent.Boxy.getIframeDialog('{0}').hide();window.parent.location='/SingleServe/SingleServeList.aspx';", Request.QueryString["iframeId"]));
                }
            }
            //释放资源
            model = null;
            tourBll = null;
        }
        #endregion



    }
}
