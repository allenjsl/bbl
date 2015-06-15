using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 包含项目接待标准、不含项目、自费项目、儿童安排、购物安排、注意事项、温馨提醒等模板数据访问接口
    /// </summary>
    /// Author:汪奇志 2011-04-28
    public interface INotepadService
    {
        /// <summary>
        /// 写入模板信息
        /// </summary>
        /// <param name="info">EyouSoft.Model.CompanyStructure.MNotepadServiceInfo</param>
        /// <returns></returns>
        bool InsertNotepad(EyouSoft.Model.CompanyStructure.MNotepadServiceInfo info);
        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.MNotepadServiceInfo GetInfo(int id);
        /// <summary>
        /// 更新模板信息
        /// </summary>
        /// <param name="info">EyouSoft.Model.CompanyStructure.MNotepadServiceInfo</param>
        /// <returns></returns>
        bool UpdateNotepad(EyouSoft.Model.CompanyStructure.MNotepadServiceInfo info);
        /// <summary>
        /// 获取模板信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="type">模板类型</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.MNotepadServiceInfo> GetNotepads(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.EnumType.CompanyStructure.NotepadServiceType? type, EyouSoft.Model.CompanyStructure.MNotepadServiceSearchInfo searchInfo);
        /// <summary>
        /// 删除模板信息
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns></returns>
        bool DeleteNotepad(int id);
    }
}
