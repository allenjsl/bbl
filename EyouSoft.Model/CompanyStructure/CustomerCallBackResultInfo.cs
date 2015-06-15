using System;
namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 实体类CustomerCallBackResult ,回访结果
    /// autor:李焕超
    /// date:2011-1-17
    /// </summary>
    [Serializable]
    public class CustomerCallBackResultInfo
    {
        public CustomerCallBackResultInfo()
        { }
        #region Model
        private int _id;
        private int _customercareforid;
        private int _routeid;
        private string _routename;
        private DateTime _departuretime;
        private byte _journey;
        private byte _meals;
        private byte _hotel;
        private byte _spot;
        private byte _guide;
        private byte _shopping;
        private byte _car;
        private string _remark;
        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 所属回访编号
        /// </summary>
        public int CustomerCareforId
        {
            set { _customercareforid = value; }
            get { return _customercareforid; }
        }
        /// <summary>
        /// 线路编号
        /// </summary>
        public int RouteID
        {
            set { _routeid = value; }
            get { return _routeid; }
        }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName
        {
            set { _routename = value; }
            get { return _routename; }
        }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime DepartureTime
        {
            set { _departuretime = value; }
            get { return _departuretime; }
        }
        /// <summary>
        /// 行程安排
        /// </summary>
        public byte Journey
        {
            set { _journey = value; }
            get { return _journey; }
        }
        /// <summary>
        /// 餐饮质量
        /// </summary>
        public byte meals
        {
            set { _meals = value; }
            get { return _meals; }
        }
        /// <summary>
        /// 酒店环境
        /// </summary>
        public byte Hotel
        {
            set { _hotel = value; }
            get { return _hotel; }
        }
        /// <summary>
        /// 景点安排
        /// </summary>
        public byte Spot
        {
            set { _spot = value; }
            get { return _spot; }
        }
        /// <summary>
        /// 导游服务
        /// </summary>
        public byte Guide
        {
            set { _guide = value; }
            get { return _guide; }
        }
        /// <summary>
        /// 购物安排
        /// </summary>
        public byte Shopping
        {
            set { _shopping = value; }
            get { return _shopping; }
        }
        /// <summary>
        /// 车辆安排
        /// </summary>
        public byte Car
        {
            set { _car = value; }
            get { return _car; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model

    }
}

