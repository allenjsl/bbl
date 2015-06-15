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
    /// 描述：同行平台 轮换图片 数据类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class SiteChangePic : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SiteStructure.ISiteChangePic
    {
        private readonly Database DB = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteChangePic()
        {
            DB = this.SystemStore;
        }

        /// <summary>
        /// 得到轮换图片列表
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SiteStructure.SiteChangePic> GetSiteChange(int CompanyId, int pageSize, int pageIndex, ref int recordCount)
        {
            IList<EyouSoft.Model.SiteStructure.SiteChangePic> items = new List<EyouSoft.Model.SiteStructure.SiteChangePic>();
            EyouSoft.Model.SiteStructure.SiteChangePic item = null;
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_SiteAttach";
            string primaryKey = "AttachId";
            string orderByString = "SortId ASC";
            StringBuilder fields = new StringBuilder();
            #region 要查询的字段
            fields.Append("AttachId,CompanyId,ItemId,ItemType,FilePath,FileName,URL,SortId");
            #endregion
            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0}", CompanyId);
            #endregion
            using (IDataReader rdr = DbHelper.ExecuteReader(this.DB, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    item = new EyouSoft.Model.SiteStructure.SiteChangePic()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("AttachId")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        FileName = rdr["FileName"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        ItemId = rdr.IsDBNull(rdr.GetOrdinal("ItemId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ItemId")),
                        ItemType = (EyouSoft.Model.EnumType.SiteStructure.ItemType)(rdr.IsDBNull(rdr.GetOrdinal("ItemType")) ? 0 : rdr.GetByte(rdr.GetOrdinal("ItemType"))),
                        SortId = rdr.IsDBNull(rdr.GetOrdinal("SortId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("SortId")),
                        URL = rdr["URL"].ToString()
                    };
                    items.Add(item);
                }
            }
            return items;
        }

        public IList<EyouSoft.Model.SiteStructure.SiteChangePic> GetSiteChange(int CompanyId)
        {
            IList<EyouSoft.Model.SiteStructure.SiteChangePic> items = new List<EyouSoft.Model.SiteStructure.SiteChangePic>();
            EyouSoft.Model.SiteStructure.SiteChangePic item = null;
            DbCommand dc = this.DB.GetSqlStringCommand(String.Format("SELECT * FROM [tbl_SiteAttach] WHERE [CompanyId]={0}", CompanyId));
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.DB))
            {
                {
                    while (rdr.Read())
                    {
                        item = new EyouSoft.Model.SiteStructure.SiteChangePic()
                        {
                            Id = rdr.GetInt32(rdr.GetOrdinal("AttachId")),
                            CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            FileName = rdr["FileName"].ToString(),
                            FilePath = rdr["FilePath"].ToString(),
                            ItemId = rdr.IsDBNull(rdr.GetOrdinal("ItemId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ItemId")),
                            ItemType = (EyouSoft.Model.EnumType.SiteStructure.ItemType)(rdr.IsDBNull(rdr.GetOrdinal("ItemType")) ? 0 : rdr.GetByte(rdr.GetOrdinal("ItemType"))),
                            SortId = rdr.IsDBNull(rdr.GetOrdinal("SortId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("SortId")),
                            URL = rdr["URL"].ToString()
                        };
                        items.Add(item);
                    }
                }
                return items;
            }
        }
        /// <summary>
        /// 得到轮换图片实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <param name="companyId">专线公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SiteStructure.SiteChangePic GetSiteChange(int Id,int CompanyId)
        {
            EyouSoft.Model.SiteStructure.SiteChangePic SiteChangePic = null;
            DbCommand dc = this.DB.GetSqlStringCommand(String.Format("SELECT * FROM [tbl_SiteAttach] WHERE [AttachId]={0} AND [CompanyId]={1}", Id, CompanyId));
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.DB))
            {
                while (rdr.Read())
                {
                    SiteChangePic = new EyouSoft.Model.SiteStructure.SiteChangePic()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("AttachId")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        FileName = rdr["FileName"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        ItemId = rdr.IsDBNull(rdr.GetOrdinal("ItemId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ItemId")),
                        ItemType = (EyouSoft.Model.EnumType.SiteStructure.ItemType)(rdr.IsDBNull(rdr.GetOrdinal("ItemType")) ? 0 : rdr.GetByte(rdr.GetOrdinal("ItemType"))),
                        SortId = rdr.IsDBNull(rdr.GetOrdinal("SortId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("SortId")),
                        URL = rdr["URL"].ToString()
                    };
                }
            }
            return SiteChangePic;
        }

        /// <summary>
        /// 添加轮换图片
        /// </summary>
        /// <param name="SiteChangePic"></param>
        /// <returns></returns>
        public bool AddSiteChangePic(EyouSoft.Model.SiteStructure.SiteChangePic SiteChangePic)
        {
            string SQL = "INSERT INTO [tbl_SiteAttach]([CompanyId],[FileName],[FilePath],[ItemId],[ItemType],[SortId],[URL]) VALUES(@CompanyId,@FileName,@FilePath,@ItemId,@ItemType,@SortId,@URL)";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, SiteChangePic.CompanyId);
            this.DB.AddInParameter(dc, "FileName", DbType.String, SiteChangePic.FileName);
            this.DB.AddInParameter(dc, "FilePath", DbType.String, SiteChangePic.FilePath);
            this.DB.AddInParameter(dc, "ItemId", DbType.Int32, SiteChangePic.ItemId);
            this.DB.AddInParameter(dc, "ItemType", DbType.Byte, (int)SiteChangePic.ItemType);
            this.DB.AddInParameter(dc, "SortId", DbType.Int32, SiteChangePic.SortId);
            this.DB.AddInParameter(dc, "URL", DbType.String, SiteChangePic.URL);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 修改轮换图片
        /// </summary>
        /// <param name="SiteChangePic"></param>
        /// <returns></returns>
        public bool UpdateSiteChangePic(EyouSoft.Model.SiteStructure.SiteChangePic SiteChangePic)
        {
            string SQL = "UPDATE [tbl_SiteAttach] SET [FileName]=@FileName,[FilePath]=@FilePath,[ItemId]=@ItemId,[ItemType]=@ItemType,[SortId]=@SortId,[URL]=@URL WHERE [AttachId]=@AttachId AND [CompanyId]=@CompanyId";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "AttachId", DbType.Int32, SiteChangePic.Id);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, SiteChangePic.CompanyId);
            this.DB.AddInParameter(dc, "FileName", DbType.String, SiteChangePic.FileName);
            this.DB.AddInParameter(dc, "FilePath", DbType.String, SiteChangePic.FilePath);
            this.DB.AddInParameter(dc, "ItemId", DbType.Int32, SiteChangePic.ItemId);
            this.DB.AddInParameter(dc, "ItemType", DbType.Byte, (int)SiteChangePic.ItemType);
            this.DB.AddInParameter(dc, "SortId", DbType.Int32, SiteChangePic.SortId);
            this.DB.AddInParameter(dc, "URL", DbType.String, SiteChangePic.URL);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 删除轮换图片
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public bool DelSiteChangePic(int Id,int CompanyId)
        {
            string SQL = "DELETE FROM [tbl_SiteAttach] WHERE [AttachId]=@AttachId AND [CompanyId]=@CompanyId";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "AttachId", DbType.Int32, Id);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }
    }
}
