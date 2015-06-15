using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Xml.Linq;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.SupplierStructure
{
    /// <summary>
    /// 供应商景点数据层
    /// </summary>
    /// 鲁功源 2011-03-08
    public class SupplierSpot : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SupplierStructure.ISupplierSpot
    {
        #region sql变量
        private const string Sql_SupplierBasic_Add = "INSERT INTO [tbl_CompanySupplier]([ProvinceId],[ProvinceName],[CityId],[CityName],[UnitName],[SupplierType],[UnitAddress],[TradeNum],[Remark],[CompanyId],[OperatorId],[IssueTime],[IsDelete],[UnitPolicy],[AgreementFile]) VALUES({0},'{1}',{2},'{3}','{4}',{5},'{6}',0,'{7}',{8},{9},getdate(),'0','{10}','{11}');select @SupplierId=@@identity;";
        private const string Sql_SupplierContact_Add = "INSERT INTO [tbl_SupplierContact]([CompanyId],[SupplierId],[SupplierType],[ContactName],[JobTitle],[ContactFax],[ContactTel],[ContactMobile],[QQ],[Email]) VALUES({0},@SupplierId,{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}');";
        private const string Sql_SupplierSpot_Add = "INSERT INTO [dbo].[tbl_SupplierSpot]([SupplierId],[Star],[TourGuide],[TeamPrice],[TravelerPrice]) VALUES(@SupplierId,{0},'{1}',{2},{3});set @SupplierId=0;";
        private const string Sql_SupplierPic_Add = "INSERT INTO [tbl_SupplierAccessory]([SupplierId],[PicName],[PicPath]) VALUES(@SupplierId,'{0}','{1}');";
        private const string Sql_SupplierSpot_Edit = "Update [dbo].[tbl_SupplierSpot] set [Star]=@Star,[TourGuide]=@TourGuide,[TeamPrice]=@TeamPrice,[TravelerPrice]=@TravelerPrice where [SupplierId]=@SupplierId;Update tbl_CompanySupplier set AgreementFile=@AgreementFile,UnitPolicy=@UnitPolicy where Id=@SupplierId";
        private const string Sql_SupplierSpot_GetModel = "Select [SupplierId],[Star],[TourGuide],[TeamPrice],[TravelerPrice],(select AgreementFile from tbl_CompanySupplier where Id=@SupplierId) as AgreementFile,(select UnitPolicy from tbl_CompanySupplier where Id=@SupplierId) as UnitPolicy  from [dbo].[tbl_SupplierSpot] where [SupplierId]=@SupplierId";
        #endregion

        private readonly Database _db = null;

        #region 构造函数
        public SupplierSpot()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ISupplierSpot 成员
        /// <summary>
        /// 批量添加供应商景点信息
        /// </summary>
        /// <param name="list">供应商景点集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(IList<EyouSoft.Model.SupplierStructure.SupplierSpot> list)
        {
            StringBuilder strSql = new StringBuilder("declare @SupplierId int;");
            foreach (var item in list)
            {
                //供应商基本信息
                strSql.AppendFormat(Sql_SupplierBasic_Add, item.ProvinceId, item.ProvinceName, item.CityId, item.CityName, item.UnitName, (int)item.SupplierType, item.UnitAddress, item.Remark, item.CompanyId, item.OperatorId, item.UnitPolicy, item.AgreementFile);

                #region 联系人信息
                if (item.SupplierContact != null && item.SupplierContact.Count > 0)
                {
                    foreach (var contact in item.SupplierContact)
                    {
                        strSql.AppendFormat(Sql_SupplierContact_Add, item.CompanyId, (int)item.SupplierType, contact.ContactName, contact.JobTitle, contact.ContactFax, contact.ContactTel, contact.ContactMobile, contact.QQ, contact.Email);
                    }
                }
                #endregion

                #region 图片信息
                if (item.SupplierPic != null && item.SupplierPic.Count > 0)
                {
                    foreach (var pic in item.SupplierPic)
                    {
                        strSql.AppendFormat(Sql_SupplierPic_Add, pic.PicName, pic.PicPath);
                    }
                }
                #endregion

                #region 供应商特有信息
                strSql.AppendFormat(Sql_SupplierSpot_Add, (int)item.Start, item.TourGuide, item.TeamPrice, item.TravelerPrice);
                #endregion
            }
            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 修改供应商景点信息
        /// </summary>
        /// <param name="model">供应商景点实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.SupplierStructure.SupplierSpot model)
        {
            DbCommand dc = this._db.GetSqlStringCommand(Sql_SupplierSpot_Edit);
            this._db.AddInParameter(dc, "Star", DbType.Byte, (int)model.Start);
            this._db.AddInParameter(dc, "TourGuide", DbType.String, model.TourGuide);
            this._db.AddInParameter(dc, "TeamPrice", DbType.Decimal, model.TeamPrice);
            this._db.AddInParameter(dc, "TravelerPrice", DbType.Decimal, model.TravelerPrice);
            this._db.AddInParameter(dc, "AgreementFile", DbType.String, model.AgreementFile);
            this._db.AddInParameter(dc, "UnitPolicy", DbType.String, model.UnitPolicy);
            this._db.AddInParameter(dc, "SupplierId", DbType.Int32, model.Id);
            return DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 获取供应商景点信息
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <param name="model">供应商景点信息</param>
        public void GetModel(int Id, ref EyouSoft.Model.SupplierStructure.SupplierSpot model)
        {
            DbCommand dc = this._db.GetSqlStringCommand(Sql_SupplierSpot_GetModel);
            this._db.AddInParameter(dc, "SupplierId", DbType.Int32, Id);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    model.TourGuide = dr.IsDBNull(dr.GetOrdinal("TourGuide")) ? "" : dr.GetString(dr.GetOrdinal("TourGuide"));
                    model.TeamPrice = dr.IsDBNull(dr.GetOrdinal("TeamPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("TeamPrice"));
                    model.TravelerPrice = dr.IsDBNull(dr.GetOrdinal("TravelerPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("TravelerPrice"));
                    model.AgreementFile = dr.IsDBNull(dr.GetOrdinal("AgreementFile")) ? "" : dr.GetString(dr.GetOrdinal("AgreementFile"));
                    model.UnitPolicy = dr.IsDBNull(dr.GetOrdinal("UnitPolicy")) ? "" : dr.GetString(dr.GetOrdinal("UnitPolicy"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Star")))
                        model.Start = (EyouSoft.Model.EnumType.SupplierStructure.ScenicSpotStar)int.Parse(dr[dr.GetOrdinal("Star")].ToString());
                }
            }
        }
        /// <summary>
        /// 分页获取供应商景点信息列表
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="query">景点查询实体</param>
        /// <returns>景点信息列表</returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierSpot> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.SupplierStructure.SupplierQuery query)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierSpot> list = new List<EyouSoft.Model.SupplierStructure.SupplierSpot>();
            string tableName = "tbl_CompanySupplier";
            StringBuilder fields = new StringBuilder();
            fields.Append(" Id,UnitAddress,UnitName,ProvinceName,CityName,TradeNum,UnitPolicy");
            fields.Append(" ,(select top 1 ContactName,ContactTel,ContactFax from tbl_SupplierContact a where a.SupplierId = tbl_CompanySupplier.[Id] for xml raw,root('root')) as ContactXML ");
            fields.AppendFormat(",(select [Star],[TourGuide],[TeamPrice],[TravelerPrice] from [tbl_SupplierSpot] where tbl_SupplierSpot.[SupplierId]=tbl_CompanySupplier.Id for xml raw,root('root')) as SpotInfo");
            
            string primaryKey = "Id";
            string orderbyStr = "IssueTime desc";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' ");
            strWhere.AppendFormat(" and SupplierType={0}", (int)EyouSoft.Model.EnumType.CompanyStructure.SupplierType.景点);
            strWhere.AppendFormat(" and CompanyId = {0}", CompanyId);
            if (query != null)
            {
                if (query.ProvinceId > 0)
                    strWhere.AppendFormat(" and ProvinceId={0} ", query.ProvinceId);
                if (!string.IsNullOrEmpty(query.ProvinceName))
                    strWhere.AppendFormat(" and ProvinceName like'%{0}%' ", query.ProvinceName);
                if (query.CityId > 0)
                    strWhere.AppendFormat(" and CityId={0} ", query.CityId);
                if (!string.IsNullOrEmpty(query.CityName))
                    strWhere.AppendFormat(" and CityName like'%{0}%' ", query.CityName);
                if (!string.IsNullOrEmpty(query.UnitName))
                    strWhere.AppendFormat(" and UnitName like'%{0}%' ", query.UnitName);
            }

            using (IDataReader dr = DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields.ToString(), strWhere.ToString(), orderbyStr))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SupplierStructure.SupplierSpot model = new EyouSoft.Model.SupplierStructure.SupplierSpot();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.UnitAddress = dr.IsDBNull(dr.GetOrdinal("UnitAddress")) ? "" : dr.GetOrdinal("UnitAddress").ToString();
                    model.UnitName = dr.GetString(dr.GetOrdinal("UnitName"));
                    model.ProvinceName = dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? "" : dr.GetString(dr.GetOrdinal("ProvinceName"));
                    model.CityName = dr.IsDBNull(dr.GetOrdinal("CityName")) ? "" : dr.GetString(dr.GetOrdinal("CityName"));
                    model.TradeNum = dr.GetInt32(dr.GetOrdinal("TradeNum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("UnitPolicy")))
                        model.UnitPolicy = dr.GetString(dr.GetOrdinal("UnitPolicy"));

                    model.SupplierContact = GetContactList(dr.IsDBNull(dr.GetOrdinal("ContactXML")) ? "" : dr.GetString(dr.GetOrdinal("ContactXML")));

                    if (!dr.IsDBNull(dr.GetOrdinal("SpotInfo")))
                    {
                        XElement xRoot = XElement.Parse(dr.GetString(dr.GetOrdinal("SpotInfo")));
                        if (xRoot != null)
                        {
                            var xRows = Utils.GetXElements(xRoot, "row");
                            if (xRows != null && xRows.Count() > 0)
                            {
                                foreach (var t in xRows)
                                {
                                    model.Start = (EyouSoft.Model.EnumType.SupplierStructure.ScenicSpotStar)Utils.GetInt(Utils.GetXAttributeValue(t, "Star"));
                                    model.TourGuide = Utils.GetXAttributeValue(t, "TourGuide");
                                    model.TeamPrice = Utils.GetDecimal(Utils.GetXAttributeValue(t, "TeamPrice"));
                                    model.TravelerPrice = Utils.GetDecimal(Utils.GetXAttributeValue(t, "TravelerPrice"));

                                    break;
                                }
                            }
                        }
                    }

                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }

        /// <summary>
        /// 获取供应商景点信息
        /// </summary>
        /// <param name="TopNum">top条数</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="query">供应商查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierSpot> GetList(int TopNum, int CompanyId
            , EyouSoft.Model.SupplierStructure.SupplierQuery query)
        {
            if (CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.SupplierStructure.SupplierSpot> list = new List<EyouSoft.Model.SupplierStructure.SupplierSpot>();
            StringBuilder strSql = new StringBuilder(" select ");
            if (TopNum > 0)
                strSql.AppendFormat(" top {0} ", TopNum);

            strSql.Append(" [Id],[ProvinceId],[ProvinceName],[CityId],[CityName],[UnitName],[SupplierType],[UnitAddress],[Commission],[AgreementFile],[TradeNum],[UnitPolicy],[Remark],[CompanyId],[OperatorId],[IssueTime],[IsDelete],ss.Star,ss.TourGuide,ss.TeamPrice,ss.TravelerPrice,(select Id,SupplierId,PicName,PicPath from tbl_SupplierAccessory where tbl_SupplierAccessory.SupplierId = cs.Id for xml raw,root('root')) as Files from tbl_CompanySupplier as cs left join tbl_SupplierSpot as ss on cs.Id = ss.SupplierId ");
            strSql.AppendFormat(" where cs.IsDelete = '0' and CompanyId = {0} and SupplierType = {1} ", CompanyId, (int)EyouSoft.Model.EnumType.CompanyStructure.SupplierType.景点);
            if (query != null)
            {
                if (query.ProvinceId > 0)
                    strSql.AppendFormat(" and cs.ProvinceId={0} ", query.ProvinceId);
                if (!string.IsNullOrEmpty(query.ProvinceName))
                    strSql.AppendFormat(" and cs.ProvinceName like'%{0}%' ", query.ProvinceName);
                if (query.CityId > 0)
                    strSql.AppendFormat(" and cs.CityId={0} ", query.CityId);
                if (!string.IsNullOrEmpty(query.CityName))
                    strSql.AppendFormat(" and cs.CityName like'%{0}%' ", query.CityName);
                if (!string.IsNullOrEmpty(query.UnitName))
                    strSql.AppendFormat(" and cs.UnitName like'%{0}%' ", query.UnitName);
                if(query.Start.HasValue)
                    strSql.AppendFormat(" and ss.Star = {0} ", (int)query.Start.Value);
            }

            strSql.Append(" order by Id desc ");

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SupplierStructure.SupplierSpot model = new EyouSoft.Model.SupplierStructure.SupplierSpot();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.UnitAddress = dr.IsDBNull(dr.GetOrdinal("UnitAddress")) ? "" : dr.GetOrdinal("UnitAddress").ToString();
                    model.UnitName = dr.GetString(dr.GetOrdinal("UnitName"));
                    model.ProvinceName = dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? "" : dr.GetString(dr.GetOrdinal("ProvinceName"));
                    model.CityName = dr.IsDBNull(dr.GetOrdinal("CityName")) ? "" : dr.GetString(dr.GetOrdinal("CityName"));
                    model.TradeNum = dr.GetInt32(dr.GetOrdinal("TradeNum"));
                    //model.SupplierContact = GetContactList(dr.IsDBNull(dr.GetOrdinal("ContactXML")) ? "" : dr.GetString(dr.GetOrdinal("ContactXML")));
                    model.TourGuide = dr.IsDBNull(dr.GetOrdinal("TourGuide")) ? "" : dr.GetString(dr.GetOrdinal("TourGuide"));
                    model.TeamPrice = dr.IsDBNull(dr.GetOrdinal("TeamPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("TeamPrice"));
                    model.TravelerPrice = dr.IsDBNull(dr.GetOrdinal("TravelerPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("TravelerPrice"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Star")))
                        model.Start = (EyouSoft.Model.EnumType.SupplierStructure.ScenicSpotStar)int.Parse(dr[dr.GetOrdinal("Star")].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("Files")))
                        model.SupplierPic = this.GetSupplierPic(dr.GetString(dr.GetOrdinal("Files")));


                    list.Add(model);
                    model = null;
                }
            }

            return list;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 转XML格式
        /// </summary>
        /// <param name="ContactXML"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.SupplierContact> GetContactList(string ContactXML)
        {
            if (string.IsNullOrEmpty(ContactXML))
                return null;
            IList<EyouSoft.Model.CompanyStructure.SupplierContact> ResultList = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
            XElement root = XElement.Parse(ContactXML);
            var xRow = root.Elements("row");
            foreach (var tmp in xRow)
            {
                EyouSoft.Model.CompanyStructure.SupplierContact model = new EyouSoft.Model.CompanyStructure.SupplierContact()
                {
                    ContactName = tmp.Attribute("ContactName").Value,
                    ContactTel = tmp.Attribute("ContactTel").Value,
                    ContactFax = tmp.Attribute("ContactFax") == null ? "" : tmp.Attribute("ContactFax").Value
                };
                ResultList.Add(model);
                model = null;
            }
            return ResultList;
        }

        /// <summary>
        /// 根据SqlXML获取供应商图片
        /// </summary>
        /// <param name="SupplierPicXML">供应商图片XML</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.SupplierStructure.SupplierPic> GetSupplierPic(string SupplierPicXML)
        {
            if (string.IsNullOrEmpty(SupplierPicXML))
                return null;

            XElement root = XElement.Parse(SupplierPicXML);
            if (root == null)
                return null;
            var xRow = Utils.GetXElements(root, "row");
            if (xRow == null || xRow.Count() <= 0)
                return null;

            IList<EyouSoft.Model.SupplierStructure.SupplierPic> list = new List<EyouSoft.Model.SupplierStructure.SupplierPic>();
            EyouSoft.Model.SupplierStructure.SupplierPic model = null;
            foreach (var t in xRow)
            {
                model = new EyouSoft.Model.SupplierStructure.SupplierPic();

                model.Id = Utils.GetInt(Utils.GetXAttributeValue(t, "Id"));
                model.SupplierId = Utils.GetInt(Utils.GetXAttributeValue(t, "SupplierId"));
                model.PicName = Utils.GetXAttributeValue(t, "PicName");
                model.PicPath = Utils.GetXAttributeValue(t, "PicPath");

                list.Add(model);
            }

            return list;
        }

        #endregion

    }
}
