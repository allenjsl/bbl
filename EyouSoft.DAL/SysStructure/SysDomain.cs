using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.SysStructure
{
    /// <summary>
    /// 公司域名数据访问层
    /// </summary>
    public class SystemDomain : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SysStructure.ISystemDomain
    {
        #region static constants
        //static constants
        private const string SQL_DOMAIN_SELECT = "SELECT top 1 A.SysId,A.Domain,(SELECT TOP 1 B.ID FROM tbl_CompanyInfo AS B WHERE B.SystemId=A.SysId) AS CompanyId,A.[Url] FROM tbl_SysDomain AS A WHERE A.Domain=@domain";
        #endregion

        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public SystemDomain()
        {
            this._db = base.SystemStore;
        }
        #endregion      
        
        /// <summary>
        /// 获取域名信息
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        public EyouSoft.Model.SysStructure.SystemDomain GetDomain(string domain)
        {
            EyouSoft.Model.SysStructure.SystemDomain model = null;
            DbCommand dc = this.SystemStore.GetSqlStringCommand(SQL_DOMAIN_SELECT);
            this.SystemStore.AddInParameter(dc, "domain", DbType.String, domain);
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(dc, this.SystemStore))
            {
                if (rdr.Read())
                {
                    model = new EyouSoft.Model.SysStructure.SystemDomain()
                    {
                        Domain = rdr["Domain"].ToString(),
                        SysId = rdr.GetInt32(rdr.GetOrdinal("SysId")),
                        CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        Url=rdr["Url"].ToString()
                    };
                }
            }

            return model;
        }

        /// <summary>
        /// 验证域名是否重复，返回重复的域名信息集合
        /// </summary>
        /// <param name="domains">域名信息集合</param>
        /// <param name="sysId">系统编号 HasValue时排除该系统原有域名</param>
        /// <returns></returns>
        public IList<string> IsExistsDomains(IList<string> domains, int? sysId)
        {
            IList<string> items = new List<string>();

            StringBuilder cmdText = new StringBuilder();
            cmdText.Append(" SELECT [Domain] FROM [tbl_SysDomain] WHERE 1=1 ");

            if (domains != null && domains.Count > 0)
            {
                cmdText.Append(" AND [Domain] IN( ");

                cmdText.AppendFormat(" '{0}' ", domains[0]);

                for (int i = 1; i < domains.Count; i++)
                {
                    cmdText.AppendFormat(" ,'{0}' ", domains[i]);
                }

                    cmdText.Append(" ) ");
            }

            if (sysId.HasValue)
            {
                cmdText.AppendFormat(" AND [SysId]<>{0} ", sysId.Value);
            }

            DbCommand cmd = this._db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(rdr[0].ToString());
                }
            }

            return items;
        }

        /// <summary>
        /// 获取域名信息集合
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.SystemDomain> GetDomains(int? sysId)
        {
            IList<EyouSoft.Model.SysStructure.SystemDomain> items = new List<EyouSoft.Model.SysStructure.SystemDomain>(0);

            DbCommand cmd = this._db.GetSqlStringCommand("SELECT 1");

            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("SELECT * FROM [tbl_SysDomain] WHERE 1=1");
            if (sysId.HasValue)
            {
                cmdText.Append(" AND [SysId]=@SysId ");
                this._db.AddInParameter(cmd, "SysId", DbType.Int32, sysId.Value);
            }
            cmd.CommandText = cmdText.ToString();

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.SysStructure.SystemDomain()
                    {
                        Domain = rdr["Domain"].ToString(),
                        SysId = rdr.GetInt32(rdr.GetOrdinal("SysId")),
                        Url = rdr["Url"].ToString(),
                        DomainType = (EyouSoft.Model.EnumType.SysStructure.DomainType)rdr.GetByte(rdr.GetOrdinal("Type"))
                    });
                }
            }

            return items;
        }
    }
}
