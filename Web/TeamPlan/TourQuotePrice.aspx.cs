using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;
using EyouSoft.Common.Function;
using Common.Enum;

namespace Web.TeamPlan
{
    /// <summary>
    /// 创建:万俊
    /// 功能:组团社询价—报价
    /// </summary>
    public partial class TourQuotePrice : BackPage
    {
        protected string type;      //操作类型
        /// <summary>
        /// 游客信息 
        /// by txb2011.7.16
        /// </summary>
        protected string cusHtml = string.Empty;
        EyouSoft.BLL.TourStructure.LineInquireQuoteInfo lineQuoteBll = new EyouSoft.BLL.TourStructure.LineInquireQuoteInfo();
        EyouSoft.BLL.CompanyStructure.Area areaBll = new EyouSoft.BLL.CompanyStructure.Area();
        protected string file;  //页面上附件的路径
        protected void Page_Load(object sender, EventArgs e)
        {
            //询价权限判断
            if (!CheckGrant(TravelPermission.团队计划_组团社询价_栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.团队计划_组团社询价_栏目, true);
            }
            int id = 0;
            string ids = "";
            this.xianluWindow1.publishType = 1;
            type = Utils.GetQueryStringValue("type");
            if (!IsPostBack)
            {
                if (type == "create")
                {
                    id = Utils.GetInt(Utils.GetQueryStringValue("id"));
                    InitInfo(id);
                }
                else if (type == "del")
                {
                    ids = Utils.GetQueryStringValue("ids");
                    Del(ids);
                }
                BindArea();
                #region 配置
                EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting set = setBll.GetSetting(CurrentUserCompanyID);//配置实体
                hd_IsRequiredTraveller.Value = set.IsRequiredTraveller.ToString();
                #endregion
            }

        }

