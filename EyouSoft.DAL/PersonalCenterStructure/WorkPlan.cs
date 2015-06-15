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
    
    public class WorkPlan : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.PersonalCenterStructure.IWorkPlan
    {
        #region 变量
        private const string Sql_WorkPlan_Delete = "update tbl_WorkPlan set IsDelete='1' where PlanId in({0})";
        private EyouSoft.Data.EyouSoftTBL dcDal = null;
        private Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkPlan()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
        }
        #endregion

        #region IWorkPlan 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">工作计划实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.PersonalCenterStructure.WorkPlan model)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_WorkPlan_Insert");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "Title", DbType.String, model.Title);
            this._db.AddInParameter(dc, "Description", DbType.String, model.Description);
            this._db.AddInParameter(dc, "FilePath", DbType.String, model.FilePath);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "ExpectedDate", DbType.DateTime, model.ExpectedDate);
            this._db.AddInParameter(dc, "ActualDate", DbType.DateTime, model.ActualDate);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, model.OperatorName);
            this._db.AddInParameter(dc, "Status", DbType.Byte, (int)model.Status);
            this._db.AddInParameter(dc, "AcceptXML", DbType.String, this.CreateAcceptXML(model.AcceptList));
            this._db.AddInParameter(dc, "PlanNO", DbType.String, model.PlanNO);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            if (obj != null && int.Parse(obj.ToString()) > 0)
                model.PlanId = int.Parse(obj.ToString());
            return int.Parse(obj.ToString()) > 0 ? true : false;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">工作计划实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.PersonalCenterStructure.WorkPlan model)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_WorkPlan_Update");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "Title", DbType.String, model.Title);
            this._db.AddInParameter(dc, "Description", DbType.String, model.Description);
            this._db.AddInParameter(dc, "FilePath", DbType.String, model.FilePath);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "ExpectedDate", DbType.DateTime, model.ExpectedDate);
            this._db.AddInParameter(dc, "ActualDate", DbType.DateTime, model.ActualDate);
            this._db.AddInParameter(dc, "PlanId", DbType.Int32, model.PlanId);
            this._db.AddInParameter(dc, "Status", DbType.Byte, (int)model.Status);
            this._db.AddInParameter(dc, "AcceptXML", DbType.String, this.CreateAcceptXML(model.AcceptList));
            this._db.AddInParameter(dc, "PlanNO", DbType.String, model.PlanNO);
            this._db.AddInParameter(dc, "DepartmentComment", DbType.String, model.DepartmentComment);
            this._db.AddInParameter(dc, "ManagerComment", DbType.String, model.ManagerComment);
            this._db.AddInParameter(dc, "LastTime", DbType.DateTime, model.LastTime);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            return int.Parse(obj.ToString()) > 0 ? true : false;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params string[] Ids)
        {
            string strIds = string.Empty;
            foreach (string str in Ids)
            {
                strIds += str + ",";
            }
            DbCommand dc = this._db.GetSqlStringCommand(string.Format(Sql_WorkPlan_Delete, strIds.TrimEnd(',')));
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 获取交流专区实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>交流专区实体</returns>
        public EyouSoft.Model.PersonalCenterStructure.WorkPlan GetModel(int Id,string us)
        {
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
            var obj = dcDal.WorkPlan.FirstOrDefault(item=>item.PlanId==Id && item.IsDelete=="0");
            if (obj != null)
            {
                return new EyouSoft.Model.PersonalCenterStructure.WorkPlan()
                {
                    ActualDate = obj.ActualDate.HasValue ? obj.ActualDate.Value : DateTime.MaxValue,
                    CompanyId = obj.CompanyId,
                    CreateTime = obj.CreateTime,
                    DepartmentComment = obj.DepartmentComment,
                    Description = obj.Description,
                    ExpectedDate = obj.ExpectedDate.HasValue ? obj.ExpectedDate.Value : DateTime.MaxValue,
                    FilePath = obj.FilePath,
                    LastTime = obj.LastTime.HasValue ? obj.LastTime.Value : DateTime.MaxValue,
                    ManagerComment = obj.ManagerComment,
                    OperatorId = obj.OperatorId,
                    OperatorName = obj.OperatorName,
                    PlanId = obj.PlanId,
                    Remark = obj.Remark,
                    Status = (EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState)int.Parse(obj.Status.ToString()),
                    Title = obj.Title,
                    AcceptList = (from accept in obj.WorkPlanAcceptList where accept.PlanId == Id select new EyouSoft.Model.PersonalCenterStructure.WorkPlanAccept() { 
                    AccetpId=accept.AccetpId,
                    PlanId=accept.PlanId,
                    AccetpName = GetAcceptName(accept.AccetpId)
                    }).ToList(),
                    PlanNO = obj.PlanNO
                };
            }
            return null;
        }
        /// <summary>
        /// 分页工作交流集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号 =0返回所有</param>
        /// <param name="OperatorId">操作人编号</param>
        /// <param name="QueryInfo">工作计划查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.WorkPlan> GetList(int pageSize, int pageIndex, ref int RecordCount, int CompanyId, int OperatorId,EyouSoft.Model.PersonalCenterStructure.QueryWorkPlan QueryInfo)
        {
            IList<EyouSoft.Model.PersonalCenterStructure.WorkPlan> list = new List<EyouSoft.Model.PersonalCenterStructure.WorkPlan>();
            string tableName = "tbl_WorkPlan";
            string fields = "PlanId,PlanNO,Title,Remark,OperatorId,(select ContactName from tbl_CompanyUser where Id=tbl_WorkPlan.OperatorId) as OperatorName,Status,ExpectedDate,ActualDate,CreateTime";
            string primaryKey = "PlanId";
            string orderbyStr = " CreateTime DESC ";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' ");
            if (CompanyId > 0)
                strWhere.AppendFormat(" and CompanyId={0} ", CompanyId);
            if (OperatorId > 0)
            {
                strWhere.AppendFormat(" and ((PlanId in(select PlanId from tbl_WorkPlanAccept where AccetpId={0})) OR (OperatorId={0}) OR (dbo.fn_ValidUserLevDepartManagers({0},OperatorId)>0))", OperatorId);
            }
            if (QueryInfo != null)
            {
                if (!string.IsNullOrEmpty(QueryInfo.Title))
                    strWhere.AppendFormat(" and Title like '%{0}%' ", QueryInfo.Title);
                if(!string.IsNullOrEmpty(QueryInfo.OperatorName))
                    strWhere.AppendFormat(" and OperatorName like '%{0}%' ", QueryInfo.OperatorName);
                if (QueryInfo.LastSTime.HasValue)
                    strWhere.AppendFormat(" and datediff(dd,LastTime,'{0}')<=0 ", QueryInfo.LastSTime.Value.ToString());
                if (QueryInfo.LastETime.HasValue)
                    strWhere.AppendFormat(" and datediff(dd,LastTime,'{0}')>=0 ", QueryInfo.LastETime.Value.ToString());
                if (QueryInfo.Status.HasValue)
                    strWhere.AppendFormat(" and Status={0} ", (int)QueryInfo.Status.Value);
            }
            using (IDataReader dr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref RecordCount, tableName, primaryKey, fields, strWhere.ToString(), orderbyStr))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.PersonalCenterStructure.WorkPlan model = new EyouSoft.Model.PersonalCenterStructure.WorkPlan();
                    model.PlanId = dr.GetInt32(dr.GetOrdinal("PlanId"));
                    model.PlanNO = dr[dr.GetOrdinal("PlanNO")].ToString();
                    model.Title = dr[dr.GetOrdinal("Title")].ToString();
                    model.OperatorName = dr[dr.GetOrdinal("OperatorName")].ToString();
                    model.Remark = dr[dr.GetOrdinal("Remark")].ToString();
                    model.Status = (EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState)int.Parse(dr[dr.GetOrdinal("Status")].ToString());
                    if(!dr.IsDBNull(dr.GetOrdinal("ExpectedDate")))
                        model.ExpectedDate = dr.GetDateTime(dr.GetOrdinal("ExpectedDate"));
                    if(!dr.IsDBNull(dr.GetOrdinal("ActualDate")))
                        model.ActualDate = dr.GetDateTime(dr.GetOrdinal("ActualDate"));
                    model.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 创建接收对象XML
        /// </summary>
        /// <returns></returns>
        private string CreateAcceptXML(IList<EyouSoft.Model.PersonalCenterStructure.WorkPlanAccept> list)
        {
            StringBuilder strXML = new StringBuilder("<ROOT>");
            foreach (EyouSoft.Model.PersonalCenterStructure.WorkPlanAccept model in list)
            {
                strXML.AppendFormat("<AcceptInfo AcceptId=\"{0}\" />", model.AccetpId);
            }
            strXML.Append("</ROOT>");
            return strXML.ToString();
        }
        /// <summary>
        /// 根据接收对象编号获取接收名称
        /// </summary>
        /// <param name="AcceptId">接收对象编号</param>
        /// <returns></returns>
        private string GetAcceptName(int AcceptId)
        {
            string AcceptName = string.Empty;
            EyouSoft.Model.CompanyStructure.ContactPersonInfo userModel = new DAL.CompanyStructure.CompanyUser().GetUserBasicInfo(AcceptId);
            if (userModel != null)
                AcceptName = userModel.ContactName;
            userModel = null;
            return AcceptName;
        }
        #endregion
    }
}
