using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;

namespace Web.Webmaster
{
    /// <summary>
    /// add system page
    /// </summary>
    /// Author:汪奇志 2011-04-15
    public partial class systemadd : WebmasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterPrintDocumentScripts();
            InitPermissions();
            InitBBLSysSetting();
        }

        #region private members
        /// <summary>
        /// register print document scripts
        /// </summary>
        private void RegisterPrintDocumentScripts()
        {
            var objValues = Enum.GetValues(typeof(EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType));
            IList<ListItem> items = new List<ListItem>();

            if (objValues != null && objValues.Length > 0)
            {
                foreach (object objValue in objValues)
                {
                    items.Add(new ListItem(Enum.GetName((typeof(EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType)), objValue), ((int)objValue).ToString()));
                }
            }

            if (items != null && items.Count > 0)
            {
                var obj = items.Select(item => new { Value = item.Value, Text = item.Text });

                string scripts = string.Format("var printDocumentTypes={0};", Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), scripts, true);
            }
            else
            {
                string scripts = string.Format("var printDocumentTypes={0};", "[]");
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), scripts, true);
            }
        }

        /// <summary>
        /// 初始化栏目、模块、权限信息
        /// </summary>
        private void InitPermissions()
        {
            System.Text.StringBuilder html = new System.Text.StringBuilder();
            EyouSoft.BLL.SysStructure.Permission bll = new EyouSoft.BLL.SysStructure.Permission();
            IList<EyouSoft.Model.SysStructure.PermissionCategory> items = bll.GetSysPermissions();
            bll = null;

            if (items != null && items.Count > 0)
            {
                foreach (var big in items)
                {
                    html.AppendFormat("<div class=\"pBig\"><input type=\"checkbox\" id=\"chk_p_big_{1}\" value=\"{1}\" name=\"chk_p_big\" /><label for=\"chk_p_big_{1}\">{0}</label><span class=\"pno\">[{1}]</span></div>", big.CategoryName
                        , big.Id);

                    if (big.PermissionClass == null || big.PermissionClass.Count < 1) continue;

                    html.Append("<div>");

                    int i = 0;
                    foreach (var small in big.PermissionClass)
                    {
                        html.Append("<ul class=\"pSmall\">");
                        html.AppendFormat("<li class=\"pSmallTitle\"><input type=\"checkbox\" id=\"chk_p_small_{1}\" value=\"{1}\" name=\"chk_p_small\" /><label for=\"chk_p_small_{1}\">{0}</label><span class=\"pno\">[{1}]</span></li>", small.ClassName
                            , small.Id);

                        if (small.Permission != null && small.Permission.Count > 0)
                        {
                            foreach (var permission in small.Permission)
                            {
                                html.AppendFormat("<li class=\"pThird\"><input type=\"checkbox\" id=\"chk_p_third_{1}\" value=\"{1}\" name=\"chk_p_third\"  /><label for=\"chk_p_third_{1}\">{0}</label><span class=\"pno\">[{1}]</span></li>", permission.PermissionName
                                    , permission.Id);
                            }
                        }

                        html.Append("</ul>");

                        if (i % 4 == 3)
                        {
                            html.Append("<ul class=\"pSmallSpace\"><li></li></ul>");
                        }
                        i++;
                    }

                    html.Append("<ul class=\"pSmallSpace\"><li></li></ul>");
                    html.Append("</div>");
                }
            }

            this.ltrPermissions.Text = html.ToString();
        }

        /// <summary>
        /// 获取域名信息集合
        /// </summary>
        /// <returns></returns>
        private IList<EyouSoft.Model.SysStructure.SystemDomain> GetSysDomains()
        {
            IList<EyouSoft.Model.SysStructure.SystemDomain> items = new List<EyouSoft.Model.SysStructure.SystemDomain>();

            string[] domains = EyouSoft.Common.Utils.GetFormValues("txtDomain");
            string[] domainPaths = EyouSoft.Common.Utils.GetFormValues("txtDomainPath");
            string[] domainTypes = EyouSoft.Common.Utils.GetFormValues("txtDomainType");
            if (domains != null && domains.Length > 0 && domainPaths != null && domainPaths.Length > 0 && domains.Length == domainPaths.Length)
            {
                for (int i = 0; i < domains.Length; i++)
                {
                    if (string.IsNullOrEmpty(domains[i])) continue;
                    if (string.IsNullOrEmpty(domains[i].Replace("http://", ""))) continue;
                    items.Add(new EyouSoft.Model.SysStructure.SystemDomain()
                    {
                        Domain = domains[i].Replace("http://", ""),
                        Url = domainPaths[i],
                        DomainType = EyouSoft.Common.Utils.GetEnumValue<EyouSoft.Model.EnumType.SysStructure.DomainType>(domainTypes[i], EyouSoft.Model.EnumType.SysStructure.DomainType.专线入口)
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// get big(first) permissions
        /// </summary>
        /// <returns></returns>
        private int[] GetBigPermissions()
        {
            string[] formBigs = EyouSoft.Common.Utils.GetFormValues("chk_p_big");
            if (formBigs == null && formBigs.Length < 1) return null;

            int[] bigs = new int[formBigs.Length];

            for (int i = 0; i < formBigs.Length; i++)
            {
                bigs[i] = EyouSoft.Common.Utils.GetInt(formBigs[i]);
            }

            return bigs;
        }

        /// <summary>
        /// get big(second) permissions
        /// </summary>
        /// <returns></returns>
        private int[] GetSmallPermissions()
        {
            string[] formSmalls = EyouSoft.Common.Utils.GetFormValues("chk_p_small");
            if (formSmalls == null && formSmalls.Length < 1) return null;

            int[] smalls = new int[formSmalls.Length];

            for (int i = 0; i < formSmalls.Length; i++)
            {
                smalls[i] = EyouSoft.Common.Utils.GetInt(formSmalls[i]);
            }

            return smalls;
        }

        /// <summary>
        /// get third permissions
        /// </summary>
        /// <returns></returns>
        private int[] GetThirdPermissions()
        {
            string[] formThirds = EyouSoft.Common.Utils.GetFormValues("chk_p_third");
            if (formThirds == null && formThirds.Length < 1) return null;

            int[] thirds = new int[formThirds.Length];

            for (int i = 0; i < formThirds.Length; i++)
            {
                thirds[i] = EyouSoft.Common.Utils.GetInt(formThirds[i]);
            }

            return thirds;
        }

        /// <summary>
        /// get print document settings
        /// </summary>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.PrintDocument> GetPrintDocumentSettings()
        {
            IList<EyouSoft.Model.CompanyStructure.PrintDocument> items = new List<EyouSoft.Model.CompanyStructure.PrintDocument>();

            string[] keys = EyouSoft.Common.Utils.GetFormValues("txtPrintDocumentK");
            string[] values = EyouSoft.Common.Utils.GetFormValues("txtPrintDocumentV");

            if (keys != null && keys.Length > 0 && values != null && values.Length > 0 && keys.Length == values.Length)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (EyouSoft.Common.Utils.GetInt(keys[i]) == 0) continue;
                    items.Add(new EyouSoft.Model.CompanyStructure.PrintDocument()
                    {
                        PrintTemplate = values[i],
                        PrintTemplateType = (EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType)EyouSoft.Common.Utils.GetInt(keys[i])
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// get company setting
        /// </summary>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.CompanyFieldSetting GetSetting()
        {
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting setting = new EyouSoft.Model.CompanyStructure.CompanyFieldSetting();

            setting.AgencyFeeInfo = (EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType)EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("radAgencyFee"));
            setting.BackTourReminderDays = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtLeaveTourReminderDays"));
            setting.CompanyId = 0;
            setting.CompanyLogo = string.Empty;
            setting.CompanyPrintFile = new EyouSoft.Model.CompanyStructure.CompanyPrintTemplate();
            setting.ComputeOrderType = (EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType)EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("radComputeOrderType"));
            setting.ContractReminderDays = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtContractReminderDays"));
            setting.DisplayAfterMonth = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtDisplayAfterMonth"));
            setting.DisplayBeforeMonth = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtDisplayBeforeMonth"));
            setting.LeaveTourReminderDays = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtLeaveTourReminderDays"));
            setting.PriceComponent = (EyouSoft.Model.EnumType.CompanyStructure.PriceComponent)EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("radPriceComponent"));
            setting.PrintDocument = this.GetPrintDocumentSettings();
            setting.ProfitStatTourPagePath = EyouSoft.Common.Utils.GetFormValue("txtProfitStatTourPagePath");
            setting.ReservationTime = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtReservationTime"));
            setting.TicketTravellerCheckedType = (EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType)EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("radTicketTravellerCheckedType"));
            setting.IsRequiredTraveller = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("radIsRequiredTraveller")) == 1 ? true : false;
            setting.TeamNumberOfPeople = (EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople)EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("radTeamNumberOfPeople"));
            setting.TicketOfficeFillTime = (EyouSoft.Model.EnumType.CompanyStructure.TicketOfficeFillTime)EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("radTicketOfficeFillTime"));
            setting.IsTicketOutRegisterPayment = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("radTicketOutClear")) == 1 ? true : false;
            setting.HuiKuanLvSFBHWeiShenHe = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("radHuiKuanLvSFBHWeiShenHe")) == 1;
            setting.SiteTourDisplayType = EyouSoft.Common.Utils.GetEnumValue<EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType>(EyouSoft.Common.Utils.GetFormValue("radSiteTourDisplayType"), EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团);
            setting.SiteTemplate = EyouSoft.Common.Utils.GetEnumValue<EyouSoft.Model.EnumType.SysStructure.SiteTemplate>(EyouSoft.Common.Utils.GetFormValue("txtSiteTemplate"), EyouSoft.Model.EnumType.SysStructure.SiteTemplate.None);

            return setting;
        }

        /// <summary>
        /// validate form
        /// </summary>
        /// <param name="sysInfo">EyouSoft.Model.SysStructure.MSystemInfo</param>
        /// <param name="message">error message</param>
        /// <returns></returns>
        private bool ValidateForm(EyouSoft.Model.SysStructure.MSysInfo sysInfo, out string message)
        {
            message = string.Empty;
            bool b = true;

            if (string.IsNullOrEmpty(sysInfo.SystemName))
            {
                b = false;
                message = "系统名称不能为空！";
                return b;
            }

            if (string.IsNullOrEmpty(sysInfo.AdminInfo.UserName))
            {
                b = false;
                message = "管理员账号不能为空！";
                return b;
            }

            if (string.IsNullOrEmpty(sysInfo.AdminInfo.PassWordInfo.NoEncryptPassword))
            {
                b = false;
                message = "管理员密码不能为空！";
                return b;
            }

            if (string.IsNullOrEmpty(sysInfo.CompanyInfo.CompanyName))
            {
                b = false;
                message = "公司名称不能为空！";
                return b;
            }

            if (string.IsNullOrEmpty(sysInfo.DepartmentInfo.DepartName))
            {
                b = false;
                message = "总部部门名称不能为空！";
                return b;
            }

            if (sysInfo.Domains == null && sysInfo.Domains.Count < 1)
            {
                b = false;
                message = "至少有一个系统域名！";
                return b;
            }

            #region 打印单据配置验证
            if (sysInfo.Setting != null && sysInfo.Setting.PrintDocument != null && sysInfo.Setting.PrintDocument.Count > 0)//打印单据配置验证
            {
                IDictionary<EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType, int> dic = new Dictionary<EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType, int>();

                foreach (var item in sysInfo.Setting.PrintDocument)
                {
                    if (dic.ContainsKey(item.PrintTemplateType))
                    {
                        dic[item.PrintTemplateType]++;
                    }
                    else
                    {
                        dic.Add(item.PrintTemplateType, 1);
                    }
                }

                bool isRepeat = false;
                EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType? repeatType = null;
                foreach (var item in dic)
                {
                    if (item.Value > 1)
                    {
                        repeatType = item.Key;
                        isRepeat = true;
                        break;
                    }
                }

                if (isRepeat)
                {
                    b = false;
                    message = "打印单据配置有重复：" + repeatType.Value.ToString();
                    return b;
                }
            }
            #endregion

            #region 验证域名是否重复
            IList<string> domains = new List<string>();
            foreach (var domain in sysInfo.Domains)
            {
                domains.Add(domain.Domain);
            }

            EyouSoft.BLL.SysStructure.SystemDomain sysdomainbll = new EyouSoft.BLL.SysStructure.SystemDomain();
            IList<string> existsDomains = sysdomainbll.IsExistsDomains(domains, null);
            sysdomainbll = null;

            if (existsDomains != null && existsDomains.Count > 0)
            {
                b = false;
                message = string.Format("域名：{0}已经存在！", existsDomains[0]);
                return b;
            }
            #endregion


            return b;
        }

        /// <summary>
        /// register alert script
        /// </summary>
        /// <param name="s">msg</param>
        private void RegisterAlertScript(string s)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), string.Format("alert('{0}');", s), true);
        }

        /// <summary>
        /// register alert and Redirect script
        /// </summary>
        /// <param name="s"></param>
        /// <param name="url"></param>
        private void RegisterAlertAndRedirectScript(string s, string url)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), string.Format("alert('{0}');window.location.href='{1}';", s, url), true);
        }

        /// <summary>
        /// 初始化芭比来系统系统配置
        /// </summary>
        private void InitBBLSysSetting()
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting settingbll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting setting = settingbll.GetSetting(1);            
            settingbll = null;
            //var setting1 = (EyouSoft.Model.CompanyStructure.CompanyFieldSetting)setting.Clone();

            string script = string.Empty;
            if (setting != null)
            {
                /*if (setting.PrintDocument != null && setting.PrintDocument.Count > 0)
                {
                    foreach (var item in setting.PrintDocument)
                    {
                        if (string.IsNullOrEmpty(item.PrintTemplate)) continue;
                        item.PrintTemplate = item.PrintTemplate.ToLower().Replace("/print/bbl/", "/print/normal/");
                    }
                }*/

                script = "var sysSetting={0};";
                script = string.Format(script, Newtonsoft.Json.JsonConvert.SerializeObject(setting));
                this.RegisterScript(script);
            }
            else
            {
                this.RegisterScript("var sysSetting=null;");
            }
        }

        /// <summary>
        /// register scripts
        /// </summary>
        /// <param name="script"></param>
        private void RegisterScript(string script)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
        #endregion

        #region btncreate click event
        /// <summary>
        /// btncreate click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.SysStructure.MSysInfo sysInfo = new EyouSoft.Model.SysStructure.MSysInfo();

            #region get form values
            sysInfo.AdminInfo = new EyouSoft.Model.CompanyStructure.CompanyUser();
            sysInfo.CompanyInfo = new EyouSoft.Model.CompanyStructure.CompanyInfo();
            sysInfo.CreateTime = DateTime.Now;
            sysInfo.DepartmentInfo = new EyouSoft.Model.CompanyStructure.Department();
            sysInfo.Domains = null;
            sysInfo.ModuleIds = null;
            sysInfo.PartIds = null;
            sysInfo.PermissionIds = null;
            sysInfo.Setting = new EyouSoft.Model.CompanyStructure.CompanyFieldSetting();

            sysInfo.SystemName = EyouSoft.Common.Utils.GetFormValue("txtSysName");

            sysInfo.AdminInfo.IsAdmin = true;
            sysInfo.AdminInfo.IsDeleted = false;
            sysInfo.AdminInfo.IsEnable = true;
            sysInfo.AdminInfo.UserName = EyouSoft.Common.Utils.GetFormValue("txtUsername");
            sysInfo.AdminInfo.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord()
            {
                NoEncryptPassword = EyouSoft.Common.Utils.GetFormValue("txtPassword")
            };

            sysInfo.CompanyInfo.CompanyName = EyouSoft.Common.Utils.GetFormValue("txtCompanyName");
            sysInfo.CompanyInfo.ContactName = EyouSoft.Common.Utils.GetFormValue("txtRealname");
            sysInfo.CompanyInfo.ContactTel = EyouSoft.Common.Utils.GetFormValue("txtTelephone");
            sysInfo.CompanyInfo.ContactFax = EyouSoft.Common.Utils.GetFormValue("txtFax");
            sysInfo.CompanyInfo.ContactMobile = EyouSoft.Common.Utils.GetFormValue("txtMobile");

            sysInfo.DepartmentInfo.DepartName = EyouSoft.Common.Utils.GetFormValue("txtDepartmentName");

            sysInfo.Domains = this.GetSysDomains();
            sysInfo.ModuleIds = this.GetBigPermissions();
            sysInfo.PartIds = this.GetSmallPermissions();
            sysInfo.PermissionIds = this.GetThirdPermissions();
            sysInfo.Setting = this.GetSetting();
            #endregion

            #region validate form
            string message = string.Empty;
            bool verifyResult = this.ValidateForm(sysInfo, out message);

            if (!verifyResult)
            {
                this.RegisterAlertScript(message);
                return;
            }
            #endregion

            EyouSoft.BLL.SysStructure.BSys bll = new EyouSoft.BLL.SysStructure.BSys();
            int addResult = bll.CreateSys(sysInfo);

            if (addResult == 1)
            {
                message = "子系统添加成功！";
                this.RegisterAlertAndRedirectScript(message, "systems.aspx");
            }
            else
            {
                message = "子系统添加失败！";
                this.RegisterAlertAndRedirectScript(message, "systemadd.aspx");
            }
        }
        #endregion
    }
}
