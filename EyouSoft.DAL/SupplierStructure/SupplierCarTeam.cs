using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;
using System.Data;
using System.Xml.Linq;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.SupplierStructure
{
    /// <summary>
    /// 车队供应商信息数据访问类
    /// </summary>
    /// Author:毛坤 2011-03-08
    public class SupplierCarTeam : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SupplierStructure.ISupplierCarTeam
    {
        #region static constants

        private readonly Database _db = null;

        private const string SQL_INSERT_InsertCarsInfo = "INSERT INTO [tbl_SupplierCarInfo]([CarType],[CarNumber],[Image],[Price],[DriverName],[DriverPhone] ,[GuideWord],[PrivaderId])VALUES(@CarType{0},@CarNumber{0},@Image{0},@Price{0},@DriverName{0},@DriverPhone{0},@GuideWord{0},@PrivaderId{0});";
        private const string SQL_DELETE_DeleteCars = "DELETE FROM [tbl_SupplierCarInfo] WHERE [PrivaderId]=@SupplierId";
        private const string SQL_SELECT_GetCarTeamAttachInfo = "SELECT * FROM [tbl_CompanySupplier] WHERE [Id]=@SupplierId;SELECT * FROM [tbl_SupplierCarInfo] WHERE [PrivaderId]=@SupplierId ORDER BY [Id] ASC";
        
        #endregion 构造函数

        public SupplierCarTeam()
        {
            this._db = base.SystemStore;
        }


        #region private members
        /// <summary>
        /// 获取供应商联系人信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.SupplierContact> GetSupplierContactersByXml(string xml)
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

        private IList<EyouSoft.Model.SupplierStructure.SupplierCarInfo> GetSupplierCarsByXml(string xml)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierCarInfo> items = new List<EyouSoft.Model.SupplierStructure.SupplierCarInfo>();

            if (!string.IsNullOrEmpty(xml))
            {
                XElement xRoot = XElement.Parse(xml);
                var xContacters = Utils.GetXElements(xRoot, "row");

                foreach (var xContacter in xContacters)
                {
                    items.Add(new EyouSoft.Model.SupplierStructure.SupplierCarInfo()
                    {
                        CarType = Utils.GetXAttributeValue(xContacter, "CarType"),
                        CarNumber = Utils.GetXAttributeValue(xContacter, "CarNumber"),
                        Image = Utils.GetXAttributeValue(xContacter, "Image"),
                        Price = Utils.GetDecimal(Utils.GetXAttributeValue(xContacter, "Price")),
                        DriverName = Utils.GetXAttributeValue(xContacter, "DriverName"),
                        DriverPhone = Utils.GetXAttributeValue(xContacter, "DriverPhone"),
                        GuideWord = Utils.GetXAttributeValue(xContacter, "GuideWord"),
                        PrivaderId = Utils.GetInt(Utils.GetXAttributeValue(xContacter, "PrivaderId"))
                    });
                }
            }

            return items;
        }
        #endregion


        #region ISupplierCarTeam 成员

        /// <summary>
        /// 添加供应商车队信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCarTeamAttach(EyouSoft.Model.SupplierStructure.SupplierCarTeam model)
        {
            return true;
        }

        /// <summary>
        /// 修改供应商车队信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCarTeamAttach(EyouSoft.Model.SupplierStructure.SupplierCarTeam model)
        {
            return true;
        }

        /// <summary>
        /// 插入车队供应商的车辆信息集合
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="carsInfo"></param>
        /// <returns></returns>
        public bool InsertCars(int supplierId, IList<EyouSoft.Model.SupplierStructure.SupplierCarInfo> carsInfo)
        {
            if (carsInfo == null) return false;
            if (supplierId < 1) return false;

            DbCommand cmd = this._db.GetSqlStringCommand("SELECT 1");
            StringBuilder cmdText = new StringBuilder();

            int i = 0;
            foreach (var item in carsInfo)
            {
                cmdText.AppendFormat(SQL_INSERT_InsertCarsInfo, i.ToString());
                this._db.AddInParameter(cmd, string.Format("CarType{0}", i.ToString()), DbType.String, item.CarType);
                this._db.AddInParameter(cmd, string.Format("CarNumber{0}", i.ToString()), DbType.String, item.CarNumber);
                this._db.AddInParameter(cmd, string.Format("Image{0}", i.ToString()), DbType.String, item.Image);
                this._db.AddInParameter(cmd, string.Format("Price{0}", i.ToString()), DbType.Decimal, item.Price);
                this._db.AddInParameter(cmd, string.Format("DriverName{0}", i.ToString()), DbType.String, item.DriverName);
                this._db.AddInParameter(cmd, string.Format("DriverPhone{0}", i.ToString()), DbType.String, item.DriverPhone);
                this._db.AddInParameter(cmd, string.Format("GuideWord{0}", i.ToString()), DbType.String, item.GuideWord);
                this._db.AddInParameter(cmd, string.Format("PrivaderId{0}", i.ToString()), DbType.Int32, supplierId);
                i++;
            }

            cmd.CommandText = cmdText.ToString();

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 删除车队供应商车辆信息
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        public bool DeleteCars(int supplierId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_DeleteCars);
            this._db.AddInParameter(cmd, "SupplierId", DbType.Int32, supplierId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取车队供应商信息(含车辆信息集合,不含供应商基本信息)
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierCarTeam GetCarTeamAttachInfo(int supplierId)
        {
            EyouSoft.Model.SupplierStructure.SupplierCarTeam info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetCarTeamAttachInfo);
            this._db.AddInParameter(cmd, "SupplierId", DbType.Int32, supplierId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.SupplierStructure.SupplierCarTeam()
                    {
                        Id = supplierId,
                        CarsInfo = new List<EyouSoft.Model.SupplierStructure.SupplierCarInfo>()
                    };
                }

                if (info != null && rdr.NextResult())
                {
                    while (rdr.Read())
                    {
                        info.CarsInfo.Add(new EyouSoft.Model.SupplierStructure.SupplierCarInfo()
                        {
                            CarType = rdr["CarType"].ToString(),
                            CarNumber = rdr["CarNumber"].ToString(),
                            Image = rdr["Image"].ToString(),
                            Price = rdr.IsDBNull(rdr.GetOrdinal("Price")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("Price")),
                            DriverName = rdr["DriverName"].ToString(),
                            DriverPhone = rdr["DriverPhone"].ToString(),
                            GuideWord = rdr["GuideWord"].ToString(),
                            PrivaderId = rdr.IsDBNull(rdr.GetOrdinal("PrivaderId")) ? 0 : (rdr.GetOrdinal("PrivaderId"))
                        });
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// 获取车队供应商信息集合
        /// </summary>
        /// <param name="companyId">车队供应商编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierCarTeam> GetCarTeams(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierCarTeamSearchInfo info)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierCarTeam> items = new List<EyouSoft.Model.SupplierStructure.SupplierCarTeam>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_CompanySupplier";
            string primaryKey = "Id";
            string orderByString = "Id DESC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("Id,ProvinceId,ProvinceName,CityId,CityName,UnitName,TradeNum");
            fields.Append(",(SELECT TOP 1 ContactName,ContactTel,ContactFax FROM tbl_SupplierContact AS A WHERE A.SupplierId = tbl_CompanySupplier.[Id] FOR XML RAW,ROOT('ROOT')) AS Contacters ");
            fields.Append(",(SELECT TOP 1 CarType FROM tbl_SupplierCarInfo AS A WHERE A.PrivaderId = tbl_CompanySupplier.[Id] FOR XML RAW,ROOT('ROOT')) AS Cars ");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} AND IsDelete='0' AND SupplierType={1} ", companyId, (int)EyouSoft.Model.EnumType.CompanyStructure.SupplierType.车队);
            info = info ?? new EyouSoft.Model.SupplierStructure.SupplierCarTeamSearchInfo();
            if (info.CityId.HasValue)
            {
                if (info.CityId.Value != 0)
                {
                    cmdQuery.AppendFormat(" AND CityId={0} ", info.CityId.Value);
                }
            }
            if (!string.IsNullOrEmpty(info.Name))
            {
                cmdQuery.AppendFormat(" AND UnitName LIKE '%{0}%' ", info.Name);
            }
            if (info.ProvinceId.HasValue)
            {
                if (info.ProvinceId.Value != 0)
                {
                    cmdQuery.AppendFormat(" AND ProvinceId={0} ", info.ProvinceId.Value);
                }
            }
            if (!string.IsNullOrEmpty(info.CarType))
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_SupplierCarInfo AS A WHERE A.PrivaderId=tbl_CompanySupplier.Id AND CarType='{0}') ", info.CarType);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.SupplierStructure.SupplierCarTeam()
                    {
                        Id = rdr.IsDBNull(rdr.GetOrdinal("Id")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("Id")),
                        ProvinceId = rdr.IsDBNull(rdr.GetOrdinal("ProvinceId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ProvinceId")),
                        ProvinceName = rdr["ProvinceName"].ToString(),
                        CityId = rdr.IsDBNull(rdr.GetOrdinal("CityId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CityId")),
                        CityName = rdr["CityName"].ToString(),
                        UnitName = rdr["UnitName"].ToString(),
                        TradeNum = rdr.IsDBNull(rdr.GetOrdinal("TradeNum")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("TradeNum")),
                        SupplierContact = this.GetSupplierContactersByXml(rdr["Contacters"].ToString()),
                        CarsInfo = this.GetSupplierCarsByXml(rdr["Cars"].ToString())
                    });
                }
            }

            return items;
        }

        #endregion
    }
}
