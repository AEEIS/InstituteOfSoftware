﻿using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    public class RoomdeWithPageXmlHelp
    {


        /// <summary>
        /// 根据类型获取房间
        /// </summary>
        /// <param name="roomType"></param>
        /// <returns></returns>
        public int GetRoomType(RoomTypeEnum.RoomType roomType) {
            string roomtypename = roomType.ToString();
            int value = 0;
            XElement xe = XElement.Load(System.Web.HttpContext.Current.Server.MapPath("/xmlfile/RoomdeWithPage.xml"));
            IEnumerable<XElement> elements = from ele in xe.Elements(roomtypename)
                                             select ele;
            foreach (var ele in elements)
            {
                value = int.Parse(ele.Attribute("id").Value);
            }
            return value;
        }

        /// <summary>
        /// 根据性别获取性别数据
        /// </summary>
        /// <param name="sexType"></param>
        /// <returns></returns>
        public int Getmale(RoomTypeEnum.SexType sexType) {
            string sextypename = sexType.ToString();
            int value = 0;

            XElement xe = XElement.Load(System.Web.HttpContext.Current.Server.MapPath("/xmlfile/RoomdeWithPage.xml"));
            IEnumerable<XElement> elements = from ele in xe.Elements(sextypename)
                                             select ele;
            foreach (var ele in elements)
            {
                value = int.Parse(ele.Attribute("val").Value);
            }
            return value;
        }

        /// <summary>
        /// 根据传的东西获取项目名称
        /// </summary>
        /// <param name="projectname"></param>
        /// <returns></returns>
        public string GetPointsdeductionproject(string projectname) {
            XElement xe = XElement.Load(System.Web.HttpContext.Current.Server.MapPath("/xmlfile/Pointsdeductionproject.xml"));
            IEnumerable<XElement> elements = from ele in xe.Elements("project")
                                             select ele;
            foreach (var ele in elements)
            {
                if (ele.Element("value").Value==projectname)
                {
                    return ele.Element("title").Value;
                }
            }
            return string.Empty;
        }
    }
}
