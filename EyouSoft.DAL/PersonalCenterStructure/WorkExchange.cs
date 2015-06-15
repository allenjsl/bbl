using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;
using System.Data;
namespace EyouSoft.DAL.PersonalCenterStructure
{
    /// <summary>
    /// 个人中心-交流专区数据层
    /// </summary>
    /// 鲁功源 2011-01-17
    public class WorkExchange : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.PersonalCenterStructure.IWorkExchange
    {
        #region 变量
        private const string Sql_WorkExchange_Delete = "update tbl_WorkExchange set IsDelete='1' where ExchangeId in({0})";
        private EyouSoft.Data.EyouSoftTBL dcDal = null;
        private Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkExchange()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
        }
        #endregion

        #region IWorkExchange 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">交流专区实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.PersonalCenterStructure.WorkExchange model)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_WorkExchange_Insert");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "Title", DbType.String, model.Title);
            this._db.AddInParameter(dc, "Description", DbType.String, model.Description);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, model.OperatorName);
            this._db.AddInParameter(dc, "IsAnonymous", DbType.String, model.IsAnonymous?"1":"0");
            this._db.AddInParameter(dc, "AcceptXML", DbType.String, this.CreateAcceptXML(model.AcceptList));
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            if (obj != null && int.Parse(obj.ToString()) > 0)
                model.ExchangeId = int.Parse(obj.ToString());
            return int.Parse(obj.ToString()) > 0 ? true : false;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">交流专区实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.PersonalCenterStructure.WorkExchange model)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_WorkExchange_Update");
            this._db.AddInParameter(dc, "ExchangeId", DbType.Int32, model.ExchangeId);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "Title", DbType.String, model.Title);
            this._db.AddInParameter(dc, "Description", DbType.String, model.Description);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, model.OperatorName);
            this._db.AddInParameter(dc, "IsAnonymous", DbType.String, model.IsAnonymous ? "1" : "0");
            this._db.AddInParameter(dc, "AcceptXML", DbType.String, this.CreateAcceptXML(model.AcceptList));
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object obj= this._db.GetParameterValue(dc, "Result");
            return int.Parse(obj.ToString()) > 0 ? true : false;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params string[] Ids)
        {
            string strIds = string.Empty;
            foreach (string str in Ids)
            {
                strIds += str + ",";
            }
            DbCommand dc = this._db.GetSqlStringCommand(string.Format(Sql_WorkExchange_Delete, strIds.TrimEnd(',')));
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 获取交流专区实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>交流专区实体</returns>
        public EyouSoft.Model.PersonalCenterStructure.WorkExchange GetModel(int Id)
        {
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
            var obj = dcDal.WorkExchange.FirstOrDefault(item => item.ExchangeId == Id && item.IsDelete=="0");
            if (obj == null || obj.ExchangeId == 0)
                return null;
            return new EyouSoft.Model.PersonalCenterStructure.WorkExchange()
            {
                Clicks = obj.Clicks,
                CompanyId = obj.CompanyId,
                CreateTime = obj.CreateTime,
                Description = obj.Description,
                ExchangeId = obj.ExchangeId,
                IsAnonymous = obj.IsAnonymous == "1" ? true : false,
                IsDelete = obj.IsDelete == "1" ? true : false,
                OperatorId = obj.OperatorId,
                OperatorName = obj.OperatorName,
                Replys = obj.Replys,
                Title = obj.Title,
                AcceptList = (from accept in obj.WorkExchangeAcceptList
                              select new EyouSoft.Model.PersonalCenterStructure.WorkExchangeAccept()
                              {
                                  AcceptId = accept.AcceptId,
                                  AcceptType = (EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType)int.Parse(accept.AcceptType.ToString()),
                                  ExchangeId = accept.ExchangeId,
                                  AcceptName = GetAcceptName(accept.AcceptId,(EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType)int.Parse(accept.AcceptType.ToString()))
                              }).ToList(),
                ReplyList = (from reply in obj.WorkExchangeReplyList select new EyouSoft.Model.PersonalCenterStructure.WorkExchangeReply() { 
                  Description=reply.Description,
                  ExchangeId=reply.ExchangeId.HasValue?reply.ExchangeId.Value:0,
                  IsAnonymous=reply.IsAnonymous=="1"?true:false,
                  IsDelete=reply.IsDelete=="1"?true:false,
                  OperatorId=reply.OperatorId.HasValue?reply.OperatorId.Value:0,
                  OperatorName=reply.OperatorName,
                  ReplyId=reply.ReplyId,
                  ReplyTime=reply.ReplyTime
                }).ToList()
            };
        }
        /// <summary>
        /// 分页获取交流专区列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号 =0返回所有</param>
        /// <param name="OperatorId">操作人编号 =0返回所有</param>
        /// <returns>交流专区列表</returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.WorkExchange> GetList(int pageSize, int pageIndex, ref int RecordCount, int CompanyId, int OperatorId)
        {
            IList<EyouSoft.Model.PersonalCenterStructure.WorkExchange> list = new List<EyouSoft.Model.PersonalCenterStructure.WorkExchange>();
            string tableName = "tbl_WorkExchange";
            string fields = "ExchangeId,Title,OperatorName,IsAnonymous,Clicks,Replys,CreateTime";
            string primaryKey = "ExchangeId";
            string orderbyStr = " CreateTime DESC ";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' ");
            if (CompanyId > 0)
                strWhere.AppendFormat(" and CompanyId={0} ", CompanyId);
            //TODO:需根据OperatorId查询对应的权限集合
            if (OperatorId > 0)
                strWhere.AppendFormat("");
            using (IDataReader dr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref RecordCount, tableName, primaryKey, fields, strWhere.ToString(), orderbyStr))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.PersonalCenterStructure.WorkExchange model = new EyouSoft.Model.PersonalCenterStructure.WorkExchange();
                    model.ExchangeId = dr.GetInt32(dr.GetOrdinal("ExchangeId"));
                    model.Title = dr[dr.GetOrdinal("Title")].ToString();
                    model.OperatorName = dr[dr.GetOrdinal("OperatorName")].ToString();
                    model.IsAnonymous = dr[dr.GetOrdinal("IsAnonymous")].ToString() == "1" ? true : false;
                    model.Clicks = dr.GetInt32(dr.GetOrdinal("Clicks"));
                    model.Replys = dr.GetInt32(dr.GetOrdinal("Replys"));
                    model.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }
        /// <summary>
        /// 更新浏览次数
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool SetClicks(int Id)
        {
            var obj = dcDal.WorkExchange.FirstOrDefault(item => item.ExchangeId == Id && item.IsDelete == "0");
            if (obj != null)
            {
                obj.Clicks += 1;
                dcDal.SubmitChanges();
            }
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
        }
        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="model">交流专区回复实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool AddReply(EyouSoft.Model.PersonalCenterStructure.WorkExchangeReply model)
        {
            EyouSoft.Data.WorkExchangeReply obj = new EyouSoft.Data.WorkExchangeReply()
            { 
               Description=model.Description,
               ExchangeId=model.ExchangeId,
               IsAnonymous=model.IsAnonymous?"1":"0",
               OperatorId=model.OperatorId,
               OperatorName=model.OperatorName,
               ReplyTime=DateTime.Now,
               IsDelete="0"
            };
            dcDal.WorkExchangeReply.InsertOnSubmit(obj);
            if (dcDal.ChangeConflicts.Count == 0)
            {
                EyouSoft.Data.WorkExchange exchangeModel = dcDal.WorkExchange.FirstOrDefault(item => item.ExchangeId == model.ExchangeId);
                if (exchangeModel != null)
                {
                    exchangeModel.Replys += 1;
                    dcDal.SubmitChanges();
                }
            }
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 创建接收对象XML
        /// </summary>
        /// <returns></returns>
        private string CreateAcceptXML(IList<EyouSoft.Model.PersonalCenterStructure.WorkExchangeAccept> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;
            StringBuilder strXML = new StringBuilder("<ROOT>");
            foreach (EyouSoft.Model.PersonalCenterStructure.WorkExchangeAccept model in list)
            {
                strXML.AppendFormat("<AcceptInfo AcceptType=\"{0}\" AcceptId=\"{1}\" />",(int)model.AcceptType,model.AcceptId);
            }
            strXML.Append("</ROOT>");
            return strXML.ToString();
        }
        /// <summary>
        /// 根据接收对象类型和编号获取接收名称
        /// </summary>
        /// <param name="AcceptId">接收对象编号</param>
        /// <param name="AcceptType">接收对象类型</param>
        /// <returns></returns>
        private string GetAcceptName(int AcceptId, EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType AcceptType)
        {
            string AcceptName = string.Empty;
            switch (AcceptType)
            { 
                case EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType.指定部门:
                    EyouSoft.Model.CompanyStructure.Department departModel= new DAL.CompanyStructure.Department().GetModel(AcceptId);
                    if (departModel != null)
                        AcceptName = departModel.DepartName;
                    departModel = null;
                    break;
                case EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType.指定人:
                    EyouSoft.Model.CompanyStructure.ContactPersonInfo userModel = new DAL.CompanyStructure.CompanyUser().GetUserBasicInfo(AcceptId);
                    if (userModel != null)
                        AcceptName = userModel.ContactName;
                    userModel = null;
                    break;
            }
            return AcceptName;
        }
        #endregion

    }
}
