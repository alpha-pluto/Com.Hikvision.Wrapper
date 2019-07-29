/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：UserInfo.cs
 * 
 * 文件功能描述：与 NET_DVR_CARD_CFG_V50结构体对应的 用户域
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Hikvision.Wrapper.Core.Domain.EntranceControl
{
    [Serializable]
    public class UserInfo
    {
        public UserInfo()
        {
            CardValid = 1;
            EmployeeNo = 0;
            DepartmentNo = 0;
            IsLeaderCard = 0;
            CardType = CHCNetSDK.CARD_TYPE.NORMAL;
            DoorRight = new byte[CHCNetSDK.MAX_DOOR_NUM_256];
            DoorRight[0] = 1;
            BelongGroup = new byte[CHCNetSDK.MAX_GROUP_NUM_128];
            BelongGroup[0] = 1;
            MaxSwipeTime = 0;
            SwipeTime = 0;
            BeginOfValidity = DateTime.Now;
            EndOfValidity = DateTime.Now;
        }

        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 卡是否有效：0- 无效，1- 有效（用于删除卡，设置时置为0进行删除，获取时此字段始终为1） 
        /// </summary>
        public byte CardValid { get; set; }

        /// <summary>
        /// 卡类型
        /// </summary>
        public CHCNetSDK.CARD_TYPE CardType { get; set; }

        /// <summary>
        /// 是否为首卡：1- 是，0- 否 
        /// </summary>
        public byte IsLeaderCard { get; set; }

        /// <summary>
        /// 门权限（梯控的楼层权限、锁权限），
        /// 按字节表示，1-为有权限，0-为无权限，
        /// 从低位到高位依次表示对门（或者梯控楼层、锁）1-N是否有权限
        /// </summary>
        public byte[] DoorRight { get; set; }

        /// <summary>
        /// 所属群组，按字节表示，1-属于，0-不属于，从低位到高位表示是否从属群组1~N
        /// </summary>
        public byte[] BelongGroup { get; set; }

        /// <summary>
        /// 卡密码
        /// </summary>
        public string CardPassword { get; set; }

        /// <summary>
        /// 最大刷卡次数，0为无次数限制
        /// </summary>
        public uint MaxSwipeTime { get; set; }

        /// <summary>
        /// 已刷卡次数
        /// </summary>
        public uint SwipeTime { get; set; }

        /// <summary>
        /// 工号（用户ID）
        /// </summary>
        public uint EmployeeNo { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public ushort DepartmentNo { get; set; }

        /// <summary>
        /// 起始
        /// 有效期参数
        /// （有效时间跨度为1970年1月1日0点0分0秒~2037年12月31日23点59分59秒）
        /// </summary>
        public DateTime BeginOfValidity { get; set; }

        /// <summary>
        /// 截止
        /// 有效期参数
        /// （有效时间跨度为1970年1月1日0点0分0秒~2037年12月31日23点59分59秒）
        /// </summary>
        public DateTime EndOfValidity { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 持卡人ID 
        /// </summary>
        public uint CardUserId { get; set; }

        /// <summary>
        /// 头像二进制流
        /// </summary>
        public byte[] Avator { get; set; }


    }
}
