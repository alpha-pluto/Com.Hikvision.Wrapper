/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：CHCNetSDK_partial_entrance_control.cs
 * 
 * 文件功能描述：SDK DLL 调用入口声明文件(门禁主机部分)
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.Runtime.InteropServices;

namespace Com.Hikvision.Wrapper.Core
{
    /// <summary>
    /// SDK DLL 调用入口声明文件(门禁主机部分)
    /// </summary>
    public partial class CHCNetSDK
    {
        public const int ACS_CARD_NO_LEN = 32;
        public const int MAX_CARD_READER_NUM_512 = 512;

        public const int MAX_DOOR_NUM_256 = 256;
        public const int MAX_GROUP_NUM_128 = 128;
        public const int CARD_PASSWORD_LEN = 8;
        public const int MAX_CARD_RIGHT_PLAN_NUM = 4;
        public const int MAX_LOCK_CODE_LEN = 8;
        public const int MAX_DOOR_CODE_LEN = 8;
        public const int ERROR_MSG_LEN = 32;

        /// <summary>
        /// 卡类型
        /// </summary>
        public enum CARD_TYPE
        {

            /// <summary>
            /// 普通卡（默认）
            /// </summary>
            NORMAL = 1,

            /// <summary>
            /// 残疾人卡
            /// </summary>
            DISABLED_PEOPLE = 2,

            /// <summary>
            /// 黑名单卡
            /// </summary>
            BLACK_LIST = 3,

            /// <summary>
            /// 巡更卡
            /// </summary>
            PATROLS = 4,

            /// <summary>
            /// 胁迫卡
            /// </summary>
            UNDER_DURESS = 5,

            /// <summary>
            /// - 超级卡，
            /// </summary>
            SUPER = 6,

            /// <summary>
            /// 来宾卡
            /// </summary>
            VISITOR = 7,

            /// <summary>
            /// 解除卡
            /// </summary>
            DEACTIVATED = 8,

            /// <summary>
            ///  员工卡
            /// </summary>
            CREW = 9,

            /// <summary>
            ///  应急卡，
            /// </summary>
            ER = 10,

            /// <summary>
            /// 应急管理卡（用于授权临时卡权限，本身不能开门），默认普通卡 
            /// </summary>
            ER_ADMIN = 11
        }

        /// <summary>
        /// fRemoteConfigCallback 回调 dwType 的取值
        /// </summary>
        public enum NET_SDK_CALLBACK_TYPE
        {
            /// <summary>
            /// 回调状态值 
            /// 前4个字节为状态值(dwStatus)：
            /// dwStatus为NET_SDK_CALLBACK_STATUS_SUCCESS，表示获取和配置成功并且结束；
            /// dwStatus为NET_SDK_CALLBACK_STATUS_PROCESSING，lpBuffer：4字节状态 + 32字节卡号；
            /// dwStatus为NET_SDK_CALLBACK_STATUS_FAILED，lpBuffer：4字节状态 + 4字节错误码 + 32字节卡号；
            /// dwStatus为NET_SDK_CALLBACK_STATUS_EXCEPTION，表示长连接配置异常
            /// dwStatus为NET_SDK_CALLBACK_STATUS_LANGUAGE_MISMATCH,表示（IPC配置文件导入）语言不匹配
            /// dwStatus为NET_SDK_CALLBACK_STATUS_DEV_TYPE_MISMATCH,表示（IPC配置文件导入）设备类型不匹配
            /// dwStatus为NET_DVR_CALLBACK_STATUS_SEND_WAIT，表示需要等待一段时间再发送
            /// </summary>
            NET_SDK_CALLBACK_TYPE_STATUS = 0,

            /// <summary>
            /// 回调进度值(暂不支持)
            /// lpBuffer的值表示进度(DWORD)
            /// </summary>
            NET_SDK_CALLBACK_TYPE_PROGRESS,

