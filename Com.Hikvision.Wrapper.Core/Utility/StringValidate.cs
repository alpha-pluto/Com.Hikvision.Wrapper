/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：StringValidate.cs
 * 
 * 文件功能描述：字符串处理
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace Com.Hikvision.Wrapper.Core.Utility
{
    /// <summary>
    /// 字符串 验证 工具类
    /// </summary>
    public sealed class StringValidate
    {
        /// <summary>
        /// 增加遮罩
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForMask(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.Replace(inStr, @"[\S]{1,3}$", "***");
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 检查字符串是否符合金额数字，允许负数
        /// negetive acceptable
        /// 不符合返回"0.00"
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForMoney(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.IsMatch(Regex.Replace(inStr, @"[^\+\-\d\.]", ""), @"^[\+\-]?[\d]+(\.[\d]{1,})?$") ? inStr : "0.00";
            }
            else
            {
                return "0.00";
            }
        }

        /// <summary>
        /// 检查 字符串是否符合数字类型（正数：整型，浮点型）
        /// Negative unacceptable
        /// 不符合返回 “0.00”
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForNumeric(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.IsMatch(Regex.Replace(inStr, @"[^\d\.]", ""), @"^[\d]+(\.[\d]{1,})?$") ? inStr : "0.00";
            }
            else
            {
                return "0.00";
            }
        }

        /// <summary>
        /// 格式化货币
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForMoneyFormat(string inStr)
        {
            string tNumeric = StringValidateForNumeric(inStr);
            tNumeric += tNumeric.IndexOf('.') > 0 ? "00" : ".00";
            return Regex.Replace(tNumeric, @"(\.\d\d)\d*$", "$1");
        }

        /// <summary>
        /// 检查字符串是否符合给定的格式
        /// </summary>
        /// <param name="inStr">输入字串</param>
        /// <param name="inIntegralDigital">整数最大长度</param>
        /// <param name="inDecimaldigital">小数最大长度</param>
        /// <returns>不符合就返回 0 </returns>
        public static string StringValidateForNumeric(
            string inStr,
            int inIntegralDigital,
            int inDecimaldigital
        )
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                inIntegralDigital = inIntegralDigital < 0 ? 0 : inIntegralDigital;
                inDecimaldigital = inDecimaldigital < 0 ? 0 : inDecimaldigital;

                string pttnNumeric = @"[\d]" + (inIntegralDigital > 1 ? @"{1," + inIntegralDigital + "}" : "");

                pttnNumeric += (
                    inDecimaldigital > 0 ?
                    (@"(\.[\d]" +
                    (inDecimaldigital > 1 ?
                        "{1," + inDecimaldigital + "})?"
                        : ""
                    ) + "")
                    : ""
                );
                return Regex.IsMatch(inStr, @"^" + pttnNumeric + @"$") ? inStr : "0";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 检查字符串是否符合给定的格式
        /// </summary>
        /// <param name="inStr">输入字串</param>
        /// <param name="inIntegralDigital">整数最大长度</param>
        /// <param name="inDecimaldigital">小数最大长度</param>
        /// <param name="inDefaultValue">默认值</param>
        /// <returns>不符合就返回 默认值 </returns>
        public static string StringValidateForNumeric(
            string inStr,
            int inIntegralDigital,
            int inDecimaldigital,
            string inDefaultValue
        )
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                inIntegralDigital = inIntegralDigital < 0 ? 0 : inIntegralDigital;
                inDecimaldigital = inDecimaldigital < 0 ? 0 : inDecimaldigital;

                string pttnNumeric = @"[\d]" + (inIntegralDigital > 1 ? @"{1," + inIntegralDigital + "}" : "");

                pttnNumeric +=
                    (inDecimaldigital > 0 ?
                    (@"(\.[\d]" +
                        (inDecimaldigital > 1 ?
                        "{1," + inDecimaldigital + "})?"
                        : "") + "")
                    : ""
                );
                return
                    Regex.IsMatch(inStr, @"^" + pttnNumeric + @"$") ? inStr : inDefaultValue;
            }
            else
            {
                return inDefaultValue;
            }
        }

        /// <summary>
        /// 查验是否为数字，允许负数
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="inIntegralDigital"></param>
        /// <param name="inDecimaldigital"></param>
        /// <param name="inDefaultValue"></param>
        /// <returns></returns>
        public static string StringValidateForNumericWithNegtiveAccept(
            string inStr,
            int inIntegralDigital,
            int inDecimaldigital,
            string inDefaultValue
        )
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                inIntegralDigital = inIntegralDigital < 0 ? 0 : inIntegralDigital;
                inDecimaldigital = inDecimaldigital < 0 ? 0 : inDecimaldigital;

                string pttnNumeric = @"[\+\-]?[\d]" + (inIntegralDigital > 1 ? @"{1," + inIntegralDigital + "}" : "");

                pttnNumeric +=
                    (inDecimaldigital > 0 ?
                    (@"(\.[\d]" +
                        (inDecimaldigital > 1 ?
                        "{1," + inDecimaldigital + "})?"
                        : "") + "")
                    : ""
                );
                return
                    Regex.IsMatch(inStr, @"^" + pttnNumeric + @"$") ? inStr : inDefaultValue;
            }
            else
            {
                return inDefaultValue;
            }
        }

        /// <summary>
        /// 检查 字符串是否为端口号
        /// 默认返回 25
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForPort(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.IsMatch(inStr, @"^([1-6]\d(?<!6[6-9])\d(?<!65[6-9])\d(?<!655[4-9])\d(?<!6553[6-9])|[1-9]\d{1,3})$") ? inStr : "25";
            }
            else
            {
                return "25";
            }
        }

        /// <summary>
        /// 授权数字 0-255
        /// 默认返回 0 
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForEmpoweredNum(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.IsMatch(inStr, @"^([1-2]\d(?<!2[6-9])\d(?<!25[6-9])|[1-9]\d{0,1}|\d)$") ? inStr : "0";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// email地址字符串过滤,不符合返回""
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForEmail(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.IsMatch(inStr, @"^([a-zA-Z_0-9]+[\.a-z0-9_-]*@[a-z0-9]+([a-z0-9-]+[a-z0-9]+)*(\.[a-z]+)+)*$", RegexOptions.IgnoreCase) ? inStr : "";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// email mask
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForemailMask(string inStr)
        {
            string strEmail = StringValidateForEmail(inStr);
            return Regex.Replace(strEmail, @"([^\@])[^\@]*?([^\@]?\@[^\@]+)", "$1***$2");

        }

        /// <summary>
        /// 判断加入的字符串是否是GUID格式，如果不是或是为空
        /// 则返回Guid.Empty
        /// </summary>
        /// <param name="inStr">输入字符串</param>
        /// <param name="hyphenStrict">是否一定要有连字符号</param>
        /// <returns>返回值</returns>
        public static string StringValidateForGuid(string inStr, bool hyphenStrict = true)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                string pttnGuid = Regex.Replace(@"^(?:(?:(?<o>\{)|\b)[a-f\d]{8}(?:(?<c>\-{$hyphenStrict})[a-f\d]{4}){3}\<c>[a-f\d]{12}(?:(?<-o>\})|\b))(?(o)(?!))$", @"(?i)\{\$hyphenStrict\}", hyphenStrict ? "" : "?");
                return Regex.IsMatch(inStr, pttnGuid, RegexOptions.IgnoreCase) ? inStr : Guid.Empty.ConvertToString(hyphenStrict);//^(\{?)[a-f\d]{8}(\-[a-f\d]{4}){3}\-[a-f\d]{12}\1$

            }
            else
            {
                return Guid.Empty.ConvertToString(hyphenStrict);
            }
        }

        /// <summary>
        /// 返回guid list
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="hyphenStrict">是否必须有连词号</param>
        /// <returns></returns>
        public static string StringValidateForGuidList(string inStr, bool hyphenStrict = true)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                string pttnGuidList = Regex.Replace(@"^([a-f\d]{8}((?<c>\-{$hyphenStrict})[a-f\d]{4}){4}[a-f\d]{8}[\s]*(\,([\s]*[a-f\d]{8}(\<c>[a-f\d]{4}){4}[a-f\d]{8})?){0,})?$", @"(?i)\{\$hyphenStrict\}", hyphenStrict ? "" : "?");
                return Regex.IsMatch(inStr, pttnGuidList, RegexOptions.IgnoreCase) ? inStr : Guid.Empty.ConvertToString(hyphenStrict);
            }
            else
            {
                return Guid.Empty.ConvertToString(hyphenStrict);
            }
        }

        /// <summary>
        /// 普通字符串过滤
        /// 为null则返回""
        /// 为解决在向存储过程
        /// 传递参数时出现错误
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForNormalString(string inStr)
        {
            return string.IsNullOrEmpty(inStr) ? "" : inStr;
        }

        /// <summary>
        /// 检查字符串是否是域名的形式
        /// 不符合就返回空
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForHostname(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.IsMatch(inStr, @"^([\w\-]+\.)*[\w\-]+\.[\w]{2,}$", RegexOptions.IgnoreCase) ? inStr : "";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// IP v4 地址过滤
        /// 如果不合规则
        /// 返回 0.0.0.0
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForIpAddress(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.IsMatch(inStr, @"^(1[\d][\d]|[\d]{1,2}|2[0-4][\d]|25[0-4])(\.(1[\d][\d]|[\d]{1,2}|2[0-4][\d]|25[0-4])){3}$") ? inStr : "0.0.0.0";
            }
            else
            {
                return "0.0.0.0";
            }
        }

        /// <summary>
        /// 检查 字符串 是否 ip地址 或是域名的格式
        /// 不符合就返回空
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForIpAddressOrHostname(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                string tmpRetString = StringValidateForIpAddress(inStr);
                if (string.Compare(tmpRetString, "0.0.0.0") == 0)
                {
                    tmpRetString = StringValidateForHostname(inStr);
                }
                return tmpRetString;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ip地址过滤，
        /// ipv4 或是ipv6
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForIpAddressComplianceV6(string inStr)
        {
            inStr = string.IsNullOrEmpty(inStr) ? "0.0.0.0" : inStr;
            string tmpIPv4 = StringValidateForIpAddress(inStr);
            string pttnIPv6Full = @"^[\da-f]{1,4}(\:[\da-f]{0,4}){7}$";
            string pttnIPv6Abbreviation = @"^[\da-f]{1,4}(\:[\da-f]{0,4}){5}\%[a-f\d]{0,2}$";
            string tmpIPv6 = (Regex.IsMatch(inStr, pttnIPv6Full) || Regex.IsMatch(inStr, pttnIPv6Abbreviation)) ? inStr : "0.0.0.0";
            if (string.Compare(tmpIPv4, "0.0.0.0") == 0)
            {
                return tmpIPv6;
            }
            else
            {
                return tmpIPv4;
            }
        }

        /// <summary>
        /// 判断货币代码是否符合格式(如:CNY  USD JPY )
        /// 如果不符合格式，则返回string.empty
        /// </summary>
        /// <param name="inStr">输入的货币代码</param>
        /// <returns>返回货币代码</returns>
        public static string StringValidateForCurrencyCode(string inStr)
        {
            return Regex.IsMatch(inStr ?? "", @"^[a-z]{3}$", RegexOptions.IgnoreCase) ? inStr : string.Empty;
        }

        /// <summary>
        /// 检查字符串是否符合 消息队列命令 的格式
        /// 不符合返回 noop
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForMessageCommand(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.IsMatch(inStr, @"^[\w]+$", RegexOptions.IgnoreCase) ? inStr : "noop";
            }
            else
            {
                return "noop";
            }
        }

        /// <summary>
        /// 域名过滤，支持字符和ip形式
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForDomain(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.IsMatch(inStr, @"^(https?\:)?\/{2}(?<site>([\w]+[.]){1,}[\w]{2,})[\S]*[\/]?$") ? inStr : "";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 域名过滤
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="defVal">默认值</param>
        /// <returns></returns>
        public static string StringValidateForDomainWithDefaultvalue(string inStr, string defVal)
        {
            return string.IsNullOrEmpty(StringValidateForDomain(inStr)) ? defVal : inStr;
        }

        /// <summary>
        ///  检查 加入 的字符 串 是否为日期
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string StringValidateForDatetime(string inString)
        {
            var retString = string.Empty;

            if (!string.IsNullOrEmpty(inString))
            {
                var retDatetime = DateTime.UtcNow;

                if (DateTime.TryParse(inString, out retDatetime))
                {
                    retString = string.Format("{0:u}", retDatetime).Replace("Z", "");
                }
            }
            return retString;
        }

        /// <summary>
        /// 检查 加入 的字符 串 是否为日期格式 20120801154220368+0800
        /// </summary>
        /// <param name="inString"></param>
        /// <param name="forceCompleted">是否强制完整格式</param>
        /// <param name="isEndDate">是否是结束日期</param>
        /// <returns></returns>
        public static string StringValidateForDatetimeOcean(string inString, bool forceCompleted = false, bool isEndDate = false)
        {
            var pttnDateOcean = @"^[12]\d(?<!2[2-9])\d{2}[01]\d(?<!1[3-9])[0-3]\d(?<!3[2-9])(?:[0-2]\d(?<!2[5-9])[0-5]\d(?:[0-5]\d(?:\d{3}(?:\D[01]\d(?<!1[5-9])[0-5]\d)?)?)?)?$";
            var completedDateSample = "20120801154220368+0800";
            var suffixDateStart = "000000000+0800";
            var suffixDateEnd = "235959999+0800";
            var retString = string.Empty;

            if (!string.IsNullOrEmpty(inString) && Regex.IsMatch(inString, pttnDateOcean))
            {
                retString = inString;

                if (forceCompleted)
                {
                    var diffDateLength = completedDateSample.Length - inString.Length;
                    var appendixDate = (isEndDate ? suffixDateEnd : suffixDateStart).Substring(suffixDateStart.Length - diffDateLength);
                    retString += appendixDate;
                }
                else
                {
                    //如果日期格式只有日期没有时间并且此时间为搜索条件中的结束日期
                    if (inString.Length <= 8 && isEndDate)
                    {
                        retString += "235959";
                    }
                }

            }
            return retString;
        }

        /// <summary>
        /// 检查 加入 的字符 串 是否为日期格式 2014-12-12T00:00:00Z
        /// 
        /// </summary>
        /// <param name="inString"></param>
        /// <param name="forceCompleted">是否强制完整格式</param>
        /// <param name="isEndDate">是否是结束日期</param>
        /// <returns></returns>
        public static string StringValidateForDatetimeISO8601(string inString, bool forceCompleted = false, bool isEndDate = false)
        {
            var pttnDateISO8601 = @"^[12]\d(?<!2[2-9])\d{2}\D[01]\d(?<!1[3-9])\D[0-3]\d(?<!3[2-9])(?:T[0-2]\d(?<!2[5-9])(?:\:[0-5]\d){2}Z)?$";
            var completedDateSample = "2014-12-12T00:00:00Z";
            var suffixDateStart = "T00:00:00Z";
            var suffixDateEnd = "T23:59:59Z";
            var retString = string.Empty;

            if (!string.IsNullOrEmpty(inString) && Regex.IsMatch(inString, pttnDateISO8601))
            {
                retString = inString;

                if (forceCompleted)
                {
                    var diffDateLength = completedDateSample.Length - inString.Length;
                    var appendixDate = (isEndDate ? suffixDateEnd : suffixDateStart).Substring(suffixDateStart.Length - diffDateLength);
                    retString += appendixDate;
                }
                else
                {
                    //如果日期格式只有日期没有时间并且此时间为搜索条件中的结束日期
                    if (inString.Length <= 10 && isEndDate)
                    {
                        retString += "T23:59:59Z";
                    }
                }

            }
            return retString;
        }

        /// <summary>
        /// 检查字符串是否是XML 的格式
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string StringValidateForXml(string inString)
        {
            string retString = string.Empty;

            try
            {
                System.Xml.XmlDocument docXml = new XmlDocument();
                docXml.LoadXml(inString);
                retString = inString;
            }
            catch
            {
                retString = string.Empty;
            }
            return retString;
        }

        /// <summary>
        /// 检查字符串是否是json格式
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string StringValidateForJson(string inString)
        {
            var pttnJsonBracket = @"^(?isx)\x5b(?>((?<b3>(?<![\x5b\x7b\x2c\x3a][\s\t\r\n]*\x22[^\x5d\x7d\x2c\x22]*)\x5b)|(?<-b3>\x5d(?![^\x22\x2c]*\x22))|(?:(?!(?<![\x5b\x7b\x2c\x3a][\s\t\r\n]*\x22[^\x5d\x7d\x2c\x22]*)\x5b|\x5d(?![^\x22\x2c]*\x22)).)*)*(?(b3)(?!)))\x5d$";
            var pttnJsonBrace = @"^(?isx)\x7b(?>((?<b3>(?<![\x5b\x7b\x2c\x3a][\s\t\r\n]*\x22[^\x5d\x7d\x2c\x22]*)\x7b)|(?<-b3>\x7d(?![^\x22\x2c]*\x22))|(?:(?!(?<![\x5b\x7b\x2c\x3a][\s\t\r\n]*\x22[^\x5d\x7d\x2c\x22]*)\x7b|\x7d(?![^\x22\x2c]*\x22)).)*)*(?(b3)(?!)))\x7d$";

            string retString = string.Empty;

            if (!string.IsNullOrEmpty(inString))
            {
                return Regex.IsMatch(inString, pttnJsonBracket) || Regex.IsMatch(inString, pttnJsonBrace) ? inString : "";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 将字符串 转码 （用于正则模板）
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string StringValidateForRegPattern(string inStr)
        {
            string regfilter = inStr;
            regfilter = regfilter.Replace(@"\", @"\\");
            regfilter = regfilter.Replace(@"/", @"\/");
            regfilter = regfilter.Replace("[", @"\[");
            regfilter = regfilter.Replace("]", @"\]");
            regfilter = regfilter.Replace("(", @"\(");
            regfilter = regfilter.Replace(")", @"\)");
            regfilter = regfilter.Replace("*", @"\*");
            regfilter = regfilter.Replace("+", @"\+");
            regfilter = regfilter.Replace(":", @"\:");
            regfilter = regfilter.Replace("?", @"\?");
            regfilter = regfilter.Replace("^", @"\^");
            regfilter = regfilter.Replace("$", @"\$");
            regfilter = regfilter.Replace("{", @"\{");
            regfilter = regfilter.Replace("}", @"\}");
            regfilter = regfilter.Replace("|", @"\|");
            return regfilter;
        }

        /// <summary>
        ///去除HTML标签以及标签中的内容
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string HtmlPeelOff(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return Regex.Replace(inStr, @"<[^><]*>", "", RegexOptions.IgnoreCase);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static UInt16 StringValidateForSQLBit(string inStr)
        {
            if (!string.IsNullOrEmpty(inStr))
            {
                return
                    (UInt16)
                    (Regex.IsMatch(
                        inStr,
                        @"^(true|yes|[1-9][\d]*)$",
                        RegexOptions.IgnoreCase
                    ) ? 1 : 0);
            }
            else
            {
                return (UInt16)0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inBool"></param>
        /// <returns></returns>
        public static UInt16 StringValidateForSQLBit(bool inBool)
        {
            return (UInt16)(inBool ? 1 : 0);
        }

    }
}



