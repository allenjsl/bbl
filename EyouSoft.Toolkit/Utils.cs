using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Globalization;

namespace EyouSoft.Toolkit
{
    public class Utils
    {
        /// <summary>
        /// 获取网站根目录的绝对地址。
        /// </summary>
        /// <value>返回的地址以'/'结束.</value>
        public static Uri AbsoluteWebRoot
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context == null)
                    throw new System.Net.WebException("The current HttpContext is null");

                if (context.Items["absoluteurl"] == null)
                    context.Items["absoluteurl"] = new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + RelativeWebRoot);

                return context.Items["absoluteurl"] as Uri;
            }
        }

        private static string _RelativeWebRoot;
        /// <summary>
        /// 获取网站根目录的相对路径。
        /// </summary>
        /// <value>返回的地址以'/'结束.</value>
        public static string RelativeWebRoot
        {
            get
            {
                if (_RelativeWebRoot == null)
                    _RelativeWebRoot = VirtualPathUtility.ToAbsolute("~/");

                return _RelativeWebRoot;
            }
        }
        /// <summary>
        /// 获取域名后缀。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDomainSuffix(Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);
                    return host.Substring(index + 1);
                }
            }

            return null;
        }
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        /// <summary>
        /// 取得客户的IP数据
        /// </summary>
        /// <returns>客户的IP</returns>
        public static string GetRemoteIP()
        {
            string Remote_IP = "";
            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    Remote_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    Remote_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            catch
            {
            }
            return Remote_IP;
        }
        /// <summary>
        /// 获取当前页面地址
        /// </summary>
        /// <returns></returns>
        public static string GetRequestUrl()
        {
            string RequestUrl = "";
            try
            {
                if (HttpContext.Current.Request.Url != null)
                {
                    RequestUrl = HttpContext.Current.Request.Url.ToString();
                }
            }
            catch
            {
            }
            return RequestUrl;
        }
        /// <summary>
        /// 将127.0.0.1 形式的IP地址转换成10进制整数，这里没有进行任何错误处理
        /// </summary>
        /// <param name="strIP">IP地址转换</param>
        /// <returns>返回0进制整数</returns>
        public static long IpToLong(string strIP)
        {
            if (string.IsNullOrEmpty(strIP))
                return 0;

            string[] strIPs = strIP.Trim().Split('.');
            if (strIPs.Length != 4)
                return 0;

            long[] ip = new long[4];
            for (int i = 0; i < strIPs.Length; i++)
            {
                ip[i] = long.Parse(strIPs[i]);
            }

            return ip[0] * 256 * 256 * 256 + ip[1] * 256 * 256 + ip[2] * 256 + ip[3];
        }

        /// <summary>
        /// 替换XML敏感字符
        /// </summary>
        /// <param name="s">输入字符串</param>
        /// <returns></returns>
        public static string ReplaceXmlSpecialCharacter(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
            }

            return s;
        }

        ///// <summary>
        ///// 将XML敏感字符替换成本身的符号
        ///// </summary>
        ///// <param name="s">输入字符串</param>
        ///// <returns></returns>
        //public static string ReplaceSpecialCharacterToXML(string s)
        //{
        //    if (string.IsNullOrEmpty(s))
        //        return s;

        //    return s.Replace("&quot;", "\"").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&");

        //}

        /// <summary>
        /// 将字符串转化为数字(无符号整数) 若值不是数字返回defaultValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(string key, int defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            int result = 0;
            bool b = Int32.TryParse(key, out result);

            return result;
        }

        /// <summary>
        /// 将字符串转化为数字(无符号整数) 若值不是数字返回0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        /// <summary>
        /// 将字符串转化为decimal 若值不是数字返回defaultValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string key, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            decimal result = 0;
            bool b = decimal.TryParse(key, out result);

            return result;
        }

        /// <summary>
        /// 将字符串转化为decimal 若值不是数字返回0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string key)
        {
            return GetDecimal(key, 0);
        }

        /// <summary>
        /// 将字符串转化为double 若值不是数字返回defaultValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetDouble(string key, double defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            double result = 0;
            bool b = double.TryParse(key, out result);

            return result;
        }

        /// <summary>
        /// 将字符串转化为double 若值不是数字返回0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetDouble(string key)
        {
            return GetDouble(key, 0);
        }

        /// <summary>
        /// 将格式为yyyyMMdd(如：2010-12-05)的字符串转换成日期格式 若不能转换成日期将返回defaultValue
        /// </summary>
        /// <param name="s">要转换的字符串 格式(yyyyMMdd 如：20101205)</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDateTime112(string s, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(s))
            {
                return defaultValue;
            }

            DateTime result = defaultValue;
            bool b = DateTime.TryParse(s.Substring(0, 4) + "-" + s.Substring(4, 2) + "-" + s.Substring(6, 2), out result);

            return result;
        }

        /// <summary>
        /// 将格式为yyyyMMdd(如：2010-12-05)的字符串转换成日期格式 若不能转换成日期将返回DateTime.MinValue
        /// </summary>
        /// <param name="s">要转换的字符串 格式(yyyyMMdd 如：20101205)</param>
        /// <returns></returns>
        public static DateTime GetDateTime112(string s)
        {
            return GetDateTime112(s, DateTime.MinValue);
        }

        /// <summary>
        /// 将字符串转换成日期格式 若不能转换成日期将返回defaultValue
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string s, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(s))
            {
                return defaultValue;
            }

            DateTime result = defaultValue;
            bool b = DateTime.TryParse(s, out result);

            return result;
        }

        /// <summary>
        /// 将字符串转换成日期格式 若不能转换成日期将返回DateTime.MinValue
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <returns></returns>
        public static DateTime GetDateTime(string s)
        {
            return GetDateTime(s, DateTime.MinValue);
        }

        /// <summary>
        /// 获取当前农历日期
        /// </summary>
        /// <returns></returns>
        public static LunarCalendar GetLunarCalendar()
        {
            return GetLunarCalendar(DateTime.Today);
        }

        /// <summary>
        /// 将指定日期转换成农历日期
        /// </summary>
        /// <param name="d">公历日期</param>
        /// <returns></returns>
        public static LunarCalendar GetLunarCalendar(DateTime d)
        {
            LunarCalendar lunarCalendar = new LunarCalendar();

            ChineseLunisolarCalendar chineseLunisolarCalendar = new ChineseLunisolarCalendar();
            lunarCalendar.Year = chineseLunisolarCalendar.GetYear(d);
            lunarCalendar.Month = chineseLunisolarCalendar.GetMonth(d);
            lunarCalendar.Day = chineseLunisolarCalendar.GetDayOfMonth(d);
            lunarCalendar.DaysInMonth = chineseLunisolarCalendar.GetDaysInMonth(lunarCalendar.Year, lunarCalendar.Month);
            lunarCalendar.DaysInYear = chineseLunisolarCalendar.GetDaysInYear(lunarCalendar.Year);

            return lunarCalendar;
        }

        /// <summary>
        /// 根据整型数组生成半角逗号分割的Sql字符串
        /// </summary>
        /// <param name="arrIds">整型数组</param>
        /// <returns>半角逗号分割的Sql字符串</returns>
        public static string GetSqlIdStrByArray(int[] arrIds)
        {
            if (arrIds == null || arrIds.Length <= 0)
                return string.Empty;

            string strTmp = arrIds.Where(i => i > 0).Aggregate(string.Empty, (current, i) => current + (i + ","));
            strTmp = strTmp.Trim(',');

            return strTmp;
        }

        /// <summary>
        /// 根据整型集合生成半角逗号分割的的Sql字符串
        /// </summary>
        /// <param name="ids">整形集合</param>
        /// <returns></returns>
        public static string GetSqlIdStrByList(IList<int> ids)
        {
            if (ids == null || ids.Count <= 0) return "0";
            StringBuilder s = new StringBuilder();
            s.AppendFormat("{0}", ids[0].ToString());

            for (int i = 1; i < ids.Count; i++)
            {
                s.AppendFormat(",{0}", ids[i].ToString());
            }

            return s.ToString();
        }

        #region XElement
        /// <summary>
        /// Get XAttribute Value
        /// </summary>
        /// <param name="XAttribute">xAttribute</param>
        /// <returns>Value</returns>
        public static string GetXAttributeValue(XAttribute xAttribute)
        {
            if (xAttribute == null)
                return string.Empty;

            return xAttribute.Value;
        }

        /// <summary>
        /// Get XAttribute Value
        /// </summary>
        /// <param name="xElement">XElement</param>
        /// <param name="attributeName">Attribute Name</param>
        /// <returns></returns>
        public static string GetXAttributeValue(XElement xElement, string attributeName)
        {
            return GetXAttributeValue(xElement.Attribute(attributeName));
        }

        /// <summary>
        /// Get XElement
        /// </summary>
        /// <param name="xElement">parent xElement</param>
        /// <param name="xName">xName</param>
        /// <returns>XElement</returns>
        public static XElement GetXElement(XElement xElement, string xName)
        {
            XElement x = xElement.Element(xName);

            if (x != null) return x;

            return new XElement(xName);
        }

        /// <summary>
        /// Get XElements
        /// </summary>
        /// <param name="xElement">parent xElement</param>
        /// <param name="xName">xName</param>
        /// <returns>XElements</returns>
        public static IEnumerable<XElement> GetXElements(XElement xElement, string xName)
        {
            var x = xElement.Elements(xName);
            if (x == null)
                return new List<XElement>();

            return x;
        }
        #endregion

        /// <summary>
        /// 将字符串转化为Int可空类型，若不是数字指定的defaultValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? GetIntNull(string key, int? defaultValue)
        {
            if (string.IsNullOrEmpty(key) || !EyouSoft.Common.Function.StringValidate.IsInteger(key))
            {
                return defaultValue;
            }
            int? i = int.Parse(key);
            return i;
        }
        /// <summary>
        /// 将字符串转化为Int可空类型，若不是数字返回null的Int?.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int? GetIntNull(string key)
        {
            return GetIntNull(key, null);
        }

        /// <summary>
        /// get enum value
        /// </summary>
        /// <param name="s">转换的字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T GetEnumValue<T>(string s, T defaultValue)
        {
            if (typeof(T).IsEnum)
            {
                int? _enum = GetIntNull(s, null);
                if (!_enum.HasValue) return defaultValue;

                if (!Enum.IsDefined(typeof(T), _enum.Value)) return defaultValue;

                return (T)(object)_enum.Value;
            }

            return defaultValue;
        }

    }


    /// <summary>
    /// 农历
    /// </summary>
    public class LunarCalendar
    {
        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 日
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 月份中的天数
        /// </summary>
        public int DaysInMonth { get; set; }
        /// <summary>
        /// 年份中的天数
        /// </summary>
        public int DaysInYear { get; set; }
    }
}
