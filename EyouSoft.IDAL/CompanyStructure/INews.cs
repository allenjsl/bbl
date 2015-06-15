using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公告信息数据层接口
    /// </summary>
    /// 鲁功源 2011-01-21
    public interface INews
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.CompanyStructure.News model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.CompanyStructure.News model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params int[] Ids);
        /// <summary>
        /// 获取公告信息实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.News GetModel(int Id);
        /// <summary>
        /// 设置点击次数
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        bool SetClicks(int id);
        /// <summary>
        /// 分页获取公告信息列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.News> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId);

        /// <summary>
        /// 获取某个用户接收到的消息列表
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="companydId">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> GetAcceptNews(int PageSize, int PageIndex, ref int RecordCount, int userId, int companydId);
    
        /// <summary>
        /// 获取组团端消息
        /// </summary>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> GetZuTuanAcceptNews(int CompanyId);

         /// <summary>
        /// 获取组团端消息
        /// </summary>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> GetZuTuanAcceptNews(int PageSize, int PageIndex, ref int RecordCount, int CompanyId,EyouSoft.Model.PersonalCenterStructure.NoticeNews SearchModel);
    }
}
