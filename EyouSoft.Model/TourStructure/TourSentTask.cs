using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    /// <summary>
    /// 描述:送团任务表实体
    /// 修改记录:
    /// 1. 2011-03-21 AM　曹胡生　创建
    /// </summary>
    public class TourSentTask
    {
        /// <summary>
        /// 团号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 出团时间
        /// </summary>
        public DateTime? LDate { get; set; }
        /// <summary>
        /// 集合时间
        /// </summary>
        public string GatheringTime { get; set; }
        /// <summary>
        /// 去程航班/时间
        /// </summary>
        public string LTraffic { get; set; }
        /// <summary>
        /// 回程航班/时间
        /// </summary>
        public string RTraffic { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PlanPeopleNumber { get; set; }
        /// <summary>
        /// 计调
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourCoordinatorInfo> TourCoordinatorInfo { get; set; }

    }

    /// <summary>
    /// 搜索实体
    /// </summary>
    public class TourSentTaskSearch
    {
        /// <summary>
        /// 出团开始时间
        /// </summary>
        public DateTime? LDate { get; set; }
        /// <summary>
        /// 出团结束时间
        /// </summary>
        public DateTime? LEDate { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
    }
}
