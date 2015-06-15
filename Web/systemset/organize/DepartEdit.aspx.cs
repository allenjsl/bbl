using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using EyouSoft.Common;
using System.Text;
using EyouSoft.Common.Function;

namespace Web.systemset.organize
{   
    /// <summary>
    /// 部门编辑
    /// xuty 2011/1/14
    /// </summary>
    public partial class DepartEdit : Eyousoft.Common.Page.BackPage
    {
        protected string pageHeader=string.Empty;//页眉
        protected string pageFooter = string.Empty;//页脚
        protected string pageModel = string.Empty;//模板
        protected string departSeal = string.Empty;//部门公章
        protected string parentD = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_组织机构_部门设置栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_组织机构_部门设置栏目, false);
                return;
            }
            int departId = Utils.GetInt(Utils.GetQueryStringValue("departId"));//报价Id
            string method = Utils.GetFormValue("hidMethod");//获取当前操作(保存/继续)
            string method2 = Utils.GetQueryStringValue("method2");//判断是否为新增或修改
            string showMess = "数据保存成功！";//提示消息
            EyouSoft.Model.CompanyStructure.Department departModel = null;
            EyouSoft.BLL.CompanyStructure.Department departBll = new EyouSoft.BLL.CompanyStructure.Department();//初始化bll
            EyouSoft.BLL.CompanyStructure.CompanyUser userBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);//初始化bll
            IList< EyouSoft.Model.CompanyStructure.Department> departList= departBll.GetAllDept(CurrentUserCompanyID);
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> userlist = userBll.GetCompanyUser(CurrentUserCompanyID);
            //绑定部门列表
            if (departList != null && departList.Count > 0)
            {
                selParentDE.DataTextField = "DepartName";
                selParentDE.DataValueField = "Id";
                selParentDE.DataSource = departList;
                selParentDE.DataBind();
                
            }
            //绑定员工列表
            if (userlist != null && userlist.Count > 0)
            {
                foreach (EyouSoft.Model.CompanyStructure.CompanyUser user in userlist)
                {
                    selDepEmp.Items.Add(new ListItem(user.PersonInfo.ContactName,user.ID.ToString()));
                }
            }
            selParentDE.Items.Insert(0, new ListItem("请选择", ""));//上级部门
            selDepEmp.Items.Insert(0, new ListItem("请选择", ""));//部门主管
            //无操作方式则为获取数据
            if (method == "")
            {             
                #region 初始化数据
             
                if (departId != 0)
                {
                    departModel = departBll.GetModel(departId);
                    if (method2 == "update")//修改
                    {
                        if (departModel != null)
                        { 
                            txtDepName.Value = departModel.DepartName;//部门名称
                            selDepEmp.Value = departModel.DepartManger.ToString();//部门主管
                            selParentDE.Value = departModel.PrevDepartId.ToString();// 上级部门
                            parentD = departModel.PrevDepartId.ToString();
                            txtTel.Value = departModel.ContactTel;//联系电话
                            txtRemark.Value = departModel.Remark;//备注
                            txtFax.Value = departModel.ContactFax;//传真
                            if (!string.IsNullOrEmpty(departModel.PageHeadFile))
                            {
                                hidHeader.Value = departModel.PageHeadFile;
                            }
                            if (!string.IsNullOrEmpty(departModel.PageFootFile))
                            {
                                hidFooter.Value = departModel.PageFootFile;
                            }
                            if (!string.IsNullOrEmpty(departModel.TemplateFile))
                            {
                                hidModel.Value = departModel.TemplateFile;
                            }
                            if (!string.IsNullOrEmpty(departModel.DepartStamp))
                            {
                                hidSeat.Value = departModel.DepartStamp;
                            }
                            pageHeader = !string.IsNullOrEmpty(departModel.PageHeadFile) ? string.Format("<a href='{0}' target='_blank'>查看</a>&nbsp;<a href='javascript:;' onclick=\"return De.del('{1}',this);\"><img src='/images/fujian_x.gif'/></a>", departModel.PageHeadFile, hidHeader.ClientID) : "暂无页眉";
                            pageFooter = !string.IsNullOrEmpty(departModel.PageFootFile) ? string.Format("<a href='{0}' target='_blank'>查看</a>&nbsp;<a href='javascript:;' onclick=\"return De.del('{1}',this);\"><img src='/images/fujian_x.gif'/></a>", departModel.PageFootFile, hidFooter.ClientID) : "暂无页脚";
                            pageModel = !string.IsNullOrEmpty(departModel.TemplateFile) ? string.Format("<a href='{0}' target='_blank'>查看</a>&nbsp;<a href='javascript:;' onclick=\"return De.del('{1}',this);\"><img src='/images/fujian_x.gif'/></a>", departModel.TemplateFile, hidModel.ClientID) : "暂无模板";
                            departSeal = !string.IsNullOrEmpty(departModel.DepartStamp) ? string.Format("<a href='{0}' target='_blank'>查看</a>&nbsp;<a href='javascript:;' onclick=\"return De.del('{1}',this);\"><img src='/images/fujian_x.gif'/></a>", departModel.DepartStamp, hidSeat.ClientID) : "暂无公章";
                            if (parentD == "0")
                            {
                                selParentDE.Attributes.Remove("valid");
                            }
                        }
                    }
                    else
                    {
                        if (departModel != null)
                        {
                            selParentDE.Value = departModel.Id.ToString();//如果是添加操作则将部门ID设置上级部门
                            
                        }
                        
                    }
                    return;
                }
                #endregion
            }
            else
            {
                #region 保存操作
               
                bool result = true;
                departModel = new EyouSoft.Model.CompanyStructure.Department();
                string fileName = string.Empty;
                string oldName=string.Empty;
                HttpPostedFile fHeader = Request.Files["fileHeader"];
                HttpPostedFile fFooter = Request.Files["fileFooter"];
                HttpPostedFile fSeal = Request.Files["fileSeal"];
                HttpPostedFile fModel = Request.Files["fileModel"];
                if (fHeader != null && !string.IsNullOrEmpty(fHeader.FileName))
                {
                    result = UploadFile.FileUpLoad(fHeader, "systemset", out fileName, out oldName);//上传页眉
                    departModel.PageHeadFile = fileName;
                }
                else
                {
                    departModel.PageHeadFile = hidHeader.Value;
                }
                if (result && (fFooter != null && !string.IsNullOrEmpty(fFooter.FileName)))
                {
                    result = UploadFile.FileUpLoad(fFooter, "systemset", out fileName, out oldName);//上传页脚
                    departModel.PageFootFile = fileName;
                }
                else
                {
                    departModel.PageFootFile = hidFooter.Value;
                }
                if (result && (fModel != null && !string.IsNullOrEmpty(fModel.FileName)))
                {
                    result = UploadFile.FileUpLoad(fModel, "systemset", out fileName, out oldName);//上传模板
                    departModel.TemplateFile = fileName;
                }
                else
                {
                    departModel.TemplateFile = hidModel.Value;
                }
                if (result && (fSeal != null && !string.IsNullOrEmpty(fSeal.FileName)))
                {
                    result = UploadFile.FileUpLoad(fSeal, "systemset", out fileName, out oldName);//上传公章
                    departModel.DepartStamp = fileName;
                }
                else
                {
                    departModel.DepartStamp = hidSeat.Value;
                }
                if (result)
                {
                    departModel.CompanyId = CurrentUserCompanyID;//公司ID
                    departModel.ContactFax = Utils.InputText(txtFax.Value);//传真
                    departModel.ContactTel = Utils.InputText(txtTel.Value);//电话
                    departModel.DepartName = Utils.InputText(txtDepName.Value);//部门名称
                    departModel.Remark = Utils.InputText(txtRemark.Value);//备注
                    departModel.IssueTime = DateTime.Now;//添加时间
                    departModel.OperatorId = SiteUserInfo.ID;//操作人
                    departModel.PrevDepartId = Utils.GetInt(Utils.GetFormValue(selParentDE.UniqueID));//上级部门
                    departModel.DepartManger = Utils.GetInt(Utils.GetFormValue(selDepEmp.UniqueID));//部门主管
                    //if (departModel.DepartManger == 0)
                    //{
                    //    MessageBox.Show(this, "请填写完整数据！");
                    //    return;
                    //}
                    if (departId != 0)
                    {
                        if (method2 == "update")
                        {
                            departModel.Id = departId;
                            result = departBll.Update(departModel);//修改部门
                        }
                        else
                        {
                            result = departBll.Add(departModel);//添加部门
                        }
                      
                    }
                    else
                    {
                        result = departBll.Add(departModel);//添加部门
                    }
                }
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                StringBuilder messBuilder=new StringBuilder();
                //如果是修改则回调父窗口的修改方法,否则回调新增方法
                if(method2=="update")
                    messBuilder.AppendFormat(";window.parent.DM.callbackUpdateD('{0}','{1}','{3}');alert('{2}');", departId, Utils.InputText(txtDepName.Value), showMess, departModel.PrevDepartId.ToString()!=Utils.GetFormValue("hidParentDE"));
                else
                    messBuilder.AppendFormat(";window.parent.DM.callbackAddD('{0}');alert('{1}');",departId,showMess);
                //如果是保存继续则刷新页面,否则关闭弹窗
                if (method == "continue")
                {
                    messBuilder.AppendFormat("window.location='/systemset/organize/DepartEdit.aspx?method2=add&departId={0}",departId);
                }
                else
                {   
                   messBuilder.AppendFormat("window.parent.Boxy.getIframeDialog('{0}').hide();", Utils.GetQueryStringValue("iframeId"));
                }
                MessageBox.ResponseScript(this, messBuilder.ToString());
                return;
                #endregion
            }
        }
       protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
