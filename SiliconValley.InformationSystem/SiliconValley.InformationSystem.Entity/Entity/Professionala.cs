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
    /// 班主任培训表
    /// </summary>
    [Table(name: "Professionala")]
    public partial class Professionala
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 培训人
        /// </summary>
        public int Trainee { get; set; }
        /// <summary>
        /// 培训标题
        /// </summary 
        public string TrainingTitle { get; set; }
        /// <summary>
        /// 培训内容
        /// </summary>
        public string Trainingcontent { get; set; }
        /// <summary>
        /// 培训日期
        /// </summary>
        public Nullable<System.DateTime> TrainingDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> Dateofregistration { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public Nullable<System.DateTime> AddTime { get; set; }

        public int Grand { get; set; }
    }
}
