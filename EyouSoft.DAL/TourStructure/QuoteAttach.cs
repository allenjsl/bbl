using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using EyouSoft.Toolkit.DAL;

using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EyouSoft.DAL.TourStructure
{
    /// <summary>
    /// 描述:报价附件数据类
    /// 修改记录:
    /// 1. 2011-03-17 PM 曹胡生 创建
    /// </summary>
    public class QuoteAttach : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.TourStructure.IQuoteAttach
    {
        private readonly Database DB = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public QuoteAttach()
        {
            DB = this.SystemStore;
        }

        /// <summary>
        /// 获得报价附件列表
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">返回第几页</param>
        /// <param name="recordCount">返回的记录数</param>
        /// <param name="QuoteAttach">报价搜索实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.QuoteAttach> GetQuoteList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach)
        {
            IList<EyouSoft.Model.TourStructure.QuoteAttach> items = new List<EyouSoft.Model.TourStructure.QuoteAttach>();
            EyouSoft.Model.TourStructure.QuoteAttach item = null;
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Quote";
            string primaryKey = "Id";
            string orderByString = "AddTime DESC";
            StringBuilder fields = new StringBuilder();
            #region 要查询的字段
            fields.Append("Id,CompanyId,FilePath,FileName,OperatorId,(select contactname from dbo.tbl_CompanyUser where id =tbl_Quote.OperatorId) as OperatorName,ValidityStart,ValidityEnd,AddTime");
            #endregion
            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0}", companyId);
            if (QuoteAttach != null)
            {
                if (!String.IsNullOrEmpty(QuoteAttach.FileName))
                {
                    cmdQuery.AppendFormat("  and FileName like '%{0}%'", QuoteAttach.FileName);
                }
                if (QuoteAttach.AddTime.HasValue && QuoteAttach.AddTime != DateTime.MinValue)
                {
                    cmdQuery.AppendFormat("  and DATEDIFF(DAY,'{0}',AddTime)=0", QuoteAttach.AddTime);
                }
            }
            #endregion
            using (IDataReader rdr = DbHelper.ExecuteReader(this.DB, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                //EyouSoft.DAL.CompanyStructure.CompanyUser CompanyUser = new EyouSoft.DAL.CompanyStructure.CompanyUser();
                while (rdr.Read())
                {
                    item = new EyouSoft.Model.TourStructure.QuoteAttach()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        OperatorId = rdr.IsDBNull(rdr.GetOrdinal("OperatorId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        OperatorName = rdr["OperatorName"].ToString(),
                        //ContactPersonInfo=CompanyUser.GetUserBasicInfo(item.OperatorId),
                        ValidityStart = rdr.IsDBNull(rdr.GetOrdinal("ValidityStart")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("ValidityStart")),
                        ValidityEnd = rdr.IsDBNull(rdr.GetOrdinal("ValidityEnd")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("ValidityEnd")),
                        FileName = rdr["FileName"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        AddTime = rdr.IsDBNull(rdr.GetOrdinal("AddTime")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("AddTime"))
                    };
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// 根据报价编号,获得报价附件信息
        /// </summary>
        /// <param name="QuoteId">报价编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.QuoteAttach GetQuoteInfo(int QuoteId)
        {
            EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach = null;
            StringBuilder SQL = new StringBuilder();
            SQL.AppendFormat("SELECT tbl_Quote.*,(SELECT ContactName FROM [tbl_CompanyUser] where Id=tbl_Quote.OperatorId) as OperatorName   FROM [tbl_Quote] WHERE [Id]={0}", QuoteId);
            DbCommand cmd = this.DB.GetSqlStringCommand(SQL.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this.DB))
            {
                while (rdr.Read())
                {
                    QuoteAttach = new EyouSoft.Model.TourStructure.QuoteAttach()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        OperatorId = rdr.IsDBNull(rdr.GetOrdinal("OperatorId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        ValidityStart = rdr.IsDBNull(rdr.GetOrdinal("ValidityStart")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("ValidityStart")),
                        ValidityEnd = rdr.IsDBNull(rdr.GetOrdinal("ValidityEnd")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("ValidityEnd")),
                        FileName = rdr["FileName"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        AddTime = rdr.IsDBNull(rdr.GetOrdinal("AddTime")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("AddTime")),
                        OperatorName = rdr["OperatorName"].ToString()
                    };
                }
            }
            return QuoteAttach;
        }

        /// <summary>
        /// 添加报价附件信息
        /// </summary>
        /// <param name="QuoteAttach"></param>
        /// <returns></returns>
        public bool AddQuote(EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach)
        {
            string SQL = "INSERT INTO [tbl_Quote](CompanyId,FilePath,FileName,OperatorId,ValidityStart,ValidityEnd,AddTime) VALUES(@CompanyId,@FilePath,@FileName,@OperatorId,@ValidityStart,@ValidityEnd,@AddTime)";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, QuoteAttach.CompanyId);
            this.DB.AddInParameter(dc, "FilePath", DbType.String, QuoteAttach.FilePath);
            this.DB.AddInParameter(dc, "FileName", DbType.String, QuoteAttach.FileName);
            this.DB.AddInParameter(dc, "OperatorId", DbType.Int32, QuoteAttach.OperatorId);
            this.DB.AddInParameter(dc, "ValidityStart", DbType.DateTime, QuoteAttach.ValidityStart);
            this.DB.AddInParameter(dc, "ValidityEnd", DbType.DateTime, QuoteAttach.ValidityEnd);
            this.DB.AddInParameter(dc, "AddTime", DbType.DateTime, QuoteAttach.AddTime);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 更新报价附件信息
        /// </summary>
        /// <param name="QuoteAttach"></param>
        /// <returns></returns>
        public bool UpdateQuote(EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach)
        {
            string SQL = String.Format("UPDATE [tbl_Quote] set CompanyId=@CompanyId,FilePath=@FilePath,FileName=@FileName,OperatorId=@OperatorId,ValidityStart=@ValidityStart,ValidityEnd=@ValidityEnd,AddTime=@AddTime WHERE [Id]={0}", QuoteAttach.Id);
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, QuoteAttach.CompanyId);
            this.DB.AddInParameter(dc, "FilePath", DbType.String, QuoteAttach.FilePath);
            this.DB.AddInParameter(dc, "FileName", DbType.String, QuoteAttach.FileName);
            this.DB.AddInParameter(dc, "OperatorId", DbType.Int32, QuoteAttach.OperatorId);
            this.DB.AddInParameter(dc, "ValidityStart", DbType.DateTime, QuoteAttach.ValidityStart);
            this.DB.AddInParameter(dc, "ValidityEnd", DbType.DateTime, QuoteAttach.ValidityEnd);
            this.DB.AddInParameter(dc, "AddTime", DbType.DateTime, QuoteAttach.AddTime);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        ///根据报价编号, 删除报价附件信息
        /// </summary>
        /// <param name="QuoteId"></param>
        /// <returns></returns>
        public bool DeleteQuote(string QuoteId)
        {
            //string SQL = String.Format("IF NOT EXISTS(SELECT 1 FROM [tbl_SysDeletedFileQue] WHERE [FilePath]=(SELECT FilePath FROM [tbl_Quote] WHERE ID={0})); INSERT INTO [tbl_SysDeletedFileQue]([FilePath]) SELECT FilePath FROM [tbl_Quote] WHERE ID={0}; DELETE FROM [tbl_Quote] WHERE [Id]={0}", QuoteId);
            string SQL = String.Format("INSERT INTO [tbl_SysDeletedFileQue]([FilePath]) SELECT FilePath FROM [tbl_Quote] WHERE ID in({0}) AND NOT EXISTS(SELECT 1 FROM [tbl_SysDeletedFileQue] WHERE [FilePath] in(SELECT FilePath FROM [tbl_Quote] WHERE ID in({0}))); DELETE FROM [tbl_Quote] WHERE [Id] in({0})", QuoteId);
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }
    }
}
