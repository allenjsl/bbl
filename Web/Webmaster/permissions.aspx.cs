using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Webmaster
{
    public partial class permissions : WebmasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.InitPermissions();
        }

        #region private members
        /// <summary>
        /// register scripts
        /// </summary>
        /// <param name="script"></param>
        private void RegisterScript(string script)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        /// <summary>
        /// register alert script
        /// </summary>
        /// <param name="s">msg</param>
        private void RegisterAlertScript(string s)
        {
            this.RegisterScript(string.Format("alert('{0}');", s));
        }

        /// <summary>
        /// register alert and Redirect script
        /// </summary>
        /// <param name="s"></param>
        /// <param name="url">IsNullOrEmpty=tru page reload</param>
        private void RegisterAlertAndRedirectScript(string s, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href='{1}';", s, url));
            }
            else
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href=window.location.href;", s));
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
                    html.AppendFormat("<div class=\"pBig\"><input type=\"checkbox\" id=\"chk_p_big_{1}\" value=\"{1}\" name=\"chk_p_big\" /><label for=\"chk_p_big_{1}\">{0}</label><span class=\"pno\">[{1}]</div></span>", big.CategoryName
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

            string script = "var permissions={0};";
            script = string.Format(script, Newtonsoft.Json.JsonConvert.SerializeObject(items));
            this.RegisterScript(script);
        }
        #endregion

        #region btn click event
        /// <summary>
        /// btnAddFirst_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddFirst_Click(object sender, EventArgs e)
        {
            string name = EyouSoft.Common.Utils.InputText(this.txtFirst.Value).Trim();
            if (string.IsNullOrEmpty(name))
            {
                this.RegisterAlertAndRedirectScript("名称不能为空！", "");
                return;
            }

            EyouSoft.BLL.SysStructure.Permission bll = new EyouSoft.BLL.SysStructure.Permission();
            int identityId =bll.InsertFirstSysPermission(name);

            if (identityId > 0)
            {
                this.RegisterAlertAndRedirectScript("添加成功！", "");
                return;
            }
            else
            {
                this.RegisterAlertAndRedirectScript("添加失败！", "");
                return;
            }
        }

        /// <summary>
        /// btnAddSecond_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddSecond_Click(object sender, EventArgs e)
        {
            string name = EyouSoft.Common.Utils.InputText(this.txtSecond.Value).Trim();
            int firstId = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtSecondFirst"), 0);
            if (string.IsNullOrEmpty(name))
            {
                this.RegisterAlertAndRedirectScript("名称不能为空！", "");
                return;
            }
            if (firstId < 1)
            {
                this.RegisterAlertAndRedirectScript("请选择第一级栏目！", "");
                return;
            }

            EyouSoft.BLL.SysStructure.Permission bll = new EyouSoft.BLL.SysStructure.Permission();
            int identityId = bll.InsertSecondSysPermission(name, firstId);

            if (identityId > 0)
            {
                this.RegisterAlertAndRedirectScript("添加成功！", "");
                return;
            }
            else
            {
                this.RegisterAlertAndRedirectScript("添加失败！", "");
                return;
            }
        }

        /// <summary>
        /// btnAddThird_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddThird_Click(object sender, EventArgs e)
        {
            string name = EyouSoft.Common.Utils.InputText(this.txtThird.Value).Trim();
            int firstId = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtThirdFirst"), 0);
            int secondId = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("txtThirdSecond"), 0);

            if (string.IsNullOrEmpty(name))
            {
                this.RegisterAlertAndRedirectScript("名称不能为空！", "");
                return;
            }
            if (firstId < 1)
            {
                this.RegisterAlertAndRedirectScript("请选择第一级栏目！", "");
                return;
            }
            if (secondId < 1)
            {
                this.RegisterAlertAndRedirectScript("请选择第二级栏目！", "");
                return;
            }

            EyouSoft.BLL.SysStructure.Permission bll = new EyouSoft.BLL.SysStructure.Permission();
            int identityId = bll.InsertThirdSysPermission(name, firstId,secondId);

            if (identityId > 0)
            {
                this.RegisterAlertAndRedirectScript("添加成功！", "");
                return;
            }
            else
            {
                this.RegisterAlertAndRedirectScript("添加失败！", "");
                return;
            }
        }
        #endregion
    }
}
