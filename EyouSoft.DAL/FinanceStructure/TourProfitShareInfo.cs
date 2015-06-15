using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data;
namespace EyouSoft.DAL.FinanceStructure
{
    /// <summary>
    /// 财务管理-团队利润分配数据层
    /// </summary>
    /// 鲁功源 2011-01-22
    public class TourProfitShareInfo : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.FinanceStructure.ITourProfitShareInfo
    {
        #region 私有变量
        private readonly Database _db = null;
        private const string Sql_TourShare_Update = "UPDATE [tbl_TourShare] SET [ShareCost] = {0},[Remark] = '{1}' WHERE [CompanyId]={2} and [TourId]='{3}' and [Id]='{4}'";
        private const string Sql_TourShare_GetModel = "SELECT [Id],[CompanyId],[TourId],[ShareItem],[ShareCost],[DepartmentId],[SaleId],[Remark],[CreateTime],[OperatorId],(select ContactName from tbl_CompanyUser where Id=tbl_TourShare.SaleId) as Saler,(select DepartName from tbl_CompanyDepartment where Id=tbl_TourShare.DepartmentId) as DepartmentName FROM [tbl_TourShare] where Id=@Id";
        private const string Sql_TourShare_GetList = "SELECT [Id],[CompanyId],[TourId],[ShareItem],[ShareCost],[DepartmentId],[SaleId],[Remark],[CreateTime],[OperatorId],(select ContactName from tbl_CompanyUser where Id=tbl_TourShare.SaleId) as Saler,(select DepartName from tbl_CompanyDepartment where Id=tbl_TourShare.DepartmentId) as DepartmentName FROM [tbl_TourShare] where TourId=@TourId";
        private const string Sql_TourShare_Insert = "INSERT INTO [tbl_TourShare]([Id],[CompanyId],[TourId],[ShareItem],[ShareCost],[DepartmentId],[SaleId],[Remark],[CreateTime],[OperatorId]) VALUES(newid(),{0},'{1}','{2}',{3},{4},{5},'{6}',getdate(),{7});";
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public TourProfitShareInfo()
        {
            _db = this.SystemStore;
        }

        #endregion

        #region ITourProfitShareInfo 成员
        /// <summary>
        /// 添加团队利润分配
        /// </summary>
        /// <param name="list">团队利润分配集合</param>
        /// <returns>1：成功 其它:失败</returns>
        public int Add(IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (EyouSoft.Model.FinanceStructure.TourProfitShareInfo model in list)
            {
                strSql.AppendFormat(Sql_TourShare_Insert, model.CompanyId, model.TourId, model.ShareItem,
                    model.ShareCost, model.DepartmentId, model.SaleId, model.Remark, model.OperatorId);
            }
            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSqlTrans(dc, this._db);
        }
        /// <summary>
        /// 修改团队利润分配
        /// </summary>
        /// <param name="list">团队利润分配集合</param>
        /// <returns>1：成功 其它:失败</returns>
        public int Update(IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (EyouSoft.Model.FinanceStructure.TourProfitShareInfo model in list)
            {
                    strSql.AppendFormat(Sql_TourShare_Update, model.ShareCost,model.Remark, model.CompanyId, model.TourId, model.ShareId);
            }
            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? 1 : 0;
        }
        /// <summary>
        /// 获取指定团队下的所有团队利润分配列表。
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <returns>团队利润分配列表</returns>
        public IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> GetList(string TourId)
        {
            IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list = new List<EyouSoft.Model.FinanceStructure.TourProfitShareInfo>();
            DbCommand dc = this._db.GetSqlStringCommand(Sql_TourShare_GetList);
            this._db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, TourId);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.FinanceStructure.TourProfitShareInfo model = new EyouSoft.Model.FinanceStructure.TourProfitShareInfo();
                    model.CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CreateTime")))
                        model.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    model.DepartmentId = dr.IsDBNull(dr.GetOrdinal("DepartmentId")) ? 0 : dr.GetInt32(dr.GetOrdinal("DepartmentId"));
                    model.DepartmentName = dr.IsDBNull(dr.GetOrdinal("DepartmentName")) ? string.Empty : dr.GetString(dr.GetOrdinal("DepartmentName"));
                    model.OperatorId = dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.Remark = dr.IsDBNull(dr.GetOrdinal("Remark")) ? string.Empty : dr.GetString(dr.GetOrdinal("Remark"));
                    model.SaleId = dr.IsDBNull(dr.GetOrdinal("SaleId")) ? 0 : dr.GetInt32(dr.GetOrdinal("SaleId"));
                    model.Saler = dr.IsDBNull(dr.GetOrdinal("Saler")) ? string.Empty : dr.GetString(dr.GetOrdinal("Saler"));
                    model.ShareCost = dr.IsDBNull(dr.GetOrdinal("ShareCost")) ? 0 : dr.GetDecimal(dr.GetOrdinal("ShareCost"));
                    model.ShareId = dr.IsDBNull(dr.GetOrdinal("Id")) ? string.Empty : dr.GetString(dr.GetOrdinal("Id"));
                    model.ShareItem = dr.IsDBNull(dr.GetOrdinal("ShareItem")) ? string.Empty : dr.GetString(dr.GetOrdinal("ShareItem"));
                    model.TourId = dr.IsDBNull(dr.GetOrdinal("TourId")) ? string.Empty : dr.GetString(dr.GetOrdinal("TourId"));
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }
        /// <summary>
        /// 获取团队利润分配实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>团队利润分配实体</returns>
        public EyouSoft.Model.FinanceStructure.TourProfitShareInfo GetModel(string Id)
        {
            EyouSoft.Model.FinanceStructure.TourProfitShareInfo model = null;
            DbCommand dc = this._db.GetSqlStringCommand(Sql_TourShare_GetModel);
            this._db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, Id);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    model = new EyouSoft.Model.FinanceStructure.TourProfitShareInfo();
                    model.CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CreateTime")))
                        model.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    model.DepartmentId = dr.IsDBNull(dr.GetOrdinal("DepartmentId")) ? 0 : dr.GetInt32(dr.GetOrdinal("DepartmentId"));
                    model.DepartmentName = dr.IsDBNull(dr.GetOrdinal("DepartmentName")) ? string.Empty : dr.GetString(dr.GetOrdinal("DepartmentName"));
                    model.OperatorId = dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.Remark = dr.IsDBNull(dr.GetOrdinal("Remark")) ? string.Empty : dr.GetString(dr.GetOrdinal("Remark"));
                    model.SaleId = dr.IsDBNull(dr.GetOrdinal("SaleId")) ? 0 : dr.GetInt32(dr.GetOrdinal("SaleId"));
                    model.Saler = dr.IsDBNull(dr.GetOrdinal("Saler")) ? string.Empty : dr.GetString(dr.GetOrdinal("Saler"));
                    model.ShareCost = dr.IsDBNull(dr.GetOrdinal("ShareCost")) ? 0 : dr.GetDecimal(dr.GetOrdinal("ShareCost"));
                    model.ShareId = dr.IsDBNull(dr.GetOrdinal("ShareId")) ? string.Empty : dr.GetString(dr.GetOrdinal("ShareId"));
                    model.ShareItem = dr.IsDBNull(dr.GetOrdinal("ShareItem")) ? string.Empty : dr.GetString(dr.GetOrdinal("ShareItem"));
                    model.TourId = dr.IsDBNull(dr.GetOrdinal("TourId")) ? string.Empty : dr.GetString(dr.GetOrdinal("TourId"));
                }
            }
            return model;
        }
        #endregion
    }
}
