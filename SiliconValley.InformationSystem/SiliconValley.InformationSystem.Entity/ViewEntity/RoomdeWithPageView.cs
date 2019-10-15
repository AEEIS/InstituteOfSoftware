﻿using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于房间详细页面
    /// </summary>
   public class RoomdeWithPageView
    {
        /// <summary>
        /// 床位列表
        /// </summary>
        public List<BenNumber> BedList { get; set; }

        /// <summary>
        /// 学生列表
        /// </summary>
        public List<Accdationinformation> AccdationList { get; set; }
    }
}