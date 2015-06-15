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
    /// 系统操作日志DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class SysHandleLogs : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ISysHandleLogs
    {

        #region static constants
        private const string SQL_INSERT_SysHandleLogs = "insert into tbl_SysHandleLogs (ID,OperatorId,DepatId,CompanyId,ModuleId,EventCode,EventMessage,EventTitle,EventIp)"
                                                       + " values(@ID,@OperatorId,@DepatId,@CompanyId,@ModuleId,@EventCode,@EventMessage,@EventTitle,@EventIp)";
        //private const string SQL_SELECT_SysHandleLogs = "select ID,OperatorId,DepatId,CompanyId,ModuleId,EventCode,EventMessage,EventTitle,EventTime,EventIp from tbl_SysHandleLogs where ID = @ID";
        private const string SQL_SELECT_SysHandleLogs = "select * from View_SysHandleLogs where ID = @ID";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public SysHandleLogs()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ISysHandleLogs 成员

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model">系统操作日志实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.SysHandleLogs model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_SysHandleLogs);

            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, Guid.NewGuid().ToString());
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "DepatId", DbType.Int32, model.DepatId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "ModuleId", DbType.Int32, (int)model.ModuleId);
            this._db.AddInParameter(cmd, "EventCode", DbType.Int32, model.EventCode);
            this._db.AddInParameter(cmd, "EventMessage", DbType.String, model.EventMessage);
            this._db.AddInParameter(cmd, "EventTitle", DbType.String, model.EventTitle);
            this._db.AddInParameter(cmd, "EventIp", DbType.String, model.EventIp);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取操作日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        public EyouSoft.Model.CompanyStructure.SysHandleLogs GetModel(string id)
        {
            EyouSoft.Model.CompanyStructure.SysHandleLogs sysHandleLogsModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_SysHandleLogs);
            this._db.AddInParameter(cmd, "ID", DbType.String, id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    sysHandleLogsModel = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                    sysHandleLogsModel.ID = rdr.GetString(rdr.GetOrdinal("ID"));
                    sysHandleLogsModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    sysHandleLogsModel.DepatId = rdr.GetInt32(rdr.GetOrdinal("DepatId"));
                    sysHandleLogsModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    sysHandleLogsModel.ModuleId = (EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass)rdr.GetInt32(rdr.GetOrdinal("ModuleId"));
                    sysHandleLogsModel.EventCode = rdr.GetInt32(rdr.GetOrdinal("EventCode"));
                    sysHandleLogsModel.EventMessage = rdr.GetString(rdr.GetOrdinal("EventMessage"));
                    sysHandleLogsModel.EventTitle = rdr.GetString(rdr.GetOrdinal("EventTitle"));
                    sysHandleLogsModel.EventTime = rdr.GetDateTime(rdr.GetOrdinal("EventTime"));
                    sysHandleLogsModel.EventIp = rdr.GetString(rdr.GetOrdinal("EventIp"));
                    sysHandleLogsModel.OperatorName = rdr.IsDBNull(rdr.GetOrdinal("UserName")) ? "" : rdr.GetString(rdr.GetOrdinal("UserName"));
                    sysHandleLogsModel.DepartName = rdr.IsDBNull(rdr.GetOrdinal("DepartName")) ? "" : rdr.GetString(rdr.GetOrdinal("DepartName"));
                }
            }

            return sysHandleLogsModel;
        }

        /// <summary>
        /// 分页获取操作日志列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="model">系统操作日志查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.SysHandleLogs> GetList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.CompanyStructure.QueryHandleLog model)
        {
            IList<EyouSoft.Model.CompanyStructure.SysHandleLogs> totals = new List<EyouSoft.Model.CompanyStructure.SysHandleLogs>();

            string tableName = "View_SysHandleLogs";
            string primaryKey = "ID";
            string orderByString = "EventTime DESC";
            string fields = " Id, OperatorId, DepatId, CompanyId, ModuleId,EventCode,EventMessage,EventTitle,EventTime,EventIp,UserName,DepartName";

            StringBuilder cmdQuery = new StringBuilder();
            cmdQuery.AppendFormat(" CompanyId = {0}", model.CompanyId);
            if(model.DepartId > 0)
                cmdQuery.AppendFormat(" and DepatId={0}", model.DepartId);
            if(model.OperatorId > 0)
                cmdQuery.AppendFormat(" and OperatorId={0}", model.OperatorId);
            if(model.HandStartTime != null)
                cmdQuery.AppendFormat(" and EventTime >= '{0}'", model.HandStartTime);
            if (model.HandEndTime != null)
                cmdQuery.AppendFormat(" and EventTime <= '{0}' ", model.HandEndTime);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref RecordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.SysHandleLogs sysHandleLogsInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();

                    sysHandleLogsInfo.ID = rdr.GetString(rdr.GetOrdinal("ID"));
                    sysHandleLogsInfo.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    sysHandleLogsInfo.DepatId = rdr.GetInt32(rdr.GetOrdinal("DepatId"));
                    sysHandleLogsInfo.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    sysHandleLogsInfo.ModuleId = (EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass)rdr.GetInt32(rdr.GetOrdinal("ModuleId"));
                    sysHandleLogsInfo.EventCode = rdr.GetInt32(rdr.GetOrdinal("EventCode"));
                    sysHandleLogsInfo.EventMessage = rdr.GetString(rdr.GetOrdinal("EventMessage"));
                    sysHandleLogsInfo.EventTitle = rdr.GetString(rdr.GetOrdinal("EventTitle"));
                    sysHandleLogsInfo.EventTime = rdr.GetDateTime(rdr.GetOrdinal("EventTime"));
                    sysHandleLogsInfo.EventIp = rdr.GetString(rdr.GetOrdinal("EventIp"));
                    sysHandleLogsInfo.OperatorName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    sysHandleLogsInfo.DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"));

                    totals.Add(sysHandleLogsInfo);
                }
            }

            return totals;
        }

        #endregion
    }
}