            /// <summary>
            /// 回调数据内容
            /// lpBuffer的值表示信息数据
            /// NET_DVR_GET_CARD_CFG时，对应结构体：NET_DVR_CARD_CFG
            /// NET_DVR_GET_CARD_CFG_V50时，对应结构体：NET_DVR_CARD_CFG_V50
            /// NET_DVR_GET_FINGERPRINT_CFG时，对应结构体：NET_DVR_FINGER_PRINT_CFG
            /// NET_DVR_SET_FINGERPRINT_CFG时，对应结构体：NET_DVR_FINGER_PRINT_STATUS
            /// NET_DVR_GET_CARD_PASSWD_CFG时，对应结构体：NET_DVR_CARD_PASSWD_CFG
            /// NET_DVR_SET_CARD_PASSWD_CFG时，对应结构体：NET_DVR_CARD_PASSWD_STATUS 
            /// </summary>
            NET_SDK_CALLBACK_TYPE_DATA
        }

        // 长连接返回状态
        public enum NET_SDK_CALLBACK_STATUS_NORMAL
        {
            /// <summary>
            /// Success
            /// </summary>
            NET_SDK_CALLBACK_STATUS_SUCCESS = 1000,

            /// <summary>
            /// Processing
            /// </summary>
            NET_SDK_CALLBACK_STATUS_PROCESSING,

            /// <summary>
            /// Failed
            /// </summary>
            NET_SDK_CALLBACK_STATUS_FAILED,

            /// <summary>
            /// Exception
            /// </summary>
            NET_SDK_CALLBACK_STATUS_EXCEPTION,

            /// <summary>
            /// Language mismatch
            /// </summary>
            NET_SDK_CALLBACK_STATUS_LANGUAGE_MISMATCH,

            /// <summary>
            /// Device type mismatchs
            /// </summary>
            NET_SDK_CALLBACK_STATUS_DEV_TYPE_MISMATCH,

            /// <summary>
            /// send wait
            /// </summary>
            NET_DVR_CALLBACK_STATUS_SEND_WAIT
        }

        /// <summary>
        /// 长连接发送数据类型
        /// </summary>
        public enum LONG_CFG_SEND_DATA_TYPE_ENUM
        {
            /// <summary>
            /// vehicle Black list check
            /// 黑名单车辆数据稽查类型
            /// </summary>
            ENUM_DVR_VEHICLE_CHECK = 1,

            /// <summary>
            /// screen control data type
            /// 屏幕控制器数据类型
            /// </summary>
            ENUM_MSC_SEND_DATA = 2,

            /// <summary>
            /// access card data type
            /// 门禁主机数据类型 
            /// </summary>
            ENUM_ACS_SEND_DATA = 3,

            /// <summary>
            /// Parking Card data type
            /// 停车场(出入口控制机)卡片数据类型 
            /// </summary>
            ENUM_TME_CARD_SEND_DATA = 4,

            /// <summary>
            /// TME Vehicle Info data type
            /// 停车场(出入口控制机)车辆数据类型
            /// </summary>
            ENUM_TME_VEHICLE_SEND_DATA = 5,

            /// <summary>
            /// Debug Cmd
            /// 调试命令信息
            /// </summary>
            ENUM_DVR_DEBUG_CMD = 6,

            /// <summary>
            /// Screen interactive
            /// 屏幕互动命令类型 
            /// </summary>
            ENUM_DVR_SCREEN_CTRL_CMD = 7,

            /// <summary>
            /// CVR get passback task executable data type
            /// CVR获取监控点回传任务可执行性
            /// </summary>
            ENUM_CVR_PASSBACK_SEND_DATA = 8,

            /// <summary>
            /// intelligent identity data type
            /// 智能身份识别终端数据类型
            /// </summary>
            ENUM_ACS_INTELLIGENT_IDENTITY_DATA = 9,

