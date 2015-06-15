using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 描述:客户留言回复业务类
    /// 修改记录:
    /// 1. 2010-03-17 AM 曹胡生 创建
    /// </summary>
    public class CustomerMessageReply : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICustomerMessageReply dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICustomerMessageReply>();

        #region 构造函数
        public CustomerMessageReply()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public CustomerMessageReply(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        #endregion 构造函数

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

        /// 获取留言列表
        /// </summary>
        /// <param name="companyId">组团登录为组团公司编号,专线登录为专线公司编号</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">返回第几页</param>
        /// <param name="recordCount">返回的记录数</param>
        /// <param name="isZhuTuan">True:组团端,False:专线端</param>
        /// <param name="customerMessageModel">留言搜索实体</param> 
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerMessageModel> GetMessageList(int companyId, int pageSize, int pageIndex, ref int recordCount, bool isZhuTuan, EyouSoft.Model.CompanyStructure.CustomerMessageModel customerMessageModel)
        {
            return dal.GetMessageList(companyId, pageSize, pageIndex, ref recordCount, isZhuTuan, customerMessageModel);
        }

        /// <summary>
        /// 根据公司编号与留言编号获得回复内容,组团端传组团公司编号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="messageID">留言编号</param>
        /// <param name="isZhuTuan">True:组团端,False:专线端</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel GetReplyByMessageId(int companyId, int messageID, bool isZhuTuan)
        {
            return dal.GetReplyByMessageId(companyId, messageID, isZhuTuan);
        }

          /// <summary>
        /// 根据留言编号获得留言
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerMessageModel GetMessageById(int Id)
        {
            return dal.GetMessageById(Id);
        }

        /// <summary>
        /// 添加一条留言
        /// </summary>
        /// <returns></returns>
        public bool AddMessage(EyouSoft.Model.CompanyStructure.CustomerMessageModel CustomerMessageModel)
        {
            if (CustomerMessageModel == null) return false;
            if (dal.AddMessage(CustomerMessageModel))
            {
                #region LGWR
                //EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                //logInfo.CompanyId = 0;
                //logInfo.DepatId = 0;
                //logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                //logInfo.EventIp = string.Empty;
                //logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"{0}在"+EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板.ToString()+"新增了一条留言，留言标题：" + CustomerMessageModel.MessageTitle;
                //logInfo.EventTime = DateTime.Now;
                //logInfo.EventTitle = "添加留言";
                //logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板;
                //logInfo.OperatorId = 0;
                //this.Logwr(logInfo);
                #endregion
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新一条留言
        /// </summary>
        /// <returns></returns>
        public bool UpdateMessage(EyouSoft.Model.CompanyStructure.CustomerMessageModel CustomerMessageModel)
        {
            if (CustomerMessageModel == null) return false;
            if (dal.UpdateMessage(CustomerMessageModel))
            {
                #region LGWR
                //EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                //logInfo.CompanyId = 0;
                //logInfo.DepatId = 0;
                //logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                //logInfo.EventIp = string.Empty;
                //logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在"+EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板.ToString()+"更新了一条留言，留言标题：" + CustomerMessageModel.MessageTitle;
                //logInfo.EventTime = DateTime.Now;
                //logInfo.EventTitle = "更新留言";
                //logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板;
                //logInfo.OperatorId = 0;
                //this.Logwr(logInfo);
                #endregion
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加一条留言回复
        /// </summary>
        /// <returns></returns>
        public bool AddReply(int companyID, EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel CustomerMessageReplyModel)
        {
            if (CustomerMessageReplyModel == null) return false;
            if (dal.AddReply(companyID, CustomerMessageReplyModel))
            {
                #region LGWR
                //EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                //logInfo.CompanyId = 0;
                //logInfo.DepatId = 0;
                //logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                //logInfo.EventIp = string.Empty;
                //logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板.ToString() + "在留言板添加了一条留言回复，回复人：" + CustomerMessageReplyModel.ReplyPersonName;
                //logInfo.EventTime = DateTime.Now;
                //logInfo.EventTitle = "添加留言回复";
                //logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板;
                //logInfo.OperatorId = 0;
                //this.Logwr(logInfo);
                #endregion
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新一条留言回复
        /// </summary>
        /// <returns></returns>
        public bool UpdateReply(int companyID, EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel CustomerMessageReplyModel)
        {
            if (CustomerMessageReplyModel == null) return false;
            if (dal.UpdateReply(companyID, CustomerMessageReplyModel))
            {
                #region LGWR
                //EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                //logInfo.CompanyId = 0;
                //logInfo.DepatId = 0;
                //logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                //logInfo.EventIp = string.Empty;
                //logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板.ToString() + "在留言板更新了一条留言回复，回复人：" + CustomerMessageReplyModel.ReplyPersonName;
                //logInfo.EventTime = DateTime.Now;
                //logInfo.EventTitle = "更新留言回复";
                //logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板;
                //logInfo.OperatorId = 0;
                //this.Logwr(logInfo);
                #endregion
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除一条留言
        /// </summary>
        /// <param name="companyID">专线公司编号</param>
        /// <param name="messageID">留言编号</param>
        /// <returns></returns>
        public bool DeleteMessage(int companyID, params int[] messageID)
        {
            if (messageID == null || messageID.Length == 0) return false;
            string messageIds = string.Empty;
            foreach (var str in messageID)
            {
                messageIds += str.ToString() + ",";
            }
            messageIds = messageIds.Trim(',');
            if (companyID > 0)
            {
                if (dal.DeleteMessage(companyID, messageIds))
                {
                    #region LGWR
                    //EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                    //logInfo.CompanyId = 0;
                    //logInfo.DepatId = 0;
                    //logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                    //logInfo.EventIp = string.Empty;
                    //logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板.ToString() + "在留言板删除了一条留言，留言编号：" + messageID.ToString();
                    //logInfo.EventTime = DateTime.Now;
                    //logInfo.EventTitle = "删除留言";
                    //logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_留言板;
                    //logInfo.OperatorId = 0;
                    //this.Logwr(logInfo);
                    #endregion
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
