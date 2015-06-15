using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.PersonalCenterStructure
{
    #region 交流专区实体
    /// <summary>
    /// 个人中心-交流专区实体
    /// </summary>
    /// 鲁功源  2011-01-17
    [Serializable]
    public class WorkExchange
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkExchange()
        { }
        #endregion

        #region Model
        /// <summary>
        /// 编号
        /// </summary>
        public int ExchangeId
        {
            get;
            set;
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 发布人
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 发布人名称
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 是否匿名
        /// </summary>
        public bool IsAnonymous
        {
            get;
            set;
        }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int Clicks
        {
            get;
            set;
        }
        /// <summary>
        /// 回复次数
        /// </summary>
        public int Replys
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDelete
        {
            get;
            set;
        }
        #endregion Model

        #region 附加属性
        /// <summary>
        /// 发布对象集合
        /// </summary>
        public IList<WorkExchangeAccept> AcceptList
        {
            get;
            set;
        }
        /// <summary>
        /// 交流专区回复集合
        /// </summary>
        public IList<WorkExchangeReply> ReplyList
        {
            get;
            set;
        }
        #endregion

    }
    #endregion

    #region 交流专区发布对象实体
    /// <summary>
    /// 个人中心-交流专区发布对象实体
    /// </summary>
    /// 鲁功源  2011-01-17
    [Serializable]
    public class WorkExchangeAccept
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkExchangeAccept() { }
        #endregion

        #region Model
        /// <summary>
        /// 主题编号
        /// </summary>
        public int ExchangeId
        {
            get;
            set;
        }
        /// <summary>
        /// 发布到的对象类型Id
        /// </summary>
        public EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType AcceptType
        {
            get;
            set;
        }
        /// <summary>
        /// 对象Id（部门Id，组团社Id，没有为0，人员等）
        /// </summary>
        public int AcceptId
        {
            get;
            set;
        }
        /// <summary>
        /// 接收对象名称[根据对象ID取]
        /// </summary>
        public string AcceptName
        {
            get;
            set;
        }
        #endregion Model
    }
    #endregion

    #region 交流专区回复实体
    /// <summary>
    /// 个人中心-交流专区回复实体
    /// </summary>
    /// 鲁功源  2011-01-17
    [Serializable]
    public class WorkExchangeReply
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkExchangeReply() { }
        #endregion

        #region Model
        /// <summary>
        /// 回复编号
        /// </summary>
        public int ReplyId
        {
            get;
            set;
        }
        /// <summary>
        /// 主题编号
        /// </summary>
        public int ExchangeId
        {
            get;
            set;
        }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 回复人
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 回复人名称
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime ReplyTime
        {
            get;
            set;
        }
        /// <summary>
        /// 是否匿名
        /// </summary>
        public bool IsAnonymous
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDelete
        {
            get;
            set;
        }
        #endregion Model
    }
    #endregion
}
