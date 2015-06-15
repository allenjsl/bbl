using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;
using System.Data;
using System.IO;
namespace EyouSoft.DAL.PersonalCenterStructure
{
    /// <summary>
    /// 工作汇报数据层
    /// </summary>
    /// 鲁功源  2011-01-20
    public class WorkReport : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.PersonalCenterStructure.IWorkReport
    {
        #region 变量
        private const string Sql_WorkReport_Delete = "update tbl_WorkReport set IsDelete='1' where ReportId in({0})";
        private EyouSoft.Data.EyouSoftTBL dcDal = null;
        private Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkReport()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
        }
        #endregion

        #region IWorkReport 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">工作汇报实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.PersonalCenterStructure.WorkReport model)
        {
           dcDal= new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
            EyouSoft.Data.WorkReport obj = new EyouSoft.Data.WorkReport() { 
                CompanyId=model.CompanyId,
                DepartmentId=model.DepartmentId,
                Description=model.Description,
                FilePath=model.FilePath,
                OperatorId=model.OperatorId,
                OperatorName=model.OperatorName,
                Status=(byte)model.Status,
                Title=model.Title,
                ReportingTime=model.ReportingTime,
                CheckerId=0,
                IsDelete="0"
            };
            dcDal.WorkReport.InsertOnSubmit(obj);
            dcDal.SubmitChanges();
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">工作汇报实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.PersonalCenterStructure.WorkReport model)
        {
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
            EyouSoft.Data.WorkReport obj = dcDal.WorkReport.FirstOrDefault(item=>item.ReportId==model.ReportId);
            if (obj != null)
            {
                obj.CompanyId = model.CompanyId;
                obj.DepartmentId = model.DepartmentId;
                obj.Description = model.Description;
                obj.FilePath = model.FilePath;
                obj.ReportId = model.ReportId;
                obj.Status = (byte)model.Status;
                obj.Title = model.Title;
                dcDal.SubmitChanges();
            }
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
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
            DbCommand dc = this._db.GetSqlStringCommand(string.Format(Sql_WorkReport_Delete, strIds.TrimEnd(',')));
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 设置审核状态
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <param name="Status">状态</param>
        /// <param name="CheckRemark">审核备注</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetChecked(int Id, EyouSoft.Model.EnumType.PersonalCenterStructure.CheckState Status, string CheckRemark)
        {
            dcDal= new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
            EyouSoft.Data.WorkReport obj = dcDal.WorkReport.FirstOrDefault(item => item.ReportId == Id);
            if (obj != null)
            {
                obj.ReportId = Id;
                obj.Status = (byte)Status;
                obj.CheckTime = DateTime.Now;
                obj.Comment = CheckRemark;
                dcDal.SubmitChanges();
            }
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
        }
        /// <summary>
        /// 获取工作汇报实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>工作汇报实体</returns>
        public EyouSoft.Model.PersonalCenterStructure.WorkReport GetModel(int Id)
        {
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
            var obj = dcDal.WorkReport.FirstOrDefault(item => item.ReportId == Id && item.IsDelete=="0");
            if (obj != null)
            {
                return new EyouSoft.Model.PersonalCenterStructure.WorkReport() { 
                    CheckerId=obj.CheckerId.HasValue?obj.CheckerId.Value:0,
                    CheckTime=obj.CheckTime.HasValue?obj.CheckTime.Value:DateTime.MaxValue,
                    Comment=obj.Comment,
                    CompanyId=obj.CompanyId,
                    DepartmentId=obj.DepartmentId,
                    Description=obj.Description,
                    FilePath=obj.FilePath,
                    OperatorId=obj.OperatorId,
                    OperatorName=obj.OperatorName,
                    ReportId=obj.ReportId,
                    ReportingTime=obj.ReportingTime,
                    Status=(EyouSoft.Model.EnumType.PersonalCenterStructure.CheckState)int.Parse(obj.Status.ToString()),
                    Title=obj.Title,
                    DepartmentName=GetDepartName(obj.DepartmentId)
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
        /// <param name="QueryInfo">工作汇报查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.WorkReport> GetList(int pageSize, int pageIndex, ref int RecordCount, int CompanyId, int OperatorId,EyouSoft.Model.PersonalCenterStructure.QueryWorkReport QueryInfo)
        {
            IList<EyouSoft.Model.PersonalCenterStructure.WorkReport> list = new List<EyouSoft.Model.PersonalCenterStructure.WorkReport>();
            string tableName = "tbl_WorkReport";
            string fields = "ReportId,Title,ReportingTime,OperatorName,Status,(select DepartName from tbl_CompanyDepartment where Id=tbl_WorkReport.DepartmentId) as DepartName ";
            string primaryKey = "ReportId";
            string orderbyStr = " ReportingTime DESC ";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' ");
            if (CompanyId > 0)
                strWhere.AppendFormat(" and CompanyId={0} ", CompanyId);
            if (OperatorId>0)
                strWhere.AppendFormat(" and ((dbo.fn_ValidUserLevDepartManagers({0},OperatorId)>0) OR (OperatorId={0})) ", OperatorId);
            if (QueryInfo != null)
            {
                if (!string.IsNullOrEmpty(QueryInfo.Title))
                    strWhere.AppendFormat(" and Title like '%{0}%' ",QueryInfo.Title);

                if (!string.IsNullOrEmpty(QueryInfo.OperatorName))
                    strWhere.AppendFormat(" and OperatorName like '%{0}%' ", QueryInfo.OperatorName);

                if (QueryInfo.DepartmentId>0)
                    strWhere.AppendFormat(" and DepartmentId={0} ", QueryInfo.DepartmentId);

                if (QueryInfo.CreateSDate.HasValue)
                    strWhere.AppendFormat(" and datediff(dd,ReportingTime,'{0}')<=0 ", QueryInfo.CreateSDate.Value.ToString());

                if (QueryInfo.CreateEDate.HasValue)
                    strWhere.AppendFormat(" and datediff(dd,ReportingTime,'{0}')>=0 ", QueryInfo.CreateEDate.Value.ToString());
            }
            using (IDataReader dr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref RecordCount, tableName, primaryKey, fields, strWhere.ToString(), orderbyStr))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.PersonalCenterStructure.WorkReport model = new EyouSoft.Model.PersonalCenterStructure.WorkReport();
                    if (!dr.IsDBNull(dr.GetOrdinal("ReportId")))
                        model.ReportId = dr.GetInt32(dr.GetOrdinal("ReportId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Title")))
                        model.Title = dr[dr.GetOrdinal("Title")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorName")))
                    model.OperatorName = dr[dr.GetOrdinal("OperatorName")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("DepartName")))
                        model.DepartmentName = dr[dr.GetOrdinal("DepartName")].ToString();
                    if(!dr.IsDBNull(dr.GetOrdinal("Status")))
                        model.Status = (EyouSoft.Model.EnumType.PersonalCenterStructure.CheckState)int.Parse(dr[dr.GetOrdinal("Status")].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("ReportingTime")))
                        model.ReportingTime = dr.GetDateTime(dr.GetOrdinal("ReportingTime"));
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 获取部门名称
        /// </summary>
        /// <param name="DepartId">部门编号</param>
        /// <returns></returns>
        private string GetDepartName(int DepartId)
        {
            string DepartName = string.Empty;
            EyouSoft.Model.CompanyStructure.Department departModel = new DAL.CompanyStructure.Department().GetModel(DepartId);
            if (departModel != null)
                DepartName = departModel.DepartName;
            departModel = null;
            return DepartName;
        }
        #endregion
    }
}
