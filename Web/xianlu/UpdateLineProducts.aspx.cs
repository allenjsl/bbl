using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common;
using EyouSoft.Common.Function;

namespace Web.xianlu
{
    /// <summary>
    /// 模块名称:修改标准线路信息列表
    /// 创建时间:2011-01-12
    /// 创建人:lixh
    /// </summary>
    public partial class UpdateLineProducts : Eyousoft.Common.Page.BackPage
    {
        protected string UploadFile = string.Empty; //上传文件路径
        protected void Page_Load(object sender, EventArgs e)
        {
            if (EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("issave")) == 1)
            {
                this.InitBindLinePro();
            }
            else
            {
                if (!this.Page.IsPostBack)
                {
                    this.InitBindLineType();

                    //获取当前操作类型
                    string ActionType = Request.QueryString["Action"];

                    #region 列表线路数据修改
                    if (ActionType == "update")
                    {
                        this.Page.Title = "修改线路_标准版_线路产品库_" + SiteUserInfo.CompanyName + "";
                        this.hideType.Value = "update";
                        int RouteID = EyouSoft.Common.Utils.GetInt(Request.QueryString["id"]);
                        if (RouteID > 0)
                        {
                            this.InitLineInfo(RouteID);
                        }
                        this.hidRouteid.Value = RouteID.ToString();
                    }
                    #endregion

                    #region 导航菜单线路数据修改
                    if (ActionType == "UpdateT")
                    {
                        this.Page.Title = "修改线路_标准版_线路产品库_" + SiteUserInfo.CompanyName + "";
                        this.hideType.Value = "UpdateT";
                        int RouteID = EyouSoft.Common.Utils.GetInt(Request.QueryString["UpdateID"]);
                        if (RouteID > 0)
                        {
                            this.InitLineInfo(RouteID);
                        }
                        this.hidRouteid.Value = RouteID.ToString();
                    }
                    #endregion

                    #region 复制线路信息
                    if (ActionType == "Copy")
                    {
                        this.Page.Title = "复制线路_标准版_线路产品库_" + SiteUserInfo.CompanyName + "";
                        this.hideType.Value = "Copy";
                        int RouteID = EyouSoft.Common.Utils.GetInt(Request.QueryString["CopyID"]);
                        if (RouteID > 0)
                        {
                            this.InitLineInfo(RouteID);
                        }
                        this.hidRouteid.Value = RouteID.ToString();
                    }
                    #endregion
                }
            }
        }

        #region 初始化线路信息
        protected void InitLineInfo(int RouteId)
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
                this.Txt_XianlDescript.Value = Model_Routeinfo.RouteDepict;
                //旅游天数
                this.Txt_Days.Value = Convert.ToString(Model_Routeinfo.RouteDays);
                //行程信息集合 
                this.xingcheng1.Bind(Model_Routeinfo.RouteNormalInfo.Plans.ToList());

                //附件信息 
                IList<EyouSoft.Model.TourStructure.TourAttachInfo> attachList = Model_Routeinfo.Attachs;
                if (attachList.Count > 0 && attachList != null)
                {
                    EyouSoft.Model.TourStructure.TourAttachInfo fileModel = attachList[0];
                    hypFilePath.NavigateUrl = fileModel.FilePath;
                    this.hidOldName.Value = fileModel.Name;
                    this.hidOldFilePath.Value = fileModel.FilePath;
                    if (fileModel.Name.Trim() == "")
                    {
                        this.pnlFile.Visible = false;
                    }
                }
                else
                {
                    this.pnlFile.Visible = false;
                }

