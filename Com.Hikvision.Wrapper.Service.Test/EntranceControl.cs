/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29
 * 
 * daniel.zhang
 * 
 * 文件名：EntranceControl.cs
 * 
 * 文件功能描述：卡片管理服务单元测试类
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.IO;
using Com.Hikvision.Wrapper.Core;
using Com.Hikvision.Wrapper.Service.EntranceControl.RemoteEntranceControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Com.Hikvision.Wrapper.Service.Test
{
    [TestClass]
    public class EntranceControl
    {
        [TestMethod]
        public void RemoteEntranceContorlGateway()
        {
            ControlGateway srvCtrlGateway = new ControlGateway();
            bool ret = false;
            string message = string.Empty;
            string deviceIPAddr = "127.0.0.1";
            int port = 8000;
            int doorId = 1;
            string user = "******";
            string password = "******";
            ret = srvCtrlGateway.RemoteControlGatewayOpen(out message, deviceIPAddr, port, doorId, user, password);
            Assert.AreEqual(ret, true, message);
        }

        [TestMethod]
        public void CardQuery()
        {
            CardManage srvCardManage = new CardManage();
            bool ret = false;
            string message = string.Empty;
            string deviceIPAddr = "127.0.0.1";
            int port = 8000;
            string cardNo = "123456";
            uint employeeNo = 0;
            string user = "******";
            string password = "******";
            ret = srvCardManage.CardQuery(out message, out employeeNo, deviceIPAddr, port, cardNo, user, password);
            Assert.AreEqual(employeeNo, Convert.ToUInt32(cardNo));
        }

        [TestMethod]
        public void CardInsert()
        {

            CardManage srvCardManage = new CardManage();
            bool ret = false;
            string message = string.Empty;
            string deviceIPAddr = "127.0.0.1";
            int port = 8000;
            Core.Domain.EntranceControl.UserInfo userInfo = new Core.Domain.EntranceControl.UserInfo();
            userInfo.CardNo = "123456";
            userInfo.EmployeeNo = 123456;
            userInfo.Name = "张三";
            userInfo.DepartmentNo = 123;
            userInfo.CardType = CHCNetSDK.CARD_TYPE.NORMAL;
            userInfo.CardPassword = "******";
            string user = "******";
            string password = "******";
            ret = srvCardManage.CardInsert(out message, deviceIPAddr, port, userInfo, user, password);
            Console.WriteLine(message);
            Assert.AreEqual(ret, true);

        }

        [TestMethod]
        public void FaceQuery()
        {
            FaceManage srvFaceManage = new FaceManage();
            bool ret = false;
            string message = string.Empty;
            string deviceIPAddr = "127.0.0.1";
            int port = 8000;
            string cardNo = "123456";// 
            string user = "******";
            string password = "******";
            ret = srvFaceManage.FaceQuery(out message, deviceIPAddr, port, cardNo, user, password);
            Assert.AreEqual(ret, true);
        }

        [TestMethod]
        public void FaceInsert()
        {

            FaceManage srvFaceManage = new FaceManage();
            bool ret = false;
            string message = string.Empty;
            string deviceIPAddr = "127.0.0.1";
            int port = 8000;
            Core.Domain.EntranceControl.UserInfo userInfo = new Core.Domain.EntranceControl.UserInfo();
            userInfo.CardNo = "123456";
            userInfo.EmployeeNo = 123456;
            userInfo.Name = "张三";
            userInfo.DepartmentNo = 12;
            userInfo.CardType = CHCNetSDK.CARD_TYPE.NORMAL;
            userInfo.CardPassword = "******";
            string user = "******";
            string password = "******";

            using (FileStream fs = new FileStream("e:\\face-storage\\" + userInfo.CardNo + ".jpg", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && fs.Length < 200 * 1024)
                {
                    userInfo.Avator = new byte[fs.Length];
                    fs.Read(userInfo.Avator, 0, (int)fs.Length);
                    ret = srvFaceManage.FaceInsert(out message, deviceIPAddr, port, userInfo, user, password);
                }
                else
                {
                    message = "人脸文件不能大于200K";
                    ret = false;
                }
            }

            Console.WriteLine(message);
            //Assert.AreEqual(ret, true);

        }

        [TestMethod]
        public void MemberEnroll()
        {
            string message = string.Empty;
            bool ret = false;
            string deviceIPAddr = "127.0.0.1";
            int port = 8000;
            Core.Domain.EntranceControl.UserInfo userInfo = new Core.Domain.EntranceControl.UserInfo();
            userInfo.CardNo = "123456";
            userInfo.EmployeeNo = 123456;
            userInfo.Name = "张三";
            userInfo.DepartmentNo = 123;
            userInfo.CardType = CHCNetSDK.CARD_TYPE.NORMAL;
            userInfo.CardPassword = "******";
            string user = "******";
            string password = "******";
            Application.EntranceControl.ControlGateway cw = new Application.EntranceControl.ControlGateway();
            ret = cw.MemberEnroll(out message, deviceIPAddr, port, userInfo.CardNo, userInfo.EmployeeNo, userInfo.Name, userInfo.DepartmentNo, "http://remote_addr/storage/123456.jpg", user, password);
            Console.WriteLine(message);
            //Assert.AreEqual(ret, true);
        }


    }
}
