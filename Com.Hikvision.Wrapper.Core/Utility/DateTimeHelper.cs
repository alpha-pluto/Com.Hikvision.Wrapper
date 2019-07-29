/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29
 * 
 * Daniel.Zhang
 * 
 * 文件名：DateTimeHelper.cs
 * 
 * 文件功能描述：时间相关辅助工具类
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.Globalization;

namespace Com.Hikvision.Wrapper.Core.Utility
{
    /// <summary>
    /// 时间相关辅助工具类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// 将月份转换成字节
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static byte ConvertMonthToByte(int month)
        {
            byte ret = 0;
            try
            {
                if (month > 0 && month <= 12)
                {
                    byte[] bMonth = BitConverter.GetBytes(month);
                    ret = bMonth[0];
                }
            }
            catch
            {

            }
            return ret;
        }

        /// <summary>
        /// 将日期转换成字节
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static byte ConvertDayToByte(int day)
        {
            byte ret = 0;
            try
            {
                if (day > 0 && day <= 31)
                {
                    byte[] bDay = BitConverter.GetBytes(day);
                    ret = bDay[0];
                }
            }
            catch
            {

            }
            return ret;
        }

        /// <summary>
        /// 将小时转化成字节
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static byte ConvertHourToByte(int hour)
        {
            byte ret = 0;
            try
            {
                if (hour >= 0 && hour <= 24)
                {
                    byte[] bHour = BitConverter.GetBytes(hour);
                    ret = bHour[0];
                }
            }
            catch
            {

            }
            return ret;
        }

        /// <summary>
        /// 将分钟转化成字节
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static byte ConvertMinuteToByte(int minute)
        {
            byte ret = 0;
            try
            {
                if (minute >= 0 && minute <= 60)
                {
                    byte[] bMinute = BitConverter.GetBytes(minute);
                    ret = bMinute[0];
                }
            }
            catch
            {

            }
            return ret;
        }

        /// <summary>
        /// 将秒转化成字节
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static byte ConvertSecondToByte(int second)
        {
            byte ret = 0;
            try
            {
                if (second >= 0 && second <= 60)
                {
                    byte[] bMinute = BitConverter.GetBytes(second);
                    ret = bMinute[0];
                }
            }
            catch
            {

            }
            return ret;
        }

        /// <summary>
        /// 这里定义两个日期格式，由于.Net平台的毫秒格式用fff表示，Ocean平台(Java)的毫秒格式用SSS表示。
        /// </summary>
        public static string DatePattern { get { return "yyyyMMddHHmmssfff"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string DatePatternForOcean { get { return "yyyyMMddHHmmssSSS"; } }

        /// <summary>
        /// ISO8601日期格式
        /// </summary>
        public static string DatePatternForISO8601 { get { return "yyyy-MM-ddTHH:mm:sszzzz"; } }

        /// <summary>
        /// Unix时间戳起始时间
        /// </summary>
        public static DateTime BaseTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// 转换DateTime时间到C#时间(UTC+8)
        /// </summary>
        /// <param name="dateTimeFromJson">DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromJson(long dateTimeFromJson)
        {
            return BaseTime.AddTicks((dateTimeFromJson + 8 * 60 * 60) * 10000000);

        }

        /// <summary>
        /// 转换DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromJson">DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromJson(string dateTimeFromJson)
        {
            return GetDateTimeFromJson(long.Parse(dateTimeFromJson));

        }

        /// <summary>
        /// 获取DateTime（UNIX时间戳）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long GetUnixTimestamp(DateTime dateTime)
        {
            return (dateTime.Ticks - BaseTime.Ticks) / 10000000 - 8 * 60 * 60;

        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimestamp()
        {

            System.DateTime current = new DateTime();

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            double ms = (current - startTime).TotalMilliseconds;

            long b = Convert.ToInt64(ms);

            return b;

        }

        /// <summary>
        /// 将时间格式化成 yyyyMMddHHmmssfff 字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String Format(DateTime date)
        {
            return date.ToString(DatePattern);
        }

        /// <summary>
        /// 将时间格式化成 yyyyMMddHHmmssSSS 字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String FormatForOcean(DateTime date)
        {

            String value = date.ToString("yyyyMMddHHmmssfffzzz");

            String newValue = value.Replace(":", "");

            return newValue;

        }

        /// <summary>
        /// 将时间字符串格式化成时间
        /// </summary>
        /// <param name="dateDesc"></param>
        /// <returns></returns>
        public static DateTime FormatFromStr(String dateDesc)
        {
            if (dateDesc.Contains("+") || dateDesc.Contains("-"))
            {

                try
                {

                    IFormatProvider culture = new CultureInfo("zh-CN", true);

                    DateTime datetime = DateTime.ParseExact(dateDesc, "yyyyMMddHHmmssfffzzz", culture);

                    return datetime;

                }

                catch
                {

                }

            }



            IFormatProvider newculture = new CultureInfo("zh-CN", true);

            DateTime newdatetime = DateTime.ParseExact(dateDesc, DatePattern, newculture);

            return newdatetime;



        }
    }
}
