using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-会议记录信息实体
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class MeetingInfo
    {
        #region Model       
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { set; get; }
        /// <summary>
        /// 会议编号
        /// </summary>
        public string MetttingNo { set; get; }
        /// <summary>
        /// 会议主题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 参会人员
        /// </summary>
        public string Personal { set; get; }
        /// <summary>
        /// 会议时间起始
        /// </summary>
        public DateTime BeginDate { set; get; }
        /// <summary>
        /// 会议时间结束
        /// </summary>
        public DateTime EndDate { set; get; }
        /// <summary>
        /// 地点
        /// </summary>
        public string Location { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { set; get; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { set; get; }
        #endregion Model
    }
}
