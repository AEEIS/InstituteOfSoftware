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
    /// 就业部调研学生记录表
    /// </summary>
    [Table(name: "SurveyRecords")]
    public partial class SurveyRecords
    {
        /// <summary>
        /// 调研记录ID
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 学生学号
        /// </summary>
        public string StudentNO { get; set; }
        /// <summary>
        /// 就业专员ID
        /// </summary>
        public int EmpStaffID { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public Nullable<System.DateTime> RecordsDate { get; set; }

        /// <summary>
        /// 就业专员评语
        /// </summary>
        public string EmpStaffComments { get; set; }
        /// <summary>
        /// 就业期望
        /// </summary>
        public string EmpExpectations { get; set; }
        /// <summary>
        /// 掌握技术
        /// </summary>
        public string MasterTechnology { get; set; }
        /// <summary>
        /// 实践经验
        /// </summary>
        public string PracticalExperience { get; set; }
        /// <summary>
        /// 自评等级
        /// </summary>
        public string SelfRating { get; set; }
        /// <summary>
        /// 调研评级
        /// </summary>
        public string SurRating { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public Nullable<bool> IsDel { get; set; }
        /// <summary>
        /// 想学的专业
        /// </summary>
        public Nullable<int> WantSpceID { get; set; }
    
    }
}
