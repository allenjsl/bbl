using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Eyousoft.Common;
using EyouSoft.Common.Function;

namespace Web.xianlu
{
    /// <summary>
    /// 模块名称:发布线路信息页面(快速版发布)
    /// 创建时间:2011-01-12 
    /// 创建人:lixh
    /// </summary>
    public partial class AddLineProducts : Eyousoft.Common.Page.BackPage
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
                    this.Page.Title = " 快速发布_新增线路_线路产品库_" + SiteUserInfo.CompanyName + "";
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

            //线路区域编号
            int LineTypeID = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(ddlLineType.UniqueID.Trim()), ""));
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
            string LineDesCription = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(txt_Description.UniqueID.Trim()), "");
            //旅游天数
            int txtDays = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(txt_Days.UniqueID.Trim()), ""));
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
            string Remarks = EyouSoft.Common.Utils.GetString(EyouSoft.Common.Utils.GetFormValue(txt_Remarks.UniqueID.Trim()), "");

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

            #region 上传附件
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
            }
            #endregion

            //设置线路的发布类型
            Routeinfo.ReleaseType = EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick;

            //公司编号
            Routeinfo.CompanyId = SiteUserInfo.CompanyID;
            //当前登录人编号
            Routeinfo.OperatorId = SiteUserInfo.ID;
            //当前登录人姓名
            if (SiteUserInfo.ContactInfo != null)
                Routeinfo.OperatorName = SiteUserInfo.ContactInfo.ContactName;

            if (result)
            {
                if (Route.InsertRouteInfo(Routeinfo) > 0)
                {
                    MessageBox.ShowAndRedirect(this, "线路信息添加成功!", "/xianlu/LineProducts.aspx");
                }
                else
                {
                    MessageBox.ShowAndRedirect(this, "线路信息添加成功!", "/xianlu/AddLineProducts.aspx");
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
