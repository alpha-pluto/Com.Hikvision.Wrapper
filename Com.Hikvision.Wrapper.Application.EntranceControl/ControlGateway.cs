/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：ControlGateway.cs
 * 
 * 文件功能描述：卡片管理服务单元应用场景封装类
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using Com.Hikvision.Wrapper.Core;
using Com.Hikvision.Wrapper.Core.Domain.EntranceControl;
using Com.Hikvision.Wrapper.Service.EntranceControl.RemoteEntranceControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Hikvision.Wrapper.Application.EntranceControl
{
    public class ControlGateway
    {
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
            Service.EntranceControl.RemoteEntranceControl.ControlGateway srvControlGateway = new Service.EntranceControl.RemoteEntranceControl.ControlGateway();
            bool retFlag = false;
            message = string.Empty;
            retFlag = srvControlGateway.RemoteControlGatewayOpen(out message, deviceIPAddr, devicePort, gatewayIdx, user, password);
            return retFlag;
        }

        /// <summary>
        /// 下发用户卡和人脸数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="deviceIPAddr"></param>
        /// <param name="devicePort"></param>
        /// <param name="userInfo"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool MemberEnroll(
            out string message,
            string deviceIPAddr,
            int devicePort,
            UserInfo userInfo,
            string user = "******",
            string password = "******")
        {
            bool retCardInsert = false;
            bool retFaceInsert = false;
            message = string.Empty;
            userInfo.CardType = CHCNetSDK.CARD_TYPE.NORMAL;
            userInfo.CardPassword = "******";
            CardManage srvCardManage = new CardManage();
            retCardInsert = srvCardManage.CardInsert(out message, deviceIPAddr, devicePort, userInfo, user, password);
            System.Threading.Thread.Sleep(100);
            if (retCardInsert)
            {
                message = string.Empty;
                FaceManage srvFaceManage = new FaceManage();
                retFaceInsert = srvFaceManage.FaceInsert(out message, deviceIPAddr, devicePort, userInfo, user, password);
            }

            return retCardInsert && retFaceInsert;
        }

        /// <summary>
        /// 下发用户卡和人脸数据
        /// </summary>
        /// <param name="message"></param>
        /// <param name="deviceIPAddr"></param>
        /// <param name="devicePort"></param>
        /// <param name="cardNo"></param>
        /// <param name="employeeNo"></param>
        /// <param name="cardName"></param>
        /// <param name="departNo"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool MemberEnroll(
            out string message,
            string deviceIPAddr,
            int devicePort,
            string cardNo,
            uint employeeNo,
            string cardName,
            ushort departNo,
            string avatarUrl,
            string user = "******",
            string password = "******"
            )
        {
            bool ret = false;

            message = string.Empty;

            if (!avatarUrl.EndsWith(".jpg", true, System.Globalization.CultureInfo.InvariantCulture))
            {
                message = "人脸图片只支持JPG文件";
                return false;
            }

            Stream stmAvatar = Core.Utility.StreamHelper.GetStreamFromUrl(avatarUrl);

            MemoryStream msAvatar = Core.Utility.StreamHelper.StreamToMemoryStream(stmAvatar);

            msAvatar.Seek(0, SeekOrigin.Begin);

            if (msAvatar.Length < 0 || msAvatar.Length > 200 * 1024)
            {
                message = "人脸文件不能大于200K";
                return false;
            }

            Core.Domain.EntranceControl.UserInfo userInfo = new Core.Domain.EntranceControl.UserInfo();
            userInfo.CardNo = cardNo;
            userInfo.EmployeeNo = employeeNo;
            userInfo.Name = cardName;
            userInfo.DepartmentNo = departNo;
            userInfo.CardType = CHCNetSDK.CARD_TYPE.NORMAL;
            userInfo.CardPassword = "******";
            userInfo.Avator = new byte[msAvatar.Length];
            msAvatar.Read(userInfo.Avator, 0, (int)msAvatar.Length);
            ret = MemberEnroll(out message, deviceIPAddr, devicePort, userInfo, user, password);

            return ret;
        }
    }
}
