using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Model.TourStructure;

namespace Web.GroupEnd.JourneyMoney
{
    /// <summary>
    /// 行程报价-询价添加
    /// 柴逸宁
    /// 修改：田想兵
    /// 修改日期:20110706
    /// 修改内容：添加游客信息
    /// </summary>
    public partial class SelectMoneyAdd : Eyousoft.Common.Page.FrontPage
    {
        #region 变量
        protected EyouSoft.Model.TourStructure.LineInquireQuoteInfo tsModel = null;
        protected EyouSoft.Model.TourStructure.XingChengMust xcModel = new EyouSoft.Model.TourStructure.XingChengMust();

        protected bool tup;
        protected int tid = 0;
        protected int mlen = 0;
        protected string type = string.Empty;
        protected string customerName = string.Empty;//询价单位=当前单位名称？？编号？？
        protected string routeName = string.Empty;//联系人=当前账号
        protected string vontactTel = string.Empty;//联系电话=当前账号电话
        /// <summary>
        /// 游客信息 
        /// by txb2011.7.16
        /// </summary>
        protected string cusHtml = string.Empty;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            tsModel = new LineInquireQuoteInfo();

            EyouSoft.BLL.TourStructure.LineInquireQuoteInfo tsBLL = new EyouSoft.BLL.TourStructure.LineInquireQuoteInfo();

            if (!IsPostBack)
            {
                loedUser();
                type = Utils.GetQueryStringValue("type");//操作
                tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
                switch (type)
                {
                    case "Update":

                        bind();
                        txtLDate.Text = tsModel.LeaveDate.Value.ToString("yyyy-MM-dd");
                        xianluWindow1.Name = tsModel.RouteName;
                        xianluWindow1.Id = tsModel.RouteId.ToString();
                        if (tsModel.SpecialClaim != "")
                        {
                            txt_xinchen.Text = tsModel.XingCheng.QuotePlan;
                        }
                        break;
                }
                #region 配置
                EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting set = setBll.GetSetting(SiteUserInfo.CompanyID);//配置实体
                hd_IsRequiredTraveller.Value = set.IsRequiredTraveller.ToString();
                #endregion

            }
            //else
            //{
            //    Save();//保存或修改操作
            //}

        }

        private void loedUser()
        {
            customerName = SiteUserInfo.TourCompany.CompanyName;//询价单位=当前单位名称？？编号
            routeName = SiteUserInfo.ContactInfo.ContactName;//联系人姓名
            vontactTel = SiteUserInfo.ContactInfo.ContactTel;//联系人电话
            tup = true;


        }



