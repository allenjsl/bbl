using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-学历信息
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class SchoolInfo
    {
        #region Model
        /// <summary>
        /// 编号
        /// </summary>
        public int Id{ set; get; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public int PersonId { set; get; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { set; get; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { set; get; }
        /// <summary>
        /// 学历
        /// </summary>
        public EyouSoft.Model.EnumType.AdminCenterStructure.DegreeType Degree { set; get; }
        /// <summary>
        /// 所学专业
        /// </summary>
        public string Professional { set; get; }
        /// <summary>
        /// 毕业院校
        /// </summary>
        public string SchoolName { set; get; }
        /// <summary>
        /// 状态(1:毕业，0:在读)
        /// </summary>
        public bool StudyStatus { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        #endregion Model
    }
}
