using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;
using System.Xml.Linq;

namespace EyouSoft.DAL.SupplierStructure
{
    public class SupplierRestaurant : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SupplierStructure.ISupplierRestaurant
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public SupplierRestaurant()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region static constants
        //static constants
        private const string SQL_INSERT_InsertRestaurantPertain = "INSERT INTO [tbl_SupplierRestaurant]([SupplierId],[Cuisine],[Introduce],[TourGuide])VALUES(@SupplierId,@Cuisine,@Introduce,@TourGuide)";
        private const string SQL_UPDATE_UpdateRestaurantPertain = "UPDATE [tbl_SupplierRestaurant] SET [Cuisine]=@Cuisine,[Introduce]=@Introduce,[TourGuide]=@TourGuide WHERE [SupplierId]=@SupplierId";
        private const string SQL_SELECT_GetRestaurantInfo = "SELECT * FROM [tbl_SupplierRestaurant] WHERE [SupplierId]=@SupplierId";
        #endregion

        #region private members
        /// <summary>
        /// 获取供应商联系人信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.SupplierContact> GetSupplierContactersByXml(string xml)
        {
            IList<EyouSoft.Model.CompanyStructure.SupplierContact> items = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();

            if (!string.IsNullOrEmpty(xml))
            {
                XElement xRoot = XElement.Parse(xml);
                var xContacters = Utils.GetXElements(xRoot, "row");

                foreach (var xContacter in xContacters)
                {
                    items.Add(new EyouSoft.Model.CompanyStructure.SupplierContact()
                    {
                        CompanyId = Utils.GetInt(Utils.GetXAttributeValue(xContacter, "CompanyId")),
                        ContactFax = Utils.GetXAttributeValue(xContacter, "ContactFax"),
                        ContactMobile = Utils.GetXAttributeValue(xContacter, "ContactMobile"),
                        ContactName = Utils.GetXAttributeValue(xContacter, "ContactName"),
                        ContactTel = Utils.GetXAttributeValue(xContacter, "ContactTel"),
                        Email = Utils.GetXAttributeValue(xContacter, "Email"),
                        Id = Utils.GetInt(Utils.GetXAttributeValue(xContacter, "Id")),
                        JobTitle = Utils.GetXAttributeValue(xContacter, "JobTitle"),
                        QQ = Utils.GetXAttributeValue(xContacter, "QQ"),
                        SupplierId = Utils.GetInt(Utils.GetXAttributeValue(xContacter, "SupplierId")),
                        SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Utils.GetInt(Utils.GetXAttributeValue(xContacter, "SupplierType"))
                    });
                }
            }

            return items;
        }
        #endregion

        #region ISupplierRestaurant 成员
        /// <summary>
        /// 写入餐馆供应商附加信息
        /// </summary>
        /// <param name="info">餐馆供应商信息业务实体</param>
        /// <returns></returns>
        public bool InsertRestaurantPertain(EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo info)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_InsertRestaurantPertain);
            this._db.AddInParameter(cmd, "SupplierId", DbType.Int32, info.Id);
            this._db.AddInParameter(cmd, "Cuisine", DbType.String, info.Cuisine);
            this._db.AddInParameter(cmd, "Introduce", DbType.String, info.Introduce);
            this._db.AddInParameter(cmd, "TourGuide", DbType.String, info.TourGuide);

            return DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
        }

        /// <summary>
        /// 更新餐馆供应商附加信息
        /// </summary>
        /// <param name="info">餐馆供应商信息业务实体</param>
        /// <returns></returns>
        public bool UpdateRestaurantPertain(EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo info)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_UpdateRestaurantPertain);            
            this._db.AddInParameter(cmd, "Cuisine", DbType.String, info.Cuisine);
            this._db.AddInParameter(cmd, "Introduce", DbType.String, info.Introduce);
            this._db.AddInParameter(cmd, "TourGuide", DbType.String, info.TourGuide);
            this._db.AddInParameter(cmd, "SupplierId", DbType.Int32, info.Id);

            return DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
        }

        /// <summary>
        /// 获取餐馆供应商附加信息(不含供应商基本信息)
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo GetRestaurantInfo(int supplierId)
        {
            EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetRestaurantInfo);
            this._db.AddInParameter(cmd, "SupplierId", DbType.Int32, supplierId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo()
                    {
                        Id = supplierId,
                        Cuisine = rdr["Cuisine"].ToString(),
                        Introduce = rdr["Introduce"].ToString(),
                        TourGuide = rdr["TourGuide"].ToString()
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 获取餐馆供应商信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo> GetRestaurants(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierRestaurantSearchInfo info)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo> items = new List<EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_CompanySupplier";
            string primaryKey = "Id";
            string orderByString = "Id DESC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("Id,ProvinceId,ProvinceName,CityId,CityName,UnitName,TradeNum,(SELECT Cuisine FROM tbl_SupplierRestaurant AS A WHERE tbl_CompanySupplier.Id=A.SupplierId) AS Cuisine");
            fields.Append(",(SELECT TOP 1 ContactName,ContactTel,ContactFax,ContactMobile FROM tbl_SupplierContact AS A WHERE A.SupplierId = tbl_CompanySupplier.[Id] FOR XML RAW,ROOT('ROOT')) AS Contacters ");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} AND IsDelete='0' AND SupplierType={1} ", companyId, (int)EyouSoft.Model.EnumType.CompanyStructure.SupplierType.餐馆);
            info = info ?? new EyouSoft.Model.SupplierStructure.SupplierRestaurantSearchInfo();
            if (info.CityId.HasValue)
            {
                cmdQuery.AppendFormat(" AND CityId={0} ", info.CityId.Value);
            }
            if (!string.IsNullOrEmpty(info.Name))
            {
                cmdQuery.AppendFormat(" AND UnitName LIKE '%{0}%' ", info.Name);
            }
            if (info.ProvinceId.HasValue)
            {
                cmdQuery.AppendFormat(" AND ProvinceId={0} ", info.ProvinceId.Value);
            }
            if (!string.IsNullOrEmpty(info.Cuisine))
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_SupplierRestaurant AS A WHERE A.SupplierId=tbl_CompanySupplier.Id AND A.Cuisine LIKE '%{0}%') ", info.Cuisine);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId")),
                        ProvinceName = rdr["ProvinceName"].ToString(),
                        CityId = rdr.GetInt32(rdr.GetOrdinal("CityId")),
                        CityName = rdr["CityName"].ToString(),
                        UnitName = rdr["UnitName"].ToString(),
                        TradeNum = rdr.GetInt32(rdr.GetOrdinal("TradeNum")),
                        SupplierContact = this.GetSupplierContactersByXml(rdr["Contacters"].ToString()),
                        Cuisine = rdr["Cuisine"].ToString()
                    });
                }
            }

            return items;
        }

        #endregion
    }
}
