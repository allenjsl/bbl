using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 统计分析-现金流量明细数据层
    /// </summary>
    /// 鲁功源 2011-01-22
    public class CompanyCashFlow : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.ICompanyCashFlow
    {
        #region 变量
        private Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyCashFlow()
        {
            _db = this.SystemStore;
        }
        #endregion

        #region ICompanyCashFlow 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">现金流量明细实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.StatisticStructure.CompanyCashFlow model)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_CompanyCashFlow_Add");
            this._db.AddInParameter(dc, "CashReserve", DbType.Decimal, model.CashReserve);
            this._db.AddInParameter(dc, "CashType", DbType.Byte, (int)model.CashType);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            if (obj == null)
                return false;
            return int.Parse(obj.ToString()) > 0 ? true : false;
        }

        #endregion
    }
}
