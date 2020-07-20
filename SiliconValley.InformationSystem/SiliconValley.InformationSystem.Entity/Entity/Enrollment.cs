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
    /// 自考本科表
    /// </summary>
    [Table(name: "Enrollment")]
    public partial class Enrollment
    {
       
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 考藉号
        /// </summary>
        public string PassNumber { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        public Nullable<System.DateTime> Datestration { get; set; }
        /// <summary>
        /// 报考学校
        /// </summary>
        public int? School { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 伪删除
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }
        /// <summary>
        /// 报考专业
        /// </summary>
        public int? MajorID { get; set; }
        /// <summary>
        /// 注册批次如(1910)
        /// </summary>
        public string Registeredbatch { get; set; }
    
    }
}
