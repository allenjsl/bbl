using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-个人履历信息实体
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class PersonalHistory
    {
        #region Model
        private int _id;
        private int _personid;
        private DateTime? _startdate;
        private DateTime? _enddate;
        private string _workplace;
        private string _workunit;
        private string _takeup;
        private string _remark;
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 员工编号
        /// </summary>
        public int PersonId
        {
            set { _personid = value; }
            get { return _personid; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate
        {
            set { _startdate = value; }
            get { return _startdate; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }
        /// <summary>
        /// 工作地点
        /// </summary>
        public string WorkPlace
        {
            set { _workplace = value; }
            get { return _workplace; }
        }
        /// <summary>
        /// 工作单位
        /// </summary>
        public string WorkUnit
        {
            set { _workunit = value; }
            get { return _workunit; }
        }
        /// <summary>
        /// 从事职业
        /// </summary>
        public string TakeUp
        {
            set { _takeup = value; }
            get { return _takeup; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
    }
}