            /// <summary>
            /// video intercom send data
            /// 
            /// </summary>
            ENUM_VIDEO_INTERCOM_SEND_DATA = 10,

            /// <summary>
            /// send json data
            /// </summary>
            ENUM_SEND_JSON_DATA = 11
        }

        //NET_DVR_StartRemoteConfig

        #region 命令宏定义(用于参数  dwCommand 赋值)

        #region 卡参数

        /// <summary>
        /// 卡是否有效参数
        /// </summary>
        public const int CARD_PARAM_CARD_VALID = 0x00000001;

        /// <summary>
        /// 有效期参数
        /// </summary>
        public const int CARD_PARAM_VALID = 0x00000002;

        /// <summary>
        /// 卡类型参数
        /// </summary>
        public const int CARD_PARAM_CARD_TYPE = 0x00000004;

        /// <summary>
        /// 门权限参数
        /// </summary>
        public const int CARD_PARAM_DOOR_RIGHT = 0x00000008;

        /// <summary>
        /// 首卡参数
        /// </summary>
        public const int CARD_PARAM_LEADER_CARD = 0x00000010;

        /// <summary>
        /// 最大刷卡次数参数 
        /// </summary>
        public const int CARD_PARAM_SWIPE_NUM = 0x00000020;

        /// <summary>
        /// 所属群组参数 
        /// </summary>
        public const int CARD_PARAM_GROUP = 0x00000040;

        /// <summary>
        /// 卡密码参数 
        /// </summary>
        public const int CARD_PARAM_PASSWORD = 0x00000080;

        /// <summary>
        /// 卡权限计划参数 
        /// </summary>
        public const int CARD_PARAM_RIGHT_PLAN = 0x00000100;

        /// <summary>
        /// 已刷卡次数 
        /// </summary>
        public const int CARD_PARAM_SWIPED_NUM = 0x00000200;

        /// <summary>
        /// 工号 
        /// </summary>
        public const int CARD_PARAM_EMPLOYEE_NO = 0x00000400;

        /// <summary>
        /// 姓名
        /// </summary>
        public const int CARD_PARAM_NAME = 0x00000800;

        /// <summary>
        /// 部门编号 
        /// </summary>
        public const int CARD_PARAM_DEPARTMENT_NO = 0x00001000;

        /// <summary>
        /// 排班计划编号 
        /// </summary>
        public const int CARD_SCHEDULE_PLAN_NO = 0x00002000;

        /// <summary>
        /// 排班计划类型
        /// </summary>
        public const int CARD_SCHEDULE_PLAN_TYPE = 0x00004000;

        /// <summary>
        /// 用户类型
        /// </summary>
        public const int CARD_USER_TYPE = 0x00040000;


        #endregion

        #region 卡和指纹参数

        /// <summary>
        /// 获取卡参数(老命令，建议使用V50扩展命令)
        /// 对应  lpInBuffer NET_DVR_CARD_CFG_COND
        /// cbStateCallback 返回状态、信息数据 
        /// </summary>
        public const int NET_DVR_GET_CARD_CFG = 2116;

        /// <summary>
        /// 设置卡参数(老命令，建议使用V50扩展命令)
        /// 对应  lpInBuffer NET_DVR_CARD_CFG_COND
        /// cbStateCallback 返回状态 
        /// </summary>
        public const int NET_DVR_SET_CARD_CFG = 2117;

        /// <summary>
        /// 获取卡参数(V50扩展，兼容老命令)
        /// 对应  lpInBuffer NET_DVR_CARD_CFG_COND
        /// cbStateCallback 返回状态、信息数据 
        /// </summary>
        public const int NET_DVR_GET_CARD_CFG_V50 = 2178;

        /// <summary>
        /// 设置卡参数(V50扩展，兼容老命令)
        /// 对应  lpInBuffer NET_DVR_CARD_CFG_COND
        /// cbStateCallback 返回状态 
        /// </summary>
        public const int NET_DVR_SET_CARD_CFG_V50 = 2179;

