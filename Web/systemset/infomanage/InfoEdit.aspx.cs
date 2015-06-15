using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.infomanage
{  
    /// <summary>
    /// 信息编辑
    /// xuty 2011/01/17
    /// </summary>
    public partial class InfoEdit : Eyousoft.Common.Page.BackPage
    {
        protected string departIds;
        public string innerChecked = string.Empty;//发布对象公司内部
        public string departChecked = string.Empty;//发布对象选中部门
        public string tourChecked = string.Empty;//发布对象组团社
        protected string attchFile = string.Empty;//附件
        protected int infoId;//信息ID
        EyouSoft.BLL.CompanyStructure.News newsBll = new EyouSoft.BLL.CompanyStructure.News();//初始化newsBll
        EyouSoft.Model.CompanyStructure.News newModel = new EyouSoft.Model.CompanyStructure.News();
        protected void Page_Load(object sender, EventArgs e)
        {  
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_信息管理_信息管理栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_信息管理_信息管理栏目, true);
                return;
            }
            infoId = Utils.GetInt(Utils.GetQueryStringValue("infoId"));
            string method = Utils.GetFormValue("hidMethod");
            //如果当前操作为保存或者保存继续添加
            if (method == "save" || method == "continue")
            {
                #region 保存信息
                string[] publichTo=Utils.GetFormValues("chkPublishTo");//发布对象
                if (publichTo != null && publichTo.Length > 0)
                {
                    IList<EyouSoft.Model.CompanyStructure.NewsAccept> acceptList = new List<EyouSoft.Model.CompanyStructure.NewsAccept>();
                    if (publichTo.Contains("cInner"))
                    {
                        EyouSoft.Model.CompanyStructure.NewsAccept acceptModel = new EyouSoft.Model.CompanyStructure.NewsAccept();
                        acceptModel.AcceptType = EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType.所有;
                        acceptList.Add(acceptModel);
                    }
                    else if (publichTo.Contains("sDepart"))
                    {
                        if (!string.IsNullOrEmpty(txtDeparts.Value))
                        {
                            string[] departs = txtDeparts.Value.Split(',');
                            foreach (string dId in departs)
                            {
                                EyouSoft.Model.CompanyStructure.NewsAccept acceptModel = new EyouSoft.Model.CompanyStructure.NewsAccept { AcceptType = EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType.指定部门, AcceptId = Utils.GetInt(dId) };
                                acceptList.Add(acceptModel);
                            }
                        }
                    }
                    if (publichTo.Contains("cTour"))
                    {
                        EyouSoft.Model.CompanyStructure.NewsAccept acceptModel = new EyouSoft.Model.CompanyStructure.NewsAccept { AcceptType = EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType.指定组团 };
                        acceptList.Add(acceptModel);
                    }
                    newModel.AcceptList = acceptList;

                }
                else
                {
                    MessageBox.Show(this, "请填写完整数据！");
                    return;
                }
                newModel.CompanyId = CurrentUserCompanyID;
               
                newModel.Title=Utils.InputText(txtInfoTitle.Value);//发布标题
                newModel.Content=Utils.EditInputText(txtInfoContent.Value);//内容
                Utils.InputText(txtDeparts.Value);//发布的部门
                newModel.OperatorId = SiteUserInfo.ID;//发布人
                newModel.OperatorName = SiteUserInfo.ContactInfo.ContactName;
                newModel.IssueTime = Utils.GetDateTime(Utils.InputText(txtPublishDate.Value));
                
                HttpPostedFile attachFile1 = Request.Files["fileAttach"];//获取附件
                bool result=false;
                if (attachFile1 != null && !string.IsNullOrEmpty(attachFile1.FileName))
                {
                    string filePath = string.Empty;
                    string fileName = string.Empty;
                    result = UploadFile.FileUpLoad(attachFile1, "systemset", out filePath, out fileName);
                    if (result) newModel.UploadFiles = filePath;
                }
                else
                {
                    result = true;
                }

                if (result)
                {
                    if (infoId != 0)
                    {
                        //修改
                        newModel.ID = infoId;
                        result=newsBll.Update(newModel);
                    }
                    else
                    {
                        //新增
                        result = newsBll.Add(newModel);
                    }
                }
                if (result)
                {
                    //保存成功跳转列表或重新发布信息
                    MessageBox.ShowAndRedirect(this, "信息保存成功", method == "save" ? "/systemset/infomanage/InfoList.aspx" : "/systemset/infomanage/InfoEdit.aspx");
                }
                else
                {
                    MessageBox.Show(this, "信息保存失败！");
                }
                #endregion
            }
            else
            {

                if (infoId != 0)
                {
                    #region 初始化数据
                    newModel = newsBll.GetModel(infoId);
                    if (newModel != null)
                    {
                        //初始化数据
                        IList<EyouSoft.Model.CompanyStructure.NewsAccept> acceptList = newModel.AcceptList;
                        if (acceptList != null && acceptList.Count > 0)
                        {
                            if (acceptList.Where(i => i.AcceptType == EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType.所有).Count() > 0)
                            {
                                innerChecked = "checked=\"checked\"";
                            }
                            IEnumerable<EyouSoft.Model.CompanyStructure.NewsAccept> acceptListDepart = acceptList.Where(i => i.AcceptType == EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType.指定部门);
                            if (acceptListDepart.Count() > 0)
                            {
                                StringBuilder strBuilder = new StringBuilder();
                                foreach (var i in acceptListDepart)
                                {
                                    strBuilder.AppendFormat("{0},", i.AcceptId);
                                }
                                txtDeparts.Value = strBuilder.ToString().TrimEnd(',');
                                departChecked = "checked=\"checked\"";

                            }
                            if (acceptList.Where(i => i.AcceptType == EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType.指定组团).Count() > 0)
                            {
                                tourChecked = "checked=\"checked\"";
                            }
                        }
                        txtAuthor.Value = newModel.OperatorName;
                        txtInfoTitle.Value = newModel.Title;
                        txtPublishDate.Value = newModel.IssueTime.ToString("yyyy-MM-dd HH:mm:ss");
                        attchFile = newModel.UploadFiles;
                        txtInfoContent.Value = newModel.Content;
                    }
                    #endregion
                }
                else
                {
                    txtAuthor.Value = SiteUserInfo.ContactInfo.ContactName;
                    txtPublishDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
          
        }
        /// <summary>
        /// 获取附件链接
        /// </summary>
        /// <returns></returns>
        protected string GetAttchHtml()
        {   
            //如果附件不为空则显示链接,否则提示信息
            if (infoId != 0)
            {
                if (!string.IsNullOrEmpty(attchFile))
                {
                    if (attchFile.Trim() != "")
                    {
                        return string.Format("<a href=\"{0}\" target='_blank'>查看附件</a>", attchFile);
                    }
                }
                else
                {
                    return "暂无附件";
                }
            }
            return "";
        }
    }
}
