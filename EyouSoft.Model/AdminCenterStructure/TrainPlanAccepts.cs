using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-培训计划接收人信息实体
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class TrainPlanAccepts
    {
        #region Model
        /// <summary>
        /// 培训计划编号
        /// </summary>
        public int TrainPlanId { set; get; }
        /// <summary>
        /// 接收对象类型[所有,指定部门,指定人]
        /// </summary>
        public EyouSoft.Model.EnumType.AdminCenterStructure.AcceptType AcceptType { set; get; }
        /// <summary>
        /// 接收对象编号(所有时为0，指定部门时为部门编号，指定人时为人员编号)
        /// </summary>
        public int AcceptId { set; get; }
        /// <summary>
        /// 接收对象名称(所有时为空，指定部门时为部门名称，指定人时为人员名称)
        /// </summary>
        public string AcceptName { set; get; }
        #endregion Model
    }
}