        /// <summary>
        /// 获取指纹参数
        /// 对应  lpInBuffer NET_DVR_FINGER_PRINT_INFO_COND
        /// 返回状态、信息数据
        /// </summary>
        public const int NET_DVR_GET_FINGERPRINT_CFG = 2150;

        /// <summary>
        /// 设置指纹参数
        /// 对应  lpInBuffer NET_DVR_FINGER_PRINT_INFO_COND
        /// 返回状态、信息数据
        /// </summary>
        public const int NET_DVR_SET_FINGERPRINT_CFG = 2151;

        /// <summary>
        /// 获取卡密码开门使能参数 
        /// 对应 lpInBuffer NET_DVR_CARD_CFG_COND 
        /// 返回状态、信息数据 
        /// </summary>
        public const int NET_DVR_GET_CARD_PASSWD_CFG = 2161;

        /// <summary>
        /// 设置卡密码开门使能参数 
        /// 对应 lpInBuffer NET_DVR_CARD_CFG_COND 
        /// 返回状态、信息数据
        /// </summary>
        public const int NET_DVR_SET_CARD_PASSWD_CFG = 2162;

        #endregion

        #region 人脸参数

        public const int MAX_FACE_NUM = 2;

        /// <summary>
        /// 获取人脸参数 
        /// lpInBuffer  NET_DVR_FACE_PARAM_COND 
        /// cbStateCallback 返回状态、进度、信息数据
        /// </summary>
        public const int NET_DVR_GET_FACE_PARAM_CFG = 2507;

        /// <summary>
        /// 设置人脸参数 
        /// lpInBuffer  NET_DVR_FACE_PARAM_COND 
        /// cbStateCallback 返回状态、进度、信息数据
        /// </summary>
        public const int NET_DVR_SET_FACE_PARAM_CFG = 2508;

        /// <summary>
        /// 删除人脸
        /// </summary>
        public const int NET_DVR_DEL_FACE_PARAM_CFG = 2509;

        /// <summary>
        /// 捕捉人脸
        /// </summary>
        public const int NET_DVR_CAPTURE_FACE_INFO = 2510;

        #endregion

        public const int NET_DVR_JSON_CONFIG = 2550;

        #endregion

        #region 结构体

        /// <summary>
        /// 有效期参数结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_VALID_PERIOD_CFG
        {
            /// <summary>
            /// 是否启用该有效期：0- 不启用，1- 启用 
            /// </summary>
            public byte byEnable;

            ///// <summary>
            ///// 
            ///// </summary>
            //[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            //public byte[] byRes1;

            /// <summary>
            /// 是否限制起始时间的标志，0-不限制，1-限制 
            /// </summary>
            public byte byBeginTimeFlag;

            /// <summary>
            /// 是否限制终止时间的标志，0-不限制，1-限制 
            /// </summary>
            public byte byEnableTimeFlag;

            /// <summary>
            /// 有效期索引,从0开始（时间段通过SDK设置给锁，后续在制卡时，只需要传递有效期索引即可，以减少数据量
            /// </summary>
            public byte byTimeDurationNo;

            /// <summary>
            /// 有效期起始时间
            /// </summary>
            public NET_DVR_TIME_EX struBeginTime;

            /// <summary>
            /// 有效期结束时间
            /// </summary>
            public NET_DVR_TIME_EX struEndTime;

            /// <summary>
            /// 保留，置为0 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;
        }

