/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * 文件名：ControlGateway.cs
 * 
 * 文件功能描述：开关门远程控制封装(服务层) 
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using Com.Hikvision.Wrapper.Core;
using Com.Hikvision.Wrapper.EntranceControl.RemoteEntranceControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Hikvision.Wrapper.Service.EntranceControl.RemoteEntranceControl
{
    /// <summary>
    /// 开关门远程控制类(服务层)
    /// </summary>
    public class ControlGateway
    {
        //
        private static readonly Core.Infrastructure coreInfrastructure = new Infrastructure();

        //控制开关门底层逻辑
        private static readonly NetDvrControlGateway logicControlGateway = new NetDvrControlGateway();

        /// <summary>
        /// 远程开门
        /// </summary>
        /// <param name="message">返回信息</param>
        /// <param name="dwStatic">开/关门</param>
        /// <param name="deviceIPAddr">设备 IP 地址</param>
        /// <param name="devicePort">设备服务端口</param>
        /// <param name="gatewayIdx">门禁序号</param>
        /// <param name="user">登录用户名</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        private bool RemoteControlGateway(
            out string message,
            DwStatic dwStatic,
            string deviceIPAddr,
            int devicePort,
            int gatewayIdx,
            string user = "******",
            string password = "******")
        {
            bool retFlag = false;
            uint errCode = 0;
            int iUserId = -1;

            message = string.Empty;

            //初始化
            coreInfrastructure.NetDvrInit();

            //设置连接超时时间与重连功能
            //CHCNetSDK.NET_DVR_SetConnectTime(PreSettings.dwWaitTime, PreSettings.dwTryTimes);
            coreInfrastructure.NetDvrSetConnectTime(PreSettings.dwWaitTime, PreSettings.dwTryTimes);

            //CHCNetSDK.NET_DVR_SetReconnect(PreSettings.dwInterval, PreSettings.enableRecon);
            coreInfrastructure.NetDvrSetReconnect(PreSettings.dwInterval, PreSettings.enableRecon);

            //CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

            ////登录设备 Login the device
            //iUserId = coreInfrastructure.NetDvrLoginV30(deviceIPAddr, devicePort, user, password, ref deviceInfo);

            CHCNetSDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();

            //登录设备 Login the device
            iUserId = coreInfrastructure.NetDvrLoginV40(deviceIPAddr, devicePort, user, password, ref struDeviceInfoV40);

            if (iUserId < 0)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();//CHCNetSDK.NET_DVR_GetLastError();
                message = "NET_DVR_Login_V30 failed, error code= " + errCode; //登录失败，输出错误号
                //CHCNetSDK.NET_DVR_Cleanup();
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            retFlag = logicControlGateway.ControlGateway(iUserId, gatewayIdx, Convert.ToUInt32(dwStatic));

            if (!retFlag)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();//CHCNetSDK.NET_DVR_GetLastError();
                message = "NET_DVR_ControlGateway failed (" + dwStatic.ToString() + "), error code= " + errCode; //开启失败，输出错误号
                //注销用户
                //CHCNetSDK.NET_DVR_Logout(iUserId);
                coreInfrastructure.NetDvrLogoutV30(iUserId);
                //释放 SDK 资源
                //CHCNetSDK.NET_DVR_Cleanup();
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            System.Threading.Thread.Sleep(PreSettings.durationCallback);
            //注销用户
            //CHCNetSDK.NET_DVR_Logout(iUserId);
            coreInfrastructure.NetDvrLogout(iUserId);
            //释放 SDK 资源
            //CHCNetSDK.NET_DVR_Cleanup();
            coreInfrastructure.NetDvrCleanUp();

            return retFlag;
        }

        /// <summary>
        /// 开门远程控制
        /// </summary>
        /// <param name="message">返回信息</param>
        /// <param name="deviceIPAddr">设备 IP 地址</param>
        /// <param name="devicePort">设备服务端口</param>
        /// <param name="gatewayIdx">门禁序号</param>
        /// <param name="user">登录用户名</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public bool RemoteControlGatewayOpen(
            out string message,
            string deviceIPAddr,
            int devicePort,
            int gatewayIdx = 1,
            string user = "******",
            string password = "******")
        {
            bool retFlag = false;
            message = string.Empty;
            retFlag = RemoteControlGateway(out message, DwStatic.Open, deviceIPAddr, devicePort, gatewayIdx, user, password);
            return retFlag;
        }

        /// <summary>
        /// 关门远程控制
        /// </summary>
        /// <param name="message">返回信息</param>
        /// <param name="deviceIPAddr">设备 IP 地址</param>
        /// <param name="devicePort">设备服务端口</param>
        /// <param name="gatewayIdx">门禁序号</param>
        /// <param name="user">登录用户名</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public bool RemoteControlGatewayClose(
            out string message,
            string deviceIPAddr,
            int devicePort,
            int gatewayIdx = 1,
            string user = "******",
            string password = "******")
        {
            bool retFlag = false;
            message = string.Empty;
            retFlag = RemoteControlGateway(out message, DwStatic.Close, deviceIPAddr, devicePort, gatewayIdx, user, password);
            return retFlag;
        }
    }
}
