﻿//------------------------------------------------------------------------------
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
    [Table(name: "DaysOff")]
   public partial class DaysOff
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public string LeaveReason { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsPassYear { get; set; }
        public Nullable<bool> IsPass { get; set; }
        public Nullable<bool> IsApproval { get; set; }

    }
}