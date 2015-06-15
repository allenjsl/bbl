using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using EyouSoft.Toolkit.DAL;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;


namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 描述:客户留言回复数据类
    /// 修改记录:
    /// 1. 2010-03-17 AM 曹胡生 创建
    /// </summary>
    public class CustomerMessageReply : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICustomerMessageReply
    {
        private readonly Database DB = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomerMessageReply()
        {
            DB = this.SystemStore;
        }

        /// 获取留言列表
        /// </summary>
        /// <param name="companyId">组团登录为组团公司编号,专线登录为专线公司编号</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">返回第几页</param>
        /// <param name="recordCount">返回的记录数</param>
        /// <param name="isZhuTuan">True:组团端,False:专线端</param>
        /// <param name="customerMessageModel">留言实体</param>  
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerMessageModel> GetMessageList(int companyId, int pageSize, int pageIndex, ref int recordCount, bool isZhuTuan, EyouSoft.Model.CompanyStructure.CustomerMessageModel customerMessageModel)
        {
            IList<EyouSoft.Model.CompanyStructure.CustomerMessageModel> items = new List<EyouSoft.Model.CompanyStructure.CustomerMessageModel>();
            EyouSoft.Model.CompanyStructure.CustomerMessageModel item = null;
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Message";
            string primaryKey = "MessageId";
            string orderByString = "MessageTime DESC";
            StringBuilder fields = new StringBuilder();
            #region 要查询的字段
            //fields.Append("tbl_Message.*,(select contactname from dbo.tbl_CompanyUser where id in(select ReplyPersonId from dbo.tbl_MessageReply where MessageId=tbl_Message.MessageId)) as MessageReplyPersonName,(select contactname from dbo.tbl_CompanyUser where id =tbl_Message.MessagePersonId) as MessagePersonName");
            fields.Append("tbl_Message.*,(select ReplyPersonName from dbo.tbl_MessageReply where MessageId =tbl_Message.MessageId) as ReplyPersonName,(select contactname from dbo.tbl_CompanyUser where id =tbl_Message.MessagePersonId) as MessagePersonName");
            #endregion
            #region 拼接查询条件
            if (isZhuTuan)
            {
                cmdQuery.AppendFormat(" MessageCompanyId={0} ", companyId);
            }
            else
            {
                cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            }
            if (customerMessageModel != null)
            {
                if (!String.IsNullOrEmpty(customerMessageModel.MessageTitle))
                {
                    cmdQuery.AppendFormat(" and MessageTitle like '%{0}%'", customerMessageModel.MessageTitle);
                }
            }
            #endregion
            using (IDataReader rdr = DbHelper.ExecuteReader(this.DB, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    item = new EyouSoft.Model.CompanyStructure.CustomerMessageModel()
                    {
                        MessageId = rdr.GetInt32(rdr.GetOrdinal("MessageId")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        MessageCompanyId = rdr.IsDBNull(rdr.GetOrdinal("MessageCompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("MessageCompanyId")),
                        MessageTitle = rdr["MessageTitle"].ToString(),
                        MessageContent = rdr["MessageContent"].ToString(),
                        MessagePersonId = rdr.IsDBNull(rdr.GetOrdinal("MessagePersonId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("MessagePersonId")),
                        MessageTime = rdr.IsDBNull(rdr.GetOrdinal("MessageTime")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("MessageTime")),
                        MessagePersonName = rdr["MessagePersonName"].ToString(),
                        MessageReplyPersonName = rdr["ReplyPersonName"].ToString(),
                        ReplyState = (EyouSoft.Model.EnumType.CompanyStructure.ReplyState)rdr.GetByte(rdr.GetOrdinal("ReplyState"))
                    };
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// 根据公司编号与留言编号获得回复内容,组团端传组团公司编号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="messageID">留言编号</param>
        /// <param name="isZhuTuan">True:组团端,False:专线端</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel GetReplyByMessageId(int companyId, int messageID, bool isZhuTuan)
        {
            EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel item = null;
            StringBuilder SQL = new StringBuilder();
            //if (isZhuTuan)
            //{
            //    SQL.AppendFormat("if Exists(SELECT * FROM tbl_Message where  MessageCompanyId={0} and MessageID={1})", companyId, messageID);
            //}
            //else
            //{
            //    SQL.AppendFormat("if Exists(SELECT * FROM tbl_Message where  CompanyId={0} and MessageID={1})", companyId, messageID);
            //}
            //SQL.Append("SELECT tbl_MessageReply.*,(select ContactName from tbl_CompanyUser where Id=tbl_MessageReply.ReplyPersonId) as ReplyPersonName");
            SQL.Append("SELECT tbl_MessageReply.*");
            //SQL.AppendFormat(",(select MessageTitle as MessageTitle,ReplyState as ReplyState,MessageContent as MessageContent,MessageTime as MessageTime,(select contactname from dbo.tbl_CompanyUser where id =(select MessagePersonId from dbo.tbl_Message where MessageId={0})) as MessagePersonName from dbo.tbl_Message FOR XML RAW,ROOT('ROOT'))  as MessageInfo ", messageID);
            SQL.AppendFormat(" FROM tbl_MessageReply  WHERE MessageId={0}", messageID);

            if (isZhuTuan)
            {
                SQL.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_Message where  MessageCompanyId={0} and MessageID={1}) ", companyId, messageID);
            }
            else
            {
                SQL.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_Message where  CompanyId={0} and MessageID={1}) ", companyId, messageID);
            }
            DbCommand dc = this.SystemStore.GetSqlStringCommand(SQL.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.DB))
            {
                while (rdr.Read())
                {
                    item = new EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel()
                    {
                        MessageId = rdr.IsDBNull(rdr.GetOrdinal("MessageId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("MessageId")),
                        ReplyId = rdr.IsDBNull(rdr.GetOrdinal("ReplyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ReplyId")),
                        ReplyContent = rdr["ReplyContent"].ToString(),
                        ReplyPersonId = rdr.IsDBNull(rdr.GetOrdinal("ReplyPersonId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ReplyPersonId")),
                        ReplyPersonName = rdr["ReplyPersonName"].ToString(),
                        ReplyTime = rdr.IsDBNull(rdr.GetOrdinal("ReplyTime")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("ReplyTime")),
                        MessageInfo = GetMessageById(messageID)
                    };
                    //if (!rdr.IsDBNull(rdr.GetOrdinal("MessageInfo")))
                    //{
                    //    XMLConvert(item, rdr["MessageInfo"].ToString());
                    //}
                }
                if (item == null)
                {
                    item = new EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel()
                    {
                        MessageInfo=GetMessageById(messageID)
                    };
                }
                return item;
            }
        }

        /// <summary>
        /// 根据留言编号获得留言
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerMessageModel GetMessageById(int Id)
        {
            StringBuilder SQL = new StringBuilder();
            EyouSoft.Model.CompanyStructure.CustomerMessageModel item = null;
            SQL.AppendFormat("select *,(select contactname from dbo.tbl_CompanyUser where id =(select MessagePersonId from dbo.tbl_Message where MessageId={0})) as MessagePersonName from dbo.tbl_Message where MessageId={0}", Id);
            DbCommand dc = this.SystemStore.GetSqlStringCommand(SQL.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.DB))
            {
                while (rdr.Read())
                {
                    item = new EyouSoft.Model.CompanyStructure.CustomerMessageModel()
                    {
                        MessageId = rdr.IsDBNull(rdr.GetOrdinal("MessageId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("MessageId")),
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        MessageCompanyId = rdr.IsDBNull(rdr.GetOrdinal("MessageCompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("MessageCompanyId")),
                        MessageContent = rdr["MessageContent"].ToString(),
                        MessagePersonName = rdr["MessagePersonName"].ToString(),
                        MessagePersonId = rdr.IsDBNull(rdr.GetOrdinal("MessageId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("MessageId")),
                        MessageTitle = rdr["MessageTitle"].ToString(),

                        MessageTime = rdr.IsDBNull(rdr.GetOrdinal("MessageTime")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("MessageTime")),
                        ReplyState = (EyouSoft.Model.EnumType.CompanyStructure.ReplyState)(int)rdr.GetByte(rdr.GetOrdinal("ReplyState"))
                    };
                }
                return item;
            }
        }

        /// <summary>
        /// 添加一条留言
        /// </summary>
        /// <returns></returns>
        public bool AddMessage(EyouSoft.Model.CompanyStructure.CustomerMessageModel CustomerMessageModel)
        {
            string SQL = "INSERT INTO tbl_Message(CompanyId,MessageCompanyId,MessageTitle,MessageContent,MessagePersonId,MessageTime,ReplyState) VALUES(@CompanyId,@MessageCompanyId,@MessageTitle,@MessageContent,@MessagePersonId,@MessageTime,0)";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, CustomerMessageModel.CompanyId);
            this.DB.AddInParameter(dc, "MessageCompanyId", DbType.Int32, CustomerMessageModel.MessageCompanyId);
            this.DB.AddInParameter(dc, "MessageTitle", DbType.String, CustomerMessageModel.MessageTitle);
            this.DB.AddInParameter(dc, "MessageContent", DbType.String, CustomerMessageModel.MessageContent);
            this.DB.AddInParameter(dc, "MessagePersonId", DbType.Int32, CustomerMessageModel.MessagePersonId);
            this.DB.AddInParameter(dc, "MessageTime", DbType.DateTime, CustomerMessageModel.MessageTime);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 更新一条留言
        /// </summary>
        /// <returns></returns>
        public bool UpdateMessage(EyouSoft.Model.CompanyStructure.CustomerMessageModel CustomerMessageModel)
        {
            string SQL = "UPDATE tbl_Message SET CompanyId=@CompanyId,MessageCompanyId=@MessageCompanyId,MessageTitle=@MessageTitle,MessageContent=@MessageContent,MessagePersonId=@MessagePersonId,MessageTime=@MessageTime,ReplyState=0 WHERE MessageId=@MessageId";
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            this.DB.AddInParameter(dc, "MessageId", DbType.Int32, CustomerMessageModel.MessageId);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, CustomerMessageModel.CompanyId);
            this.DB.AddInParameter(dc, "MessageCompanyId", DbType.Int32, CustomerMessageModel.MessageCompanyId);
            this.DB.AddInParameter(dc, "MessageTitle", DbType.String, CustomerMessageModel.MessageTitle);
            this.DB.AddInParameter(dc, "MessageContent", DbType.String, CustomerMessageModel.MessageContent);
            this.DB.AddInParameter(dc, "MessagePersonId", DbType.Int32, CustomerMessageModel.MessagePersonId);
            this.DB.AddInParameter(dc, "MessageTime", DbType.DateTime, CustomerMessageModel.MessageTime);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 添加一条留言回复
        /// </summary>
        /// <returns></returns>
        public bool AddReply(int companyID, EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel CustomerMessageReplyModel)
        {
            DbCommand dc = this.DB.GetStoredProcCommand("proc_MessageReply_Add");
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, companyID);
            this.DB.AddInParameter(dc, "MessageId", DbType.Int32, CustomerMessageReplyModel.MessageId);
            this.DB.AddInParameter(dc, "ReplyPersonId", DbType.Int32, CustomerMessageReplyModel.ReplyPersonId);
            this.DB.AddInParameter(dc, "ReplyContent", DbType.String, CustomerMessageReplyModel.ReplyContent);
            this.DB.AddInParameter(dc, "ReplyState", DbType.Byte, (int)EyouSoft.Model.EnumType.CompanyStructure.ReplyState.已回复);
            this.DB.AddInParameter(dc, "ReplyPersonName", DbType.String, CustomerMessageReplyModel.ReplyPersonName);
            this.DB.AddInParameter(dc, "ReplyTime", DbType.DateTime, CustomerMessageReplyModel.ReplyTime);
            this.DB.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, DB);
            object Result = this.DB.GetParameterValue(dc, "Result");
            return int.Parse(Result.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 更新一条留言回复
        /// </summary>
        /// <returns></returns>
        public bool UpdateReply(int companyID, EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel CustomerMessageReplyModel)
        {
            DbCommand dc = this.DB.GetStoredProcCommand("proc_MessageReply_Update");
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, companyID);
            this.DB.AddInParameter(dc, "ReplyId", DbType.Int32, CustomerMessageReplyModel.ReplyId);
            this.DB.AddInParameter(dc, "MessageId", DbType.Int32, CustomerMessageReplyModel.MessageId);
            this.DB.AddInParameter(dc, "ReplyPersonId", DbType.Int32, CustomerMessageReplyModel.ReplyPersonId);
            this.DB.AddInParameter(dc, "ReplyContent", DbType.String, CustomerMessageReplyModel.ReplyContent);
            this.DB.AddInParameter(dc, "ReplyState", DbType.Byte, (int)EyouSoft.Model.EnumType.CompanyStructure.ReplyState.已回复);
            this.DB.AddInParameter(dc, "ReplyPersonName", DbType.String, CustomerMessageReplyModel.ReplyPersonName);
            this.DB.AddInParameter(dc, "ReplyTime", DbType.DateTime, CustomerMessageReplyModel.ReplyTime);
            this.DB.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, DB);
            object Result = this.DB.GetParameterValue(dc, "Result");
            return int.Parse(Result.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除一条留言
        /// </summary>
        /// <param name="companyID">专线公司编号</param>
        /// <param name="messageID">留言编号</param>
        /// <returns></returns>
        public bool DeleteMessage(int companyID, string messageID)
        {
            string SQL = string.Format("delete from tbl_Message where MessageId in({1}) and Exists(select * from tbl_Message where CompanyId={0})", companyID, messageID);
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// XML转换成留言实体
        /// </summary>
        /// <param name="model"></param>
        private void XMLConvert(EyouSoft.Model.CompanyStructure.CustomerMessageReplyModel model1, string MessageInfo)
        {
            System.Xml.Linq.XElement root = System.Xml.Linq.XElement.Parse(MessageInfo);
            var xRow = root.Element("row");
            EyouSoft.Model.CompanyStructure.CustomerMessageModel Model = new EyouSoft.Model.CompanyStructure.CustomerMessageModel();
            if (xRow != null)
            {
                Model.MessageTitle = EyouSoft.Toolkit.Utils.GetXAttributeValue(xRow, "MessageTitle");
                Model.MessageTime = DateTime.Parse(EyouSoft.Toolkit.Utils.GetXAttributeValue(xRow, "MessageTime"));
                Model.MessageContent = EyouSoft.Toolkit.Utils.GetXAttributeValue(xRow, "MessageContent");
                Model.MessagePersonName = EyouSoft.Toolkit.Utils.GetXAttributeValue(xRow, "MessagePersonName");
                Model.ReplyState = (EyouSoft.Model.EnumType.CompanyStructure.ReplyState)int.Parse(EyouSoft.Toolkit.Utils.GetXAttributeValue(xRow, "ReplyState"));
                model1.MessageInfo = Model;
            }
        }
    }
}