                //包含信息项目
                this.ProjectControl1.SetList = Model_Routeinfo.RouteNormalInfo.Services;
                this.ProjectControl1.SetDataList();
                //不包含项目 
                this.Txt_XianlProjectNo.Value = Model_Routeinfo.RouteNormalInfo.BuHanXiangMu;
                //儿童安排
                this.Txt_ChildPlan.Value = Model_Routeinfo.RouteNormalInfo.ErTongAnPai;
                //购物安排
                this.Txt_ShoppPlan.Value = Model_Routeinfo.RouteNormalInfo.GouWuAnPai;
                //内部信息 
                this.Txt_Reception.Value = Model_Routeinfo.RouteNormalInfo.NeiBuXingXi;
                //温馨提醒
                this.Txt_Remind.Value = Model_Routeinfo.RouteNormalInfo.WenXinTiXing;
                //主要事项                      
                this.Txt_Notes.Value = Model_Routeinfo.RouteNormalInfo.ZhuYiShiXiang;
                //自费项目
                this.Txt_expensce.Value = Model_Routeinfo.RouteNormalInfo.ZiFeiXIangMu;
            }
            Bll_Route = null;
            Model_Routeinfo = null;
        }
        #endregion

        #region 绑定线路区域信息
        protected void InitBindLineType()
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

        #region 保存线路信息
        protected void InitBindLinePro()
        {
            //线路区域编号
            int LineTypeID = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.ddlLineType.UniqueID), ""));
            if (LineTypeID == 0)
            {
                SetErrorMsg(false, "请选择线路区域!");
                return;
            }
            //线路名称
            string LineName = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.txt_LineName.UniqueID.Trim()), "");
            if (LineName == "")
            {
                SetErrorMsg(false, "请填写线路名称!");
                return;
            }
            //线路描述
            string Desriptoin = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.Txt_XianlDescript.UniqueID.Trim()), "");
            //旅游天数
            int Days = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue(this.Txt_Days.UniqueID.Trim()));
            if (Days == 0)
            {
                SetErrorMsg(false, "请填写旅游天数!");
                return;
            }

            //不包含项目
            string ProjectNo = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.Txt_XianlProjectNo.UniqueID.Trim()), "");
            //购物安排
            string ShoppPlan = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.Txt_ShoppPlan.UniqueID.Trim()), "");
            //儿童安排
            string ChildPlan = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.Txt_ChildPlan.UniqueID.Trim()), "");
            //自费项目
            string Expensce = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.Txt_expensce.UniqueID.Trim()), "");
            //注意事项
            string Notes = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.Txt_Notes.UniqueID.Trim()), "");
            //温馨提醒
            string Remind = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.Txt_Remind.UniqueID.Trim()), "");
            //内部信息
            string Reception = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(this.Txt_Reception.UniqueID.Trim()), "");

            //线路库业务逻辑类
            EyouSoft.BLL.RouteStructure.Route Route = new EyouSoft.BLL.RouteStructure.Route();
            //线路基本信息实体类
            EyouSoft.Model.RouteStructure.RouteInfo Routeinfo = new EyouSoft.Model.RouteStructure.RouteInfo();
            Routeinfo.AreaId = LineTypeID;
            Routeinfo.RouteName = LineName;
            Routeinfo.RouteDepict = Desriptoin;
            Routeinfo.RouteDays = Days;
            Routeinfo.Attachs = new List<EyouSoft.Model.TourStructure.TourAttachInfo>();

            //标准发布团队行程信息业务实体  
            Routeinfo.RouteNormalInfo = new EyouSoft.Model.TourStructure.TourNormalPrivateInfo();
            #region 包含项目            
            Routeinfo.RouteNormalInfo.Services = this.ProjectControl1.GetDataList();
            Routeinfo.RouteNormalInfo.WenXinTiXing = Remind;
            Routeinfo.RouteNormalInfo.ZhuYiShiXiang = Notes;
            Routeinfo.RouteNormalInfo.ZiFeiXIangMu = Expensce;
            #endregion

            #region 获取行程信息
            List<EyouSoft.Model.TourStructure.TourPlanInfo> PlanInfo = this.xingcheng1.GetValues();
            Routeinfo.RouteNormalInfo.Plans = PlanInfo;            
            Routeinfo.RouteNormalInfo.BuHanXiangMu = ProjectNo;
            Routeinfo.RouteNormalInfo.ErTongAnPai = ChildPlan;
            Routeinfo.RouteNormalInfo.GouWuAnPai = ShoppPlan;
            Routeinfo.RouteNormalInfo.NeiBuXingXi = Reception;
            #endregion
            
            #region 上传附件
            //线路附件实体
            EyouSoft.Model.TourStructure.TourAttachInfo TourAttachInfo = new EyouSoft.Model.TourStructure.TourAttachInfo();
            //文件路径
            string filePath = string.Empty;
            //文件名
            string fileName = string.Empty;

            HttpPostedFile FileUpload = Request.Files["fileUpLoad"];
            if (!string.IsNullOrEmpty(FileUpload.FileName) && FileUpload.ContentLength > 0)
            {
                //文件上传
                if (EyouSoft.Common.Function.UploadFile.FileUpLoad(FileUpload, "LineListFile", out filePath, out fileName))
                {
                    if (filePath.Trim() != "" && fileName.Trim() != "")
                    {
                        //设置文件上传后的虚拟路劲
                        TourAttachInfo.FilePath = filePath;
                        //保存原文件名
                        TourAttachInfo.Name = fileName;
                        Routeinfo.Attachs.Add(TourAttachInfo);
                    }
                }
                else
                {
                    //上传失败提示
                    this.litMsg.Text = EyouSoft.Common.Utils.ShowMsg("文件上传失败!");
                    return;
                }    
            }
            else
            {
                TourAttachInfo.FilePath = hidOldFilePath.Value;
                TourAttachInfo.Name = hidOldName.Value;
                Routeinfo.Attachs.Add(TourAttachInfo);
            }           
            #endregion           

            //公司编号
            Routeinfo.CompanyId = SiteUserInfo.CompanyID;
            //当前操作员编号
            Routeinfo.OperatorId = SiteUserInfo.ID;
            //当前操作员姓名
            if (SiteUserInfo.ContactInfo != null)
                Routeinfo.OperatorName = SiteUserInfo.ContactInfo.ContactName;

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
                if (Route.InsertRouteInfo(Routeinfo) > 0)
                {
                    SetErrorMsg(true, "线路信息添加成功!");
                }
                else
                {
                    SetErrorMsg(false, "线路信息添加失败!");
                }
            }

            if (hidType == "update" || hidType == "UpdateT")
            {
                if (Route.UpdateRouteInfo(Routeinfo) > 0)
                {
                    SetErrorMsg(true, "线路信息修改成功!");
                }
                else
                {
                    SetErrorMsg(false, "线路信息修改失败!");
                }
            }
        }
        #endregion

        #region 提示操作信息方法
        /// <summary>
        /// 提示提交信息
        /// </summary>
        /// <param name="IsSuccess">true 执行成功 flase 执行失败</param>
        /// <param name="Msg">提示信息</param>
        private void SetErrorMsg(bool isSuccess, string msg)
        {
            if (isSuccess)
            {
                MessageBox.ShowAndRedirect(this, "保存成功!", "/xianlu/LineProducts.aspx");
            }
            else
            {
                MessageBox.Show(this, "保存失败!");
            }
        }
        #endregion
    }
}
