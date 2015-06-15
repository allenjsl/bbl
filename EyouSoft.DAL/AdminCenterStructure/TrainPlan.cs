using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;


namespace EyouSoft.DAL.AdminCenterStructure
{
    public class TrainPlan : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.AdminCenterStructure.ITrainPlan
    {
        private readonly Database _db = null;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public TrainPlan()
        {
            _db = this.SystemStore;
        }
        #endregion 构造函数

        #region 实现接口公共方法
        /// <summary>
        /// 获取培训计划实体信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">培训编号（主键）</param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.TrainPlan GetModel(int CompanyId, int Id)
        {
            EyouSoft.Model.AdminCenterStructure.TrainPlan model = null;
            string StrSql = "SELECT [Id],[PlanTitle],[PlanContent],[OperatorName],IssueTime,TrainPlanFile,(SELECT [AcceptType],[AcceptId] FROM [tbl_TrainPlanAccepts] WHERE TrainPlanId=[tbl_TrainPlan].[Id] FOR XML Raw,Root('Root')) AS TrainPlanAcceptXML FROM tbl_TrainPlan WHERE CompanyId=@CompanyId AND Id=@Id";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            this._db.AddInParameter(dc, "Id", DbType.Int32, Id);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.AdminCenterStructure.TrainPlan()
                    {
                        CompanyId = CompanyId,
                        IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime")),
                        TrainPlanFile = dr.IsDBNull(dr.GetOrdinal("TrainPlanFile")) ? "" : dr.GetString(dr.GetOrdinal("TrainPlanFile")),
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        OperatorName = dr.IsDBNull(dr.GetOrdinal("OperatorName")) ? "" : dr.GetString(dr.GetOrdinal("OperatorName")),
                        PlanContent = dr.IsDBNull(dr.GetOrdinal("PlanContent")) ? "" : dr.GetString(dr.GetOrdinal("PlanContent")),
                        PlanTitle = dr.IsDBNull(dr.GetOrdinal("PlanTitle")) ? "" : dr.GetString(dr.GetOrdinal("PlanTitle")),
                        AcceptList = GetAcceptList(dr["TrainPlanAcceptXML"].ToString())                         
                    };
                    foreach(EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts item in model.AcceptList)
                    {
                        if (item.AcceptType != EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.所有)
                        {
                            if (item.AcceptType == EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType.指定部门)
                            {
                                EyouSoft.Model.CompanyStructure.Department Depart = null;
                                EyouSoft.IDAL.CompanyStructure.IDepartment idal = new EyouSoft.DAL.CompanyStructure.Department();
                                Depart = idal.GetModel(item.AcceptId);
                                if (Depart != null)
                                {
                                    item.AcceptName = Depart.DepartName;
                                }
                                Depart = null;
                                idal = null;
                            }
                            else
                            {
                                EyouSoft.Model.CompanyStructure.ContactPersonInfo Person = null;
                                EyouSoft.IDAL.CompanyStructure.ICompanyUser idal = new EyouSoft.DAL.CompanyStructure.CompanyUser();
                                Person = idal.GetUserBasicInfo(item.AcceptId);
                                if (Person != null)
                                {
                                    item.AcceptName = Person.ContactName;
                                }                                
                                Person = null;
                                idal = null;
                            }
                        }
                    }                   
                }                
            }
            return model;
        }
        /// <summary>
        /// 获取培训计划信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>    
        /// <param name="UserId">当前用户编号</param> 
        /// <param name="DepartmentId">部门编号</param> 
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.TrainPlan> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, int UserId, int DepartmentId)
        {
            IList<EyouSoft.Model.AdminCenterStructure.TrainPlan> ResultList = null;
            string tableName = "tbl_TrainPlan";
            string identityColumnName = "Id";
            string fields = "[Id],[PlanTitle],[PlanContent],[OperatorName],IssueTime,(SELECT [AcceptType],[AcceptId] FROM [tbl_TrainPlanAccepts] WHERE TrainPlanId=[tbl_TrainPlan].[Id] FOR XML Raw,Root('Root')) AS TrainPlanAcceptXML";
            StringBuilder query = new StringBuilder();
            query.AppendFormat(" [CompanyId]={0} AND ", CompanyId);
            query.Append(" ( exists(SELECT 1 FROM tbl_TrainPlanAccepts WHERE TrainPlanId=[tbl_TrainPlan].[id] AND AcceptType=0) OR ");
            query.AppendFormat(" exists(SELECT 1 FROM tbl_TrainPlanAccepts WHERE TrainPlanId=[tbl_TrainPlan].[id] AND AcceptType=1 AND AcceptId={0}) OR ", DepartmentId);
            query.AppendFormat(" exists(SELECT 1 FROM tbl_TrainPlanAccepts WHERE TrainPlanId=[tbl_TrainPlan].[id] AND AcceptType=2 AND AcceptId={0})) ", UserId);
            string orderByString = " IssueTime DESC";
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, identityColumnName, fields, query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.TrainPlan>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.TrainPlan model = new EyouSoft.Model.AdminCenterStructure.TrainPlan()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        OperatorName = dr.IsDBNull(dr.GetOrdinal("OperatorName")) ? "" : dr.GetString(dr.GetOrdinal("OperatorName")),
                        PlanContent = dr.IsDBNull(dr.GetOrdinal("PlanContent")) ? "" : dr.GetString(dr.GetOrdinal("PlanContent")),
                        PlanTitle = dr.IsDBNull(dr.GetOrdinal("PlanTitle")) ? "" : dr.GetString(dr.GetOrdinal("PlanTitle")),
                        IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime")),
                        AcceptList = GetAcceptList(dr["TrainPlanAcceptXML"].ToString())
                    };
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">培训计划实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.TrainPlan model)
        {
            bool IsTrue = false;
            string TrainPlanAcceptXML = CreateTrainPlanAcceptXML(model.AcceptList);
            DbCommand dc = this._db.GetStoredProcCommand("proc_TrainPlan_Insert");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "PlanTitle", DbType.AnsiString, model.PlanTitle);
            this._db.AddInParameter(dc, "PlanContent", DbType.AnsiString, model.PlanContent);
            this._db.AddInParameter(dc, "TrainPlanFile", DbType.AnsiString, model.TrainPlanFile);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "OperatorName", DbType.AnsiString, model.OperatorName);
            this._db.AddInParameter(dc, "TrainPlanAcceptXML", DbType.AnsiString, TrainPlanAcceptXML);
            this._db.AddInParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
            return IsTrue;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">培训计划实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.AdminCenterStructure.TrainPlan model)
        {
            bool IsTrue = false;
            string TrainPlanAcceptXML = CreateTrainPlanAcceptXML(model.AcceptList);
            DbCommand dc = this._db.GetStoredProcCommand("proc_TrainPlan_Update");
            this._db.AddInParameter(dc, "TrainPlanId", DbType.Int32, model.Id);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "PlanTitle", DbType.AnsiString, model.PlanTitle);
            this._db.AddInParameter(dc, "PlanContent", DbType.AnsiString, model.PlanContent);
            this._db.AddInParameter(dc, "TrainPlanFile", DbType.AnsiString, model.TrainPlanFile);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "OperatorName", DbType.AnsiString, model.OperatorName);
            this._db.AddInParameter(dc, "TrainPlanAcceptXML", DbType.AnsiString, TrainPlanAcceptXML);
            this._db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);
            this._db.AddInParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
            return IsTrue;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, int Id)
        {
            bool IsTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_TrainPlan_Delete");
            this._db.AddInParameter(dc, "TrainPlanId", DbType.Int32, Id);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            this._db.AddInParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
            return IsTrue;
        }
        #endregion 实现接口公共方法

        #region 私有方法
        /// <summary>
        /// 生成XML
        /// </summary>
        /// <param name="AcceptList">培训计划接受人信息集合</param>
        /// <returns></returns>
        private string CreateTrainPlanAcceptXML(IList<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts> AcceptList)
        {
            if (AcceptList == null) return "";
            StringBuilder StrBuild = new StringBuilder();
            StrBuild.Append("<ROOT>");
            foreach (EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts model in AcceptList)
            {
                StrBuild.AppendFormat("<TrainPlanAccept AcceptId=\"{0}\"", model.AcceptId);
                StrBuild.AppendFormat(" AcceptType=\"{0}\" ", (int)model.AcceptType);
                StrBuild.AppendFormat(" TrainPlanId=\"{0}\" /> ", (int)model.TrainPlanId);
            }
            StrBuild.Append("</ROOT>");
            return StrBuild.ToString();
        }
        /// <summary>
        /// 获取接受对象集合
        /// </summary>
        /// <param name="AcceptXML">XML字符串</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts> GetAcceptList(string AcceptXML)
        {
            if (string.IsNullOrEmpty(AcceptXML)) return null;
            IList<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts> ResultList = null;
            ResultList = new List<EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts>();
            XElement root = XElement.Parse(AcceptXML);
            var xRow = root.Elements("row");
            foreach (var tmp1 in xRow)
            {                
                EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts model = new EyouSoft.Model.AdminCenterStructure.TrainPlanAccepts()
                {
                    AcceptId = Convert.ToInt32(tmp1.Attribute("AcceptId").Value),
                    AcceptType = (EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType)Enum.Parse(typeof(EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType), tmp1.Attribute("AcceptType").Value.ToString())
                };               
                ResultList.Add(model);
                model = null;
            }
            return ResultList;
        }
       
        #endregion 私有方法
    }
}
