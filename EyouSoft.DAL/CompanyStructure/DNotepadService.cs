/*Author:汪奇志 2011-04-28*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;
using System.Xml.Linq;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 包含项目接待标准、不含项目、自费项目、儿童安排、购物安排、注意事项、温馨提醒等模板数据访问类
    /// </summary>
    /// Author:汪奇志 2011-04-28
    public class DNotepadService:EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.INotepadService
    {
        #region static constants
        //static constants
        private const string SQL_INSERT_InsertNotepad = "INSERT INTO [tbl_NotepadService]([CompanyId],[Type],[Text],[OperatorId]) VALUES (@CompanyId,@Type,@Text,@OperatorId)";
        private const string SLQ_UPDATE_UpdateNotepad = "UPDATE [tbl_NotepadService] SET [Text] = @Text,[OperatorId] = @OperatorId WHERE [Id]=@Id";
        private const string SQL_SELECT_GetInfo = "SELECT * FROM [tbl_NotepadService] WHERE [Id]=@Id";
        private const string SQL_DELETE_DeleteNotepad = "DELETE FROM [tbl_NotepadService] WHERE [Id]=@Id";
        #endregion

        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DNotepadService()
        {
            this._db = base.SystemStore;
        }
        #endregion      

        #region INotepadService 成员
        /// <summary>
        /// 写入模板信息
        /// </summary>
        /// <param name="info">EyouSoft.Model.CompanyStructure.MNotepadServiceInfo</param>
        /// <returns></returns>
        public bool InsertNotepad(EyouSoft.Model.CompanyStructure.MNotepadServiceInfo info)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_InsertNotepad);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "Type", DbType.Byte, info.Type);
            this._db.AddInParameter(cmd, "Text", DbType.String, info.Text);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);

            return DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
        }

        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.MNotepadServiceInfo GetInfo(int id)
        {
            EyouSoft.Model.CompanyStructure.MNotepadServiceInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetInfo);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.CompanyStructure.MNotepadServiceInfo()
                    {
                        CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        CreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime")),
                        Id = id,
                        OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        Text = rdr["Text"].ToString(),
                        Type = (EyouSoft.Model.EnumType.CompanyStructure.NotepadServiceType)rdr.GetByte(rdr.GetOrdinal("Type"))
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 更新模板信息
        /// </summary>
        /// <param name="info">EyouSoft.Model.CompanyStructure.MNotepadServiceInfo</param>
        /// <returns></returns>
        public bool UpdateNotepad(EyouSoft.Model.CompanyStructure.MNotepadServiceInfo info)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SLQ_UPDATE_UpdateNotepad);
            this._db.AddInParameter(cmd, "Text", DbType.String, info.Text);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, info.Id);

            return DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
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
            IList<EyouSoft.Model.CompanyStructure.MNotepadServiceInfo> items = new List<EyouSoft.Model.CompanyStructure.MNotepadServiceInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_NotepadService";
            string primaryKey = "Id";
            string orderByString = "Id DESC";
            string fields = "*";

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            if (type.HasValue)
            {
                cmdQuery.AppendFormat(" AND Type={0} ", (byte)type.Value);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.CompanyStructure.MNotepadServiceInfo()
                    {
                        CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        CreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime")),
                        Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        Text = rdr["Text"].ToString(),
                        Type = (EyouSoft.Model.EnumType.CompanyStructure.NotepadServiceType)rdr.GetByte(rdr.GetOrdinal("Type"))
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 删除模板信息
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns></returns>
        public bool DeleteNotepad(int id)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_DeleteNotepad);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);

            return DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
        }

        #endregion
    }
}
