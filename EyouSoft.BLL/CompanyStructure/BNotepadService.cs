using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 包含项目接待标准、不含项目、自费项目、儿童安排、购物安排、注意事项、温馨提醒等模板业务逻辑类
    /// </summary>
    /// Author:汪奇志 2011-04-28
    public class BNotepadService
    {
        private readonly EyouSoft.IDAL.CompanyStructure.INotepadService dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.INotepadService>();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public BNotepadService() { }
        #endregion

        #region public members
        /// <summary>
        /// 写入模板信息
        /// </summary>
        /// <param name="info">EyouSoft.Model.CompanyStructure.MNotepadServiceInfo</param>
        /// <returns></returns>
        public bool InsertNotepad(EyouSoft.Model.CompanyStructure.MNotepadServiceInfo info)
        {
            return dal.InsertNotepad(info);
        }
        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.MNotepadServiceInfo GetInfo(int id)
        {
            if (id < 1) return null; ;
            return dal.GetInfo(id);
        }
        /// <summary>
        /// 更新模板信息
        /// </summary>
        /// <param name="info">EyouSoft.Model.CompanyStructure.MNotepadServiceInfo</param>
        /// <returns></returns>
        public bool UpdateNotepad(EyouSoft.Model.CompanyStructure.MNotepadServiceInfo info)
        {
            if (info.Id < 1) return false;

            return dal.UpdateNotepad(info);
        }
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
        public IList<EyouSoft.Model.CompanyStructure.MNotepadServiceInfo> GetNotepads(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.EnumType.CompanyStructure.NotepadServiceType? type, EyouSoft.Model.CompanyStructure.MNotepadServiceSearchInfo searchInfo)
        {
            if (companyId < 1) return null;

            return dal.GetNotepads(companyId, pageSize, pageIndex, ref recordCount, type, searchInfo);
        }
        /// <summary>
        /// 删除模板信息
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns></returns>
        public bool DeleteNotepad(int id)
        {
            if (id < 1) return false;
            return dal.DeleteNotepad(id);
        }
        #endregion
    }
}
