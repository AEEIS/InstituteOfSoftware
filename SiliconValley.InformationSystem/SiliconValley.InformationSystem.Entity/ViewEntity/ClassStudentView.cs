﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
  public  class ClassStudentView
    {
        /// <summary>
        /// 学员学号
        /// </summary>
        public string StuNameID { get; set; }//
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string Name { get; set; }//
        /// <summary>
        /// 学员性别
        /// </summary>
        public bool Sex { get; set; }//
        /// <summary>
        /// 班委名称
        /// </summary>
        public object Nameofmembers { get; set; }//
         /// <summary>
         /// 当前班级
         /// </summary>
        public object ClassNameView { get; set; }//
        /// <summary>
        /// 状态名称
        /// </summary>
        public string Statusname { get; set; }
        /// <summary>
        /// 当前班级id
        /// </summary>
        public int ClassID { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string identitydocument { get; set; }
    }
}
