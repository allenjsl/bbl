using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 系统登录日志DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class SysLoginLog : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ISysLoginLog
    {
        #region static constants

        private const string SQL_INSERT_SysLoginLog = "insert into tbl_SysLoginLog (ID,Operator,DepatId,CompanyId,LoginIp) values(@ID,@Operator,@DepatId,@CompanyId,@LoginIp)";
        private const string SQL_SELECT_SysLoginLog = "select ID,Operator,DepatId,CompanyId,LoginTime,LoginIp from tbl_SysLoginLog where ID = @ID";
        private readonly Database _db = null;

        #endregion

        #region 构造函数
        public SysLoginLog()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ISysLoginLog 成员

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">系统登陆日志实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.CompanyStructure.SysLoginLog model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_SysLoginLog);

            this._db.AddInParameter(cmd, "ID", DbType.String, model.ID);
            this._db.AddInParameter(cmd, "Operator", DbType.Int32, model.Operator);
            this._db.AddInParameter(cmd, "DepatId", DbType.Int32, model.DepatId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "LoginIp", DbType.String, model.LoginIp);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取登录日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        public EyouSoft.Model.CompanyStructure.SysLoginLog GetModel(string id)
        {
            EyouSoft.Model.CompanyStructure.SysLoginLog sysLoginLogModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_SysLoginLog);
            this._db.AddInParameter(cmd, "ID", DbType.String, id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    sysLoginLogModel = new EyouSoft.Model.CompanyStructure.SysLoginLog();
                    sysLoginLogModel.ID = rdr.GetString(rdr.GetOrdinal("Id"));
                    sysLoginLogModel.Operator = rdr.GetInt32(rdr.GetOrdinal("Operator"));
                    sysLoginLogModel.DepatId = rdr.GetInt32(rdr.GetOrdinal("DepatId"));
                    sysLoginLogModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    sysLoginLogModel.LoginTime = rdr.GetDateTime(rdr.GetOrdinal("LoginTime"));
                    sysLoginLogModel.LoginIp = rdr.GetString(rdr.GetOrdinal("LoginIp"));
                }
            }

            return sysLoginLogModel;
        }

        #endregion
    }
}
