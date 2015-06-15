using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 机票政策 数据类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class TicketPolicy : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SiteStructure.ITicketPolicy
    {
        private readonly Database DB = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public TicketPolicy()
        {
            DB = this.SystemStore;
        }
        /// <summary>
        /// 获得当前公司的机票政策
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SiteStructure.TicketPolicy> GetTicketPolicy(int CompanyId, int pageSize, int pageIndex, ref int recordCount)
        {
            IList<EyouSoft.Model.SiteStructure.TicketPolicy> items = new List<EyouSoft.Model.SiteStructure.TicketPolicy>();
            EyouSoft.Model.SiteStructure.TicketPolicy item = null;
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_SiteTicketPolicy";
            string primaryKey = "PolicyId";
            string orderByString = "PolicyId ASC";
            StringBuilder fields = new StringBuilder();
            #region 要查询的字段
            fields.Append("*");
            #endregion
            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0}", CompanyId);
            #endregion
            using (IDataReader rdr = DbHelper.ExecuteReader(this.DB, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    item = new EyouSoft.Model.SiteStructure.TicketPolicy()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("PolicyId")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        Content = rdr["Content"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        Title = rdr["Title"].ToString()
                    };
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// 获得机票政策实体信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SiteStructure.TicketPolicy GetTicketPolicy(int Id,int CompanyId)
        {
            EyouSoft.Model.SiteStructure.TicketPolicy TicketPolicy = null;
            DbCommand dc = this.DB.GetSqlStringCommand(String.Format("SELECT * FROM [tbl_SiteTicketPolicy] WHERE [PolicyId]={0} AND [CompanyId]={1}", Id, CompanyId));
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.DB))
            {
                while (rdr.Read())
                {
                    TicketPolicy = new EyouSoft.Model.SiteStructure.TicketPolicy()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("PolicyId")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        Content = rdr["Content"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        Title = rdr["Title"].ToString()
                    };
                }
            }
            return TicketPolicy;
        }

        /// <summary>
        /// 添加机票政策
        /// </summary>
        /// <param name="TicketPolicy"></param>
        /// <returns></returns>
        public bool AddTicketPolicy(EyouSoft.Model.SiteStructure.TicketPolicy TicketPolicy)
        {
            string SQL = "INSERT INTO [tbl_SiteTicketPolicy]([CompanyId],[Content],[FilePath],[Title]) VALUES(@CompanyId,@Content,@FilePath,@Title)";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, TicketPolicy.CompanyId);
            this.DB.AddInParameter(dc, "Content", DbType.String, TicketPolicy.Content);
            this.DB.AddInParameter(dc, "FilePath", DbType.String, TicketPolicy.FilePath);
            this.DB.AddInParameter(dc, "Title", DbType.String, TicketPolicy.Title);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 修改机票政策
        /// </summary>
        /// <param name="TicketPolicy"></param>
        /// <returns></returns>
        public bool UpdateTicketPolicy(EyouSoft.Model.SiteStructure.TicketPolicy TicketPolicy)
        {
            string SQL = "UPDATE [tbl_SiteTicketPolicy] SET [Content]=@Content,[FilePath]=@FilePath,[Title]=@Title WHERE [PolicyId]=@PolicyId AND [CompanyId]=@CompanyId";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "PolicyId", DbType.Int32, TicketPolicy.Id);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, TicketPolicy.CompanyId);
            this.DB.AddInParameter(dc, "Content", DbType.String, TicketPolicy.Content);
            this.DB.AddInParameter(dc, "FilePath", DbType.String, TicketPolicy.FilePath);
            this.DB.AddInParameter(dc, "Title", DbType.String, TicketPolicy.Title);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 删除机票政策
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DelTicketPolicy( int Id,int CompanyId)
        {
            string SQL = "DELETE FROM [tbl_SiteTicketPolicy] WHERE [PolicyId]=@PolicyId AND [CompanyId]=@CompanyId";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "PolicyId", DbType.Int32, Id);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }
    }
}
