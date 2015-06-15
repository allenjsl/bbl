using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Function;

namespace Web.xianlu
{
    public partial class UpdateQuote   :Eyousoft.Common.Page.BackPage
    {
        protected IList<EyouSoft.Model.TourStructure.TourAttachInfo> Attach = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("issave")) == 1)
            {
                this.OnSave();                
            }

            if (!this.Page.IsPostBack)
            {
                InItBindLineType();

                //获取当前操作类型
                string ActionType = Request.QueryString["Action"];
                #region 列表线路数据修改
                if (ActionType == "update")
                {
                    this.Page.Title = "修改线路_快速版_线路产品库_" + SiteUserInfo.CompanyName + "";
                    this.hideType.Value = "update";
                    int RouteID = EyouSoft.Common.Utils.GetInt(Request.QueryString["id"]);
                    if (RouteID > 0)
                    {
                        this.InitQuoteBind(RouteID);
                    }
                    this.hidRouteid.Value = RouteID.ToString();
                }
                #endregion

                #region 导航菜单线路数据修改
                if (ActionType == "UpdateT")
                {
                    this.Page.Title = "修改线路_快速版_线路产品库_" + SiteUserInfo.CompanyName + "";
                    this.hideType.Value = "UpdateT";
                    int RouteID = EyouSoft.Common.Utils.GetInt(Request.QueryString["UpdateID"]);
                    if (RouteID > 0)
                    {
                        this.InitQuoteBind(RouteID);
                    }
                    this.hidRouteid.Value = RouteID.ToString();
                }
                #endregion

                #region 复制线路信息
                if (ActionType == "Copy")
                {
                    this.Page.Title = "复制线路_快速版_线路产品库_" + SiteUserInfo.CompanyName + "";
                    this.hideType.Value = "Copy";
                    int RouteID = EyouSoft.Common.Utils.GetInt(Request.QueryString["CopyID"]);
                    if (RouteID > 0)
                    {
                        this.InitQuoteBind(RouteID);
                    }
                    this.hidRouteid.Value = RouteID.ToString();
                }
                #endregion              
            }
        }

        #region 绑定线路区域
        protected void InItBindLineType()
        {
            //清空下拉框选项
            this.ddlLineType.Items.Clear();
            this.ddlLineType.Items.Add(new ListItem("--请选择线路区域--", ""));
            IList<EyouSoft.Model.CompanyStructure.Area> areaList = new EyouSoft.BLL.CompanyStructure.Area().GetAreaList(SiteUserInfo.ID);
            if (areaList != null && areaList.Count > 0)
            {
                //将数据添加至下拉框
                for (int i = 0; i < areaList.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = areaList[i].Id.ToString();
                    item.Text = areaList[i].AreaName;
                    this.ddlLineType.Items.Add(item);
                }
            }
            //释放资源
            areaList = null;
        }
        #endregion

        #region 获取快速发布线路信息
        protected void InitQuoteBind(int RouteId)
        {
            EyouSoft.BLL.RouteStructure.Route Bll_Route = new EyouSoft.BLL.RouteStructure.Route();
            EyouSoft.Model.RouteStructure.RouteInfo Model_Routeinfo = new EyouSoft.Model.RouteStructure.RouteInfo();
            //根据线路区域编号获取线路信息
            Model_Routeinfo = Bll_Route.GetRouteInfo(RouteId);
            if (Model_Routeinfo != null)
            {
                //线路区域编号
                if (this.ddlLineType.Items.FindByValue(Model_Routeinfo.AreaId.ToString()) != null)
                {
                    this.ddlLineType.Items.FindByValue(Model_Routeinfo.AreaId.ToString()).Selected = true;
                }
                //线路名称
                this.txt_LineName.Value = Model_Routeinfo.RouteName;
                //线路描述
                this.txt_Description.Value = Model_Routeinfo.RouteDepict;
                //旅游天数
                this.txt_Days.Value = Convert.ToString(Model_Routeinfo.RouteDays);
                if (Model_Routeinfo.RouteQuickInfo != null)
                {
                    //行程安排
                    this.txt_Travel.Value = Model_Routeinfo.RouteQuickInfo.QuickPlan;
                    //服务标准
                    this.txt_Services.Text = Model_Routeinfo.RouteQuickInfo.Service;
                    //备注
                    this.txt_Remarks.Value = Model_Routeinfo.RouteQuickInfo.Remark;
                }
                //附件                
                Attach = Model_Routeinfo.Attachs;
            }
            Bll_Route = null;
            Model_Routeinfo = null;
        }
        #endregion

        #region 保存线路信息
        protected void OnSave()
        {
            bool result = true;

            //线路区域编号
            int LineTypeID = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(ddlLineType.UniqueID), ""));
            if (LineTypeID == 0)
            {
                result = false;
                MessageBox.Show(this, "请选择线路区域!");
                return;
            }
            //线路名称
            string LineName = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.txt_LineName.Name), "");
            if (LineName == "")
            {
                result = false;
                MessageBox.Show(this, "请填写线路名称!");
                return;
            }
            //线路描述
            string LineDesCription = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(txt_Description.UniqueID), "");
            //旅游天数
            int txtDays = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(txt_Days.UniqueID), ""));
            if (txtDays == 0)
            {
                result = false;
                MessageBox.Show(this, "请填写旅游天数!");
                return;
            }
            //行程安排
            string Arrange = EyouSoft.Common.Utils.EditInputText(Request.Form[txt_Travel.UniqueID]);
            //服务标准
            string Standard = EyouSoft.Common.Utils.EditInputText(Request.Form[txt_Services.UniqueID]);
            //备注
            string Remarks = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(txt_Remarks.UniqueID), "");            

            //线路库业务逻辑类
            EyouSoft.BLL.RouteStructure.Route Route = new EyouSoft.BLL.RouteStructure.Route();

            //线路信息实体类
            EyouSoft.Model.RouteStructure.RouteInfo Routeinfo = new EyouSoft.Model.RouteStructure.RouteInfo();
            Routeinfo.AreaId = LineTypeID;
            Routeinfo.RouteName = LineName;

            
            //快速发布团队专有信息实体类
            EyouSoft.Model.TourStructure.TourQuickPrivateInfo TourQuickPrivateInfo = new EyouSoft.Model.TourStructure.TourQuickPrivateInfo();
            TourQuickPrivateInfo.QuickPlan = Arrange;
            TourQuickPrivateInfo.Remark = Remarks;
            TourQuickPrivateInfo.Service = Standard;

            Routeinfo.RouteQuickInfo = TourQuickPrivateInfo;
            Routeinfo.RouteDepict = LineDesCription;
            Routeinfo.RouteDays = txtDays;

            //设置线路的发布类型
            Routeinfo.ReleaseType = EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick;

            //公司编号
            Routeinfo.CompanyId = SiteUserInfo.CompanyID;
            //当前登录人编号
            Routeinfo.OperatorId = SiteUserInfo.ID;
            //当前登录人姓名
            if (SiteUserInfo.ContactInfo != null)
                Routeinfo.OperatorName = SiteUserInfo.ContactInfo.ContactName;

            if (EyouSoft.Common.Utils.GetFormValue("filehid") == "dele")
            {
                Routeinfo.Attachs = new List<EyouSoft.Model.TourStructure.TourAttachInfo>();
                //线路附件实体
                EyouSoft.Model.TourStructure.TourAttachInfo TourAttachInfo = new EyouSoft.Model.TourStructure.TourAttachInfo();                
                TourAttachInfo.FilePath = "";
                TourAttachInfo.Name = "";
                Routeinfo.Attachs.Add(TourAttachInfo);
            }
            
            //附件
            if (Request.Files.Count > 0)
            {
                Routeinfo.Attachs = new List<EyouSoft.Model.TourStructure.TourAttachInfo>();
                //线路附件实体
                EyouSoft.Model.TourStructure.TourAttachInfo TourAttachInfo = new EyouSoft.Model.TourStructure.TourAttachInfo();
                //文件路径
                string filePath = string.Empty;
                //文件名
                string fileName = string.Empty;
                //文件上传
                if (EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["fileUpLoad"], "LineListFile", out filePath, out fileName))
                {
                    if (filePath.Trim() != "" && fileName.Trim() != "")
                    {
                        //设置文件上传后的虚拟路劲
                        TourAttachInfo.FilePath = filePath;
                        //保存原文件名
                        TourAttachInfo.Name = fileName;
                        Routeinfo.Attachs.Add(TourAttachInfo);
                    }
                    else
                    {
                        //设置文件上传后的虚拟路劲
                        TourAttachInfo.FilePath = "";
                        //保存原文件名
                        TourAttachInfo.Name = "";
                        Routeinfo.Attachs.Add(TourAttachInfo);
                    }
                }
            }

            string hidType = EyouSoft.Common.Utils.GetFormValue(this.hideType.UniqueID);
            #region 列表线路数据修改
            if (hidType == "update")
            {
                //线路编号
                int RouteId = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue(this.hidRouteid.UniqueID));
                Routeinfo.RouteId = RouteId;
            }
            #endregion

            #region 导航菜单线路数据修改
            if (hidType == "UpdateT")
            {
                //线路编号
                int RouteID = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue(this.hidRouteid.UniqueID));
                Routeinfo.RouteId = RouteID;
            }
            #endregion

            #region 复制线路信息
            if (hidType == "Copy")
            {
                //线路编号
                int RouteID = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue(this.hidRouteid.UniqueID));
                Routeinfo.RouteId = RouteID;
            }
            #endregion

            if (hidType == "Copy")
            {
                if (result)
                {
                    if (Route.InsertRouteInfo(Routeinfo) > 0)
                    {
                        MessageBox.ShowAndRedirect(this, "线路信息添加成功!", "/xianlu/LineProducts.aspx");
                    }
                    else
                    {
                        MessageBox.ShowAndRedirect(this, "线路信息添加失败!", "/xianlu/LineProducts.aspx");
                    }
                }
            }

            if (hidType == "update" || hidType == "UpdateT")
            {
                if (result)
                {
                    if (Route.UpdateRouteInfo(Routeinfo) > 0)
                    {
                        MessageBox.ShowAndRedirect(this, "线路信息修改成功!", "/xianlu/LineProducts.aspx");
                    }
                    else
                    {
                        MessageBox.ShowAndRedirect(this, "线路信息修改失败!", "/xianlu/LineProducts.aspx");
                    }
                }
            }           
            #region 释放资源
            Route = null;
            Routeinfo = null;
            TourQuickPrivateInfo = null;
            #endregion
        }
        #endregion
    }

}
