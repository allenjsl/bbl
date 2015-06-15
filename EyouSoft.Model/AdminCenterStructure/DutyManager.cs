using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{   
    /// <summary>
    /// 行政中心-职务管理信息实体
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class DutyManager
    {
        #region Model
        private int _id;
        private int _companyid;
        private string _jobname;
        private string _help;
        private string _requirement;
        private string _remark;
        private int _operatorid;
        private DateTime _issuetime;
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 职务名称
        /// </summary>
        public string JobName
        {
            set { _jobname = value; }
            get { return _jobname; }
        }
        /// <summary>
        /// 职务说明
        /// </summary>
        public string Help
        {
            set { _help = value; }
            get { return _help; }
        }
        /// <summary>
        /// 职务具体要求
        /// </summary>
        public string Requirement
        {
            set { _requirement = value; }
            get { return _requirement; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId
        {
            set { _operatorid = value; }
            get { return _operatorid; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime
        {
            set { _issuetime = value; }
            get { return _issuetime; }
        }
        #endregion Model
    }
}
