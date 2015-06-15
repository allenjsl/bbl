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
    /// 城市管理DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class City : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICity
    {
        #region static constants
        private const string SQL_SELECT_ISEXISTS = "select count(*) from tbl_CompanyCity where CityName = @cityName and CompanyId = @companyId and Id != @Id";
        private const string SQL_INSERT_CITY = "insert into tbl_CompanyCity (ProvinceId,CityName,CompanyId,IsFav,OperatorId) values(@ProvinceId,@CityName,@CompanyId,@IsFav,@OperatorId)";
        private const string SQL_UPDATE_CITY = "update tbl_CompanyCity set ProvinceId=@ProvinceId,CityName = @CityName where Id = @Id";
        private const string SQL_SELECT_CITY = "select Id,ProvinceId,CityName,CompanyId,IsFav,OperatorId,IssueTime from tbl_CompanyCity where Id = @Id";
        private const string SQL_DELETE_CITY = "delete from tbl_CompanyCity ";
        private const string SQL_SetFav = "update tbl_CompanyCity set IsFav = @IsFav where Id = @Id";
        private const string SQL_GetList = "select Id,ProvinceId,CityName,CompanyId,IsFav,OperatorId,IssueTime from tbl_CompanyCity where CompanyId=@CompanyId and ProvinceId=@ProvinceId ";
        private const string SQL_GetAllList = "select Id,ProvinceId,CityName,CompanyId,IsFav,OperatorId,IssueTime from tbl_CompanyCity where CompanyId=@CompanyId and ProvinceId=@ProvinceId";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public City()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ICity 成员

        /// <summary>
        /// 验证城市名是否已经存在
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="cityId">城市编号</param>
        /// <returns>true:已存在 false:不存在</returns>
        public bool IsExists(string cityName, int companyId, int cityId)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_ISEXISTS);

            this._db.AddInParameter(cmd, "cityName", DbType.String, cityName);
            this._db.AddInParameter(cmd, "companyId", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, cityId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    isExists = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return isExists;
        }

        /// <summary>
        /// 添加城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.City model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_CITY);

            this._db.AddInParameter(cmd, "ProvinceId", DbType.String, model.ProvinceId);
            this._db.AddInParameter(cmd, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "IsFav", DbType.Boolean, model.IsFav);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.City model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_CITY);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(cmd, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取城市实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.City GetModel(int Id)
        {
            EyouSoft.Model.CompanyStructure.City cityModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_CITY);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    cityModel = new EyouSoft.Model.CompanyStructure.City();
                    cityModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    cityModel.ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId"));
                    cityModel.CityName = rdr.GetString(rdr.GetOrdinal("CityName"));
                    cityModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    cityModel.IsFav = Convert.ToBoolean(rdr.GetOrdinal("IsFav"));
                    cityModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    cityModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                }
            }

            return cityModel;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] Ids)
        {
            if (Ids == null || Ids.Length <= 0)
                return false;

            string strIds = string.Empty;
            foreach (int str in Ids)
            {
                strIds += "'" + str.ToString().Trim() + "',";
            }
            strIds = strIds.Trim(',');

            DbCommand dc = _db.GetSqlStringCommand(SQL_DELETE_CITY + " where Id in (" + strIds + ");");
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 设置是否常用
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="IsFav">是否常用</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetFav(int id, bool IsFav)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SetFav);
            this._db.AddInParameter(cmd, "IsFav", DbType.Int32, IsFav);
            this._db.AddInParameter(cmd, "Id", DbType.String, id);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ProvinceId">省份编号</param>
        /// <param name="IsFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.City> GetList(int CompanyId, int ProvinceId, bool? IsFav)
        {
            IList<EyouSoft.Model.CompanyStructure.City> lsCity = new List<EyouSoft.Model.CompanyStructure.City>();
            DbCommand cmd = null;

            if (IsFav == null)
            {
                cmd = this._db.GetSqlStringCommand(SQL_GetList);
            }
            else
            {
                cmd = this._db.GetSqlStringCommand(SQL_GetList + " and IsFav=@IsFav ");
                this._db.AddInParameter(cmd, "IsFav", DbType.String, IsFav == true ? "1" : "0");
            }

            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);
            this._db.AddInParameter(cmd, "ProvinceId", DbType.Int32, ProvinceId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                EyouSoft.Model.CompanyStructure.City cityModel = null;

                while (rdr.Read())
                {
                    cityModel = new EyouSoft.Model.CompanyStructure.City();
                    cityModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    cityModel.ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId"));
                    cityModel.CityName = rdr.GetString(rdr.GetOrdinal("CityName"));
                    cityModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    cityModel.IsFav = Convert.ToBoolean(rdr.GetOrdinal("IsFav"));
                    cityModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    cityModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    lsCity.Add(cityModel);
                }
            }

            return lsCity;
        }

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ProvinceId">省份编号</param>
        /// <param name="IsFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.City> GetList(int CompanyId, int? ProvinceId, bool? IsFav)
        {
            if (CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.CompanyStructure.City> lsCity = new List<EyouSoft.Model.CompanyStructure.City>();
            StringBuilder strSql = new StringBuilder(" select Id,ProvinceId,CityName,CompanyId,IsFav,OperatorId,IssueTime from tbl_CompanyCity where CompanyId=@CompanyId ");
            if (ProvinceId.HasValue && ProvinceId.Value > 0)
                strSql.AppendFormat(" and ProvinceId = {0} ", ProvinceId);
            if (IsFav.HasValue)
                strSql.AppendFormat("  and IsFav = {0} ", IsFav.Value ? "1" : "0");

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(dc, this._db))
            {
                EyouSoft.Model.CompanyStructure.City cityModel = null;

                while (rdr.Read())
                {
                    cityModel = new EyouSoft.Model.CompanyStructure.City();

                    cityModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    cityModel.ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId"));
                    cityModel.CityName = rdr.GetString(rdr.GetOrdinal("CityName"));
                    cityModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    cityModel.IsFav = Convert.ToBoolean(rdr.GetOrdinal("IsFav"));
                    cityModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    cityModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));

                    lsCity.Add(cityModel);
                }
            }

            return lsCity;
        }

        #endregion
    }
}