        #region 保存数据（添加修改公用）
        private void Save()
        {
            EyouSoft.BLL.TourStructure.LineInquireQuoteInfo tsBLL = new EyouSoft.BLL.TourStructure.LineInquireQuoteInfo();
            tsModel = new LineInquireQuoteInfo();//初始化model

            tid = Utils.GetInt(Utils.GetFormValue("tid"));//获取表操作值
            bool res;
            if (tid > 0)
            {
                tsModel = tsBLL.GetQuoteModel(tid, 0, SiteUserInfo.TourCompany.TourCompanyId, true);
                xcModel = tsModel.XingCheng;

            }
            /////////////////////////////////////普通数据

            tsModel.CustomerName = SiteUserInfo.TourCompany.CompanyName;//询价单位=当前单位名称？？编号？？
            tsModel.ContactName = SiteUserInfo.ContactInfo.ContactName;//联系人姓名
            tsModel.CustomerId = SiteUserInfo.TourCompany.TourCompanyId;//询价单位编号
            tsModel.CompanyId = SiteUserInfo.CompanyID;//专线公司编号

            tsModel.ContactTel = Utils.GetFormValue("vontactTel").ToString();//联系人电话

            tsModel.RouteName = xianluWindow1.Name;//线路名称
            tsModel.RouteId = Utils.GetInt(xianluWindow1.Id);//线路ID
            tsModel.LeaveDate = Convert.ToDateTime(this.txtLDate.Text); //预计出团日期
            tsModel.PeopleNum = Utils.GetInt(Utils.GetFormValue("number"));//人数？？总人数？？成人数？？儿童数？？




            /////////////////////////////////////行程要求////////上传验证

            if (string.IsNullOrEmpty(xcModel.PlanAccessory))
            {
                string[] allowExtensions = new string[] { ".jpeg", ".jpg", ".bmp", ".gif", ".pdf", ".xls", ".xlsx", ".doc", ".docx" };
                string msg = string.Empty;
                bool nameForm = EyouSoft.Common.Function.UploadFile.CheckFileType(Request.Files, "workAgree", allowExtensions, null, out msg);
                if (!nameForm)
                {
                    lstMsg.Text = msg;
                }
            }
            ////////////////////////////////////行程要求////////上传文件


            if (Request.Files.Count > 0)//上传
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                bool result = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["workAgree"], "SupplierControlFile", out filepath, out oldfilename);
                if (result)
                {
                    xcModel.PlanAccessoryName = oldfilename;//协议名
                    xcModel.PlanAccessory = filepath;
                }

            }

            /////////////////////////////////////////行程要求////文本输入
            xcModel.QuotePlan = Utils.EditInputText(txt_xinchen.Text);


            tsModel.XingCheng = xcModel; //行程要求=输入框+附件

            tsModel.Requirements = ConProjectControl1.GetDataList(); //客人要求


            tsModel.SpecialClaim = Utils.GetFormValue("quotePlan"); ;//特殊要求说明

            ///////////////////////////////////////游客信息

            int cusLength = Utils.GetFormValues("cusName").Length;
            tsModel.Traveller = new TourEverydayApplyTravellerInfo();
            tsModel.Traveller.Travellers = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
            for (int i = 0; i < cusLength; i++)
            {
                //游客id
                string cusID = Utils.GetFormValues("cusID")[i];
                EyouSoft.Model.TourStructure.TourOrderCustomer toc = new TourOrderCustomer();
                toc.VisitorName = Utils.GetFormValues("cusName")[i];
                toc.VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)(Utils.GetInt(Utils.GetFormValues("cusType")[i]));
                toc.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)(Utils.GetInt(Utils.GetFormValues("cusCardType")[i]));
                toc.CradNumber = Utils.GetFormValues("cusCardNo")[i];

                //游客性别
                int cusSex = Utils.GetInt(Utils.GetFormValues("cusSex")[i]);
                toc.Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)cusSex;
                #region 性别
                //if (cusSex == 1)
                //{
                //    toc.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.男;
                //}
                //else if (cusSex == 2)
                //{
                //    toc.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.女;
                //}
                //else
                //{
                //    toc.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.未知;
                //}
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
                tsModel.Traveller.Travellers.Add(toc);
            }
            ///
            if (tid > 0 && lstMsg.Text == "")
            {
                tsModel.Id = tid;

                res = tsBLL.UpdateInquire(tsModel);
            }
            else
            {
                res = tsBLL.AddInquire(tsModel);
            }


            if (res)
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, ";alert('保存成功!');location.href='SelectMoney.aspx';</script>");
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, ";alert('保存失败!');");
            }

        }
        #endregion

        #region 绑定原有数据
        private void bind()
        {
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            List<EnumObj> selectList = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType));
            EyouSoft.BLL.TourStructure.LineInquireQuoteInfo tsBLL = new EyouSoft.BLL.TourStructure.LineInquireQuoteInfo(SiteUserInfo);
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                tsModel = tsBLL.GetQuoteModel(tid, 0, SiteUserInfo.TourCompany.TourCompanyId, true);
                tup = true;

                if (tsModel != null)
                {
                    if (tsModel.QuoteState != EyouSoft.Model.EnumType.TourStructure.QuoteState.未处理)
                    {
                        tup = false;
                        EyouSoft.Model.TourStructure.LineInquireQuoteInfo infoModel = new LineInquireQuoteInfo();
                        EyouSoft.BLL.TourStructure.LineInquireQuoteInfo lineQuoteBll = new EyouSoft.BLL.TourStructure.LineInquireQuoteInfo(SiteUserInfo);
                        infoModel = lineQuoteBll.GetQuoteModel(tid, SiteUserInfo.CompanyID, 0, false);
                        IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> list = infoModel.Services;
                        //decimal  Money=0;

                        if (infoModel.Services.Count > 0)
                        {

                            this.rptList.DataSource = list;
                            this.rptList.DataBind();
                            //for (int i = 0; i < infoModel.Services.Count; i++)
                            //{
                            //    Money += infoModel.Services[i].SelfPrice;
                            //}

                        }
                        mlen = infoModel.Services.Count;
                        txtAllPrice.Value = infoModel.TotalAmount.ToString("#,##0.00");
                    }

                    if (tsModel.QuoteState == EyouSoft.Model.EnumType.TourStructure.QuoteState.已成功)
                    {
                        this.txtRemarks.Text = tsModel.Remark;
                    }
                    else
                    {
                        this.lblRemarks.Visible = false;
                        this.txtRemarks.Visible = false;
                    }


                    vontactTel = tsModel.ContactTel;
                    ConProjectControl1.SetList = tsModel.Requirements;
                    ConProjectControl1.SetDataList();
                    xcModel = tsModel.XingCheng;



                    #region 订单游客数据
                    System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrderCustomer> curList = tsModel.Traveller.Travellers;
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
                                        stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                        stringBuilder.Append("<option value=\"6\" selected=\"selected\">户口本</option>");
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
                                        stringBuilder.Append("<option value=\"6\">户口本</option>");
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

            }

        }
        #endregion


        #region 客户要求 Model
        private IList<EyouSoft.Model.TourStructure.TourServiceInfo> krMoedel()
        {
            string[] CustomerItemArr = Request.Form.GetValues("ddlGuestList");//服务类别
            string[] CustomerServeDetailArr = Request.Form.GetValues("txtGuestRequest");//具体要求

            IList<EyouSoft.Model.TourStructure.TourServiceInfo> krList = null;//客户要求集合
            if (CustomerItemArr != null)
            {
                for (int i = 0; i < CustomerItemArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(CustomerItemArr[i].Trim()) || !string.IsNullOrEmpty(CustomerServeDetailArr[i].Trim()))
                    {
                        TourSingleServiceInfo cModel = new TourSingleServiceInfo();
                        if (!string.IsNullOrEmpty(CustomerItemArr[i]))
                            cModel.ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)int.Parse(CustomerItemArr[i].Trim());

                        krList.Add(cModel);
                    }
                }
            }
            return krList;
        }
        #endregion
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void linkSave_Click(object sender, EventArgs e)
        {
            Save();
        }


    }
}
