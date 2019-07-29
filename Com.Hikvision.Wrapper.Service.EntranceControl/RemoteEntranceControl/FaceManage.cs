/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：FaceManage.cs
 * 
 * 文件功能描述：人脸图片管理服务调用封装类
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/

using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using Com.Hikvision.Wrapper.Core;
using Com.Hikvision.Wrapper.EntranceControl.RemoteEntranceControl;

namespace Com.Hikvision.Wrapper.Service.EntranceControl.RemoteEntranceControl
{
    /// <summary>
    /// 人脸图片管理服务调用封装类
    /// </summary>
    public class FaceManage
    {
        private string TempFaceOutputPath = ConfigurationManager.AppSettings["TempFaceOutputPath"] ?? "e:\\facestorage";

        //private const string TempFaceOutputPath = "e:\\face-storage";

        private string errCodeCallback = string.Empty;


        #region common

        //
        private static readonly Core.Infrastructure coreInfrastructure = new Infrastructure();

        private static readonly NetDvrFaceManage logicFaceManage = new NetDvrFaceManage();

        #endregion

        /// <summary>
        /// 获取人脸数据是否完成
        /// </summary>
        bool bGetFaceParamCfgFinish = false;

        /// <summary>
        /// 设置人脸数据是否完成 
        /// </summary>
        bool bSetFaceParamCfgFinish = false;

        #region delegate instantiate

        public CHCNetSDK.RemoteConfigCallback GetFaceParaDelegate { get; private set; }

        public CHCNetSDK.RemoteConfigCallback SetFaceParaDelegare { get; private set; }

        /// <summary>
        /// 处理 查询 人脸 回调
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pBuffer"></param>
        /// <param name="bufferLen"></param>
        /// <param name="pUserData"></param>
        private void ProcessFaceQueryCallback(uint type, IntPtr pBuffer, uint bufferLen, IntPtr pUserData)
        {

            if (pUserData == null)
            {
                return;
            }

            if (type == (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_STATUS)
            {
                uint status = (uint)Marshal.ReadInt32(pBuffer);

                if (status == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_PROCESSING)
                {
                    //g_formList.AddLog(m_lDeviceIndex, AcsDemoPublic.OPERATION_SUCC_T, "GetFaceParam Processing");
                }
                else if (status == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_FAILED)
                {
                    //g_formList.AddLog(m_lDeviceIndex, AcsDemoPublic.OPERATION_FAIL_T, "GetFaceParam Failed");
                    //CHCNetSDK.PostMessage(pUserData, 1002, 0, 0);
                }
                else if (status == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_SUCCESS)
                {
                    //g_formList.AddLog(m_lDeviceIndex, AcsDemoPublic.OPERATION_SUCC_T, "GetFaceParam Success");
                    //CHCNetSDK.PostMessage(pUserData, 1002, 0, 0);

                }
                else if (status == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_EXCEPTION)
                {
                    //g_formList.AddLog(m_lDeviceIndex, AcsDemoPublic.OPERATION_FAIL_T, "GetFaceParam Exception");
                    //CHCNetSDK.PostMessage(pUserData, 1002, 0, 0);
                }
                else
                {
                    //g_formList.AddLog(m_lDeviceIndex, AcsDemoPublic.OPERATION_FAIL_T, "Unknown Status");
                    //CHCNetSDK.PostMessage(pUserData, 1002, 0, 0);
                }
            }
            else if (type == (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_DATA)
            {
                var result = Marshal.PtrToStructure(pBuffer, typeof(CHCNetSDK.NET_DVR_FACE_PARAM_CFG));
                var struFaceParamCfg = (CHCNetSDK.NET_DVR_FACE_PARAM_CFG)result;
                if (struFaceParamCfg.byEnableCardReader[0] != 1)
                {
                    //g_formList.AddLog(m_lDeviceIndex, AcsDemoPublic.OPERATION_FAIL_T, "GetFaceParam Return Failed");
                }
                if (struFaceParamCfg.dwFaceLen != 0)
                {
                    //CHCNetSDK.PostMessage(pUserData, 1003, (int)pBuffer, 0);
                    string sCardNo = System.Text.Encoding.GetEncoding(PreSettings.charset).GetString(struFaceParamCfg.byCardNo);
                    sCardNo = sCardNo.Replace("\0", "");
                    FileStream fs = new FileStream(string.Format("{0}\\{1}.jpg", TempFaceOutputPath, sCardNo), FileMode.Create);
                    int iLen = (int)struFaceParamCfg.dwFaceLen;
                    byte[] byTempFacePic = new byte[iLen];
                    Marshal.Copy(struFaceParamCfg.pFaceBuffer, byTempFacePic, 0, iLen);
                    fs.Write(byTempFacePic, 0, iLen);
                    fs.Close();
                    bGetFaceParamCfgFinish = true;
                }
            }
            return;
        }

        /// <summary>
        /// 处理下发人脸数据的回调 
        /// </summary>
        /// <param name="dwType"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUserData"></param>
        private void ProcessFaceInsertCallback(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData)
        {
            if (pUserData == null)
            {
                return;
            }

            if (dwType == (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_STATUS)
            {
                uint dwStatus = (uint)Marshal.ReadInt32(lpBuffer);

                if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_PROCESSING)
                {
                    errCodeCallback = "SetFaceParam Processing";
                }
                else if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_FAILED)
                {
                    errCodeCallback = "SetFaceParam Failed";
                    //CHCNetSDK.PostMessage(pUserData, 1001, 0, 0);
                }
                else if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_SUCCESS)
                {
                    errCodeCallback = "SetFaceParam Success";
                    bSetFaceParamCfgFinish = true;
                    //CHCNetSDK.PostMessage(pUserData, 1001, 0, 0);
                }
                else if (dwStatus == (uint)CHCNetSDK.NET_SDK_CALLBACK_STATUS_NORMAL.NET_SDK_CALLBACK_STATUS_EXCEPTION)
                {
                    errCodeCallback = "SetFaceParam Exception";
                    //CHCNetSDK.PostMessage(pUserData, 1001, 0, 0);
                }
                else
                {
                    errCodeCallback = "Unknown Status";
                    //CHCNetSDK.PostMessage(pUserData, 1001, 0, 0);
                }
            }
            else if (dwType == (uint)CHCNetSDK.NET_SDK_CALLBACK_TYPE.NET_SDK_CALLBACK_TYPE_DATA)
            {
                var result = Marshal.PtrToStructure(lpBuffer, typeof(CHCNetSDK.NET_DVR_FACE_PARAM_STATUS));
                var struFaceParamStatus = (CHCNetSDK.NET_DVR_FACE_PARAM_STATUS)result;
                if (struFaceParamStatus.byCardReaderRecvStatus[0] != 1)
                {
                    errCodeCallback = "SetFaceParam Return Failed";
                }
            }
            return;
        }

