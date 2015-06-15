using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.xianlu
{
    /// <summary>
    /// 模块名称:发布线路信息页面(标准版发布)
    /// 创建时间:2011-01-12 
    /// 创建人:lixh
    /// </summary>
    public partial class Add_xl_Standard :Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("issave")) == 1)
            {
                this.OnSave();
            }
            else
            {
                if (!this.Page.IsPostBack)
                {
                    this.Page.Title = "标准发布_新增线路_线路产品库_" + SiteUserInfo.CompanyName + "";
                    InitBindLineType();
                }
            }
        }

        #region 绑定线路区域
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
        protected void OnSave()
        {
            bool result = true;

            //线路区域
            int lineTypeID = Utils.GetInt(Utils.GetString(Utils.GetFormValue(ddlLineType.UniqueID), ""));
            if (lineTypeID == 0)
            {
                result = false;
                MessageBox.Show(this, "请选择线路区域!");
                return;
            }
            //线路名称
            string lineName = Utils.GetString(Utils.GetFormValue(this.txt_LineName.Name), "");
            if (lineName == "")
            {
                result = false;
                MessageBox.Show(this, "请填写线路名称!");                
                return;
            }
            //线路描述
            string Description = Utils.GetString(Utils.GetFormValue(this.txt_Description.UniqueID), "");
            //旅游天数
            int Days = Utils.GetInt(Utils.GetString(Utils.GetFormValue(this.txt_Days.UniqueID), ""));
            if (Days == 0)
            {
                result = false;
                MessageBox.Show(this, "请填写旅游天数!");                
                return;
            }
            //不包含项目
            string ProjectNo = Utils.GetString(Utils.GetFormValue(this.txt_ProjectNo.UniqueID), "");
            //购物安排
            string ShoppingPlan = Utils.GetString(Utils.GetFormValue(this.txt_ShoppingPlan.UniqueID), "");
            //儿童安排
            string ChildrenPlan = Utils.GetString(Utils.GetFormValue(this.txt_ChildrenPlan.UniqueID), "");
            //自费项目
            string ExpenseProject = Utils.GetString(Utils.GetFormValue(this.txt_ExpenseProject.UniqueID), "");
            //注意事项
            string Notes = Utils.GetString(Utils.GetFormValue(this.txt_Notes.UniqueID), "");
            //温馨提示
            string reminder = Utils.GetString(Utils.GetFormValue(this.Txt_Reminder.UniqueID), "");
            //内部信息
            string Infromation = Utils.GetString(Utils.GetFormValue(this.Txt_Infromation.UniqueID), "");

            //线路库业务逻辑类
            EyouSoft.BLL.RouteStructure.Route Route = new EyouSoft.BLL.RouteStructure.Route();
            //线路基本信息实体类
            EyouSoft.Model.RouteStructure.RouteInfo Routeinfo = new EyouSoft.Model.RouteStructure.RouteInfo();
            Routeinfo.AreaId = lineTypeID;
            Routeinfo.RouteName = lineName;
            Routeinfo.RouteDepict = Description;
            Routeinfo.RouteDays = Days;
            Routeinfo.Attachs = new List<EyouSoft.Model.TourStructure.TourAttachInfo>(); 

            //标准发布团队行程信息业务实体  
            Routeinfo.RouteNormalInfo = new EyouSoft.Model.TourStructure.TourNormalPrivateInfo();
            #region 获取行程信息
            Routeinfo.RouteNormalInfo.Plans = this.xingcheng1.GetValues();
            Routeinfo.RouteNormalInfo.BuHanXiangMu = ProjectNo;
            Routeinfo.RouteNormalInfo.ErTongAnPai = ChildrenPlan;
            Routeinfo.RouteNormalInfo.GouWuAnPai = ShoppingPlan;
            Routeinfo.RouteNormalInfo.NeiBuXingXi = Infromation;
            #endregion
          
            //包含项目            
            Routeinfo.RouteNormalInfo.Services = this.ConProjectControl1.GetDataList();
            Routeinfo.RouteNormalInfo.WenXinTiXing = reminder;
            Routeinfo.RouteNormalInfo.ZhuYiShiXiang = Notes;
            Routeinfo.RouteNormalInfo.ZiFeiXIangMu = ExpenseProject;
            
            //设置线路的发布类型
            Routeinfo.ReleaseType = EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal;

            //公司编号
            Routeinfo.CompanyId = SiteUserInfo.CompanyID;
            //当前操作员编号
            Routeinfo.OperatorId = SiteUserInfo.ID;
            //当前操作员姓名
            if (SiteUserInfo.ContactInfo != null)
            {
                Routeinfo.OperatorName = SiteUserInfo.ContactInfo.ContactName;
            }            

            #region 上传附件
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
            }
            else
            {
                //上传失败提示
                this.litMsg.Text = Utils.ShowMsg("文件上传失败!");
                return;
            }
            #endregion           

            if (result)
            {
                if (Route.InsertRouteInfo(Routeinfo) > 0)
                {
                    MessageBox.ShowAndRedirect(this, "线路信息添加成功!", "/xianlu/LineProducts.aspx");
                }
                else
                {
                    MessageBox.ShowAndRedirect(this, "线路信息添加成功!", "/xianlu/Add_xl_Standard.aspx");
                }
            }
            Route=null;
            Routeinfo = null;
        }
        #endregion

    }
}
