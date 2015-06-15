using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using System.Text;
namespace Web.systemset.basicinfo
{   
    /// <summary>
    /// 编辑线路区域
    /// xuty 2011/1/24
    /// </summary>
    public partial class EditRouteArea : Eyousoft.Common.Page.BackPage
    {
        protected string areaName = string.Empty;//线路区域
        protected string txtOperatorName;//计调员文本框ID
        protected string txtHidOperatorId;//计调员隐藏域ID
        protected int AreaSortId,MinSortId,MaxSortId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_基础设置_线路区域栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_基础设置_线路区域栏目, false);
                return;
            }
            int areaId = Utils.GetInt(Utils.GetQueryStringValue("areaId"));//区域Id
            string method = Utils.GetFormValue("hidMethod");//获取当前操作(保存/继续)
            string showMess="数据保存成功";//提示消息
            EyouSoft.Model.CompanyStructure.Area areaModel=null;//线路区域实体
            EyouSoft.BLL.CompanyStructure.Area areaBll = new EyouSoft.BLL.CompanyStructure.Area();//初始化areaBll

            areaBll.GetAreaSortId(CurrentUserCompanyID, out MinSortId, out MaxSortId);

            //城市Id不为空则添加,否则视为保存
            if (method == "")
            {
                txtOperatorName = selOperator.FindControl("txt_op_Name").ClientID;
                txtHidOperatorId = selOperator.FindControl("hd_op_id").ClientID;
                #region 初次加载数据
                if (areaId != 0)
                {
                    areaModel=areaBll.GetModel(areaId);
                    if(areaModel!=null)
                    {
                        areaName=areaModel.AreaName;
                        AreaSortId = areaModel.SortId;
                        IList<EyouSoft.Model.CompanyStructure.UserArea> userAreaList = areaModel.AreaUserList;
                        if (userAreaList != null && userAreaList.Count > 0)
                        {   
                            StringBuilder operIds=new StringBuilder();
                            StringBuilder operNames=new StringBuilder();
                            foreach(var userArea in userAreaList)
                            {
                                operIds.AppendFormat("{0},",userArea.UserId);
                                operNames.AppendFormat("{0},", userArea.ContactName);
                            }
                            selOperator.OperId = operIds.ToString().TrimEnd(',');//计调人编号
                            selOperator.OperName = operNames.ToString().TrimEnd(',');//计调人姓名
                        }
                    }
                    return;
                }
                #endregion
            }
            else
            {
                #region 保存数据
                
                bool result = false;
                //构造区域实体
                areaModel = new EyouSoft.Model.CompanyStructure.Area();
                areaModel.OperatorId = SiteUserInfo.ID;
                areaModel.CompanyId = CurrentUserCompanyID;
                areaModel.AreaName = Utils.GetFormValue("txtAreaName");//修改线路区域
                areaModel.SortId = Utils.GetIntSign(Utils.GetFormValue("txtSortId"));
                if (areaModel.AreaName == "")
                {
                    MessageBox.Show(this,"线路区域不为空！");
                    return;
                }
                if (!string.IsNullOrEmpty(selOperator.OperId))
                {   //赋值责任计调
                    IList<EyouSoft.Model.CompanyStructure.UserArea> userAreaList = new List<EyouSoft.Model.CompanyStructure.UserArea>();
                    userAreaList = selOperator.OperId.Split(',').Select(u => new EyouSoft.Model.CompanyStructure.UserArea { AreaId = areaId, UserId = Utils.GetInt(u) }).ToList();
                    areaModel.AreaUserList = userAreaList;
                }
                if (areaId != 0)
                {
                    areaModel.Id = areaId;
                    result = areaBll.Update(areaModel,selOperator.OperId.Split(','));//修改线路区域
                }
                else
                {
                    areaModel.IssueTime = DateTime.Now;
                    result = areaBll.Add(areaModel,selOperator.OperId.Split(',').ToArray());//添加线路区域
                }
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                //继续添加则刷新页面,否则关闭当前窗口
                if (method == "continue")
                {
                    MessageBox.ShowAndRedirect(this, showMess, "EditRouteArea.aspx");
                }
                else
                {
                    MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/systemset/basicinfo/RouteArea.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", showMess, Utils.GetQueryStringValue("iframeId")));
                }
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
