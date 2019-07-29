/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：ObjectBasicTransformat.cs
 * 
 * 文件功能描述：转换基本处理
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;

namespace Com.Hikvision.Wrapper.Core.Utility
{
    public static class ObjectBasicTransformat
    {
        /// <summary>
        /// 将GUID转换为字符串形式
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="hypeonStrict"></param>
        /// <returns></returns>
        public static string ConvertToString(this Guid guid, bool hypeonStrict = false)
        {
            var retStringGuid = string.Empty;
            //retStringGuid = (guid == null ? string.Empty : Convert.ToString(guid));
            if (!hypeonStrict)
            {
                retStringGuid = System.Text.RegularExpressions.Regex.Replace(retStringGuid, @"\x2d", "");
            }
            return retStringGuid;
        }

        /// <summary>
        /// 生成一个String 类型的 Guid
        /// </summary>
        /// <param name="hypeonStrict">是否保留连字符号</param>
        /// <returns></returns>
        public static string GenerateStringGuid(bool hypeonStrict = false)
        {
            var tmpGuid = Guid.NewGuid();
            return tmpGuid.ConvertToString(hypeonStrict);
        }

        /// <summary>
        /// 将字符串转换为Guid的形式
        /// </summary>
        /// <param name="strGuid"></param>
        /// <returns></returns>
        public static Guid ConvertToGuid(this string strGuid)
        {
            var tmpStringGuid = StringValidate.StringValidateForGuid(strGuid, false);
            tmpStringGuid = System.Text.RegularExpressions.Regex.Replace(tmpStringGuid, @"(?i)[^a-f\d]", "");
            tmpStringGuid = System.Text.RegularExpressions.Regex.Replace(tmpStringGuid, @"(?i)([a-f\d]{8})([a-f\d]{4})([a-f\d]{4})([a-f\d]{4})([a-f\d]{12})", "$1-$2-$3-$4-$5");
            return new Guid(tmpStringGuid);
        }

        /// <summary>
        /// 判断字符串是否符合guid的形式
        /// </summary>
        /// <param name="strGuid"></param>
        /// <returns></returns>
        public static bool IsGuidNullOrEmpty(this string strGuid)
        {
            var tmpGuid = strGuid.ConvertToGuid();
            return tmpGuid == Guid.Empty;
        }

        /// <summary>
        /// 判断Guid是否为null或空
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsGuidNullOrEmpty(this Guid guid)
        {
            return guid == null || guid == Guid.Empty;
        }

    }

}
