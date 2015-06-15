using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-会议记录BLL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class MeetingInfo : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.AdminCenterStructure.IMeetingInfo idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.AdminCenterStructure.IMeetingInfo>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MeetingInfo()
        {
        }
                /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public MeetingInfo(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        #endregion 构造函数

        #region 公共方法
        /// <summary>
        /// 获取会议记录管理实体信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">会议主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.MeetingInfo GetModel(int CompanyId, int Id)
        {
            return idal.GetModel(CompanyId,Id);
        }
        /// <summary>
        /// 获取会议记录管理信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="MeetingNo">会议编号</param>
        /// <param name="MeetingTile">会议主题</param>
        /// <param name="BeginStart">会议时间开始</param>
        /// <param name="BeginEnd">会议时间结束</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.MeetingInfo> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, string MeetingNo, string MeetingTile, DateTime? BeginStart, DateTime? BeginEnd)
        {
            return idal.GetList(PageSize, PageIndex, ref RecordCount, CompanyId, MeetingNo, MeetingTile,BeginStart, BeginEnd);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">会议记录管理实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.MeetingInfo model)
        {
            bool IsTrue = false;
            IsTrue = idal.Add(model);            
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_会议记录.ToString() + "新增了会议记录信息数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "新增会议记录信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_会议记录;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return IsTrue;               
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">会议记录管理实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.AdminCenterStructure.MeetingInfo model)
        {
            bool IsTrue = false;
            IsTrue = idal.Update(model);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_会议记录.ToString() + "修改了会议记录信息数据，编号为：" + model.Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改会议记录信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_会议记录;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return IsTrue;               
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">会议记录管理编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, int Id)
        {
            bool IsTrue = false;
            IsTrue = idal.Delete(CompanyId, Id);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_会议记录.ToString() + "删除了会议记录信息数据，编号为：" + Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除会议记录信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_会议记录;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return IsTrue;                     
        }
        #endregion 公共方法

        #region 私有方法
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo">日志信息</param>
        private void Logwr(EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo)
        {
            EyouSoft.BLL.CompanyStructure.SysHandleLogs logbll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();
            logbll.Add(logInfo);
            logbll = null;
        }
        #endregion
    }
}
