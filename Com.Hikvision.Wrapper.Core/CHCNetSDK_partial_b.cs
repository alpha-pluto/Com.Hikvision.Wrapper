/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29 
 * 
 * daniel.zhang
 * 
 * 文件名：CHCNetSDK_partial_b.cs
 * 
 * 文件功能描述：SDK DLL 调用入口声明文件
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Com.Hikvision.Wrapper.Core
{
    public partial class CHCNetSDK
    {

        #region 参数设定

        public const int NET_DVR_DEV_ADDRESS_MAX_LEN = 129;
        public const int NET_DVR_LOGIN_USERNAME_MAX_LEN = 64;
        public const int NET_DVR_LOGIN_PASSWD_MAX_LEN = 64;

        public const int NET_SDK_MAX_FDID_LEN = 256;
        public const int NET_SDK_MAX_INDENTITY_KEY_LEN = 64;

        //public const int NAME_LEN = 32;

        //上传证书
        //public const int UPLOAD_CERTIFICATE = 1;
        //上传录像文件(云存储模式)
        public const int UPLOAD_RECORD_FILE = 4;
        //上传黑白名单配置文件功能
        public const int UPLOAD_VEHICLE_BLACKWHITELST_FILE = 13;
        //导入人脸数据(人脸图片+图片附加信息)到人脸库
        public const int IMPORT_DATA_TO_FACELIB = 39;

        public const int MAX_URL_LEN = 240;

        /// <summary>
        /// 图片上传状态返回值
        /// </summary>
        public enum UploadState
        {

            Unknow = -1,

            /// <summary>
            /// 1    上传成功    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("上传成功")]
            Success = 1,

            /// <summary>
            /// 2    正在上传    不需要处理 
            /// </summary>
            [Description("正在上传")]
            Process = 2,

            /// <summary>
            /// 3    上传失败    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("上传失败")]
            Failed = 3,

            /// <summary>
            /// 4    网络断开，状态未知    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("网络断开，状态未知")]
            Disconnected = 4,

            /// <summary>
            /// 6    硬盘错误    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("硬盘错误")]
            DiskError = 6,

            /// <summary>
            /// 7    无审讯文件存放盘    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("无审讯文件存放盘")]
            NoStorage = 7,

            /// <summary>
            /// 8    容量不足    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("容量不足")]
            InsufficientCapacity = 8,

            /// <summary>
            /// 9    设备资源不足    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("设备资源不足")]
            InsufficientResource = 9,

            /// <summary>
            /// 10    文件个数超过最大值    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("文件个数超过最大值")]
            OutOfFileCount = 10,

            /// <summary>
            /// 11    文件过大    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("文件过大")]
            OutOfFileSize = 11,

            /// <summary>
            /// 12    文件类型错误    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("文件类型错误")]
            FileExtNotSupported = 12,

            /// <summary>
            /// 19    文件格式不正确    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("文件格式不正确")]
            FileFormatNotSupported = 19,

            /// <summary>
            /// 20    文件内容不正确    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("文件内容不正确")]
            InvalidFileContent = 20,

            /// <summary>
            /// 21    上传音频采样率不支持    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("上传音频采样率不支持")]
            AudioSampleRateNotSupported = 21,

            /// <summary>
            /// 26    名称错误    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("名称错误")]
            InvalidName = 26,

            /// <summary>
            /// 27    图片分辨率无效错误    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("图片分辨率无效错误")]
            InvalidResolution = 27,

            /// <summary>
            /// 28    图片目标个数超过上限    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("图片目标个数超过上限")]
            NumberOfPicExceedUpperlimit = 28,

            /// <summary>
            /// 29    图片未识别到目标    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("图片未识别到目标")]
            NotRecognized = 29,

            /// <summary>
            /// 30    图片数据识别失败    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("图片数据识别失败")]
            RecognitionFailed = 30,

            /// <summary>
            /// 31    分析引擎异常    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("分析引擎异常")]
            AnalysisEngineException = 31,

            /// <summary>
            /// 32    解析图片数据出错    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("解析图片数据出错")]
            ErrorParsingPicData = 32,

            /// <summary>
            /// 34    安全校验密钥错误    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("安全校验密钥错误")]
            InvalidSecKey = 34,

            /// <summary>
            /// 35    图片URL未开始下载    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("图片URL未开始下载")]
            UrlDownloadNotStarted = 35,

            /// <summary>
            /// 36    自定义人员ID重复    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("自定义人员ID重复")]
            CustomHumanIdDuplicated = 36,

            /// <summary>
            /// 37    自定义人员ID有误(注:该ID存在附加信息FaceAppendData中customHumanID)    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("自定义人员ID有误(注:该ID存在附加信息FaceAppendData中customHumanID)")]
            CustomHumanIdError = 37,

            /// <summary>
            /// 38    建模失败,设备内部错误    关闭连接，需要调用NET_DVR_UploadClose释放资源 
            /// </summary>
            [Description("建模失败,设备内部错误")]
            DeviceModelingFailed = 38,

            /// <summary>
            /// 39    建模失败，人脸建模错误    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("建模失败，人脸建模错误")]
            FaceModelingFailed = 39,

            /// <summary>
            /// 40    建模失败，人脸质量评分错误    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("建模失败，人脸质量评分错误")]
            FaceRankModelingFailed = 40,

            /// <summary>
            /// 41    建模失败，特征点提取错误    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("建模失败，特征点提取错误")]
            FeatureExtModelingFailed = 41,

            /// <summary>
            /// 42    建模失败，属性提取错误    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("建模失败，属性提取错误")]
            PropertyModelingFailed = 42,

            /// <summary>
            /// 43    图片数据错误    不会断开连接，可以继续重新上传正确的文件 
            /// </summary>
            [Description("图片数据错误")]
            PicDataError = 43,

            /// <summary>
            /// 44    图片附加信息错误    
            /// </summary>
            [Description("图片附加信息错误")]
            PicAppendixDataError = 44,


        }

        #endregion

        /// <summary>
        /// 用户登录参数结构体
        /// </summary>
        /// <param name="userId">NET_DVR_Login_V40的返回值</param>
        /// <param name="result"></param>
        /// <param name="deviceInfo"></param>
        /// <param name="pUser"></param>
        public delegate void LoginResultCallBack(int userId, uint result, ref NET_DVR_DEVICEINFO_V30 deviceInfo, IntPtr pUser);

        /// <summary>
        /// 用户登录参数结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_USER_LOGIN_INFO
        {
            /// <summary>
            /// 设备地址，IP 或者普通域名
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NET_DVR_DEV_ADDRESS_MAX_LEN, ArraySubType = UnmanagedType.I1)]
            public char[] sDeviceAddress;

            /// <summary>
            /// 是否启用能力集透传：0- 不启用透传，默认；1- 启用透传 
            /// </summary>
            public byte byUseTransport;

            /// <summary>
            /// 设备端口号，例如：8000 
            /// </summary>
            public UInt16 wPort;

            /// <summary>
            /// 登录用户名，例如：admin 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NET_DVR_LOGIN_USERNAME_MAX_LEN, ArraySubType = UnmanagedType.I1)]
            public char[] sUserName;

            /// <summary>
            /// 登录密码，例如：12345 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NET_DVR_LOGIN_PASSWD_MAX_LEN, ArraySubType = UnmanagedType.I1)]
            public char[] sPassword;

            /// <summary>
            /// 登录状态回调函数，bUseAsynLogin 为1时有效 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.FunctionPtr)]
            public LoginResultCallBack cbLoginResult;

            /// <summary>
            /// 用户数据
            /// </summary>
            public IntPtr pUser;

            /// <summary>
            /// 是否异步登录：0- 否，1- 是 
            /// </summary>
            public bool bUseAsynLogin;

            /// <summary>
            /// 代理服务器类型：0- 不使用代理，1- 使用标准代理，2- 使用EHome代理
            /// </summary>
            public byte byProxyType;

            /// <summary>
            /// 是否使用UTC时间：
            /// 0- 不进行转换，默认；
            /// 1- 输入输出UTC时间，SDK进行与设备时区的转换；
            /// 2- 输入输出平台本地时间，SDK进行与设备时区的转换
            /// </summary>
            public byte byUseUTCTime;

            /// <summary>
            /// 登录模式(不同模式具体含义详见“Remarks”说明)：
            /// 0- SDK私有协议，
            /// 1- ISAPI协议，
            /// 2- 自适应（设备支持协议类型未知时使用，一般不建议）
            /// </summary>
            public byte byLoginMode;

            /// <summary>
            /// ISAPI协议登录时是否启用HTTPS(byLoginMode为1时有效)：
            /// 0- 不启用，
            /// 1- 启用，
            /// 2- 自适应（设备支持协议类型未知时使用，一般不建议）
            /// </summary>
            public byte byHttps;

            /// <summary>
            /// 代理服务器序号，添加代理服务器信息时相对应的服务器数组下表值
            /// </summary>
            public int iProxyID;

            /// <summary>
            /// 保留，置为0 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes3;


        }

        /// <summary>
        /// 设备参数结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_DEVICEINFO_V40
        {
            /// <summary>
            /// 设备参数 
            /// </summary>
            public NET_DVR_DEVICEINFO_V30 struDeviceV30;

            /// <summary>
            /// 设备是否支持锁定功能，bySupportLock为1时，dwSurplusLockTime和byRetryLoginTime有效
            /// </summary>
            public byte bySupportLock;

            /// <summary>
            /// 剩余可尝试登陆的次数，用户名、密码错误时，此参数有效 
            /// </summary>
            public byte byRetryLoginTime;

            /// <summary>
            /// 密码安全等级：
            /// 0- 无效，
            /// 1- 默认密码，
            /// 2- 有效密码，
            /// 3- 风险较高的密码，
            /// 当管理员用户的密码为出厂默认密码（12345）或者风险较高的密码时，建议上层客户端提示用户更改密码 
            /// </summary>
            public byte byPasswordLevel;

            /// <summary>
            /// 代理服务器类型：
            /// 0- 不使用代理，
            /// 1- 使用标准代理，
            /// 2- 使用EHome代理 
            /// </summary>
            public byte byProxyType;

            /// <summary>
            /// 剩余时间，单位：秒，用户锁定时此参数有效。
            /// 在锁定期间，用户尝试登陆，不管用户名密码输入对错，设备锁定剩余时间重新恢复到30分钟 
            /// </summary>
            public uint dwSurplusLockTime;

            /// <summary>
            /// 字符编码类型（SDK所有接口返回的字符串编码类型，透传接口除外）：
            /// 0- 无字符编码信息(老设备)，
            /// 1- GB2312(简体中文)，
            /// 2- GBK，
            /// 3- BIG5(繁体中文)，
            /// 4- Shift_JIS(日文)，
            /// 5- EUC-KR(韩文)，
            /// 6- UTF-8，
            /// 7- ISO8859-1，
            /// 8- ISO8859-2，
            /// 9- ISO8859-3，
            /// …，
            /// 依次类推，
            /// 21- ISO8859-15(西欧)
            /// </summary>
            public byte byCharEncodeType;

            /// <summary>
            /// 支持v50版本的设备参数获取，设备名称和设备类型名称长度扩展为64字节
            /// </summary>
            public byte bySupportDev5;

            /// <summary>
            /// 登录模式(不同模式具体含义详见“Remarks”说明)：
            /// 0- SDK私有协议，
            /// 1- ISAPI协议
            /// </summary>
            public byte byLoginMode;

            /// <summary>
            /// 保留，置为0
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 253, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;


        }

        /// <summary>
        /// 导入人脸数据(人脸图片+图片附件信息)到人脸库的条件参数结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACELIB_COND
        {
            //结构体大小
            public uint dwSize;

            //人脸库ID(设备自动生成的FDID或者是自定义的customFaceLibID)，唯一标识
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NET_SDK_MAX_FDID_LEN, ArraySubType = UnmanagedType.I1)]
            public char[] szFDID;

            //设备并发处理：0- 不开启(设备自动会建模)，1- 开始(设备不会自动进行建模)
            public byte byConcurrent;

            //是否覆盖式导入(人脸库存储满的情况下强制覆盖导入时间最久的图片数据)：0- 否，1- 是 
            public byte byCover;

            //人脸库ID是否是自定义：0- 不是，1- 是 
            public byte byCustomFaceLibID;

            //保留，置为0 
            public byte byRes1;

            //交互操作口令
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NET_SDK_MAX_INDENTITY_KEY_LEN, ArraySubType = UnmanagedType.I1)]
            public char[] byIdentityKey;

            //保留，置为0
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;

        }

        /// <summary>
        /// 数据发送输入参数结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_SEND_PARAM_IN
        {
            /// <summary>
            /// 发送的缓冲区，存放图片二进制数据
            /// </summary>
            public IntPtr pSendData;

            /// <summary>
            /// 发送数据长度
            /// </summary>
            public uint dwSendDataLen;

            /// <summary>
            /// 图片时间
            /// </summary>
            public NET_DVR_TIME_V30 struTime;

            /// <summary>
            /// 图片格式：1- jpg，2- bmp，3- png，4- SWF，5- GIF 
            /// 目前人脸库只支持jpg图片格式
            /// </summary>
            public byte byPicType;

            /// <summary>
            /// 图片格式：1- jpg，2- bmp，3- png，4- SWF，5- GIF 
            /// </summary>
            public byte byPicURL;

            /// <summary>
            /// 保留，置为0
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;

            /// <summary>
            /// 图片管理号，人脸库不支持，设为0
            /// </summary>
            public uint dwPicMangeNo;

            /// <summary>
            /// 图片名称
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] sPicName;

            /// <summary>
            /// 图片播放时长，单位：秒，人脸库不支持，设为0
            /// </summary>
            public uint dwPicDisplayTime;

            /// <summary>
            /// 发送图片的附加信息缓冲区，对应XML格式数据：FaceAppendData 
            /// </summary>
            public IntPtr pSendAppendData;

            /// <summary>
            /// 发送图片的附加信息数据长度
            /// </summary>
            public uint dwSendAppendDataLen;

            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 192, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;

        }

        /// <summary>
        /// 文件上传结果信息结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_UPLOAD_FILE_RET
        {
            /// <summary>
            /// 文件上传返回的URL或者ID 
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_URL_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] sUrl;

            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 260, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        /// <summary>
        /// 
        /// 门禁控制功能使用的是该接口，报警主机的辅助功能控制中的电锁、移动门控制通过接口NET_DVR_AlarmHostAssistantControl实现。
        /// 控制智能锁为2-常开（自由、通道状态）时，到当天24时，会自动变成4-恢复（普通状态。
        /// 
        /// </summary>
        /// <param name="lUserID">NET_DVR_Login_V40等登录接口的返回值</param>
        /// <param name="lGatewayIndex">门禁序号（楼层编号、锁ID），从1开始，-1表示对所有门（或者梯控的所有楼层）进行操作</param>
        /// <param name="dwStaic">命令值：0- 关闭（对于梯控，表示受控），1- 打开（对于梯控，表示开门），2- 常开（对于梯控，表示自由、通道状态），3- 常关（对于梯控，表示禁用），4- 恢复（梯控，普通状态），5- 访客呼梯（梯控），6- 住户呼梯（梯控）</param>
        /// <returns>TRUE表示成功，FALSE表示失败。接口返回失败请调用NET_DVR_GetLastError获取错误码，通过错误码判断出错原因。</returns>
        [DllImport(@"HCNetSDK.dll")]
        public static extern bool NET_DVR_ControlGateway(int lUserID, int lGatewayIndex, uint dwStaic);

        [DllImport(@"HCNetSDK.dll")]
        public static extern bool NET_DVR_AlarmHostAssistantControl(int lUserId, uint cmdType, uint number, uint cmdPara);

        #region upload pic

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="userId">NET_DVR_Login_V40等登录接口的返回值</param>
        /// <param name="uploadType">上传文件类型，详细参见文档</param>
        /// <param name="inBuffer">输入参数。不同的dwUploadType，输入参数不同,详细参见文档</param>
        /// <param name="inBufferSize">输入缓冲区大小</param>
        /// <param name="fileName">上传文件的路径（包括文件名）</param>
        /// <param name="outBuffer">输出参数。不同的dwUploadType，输出参数不同，具体参见文档</param>
        /// <param name="outBufferSize">输出缓冲区大小</param>
        /// <returns></returns>
        [DllImport(@"HCNetSDK.dll")]
        public static extern int NET_DVR_UploadFile_V40(int userId, uint uploadType, IntPtr inBuffer, uint inBufferSize, string fileName, IntPtr outBuffer, out uint outBufferSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadHandle"></param>
        /// <param name="struSendParamIN"></param>
        /// <param name="outBuffer"></param>
        /// <returns></returns>
        [DllImport(@"HCNetSDK.dll")]
        public static extern int NET_DVR_UploadSend(int uploadHandle, ref NET_DVR_SEND_PARAM_IN struSendParamIN, IntPtr outBuffer);

        /// <summary>
        /// 获取文件上传的进度和状态
        /// </summary>
        /// <param name="uploadHandle">文件上传的句柄，NET_DVR_UploadFile_V40的返回值</param>
        /// <param name="progress">返回的进度值，0~100 </param>
        /// <returns></returns>
        [DllImport(@"HCNetSDK.dll")]
        public static extern int NET_DVR_GetUploadState(int uploadHandle, out uint progress);

        /// <summary>
        /// 获取当前上传的结果信息
        /// </summary>
        /// <param name="uploadHandle"></param>
        /// <param name="outBuffer"></param>
        /// <param name="utBufferSize"></param>
        /// <returns></returns>
        [DllImport(@"HCNetSDK.dll")]
        public static extern bool NET_DVR_GetUploadResult(int uploadHandle, IntPtr outBuffer, out uint utBufferSize);

        /// <summary>
        /// 停止文件上传
        /// </summary>
        /// <param name="uploadHandle"></param>
        /// <returns></returns>
        [DllImport(@"HCNetSDK.dll")]
        public static extern bool NET_DVR_UploadClose(int uploadHandle);

        /// <summary>
        /// 用户注册设备（支持异步登录）。
        /// </summary>
        /// <param name="pLoginInfo"></param>
        /// <param name="lpDeviceInfo"></param>
        /// <returns></returns>
        [DllImport(@"HCNetSDK.dll")]
        public static extern int NET_DVR_Login_V40(ref NET_DVR_USER_LOGIN_INFO pLoginInfo, ref NET_DVR_DEVICEINFO_V40 lpDeviceInfo);

        #endregion

        #region remote config

        public delegate void RemoteConfigCallback(uint type, IntPtr buffer, uint bufferLength, IntPtr userData);

        /// <summary>
        /// 启动远程配置
        /// </summary>
        /// <param name="userId">NET_DVR_Login_V40等登录接口的返回值</param>
        /// <param name="command">配置命令，不同的功能对应不同的命令号(dwCommand)，lpInBuffer等参数也对应不同的内容</param>
        /// <param name="inBuffer">输入参数，具体内容跟配置命令相关</param>
        /// <param name="inBufferSize">输入缓冲的大小</param>
        /// <param name="cbStateCallback">状态回调函数</param>
        /// <param name="userData">用户数据</param>
        /// <returns></returns>
        [DllImport(@"HCNetSDK.dll")]
        public static extern int NET_DVR_StartRemoteConfig(int userId, uint command, IntPtr inBuffer, uint inBufferSize, RemoteConfigCallback cbStateCallback, IntPtr userData);

        /// <summary>
        /// 发送长连接数据
        /// </summary>
        /// <param name="handle">长连接句柄，NET_DVR_StartRemoteConfig的返回值</param>
        /// <param name="dataType">数据类型，跟长连接接口NET_DVR_StartRemoteConfig的命令参数（dwCommand）</param>
        /// <param name="pSendBuf">保存发送数据的缓冲区，与dwDataType有关</param>
        /// <param name="bufSize">发送数据的长度</param>
        /// <returns></returns>
        [DllImportAttribute(@"HCNetSDK.dll")]
        public static extern bool NET_DVR_SendRemoteConfig(int handle, uint dataType, IntPtr pSendBuf, uint bufSize);

        /// <summary>
        /// 关闭长连接配置接口所创建的句柄，释放资源
        /// </summary>
        /// <param name="handle">句柄，NET_DVR_StartRemoteConfig的返回值</param>
        /// <returns></returns>
        [DllImportAttribute(@"HCNetSDK.dll")]
        public static extern bool NET_DVR_StopRemoteConfig(int handle);

        #endregion

    }
}
