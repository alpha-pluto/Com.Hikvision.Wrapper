/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29
 * 
 * daniel.zhang
 * 
 * 文件名：NetDvrCardManage.cs
 * 
 * 文件功能描述：卡片管理底层调用封装类
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
    /// 卡片管理底层调用封装类
    /// </summary>
    public class NetDvrCardManage
    {

        #region build struct

        /// <summary>
        /// 构造 卡参数配置条件结构体
        /// </summary>
        /// <param name="cardNum"></param>
        /// <param name="byCheckCardNo">校验卡号</param>
        /// <returns></returns>
        private CHCNetSDK.NET_DVR_CARD_CFG_COND BuildCardCfgCondStruct(uint cardNum = 1, byte byCheckCardNo = 1)
        {
            CHCNetSDK.NET_DVR_CARD_CFG_COND struCardCfgCond = new CHCNetSDK.NET_DVR_CARD_CFG_COND();
            struCardCfgCond.dwSize = (uint)Marshal.SizeOf(struCardCfgCond);
            struCardCfgCond.dwCardNum = cardNum;
            struCardCfgCond.byCheckCardNo = byCheckCardNo;
            struCardCfgCond.wLocalControllerID = 0;
            return struCardCfgCond;
        }

        /// <summary>
        /// 构造 获取卡参数的发送数据
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        private CHCNetSDK.NET_DVR_CARD_CFG_SEND_DATA BuildCardCfgSendData(string cardNo = "1")
        {
            CHCNetSDK.NET_DVR_CARD_CFG_SEND_DATA struCardCfgSendData = new CHCNetSDK.NET_DVR_CARD_CFG_SEND_DATA();
            struCardCfgSendData.dwSize = (uint)Marshal.SizeOf(struCardCfgSendData);
            struCardCfgSendData.byCardNo = new byte[CHCNetSDK.ACS_CARD_NO_LEN];
            byte[] bCardNo = System.Text.Encoding.GetEncoding(PreSettings.charset).GetBytes(cardNo);
            bCardNo.CopyTo(struCardCfgSendData.byCardNo, 0);
            return struCardCfgSendData;
        }

        /// <summary>
        /// 构造 "卡参数配置结构体"（下发卡数据用）
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private CHCNetSDK.NET_DVR_CARD_CFG_V50 BuildCardCfgStruct(Core.Domain.EntranceControl.UserInfo userInfo)
        {
            CHCNetSDK.NET_DVR_CARD_CFG_V50 struCardCfg = new CHCNetSDK.NET_DVR_CARD_CFG_V50();
            struCardCfg.Init();
            struCardCfg.dwSize = (uint)Marshal.SizeOf(struCardCfg);
            struCardCfg.dwModifyParamType = 0x00001D8D;

            byte[] bCardNo = System.Text.Encoding.GetEncoding(PreSettings.charset).GetBytes(userInfo.CardNo);
            bCardNo.CopyTo(struCardCfg.byCardNo, 0);

            struCardCfg.byCardValid = userInfo.CardValid;
            struCardCfg.byCardType = (byte)userInfo.CardType;
            struCardCfg.byLeaderCard = userInfo.IsLeaderCard;
            struCardCfg.byDoorRight[0] = userInfo.DoorRight[0];

            struCardCfg.wCardRightPlan[0] = 1;

            byte[] bName = System.Text.Encoding.GetEncoding(PreSettings.charset).GetBytes(userInfo.Name);
            bName.CopyTo(struCardCfg.byName, 0);

            struCardCfg.dwEmployeeNo = userInfo.EmployeeNo;
            struCardCfg.wDepartmentNo = userInfo.DepartmentNo;

            byte[] bCardPassword = System.Text.Encoding.GetEncoding(PreSettings.charset).GetBytes(userInfo.CardPassword);
            bCardPassword.CopyTo(struCardCfg.byCardPassword, 0);

            struCardCfg.dwMaxSwipeTime = userInfo.MaxSwipeTime;
            struCardCfg.dwSwipeTime = userInfo.SwipeTime;

            struCardCfg.struValid.byEnable = 0;

            return struCardCfg;
        }

        #endregion

        #region card query

        /// <summary>
        /// 按卡号查询
        /// </summary>
        /// <param name="deviceUserId">NET_DVR_Login_V40等登录接口的返回值</param>
        /// <param name="cbStateCallback"></param>
        /// <param name="pUserData"></param>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public int StartRemoteConfigForCardQuery(
            int deviceUserId,
            CHCNetSDK.RemoteConfigCallback cbStateCallback,
            IntPtr pUserData,
            string cardNo
            )
        {
            int handle = -1;
            uint command = CHCNetSDK.NET_DVR_GET_CARD_CFG_V50;
            CHCNetSDK.NET_DVR_CARD_CFG_COND struCardCfgCond = BuildCardCfgCondStruct();

            int struCardCfgCondSize = Marshal.SizeOf(struCardCfgCond);
            IntPtr ptrCardCfgCond = Marshal.AllocHGlobal(struCardCfgCondSize);
            Marshal.StructureToPtr(struCardCfgCond, ptrCardCfgCond, false);

            handle = CHCNetSDK.NET_DVR_StartRemoteConfig(deviceUserId, command, ptrCardCfgCond, (uint)struCardCfgCondSize, cbStateCallback, pUserData);

            Marshal.FreeHGlobal(ptrCardCfgCond);

            return handle;
        }

        /// <summary>
        /// 发送"卡查询"长连接数据
        /// </summary>
        /// <param name="hndStartRemoteConfig"></param>
        /// <param name="cardNo">卡号</param>
        /// <returns>TRUE表示成功，FALSE表示失败</returns>
        public bool SendRemoteConfigForCardQuery(
            int hndStartRemoteConfig,
            string cardNo)
        {
            bool ret = false;
            uint dataType = (uint)CHCNetSDK.LONG_CFG_SEND_DATA_TYPE_ENUM.ENUM_ACS_SEND_DATA;

            //构建“获取卡参数的发送数据”结构体 并转化为指针
            CHCNetSDK.NET_DVR_CARD_CFG_SEND_DATA struCardCfgSendData = BuildCardCfgSendData(cardNo);
            int struCardCfgSendDataSize = Marshal.SizeOf(struCardCfgSendData);
            IntPtr ptrCardCfgSendData = Marshal.AllocHGlobal(struCardCfgSendDataSize);
            Marshal.StructureToPtr(struCardCfgSendData, ptrCardCfgSendData, false);

            ret = CHCNetSDK.NET_DVR_SendRemoteConfig(hndStartRemoteConfig, dataType, ptrCardCfgSendData, (uint)struCardCfgSendDataSize);

            Marshal.FreeHGlobal(ptrCardCfgSendData);
            return ret;
        }

        #endregion

        #region card insert

        /// <summary>
        /// 下发卡号数据
        /// </summary>
        /// <param name="deviceUserId">NET_DVR_Login_V40等登录接口的返回值</param>
        /// <param name="cbStateCallback"></param>
        /// <param name="pUserData"></param>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public int StartRemoteConfigForCardInsert(
            int deviceUserId,
            CHCNetSDK.RemoteConfigCallback cbStateCallback,
            IntPtr pUserData,
            Core.Domain.EntranceControl.UserInfo userInfo
            )
        {
            int handle = -1;
            uint command = CHCNetSDK.NET_DVR_SET_CARD_CFG_V50;
            CHCNetSDK.NET_DVR_CARD_CFG_COND struCardCfgCond = BuildCardCfgCondStruct();

            int struCardCfgCondSize = Marshal.SizeOf(struCardCfgCond);
            IntPtr ptrCardCfgCond = Marshal.AllocHGlobal(struCardCfgCondSize);
            Marshal.StructureToPtr(struCardCfgCond, ptrCardCfgCond, false);

            handle = CHCNetSDK.NET_DVR_StartRemoteConfig(deviceUserId, command, ptrCardCfgCond, (uint)struCardCfgCondSize, cbStateCallback, pUserData);

            Marshal.FreeHGlobal(ptrCardCfgCond);

            return handle;
        }

        /// <summary>
        /// 发送"卡数据"长连接数据
        /// </summary>
        /// <param name="hndStartRemoteConfig"></param>
        /// <param name="userInfo">卡号数据</param>
        /// <returns>TRUE表示成功，FALSE表示失败</returns>
        public bool SendRemoteConfigForCardInsert(
            int hndStartRemoteConfig,
            Core.Domain.EntranceControl.UserInfo userInfo)
        {
            bool ret = false;
            uint dataType = (uint)CHCNetSDK.LONG_CFG_SEND_DATA_TYPE_ENUM.ENUM_ACS_SEND_DATA;

            //构建“卡参数配置结构体”结构体 并转化为指针
            CHCNetSDK.NET_DVR_CARD_CFG_V50 struCardCfg = BuildCardCfgStruct(userInfo);
            int struCardCfgSize = Marshal.SizeOf(struCardCfg);
            IntPtr ptrCardCfg = Marshal.AllocHGlobal(struCardCfgSize);
            Marshal.StructureToPtr(struCardCfg, ptrCardCfg, false);

            ret = CHCNetSDK.NET_DVR_SendRemoteConfig(hndStartRemoteConfig, dataType, ptrCardCfg, (uint)struCardCfgSize);

            Marshal.FreeHGlobal(ptrCardCfg);
            return ret;
        }

        #endregion

    }
}
