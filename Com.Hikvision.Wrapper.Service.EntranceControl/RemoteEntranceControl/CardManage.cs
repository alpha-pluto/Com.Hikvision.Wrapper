/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：CardManage.cs
 * 
 * 文件功能描述：卡片管理服务调用封装类
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/

using System;
using System.Runtime.InteropServices;
using Com.Hikvision.Wrapper.Core;
using Com.Hikvision.Wrapper.EntranceControl.RemoteEntranceControl;

namespace Com.Hikvision.Wrapper.Service.EntranceControl.RemoteEntranceControl
{
    /// <summary>
    /// 卡片管理服务调用封装类
    /// </summary>
    public class CardManage
    {
        private string errCodeCallback = string.Empty;

        //临时存放卡信息
        private CHCNetSDK.NET_DVR_CARD_CFG_V50 struCardInfo = new CHCNetSDK.NET_DVR_CARD_CFG_V50();

        //
        private static readonly Core.Infrastructure coreInfrastructure = new Infrastructure();

        //卡片管理底层逻辑
        private static readonly NetDvrCardManage logicCardManage = new NetDvrCardManage();

        /// <summary>
        /// 获取卡消息是否完成
        /// </summary>
        bool bGetCardCfgFinish = false;

        /// <summary>
        /// 设置卡消息是否完成 
        /// </summary>
        bool bSetCardCfgFinish = false;

        #region delegate instantiate

        public CHCNetSDK.RemoteConfigCallback GetRemoteConfigDelegate { get; private set; }

        public CHCNetSDK.RemoteConfigCallback SetRemoteConfigDelegare { get; private set; }

