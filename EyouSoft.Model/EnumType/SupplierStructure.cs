using System;

namespace EyouSoft.Model.EnumType
{
    public class SupplierStructure
    {
        #region 酒店星级
        /// <summary>
        /// 酒店星级
        /// </summary>
        public enum HotelStar
        {
            /// <summary>
            /// 3星以下
            /// </summary>
            _3星以下 = 1,
            /// <summary>
            /// 挂3
            /// </summary>
            挂3,
            /// <summary>
            /// 准3
            /// </summary>
            准3,
            /// <summary>
            /// 挂4
            /// </summary>
            挂4,
            /// <summary>
            /// 准4
            /// </summary>
            准4,
            /// <summary>
            /// 挂5
            /// </summary>
            挂5,
            /// <summary>
            /// 准5
            /// </summary>
            准5
        }
        #endregion

        #region 景点星级
        /// <summary>
        /// 景点星级
        /// </summary>
        public enum ScenicSpotStar
        {
            /// <summary>
            /// 1星
            /// </summary>
            _1星 = 1,
            /// <summary>
            /// 2星
            /// </summary>
            _2星,
            /// <summary>
            /// 3星
            /// </summary>
            _3星,
            /// <summary>
            /// 4星
            /// </summary>
            _4星,
            /// <summary>
            /// 5星
            /// </summary>
            _5星
        }
        #endregion
    }
}
