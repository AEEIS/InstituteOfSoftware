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
    [Table(name: "InstructorList")]
    // 教导大队人员表
    public partial class InstructorList
    {
        [Key]
        public int ID { get; set; }
        //员工编号
        public string EmployeeNumber { get; set; }
        //是否删除
        public Nullable<bool> IsDel { get; set; }
        //添加时间
        public Nullable<System.DateTime> AddTime { get; set; }
        //备注
        public string Remarks { get; set; }


    }
}
