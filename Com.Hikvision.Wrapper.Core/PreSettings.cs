/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * 文件名：PreSettings.cs
 * 
 * 文件功能描述： 
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Hikvision.Wrapper.Core
{
    public class PreSettings
    {
        /// <summary>
        /// 连接等待时间
        /// </summary>

        public const uint dwWaitTime = 2000;

        /// <summary>
        /// 失败重试次数
        /// </summary>
        public const uint dwTryTimes = 1;

        public const int enableRecon = 1;

        public const uint dwInterval = 10000;

        /// <summary>
        /// 等待回调函数的时间
        /// </summary>
        public const int durationCallback = 500;

        public const string charset = "GB2312";


    }
}
