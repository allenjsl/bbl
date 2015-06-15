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
    /// 省份管理DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class Province : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.IProvince
    {
        #region static constants
        private const string SQL_SELECT_ISEXISTS = "select count(*) from tbl_CompanyProvince where ProvinceName = @ProvinceName and CompanyId = @CompanyId and Id != @Id";
        private const string SQL_INSERT_PROVINCE = "insert into tbl_CompanyProvince (ProvinceName,CompanyId,OperatorId) values(@ProvinceName,@CompanyId,@OperatorId)";
        private const string SQL_UPDATE_PROVINC = "update tbl_CompanyProvince set ProvinceName = @ProvinceName where Id = @Id";
        private const string SQL_SELECT_PROVINC = "select Id,ProvinceName,CompanyId,OperatorId,IssueTime from tbl_CompanyProvince where Id = @Id";
        private const string SQL_DELETE_PROVINC = "delete from tbl_CompanyProvince ";
        private const string SQL_GetList = "select Id,ProvinceName,CompanyId,OperatorId,IssueTime from tbl_CompanyProvince where CompanyId=@CompanyId";
        //private const string SQL_GetProvinceInfo = "select a.Id as ProvinceId,a.ProvinceName,isnull(b.Id,'') as CityId,isnull(b.CityName,'') as CityName,isnull(b.IsFav,'') as IsFav,a.CompanyId,a.OperatorId,a.IssueTime"
        //                                            +" from tbl_CompanyProvince a left join tbl_CompanyCity b"
        //                                            + " on a.Id = b.ProvinceId where a.CompanyId = @CompanyId";
        private const string SQL_GetCityByPid = "select isnull(Id,0) as CityId,isnull(CityName,'') as CityName,IsFav from tbl_CompanyCity where ProvinceId = @ProvinceId";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public Province()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region IProvince 成员

        /// <summary>
        /// 验证省份名是否已经存在
        /// </summary>
        /// <param name="provinceName">省份名称</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>true:已存在 false:不存在</returns>
        public bool IsExists(string provinceName, int companyId,int provinceId)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_ISEXISTS);

            this._db.AddInParameter(cmd, "ProvinceName", DbType.String, provinceName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, provinceId);

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
        /// 添加省份
        /// </summary>
        /// <param name="model">省份实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.Province model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_PROVINCE);

            this._db.AddInParameter(cmd, "ProvinceName", DbType.String, model.ProvinceName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.String, model.CompanyId);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改省份
        /// </summary>
        /// <param name="model">省份实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.Province model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_PROVINC);
            this._db.AddInParameter(cmd, "ProvinceName", DbType.String, model.ProvinceName);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取省份实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.Province GetModel(int Id)
        {
            EyouSoft.Model.CompanyStructure.Province provinceModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_PROVINC);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    provinceModel = new EyouSoft.Model.CompanyStructure.Province();
                    provinceModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    provinceModel.ProvinceName = rdr.GetString(rdr.GetOrdinal("ProvinceName"));
                    provinceModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    provinceModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    provinceModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                }
            }

            return provinceModel;
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

            DbCommand dc = _db.GetSqlStringCommand("delete from tbl_CompanyCity where ProvinceId in (" + strIds + ");" + SQL_DELETE_PROVINC + " where Id in (" + strIds + ");");
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取指定公司的省份集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>省份集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.Province> GetList(int CompanyId)
        {
            IList<EyouSoft.Model.CompanyStructure.Province> lsProvince = new List<EyouSoft.Model.CompanyStructure.Province>();

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetList);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                EyouSoft.Model.CompanyStructure.Province provinceModel = null;

                while(rdr.Read())
                {
                    provinceModel = new EyouSoft.Model.CompanyStructure.Province();
                    provinceModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    provinceModel.ProvinceName = rdr.GetString(rdr.GetOrdinal("ProvinceName"));
                    provinceModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    provinceModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    provinceModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    lsProvince.Add(provinceModel);
                }
            }

            return lsProvince;
        }

        /// <summary>
        /// 获取某个公司所有省份的信息包括城市
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Province> GetProvinceInfo(int CompanyId)
        {
            IList<EyouSoft.Model.CompanyStructure.Province> lsProvince = new List<EyouSoft.Model.CompanyStructure.Province>();
            IList<EyouSoft.Model.CompanyStructure.City> lsCity = null;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetList);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                EyouSoft.Model.CompanyStructure.Province provinceModel = null;

                while (rdr.Read())
                {
                    //a.Id as ProvinceId,a.ProvinceName,b.Id as CityId,b.CityName,b.IsFav,a.CompanyId,a.OperatorId,a.IssueTime
                    provinceModel = new EyouSoft.Model.CompanyStructure.Province();
                    provinceModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    provinceModel.ProvinceName = rdr.GetString(rdr.GetOrdinal("ProvinceName"));
                    provinceModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    provinceModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    provinceModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));

                    DbCommand cmd1 = this._db.GetSqlStringCommand(SQL_GetCityByPid);
                    this._db.AddInParameter(cmd1, "ProvinceId", DbType.Int32, provinceModel.Id);
                    using (IDataReader rdr1 = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd1, this._db))
                    {
                        lsCity = new List<EyouSoft.Model.CompanyStructure.City>();
                        EyouSoft.Model.CompanyStructure.City cityModel = null;

                        while (rdr1.Read())
                        {
                            cityModel = new EyouSoft.Model.CompanyStructure.City();
                            cityModel.Id = rdr1.GetInt32(rdr1.GetOrdinal("CityId"));
                            cityModel.CityName = rdr1.GetString(rdr1.GetOrdinal("CityName"));
                            cityModel.IsFav = rdr1.GetString(rdr1.GetOrdinal("IsFav")) == "1";
                            lsCity.Add(cityModel);
                        }
                    }
                    provinceModel.CityList = lsCity;
                    lsProvince.Add(provinceModel);
                }
            }

            return lsProvince;
        }

        /// <summary>
        /// 获取有常用城市的省份列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Province> GetHasFavCityProvince(int companyId)
        {
            string sql = string.Format("select distinct p.* from tbl_CompanyProvince p inner join tbl_CompanyCity c"
                                      +" on p.Id = c.ProvinceId where c.IsFav = '1' and p.CompanyId = {0} "
                                      +" order by p.IssueTime desc", companyId);
            IList<EyouSoft.Model.CompanyStructure.Province> lsProvince = new List<EyouSoft.Model.CompanyStructure.Province>();
            EyouSoft.Model.CompanyStructure.Province model = null;
            DbCommand cmd = this._db.GetSqlStringCommand(sql);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.Province();
                    model.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    model.ProvinceName = rdr.GetString(rdr.GetOrdinal("ProvinceName"));
                    model.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    model.OperatorId = rdr.IsDBNull(rdr.GetOrdinal("OperatorId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    lsProvince.Add(model);
                }
            }

            return lsProvince;
        }

        #endregion
    }
}
