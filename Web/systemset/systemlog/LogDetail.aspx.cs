using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.systemlog
{  
    /// <summary>
    /// 查看日志详情
    /// xuty 2011/1/17
    /// </summary>
    public partial class LogDetail : Eyousoft.Common.Page.BackPage
    {   
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_系统日志_系统日志栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_系统日志_系统日志栏目, false);
                return;
            }
            EyouSoft.BLL.CompanyStructure.SysHandleLogs logBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();//日志bll
            //初始化日志信息
            string logId=Utils.GetQueryStringValue("logid");
            EyouSoft.Model.CompanyStructure.SysHandleLogs logModel= logBll.GetModel(logId);//日志实体
            if (logModel != null)
            {
                litContent.Text = logModel.EventMessage;//内容
                litDepart.Text = logModel.DepartName;//部门
                litIP.Text = logModel.EventIp;//IP
                litModule.Text = logModel.ModuleId.ToString();//操作模块
                litOperator.Text = logModel.OperatorName;//操作人姓名
                litTime.Text = logModel.EventTime.ToString("yyyy-MM-dd HH:mm");//操作时间
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
