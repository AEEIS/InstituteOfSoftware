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
    /// 学员出勤类
    /// </summary>
    [Table(name: "StudentAttendance")]
    public  class StudentAttendance
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public Nullable<System.DateTime> InspectionDate { get; set; }
        /// <summary>
        /// 违规类型
        /// </summary>
        public string Attendancestatus { get; set; }
        /// <summary>
        /// 员工
        /// </summary>
        public string Registrar { get; set; }
        /// <summary>
        /// 出勤类型
        /// </summary>
        public string AttendanceTitle { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public Nullable<System.DateTime> Addtime { get; set; }
        /// <summary>
        /// 班级id
        /// </summary>
        public int ClassID { get; set; }
    }
}