        #region 初始化赋值_返回保存的类
        protected EyouSoft.Model.TourStructure.LineInquireQuoteInfo InitInfo(int id)
        {
            //记录我社报价价格
            decimal price = 0;
            //询价实体类
            EyouSoft.Model.TourStructure.LineInquireQuoteInfo lineInfo = new EyouSoft.Model.TourStructure.LineInquireQuoteInfo();
            lineQuoteBll = new EyouSoft.BLL.TourStructure.LineInquireQuoteInfo(SiteUserInfo);
            lineInfo = lineQuoteBll.GetQuoteModel(id, SiteUserInfo.CompanyID, 0, false);
            if (lineInfo != null)
            {
                //为类赋值
                //折扣
                this.txt_agio.Value = Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(lineInfo.TicketAgio.ToString()).ToString("0.00"));
                //出团日期
                this.txt_BegTime.Value = ((DateTime)(lineInfo.LeaveDate)).ToString("yyyy-MM-dd");
                //联系人姓名 
                this.txt_contactName.Value = Utils.GetString(lineInfo.ContactName, "");
                //联系人电话
                this.txt_contactTel.Value = Utils.GetString(lineInfo.ContactTel, "");
                //总人数
                this.txt_peopleCount.Value = Utils.GetString(Convert.ToString(lineInfo.PeopleNum), "");
                //询价单位名称
                this.txt_quoteCompany.Value = Utils.GetString(lineInfo.CustomerName, "");
                //询价单位编号
                this.hid_QuoteCompanyId.Value = Convert.ToString(lineInfo.CustomerId);
                //客户要求用户控件赋值
                this.ConProjectControl1.SetList = lineInfo.Requirements;
                ConProjectControl1.SetDataList();
                //价格组成用户控件赋值
                this.PriceControl1.SetList = lineInfo.Services;
                this.PriceControl1.TotalAmount = lineInfo.TotalAmount;
                //行程要求
                if (lineInfo.XingCheng != null)
                {
                    this.txtRoutingNeed.Text = Utils.GetString(lineInfo.XingCheng.QuotePlan, "");
                    file = Utils.GetString(lineInfo.XingCheng.PlanAccessory, "");
                    this.hid_img_name.Value = Utils.GetString(lineInfo.XingCheng.PlanAccessoryName, "");
                    this.hid_img_path.Value = Utils.GetString(lineInfo.XingCheng.PlanAccessory, "");
                }
                //状态
                this.lblResult.Text = Convert.ToString(lineInfo.QuoteState);
                //特殊需求
                this.txt_need.Value = Utils.GetString(lineInfo.SpecialClaim, "");
                //线路用户控件赋值
                this.xianluWindow1.Name = lineInfo.RouteName;
                this.xianluWindow1.Id = Convert.ToString(lineInfo.RouteId);
                this.xianluWindow1.publishType = 1;
                //备注
                this.txt_remark.Value = lineInfo.Remark;

                #region 订单游客数据
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrderCustomer> curList = lineInfo.Traveller.Travellers;
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                if (curList != null && curList.Count > 0)
                {
                    for (int i = 0; i < curList.Count; i++)
                    {
                        if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.成人)
                        {
                            stringBuilder.AppendFormat("<tr itemtype=\"{0}\">", "adult");
                        }
                        else if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童)
                        {
                            stringBuilder.AppendFormat("<tr itemtype=\"{0}\">", "child");
                        }
                        else
                        {
                            stringBuilder.AppendFormat("<tr itemtype=\"{0}\">", "other");
                        }
                        stringBuilder.AppendFormat("<td style=\"width: 5%\" bgcolor=\"#e3f1fc\" index=\"{0}\" align=\"center\">{0}</td><td height=\"25\" bgcolor=\"#e3f1fc\" align=\"center\">", i + 1);
                        stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusName\" MaxLength=\"50\" valid=\"required\" errmsg=\"请填写姓名!\" name=\"cusName\" value=\"{0}\" /></td>", curList[i].VisitorName);
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                        #region 游客类型
                        if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.成人)
                        {
                            stringBuilder.Append("<select disabled=\"disabled\" title=\"请选择\" id=\"cusType\" name=\"cusType\">");
                            stringBuilder.Append("<option value=\"\">请选择</option>");
                            stringBuilder.Append("<option value=\"1\" selected=\"selected\">成人</option>");
                            stringBuilder.Append("<option value=\"2\">儿童</option>");
                            stringBuilder.Append(" </select>");
                        }
                        //儿童
                        else if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童)
                        {
                            stringBuilder.Append("<select disabled=\"disabled\" title=\"请选择\"  id=\"cusType\" name=\"cusType\">");
                            stringBuilder.Append("<option value=\"\">请选择</option>");
                            stringBuilder.Append("<option value=\"1\" >成人</option>");
                            stringBuilder.Append("<option value=\"2\" selected=\"selected\">儿童</option>");
                            stringBuilder.Append(" </select>");
                        }
                        //其它
                        else
                        {
                            stringBuilder.Append("<select id=\"cusType\" title=\"请选择\"  name=\"cusType\">");
                            stringBuilder.Append("<option value=\"\"  selected=\"selected\">请选择</option>");
                            stringBuilder.Append("<option value=\"1\" >成人</option>");
                            stringBuilder.Append("<option value=\"2\">儿童</option>");
                            stringBuilder.Append(" </select>");
                        }
                        #endregion

                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                        #region 游客证件类型
                        switch (curList[i].CradType)
                        {
                            case EyouSoft.Model.EnumType.TourStructure.CradType.身份证:
                                {
                                    stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                    stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                    stringBuilder.Append("<option value=\"1\" selected=\"selected\">身份证</option>");
                                    stringBuilder.Append("<option value=\"2\">护照</option>");
                                    stringBuilder.Append("<option value=\"3\">军官证</option>");
                                    stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                    stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                    stringBuilder.Append("<option value=\"6\">户口本</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            case EyouSoft.Model.EnumType.TourStructure.CradType.护照:
                                {
                                    stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                    stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                    stringBuilder.Append("<option value=\"1\">身份证</option>");
                                    stringBuilder.Append("<option value=\"2\" selected=\"selected\">护照</option>");
                                    stringBuilder.Append("<option value=\"3\">军官证</option>");
                                    stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                    stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                    stringBuilder.Append("<option value=\"6\">户口本</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            case EyouSoft.Model.EnumType.TourStructure.CradType.军官证:
                                {
                                    stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                    stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                    stringBuilder.Append("<option value=\"1\">身份证</option>");
                                    stringBuilder.Append("<option value=\"2\">护照</option>");
                                    stringBuilder.Append("<option value=\"3\" selected=\"selected\">军官证</option>");
                                    stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                    stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                    stringBuilder.Append("<option value=\"6\">户口本</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            case EyouSoft.Model.EnumType.TourStructure.CradType.台胞证:
                                {
                                    stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                    stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                    stringBuilder.Append("<option value=\"1\">身份证</option>");
                                    stringBuilder.Append("<option value=\"2\">护照</option>");
                                    stringBuilder.Append("<option value=\"3\">军官证</option>");
                                    stringBuilder.Append("<option value=\"4\" selected=\"selected\">台胞证</option>");
                                    stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                    stringBuilder.Append("<option value=\"6\">户口本</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            case EyouSoft.Model.EnumType.TourStructure.CradType.港澳通行证:
                                {
                                    stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                    stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                    stringBuilder.Append("<option value=\"1\">身份证</option>");
                                    stringBuilder.Append("<option value=\"2\">护照</option>");
                                    stringBuilder.Append("<option value=\"3\">军官证</option>");
                                    stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                    stringBuilder.Append("<option value=\"5\" selected=\"selected\">港澳通行证</option>");
                                    stringBuilder.Append("<option value=\"6\">户口本</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            case EyouSoft.Model.EnumType.TourStructure.CradType.户口本:
                                {
                                    stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                    stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                    stringBuilder.Append("<option value=\"1\">身份证</option>");
                                    stringBuilder.Append("<option value=\"2\">护照</option>");
                                    stringBuilder.Append("<option value=\"3\">军官证</option>");
                                    stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                    stringBuilder.Append("<option value=\"5\" >港澳通行证</option>");
                                    stringBuilder.Append("<option value=\"6\" selected=\"selected\">户口本</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            default:
                                {
                                    stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                    stringBuilder.Append("<option value=\"0\" selected=\"selected\">请选择证件</option>");
                                    stringBuilder.Append("<option value=\"1\" >身份证</option>");
                                    stringBuilder.Append("<option value=\"2\">护照</option>");
                                    stringBuilder.Append("<option value=\"3\">军官证</option>");
                                    stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                    stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                        }
                        #endregion

                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                        stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput searchinput02\" id=\"cusCardNo\" MaxLength=\"150\" onblur='getSex(this)' name=\"cusCardNo\" value=\"{0}\">", curList[i].CradNumber);
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                        #region 游客性别
                        switch (curList[i].Sex)
                        {
                            case EyouSoft.Model.EnumType.CompanyStructure.Sex.男:
                                {
                                    stringBuilder.Append("<select class='ddlSex' id=\"cusSex\" name=\"cusSex\">");
                                    stringBuilder.Append("<option value=\"0\">请选择</option>");
                                    stringBuilder.Append("<option value=\"1\" selected=\"selected\">男</option>");
                                    stringBuilder.Append("<option value=\"2\">女</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            case EyouSoft.Model.EnumType.CompanyStructure.Sex.女:
                                {
                                    stringBuilder.Append("<select class='ddlSex' id=\"cusSex\" name=\"cusSex\">");
                                    stringBuilder.Append("<option value=\"0\">请选择</option>");
                                    stringBuilder.Append("<option value=\"1\">男</option>");
                                    stringBuilder.Append("<option value=\"2\" selected=\"selected\">女</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            default:
                                {
                                    stringBuilder.Append("<select class='ddlSex' id=\"cusSex\" name=\"cusSex\">");
                                    stringBuilder.Append("<option value=\"0\" selected=\"selected\">请选择</option>");
                                    stringBuilder.Append("<option value=\"1\">男</option>");
                                    stringBuilder.Append("<option value=\"2\">女</option>");
                                    break;
                                }
                        }

                        #endregion

                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                        stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusPhone\" MaxLength=\"50\" name=\"cusPhone\" value=\"{0}\">", curList[i].ContactTel);
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\"  width=\"6%\">");
                        if (curList[i].SpecialServiceInfo != null)
                        {
                            string str = string.Format("txtItem={0}&txtServiceContent={1}&txtCost={2}&ddlOperate={3}", curList[i].SpecialServiceInfo.ProjectName, curList[i].SpecialServiceInfo.ServiceDetail, curList[i].SpecialServiceInfo.Fee, (curList[i].SpecialServiceInfo.IsAdd ? "0" : "1"));
                            stringBuilder.AppendFormat("<input id=\"spe{0}\" type=\"hidden\" name=\"specive\" value=\"{1}\" />", curList[i].ID, str);
                        }
                        else
                        {
                            string special = "";
                            if (curList[i].SpecialServiceInfo != null)
                            {
                                string isadd = curList[i].SpecialServiceInfo.IsAdd ? "0" : "1";
                                special = "txtItem=" + curList[i].SpecialServiceInfo.ProjectName + "&txtServiceContent=" + curList[i].SpecialServiceInfo.ServiceDetail + "&ddlOperate=" + isadd + "&txtCost=" + Utils.FilterEndOfTheZeroDecimal(curList[i].SpecialServiceInfo.Fee);
                            }
                            stringBuilder.AppendFormat("<input id=\"spe{0}\" type=\"hidden\" name=\"specive\" value=\"{1}\" />", curList[i].ID, special);
                        }
                        stringBuilder.AppendFormat("<a sign=\"speService\" href=\"javascript:void(0)\" onclick=\"OrderEdit.OpenSpecive('spe{0}')\">特服</a></td>", curList[i].ID);
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\" width=\"15%\">");
                        stringBuilder.AppendFormat("<input type=\"hidden\" name=\"cusID\" value=\"{0}\" />", curList[i].ID);
                        stringBuilder.Append("<a sign=\"add\" href=\"javascript:void(0)\" onclick=\"OrderEdit.AddCus()\">添加</a>&nbsp;");
                        stringBuilder.Append("<input type=\"hidden\" name=\"cusState\" value=\"EDIT\" />");
                        string msg = "";
                        if (TourOrderBll.IsDoDelete(curList[i].ID, ref msg))
                        {
                            stringBuilder.Append("<a sign=\"del\" href=\"javascript:void(0)\" onclick=\"OrderEdit.DelCus($(this))\">删除</a></td></tr>");
                        }
                        else
                        {
                            stringBuilder.AppendFormat("<span>{0}</span>", msg);
                        }
                    }
                }
                cusHtml = stringBuilder.ToString();
                #endregion
            }
            CheckBtn(lineInfo);
            return lineInfo;
        }
        #endregion

        #region 取值_返回一个赋值的lineInfo
        protected EyouSoft.Model.TourStructure.LineInquireQuoteInfo SaveInfo(int id)
        {
            EyouSoft.Model.TourStructure.LineInquireQuoteInfo lineInfo = new EyouSoft.Model.TourStructure.LineInquireQuoteInfo();
            //预计出团日期
            lineInfo.LeaveDate = Utils.GetDateTime(Utils.GetFormValue(this.txt_BegTime.UniqueID));
            //总人数
            lineInfo.PeopleNum = Utils.GetInt(Utils.GetFormValue(this.txt_peopleCount.UniqueID), 0);
            //状态
            lineInfo.QuoteState = EyouSoft.Model.EnumType.TourStructure.QuoteState.已处理;
            //备注
            lineInfo.Remark = Utils.GetFormValue(this.txt_remark.UniqueID);
            //客人要求集合
            lineInfo.Requirements = ConProjectControl1.GetDataList();
            //线路编号
            lineInfo.RouteId = Utils.GetInt(xianluWindow1.Id);
            //线路名称
            lineInfo.RouteName = xianluWindow1.Name;
            //价格组成集合
            lineInfo.Services = PriceControl1.GetList;
            //总价
            lineInfo.TotalAmount = PriceControl1.TotalAmount;
            //行程
            lineInfo.XingCheng = new EyouSoft.Model.TourStructure.XingChengMust();
            lineInfo.XingCheng.QuotePlan = Request.Form[this.txtRoutingNeed.UniqueID];
            lineInfo.XingCheng.QuoteId = id;
            //行程_附件
            string filepath = string.Empty;
            string oldfilename = string.Empty;
            bool file_result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["routingFile"], "TourQuotePriceFile", out filepath, out oldfilename);
            if (filepath != "")
            {
                lineInfo.XingCheng.PlanAccessory = filepath;
                lineInfo.XingCheng.PlanAccessoryName = oldfilename;
            }
            else
            {
                if (hid_img_path.Value != "null")
                {
                    lineInfo.XingCheng.PlanAccessory = hid_img_path.Value;
                    lineInfo.XingCheng.PlanAccessoryName = hid_img_name.Value;
                }
                else
                {
                    lineInfo.XingCheng.PlanAccessoryName = "";
                    lineInfo.XingCheng.PlanAccessory = "";
                }
            }
            //机票折扣
            lineInfo.TicketAgio = Utils.GetDecimal(Utils.GetFormValue(this.txt_agio.UniqueID));
            //特殊要求 
            lineInfo.SpecialClaim = Utils.GetFormValue(this.txt_need.UniqueID);
            //编号
            lineInfo.Id = id;
            //公司ID
            lineInfo.CompanyId = SiteUserInfo.CompanyID;
            //联系人姓名
            lineInfo.ContactTel = Utils.GetFormValue(this.txt_contactTel.UniqueID);
            //联系人电话
            lineInfo.ContactName = Utils.GetFormValue(this.txt_contactName.UniqueID);
            //询价单位编号
            lineInfo.CustomerId = Utils.GetInt(this.hid_QuoteCompanyId.Value, 0);
            //询价单位名称
            lineInfo.CustomerName = Utils.GetFormValue(this.txt_quoteCompany.UniqueID);
            ///////////////////////////////////////游客信息

            int cusLength = Utils.GetFormValues("cusName").Length;
            lineInfo.Traveller = new EyouSoft.Model.TourStructure.TourEverydayApplyTravellerInfo();
            lineInfo.Traveller.Travellers = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
            for (int i = 0; i < cusLength; i++)
            {
                //游客id
                string cusID = Utils.GetFormValues("cusID")[i];
                EyouSoft.Model.TourStructure.TourOrderCustomer toc = new EyouSoft.Model.TourStructure.TourOrderCustomer();
                toc.VisitorName = Utils.GetFormValues("cusName")[i];
                toc.VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)(Utils.GetInt(Utils.GetFormValues("cusType")[i]));
                toc.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)(Utils.GetInt(Utils.GetFormValues("cusCardType")[i]));
                toc.CradNumber = Utils.GetFormValues("cusCardNo")[i];

                //游客性别
                int cusSex = Utils.GetInt(Utils.GetFormValues("cusSex")[i]);

                #region 性别
                if (cusSex == 1)
                {
                    toc.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.男;
                }
                else if (cusSex == 2)
                {
                    toc.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.女;
                }
                else
                {
                    toc.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.未知;
                }
                #endregion

                toc.ContactTel = Utils.GetFormValues("cusPhone")[i];

                toc.IssueTime = DateTime.Now;
                EyouSoft.Model.TourStructure.CustomerSpecialService specModel = new EyouSoft.Model.TourStructure.CustomerSpecialService();
                if (string.IsNullOrEmpty(cusID))
                {
                    toc.ID = System.Guid.NewGuid().ToString();
                }
                else
                {
                    toc.ID = cusID;
                }

                if (Utils.GetFormValues("cusState")[i] == "DEL")
                {
                    toc.IsDelete = true;
                }
                else
                {
                    toc.IsDelete = false;
                }
                //特服
                string specive = Utils.GetFormValues("specive")[i];
                if (!string.IsNullOrEmpty(specive))
                {
                    if (specive.LastIndexOf(',') + 1 == specive.Length)
                    {
                        specive = specive.Substring(0, specive.Length - 1);
                    }
                    specModel.CustormerId = toc.ID;
                    specModel.ProjectName = Server.UrlDecode(Utils.GetFromQueryStringByKey(specive, "txtItem"));
                    specModel.ServiceDetail = Server.UrlDecode(Utils.GetFromQueryStringByKey(specive, "txtServiceContent"));
                    specModel.IsAdd = Utils.GetFromQueryStringByKey(specive, "ddlOperate") == "0" ? true : false;
                    specModel.Fee = Utils.GetDecimal(Utils.GetFromQueryStringByKey(specive,
"txtCost"));
                    specModel.IssueTime = DateTime.Now;
                    //特服项目名不能为空，否则不添加该条特服信息
                    if (!string.IsNullOrEmpty(specModel.ProjectName))
                    {
                        toc.SpecialServiceInfo = specModel;
                    }
                }
                lineInfo.Traveller.Travellers.Add(toc);
            }
            ///
            return lineInfo;
        }

        #endregion


        #region  报价(生成快速发布团队计划数据)
        protected void CreatQuicklyTeam()
        {
            int id = Convert.ToInt32(Utils.GetQueryStringValue("id"));
            EyouSoft.Model.TourStructure.LineInquireQuoteInfo lineInfo = SaveInfo(id);    //生成页面上取值后的lineInfo
            EyouSoft.Model.RouteStructure.RouteInfo route = new EyouSoft.Model.RouteStructure.RouteInfo(); //生成线路信息
            EyouSoft.BLL.RouteStructure.Route routeBll = new EyouSoft.BLL.RouteStructure.Route();          //routeBll类
            EyouSoft.Model.TourStructure.TourTeamInfo tourTeamInfo = new EyouSoft.Model.TourStructure.TourTeamInfo();   //生成团队计划类
            //获取线路
            route = routeBll.GetRouteInfo(lineInfo.RouteId);
            //客户单位编号
            tourTeamInfo.BuyerCId = lineInfo.CustomerId;
            //客户单位名称
            tourTeamInfo.BuyerCName = lineInfo.CustomerName;
            //公司编号
            tourTeamInfo.CompanyId = lineInfo.CompanyId;
            //出团日期
            tourTeamInfo.LDate = (DateTime)lineInfo.LeaveDate;
            //线路编号
            tourTeamInfo.RouteId = lineInfo.RouteId;
            //线路名称
            tourTeamInfo.RouteName = lineInfo.RouteName;
            //客户单位编号
            tourTeamInfo.BuyerCId = lineInfo.CustomerId;
            //客户单位名称
            tourTeamInfo.BuyerCName = lineInfo.CustomerName;
            //公司编号
            tourTeamInfo.CompanyId = lineInfo.CompanyId;
            //发布人编号
            tourTeamInfo.OperatorId = SiteUserInfo.ID;
            //总人数
            tourTeamInfo.PlanPeopleNumber = lineInfo.PeopleNum;
            //团队计划发布类型
            tourTeamInfo.ReleaseType = EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick;

            if (new EyouSoft.BLL.CompanyStructure.CompanySetting().GetTeamNumberOfPeople(SiteUserInfo.CompanyID) == EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople.PartNumber)
            {
                tourTeamInfo.TourTeamUnit = new EyouSoft.Model.TourStructure.MTourTeamUnitInfo()
                {
                    NumberCr = tourTeamInfo.PlanPeopleNumber,
                    NumberEt = 0,
                    NumberQp = 0,
                    UnitAmountCr = 0,
                    UnitAmountEt = 0,
                    UnitAmountQp = 0
                };
            }
            else
            {
                tourTeamInfo.TourTeamUnit = null;
            }


            //生成快速发布信息实体
            tourTeamInfo.TourQuickInfo = new EyouSoft.Model.TourStructure.TourQuickPrivateInfo();
            if (lineInfo.XingCheng != null)
            {
                //快速发布——行程内容
                tourTeamInfo.TourQuickInfo.QuickPlan = lineInfo.XingCheng.QuotePlan;
            }
            //快速发布——备注
            tourTeamInfo.TourQuickInfo.Remark = lineInfo.Remark;
            //快速发布——服务标准
            tourTeamInfo.TourQuickInfo.Service = lineInfo.SpecialClaim;




            //销售员ID
            tourTeamInfo.SellerId = SiteUserInfo.ID;
            //我社报价
            tourTeamInfo.TotalAmount = PriceControl1.TotalAmount;
            //团号
            tourTeamInfo.TourCode = this.hid_tourCode.Value;
            //天数
            tourTeamInfo.TourDays = Utils.GetInt(this.hid_dayCount.Value, 1);
            //计调员
            tourTeamInfo.Coordinator = new EyouSoft.Model.TourStructure.TourCoordinatorInfo();
            tourTeamInfo.Coordinator.CoordinatorId = Utils.GetInt(this.hid_peopleId.Value, 0);
            tourTeamInfo.Coordinator.Name = this.hid_peopleName.Value;
            //包含项目集合
            tourTeamInfo.Services = this.PriceControl1.GetList;
            //区域路线编号
            tourTeamInfo.AreaId = Utils.GetInt(this.hid_areaId.Value, 0);
            TheResult(lineQuoteBll.AddQuote(tourTeamInfo, lineInfo), "Submit");




        }
        #endregion

        #region 删除
        protected void Del(string ids)
        {
            bool result = false;
            string[] strs = ids.Split('|');
            int[] ints = new int[strs.Length - 1];
            for (int i = 0; i < strs.Length - 1; i++)
            {
                ints[i] = Utils.GetInt(strs[i]);
            }
            result = lineQuoteBll.DelInquire(SiteUserInfo.CompanyID, ints);
            Response.Clear();
            Response.Write(result);
            Response.End();
        }

        #endregion

        protected void lkBtn_save_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.TourStructure.LineInquireQuoteInfo lineInfo = new EyouSoft.Model.TourStructure.LineInquireQuoteInfo();
            int id = 0;
            id = Convert.ToInt32(Utils.GetQueryStringValue("id"));
            lineInfo = SaveInfo(id);
            TheResult(lineQuoteBll.UpdateQuote(lineInfo), "save");
        }

        protected void lkBtn_quote_Click(object sender, EventArgs e)
        {
            CreatQuicklyTeam();
        }


        //设定该页面为弹窗页面
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        //判断是否有图片来控制“查看”连接是否呈现
        protected bool GetImg(string img)
        {
            if (img != "" && img != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 绑定区域数据
        protected void BindArea()
        {
            IList<EyouSoft.Model.CompanyStructure.Area> list = new List<EyouSoft.Model.CompanyStructure.Area>();
            list = areaBll.GetAreaByCompanyId(SiteUserInfo.CompanyID);
            this.dlArea.DataSource = list;
            this.dlArea.DataValueField = "Id";
            this.dlArea.DataTextField = "AreaName";
            this.dlArea.DataBind();
            ListItem item = new ListItem();
            item.Value = "-1";
            item.Text = "--请选择--";
            this.dlArea.Items.Insert(0, item);
        }
        #endregion

        protected void lkbCreateTour_Click(object sender, EventArgs e)
        {
            CreatQuicklyTeam();
        }
        //操作结果
        protected void TheResult(bool result, string result_type)
        {
            if (result == true)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/TeamPlan/TourAskPriceList.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", result_type == "save" ? "保存成功" : "报价成功", Utils.GetQueryStringValue("iframeId")));
            }
            else
            {
                MessageBox.ResponseScript(this, result_type == "save" ? ";alert('保存失败!');" : "alert('报价失败!');");
            }
        }
        //根据状态屏蔽按钮
        protected void CheckBtn(EyouSoft.Model.TourStructure.LineInquireQuoteInfo lineInfo)
        {
            if (lineInfo.QuoteState == EyouSoft.Model.EnumType.TourStructure.QuoteState.已成功)
            {
                this.lkBtn_save.Visible = false;
                this.submit.Visible = false;
            }
            else
            {
                this.lkBtn_save.Visible = true;
                this.submit.Visible = true;

            }
        }
    }
}
