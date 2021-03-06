//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 教员表
    /// </summary>
    [Table(name: "Teacher")]
    public partial class Teacher
    {
       
        [Key]
        public int TeacherID { get; set; }
        /// <summary>
        /// 员工编号（外键）
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 工作经验
        /// </summary>
        public string WorkExperience { get; set; }
        /// <summary>
        /// 项目经验
        /// </summary>
        public string ProjectExperience { get; set; }
        /// <summary>
        /// 教学经验
        /// </summary>
        public string TeachingExperience { get; set; }
        /// <summary>
        /// 带班风格
        /// </summary>
        public string AttendClassStyle { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> IsDel { get; set; }
        /// <summary>
        /// 底课时
        /// </summary>
        public Nullable<int> MinimumCourseHours { get; set; }

       
    
    }

    
}
