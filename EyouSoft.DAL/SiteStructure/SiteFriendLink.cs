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
    /// 描述：同行平台 友情链接 数据类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class SiteFriendLink : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SiteStructure.ISiteFriendLink
    {
        private readonly Database DB = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteFriendLink()
        {
            DB = this.SystemStore;
        }
        /// <summary>
        /// 获得当前公司的友情链接列表
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SiteStructure.SiteFriendLink> GetSiteFriendLink(int CompanyId, int pageSize, int pageIndex, ref int recordCount)
        {
            IList<EyouSoft.Model.SiteStructure.SiteFriendLink> items = new List<EyouSoft.Model.SiteStructure.SiteFriendLink>();
            EyouSoft.Model.SiteStructure.SiteFriendLink item = null;
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_SiteFriendLink";
            string primaryKey = "LinkId";
            string orderByString = "SortId ASC";
            StringBuilder fields = new StringBuilder();
            #region 要查询的字段
            fields.Append("LinkId,CompanyId,LinkType,LinkName,FilePath,LinkUrl,SortId,CreateTime");
            #endregion
            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0}", CompanyId);
            #endregion
            using (IDataReader rdr = DbHelper.ExecuteReader(this.DB, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    item = new EyouSoft.Model.SiteStructure.SiteFriendLink()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("LinkId")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        LinkName = rdr["LinkName"].ToString(),
                        LinkUrl = rdr["LinkUrl"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        LinkType = (EyouSoft.Model.EnumType.SiteStructure.LinkType)(rdr.IsDBNull(rdr.GetOrdinal("LinkType")) ? 0 : rdr.GetByte(rdr.GetOrdinal("LinkType"))),
                        SortId = rdr.IsDBNull(rdr.GetOrdinal("SortId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("SortId")),
                        CreateTime = rdr.IsDBNull(rdr.GetOrdinal("CreateTime")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("CreateTime"))
                    };
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// 获得友情链接实体信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SiteStructure.SiteFriendLink GetSiteFriendLink(int Id, int CompanyId)
        {
            EyouSoft.Model.SiteStructure.SiteFriendLink SiteFriendLink = null;
            DbCommand dc = this.DB.GetSqlStringCommand(String.Format("SELECT * FROM [tbl_SiteFriendLink] WHERE [LinkId]={0} AND [CompanyId]={1}", Id, CompanyId));
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.DB))
            {
                while (rdr.Read())
                {
                    SiteFriendLink = new EyouSoft.Model.SiteStructure.SiteFriendLink()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("LinkId")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        LinkName = rdr["LinkName"].ToString(),
                        LinkUrl = rdr["LinkUrl"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        LinkType = (EyouSoft.Model.EnumType.SiteStructure.LinkType)(rdr.IsDBNull(rdr.GetOrdinal("LinkType")) ? 0 : rdr.GetByte(rdr.GetOrdinal("LinkType"))),
                        SortId = rdr.IsDBNull(rdr.GetOrdinal("SortId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("SortId")),
                        CreateTime = rdr.IsDBNull(rdr.GetOrdinal("CreateTime")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("CreateTime"))
                    };
                }
            }
            return SiteFriendLink;
        }

        /// <summary>
        /// 添加友情链接
        /// </summary>
        /// <param name="SiteFriendLink"></param>
        /// <returns></returns>
        public bool AddFriendLink(EyouSoft.Model.SiteStructure.SiteFriendLink SiteFriendLink)
        {
            string SQL = "INSERT INTO [tbl_SiteFriendLink]([CompanyId],[LinkName],[LinkUrl],[FilePath],[LinkType],[SortId]) VALUES(@CompanyId,@LinkName,@LinkUrl,@FilePath,@LinkType,@SortId)";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, SiteFriendLink.CompanyId);
            this.DB.AddInParameter(dc, "LinkUrl", DbType.String, SiteFriendLink.LinkUrl);
            this.DB.AddInParameter(dc, "LinkName", DbType.String, SiteFriendLink.LinkName);
            this.DB.AddInParameter(dc, "FilePath", DbType.String, SiteFriendLink.FilePath);
            this.DB.AddInParameter(dc, "LinkType", DbType.Byte, (int)SiteFriendLink.LinkType);
            this.DB.AddInParameter(dc, "SortId", DbType.Int32, SiteFriendLink.SortId);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 修改友情链接
        /// </summary>
        /// <param name="SiteFriendLink"></param>
        /// <returns></returns>
        public bool UpdateFriendLink(EyouSoft.Model.SiteStructure.SiteFriendLink SiteFriendLink)
        {
            string SQL = "UPDATE [tbl_SiteFriendLink] SET [LinkName]=@LinkName,[LinkUrl]=@LinkUrl,[FilePath]=@FilePath,[LinkType]=@LinkType,[SortId]=@SortId WHERE [LinkId]=@LinkId AND [CompanyId]=@CompanyId";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "LinkId", DbType.Int32, SiteFriendLink.Id);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, SiteFriendLink.CompanyId);
            this.DB.AddInParameter(dc, "LinkUrl", DbType.String, SiteFriendLink.LinkUrl);
            this.DB.AddInParameter(dc, "LinkName", DbType.String, SiteFriendLink.LinkName);
            this.DB.AddInParameter(dc, "FilePath", DbType.String, SiteFriendLink.FilePath);
            this.DB.AddInParameter(dc, "LinkType", DbType.Byte, (int)SiteFriendLink.LinkType);
            this.DB.AddInParameter(dc, "SortId", DbType.Int32, SiteFriendLink.SortId);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DelFriendLink(int Id, int CompanyId)
        {
            string SQL = @"INSERT INTO [tbl_SysDeletedFileQue] ([FilePath]) select FilePath FROM [tbl_SiteFriendLink] WHERE [LinkId]=@LinkId AND [CompanyId]=@CompanyId and FilePath is not null and len(FilePath) > 1; DELETE FROM [tbl_SiteFriendLink] WHERE [LinkId]=@LinkId AND [CompanyId]=@CompanyId;";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "LinkId", DbType.Int32, Id);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }
    }
}
