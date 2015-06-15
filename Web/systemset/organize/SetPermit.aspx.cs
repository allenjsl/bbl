using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.organize
{   
    /// <summary>
    /// 设置员工权限
    /// xuty 2011/1/24
    /// </summary>
    public partial class SetPermit : Eyousoft.Common.Page.BackPage
    {
        protected int roleId;
        protected int empId;
        EyouSoft.BLL.CompanyStructure.SysRoleManage roleBll;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_组织机构_部门人员栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_组织机构_部门人员栏目, false);
                return;
            }
            this.cu_perList.SysId = SiteUserInfo.SysId;
            string method = Utils.GetQueryStringValue("method");//获取当期操作
            empId = Utils.GetInt(Utils.GetQueryStringValue("empId"));//获取要设置的员工Id
            roleBll = new EyouSoft.BLL.CompanyStructure.SysRoleManage();
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);//初始化bll
            if (method == "setPermit")
            {         
                #region 设置权限
                //设置权限
                roleId = Utils.GetInt(Utils.GetFormValue("roleId"));
                string[] perIds = Utils.GetFormValue("perIds").Split(',');//获取选中的权限
                if (userBll.SetPermission(empId, roleId, perIds))
                {
                    Utils.ResponseMegSuccess();
                }
                else
                {
                    Utils.ResponseMegError();
                }
                return;
                #endregion 
            }
            else
            {
               #region 初始化数据或切换角色
               int recordCount=0;
                //绑定角色下拉框
               IList<EyouSoft.Model.CompanyStructure.SysRoleManage> roleList= roleBll.GetList(100000, 1, ref recordCount, CurrentUserCompanyID);
               if (roleList != null)
               {
                   selRole.DataTextField = "RoleName";
                   selRole.DataValueField = "Id";
                   selRole.DataSource = roleList;
                   selRole.DataBind();
                   selRole.Attributes.Add("onchange", "SepPermit.changeRole(this);");
               }
                if (method == "getPermit")//切换角色
                {
                    roleId = Utils.GetInt(Utils.GetQueryStringValue("roleId"));//获取角色
                    if (roleId != 0)
                    {
                        //获取角色拥有的权限
                        BindPermit(roleId);
                    }
                }
                else if (method == "")//初始化数据
                {
                    EyouSoft.Model.CompanyStructure.CompanyUser userModel = userBll.GetUserInfo(empId);
                    selRole.Value = userModel.RoleID.ToString();
                    if (userModel != null)
                    {
                        if (!string.IsNullOrEmpty(userModel.PermissionList))
                        {
                            string[] permits = userModel.PermissionList.Split(',');
                            cu_perList.SetPermitList = permits;
                        }
                        else
                        {
                            if (userModel.RoleID == 0)
                            {
                                BindPermit(roleList != null && roleList.Count > 0 ? roleList[0].Id : 0);
                            }
                        }
                    }
                    else
                    {
                        BindPermit(roleList != null && roleList.Count > 0 ? roleList[0].Id : 0);
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// 绑定角色的权限
        /// </summary>
        /// <param name="roleIdp"></param>
        protected void BindPermit(int roleIdp)
        {
            EyouSoft.Model.CompanyStructure.SysRoleManage roleModel = roleBll.GetModel(CurrentUserCompanyID, roleIdp);
            EyouSoft.BLL.SysStructure.Permission permitBll = new EyouSoft.BLL.SysStructure.Permission();
            string[] permits = roleModel.RoleChilds.Split(',');
            cu_perList.SetPermitList = permits;
            selRole.Value = roleId.ToString();
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