        /// <summary>
        /// 因具体硬件上的版本，此结构体不能使用最新版本的定义 
        /// 
        /// 卡参数配置结构体
        /// 结构体里面的工号、姓名、部门编号、排班计划编号、排班计划类型，对于考勤一体机（DS-K1T803F、DS-K1A801F）为必填项，
        /// 对于其他门禁设备不是必须项。
        /// 卡参数能力，对应门禁主机能力集（接口：NET_DVR_GetDeviceAbility，能力集类型：ACS_ABILITY）中节点<Card>。
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_CARD_CFG_V50
        {
            /// <summary>
            /// 结构体大小 
            /// </summary>
            public uint dwSize;

            /// <summary>
            /// 需要修改的卡参数（设置卡参数时有效），按位表示，每位代表一种参数，值：0- 不修改，1- 需要修改
            /// </summary>
            public uint dwModifyParamType;

            /// <summary>
            /// 卡号，特殊卡号定义如下：
            ///0xFFFFFFFFFFFFFFFF：非法卡号
            ///0xFFFFFFFFFFFFFFFE：胁迫码
            ///0xFFFFFFFFFFFFFFFD：超级码
            ///0xFFFFFFFFFFFFFFFC~0xFFFFFFFFFFFFFFF1：预留的特殊卡
            ///0xFFFFFFFFFFFFFFF0：最大合法卡号 
            ///
            /// the card parameter need to modify, valid when set card parameter, use by bit, every bit means a kind of parameter, 1 means modify, 0 means not 
            /// #define CARD_PARAM_CARD_VALID       0x00000001 //card valid parameter 
            /// #define CARD_PARAM_VALID            0x00000002  //valid period parameter
            /// #define CARD_PARAM_CARD_TYPE        0x00000004  //card type parameter
            /// #define CARD_PARAM_DOOR_RIGHT       0x00000008  //door right parameter
            /// #define CARD_PARAM_LEADER_CARD      0x00000010  //leader card parameter
            /// #define CARD_PARAM_SWIPE_NUM        0x00000020  //max swipe time parameter
            /// #define CARD_PARAM_GROUP            0x00000040  //belong group parameter
            /// #define CARD_PARAM_PASSWORD         0x00000080  //card password parameter
            /// #define CARD_PARAM_RIGHT_PLAN       0x00000100  //card right plan parameter
            /// #define CARD_PARAM_SWIPED_NUM       0x00000200  //has swiped card times parameter
            /// #define CARD_PARAM_EMPLOYEE_NO      0x00000400  //employee no
            /// #define CARD_PARAM_NAME             0x00000800  //name
            /// #define CARD_PARAM_DEPARTMENT_NO    0x00001000  //department no
            /// #define CARD_SCHEDULE_PLAN_NO       0x00002000  //schedule plan no
            /// #define CARD_SCHEDULE_PLAN_TYPE     0x00004000  //schedule plan type
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo; //card No

            /// <summary>
            /// 卡是否有效：0- 无效，1- 有效（用于删除卡，设置时置为0进行删除，获取时此字段始终为1） 
            /// </summary>
            public byte byCardValid;

            /// <summary>
            /// 卡类型：
            /// 1- 普通卡（默认），
            /// 2- 残疾人卡，
            /// 3- 黑名单卡，
            /// 4- 巡更卡，
            /// 5- 胁迫卡，
            /// 6- 超级卡，
            /// 7- 来宾卡，
            /// 8- 解除卡，
            /// 9- 员工卡，
            /// 10- 应急卡，
            /// 11- 应急管理卡（用于授权临时卡权限，本身不能开门），默认普通卡 
            /// </summary>
            public byte byCardType;

            /// <summary>
            /// 是否为首卡：1- 是，0- 否 
            /// </summary>
            public byte byLeaderCard;

            /// <summary>
            /// 用户类型：
            /// 0 – 普通用户
            /// 1- 管理员用户
            /// </summary>
            //public byte byUserType;

            public byte byRes1;

            /// <summary>
            /// 门权限（梯控的楼层权限、锁权限），按字节表示，
            /// 1-为有权限，0-为无权限，从低位到高位依次表示对门（或者梯控楼层、锁）1-N是否
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DOOR_NUM_256, ArraySubType = UnmanagedType.I1)]
            public byte[] byDoorRight;

