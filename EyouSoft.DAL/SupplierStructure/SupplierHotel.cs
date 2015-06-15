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
    /// <summary>
    /// 酒店供应商信息数据访问类接口
    /// </summary>
    /// Author:汪奇志 2011-03-08
    public class SupplierHotel : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SupplierStructure.ISupplierHotel
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public SupplierHotel()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region static constants
        //static constants
        private const string SQL_INSERT_InsertHotelPertain = "INSERT INTO [tbl_SupplierHotel]([SupplierId],[Star],[Introduce],[TourGuide])VALUES(@SupplierId,@Star,@Introduce,@TourGuide)";
        private const string SQL_UPDATE_UpdateHotelPertain = "UPDATE [tbl_SupplierHotel] SET [Star]=@Star,[Introduce]=@Introduce,[TourGuide]=@TourGuide WHERE [SupplierId]=@SupplierId";
        private const string SQL_INSERT_InsertRoomType = "INSERT INTO [tbl_SupplierHotelRoomType]([SupplierId],[Name],[SellingPrice],[AccountingPrice],[IsBreakfast])VALUES(@SupplierId{0},@Name{0},@SellingPrice{0},@AccountingPrice{0},@IsBreakfast{0});";
        private const string SQL_DELETE_DeleteRoomType = "DELETE FROM [tbl_SupplierHotelRoomType] WHERE [SupplierId]=@SupplierId";
        private const string SQL_SELECT_GetHotelPertainInfo = "SELECT * FROM [tbl_SupplierHotel] WHERE [SupplierId]=@SupplierId;SELECT * FROM [tbl_SupplierHotelRoomType] WHERE [SupplierId]=@SupplierId ORDER BY [RoomTypeId] ASC";
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

        #region ISupplierHotel 成员
        /// <summary>
        /// 写入酒店供应商附加信息
        /// </summary>
        /// <param name="info">酒店供应商信息业务实体</param>
        /// <returns></returns>
        public bool InsertHotelPertain(EyouSoft.Model.SupplierStructure.SupplierHotelInfo info)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_InsertHotelPertain);
            this._db.AddInParameter(cmd, "SupplierId", DbType.Int32, info.Id);
            this._db.AddInParameter(cmd, "Star", DbType.Byte, info.Star);
            this._db.AddInParameter(cmd, "Introduce", DbType.String, info.Introduce);
            this._db.AddInParameter(cmd, "TourGuide", DbType.String, info.TourGuide);

            return DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
        }

        /// <summary>
        /// 更新酒店供应商附加信息
        /// </summary>
        /// <param name="info">酒店供应商信息业务实体</param>
        /// <returns></returns>
        public bool UpdateHotelPertain(EyouSoft.Model.SupplierStructure.SupplierHotelInfo info)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_UpdateHotelPertain);
            this._db.AddInParameter(cmd, "Star", DbType.Byte, info.Star);
            this._db.AddInParameter(cmd, "Introduce", DbType.String, info.Introduce);
            this._db.AddInParameter(cmd, "TourGuide", DbType.String, info.TourGuide);
            this._db.AddInParameter(cmd, "SupplierId", DbType.Int32, info.Id);

            return DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
        }

        /// <summary>
        /// 写入酒店供应商房型信息集合
        /// </summary>
        /// <param name="roomTypes">房型信息集合</param>
        /// <returns></returns>
        public bool InsertRoomTypes(int supplierId, IList<EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo> roomTypes)
        {
            DbCommand cmd = this._db.GetSqlStringCommand("SELECT 1");
            StringBuilder cmdText = new StringBuilder();

            int i = 0;
            foreach (var item in roomTypes)
            {
                cmdText.AppendFormat(SQL_INSERT_InsertRoomType, i.ToString());
                this._db.AddInParameter(cmd, string.Format("SupplierId{0}", i.ToString()), DbType.Int32, supplierId);
                this._db.AddInParameter(cmd, string.Format("Name{0}", i.ToString()), DbType.String, item.Name);
                this._db.AddInParameter(cmd, string.Format("SellingPrice{0}", i.ToString()), DbType.Decimal, item.SellingPrice);
                this._db.AddInParameter(cmd, string.Format("AccountingPrice{0}", i.ToString()), DbType.Decimal, item.AccountingPrice);
                this._db.AddInParameter(cmd, string.Format("IsBreakfast{0}", i.ToString()), DbType.AnsiStringFixedLength, item.IsBreakfast ? "1" : "0");
                i++;
            }

            cmd.CommandText = cmdText.ToString();

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 删除酒店供应商房型信息
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        public bool DeleteRoomType(int supplierId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_DeleteRoomType);
            this._db.AddInParameter(cmd, "SupplierId", DbType.Int32, supplierId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取酒店供应商附加信息(含房型信息集合，不含供应商基本信息)
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierHotelInfo GetHotelPertainInfo(int supplierId)
        {
            EyouSoft.Model.SupplierStructure.SupplierHotelInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetHotelPertainInfo);
            this._db.AddInParameter(cmd, "SupplierId", DbType.Int32, supplierId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.SupplierStructure.SupplierHotelInfo()
                    {
                        Id = supplierId,
                        Star = (EyouSoft.Model.EnumType.SupplierStructure.HotelStar)rdr.GetByte(rdr.GetOrdinal("Star")),
                        Introduce = rdr["Introduce"].ToString(),
                        TourGuide = rdr["TourGuide"].ToString(),
                        RoomTypes = new List<EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo>()
                    };
                }

                if (info != null && rdr.NextResult())
                {
                    while (rdr.Read())
                    {
                        info.RoomTypes.Add(new EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo()
                        {
                            AccountingPrice = rdr.GetDecimal(rdr.GetOrdinal("AccountingPrice")),
                            IsBreakfast = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsBreakfast"))),
                            Name = rdr["Name"].ToString(),
                            RoomTypeId = rdr.GetInt32(rdr.GetOrdinal("RoomTypeId")),
                            SellingPrice = rdr.GetDecimal(rdr.GetOrdinal("SellingPrice"))
                        });
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// 获取酒店供应商信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> GetHotels(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo info)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> items = new List<EyouSoft.Model.SupplierStructure.SupplierHotelInfo>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_CompanySupplier";
            string primaryKey = "Id";
            string orderByString = "Id DESC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("Id,ProvinceId,ProvinceName,CityId,CityName,UnitName,TradeNum");
            fields.Append(",(SELECT TOP 1 ContactName,ContactTel,ContactFax,ContactMobile FROM tbl_SupplierContact AS A WHERE A.SupplierId = tbl_CompanySupplier.[Id] FOR XML RAW,ROOT('ROOT')) AS Contacters ");
            fields.Append(",(SELECT Star FROM tbl_SupplierHotel AS B WHERE B.SupplierId=tbl_CompanySupplier.Id) AS Star");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} AND IsDelete='0' AND SupplierType={1} ", companyId, (int)EyouSoft.Model.EnumType.CompanyStructure.SupplierType.酒店);
            info = info ?? new EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo();
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
            if (info.Star.HasValue)
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_SupplierHotel AS A WHERE A.SupplierId=tbl_CompanySupplier.Id AND Star={0}) ", (int)info.Star.Value);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.SupplierStructure.SupplierHotelInfo()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId")),
                        ProvinceName = rdr["ProvinceName"].ToString(),
                        CityId = rdr.GetInt32(rdr.GetOrdinal("CityId")),
                        CityName = rdr["CityName"].ToString(),
                        UnitName = rdr["UnitName"].ToString(),
                        TradeNum = rdr.GetInt32(rdr.GetOrdinal("TradeNum")),
                        SupplierContact = this.GetSupplierContactersByXml(rdr["Contacters"].ToString()),
                        Star = (EyouSoft.Model.EnumType.SupplierStructure.HotelStar)rdr.GetByte(rdr.GetOrdinal("Star"))
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取酒店供应商信息集合(供应商基本信息，图片信息，房型信息。同行登录口适用)
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> GetSiteHotels(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo info)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> items = new List<EyouSoft.Model.SupplierStructure.SupplierHotelInfo>();
            EyouSoft.Model.SupplierStructure.SupplierHotelInfo tmpModel = null;
            IEnumerable<XElement> xRows = null;
            XElement xRoot = null;
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_CompanySupplier";
            string primaryKey = "Id";
            string orderByString = "Id DESC";
            StringBuilder fields = new StringBuilder();

            #region fields

            fields.Append(" Id,ProvinceId,ProvinceName,CityId,CityName,UnitName,TradeNum,UnitAddress ");
            fields.Append(" ,(select Id,SupplierId,PicName,PicPath from tbl_SupplierAccessory where tbl_SupplierAccessory.SupplierId = tbl_CompanySupplier.Id for xml raw,root('root')) as HotelPics ");
            fields.Append(" ,(select RoomTypeId,SupplierId,Name,SellingPrice,AccountingPrice,IsBreakfast from tbl_SupplierHotelRoomType where tbl_SupplierHotelRoomType.SupplierId = tbl_CompanySupplier.Id for xml raw,root('root')) as HotelRoomTypes ");
            fields.Append(",(SELECT Star,Introduce FROM tbl_SupplierHotel AS B WHERE B.SupplierId=tbl_CompanySupplier.Id for xml raw,root('root')) AS Star");
            #endregion

            #region 拼接查询条件

            cmdQuery.AppendFormat(" CompanyId={0} AND IsDelete='0' AND SupplierType={1} ", companyId, (int)EyouSoft.Model.EnumType.CompanyStructure.SupplierType.酒店);
            info = info ?? new EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo();
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
            if (info.Star.HasValue)
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_SupplierHotel AS A WHERE A.SupplierId=tbl_CompanySupplier.Id AND Star={0}) ", (int)info.Star.Value);
            }

            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    tmpModel = new EyouSoft.Model.SupplierStructure.SupplierHotelInfo();

                    if (!rdr.IsDBNull(0))
                        tmpModel.Id = rdr.GetInt32(0);
                    if (!rdr.IsDBNull(1))
                        tmpModel.ProvinceId = rdr.GetInt32(1);
                    if (!rdr.IsDBNull(2))
                        tmpModel.ProvinceName = rdr.GetString(2);
                    if (!rdr.IsDBNull(3))
                        tmpModel.CityId = rdr.GetInt32(3);
                    if (!rdr.IsDBNull(4))
                        tmpModel.CityName = rdr.GetString(4);
                    if (!rdr.IsDBNull(5))
                        tmpModel.UnitName = rdr.GetString(5);
                    if (!rdr.IsDBNull(6))
                        tmpModel.TradeNum = rdr.GetInt32(6);
                    if (!rdr.IsDBNull(7))
                        tmpModel.UnitAddress = rdr.GetString(7);

                    if (!rdr.IsDBNull(8))
                    {
                        tmpModel.SupplierPic = new List<EyouSoft.Model.SupplierStructure.SupplierPic>();
                        EyouSoft.Model.SupplierStructure.SupplierPic spModel = null;
                        xRoot = XElement.Parse(rdr.GetString(8));
                        if (xRoot != null)
                        {
                            xRows = Utils.GetXElements(xRoot, "row");
                            if (xRows != null && xRows.Count() > 0)
                            {
                                foreach (var t in xRows)
                                {
                                    spModel = new EyouSoft.Model.SupplierStructure.SupplierPic();
                                    //Id,SupplierId,PicName,PicPath
                                    spModel.Id = Utils.GetInt(Utils.GetXAttributeValue(t, "Id"));
                                    spModel.SupplierId = Utils.GetInt(Utils.GetXAttributeValue(t, "SupplierId"));
                                    spModel.PicName = Utils.GetXAttributeValue(t, "PicName");
                                    spModel.PicPath = Utils.GetXAttributeValue(t, "PicPath");

                                    tmpModel.SupplierPic.Add(spModel);
                                }
                            }
                        }
                    }
                    if (!rdr.IsDBNull(9))
                    {
                        tmpModel.RoomTypes = new List<Model.SupplierStructure.SupplierHotelRoomTypeInfo>();
                        Model.SupplierStructure.SupplierHotelRoomTypeInfo shrtModel = null;
                        xRoot = XElement.Parse(rdr.GetString(9));
                        if (xRoot != null)
                        {
                            xRows = Utils.GetXElements(xRoot, "row");
                            if (xRows != null && xRows.Count() > 0)
                            {
                                foreach (var t in xRows)
                                {
                                    shrtModel = new EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo();
                                    //RoomTypeId,SupplierId,Name,SellingPrice,AccountingPrice,IsBreakfast
                                    shrtModel.RoomTypeId = Utils.GetInt(Utils.GetXAttributeValue(t, "RoomTypeId"));
                                    shrtModel.Name = Utils.GetXAttributeValue(t, "Name");
                                    shrtModel.SellingPrice = Utils.GetDecimal(Utils.GetXAttributeValue(t, "SellingPrice"));
                                    shrtModel.AccountingPrice = Utils.GetDecimal(Utils.GetXAttributeValue(t, "AccountingPrice"));
                                    shrtModel.IsBreakfast = (Utils.GetXAttributeValue(t, "IsBreakfast") == "1" || Utils.GetXAttributeValue(t, "IsBreakfast").ToLower() == "true") ? true : false;

                                    tmpModel.RoomTypes.Add(shrtModel);
                                }
                            }
                        }
                    }
                    if (!rdr.IsDBNull(10))
                    {
                        xRoot = XElement.Parse(rdr.GetString(10));
                        if (xRoot != null)
                        {
                            xRows = Utils.GetXElements(xRoot, "row");
                            if (xRows != null && xRows.Count() > 0)
                            {
                                foreach (var t in xRows)
                                {
                                    tmpModel.Star = (EyouSoft.Model.EnumType.SupplierStructure.HotelStar)Utils.GetInt(Utils.GetXAttributeValue(t, "Star"));
                                    tmpModel.Introduce = Utils.GetXAttributeValue(t, "Introduce");

                                    break;
                                }
                            }
                        }
                    }


                    items.Add(tmpModel);
                }
            }

            return items;
        }

        #endregion
    }
}
