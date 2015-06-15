using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 公告管理BLL
    /// xuqh 2011-01-25
    /// </summary>
    public class News
    {
        private readonly EyouSoft.IDAL.CompanyStructure.INews Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.INews>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region 成员方法
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.News model)
        {
            bool result = false;
            result = Dal.Add(model);
            handleLogsBll.Add(AddLogs("添加", result));

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.News model)
        {
            bool result = false;
            result = Dal.Update(model);
            handleLogsBll.Add(AddLogs("修改", result));

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] Ids)
        {
            bool result = false;
            result = Dal.Delete(Ids);
            handleLogsBll.Add(AddLogs("删除", result));

            return result;
        }

        /// <summary>
        /// 获取公告信息实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.News GetModel(int Id)
        {
            return Dal.GetModel(Id);
        }

        /// <summary>
        /// 设置点击次数
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetClicks(int id)
        {
            return Dal.SetClicks(id);
        }

        /// <summary>
        /// 分页获取公告信息列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.News> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            return Dal.GetList(PageSize, PageIndex, ref RecordCount, CompanyId);
        }

        /// <summary>
        /// 获取某个用户接收到的消息列表
        /// </summary>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">开始页码</param>
        /// <param name="RecordCount">总数</param>
        /// <param name="userId">用户编号</param>
        /// <param name="companydId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> GetAcceptNews(int PageSize, int PageIndex, ref int RecordCount, int userId, int companydId)
        {
            return Dal.GetAcceptNews(PageSize, PageIndex, ref RecordCount, userId, companydId);
        }

        /// <summary>
        /// 获取组团端消息
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> GetZuTuanAcceptNews(int CompanyId)
        {
            return Dal.GetZuTuanAcceptNews(CompanyId);
        }

         /// <summary>
        /// 获取组团端消息
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> GetZuTuanAcceptNews(int PageSize, int PageIndex, ref int RecordCount, int CompanyId,EyouSoft.Model.PersonalCenterStructure.NoticeNews SearchModel)
        {
            return Dal.GetZuTuanAcceptNews(PageSize, PageIndex, ref RecordCount, CompanyId,SearchModel);
        }
        #endregion


        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="flag">操作状态</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, bool flag)
        {
            EyouSoft.Model.CompanyStructure.SysHandleLogs model = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            model.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_信息管理;
            model.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_信息管理.ToString() + (flag ? actionName : actionName + "失败") + "了信息管理数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_信息管理.ToString() + "数据";

            return model;
        }
    }
}
