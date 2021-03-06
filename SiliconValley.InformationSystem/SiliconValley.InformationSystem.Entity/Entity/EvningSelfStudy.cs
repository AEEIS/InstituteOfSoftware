﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    /// <summary>
    /// 晚自习实体类
    /// </summary>
    [Table("EvningSelfStudy")]
    public class EvningSelfStudy
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 班级编号
        /// </summary>
        public int ClassSchedule_id { get; set; }
        /// <summary>
        /// 教室编号
        /// </summary>
        public int Classroom_id { get; set; }
        /// <summary>
        /// 上课时间段
        /// </summary>
        public string curd_name { get; set; }
        /// <summary>
        /// 排课时间
        /// </summary>
        public DateTime Anpaidate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Newdate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Rmark { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 教员编号
        /// </summary>
        public string emp_id { get; set; }

        public EvningSelfStudy ()
        {

        }

        public EvningSelfStudy(int ClassSchedule_id,int Classroom_id,string curd_name,DateTime Anpaidate,DateTime Newdate,string emp_id,bool IsDelete)
        {
            this.ClassSchedule_id = ClassSchedule_id;
            this.Classroom_id = Classroom_id;
            this.curd_name = curd_name;
            this.Anpaidate = Anpaidate;
            this.Newdate = Newdate;
            this.IsDelete = IsDelete;
        }
    }
}
