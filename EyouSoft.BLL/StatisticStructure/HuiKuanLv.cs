using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 回款率分析业务逻辑类
    /// </summary>
    /// 汪奇志 2012-02-27
    public class HuiKuanLv
    {
        private readonly EyouSoft.IDAL.StatisticStructure.IHuiKuanLv dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.StatisticStructure.IHuiKuanLv>();

        /// <summary>
        /// default constructor
        /// </summary>
        public HuiKuanLv() { }

        /// <summary>
        /// 获取回款率统计集合，统计截止时间前beforeDays天的回款率信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="tongJiShiJianE">统计截止时间</param>
        /// <param name="beforeDays">统计截止时间前N天</param>
        /// <param name="chaXun">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.HuiKuanLvInfo> GetHuiKuanLv(int companyId, DateTime tongJiShiJianE, int beforeDays, EyouSoft.Model.StatisticStructure.HuiKuanLvChaXunInfo chaXun)
        {
            if (beforeDays > 31) beforeDays = 31;
            if (beforeDays < 1) beforeDays = 1;
            DateTime now = DateTime.Now;
            tongJiShiJianE = tongJiShiJianE.Date;

            if (chaXun == null)
            {
                chaXun =new EyouSoft.Model.StatisticStructure.HuiKuanLvChaXunInfo();
                
                chaXun.LSDate = new DateTime(now.Year, now.Month, 1);
                chaXun.LEDate = chaXun.LSDate.Value.AddMonths(1).AddDays(-1);
                chaXun.OperatorDepartIds = null;
                chaXun.OperatorIds = null;
                chaXun.SFBHWeiShenHe = true;
                chaXun.KeHuDanWei = null;
            }
            
            if (chaXun.LSDate.HasValue && chaXun.LEDate.HasValue&&chaXun.LSDate.Value>chaXun.LEDate)
            {
                var _tmp = chaXun.LSDate;
                chaXun.LSDate = chaXun.LEDate;
                chaXun.LEDate = _tmp;
            }

            IList<EyouSoft.Model.StatisticStructure.HuiKuanLvInfo> items = new List<EyouSoft.Model.StatisticStructure.HuiKuanLvInfo>();

            for (int i = beforeDays; i >= 0; i--)
            {
                items.Add(dal.GetHuiKuanLv(companyId, tongJiShiJianE.AddDays(-i), chaXun));
            }

            items.Add(dal.GetHuiKuanLv(companyId, now, chaXun));

            return items;
        }
    }
}