        #endregion

        #region query

        /// <summary>
        /// 查询人脸数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="deviceIPAddr"></param>
        /// <param name="devicePort"></param>
        /// <param name="cardNo"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="cbGetFacePara"></param>
        /// <returns></returns>
        public bool FaceQuery(
            out string message,
            string deviceIPAddr,
            int devicePort,
            string cardNo,
            string user = "******",
            string password = "******",
            CHCNetSDK.RemoteConfigCallback cbGetFacePara = null)
        {
            bool ret = false;
            message = string.Empty;
            uint errCode = 0;
            int iUserId = -1;
            //用户数据指针
            IntPtr ptrUserData = IntPtr.Zero;
            //长连接句柄
            int hndRemoteConfig = -1;

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

            if (null == cbGetFacePara)
                GetFaceParaDelegate = new CHCNetSDK.RemoteConfigCallback(ProcessFaceQueryCallback);
            else
                GetFaceParaDelegate = cbGetFacePara;

            hndRemoteConfig = logicFaceManage.StartRemoteConfigForFaceQuery(iUserId, GetFaceParaDelegate, ptrUserData, cardNo);

            if (hndRemoteConfig < 0)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();
                message = "NET_DVR_StartRemoteConfig, error code= " + errCode; //登录失败，输出错误号
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            #endregion

            System.Threading.Thread.Sleep(PreSettings.durationCallback);

            #region 获取数据

            ret = bGetFaceParamCfgFinish;

            #endregion

            #region 关闭长连接配置接口所创建的句柄，释放资源

            if (bGetFaceParamCfgFinish)
            {
                coreInfrastructure.NetDvrStopRemoteConfig(hndRemoteConfig);
            }

            bGetFaceParamCfgFinish = false;

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
        /// 下发人脸数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="deviceIPAddr"></param>
        /// <param name="devicePort"></param>
        /// <param name="userInfo"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="cbSetRemoteConfig"></param>
        /// <returns></returns>
        public bool FaceInsert(
            out string message,
            string deviceIPAddr,
            int devicePort,
            Core.Domain.EntranceControl.UserInfo userInfo,
            string user = "******",
            string password = "******",
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
                SetFaceParaDelegare = new CHCNetSDK.RemoteConfigCallback(ProcessFaceInsertCallback);
            else
                SetFaceParaDelegare = cbSetRemoteConfig;

            hndRemoteConfig = logicFaceManage.StartRemoteConfigForFaceInsert(iUserId, SetFaceParaDelegare, ptrUserData, userInfo.CardNo);

            if (hndRemoteConfig < 0)
            {
                errCode = coreInfrastructure.NetDvrGetLastError();
                message = "NET_DVR_StartRemoteConfig, error code= " + errCode; //登录失败，输出错误号
                coreInfrastructure.NetDvrCleanUp();
                return false;
            }

            #endregion

            #region send remote config 发送长连接数据

            retSendRemoteConfig = logicFaceManage.SendRemoteConfigForCardInsert(hndRemoteConfig, userInfo.CardNo, userInfo.Avator);

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

            ret = retSendRemoteConfig && errCode == 0 && bSetFaceParamCfgFinish;

            message = errCode + "";

            #endregion

            #region 关闭长连接配置接口所创建的句柄，释放资源

            if (bSetFaceParamCfgFinish)
            {
                coreInfrastructure.NetDvrStopRemoteConfig(hndRemoteConfig);
            }

            bSetFaceParamCfgFinish = false;

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
