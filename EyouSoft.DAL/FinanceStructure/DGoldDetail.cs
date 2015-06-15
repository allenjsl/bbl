/*Author:汪奇志 2011-04-29*/
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

namespace EyouSoft.DAL.FinanceStructure
{
    /// <summary>
    /// 收入、支出增加减少费用明细信息数据访问类
    /// </summary>
    /// Author:汪奇志 2011-04-29
    public class DGoldDetail : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.FinanceStructure.IGoldDetail
    {
        #region static constants
        //static constants
        private const string SQL_SELECT_GetDetails = "SELECT * FROM [tbl_GoldDetail] WHERE [ItemType]=@ItemType AND [ItemId]=@ItemId";
        #endregion

        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DGoldDetail()
        {
            this._db = base.SystemStore;
        }
        #endregion    

        #region private members
        /// <summary>
        /// 创建增加减少费用信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateGoldDetailsXML(IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> items)
        {
            //XML：<ROOT><Info DetailId="明细编号" ItemType="关联类型" ItemId="关联类型" AddAmount="增加费用" ReduceAmount="减少费用" Remark="备注" OperatorId="操作人编号" /></ROOT>
            if (items == null || items.Count < 1) return string.Empty;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info DetailId=\"{0}\" ItemType=\"{1}\" ItemId=\"{2}\" AddAmount=\"{3}\" ReduceAmount=\"{4}\" Remark=\"{5}\" OperatorId=\"{6}\" />", item.DetailId
                    , (int)item.ItemType
                    , item.ItemId
                    , item.AddAmount
                    , item.ReduceAmount
                    , Utils.ReplaceXmlSpecialCharacter(item.Remark)
                    , item.OperatorId);
            }
            xmlDoc.Append("</ROOT>");
            return xmlDoc.ToString();
        }
        #endregion

        #region IGoldDetail 成员
        /// <summary>
        /// 设置收入、支出增加减少费用明细信息集合，1成功 其它失败
        /// </summary>
        /// <param name="details">收入、支出增加减少费用明细信息集合</param>
        /// <returns></returns>
        public int SetDetais(IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> details)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_GoldDetail_SetDetails");

            this._db.AddInParameter(cmd, "DetailsXML", DbType.String, this.CreateGoldDetailsXML(details));
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            int sqlExceptionCode = 0;

            try
            {
                DbHelper.RunProcedure(cmd, this._db);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return sqlExceptionCode;

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 获取收入、支出增加减少费用明细信息集合
        /// </summary>
        /// <param name="itemType">项目类型</param>
        /// <param name="itemId">项目编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GetDetails(EyouSoft.Model.EnumType.FinanceStructure.GoldType itemType, string itemId)
        {
            IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> items=new List<EyouSoft.Model.FinanceStructure.MGoldDetailInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetDetails);
            this._db.AddInParameter(cmd, "ItemType", DbType.Byte, itemType);
            this._db.AddInParameter(cmd, "ItemId", DbType.AnsiStringFixedLength, itemId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.FinanceStructure.MGoldDetailInfo()
                    {
                        AddAmount = rdr.GetDecimal(rdr.GetOrdinal("AddAmount")),
                        CreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime")),
                        DetailId = rdr.GetString(rdr.GetOrdinal("DetailId")),
                        ItemId = rdr.GetString(rdr.GetOrdinal("ItemId")),
                        ItemType = (EyouSoft.Model.EnumType.FinanceStructure.GoldType)rdr.GetByte(rdr.GetOrdinal("ItemType")),
                        OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        ReduceAmount = rdr.GetDecimal(rdr.GetOrdinal("ReduceAmount")),
                        Remark = rdr["Remark"].ToString()
                    });
                }
            }

            return items;
        }

        #endregion
    }
}