        /// <summary>
        /// 处理卡查询回调的函数
        /// </summary>
        /// <param name="type">状态</param>
        /// <param name="buffer">存放数据的缓冲区指针，获取音量时dwType状态无效，lpBuffer对应4字节声音强度</param>
        /// <param name="bufferLength">缓冲区大小</param>
        /// <param name="pUserData">用户数据</param>
        private void ProcessCardQueryCallback(uint type, IntPtr pBuffer, uint bufferLength, IntPtr pUserData)
        {
            if (pUserData == null)
                return;

            switch (type)
            {
                case (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_DATA:
                    CHCNetSDK.NET_DVR_CARD_CFG_V50 struCardCfg = new CHCNetSDK.NET_DVR_CARD_CFG_V50();
                    struCardCfg.Init();
                    struCardCfg = (CHCNetSDK.NET_DVR_CARD_CFG_V50)Marshal.PtrToStructure(pBuffer, typeof(CHCNetSDK.NET_DVR_CARD_CFG_V50));
                    struCardInfo = struCardCfg;
                    break;
                case (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_STATUS:
                    uint status = (uint)Marshal.ReadInt32(pBuffer);
                    if (status == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_SUCCESS)
                    {
                        bGetCardCfgFinish = true;
                    }
                    else if (status == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_FAILED)
                    {

                    }
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pBuffer"></param>
        /// <param name="bufferLen"></param>
        /// <param name="pUserData"></param>
        private void ProcessCardInsertCallback(uint type, IntPtr pBuffer, uint bufferLen, IntPtr pUserData)
        {

            if (pUserData == null)
            {
                return;
            }

            if (type != (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_STATUS)
            {
                return;
            }
            uint dwStatus = (uint)Marshal.ReadInt32(pBuffer);

            if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_PROCESSING)
            {
                errCodeCallback = string.Format("Send SUCC,CardNO:{0}", System.Text.Encoding.GetEncoding(PreSettings.charset).GetString(struCardInfo.byCardNo).TrimEnd('\0'));
            }
            else if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_FAILED)
            {
                errCodeCallback = "NET_DVR_SET_CARD_CFG_V50 Set Failed";
            }
            else if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_SUCCESS)
            {
                bSetCardCfgFinish = true;
                errCodeCallback = "NET_DVR_SET_CARD_CFG_V50 Set finish";
            }
            else if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_EXCEPTION)
            {
                errCodeCallback = "NET_DVR_SET_CARD_CFG_V50 Set Exception";
            }
        }

        #endregion

        #region query

        /// <summary>
        /// 查询卡数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="employeeNo"></param>
        /// <param name="deviceIPAddr"></param>
        /// <param name="devicePort"></param>
        /// <param name="cardNo"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="cbGetRemoteConfig"></param>
        /// <returns></returns>
        public bool CardQuery(
            out string message,
            out uint employeeNo,
            string deviceIPAddr,
            int devicePort,
            string cardNo,
            string user = "admin",
            string password = "",
            CHCNetSDK.RemoteConfigCallback cbGetRemoteConfig = null)
        {
            bool ret = false;
            message = string.Empty;
            employeeNo = 0;
            uint errCode = 0;
            int iUserId = -1;
            IntPtr pUserData = IntPtr.Zero;
            //长连接句柄
            int hndRemoteConfig = -1;
            //长连接数据发送是否成功
            bool retSendRemoteConfig = false;
            bool retCloseRemoteConfig = false;

            #region init

            //初始化
            coreInfrastructure.NetDvrInit();

            //设置连接超时时间与重连功能
            //CHCNetSDK.NET_DVR_SetConnectTime(PreSettings.dwWaitTime, PreSettings.dwTryTimes);
            coreInfrastructure.NetDvrSetConnectTime(PreSettings.dwWaitTime, PreSettings.dwTryTimes);

            //CHCNetSDK.NET_DVR_SetReconnect(PreSettings.dwInterval, PreSettings.enableRecon);
            coreInfrastructure.NetDvrSetReconnect(PreSettings.dwInterval, PreSettings.enableRecon);

            #endregion

            #region login

            CHCNetSDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();

            //登录设备 Login the device
            iUserId = coreInfrastructure.NetDvrLoginV40(deviceIPAddr, devicePort, user, password, ref struDeviceInfoV40);

            if (iUserId < 0)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();
                message = "NET_DVR_Login_V40 failed, error code= " + errCode; //登录失败，输出错误号
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            #endregion

            #region start remote config 启动长连接配置

            if (null == cbGetRemoteConfig)
                GetRemoteConfigDelegate = new CHCNetSDK.RemoteConfigCallback(ProcessCardQueryCallback);
            else
                GetRemoteConfigDelegate = cbGetRemoteConfig;

            hndRemoteConfig = logicCardManage.StartRemoteConfigForCardQuery(iUserId, GetRemoteConfigDelegate, pUserData, cardNo);

            if (hndRemoteConfig < 0)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();
                message = "NET_DVR_StartRemoteConfig, error code= " + errCode; //登录失败，输出错误号
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            #endregion

            #region send remote config 发送长连接数据

            retSendRemoteConfig = logicCardManage.SendRemoteConfigForCardQuery(hndRemoteConfig, cardNo);

            if (!retSendRemoteConfig)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();
                message = "NET_DVR_SendRemoteConfig, error code= " + errCode; //登录失败，输出错误号
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            #endregion

            System.Threading.Thread.Sleep(PreSettings.durationCallback);

            #region 获取数据

            ret = retSendRemoteConfig;

            employeeNo = struCardInfo.dwEmployeeNo;

            //struCardInfo = (CHCNetSDK.NET_DVR_CARD_CFG_V50)Marshal.PtrToStructure(buffer, typeof(CHCNetSDK.NET_DVR_CARD_CFG_V50));

            message = "error:" + errCode;

            #endregion

            #region 关闭长连接配置接口所创建的句柄，释放资源

            if (bGetCardCfgFinish)
            {
                retCloseRemoteConfig = coreInfrastructure.NetDvrStopRemoteConfig(hndRemoteConfig);
            }

            bGetCardCfgFinish = false;
            #endregion

            //System.Threading.Thread.Sleep(PreSettings.durationCallback);
            //注销用户
            //CHCNetSDK.NET_DVR_Logout(iUserId);
            coreInfrastructure.NetDvrLogout(iUserId);
            //释放 SDK 资源
            //CHCNetSDK.NET_DVR_Cleanup();
            coreInfrastructure.NetDvrCleanUp();

            return ret;
        }

        #endregion

        #region insert

        /// <summary>
        /// 下发卡数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="deviceIPAddr"></param>
        /// <param name="devicePort"></param>
        /// <param name="userInfo"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="cbSetRemoteConfig"></param>
        /// <returns></returns>
        public bool CardInsert(
            out string message,
            string deviceIPAddr,
            int devicePort,
            Core.Domain.EntranceControl.UserInfo userInfo,
            string user = "admin",
            string password = "",
            CHCNetSDK.RemoteConfigCallback cbSetRemoteConfig = null)
        {
            bool ret = false;
            message = string.Empty;
            uint errCode = 0;
            int iUserId = -1;
            //用户数据指针
            IntPtr ptrUserData = IntPtr.Zero;
            //长连接句柄
            int hndRemoteConfig = -1;
            //长连接数据发送是否成功
            bool retSendRemoteConfig = false;
            bool retCloseRemoteConfig = false;

            #region init

            //初始化
            coreInfrastructure.NetDvrInit();

            //设置连接超时时间与重连功能
            //CHCNetSDK.NET_DVR_SetConnectTime(PreSettings.dwWaitTime, PreSettings.dwTryTimes);
            coreInfrastructure.NetDvrSetConnectTime(PreSettings.dwWaitTime, PreSettings.dwTryTimes);

            //CHCNetSDK.NET_DVR_SetReconnect(PreSettings.dwInterval, PreSettings.enableRecon);
            coreInfrastructure.NetDvrSetReconnect(PreSettings.dwInterval, PreSettings.enableRecon);

            #endregion

            #region login

            CHCNetSDK.NET_DVR_DEVICEINFO_V40 struDeviceInfoV40 = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();

            //登录设备 Login the device
            iUserId = coreInfrastructure.NetDvrLoginV40(deviceIPAddr, devicePort, user, password, ref struDeviceInfoV40);

            if (iUserId < 0)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();
                message = "NET_DVR_Login_V40 failed, error code= " + errCode; //登录失败，输出错误号
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            #endregion

            #region start remote config 启动长连接配置

            if (null == cbSetRemoteConfig)
                SetRemoteConfigDelegare = new CHCNetSDK.RemoteConfigCallback(ProcessCardInsertCallback);
            else
                SetRemoteConfigDelegare = cbSetRemoteConfig;

            hndRemoteConfig = logicCardManage.StartRemoteConfigForCardInsert(iUserId, GetRemoteConfigDelegate, ptrUserData, userInfo);

            if (hndRemoteConfig < 0)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();
                message = "NET_DVR_StartRemoteConfig, error code= " + errCode; //登录失败，输出错误号
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            #endregion

            #region send remote config 发送长连接数据

            retSendRemoteConfig = logicCardManage.SendRemoteConfigForCardInsert(hndRemoteConfig, userInfo);

            if (!retSendRemoteConfig)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();
                message = "NET_DVR_SendRemoteConfig, error code= " + errCode; //登录失败，输出错误号
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            #endregion

            System.Threading.Thread.Sleep(PreSettings.durationCallback);

            #region 获取数据

            ret = retSendRemoteConfig && errCode == 0;

            message = errCode + "";

            #endregion

            #region 关闭长连接配置接口所创建的句柄，释放资源

            if (bSetCardCfgFinish)
            {
                retCloseRemoteConfig = coreInfrastructure.NetDvrStopRemoteConfig(hndRemoteConfig);
            }

            bSetCardCfgFinish = false;

            #endregion


            //注销用户
            //CHCNetSDK.NET_DVR_Logout(iUserId);
            coreInfrastructure.NetDvrLogout(iUserId);
            //释放 SDK 资源
            //CHCNetSDK.NET_DVR_Cleanup();
            coreInfrastructure.NetDvrCleanUp();

            return errCode == 0;
        }

        #endregion
    }
}
