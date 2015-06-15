using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 描述:客户留言回复实体类
    /// 修改记录:
    /// 1. 2011-3-17 曹胡生 创建
    /// </summary>

    #region 客户留言实体
    public class CustomerMessageModel
    {
        /// <summary>
        /// 留言记录编号
        /// </summary>
        public int MessageId
        {
            get;
            set;
        }

        /// <summary>
        /// 专线公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 留言公司编号
        /// </summary>
        public int MessageCompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 留言标题
        /// </summary>
        public string MessageTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 留言内容
        /// </summary>
        public string MessageContent
        {
            get;
            set;
        }

        /// <summary>
        /// 留言人编号
        /// </summary>
        public int MessagePersonId
        {
            get;
            set;
        }

        /// <summary>
        /// 留言人姓名
        /// </summary>
        public string  MessagePersonName
        {
            get;
            set;
        }

        /// <summary>
        /// 回复人姓名
        /// </summary>
        public string MessageReplyPersonName
        {
            get;
            set;
        }

        /// <summary>
        /// 留言时间
        /// </summary>
        public DateTime? MessageTime
        {
            get;
            set;
        }

        /// <summary>
        /// 留言回复状态
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.ReplyState ReplyState
        {
            get;
            set;
        }
    }
    #endregion

    #region 客户留言回复实体
    public class CustomerMessageReplyModel
    {
        /// <summary>
        /// 留言回复编号
        /// </summary>
        public int ReplyId
        {
            get;
            set;
        }

        /// <summary>
        /// 留言编号
        /// </summary>
        public int MessageId
        {
            get;
            set;
        }

        /// <summary>
        /// 留言回复内容
        /// </summary>
        public string ReplyContent
        {
            get;
            set;
        }

        /// <summary>
        /// 操作人编号
        /// </summary>
        public int ReplyPersonId
        {
            get;
            set;
        }

        /// <summary>
        /// 留言回复人姓名
        /// </summary>
        public string  ReplyPersonName
        {
            get;
            set;
        }

        /// <summary>
        /// 留言回复时间
        /// </summary>
        public DateTime? ReplyTime
        {
            get;
            set;
        }
        /// <summary>
        /// 留言人信息
        /// </summary>
        public CustomerMessageModel MessageInfo { get; set; }
    }
    #endregion
}