            /// <summary>
            /// 有效期参数（有效时间跨度为1970年1月1日0点0分0秒~2037年12月31日23点59分59秒）
            /// </summary>
            public NET_DVR_VALID_PERIOD_CFG struValid;

            /// <summary>
            /// 所属群组，按字节表示，1-属于，0-不属于，从低位到高位表示是否从属群组1~N
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_GROUP_NUM_128, ArraySubType = UnmanagedType.I1)]
            public byte[] byBelongGroup;

            /// <summary>
            /// 卡密码 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = CARD_PASSWORD_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardPassword;

            /// <summary>
            /// 卡权限计划，取值为计划模板编号，同个门（锁）不同计划模板采用权限或的方式处理 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DOOR_NUM_256 * MAX_CARD_RIGHT_PLAN_NUM, ArraySubType = UnmanagedType.I1)]
            public ushort[] wCardRightPlan;

            /// <summary>
            /// 最大刷卡次数，0为无次数限制 
            /// </summary>
            public uint dwMaxSwipeTime;

            /// <summary>
            /// 已刷卡次数
            /// </summary>
            public uint dwSwipeTime;

            /// <summary>
            /// 房间号 
            /// </summary>
            public ushort wRoomNumber;

            /// <summary>
            /// 层号 
            /// </summary>
            public short wFloorNumber;

            /// <summary>
            /// 工号（用户ID）
            /// </summary>
            public uint dwEmployeeNo;

            /// <summary>
            /// 姓名
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byName;

            /// <summary>
            /// 部门编号
            /// </summary>
            public ushort wDepartmentNo;

            /// <summary>
            /// 排班计划编号
            /// </summary>
            public ushort wSchedulePlanNo;

            /// <summary>
            /// 排班计划类型：0- 无意义，1- 个人，2- 部门
            /// </summary>
            public byte bySchedulePlanType;

            /// <summary>
            /// 保留，置为0 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;

            /// <summary>
            /// 锁ID 
            /// </summary>
            public uint dwLockID;

            /// <summary>
            /// 锁代码 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_LOCK_CODE_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byLockCode;

            /// <summary>
            /// 房间代码
            /// 按位表示，0-无权限，1-有权限
            /// 第0位表示：弱电报警
            /// 第1位表示：开门提示音
            /// 第2位表示：限制客卡
            /// 第3位表示：通道
            /// 第4位表示：反锁开门
            /// 第5位表示：巡更功能 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DOOR_CODE_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byRoomCode;

            /// <summary>
            /// 卡权限 
            /// bit，0-no，1-yes
            /// bit0：low voltage alarm
            /// bit1：open door with prompt tone
            /// bit2：limit customer card 
            /// bit3：channel
            /// bit4：open locked door
            /// bit5：patrol function
            /// </summary>
            public uint dwCardRight;

            /// <summary>
            /// 计划模板(每天)各时间段是否启用，按位表示，0--不启用，1-启用
            /// </summary>
            public uint dwPlanTemplate;

            /// <summary>
            /// 持卡人ID 
            /// </summary>
            public uint dwCardUserId;

            /// <summary>
            /// 0-空，
            /// 1- MIFARE S50，
            /// 2- MIFARE S70，
            /// 3- FM1208 CPU卡，
            /// 4- FM1216 CPU卡，
            /// 5-国密CPU
            /// 0-NULL,1-MIFARE,2-S50MIFARE,3-S70FM1208,4-CPUFM1216,5-CPUGMB Algorithm CPU,6-ID Card,7-NFC
            /// </summary>
            public byte byCardModelType;

            /// <summary>
            /// 保留，置为0 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 83, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes3;

            public void Init()
            {
                byDoorRight = new byte[CHCNetSDK.MAX_DOOR_NUM_256];
                byBelongGroup = new byte[CHCNetSDK.MAX_GROUP_NUM_128];
                wCardRightPlan = new ushort[CHCNetSDK.MAX_DOOR_NUM_256 * CHCNetSDK.MAX_CARD_RIGHT_PLAN_NUM];
                byCardNo = new byte[CHCNetSDK.ACS_CARD_NO_LEN];
                byCardPassword = new byte[CHCNetSDK.CARD_PASSWORD_LEN];
                byName = new byte[CHCNetSDK.NAME_LEN];
                byRes2 = new byte[3];
                byLockCode = new byte[CHCNetSDK.MAX_LOCK_CODE_LEN];
                byRoomCode = new byte[CHCNetSDK.MAX_DOOR_CODE_LEN];
                byRes3 = new byte[83];
            }
        }

        /// <summary>
        /// 卡参数配置条件结构体
        /// 设置卡参数（下发卡参数）时，如果将byCheckCardNo置为0，那么设备将不校验应用层下发的卡号信息，
        /// 直接写入本地存储，可以一定程度提高卡号下发的速度，
        /// 但是需要上层应用自己保证卡号信息不重复（整型值不能重复，比如，不能同时含有1和01这两种卡号）。
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_CARD_CFG_COND
        {
            /// <summary>
            /// 结构体大小 
            /// </summary>
            public uint dwSize;

            /// <summary>
            /// 设置或获取卡数量，获取时置为0xffffffff表示获取所有卡信息 
            /// </summary>
            public uint dwCardNum;

            /// <summary>
            /// 设备是否进行卡号校验：0- 不校验，1- 校验 
            /// </summary>
            public byte byCheckCardNo;

            /// <summary>
            /// 保留，置为0 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;

            /// <summary>
            /// 就地控制器序号，表示往就地控制器下发离线卡参数，0代表是门禁主机
            /// </summary>
            public ushort wLocalControllerID;

            /// <summary>
            /// 保留，置为0
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;

            /// <summary>
            /// 锁ID 
            /// </summary>
            public uint dwLockID;

            /// <summary>
            /// 保留，置为0
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes3;
        }

        /// <summary>
        /// 指纹参数配置条件结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FINGER_PRINT_INFO_COND
        {
            /// <summary>
            /// 结构体大小
            /// </summary>
            public uint dwSize;

            /// <summary>
            /// 指纹关联的卡号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo;

            /// <summary>
            /// 指纹的读卡器是否有效，数组下标表示读卡器序号，数组值：0- 无效，1- 有效
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CARD_READER_NUM_512, ArraySubType = UnmanagedType.I1)]
            public byte[] byEnableCardReader;

            /// <summary>
            /// 设置或获指纹数量，获取时置为0xffffffff表示获取所有指纹信息
            /// </summary>
            public uint dwFingerPrintNum;

            /// <summary>
            /// 指纹编号，有效值范围为1~10，获取时置为0xff表示该卡所有指纹
            /// </summary>
            public byte byFingerPrintID;

            /// <summary>
            /// 设备回调方式：0- 已向所有读卡器下发完成，1- 在时间段内只下发了部分也返回 
            /// </summary>
            public byte byCallbackMode;

            /// <summary>
            /// 保留 
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;

            public void Init()
            {
                byCardNo = new byte[ACS_CARD_NO_LEN];
                byEnableCardReader = new byte[MAX_CARD_READER_NUM_512];
                byRes1 = new byte[26];
            }
        }

        /// <summary>
        /// 人脸参数配置条件结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACE_PARAM_COND
        {
            /// <summary>
            /// 结构体大小 
            /// </summary>
            public uint dwSize;

            /// <summary>
            /// 人脸关联的卡号
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo;

            /// <summary>
            /// 人脸的读卡器是否有效，按数组表示，每位数组表示一个读卡器，数组取值：0-无效，1-有效 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_CARD_READER_NUM_512, ArraySubType = UnmanagedType.I1)]
            public byte[] byEnableCardReader;

            /// <summary>
            /// 设置或获取人脸数量，获取时置为0xffffffff表示获取所有人脸信息
            /// </summary>
            public uint dwFaceNum;

            /// <summary>
            /// 人脸ID编号，有效取值范围：1~2，0xff表示该卡所有人脸
            /// </summary>
            public byte byFaceID;

            /// <summary>
            /// 保留，置为0 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 127, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        /// <summary>
        /// 获取卡参数的发送数据
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_CARD_CFG_SEND_DATA
        {
            /// <summary>
            /// 结构体大小 
            /// </summary>
            public uint dwSize;

            /// <summary>
            /// 卡号 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo; //card No 

            /// <summary>
            /// 持卡人ID 
            /// </summary>
            public uint dwCardUserId;

            /// <summary>
            /// 保留，置为0 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        /// <summary>
        /// 人脸参数配置结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACE_PARAM_CFG
        {
            /// <summary>
            /// 结构体大小
            /// </summary>
            public uint dwSize;

            /// <summary>
            /// 人脸关联的卡号
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo;

            /// <summary>
            /// 人脸数据长度
            /// </summary>
            public uint dwFaceLen;

            /// <summary>
            /// 人脸数据缓冲区指针，dwFaceLen不为0时存放人脸数据（DES加密处理，设备端返回的即加密后的数据）
            /// </summary>
            public IntPtr pFaceBuffer;

            /// <summary>
            /// 需要下发人脸的读卡器，按数组表示，每位数组表示一个读卡器，
            /// 数组取值：0-不下发该读卡器，1-下发到该读卡器
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_CARD_READER_NUM_512, ArraySubType = UnmanagedType.I1)]
            public byte[] byEnableCardReader;

            /// <summary>
            /// 人脸ID编号，有效取值范围：1~2 
            /// </summary>
            public byte byFaceID;

            /// <summary>
            /// 人脸数据类型：0- 模板（默认），1- 图片 
            /// </summary>
            public byte byFaceDataType;

            /// <summary>
            /// 保留，置为0
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 126, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        /// <summary>
        /// 人脸参数下发状态信息结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACE_PARAM_STATUS
        {
            /// <summary>
            /// 结构体大小
            /// </summary>
            public uint dwSize;

            /// <summary>
            /// 人脸关联的卡号 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo;

            /// <summary>
            /// 人脸读卡器状态，按数组表示，每位数组表示一个读卡器，取值：
            /// 0-失败，
            /// 1-成功，
            /// 2-重试或人脸质量差，
            /// 3-内存已满，
            /// 4-已存在该人脸，
            /// 5-非法人脸ID，
            /// 6-算法建模失败，
            /// 7-未下发卡权限，
            /// 8-未定义（保留），
            /// 9-人眼间距小，
            /// 10-图片数据长度小于1KB，
            /// 11-图片格式不符（png/jpg/bmp）,
            /// 12-图片像素数量超过上限，
            /// 13-图片像素数量低于下限，
            /// 14-图片信息校验失败，
            /// 15-图片解码失败，
            /// 16-人脸检测失败，
            /// 17-人脸评分失败 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_CARD_READER_NUM_512, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardReaderRecvStatus;

            /// <summary>
            /// 下发错误信息，当byCardReaderRecvStatus为4时表示已存在人脸对应的卡号
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ERROR_MSG_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byErrorMsg;

            /// <summary>
            /// 指纹读卡器编号 
            /// </summary>
            public uint dwCardReaderNo;

            /// <summary>
            /// 下发总的状态：
            /// 0- 当前人脸未下完所有读卡器，
            /// 1- 已下完所有读卡器(这里的所有指的是门禁主机往所有的读卡器下发了，不管成功与否) 
            /// </summary>
            public byte byTotalStatus;

            /// <summary>
            /// 人脸ID编号，有效取值范围：1~2 
            /// </summary>
            public byte byFaceID;

            /// <summary>
            /// 保留，置为0 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 130, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        #endregion

    }
}
