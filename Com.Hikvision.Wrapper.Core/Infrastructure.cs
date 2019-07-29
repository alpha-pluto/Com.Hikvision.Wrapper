/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * 文件名：Infrastructure.cs
 * 
 * 文件功能描述：SDK通用函数封装，在通用函数有变化时修改此类 
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;


namespace Com.Hikvision.Wrapper.Core
{
    /// <summary>
    /// SDK通用函数封装类
    /// SDK公共服务函数
    /// </summary>
    public class Infrastructure
    {
        #region preparation

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public bool NetDvrInit()
        {
            return CHCNetSDK.NET_DVR_Init();
        }

        /// <summary>
        /// 释放SDK资源，在程序结束之前调用
        /// </summary>
        /// <returns></returns>
        public bool NetDvrCleanUp()
        {
            return CHCNetSDK.NET_DVR_Cleanup();
        }

        /// <summary>
        /// 设置网络连接超时时间和连接尝试次数
        /// </summary>
        /// <param name="dwWaitTime">超时时间，单位毫秒，取值范围[300,75000]，实际最大超时时间因系统的connect超时时间而不同</param>
        /// <param name="dwTryTimes">连接尝试次数（保留）</param>
        /// <returns></returns>
        public bool NetDvrSetConnectTime(uint dwWaitTime, uint dwTryTimes)
        {
            return CHCNetSDK.NET_DVR_SetConnectTime(dwWaitTime, dwTryTimes);
        }

        /// <summary>
        /// 设置重连功能
        /// </summary>
        /// <param name="dwInterval">重连间隔，单位:毫秒</param>
        /// <param name="bEnableRecon">是否重连，0-不重连，1-重连，参数默认值为1</param>
        /// <returns></returns>
        public bool NetDvrSetReconnect(uint dwInterval, int bEnableRecon)
        {
            return CHCNetSDK.NET_DVR_SetReconnect(dwInterval, bEnableRecon);
        }

        /// <summary>
        /// 用户注册设备
        /// </summary>
        /// <param name="deviceIPAddr">设备IP地址或是静态域名，字符数不大于128个</param>
        /// <param name="devicePort">设备端口号</param>
        /// <param name="user">登录的用户名</param>
        /// <param name="password">用户密码</param>
        /// <param name="deviceInfo">设备信息</param>
        /// <returns></returns>
        public int NetDvrLoginV30(string deviceIPAddr, int devicePort, string user, string password, ref CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo)
        {
            return CHCNetSDK.NET_DVR_Login_V30(deviceIPAddr, devicePort, user, password, ref deviceInfo);
        }

        /// <summary>
        /// 用户注册设备（支持异步登录）。
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        public int NetDvrLoginV40(ref CHCNetSDK.NET_DVR_USER_LOGIN_INFO loginInfo, ref CHCNetSDK.NET_DVR_DEVICEINFO_V40 deviceInfo)
        {
            return CHCNetSDK.NET_DVR_Login_V40(ref loginInfo, ref deviceInfo);
        }

        /// <summary>
        /// 用户注册设备（非异步登录）。
        /// </summary>
        /// <param name="deviceIPAddr"></param>
        /// <param name="devicePort"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        public int NetDvrLoginV40(string deviceIPAddr, int devicePort, string user, string password, ref CHCNetSDK.NET_DVR_DEVICEINFO_V40 deviceInfo)
        {
            CHCNetSDK.NET_DVR_USER_LOGIN_INFO loginInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();

            #region 参数赋值

            loginInfo.sDeviceAddress = new char[CHCNetSDK.NET_DVR_DEV_ADDRESS_MAX_LEN];
            byte[] bDeviceAddress = System.Text.Encoding.Default.GetBytes(deviceIPAddr);
            bDeviceAddress.CopyTo(loginInfo.sDeviceAddress, 0);

            loginInfo.wPort = Convert.ToUInt16(devicePort);

            loginInfo.sUserName = new char[CHCNetSDK.NET_DVR_LOGIN_USERNAME_MAX_LEN];
            byte[] bUserName = System.Text.Encoding.Default.GetBytes(user);
            bUserName.CopyTo(loginInfo.sUserName, 0);

            loginInfo.sPassword = new char[CHCNetSDK.NET_DVR_LOGIN_PASSWD_MAX_LEN];
            byte[] bPassword = System.Text.Encoding.Default.GetBytes(password);
            bPassword.CopyTo(loginInfo.sPassword, 0);

            //是否异步登录：0- 否，1- 是
            loginInfo.bUseAsynLogin = false;

            //代理服务器类型：0- 不使用代理，1- 使用标准代理，2- 使用EHome代理
            loginInfo.byProxyType = 0;

            /*是否使用UTC时间：
             * 0- 不进行转换，默认；
             * 1- 输入输出UTC时间，SDK进行与设备时区的转换；
             * 2- 输入输出平台本地时间，SDK进行与设备时区的转换            
             */
            loginInfo.byUseUTCTime = 0;

            /*
             * 登录模式(不同模式具体含义详见“Remarks”说明)：
             * 0- SDK私有协议，
             * 1- ISAPI协议，
             * 2- 自适应（设备支持协议类型未知时使用，一般不建议）
             */
            loginInfo.byLoginMode = 0;

            /*
             * ISAPI协议登录时是否启用HTTPS(byLoginMode为1时有效)：
             * 0- 不启用，
             * 1- 启用，
             * 2- 自适应（设备支持协议类型未知时使用，一般不建议）
             */
            loginInfo.byHttps = 0;

            #endregion

            return CHCNetSDK.NET_DVR_Login_V40(ref loginInfo, ref deviceInfo);

        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="userId">用户ID号，NET_DVR_Login_V30等登录接口的返回值</param>
        /// <returns></returns>
        public bool NetDvrLogoutV30(int userId)
        {
            return CHCNetSDK.NET_DVR_Logout_V30(userId);
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="userId">用户ID号，NET_DVR_Login_V30等登录接口的返回值</param>
        /// <returns></returns>
        public bool NetDvrLogout(int userId)
        {
            return CHCNetSDK.NET_DVR_Logout(userId);
        }

        /// <summary>
        /// 返回最后操作的错误码
        /// 具体错误码请参看sdk文档
        /// </summary>
        /// <returns></returns>
        public uint NetDvrGetLastError()
        {
            return CHCNetSDK.NET_DVR_GetLastError();
        }

        /// <summary>
        /// 关闭长连接配置接口所创建的句柄，释放资源
        /// </summary>
        /// <param name="hndRemoteConfig"></param>
        /// <returns></returns>
        public bool NetDvrStopRemoteConfig(int hndRemoteConfig)
        {
            return CHCNetSDK.NET_DVR_StopRemoteConfig(hndRemoteConfig);
        }

        #endregion


    }
}
