using System;
namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 实体类CustomerMarketingInfo 客户营销活动策划
    /// autor:李焕超
    /// date:2011-1-17
    /// </summary>
    [Serializable]
    public class CustomerMarketingInfo
    {
        public CustomerMarketingInfo()
        { }
        #region Model
        private int _id;
        private DateTime _time;
        private string _theme;
        private string _content;
        private string _participant;
        private string _effect;
        private string _sponsor;
        private byte _state;
        private int _companyid;
        private int _operatorid;
        private DateTime _issuetime;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 活动时间
        /// </summary>
        public DateTime Time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// 活动主题
        /// </summary>
        public string Theme
        {
            set { _theme = value; }
            get { return _theme; }
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 参加者
        /// </summary>
        public string Participant
        {
            set { _participant = value; }
            get { return _participant; }
        }
        /// <summary>
        /// 活动效果
        /// </summary>
        public string Effect
        {
            set { _effect = value; }
            get { return _effect; }
        }
        /// <summary>
        /// 主办者
        /// </summary>
        public string Sponsor
        {
            set { _sponsor = value; }
            get { return _sponsor; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public byte State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 添加人
        /// </summary>
        public int OperatorId
        {
            set { _operatorid = value; }
            get { return _operatorid; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime
        {
            set { _issuetime = value; }
            get { return _issuetime; }
        }
        #endregion Model

    }
}

