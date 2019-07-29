/*----------------------------------------------------------------
 * 
 * 
 * Hikvision SDK
 * 
 * 2019-7-29
 * 
 * daniel.zhang
 * 
 * 文件名：FaceAppendData.cs
 * 
 * 文件功能描述：人脸比对库图片数据关联信息配置 封装类
 * 
 * 修改记录：
 * 
----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Com.Hikvision.Wrapper.Core.Domain.Smart.FaceStorage
{
    [Serializable]
    [XmlRoot("FaceAppendData")]
    public class FaceAppendData
    {
        private string bornTime;

        private List<PersonInfoExtend> personInfoExtendList;

        private List<RegionCoordinates> regionCoordinatesList;

        public FaceAppendData()
        {
            personInfoExtendList = new List<PersonInfoExtend>();
            regionCoordinatesList = new List<RegionCoordinates>();

        }

        /// <summary>
        /// opt
        /// </summary>
        [XmlElement("bornTime")]
        public string BornTime
        {
            set { bornTime = value; }
            get { return Utility.StringValidate.StringValidateForDatetimeISO8601(bornTime); }
        }

        /// <summary>
        /// opt
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// opt
        /// male,female
        /// </summary>
        [XmlElement("sex")]
        public string Gender { get; set; }

        /// <summary>
        /// opt
        /// </summary>
        [XmlElement("province")]
        public string Province { get; set; }

        /// <summary>
        /// opt
        /// </summary>
        [XmlElement("city")]
        public string City { get; set; }

        /// <summary>
        /// opt
        /// 证件类型: OfficerID-军官证, ID-身份证, passportID-护照, other-其他
        /// </summary>
        [XmlElement("certificateType")]
        public string CertificateType { get; set; }

        /// <summary>
        /// dep
        /// 证件号
        /// </summary>
        [XmlElement("certificateNumber")]
        public string CertificateNumber { get; set; }

        /// <summary>
        /// 人员扩展信息列表
        /// </summary>
        [XmlArrayItem(ElementName = "PersonInfoExtendList", Type = typeof(PersonInfoExtend), IsNullable = true)]
        public List<PersonInfoExtend> PersonInfoExtendList
        {
            get { return personInfoExtendList; }
            set { personInfoExtendList = value; }
        }

        [XmlElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayItem(ElementName = "RegionCoordinatesList", Type = typeof(RegionCoordinates), IsNullable = true)]
        public List<RegionCoordinates> RegionCoordinatesList
        {
            get { return regionCoordinatesList; }
            set { regionCoordinatesList = value; }
        }

        /// <summary>
        /// 自定义人员ID
        /// </summary>
        [XmlElement("customHumanID")]
        public string CustomHumanID { get; set; }

    }

    [Serializable]
    public class PersonInfoExtend
    {
        /// <summary>
        /// 人员扩展信息序号, 从"1"开始赋值, 依次增加
        /// </summary>
        [XmlElement("id")]
        public int Id { get; set; }

        /// <summary>
        /// 人员扩展信息使能
        /// </summary>
        [XmlElement("enable")]
        public bool Enable { get; set; }

        /// <summary>
        /// 人员扩展信息使能
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 人员标签信息扩展内容
        /// </summary>
        [XmlElement("value")]
        public string Value { get; set; }

    }

    /// <summary>
    /// 人脸目标范围矩形框点坐标
    /// </summary>
    [Serializable]
    public class RegionCoordinates
    {
        /// <summary>
        /// X坐标
        /// </summary>
        [XmlElement("positionX")]
        public int PositionX { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        [XmlElement("positionY")]
        public int PositionY { get; set; }

    }
}
