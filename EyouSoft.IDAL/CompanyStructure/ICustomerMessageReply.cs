using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 描述:客户留言回复接口类
    /// 修改记录:
    /// 1. 2010-03-17 AM 曹胡生 创建
    /// </summary>
    public interface ICustomerMessageReply
    {
        /// 获取留言列表
        /// </summary>
        /// <param name="companyId">组团登录为组团公司编号,专线登录为专线公司编号</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">返回第几页</param>
        /// <param name="recordCount">返回的记录数</param>
        /// <param name="isZhuTuan">True:组团端,False:专线端</param>
        /// <param name="customerMessageModel">留言实体</param>   
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CustomerMessageModel> GetMessageList(int companyId, int pageSize, int pageIndex, ref int recordCount, bool isZhuTuan, EyouSoft.Model.CompanyStructure.CustomerMessageModel customerMessageModel);

        /// <summary>
        /// 根据公司编号与留言编号获得回复内容,组团端传组团公司编号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="messageID">留言编号</param>
        /// <param name="isZhuTuan">True:组团端,False:专线端</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel GetReplyByMessageId(int companyId, int messageID, bool isZhuTuan);

            /// <summary>
        /// 根据留言编号获得留言
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CustomerMessageModel GetMessageById(int Id);

        /// <summary>
        /// 添加一条留言
        /// </summary>
        /// <returns></returns>
        bool AddMessage(EyouSoft.Model.CompanyStructure.CustomerMessageModel CustomerMessageModel);

        /// <summary>
        /// 更新一条留言
        /// </summary>
        /// <returns></returns>
        bool UpdateMessage(EyouSoft.Model.CompanyStructure.CustomerMessageModel CustomerMessageModel);

        /// <summary>
        /// 添加一条留言回复
        /// </summary>
        /// <returns></returns>
        bool AddReply(int companyID, EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel CustomerMessageReplyModel);

        /// <summary>
        /// 更新一条留言回复
        /// </summary>
        /// <returns></returns>
        bool UpdateReply(int companyID, EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel CustomerMessageReplyModel);

        /// <summary>
        /// 删除一条留言
        /// </summary>
        /// <param name="companyID">专线公司编号</param>
        /// <param name="messageID">留言编号</param>
        /// <returns></returns>
        bool DeleteMessage(int companyID, string messageID);
    }
}
