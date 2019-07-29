/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：NetDvrFaceManage.cs
 * 
 * 文件功能描述：人脸图片管理底层调用封装类
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/

using System;
using System.Runtime.InteropServices;
using Com.Hikvision.Wrapper.Core;

namespace Com.Hikvision.Wrapper.EntranceControl.RemoteEntranceControl
{
    /// <summary>
    /// 人脸图片管理底层调用封装类
    /// </summary>
    public class NetDvrFaceManage
    {
        #region build struct

        /// <summary>
        /// 构造“人脸参数配置条件结构体” （查询）
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        private CHCNetSDK.NET_DVR_FACE_PARAM_COND BuildFaceParamCondStruct(string cardNo)
        {
            CHCNetSDK.NET_DVR_FACE_PARAM_COND struFaceParamCond = new CHCNetSDK.NET_DVR_FACE_PARAM_COND();

            struFaceParamCond.dwSize = (uint)Marshal.SizeOf(struFaceParamCond);
            struFaceParamCond.dwFaceNum = 1;
            struFaceParamCond.byFaceID = 1;
            struFaceParamCond.byEnableCardReader = new byte[CHCNetSDK.MAX_CARD_READER_NUM_512];
            struFaceParamCond.byEnableCardReader[0] = 1;

            struFaceParamCond.byCardNo = new byte[CHCNetSDK.ACS_CARD_NO_LEN];
            byte[] byTempCardNo = System.Text.Encoding.GetEncoding(PreSettings.charset).GetBytes(cardNo);
            byTempCardNo.CopyTo(struFaceParamCond.byCardNo, 0);

            return struFaceParamCond;
        }

        /// <summary>
        /// 构造“人脸参数配置结构体”
        /// </summary>
        /// <param name="sCardNo"></param>
        /// <param name="bFacePic"></param>
        /// <returns></returns>
        private CHCNetSDK.NET_DVR_FACE_PARAM_CFG BuildFaceParamCfgStruct(string sCardNo, byte[] bFacePic)
        {
            CHCNetSDK.NET_DVR_FACE_PARAM_CFG struFaceParamCfg = new CHCNetSDK.NET_DVR_FACE_PARAM_CFG();

            struFaceParamCfg.dwSize = (uint)Marshal.SizeOf(struFaceParamCfg);
            struFaceParamCfg.byFaceID = 1;
            struFaceParamCfg.byFaceDataType = 1;
            struFaceParamCfg.byEnableCardReader = new byte[CHCNetSDK.MAX_CARD_READER_NUM_512];
            struFaceParamCfg.byEnableCardReader[0] = 1;
            struFaceParamCfg.byCardNo = new byte[CHCNetSDK.ACS_CARD_NO_LEN];
            byte[] byTempFaceCardNo = System.Text.Encoding.GetEncoding(PreSettings.charset).GetBytes(sCardNo);
            byTempFaceCardNo.CopyTo(struFaceParamCfg.byCardNo, 0);

            struFaceParamCfg.dwFaceLen = (uint)bFacePic.Length;
            struFaceParamCfg.pFaceBuffer = Marshal.AllocHGlobal(bFacePic.Length);
            Marshal.Copy(bFacePic, 0, struFaceParamCfg.pFaceBuffer, bFacePic.Length);

            return struFaceParamCfg;
        }

        #endregion

        #region Face query

        /// <summary>
        /// 按卡号查询人脸数据
        /// </summary>
        /// <param name="deviceUserId"></param>
        /// <param name="cbStateCallback"></param>
        /// <param name="pUserData"></param>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public int StartRemoteConfigForFaceQuery(
            int deviceUserId,
            CHCNetSDK.RemoteConfigCallback cbStateCallback,
            IntPtr pUserData,
            string cardNo
            )
        {
            int handle = -1;
            uint command = CHCNetSDK.NET_DVR_GET_FACE_PARAM_CFG;
            CHCNetSDK.NET_DVR_FACE_PARAM_COND struFaceParamCond = BuildFaceParamCondStruct(cardNo);

            int struFaceParamCondSize = Marshal.SizeOf(struFaceParamCond);
            IntPtr ptrFaceParaCond = Marshal.AllocHGlobal(struFaceParamCondSize);
            Marshal.StructureToPtr(struFaceParamCond, ptrFaceParaCond, false);

            handle = CHCNetSDK.NET_DVR_StartRemoteConfig(deviceUserId, command, ptrFaceParaCond, (uint)struFaceParamCondSize, cbStateCallback, pUserData);

            Marshal.FreeHGlobal(ptrFaceParaCond);

            return handle;
        }

        #endregion

        #region Face Insert

        /// <summary>
        /// 给指定的卡号下发人脸数据
        /// </summary>
        /// <param name="deviceUserId"></param>
        /// <param name="cbStateCallback"></param>
        /// <param name="pUserData"></param>
        /// <param name="sCardNo"></param>
        /// <param name="byFacePic"></param>
        /// <returns></returns>
        public int StartRemoteConfigForFaceInsert(
            int deviceUserId,
            CHCNetSDK.RemoteConfigCallback cbStateCallback,
            IntPtr pUserData,
            string sCardNo)
        {
            int handle = -1;
            uint command = CHCNetSDK.NET_DVR_SET_FACE_PARAM_CFG;
            CHCNetSDK.NET_DVR_FACE_PARAM_COND struFaceParamCond = BuildFaceParamCondStruct(sCardNo);
            int struFaceParamCondSize = Marshal.SizeOf(struFaceParamCond);
            IntPtr ptrFaceParaCond = Marshal.AllocHGlobal(struFaceParamCondSize);
            Marshal.StructureToPtr(struFaceParamCond, ptrFaceParaCond, false);

            handle = CHCNetSDK.NET_DVR_StartRemoteConfig(deviceUserId, command, ptrFaceParaCond, (uint)struFaceParamCondSize, cbStateCallback, pUserData);

            Marshal.FreeHGlobal(ptrFaceParaCond);

            return handle;
        }

        /// <summary>
        /// 发送长连接数据（人脸参数）
        /// </summary>
        /// <param name="hndStartRemoteConfig"></param>
        /// <param name="sCardNo"></param>
        /// <param name="bFacePic"></param>
        /// <returns></returns>
        public bool SendRemoteConfigForCardInsert(
            int hndStartRemoteConfig,
            string sCardNo,
            byte[] bFacePic)
        {
            bool ret = false;
            uint dataType = (uint)CHCNetSDK.LONG_CFG_SEND_DATA_TYPE_ENUM.ENUM_ACS_INTELLIGENT_IDENTITY_DATA;

            //构建“人脸参数配置结构体”结构体 并转化为指针
            CHCNetSDK.NET_DVR_FACE_PARAM_CFG struFaceParamCfg = BuildFaceParamCfgStruct(sCardNo, bFacePic);
            int struFaceParamCfgSize = Marshal.SizeOf(struFaceParamCfg);
            IntPtr ptrFaceParamCfg = Marshal.AllocHGlobal(struFaceParamCfgSize);
            Marshal.StructureToPtr(struFaceParamCfg, ptrFaceParamCfg, false);
            ret = CHCNetSDK.NET_DVR_SendRemoteConfig(hndStartRemoteConfig, dataType, ptrFaceParamCfg, (uint)struFaceParamCfgSize);
            Marshal.FreeHGlobal(ptrFaceParamCfg);
            return ret;

        }

        #endregion
    }
}
