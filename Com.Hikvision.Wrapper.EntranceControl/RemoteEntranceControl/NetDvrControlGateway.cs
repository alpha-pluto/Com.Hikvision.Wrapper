/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29
 * 
 * Daniel.Zhang
 * 
 * 文件名：NetDvrControlGateway.cs
 * 
 * 文件功能描述：开关门远程控制
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.ComponentModel;
using Com.Hikvision.Wrapper.Core;

namespace Com.Hikvision.Wrapper.EntranceControl.RemoteEntranceControl
{
    /// <summary>
    /// 命令值： 0-关闭， 1-打开， 2-常开， 3-常关
    /// </summary>
    public enum DwStatic
    {
        [Description("关闭")]
        Close = 0,

        [Description("打开")]
        Open = 1,

        [Description("常开")]
        AlwaysOpen = 2,

        [Description("常关")]
        AlwaysClosed = 3
    }

    /// <summary>
    /// 开关门远程控制底层封装类
    /// </summary>
    public class NetDvrControlGateway
    {

        /// <summary>
        /// 开关门远程控制
        /// 调用SDK底层函数 NET_DVR_ControlGateway
        /// </summary>
        /// <param name="userId">NET_DVR_Login_V40等登录接口的返回值</param>
        /// <param name="gatewayIndex">门禁序号（楼层编号、锁ID），从1开始，-1表示对所有门（或者梯控的所有楼层）进行操作 </param>
        /// <param name="dwStatic">命令值：0- 关闭（对于梯控，表示受控），1- 打开（对于梯控，表示开门），2- 常开（对于梯控，表示自由、通道状态），3- 常关（对于梯控，表示禁用），4- 恢复（梯控，普通状态），5- 访客呼梯（梯控），6- 住户呼梯（梯控） </param>
        /// <returns></returns>
        public bool ControlGateway(int userId, int gatewayIndex, uint dwStatic)
        {
            return CHCNetSDK.NET_DVR_ControlGateway(userId, gatewayIndex, dwStatic);
        }

        /// <summary>
        /// 开门
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gatewayIndex"></param>
        /// <returns></returns>
        public bool ControlGatewayOpen(int userId, int gatewayIndex)
        {
            return ControlGateway(userId, gatewayIndex, Convert.ToUInt32(DwStatic.Open));
        }

        /// <summary>
        /// 关门
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gatewayIndex"></param>
        /// <returns></returns>
        public bool ControlGatewayClose(int userId, int gatewayIndex)
        {
            return ControlGateway(userId, gatewayIndex, Convert.ToUInt32(DwStatic.Close));
        }
    }
}
